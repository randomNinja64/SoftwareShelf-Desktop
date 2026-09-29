using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;

namespace SoftwareShelf_Desktop
{
    public class Download : INotifyPropertyChanged
    {
        // Raised on the UI thread. BindingList forwards it to the downloads grid.
        public event PropertyChangedEventHandler PropertyChanged;

        // Web Client Associated With Download
        public WebClient WebClient;

        // Aria2 process started for this download. Cancel kills this process only.
        public Process aria2Process;

        // Full path passed to WebClient or Aria2. Cancel deletes this file.
        public string localPath;
        public bool cancelRequested;

        private double progress;
        private string speed;

        public Uri downloadUrl { get; set; }
        public Stopwatch downloadTime { get; set; }
        public string downloadIdentifier { get; set; }
        public string fileName { get; set; }

        public double downloadProgress
        {
            get => progress;
            set => SetField(ref progress, value, nameof(downloadProgress));
        }

        public string downloadSpeed
        {
            get => speed;
            set => SetField(ref speed, value, nameof(downloadSpeed));
        }

        public Download(Uri downloadUrl, string identifier, string fileName)
        {
            this.downloadUrl = downloadUrl;
            this.downloadIdentifier = identifier;
            this.fileName = fileName;
            this.downloadTime = new Stopwatch();
        }

        private void SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return;
            }
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
