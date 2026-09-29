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

        private static readonly string[] UncheckedExtensions = new string[]
        {
            ".xml",
            ".sqlite",
            ".torrent",
            ".webp",
            ".jpg",
            ".png",
            ".bmp"
        };

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
                Uri URL = ArchiveHandler.FileDownloadUrl(itemIdentifier, file.name);
                downloadHandler.addDownload(URL, itemIdentifier, file.name, progressTimer);
            }

            this.Close();
        }

        private void filterTxt_TextChanged(object sender, EventArgs e)
        {
            List<ArchiveHandler.ArchiveFile> filteredItems = new List<ArchiveHandler.ArchiveFile>();
            foreach (ArchiveHandler.ArchiveFile item in files)
            {
                if (item.name.IndexOf(filterTxt.Text, StringComparison.OrdinalIgnoreCase) >= 0)
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

        private static bool IsUncheckedByDefault(string name)
        {
            foreach (string extension in UncheckedExtensions)
            {
                if (name.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
