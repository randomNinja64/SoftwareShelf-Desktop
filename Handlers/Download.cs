using System;
using System.Diagnostics;
using System.Net;

namespace SoftwareShelf_Desktop
{
    public class Download
    {
        private Uri DownloadUrl;
        private string DownloadIdentifier;
        private string FileName;
        private double DownloadProgress;
        private Stopwatch DownloadTime = new Stopwatch();
        private string DownloadSpeed;

        // Web Client Associated With Download
        public WebClient WebClient;

        // Aria2 process started for this download. Cancel kills this process only.
        public Process aria2Process;

        // Full path passed to WebClient or Aria2. Cancel deletes this file.
        public string localPath;
        public bool cancelRequested;

        // Create properties
        public Uri downloadUrl
        {
            get { return DownloadUrl; }
            set { DownloadUrl = value; }
        }
        public double downloadProgress
        {
            get { return DownloadProgress; }
            set { DownloadProgress = value; }
        }
        public Stopwatch downloadTime
        {
            get { return DownloadTime; }
            set { DownloadTime = value; }
        }
        public string downloadIdentifier
        {
            get { return DownloadIdentifier; }
            set { DownloadIdentifier = value; }
        }
        public string fileName
        {
            get { return FileName; }
            set { FileName = value; }
        }

        public string downloadSpeed
        {
            get { return DownloadSpeed; }
            set { DownloadSpeed = value; }
        }

        public Download(Uri downloadUrl, string identifier, string fileName)
        {
            this.DownloadUrl = downloadUrl;
            this.DownloadProgress = 0;
            this.DownloadIdentifier = identifier;
            this.FileName = fileName;
            this.DownloadProgress = 0;
            this.DownloadTime = new Stopwatch();
        }        
    }
}
