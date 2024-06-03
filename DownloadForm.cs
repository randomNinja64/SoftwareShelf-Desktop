using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace SoftwareShelf_Desktop
{
    public partial class DownloadForm : Form
    {
        public string itemIdentifier;
        public DownloadHandler downloadHandler;
        public Timer progressTimer;
        public DownloadForm()
        {
            InitializeComponent();
        }
        public DownloadForm(string identifier, DownloadHandler downloadHandler, Timer progressTimer)
        {
            this.downloadHandler = downloadHandler;
            itemIdentifier = identifier;
            this.progressTimer = progressTimer;
            InitializeComponent();
        }

        private void DownloadForm_Load(object sender, EventArgs e)
        {
            // Get available files using ArchiveHandler
            List<string> files = ArchiveHandler.GetAvailableFiles(itemIdentifier);

            // Add files to listbox
            foreach (string file in files)
            {
                filesListBox.Items.Add(file);
            }

            // Blacklist specific file types: xml|sqlite|torrent
            for (int i = 0; i < filesListBox.Items.Count; i++)
            {
                string fileExtension = filesListBox.Items[i].ToString().ToLower();
                if (!fileExtension.EndsWith(".xml") && !fileExtension.EndsWith(".sqlite") && !fileExtension.EndsWith(".torrent") && !fileExtension.EndsWith(".webp") && !fileExtension.EndsWith(".jpg") && !fileExtension.EndsWith(".png") && !fileExtension.EndsWith(".bmp") && !fileExtension.EndsWith(".jpg"))
                {
                    filesListBox.SetItemChecked(i, true);
                }
            }
        }

        private void selectAllBtn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < filesListBox.Items.Count; i++)
            {
                filesListBox.SetItemChecked(i, true);
            }
        }

        private void selectNoneBtn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < filesListBox.Items.Count; i++)
            {
                filesListBox.SetItemChecked(i, false);
            }
        }

        private void downloadSelectedBtn_Click(object sender, EventArgs e)
        {
            // Iterate through selected items and add them to downloads
            foreach (string item in filesListBox.CheckedItems)
            {
                // Add download to download handler
                Uri URL = new Uri("http://archive.org/download/" + itemIdentifier + "/" + item);
                downloadHandler.addDownload(URL, itemIdentifier, item, progressTimer);
            }

            // Close Downloads Form
            this.Close();
        }
    }
}
