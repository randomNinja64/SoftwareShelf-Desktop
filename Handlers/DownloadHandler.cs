using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace SoftwareShelf_Desktop
{
    public class DownloadHandler
    {
        public BindingList<Download> Downloads;

        private readonly Control ui;
        private readonly Timer progressTimer;
        private Download activeDownload;

        public DownloadHandler(Control ui, Timer progressTimer)
        {
            this.ui = ui;
            this.progressTimer = progressTimer;
            this.Downloads = new BindingList<Download>();
        }

        // Function to Add Download
        public void addDownload(Uri downloadUrl, string identifier, string fileName)
        {
            string safeName = SanitizePathSegment(fileName);
            RunOnUi(delegate
            {
                Downloads.Add(new Download(downloadUrl, identifier, safeName));
                StartNext();
            });
        }

        // Function to abort download
        public void Abort(Download downloadToAbort)
        {
            if (downloadToAbort == null)
            {
                return;
            }

            RunOnUi(delegate { AbortOnUi(downloadToAbort); });
        }

        // Drop queued items and cancel the active transfer, Aria2 or WebClient.
        public void Shutdown()
        {
            RunOnUi(delegate
            {
                Download current = activeDownload;
                for (int i = Downloads.Count - 1; i >= 0; i--)
                {
                    if (Downloads[i] != current)
                    {
                        Downloads.RemoveAt(i);
                    }
                }

                if (current != null)
                {
                    CancelActive(current);
                }
            });
        }

        private void AbortOnUi(Download downloadToAbort)
        {
            if (downloadToAbort == activeDownload)
            {
                CancelActive(downloadToAbort);
            }
            else if (Downloads.Contains(downloadToAbort))
            {
                Downloads.Remove(downloadToAbort);
            }
        }

        // Cancel the transport that is actually running for this download.
        // Completion removes it and starts the next item.
        private void CancelActive(Download download)
        {
            download.cancelRequested = true;

            Process process = download.aria2Process;
            if (process != null)
            {
                try
                {
                    if (!process.HasExited)
                    {
                        process.Kill();
                        return;
                    }
                }
                catch (InvalidOperationException)
                {
                }
                catch (Win32Exception)
                {
                }
            }

            if (download.WebClient != null)
            {
                try
                {
                    download.WebClient.CancelAsync();
                    return;
                }
                catch (InvalidOperationException)
                {
                }
            }

            CompleteDownloadOnUi(download);
        }

        private void StartNext()
        {
            if (activeDownload != null)
            {
                return;
            }

            if (Downloads.Count == 0)
            {
                progressTimer.Stop();
                return;
            }

            activeDownload = Downloads[0];
            progressTimer.Start();

            try
            {
                downloadItem(activeDownload, Properties.Settings.Default.DownloadPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ShowDownloadFailed(ex);
                CompleteDownloadOnUi(activeDownload);
            }
        }

        // Function to download item
        private void downloadItem(Download downloadItem, string destination)
        {
            // Split the URL into segments
            Uri uri = downloadItem.downloadUrl;
            string[] segments = uri.AbsolutePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < segments.Length; i++)
            {
                segments[i] = Uri.UnescapeDataString(segments[i]);
            }

            // Find the index of the download identifier in the segments.
            int identifierIndex = Array.IndexOf(segments, downloadItem.downloadIdentifier);

            // If the download identifier is found, remove any segments before it.
            if (identifierIndex >= 0)
            {
                string[] tail = new string[segments.Length - identifierIndex];
                Array.Copy(segments, identifierIndex, tail, 0, tail.Length);
                segments = tail;
            }
            else
            {
                // If not found, default to using the provided downloadIdentifier.
                segments = new string[] { downloadItem.downloadIdentifier };
            }

            for (int i = 0; i < segments.Length; i++)
            {
                segments[i] = SanitizePathSegment(segments[i]);
            }

            // Build the destination folder path using all segments except the last one (assumed to be the file name).
            // A compress URL has only the identifier, so the zip lands in that folder.
            if (segments.Length > 1)
            {
                string folderStructure = segments[0];
                for (int i = 1; i < segments.Length - 1; i++)
                {
                    folderStructure = Path.Combine(folderStructure, segments[i]);
                }
                destination = Path.Combine(destination, folderStructure);
            }
            else
            {
                destination = Path.Combine(destination, segments[0]);
            }

            Directory.CreateDirectory(destination);

            downloadItem.localPath = Path.Combine(destination, downloadItem.fileName);

            // If Aria2 is disabled, use standard procedures
            if (!Properties.Settings.Default.AriaMode)
            {
                downloadItemSinglethreaded(downloadItem);
            }
            else
            {
                // Use Chunked/Multithreaded Downloading
                downloadItemMultithreaded(downloadItem, destination, Properties.Settings.Default.DLThreads);
            }
        }

        // Function to download item (non-multithreaded)
        private void downloadItemSinglethreaded(Download downloadItem)
        {
            downloadItem.WebClient = new WebClient();

            downloadItem.WebClient.DownloadProgressChanged += delegate (object sender, DownloadProgressChangedEventArgs e)
            {
                long received = e.BytesReceived;
                int percent = e.ProgressPercentage;
                PostOnUi(delegate { ApplyWebClientProgress(downloadItem, received, percent); });
            };
            downloadItem.WebClient.DownloadFileCompleted += delegate (object sender, AsyncCompletedEventArgs e)
            {
                bool cancelled = e.Cancelled;
                Exception error = e.Error;
                PostOnUi(delegate { OnWebClientCompleted(downloadItem, cancelled, error); });
            };

            downloadItem.WebClient.DownloadFileAsync(downloadItem.downloadUrl, downloadItem.localPath);
            downloadItem.downloadTime.Start();
        }

        // Function to download item using chunked/multithreaded downloading
        private void downloadItemMultithreaded(Download downloadItem, string destination, int numChunks)
        {
            string downloadUrl = downloadItem.downloadUrl.ToString();

            // If the user is trying to download a torrent file and process torrents is disabled, download the .torrent singlethreaded
            if (!Properties.Settings.Default.TorrentProcessing && downloadUrl.ToLower().EndsWith(".torrent"))
            {
                downloadItemSinglethreaded(downloadItem);
                return;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "aria2c.exe";
            startInfo.Arguments = "-x " + numChunks + " -d \"" + destination + "\" -o \"" + downloadItem.fileName + "\" --allow-overwrite=true --seed-time=0 --check-certificate=false \"" + downloadUrl + "\" ";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;

            Process process = new Process();
            process.StartInfo = startInfo;
            process.EnableRaisingEvents = true;
            process.OutputDataReceived += delegate (object sender, DataReceivedEventArgs e)
            {
                updateAria2Progress(downloadItem, e.Data);
            };
            process.Exited += delegate
            {
                PostOnUi(delegate
                {
                    if (downloadItem != activeDownload)
                    {
                        return;
                    }

                    // Kill() from cancel or close is a non-zero exit. That is not a failure.
                    int exitCode = process.ExitCode;
                    if (!downloadItem.cancelRequested && exitCode != 0)
                    {
                        MessageBox.Show("Error 22: File Download Failed. Aria2 exited with code " + exitCode + ".\n\nIf this continues, please try the ZIP option or turn Aria2 off.");
                    }
                    else if (!downloadItem.cancelRequested)
                    {
                        downloadItem.downloadProgress = 100;
                    }

                    CompleteDownloadOnUi(downloadItem);
                });
            };

            downloadItem.aria2Process = process;
            try
            {
                process.Start();
                process.BeginOutputReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("Error 22: File Download Failed. Aria2 could not start: " + ex + "\n\nIf this continues, please try the ZIP option or turn Aria2 off.");
                CompleteDownloadOnUi(downloadItem);
                return;
            }

            downloadItem.downloadTime.Start();
        }

        private void updateAria2Progress(Download download, string aria2output)
        {
            Console.WriteLine(aria2output);
            if (aria2output == null)
            {
                return;
            }

            bool allocating = aria2output.Contains("Allocating");
            bool sslFailure = aria2output.Contains("SSL/TLS handshake failure");
            bool hasProgress = false;
            double progressValue = 0;
            string speed = null;

            // Check if the output contains a % sign and does not contain "archive.org"
            if (aria2output.Contains("%") && !aria2output.Contains("archive.org") && aria2output.IndexOf("%") >= 3)
            {
                string progress = aria2output.Substring(aria2output.IndexOf("%") - 3, 3);
                string digits = string.Empty;
                foreach (char c in progress)
                {
                    if (char.IsDigit(c))
                    {
                        digits += c;
                    }
                }

                if (double.TryParse(digits, out progressValue))
                {
                    hasProgress = true;
                }

                if (aria2output.Contains("DL:") && aria2output.Contains("ETA:"))
                {
                    speed = aria2output.Substring(aria2output.IndexOf("DL:") + 3, aria2output.IndexOf("ETA:") - aria2output.IndexOf("DL:") - 3);
                    speed = speed.Trim();
                }
            }

            PostOnUi(delegate
            {
                if (download != activeDownload)
                {
                    return;
                }

                if (allocating)
                {
                    download.downloadSpeed = "Preallocating";
                }

                if (hasProgress)
                {
                    download.downloadProgress = progressValue;
                }

                if (speed != null)
                {
                    download.downloadSpeed = speed + "/s";
                }

                if (sslFailure && !download.cancelRequested)
                {
                    download.cancelRequested = true;
                    MessageBox.Show("Error 23: File Download Failed. It appears that Archive.org has redirected your download to an HTTPS link, which is not currently supported. Please try again later or try the ZIP option.");
                    CancelActive(download);
                }
            });
        }

        private void ApplyWebClientProgress(Download download, long bytesReceived, int progressPercentage)
        {
            if (download != activeDownload)
            {
                return;
            }

            download.downloadProgress = progressPercentage;
            double seconds = download.downloadTime.Elapsed.TotalSeconds;
            if (seconds > 0)
            {
                download.downloadSpeed = (bytesReceived / 1024d / seconds).ToString("0.00") + " KB/s";
            }
        }

        private void OnWebClientCompleted(Download currentDownload, bool cancelled, Exception error)
        {
            if (currentDownload != activeDownload)
            {
                return;
            }

            // If download fails and was not canceled, alert user
            if (error != null && !cancelled)
            {
                ShowDownloadFailed(error);
            }

            if (!cancelled)
            {
                currentDownload.downloadProgress = 100;
            }

            CompleteDownloadOnUi(currentDownload);
        }

        private void CompleteDownloadOnUi(Download download)
        {
            if (download != activeDownload)
            {
                return;
            }

            if (download.cancelRequested)
            {
                DeleteCanceledFile(download);
            }

            if (download.downloadTime.IsRunning)
            {
                download.downloadTime.Stop();
            }

            Console.WriteLine("Download completed in " + download.downloadTime.Elapsed.TotalSeconds + " seconds");

            // Clear the active slot before Remove. ListChanged can re-enter, and a
            // second finish must see that this download is no longer active.
            activeDownload = null;

            Downloads.Remove(download);

            ReleaseDownload(download);

            if (Application.OpenForms.Count == 0)
            {
                progressTimer.Stop();
                return;
            }

            StartNext();
        }

        private void DeleteCanceledFile(Download download)
        {
            if (string.IsNullOrEmpty(download.localPath) || !File.Exists(download.localPath))
            {
                return;
            }

            try
            {
                using (new FileStream(download.localPath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                }
                File.Delete(download.localPath);
            }
            catch (IOException)
            {
                MessageBox.Show("Error 21: File In Use");
            }
        }

        private void ReleaseDownload(Download download)
        {
            WebClient client = download.WebClient;
            download.WebClient = null;
            if (client != null)
            {
                try
                {
                    client.Dispose();
                }
                catch (Exception)
                {
                }
            }

            Process process = download.aria2Process;
            download.aria2Process = null;
            if (process != null)
            {
                try
                {
                    process.Dispose();
                }
                catch (Exception)
                {
                }
            }
        }

        private static string SanitizePathSegment(string segment)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                segment = segment.Replace(c, '-');
            }
            return segment;
        }

        private void ShowDownloadFailed(Exception error)
        {
            MessageBox.Show("Error 22: File Download Failed: " + error + "\n\nIf this continues, please try using the Aria2 or ZIP option.");
        }

        private void RunOnUi(MethodInvoker action)
        {
            MarshalToUi(action, true);
        }

        private void PostOnUi(MethodInvoker action)
        {
            MarshalToUi(action, false);
        }

        private void MarshalToUi(MethodInvoker action, bool wait)
        {
            if (ui == null || ui.IsDisposed || !ui.IsHandleCreated)
            {
                return;
            }

            if (!ui.InvokeRequired)
            {
                action();
                return;
            }

            try
            {
                if (wait)
                {
                    ui.Invoke(action);
                }
                else
                {
                    ui.BeginInvoke(action);
                }
            }
            catch (ObjectDisposedException)
            {
            }
            catch (InvalidOperationException)
            {
            }
        }
    }
}
