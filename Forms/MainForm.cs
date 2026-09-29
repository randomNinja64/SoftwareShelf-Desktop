using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace SoftwareShelf_Desktop
{
    public partial class MainForm : Form
    {
        readonly DownloadHandler downloadHandler;
        readonly BindingSource resultsBindingSource = new BindingSource();

        public MainForm()
        {
            InitializeComponent();
            downloadHandler = new DownloadHandler(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // If a download path hasn't been set, prompt the user for one
            if (Properties.Settings.Default.DownloadPath == "")
            {
                if (setDownloadPath() == 1)
                {
                    Application.Exit();
                    return;
                }
            }

            // Aria2 does not run below Windows XP. A copied settings file must not turn it back on.
            if (!supportsAria2())
            {
                Properties.Settings.Default.AriaMode = false;
            }

            Properties.Settings.Default.Save();

            dlDirTxtBox.Text = Properties.Settings.Default.DownloadPath;

            resultsGrid.DataSource = resultsBindingSource;

            BindingSource downloadBindingSource = new BindingSource
            {
                DataSource = downloadHandler.Downloads
            };
            downloadsDataGridView.DataSource = downloadBindingSource;

            if (!supportsAria2())
            {
                boostChk.Visible = false;
                torrentChk.Visible = false;
                threadsNum.Visible = false;
                threadLbl.Visible = false;
                dlDirTxtBox.Width = openDownloadsBtn.Left - dlDirTxtBox.Left - 4;
            }
            else
            {
                boostChk.Checked = Properties.Settings.Default.AriaMode;

                // If aria2 is enabled, allow torrentChk to be checked/unchecked, and threads to be changed
                if (boostChk.Checked)
                {
                    SetAriaChildrenEnabled(true);
                }

                torrentChk.Checked = Properties.Settings.Default.TorrentProcessing;

                threadsNum.Value = Properties.Settings.Default.DLThreads;
            }

            typeDropDown.SelectedIndex = 5;
        }

        private static bool supportsAria2()
        {
            return Environment.OSVersion.Version >= new Version(5, 1);
        }

        private string getMediaType(string mediaTypeIn)
        {
            switch (mediaTypeIn)
            {
                case "Audio":
                    return "audio";
                case "Books":
                    return "texts";
                case "Images":
                    return "image";
                case "Movies":
                    return "movies";
                case "Software":
                    return "software";
                default:
                    return string.Empty;
            }
        }

        private void clearShownItems()
        {
            resultDescription.Text = "";

            creatorInfoLbl.Text = "Creator: ";
            publishedInfoLbl.Text = "Published: ";
            topicInfoLbl.Text = "Topic: ";

            resultPreview.Image = Properties.Resources.placeholder;
        }

        // A new list per search, so a header sort from the last search is not carried over.
        private void showResults(List<ArchiveHandler.ArchiveItem> results, string category)
        {
            clearShownItems();
            if (results == null)
            {
                results = new List<ArchiveHandler.ArchiveItem>();
            }

            foreach (ArchiveHandler.ArchiveItem result in results)
            {
                result.category = category;
            }
            resultsBindingSource.DataSource = new SortableBindingList<ArchiveHandler.ArchiveItem>(results);
        }

        private ArchiveHandler.ArchiveItem selectedResult()
        {
            if (resultsGrid.SelectedRows.Count == 0)
            {
                return null;
            }
            return (ArchiveHandler.ArchiveItem)resultsGrid.SelectedRows[0].DataBoundItem;
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            string category = typeDropDown.SelectedItem.ToString();
            showResults(ArchiveHandler.Search(searchTxtBox.Text, getMediaType(category), creatorTxt.Text, topicTxt.Text, yearTxt.Text), category);
        }

        private void searchTxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Suppress alert noise
                e.SuppressKeyPress = true;
                searchBtn_Click(this, new EventArgs());
            }
        }

        private void resultsGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex != resultSize.Index || e.Value == null)
            {
                return;
            }

            long bytes;
            if (long.TryParse(e.Value.ToString(), out bytes))
            {
                e.Value = SizeFormatter.Format(bytes);
                e.FormattingApplied = true;
            }
        }

        private void resultsGrid_SelectionChanged(object sender, EventArgs e)
        {
            ArchiveHandler.ArchiveItem item = selectedResult();
            if (item == null)
            {
                reviewButton.Enabled = false;
                downloadButton.Enabled = false;
                zipBtn.Enabled = false;
                return;
            }

            resultDescription.Text = item.description;

            resultPreview.CancelAsync();
            resultPreview.ImageLocation = "http://archive.org/download/" + item.identifier + "/__ia_thumb.jpg";
            creatorInfoLbl.Text = "Creator: " + item.creator;
            publishedInfoLbl.Text = "Published: " + item.date;
            topicInfoLbl.Text = "Topic: " + item.topic;

            downloadButton.Enabled = true;
            reviewButton.Enabled = true;

            zipBtn.Enabled = item.size < 40L * 1024 * 1024 * 1024;
        }

        private void showDownloadsIfQueued(MethodInvoker queue)
        {
            int countBefore = downloadHandler.Downloads.Count;
            queue();
            if (downloadHandler.Downloads.Count > countBefore)
            {
                controlTabs.SelectedTab = downloadTab;
            }
        }

        private void downloadButton_Click(object sender, EventArgs e)
        {
            ArchiveHandler.ArchiveItem item = selectedResult();
            if (item != null)
            {
                showDownloadsIfQueued(delegate
                {
                    DownloadForm downloadForm = new DownloadForm(item.identifier, item.category, downloadHandler);
                    downloadForm.ShowDialog();
                });
            }
        }

        private void resultsGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                downloadButton_Click(this, new EventArgs());
            }
        }

        private void setDirBtn_Click(object sender, EventArgs e)
        {
            setDownloadPath();

            dlDirTxtBox.Text = Properties.Settings.Default.DownloadPath;

            Properties.Settings.Default.Save();
        }

        private int setDownloadPath()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
            {
                Description = "Please select path for downloaded files."
            };

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.DownloadPath = folderBrowserDialog.SelectedPath;
                return 0;
            }
            else
            {
                return 1;
            }
        }

        private void cancelDlButton_Click(object sender, EventArgs e)
        {
            if (downloadsDataGridView.SelectedRows.Count == 0)
            {
                cancelDlButton.Enabled = false;
                return;
            }

            if (downloadHandler.Downloads.Count > 0)
            {
                Download selectedDownload = (Download)downloadsDataGridView.SelectedRows[0].DataBoundItem;

                downloadHandler.Abort(selectedDownload);
            }
        }

        private void downloadsDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            cancelDlButton.Enabled = downloadsDataGridView.SelectedRows.Count > 0;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // If there are downloads running, ask user if they would like to close
            if (downloadHandler.Downloads.Count > 0)
            {
                DialogResult result = MessageBox.Show("Downloads are running. Are you sure you would like to exit?", "Downloads Running", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            // Drop the queue and cancel the active download.
            downloadHandler.Shutdown();
        }

        private void resultsGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Do nothing if header clicked.
            if (e.RowIndex == -1)
                return;

            downloadButton_Click(this, new EventArgs());
        }

        private void openDownloadsBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("explorer.exe", dlDirTxtBox.Text);
            }
            catch
            {
                MessageBox.Show("Error 31: Opening directory failed. Directory may not exist or permissions may be incorrect.");
            }
        }

        private void boostChk_CheckedChanged(object sender, EventArgs e)
        {
            if (!supportsAria2())
            {
                Properties.Settings.Default.AriaMode = false;
                Properties.Settings.Default.Save();
                SetAriaChildrenEnabled(false);
                if (boostChk.Checked)
                {
                    boostChk.Checked = false;
                }
                return;
            }

            Properties.Settings.Default.AriaMode = boostChk.Checked;
            Properties.Settings.Default.Save();
            SetAriaChildrenEnabled(boostChk.Checked);
        }

        private void SetAriaChildrenEnabled(bool enabled)
        {
            torrentChk.Enabled = enabled;
            threadsNum.Enabled = enabled;
        }

        private void boostChk_Click(object sender, EventArgs e)
        {
            //If downloads are running, do nothing
            if (downloadHandler.Downloads.Count > 0)
            {
                boostChk.Checked = !boostChk.Checked;
            }
        }

        private void zipBtn_Click(object sender, EventArgs e)
        {
            ArchiveHandler.ArchiveItem item = selectedResult();
            if (item != null)
            {
                string itemIdentifier = item.identifier;
                string fileName = itemIdentifier + ".zip";
                Uri URL = new Uri("http://archive.org/compress/" + Uri.EscapeDataString(itemIdentifier));
                showDownloadsIfQueued(delegate
                {
                    downloadHandler.addDownload(URL, itemIdentifier, fileName);
                });
            }
        }

        private void threadsNum_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.DLThreads = (int)threadsNum.Value;
            Properties.Settings.Default.Save();
        }

        private void yearTxt_TextChanged(object sender, EventArgs e)
        {
            string digits = string.Empty;
            foreach (char c in yearTxt.Text)
            {
                if (char.IsDigit(c))
                {
                    digits += c;
                }
            }

            if (digits.Length > 4)
            {
                digits = digits.Substring(0, 4);
            }

            if (yearTxt.Text == digits)
            {
                return;
            }

            yearTxt.Text = digits;
            yearTxt.SelectionStart = yearTxt.Text.Length;
        }

        private void tab_Resize(object sender, EventArgs e)
        {
            ((Control)sender).Invalidate();
        }

        private void latestBtn_Click(object sender, EventArgs e)
        {
            string category = typeDropDown.SelectedItem.ToString();
            showResults(ArchiveHandler.GetLatestItems(getMediaType(category)), category);
        }

        private void resultPreview_LoadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                resultPreview.Image = Properties.Resources.placeholder;
            }
        }

        private void torrentChk_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.TorrentProcessing = torrentChk.Checked;
            Properties.Settings.Default.Save();
        }

        private void reviewButton_Click(object sender, EventArgs e)
        {
            ArchiveHandler.ArchiveItem item = selectedResult();
            if (item == null)
            {
                return;
            }
            ReviewForm reviewForm = new ReviewForm(item.identifier);
            reviewForm.ShowDialog();
        }
    }
}
