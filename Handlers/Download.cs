using System;
using System.Diagnostics;
using System.Net;

namespace SoftwareShelf_Desktop
{
    public class Download
    {
        // Web Client Associated With Download
        public WebClient WebClient;

        // Aria2 process started for this download. Cancel kills this process only.
        public Process aria2Process;

        // Full path passed to WebClient or Aria2. Cancel deletes this file.
        public string localPath;
        public bool cancelRequested;

        public Uri downloadUrl { get; set; }
        public double downloadProgress { get; set; }
        public Stopwatch downloadTime { get; set; }
        public string downloadIdentifier { get; set; }
        public string fileName { get; set; }
        public string downloadSpeed { get; set; }

        public Download(Uri downloadUrl, string identifier, string fileName)
        {
            this.downloadUrl = downloadUrl;
            this.downloadProgress = 0;
            this.downloadIdentifier = identifier;
            this.fileName = fileName;
            this.downloadTime = new Stopwatch();
        }
    }
}
