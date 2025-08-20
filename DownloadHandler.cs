using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace SoftwareShelf_Desktop
{
    public class DownloadHandler
    {
        public BindingList<Download> Downloads;

        // Constructor
        public DownloadHandler()
        {
            this.Downloads = new BindingList<Download>();
        }

        // Function to Add Download
        public void addDownload(Uri downloadUrl, string identifier, string fileName, System.Windows.Forms.Timer progressTimer)
        {
            // Correct filename, removing any invalid characters for Windows, replacing them with -
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '-');
            }

            // Msgbox to show filename
            //MessageBox.Show(downloadUrl.ToString());
            

            Downloads.Add(new Download(downloadUrl, identifier, fileName));

            // If progress timer isn't running, start it and start the first download
            if (!progressTimer.Enabled)
            {
                progressTimer.Start();
                downloadItem(Downloads[0], Properties.Settings.Default.DownloadPath);
            }
        }

        // Function to download item
        public void downloadItem(Download downloadItem, string destination)
        {
            // Calculate destination path
            if (!downloadItem.downloadUrl.ToString().StartsWith("http://archive.org/compress"))
            {
                /*   
                // Download 
                destination = Path.Combine(destination, downloadItem.downloadIdentifier);
                // Create destination directory
                Directory.CreateDirectory(destination);
                // Download file*/

                // Split the URL into segments
                Uri uri = downloadItem.downloadUrl;
                string[] segments = uri.AbsolutePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                // Find the index of the download identifier in the segments.
                int identifierIndex = Array.IndexOf(segments, downloadItem.downloadIdentifier);

                // If the download identifier is found, remove any segments before it.
                if (identifierIndex >= 0)
                {
                    segments = segments.Skip(identifierIndex).ToArray();
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
        public void downloadItemSinglethreaded(Download downloadItem, string destination)
        {
            downloadItem.WebClient = new WebClient();

            // Attach event handlers
            // Add Event Handlers For Progress and Completion
            downloadItem.WebClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);
            // Create async event handler for completion and pass current download into it 
            downloadItem.WebClient.DownloadFileCompleted += (sender, e) =>
            {
                webClient_DownloadFileCompleted(sender, e, downloadItem);
                downloadItem.WebClient.Dispose();
            };

            downloadItem.WebClient.DownloadFileAsync(downloadItem.downloadUrl, destination + "\\" + downloadItem.fileName);

            // Start Stopwatch
            downloadItem.downloadTime.Start();    
        }

        // Function to download item using chunked/multithreaded downloading
        public void downloadItemMultithreaded(Download downloadItem, string destination, int numChunks)
        {
            string aria2cPath = "aria2c.exe";
            string downloadUrl = downloadItem.downloadUrl.ToString();
            string fileName = downloadItem.fileName;
            string downloadPath = destination;

            // If the user is trying to download a torrent file and process torrents is disabled, download the .torrent singlethreaded
            if (!Properties.Settings.Default.TorrentProcessing && downloadUrl.ToLower().EndsWith(".torrent"))
            {
                downloadItemSinglethreaded(downloadItem, destination);
                return;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = aria2cPath;
            startInfo.Arguments = $"-x {numChunks} -d \"{downloadPath}\" -o \"{fileName}\" --allow-overwrite=true --seed-time=0 --check-certificate=false \"{downloadUrl}\" ";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true; // Add this line to redirect the console output

            Process process = new Process();
            process.EnableRaisingEvents = true;
            process.StartInfo = startInfo;

            // Event handler for when process exits
            //process.Exited += (sender, e) => OnDownloadCompleted(downloadItem);

            process.Start();

            // Read the console output and write it to Console.WriteLine
            process.OutputDataReceived += (sender, e) => updateAria2Progress(e.Data);
            process.BeginOutputReadLine();
            // Async event handler for when process exits
            process.Exited += (sender, e) =>
            {
                OnDownloadCompleted(downloadItem);
                process.Dispose();
            };
        }

        public void updateAria2Progress(string aria2output)
        {
            Console.WriteLine(aria2output);
            // If aria2output is not null and contains "Allocating", set Downloads[0].downloadSpeed to "Preallocating"
            if (aria2output != null && aria2output.Contains("Allocating"))
            {
                Downloads[0].downloadSpeed = "Preallocating";
            }

            // Check if the output is not null and contains a % sign and does not contain "archive.org"
            if (aria2output != null && aria2output.Contains("%") && !aria2output.Contains("archive.org"))
            {
                if (aria2output.IndexOf("%") >= 3)
                {
                    // The below code sets progress
                    string progress = aria2output.Substring(aria2output.IndexOf("%") - 3, 3);
                    progress = new string(progress.Where(c => char.IsDigit(c)).ToArray());
                    if (double.TryParse(progress, out double progressDouble))
                    {
                        try
                        {
                            Downloads[0].downloadProgress = progressDouble;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Download 0's info could not be set: {ex.Message}");
                            return;
                        }
                    }

                    // The below code sets speed
                    // Check if aria2output contains the string "DL:" and "ETA:"
                    if (aria2output.Contains("DL:") && aria2output.Contains("ETA:"))
                    {
                        // Get the string between "DL:" and "ETA:"
                        string speed = aria2output.Substring(aria2output.IndexOf("DL:") + 3, aria2output.IndexOf("ETA:") - aria2output.IndexOf("DL:") - 3);
                        // Remove trailing whitespace
                        speed = speed.Trim();
                        // Set download speed to the string between "DL:" and "ETA:" adding a "/s" to the end
                        Downloads[0].downloadSpeed = speed + "/s";
                    }
                }
            }

            // Error handling for when Archive.org redirects to HTTPS downloads
            if (aria2output != null && aria2output.Contains("SSL/TLS handshake failure"))
            {
                MessageBox.Show("Error 23: File Download Failed. It appears that Archive.org has redirected your download to an HTTPS link, which is not currently supported. Please try again later or try the ZIP option.");
            }
        }

        // Download Completed for chunked/multithreaded downloads
        private void OnDownloadCompleted(Download currentDownload)
        {
            // Messagebox to show download count
            //MessageBox.Show("Downloads remaining in queue: " + Downloads.Count);

            // Write line to show aria2c has completed
            Console.WriteLine("aria2c has completed");

            currentDownload.downloadTime.Stop();
            Console.WriteLine($"Download completed in {currentDownload.downloadTime.Elapsed.TotalSeconds} seconds");

            //Downloads.RemoveAt(0);
            if (Downloads.Count > 0)
            {
                // Messagebox, attempting to remove a download
                //MessageBox.Show("Attempting to remove a download");

                // Workaround for when form is closing
                // If no forms are open, return
                if (Application.OpenForms.Count == 0)
                {
                    return;
                }

                // Check if the current thread is the UI thread
                if (Application.OpenForms[0].InvokeRequired)
                {
                    // Use BeginInvoke to execute the downloadItem method on the UI thread
                    Application.OpenForms[0].BeginInvoke(new Action(() => Downloads.RemoveAt(0)));

                    // Wait for the above line to finish
                    Thread.Sleep(100);

                    // If there are still more downloads in the queue, start the next one
                    if (Downloads.Count > 0)
                    {
                        Application.OpenForms[0].BeginInvoke(new Action(() => downloadItem(Downloads[0], Properties.Settings.Default.DownloadPath)));
                    }

                    // If there are no more downloads in the queue, stop the timer
                    else
                    {
                        MainForm.frmObj.progressTimer.Stop();
                    }
                }
                else
                {
                    Downloads.RemoveAt(0); 
                    // If there are still more downloads in the queue, start the next one
                    if (Downloads.Count > 0)
                    {
                        downloadItem(Downloads[0], Properties.Settings.Default.DownloadPath);
                    }
                }
            }
            else
            {
                // Stop the timer
                MainForm.frmObj.progressTimer.Stop();
            }
        }

        // The event that will fire whenever the progress of the WebClient is changed
        private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Downloads[0].downloadProgress = e.ProgressPercentage;

            // Calculate download speed
            Downloads[0].downloadSpeed = (e.BytesReceived / 1024d / Downloads[0].downloadTime.Elapsed.TotalSeconds).ToString("0.00") + " KB/s";
        }

        // The event that will fire whenever the progress of the WebClient is completed
        private void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e, Download currentDownload)
        {
            // Check if download was canceled, delete the file if it isn't in use.
            if (e.Cancelled)
            {
                // Check if file is in use and delete if not
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
                    // File locked, show Message stating file could not be deleted.
                    MessageBox.Show("Error 21: File In Use");
                }
            }

            // If download fails and was not canceled, alert user
            if (e.Error != null && !e.Cancelled)
            {
                MessageBox.Show("Error 22: File Download Failed: " + e.Error.ToString() + "\n\nIf this continues, please try using the Aria2 or ZIP option.");
            }

            // Set Progress to 100%
            Downloads[0].downloadProgress = 100;
            Downloads.RemoveAt(0);

            // Check if there are more in the queue and start the next item
            if (Downloads.Count > 0)
            {
                // Check if the current thread is the UI thread
                if (Application.OpenForms[0].InvokeRequired)
                {
                    // Use BeginInvoke to execute the downloadItem method on the UI thread
                    Application.OpenForms[0].BeginInvoke(new Action(() => downloadItem(Downloads[0], Properties.Settings.Default.DownloadPath)));
                }
                else
                {
                    downloadItem(Downloads[0], Properties.Settings.Default.DownloadPath);
                }
            }
            else
            {
                // Stop the timer
                MainForm.frmObj.progressTimer.Stop();
            }
        }

        // Function to abort download
        public void Abort(Download downloadToAbort)
        {
            // If downloadToAbort is the first item in the queue, abort it and remove it from the queue
            if (Downloads[0] == downloadToAbort)
            {
                if (Properties.Settings.Default.AriaMode == true)
                {
                    // Kill running aria2c process
                    Process[] aria2cProcesses = Process.GetProcessesByName("aria2c");
                    foreach (Process process in aria2cProcesses)
                    {
                        process.Kill();
                    }
                }
                else
                {
                    downloadToAbort.WebClient.CancelAsync();
                }
            }
            else
            {
                // Remove download from queue
                Downloads.Remove(downloadToAbort);
            }
        }
    }
}
