using System;
using System.Collections.Generic;
using System.Windows.Forms;
namespace SoftwareShelf_Desktop
{
    public partial class DownloadForm : Form
    {
        public string itemIdentifier;
        public DownloadHandler downloadHandler;
        public Timer progressTimer;

        private List<ArchiveHandler.ArchiveFile> files;
        private Dictionary<string, bool> checkedFiles = new Dictionary<string, bool>();

        public DownloadForm(string identifier, DownloadHandler downloadHandler, Timer progressTimer)
        {
            this.downloadHandler = downloadHandler;
            itemIdentifier = identifier;
            this.progressTimer = progressTimer;
            InitializeComponent();
        }

        private void DownloadForm_Load(object sender, EventArgs e)
        {
            files = ArchiveHandler.ParseAvailableFiles(ArchiveHandler.GetItemMetadata(itemIdentifier));
            if (files == null)
            {
                MessageBox.Show("Error 41: No files found. Please check your Internet connection. Additionally, Archive.org may be down.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            ShowFiles(files);
        }

        private void selectAllBtn_Click(object sender, EventArgs e)
        {
            SetVisibleChecks(true);
        }

        private void selectNoneBtn_Click(object sender, EventArgs e)
        {
            SetVisibleChecks(false);
        }

        private void downloadSelectedBtn_Click(object sender, EventArgs e)
        {
            foreach (ArchiveHandler.ArchiveFile file in filesListBox.CheckedItems)
            {
                Uri URL = ArchiveDownloadUri(itemIdentifier, file.name);
                downloadHandler.addDownload(URL, itemIdentifier, file.name, progressTimer);
            }

            this.Close();
        }

        private void filterTxt_TextChanged(object sender, EventArgs e)
        {
            string filter = filterTxt.Text.ToLower();
            List<ArchiveHandler.ArchiveFile> filteredItems = new List<ArchiveHandler.ArchiveFile>();
            foreach (ArchiveHandler.ArchiveFile item in files)
            {
                if (item.name.ToLower().Contains(filter))
                {
                    filteredItems.Add(item);
                }
            }

            ShowFiles(filteredItems);
        }

        private void ShowFiles(List<ArchiveHandler.ArchiveFile> visible)
        {
            filesListBox.Items.Clear();
            foreach (ArchiveHandler.ArchiveFile file in visible)
            {
                int index = filesListBox.Items.Add(file);
                bool include;
                if (!checkedFiles.TryGetValue(file.name, out include))
                {
                    include = !IsUncheckedByDefault(file.name);
                }
                filesListBox.SetItemChecked(index, include);
            }
        }

        private void SetVisibleChecks(bool include)
        {
            for (int i = 0; i < filesListBox.Items.Count; i++)
            {
                filesListBox.SetItemChecked(i, include);
            }
        }

        private void filesListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            ArchiveHandler.ArchiveFile file = (ArchiveHandler.ArchiveFile)filesListBox.Items[e.Index];
            checkedFiles[file.name] = e.NewValue == CheckState.Checked;
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
