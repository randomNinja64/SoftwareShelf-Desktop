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

        private Download activeDownload;
        private Timer progressTimer;

        // Constructor
        public DownloadHandler()
        {
            this.Downloads = new BindingList<Download>();
        }

        // Function to Add Download
        public void addDownload(Uri downloadUrl, string identifier, string fileName, Timer progressTimer)
        {
            // Correct filename, removing any invalid characters for Windows, replacing them with -
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '-');
            }

            string safeName = fileName;
            RunOnUi(delegate
            {
                this.progressTimer = progressTimer;
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

        // Drop queued items and kill the active Aria2 process, if this queue started one.
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

                Process process = current == null ? null : current.aria2Process;
                if (process == null)
                {
                    return;
                }

                try
                {
                    if (!process.HasExited)
                    {
                        process.Kill();
                    }
                }
                catch (InvalidOperationException)
                {
                }
                catch (Win32Exception)
                {
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
                StopTimer();
                return;
            }

            activeDownload = Downloads[0];
            if (progressTimer != null && !progressTimer.Enabled)
            {
                progressTimer.Start();
            }

            try
            {
                downloadItem(activeDownload, Properties.Settings.Default.DownloadPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("Error 22: File Download Failed: " + ex + "\n\nIf this continues, please try using the Aria2 or ZIP option.");
                CompleteDownloadOnUi(activeDownload);
            }
        }

        // Function to download item
        private void downloadItem(Download downloadItem, string destination)
        {
            // Calculate destination path
            if (!downloadItem.downloadUrl.ToString().StartsWith("http://archive.org/compress"))
            {
                // Split the URL into segments
                Uri uri = downloadItem.downloadUrl;
                string[] segments = uri.AbsolutePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

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

                // Build the destination folder path using all segments except the last one (assumed to be the file name).
                if (segments.Length > 1)
                {
                    // Start with the first segment.
                    string folderStructure = segments[0];
                    // Loop through the segments, excluding the last element.
                    for (int i = 1; i < segments.Length - 1; i++)
                    {
                        folderStructure = Path.Combine(folderStructure, segments[i]);
                    }
                    destination = Path.Combine(destination, folderStructure);
                }
                else
                {
                    // Only the download identifier is available.
                    destination = Path.Combine(destination, segments[0]);
                }

                // Create the destination directory.
                Directory.CreateDirectory(destination);
            }

            // If Aria2 is disabled, use standard procedures
            if (!Properties.Settings.Default.AriaMode)
            {
                downloadItemSinglethreaded(downloadItem, destination);
            }
            else
            {
                // Use Chunked/Multithreaded Downloading
                downloadItemMultithreaded(downloadItem, destination, Properties.Settings.Default.DLThreads);
            }
        }

        // Function to download item (non-multithreaded)
        private void downloadItemSinglethreaded(Download downloadItem, string destination)
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

            downloadItem.WebClient.DownloadFileAsync(downloadItem.downloadUrl, destination + "\\" + downloadItem.fileName);
            downloadItem.downloadTime.Start();
        }

        // Function to download item using chunked/multithreaded downloading
        private void downloadItemMultithreaded(Download downloadItem, string destination, int numChunks)
        {
            string downloadUrl = downloadItem.downloadUrl.ToString();

            // If the user is trying to download a torrent file and process torrents is disabled, download the .torrent singlethreaded
            if (!Properties.Settings.Default.TorrentProcessing && downloadUrl.ToLower().EndsWith(".torrent"))
            {
                downloadItemSinglethreaded(downloadItem, destination);
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
                PostOnUi(delegate { CompleteDownloadOnUi(downloadItem); });
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
                MessageBox.Show("Error 22: File Download Failed: " + ex + "\n\nIf this continues, please try using the Aria2 or ZIP option.");
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

                if (sslFailure)
                {
                    MessageBox.Show("Error 23: File Download Failed. It appears that Archive.org has redirected your download to an HTTPS link, which is not currently supported. Please try again later or try the ZIP option.");
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

            // Check if download was canceled, delete the file if it isn't in use.
            if (cancelled)
            {
                string fileToDelete = Properties.Settings.Default.DownloadPath + "\\" + currentDownload.downloadIdentifier + "\\" + currentDownload.fileName;
                FileInfo file = new FileInfo(fileToDelete);

                try
                {
                    using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        stream.Close();
                    }
                    File.Delete(fileToDelete);
                }
                catch (IOException)
                {
                    MessageBox.Show("Error 21: File In Use");
                }
            }

            // If download fails and was not canceled, alert user
            if (error != null && !cancelled)
            {
                MessageBox.Show("Error 22: File Download Failed: " + error + "\n\nIf this continues, please try using the Aria2 or ZIP option.");
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
                StopTimer();
                return;
            }

            if (Downloads.Count > 0)
            {
                StartNext();
            }
            else
            {
                StopTimer();
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

        private void StopTimer()
        {
            if (progressTimer != null && progressTimer.Enabled)
            {
                progressTimer.Stop();
            }
        }

        private void RunOnUi(MethodInvoker action)
        {
            Form form = MainForm.frmObj;
            if (form == null || form.IsDisposed || !form.IsHandleCreated)
            {
                return;
            }

            if (form.InvokeRequired)
            {
                try
                {
                    form.Invoke(action);
                }
                catch (ObjectDisposedException)
                {
                }
                catch (InvalidOperationException)
                {
                }
            }
            else
            {
                action();
            }
        }

        private void PostOnUi(MethodInvoker action)
        {
            Form form = MainForm.frmObj;
            if (form == null || form.IsDisposed || !form.IsHandleCreated)
            {
                return;
            }

            if (form.InvokeRequired)
            {
                try
                {
                    form.BeginInvoke(action);
                }
                catch (ObjectDisposedException)
                {
                }
                catch (InvalidOperationException)
                {
                }
            }
            else
            {
                action();
            }
        }
    }
}
