using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
namespace SoftwareShelf_Desktop
{
    public partial class DownloadForm : Form
    {
        public string itemIdentifier;
        public DownloadHandler downloadHandler;
        public Timer progressTimer;

        // Store files in private variable
        private List<string> files;

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
            files = ArchiveHandler.ParseAvailableFiles(ArchiveHandler.GetItemMetadata(itemIdentifier));
            if (files == null)
            {
                MessageBox.Show("Error 41: No files found. Please check your Internet connection. Additionally, Archive.org may be down.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            foreach (string file in files)
            {
                filesListBox.Items.Add(file);
            }

            // Blacklist specific file types: xml|sqlite|torrent
            for (int i = 0; i < filesListBox.Items.Count; i++)
            {
                filesListBox.SetItemChecked(i, !IsUncheckedByDefault(filesListBox.Items[i].ToString()));
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
                Uri URL = ArchiveDownloadUri(itemIdentifier, item);
                downloadHandler.addDownload(URL, itemIdentifier, item, progressTimer);
            }

            // Close Downloads Form
            this.Close();
        }

        private void filterTxt_TextChanged(object sender, EventArgs e)
        {
            // Get the filter text
            string filter = filterTxt.Text.ToLower();

            // Filter the items
            List<string> filteredItems = new List<string>();
            foreach (string item in files)
            {
                if (item.ToLower().Contains(filter))
                {
                    filteredItems.Add(item);
                }
            }

            // Update filesListBox
            filesListBox.Items.Clear();
            filesListBox.Items.AddRange(filteredItems.ToArray());
        }

        private static Uri ArchiveDownloadUri(string identifier, string relativeFile)
        {
            System.Text.StringBuilder path = new System.Text.StringBuilder("http://archive.org/download/");
            path.Append(Uri.EscapeDataString(identifier));
            string[] parts = relativeFile.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string part in parts)
            {
                path.Append('/');
                path.Append(Uri.EscapeDataString(part));
            }
            return new Uri(path.ToString());
        }

        private static bool IsUncheckedByDefault(string name)
        {
            string lower = name.ToLower();
            return lower.EndsWith(".xml")
                || lower.EndsWith(".sqlite")
                || lower.EndsWith(".torrent")
                || lower.EndsWith(".webp")
                || lower.EndsWith(".jpg")
                || lower.EndsWith(".png")
                || lower.EndsWith(".bmp");
        }
    }
}
