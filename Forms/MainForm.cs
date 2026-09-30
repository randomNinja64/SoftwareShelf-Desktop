using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace SoftwareShelf_Desktop
{
    public partial class MainForm : Form
    {
        readonly DownloadHandler downloadHandler;
        readonly ItemPane searchPane;
        readonly ItemPane browsePane;

        private bool fillingCollections;
        private bool showingSaved;
        private readonly ToolTip collectionTip;
        private int collectionTipIndex = -1;

        public MainForm()
        {
            InitializeComponent();
            downloadHandler = new DownloadHandler(this);
            searchPane = new ItemPane(this, resultsGrid, resultPreview, resultDescription, creatorInfoLbl, publishedInfoLbl, topicInfoLbl, reviewButton, downloadButton, zipBtn);
            browsePane = searchPane.CloneInto(browseTab);
            collectionTip = new ToolTip(components);
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
            sortDropDown.SelectedIndex = 1;
            browseTypeDropDown.SelectedIndex = 4;
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

        private void searchBtn_Click(object sender, EventArgs e)
        {
            string category = typeDropDown.SelectedItem.ToString();
            searchPane.ShowResults(ArchiveHandler.Search(searchTxtBox.Text, getMediaType(category), creatorTxt.Text, topicTxt.Text, yearTxt.Text), category);
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

        private void showDownloadsIfQueued(MethodInvoker queue)
        {
            int countBefore = downloadHandler.Downloads.Count;
            queue();
            if (downloadHandler.Downloads.Count > countBefore)
            {
                controlTabs.SelectedTab = downloadTab;
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
            searchPane.ShowResults(ArchiveHandler.GetLatestItems(getMediaType(category)), category);
        }

        private void torrentChk_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.TorrentProcessing = torrentChk.Checked;
            Properties.Settings.Default.Save();
        }

        private void browseFilter_Changed(object sender, EventArgs e)
        {
            if (controlTabs.SelectedTab == browseTab)
            {
                loadCollections();
            }
        }

        private void collectionSearchBtn_Click(object sender, EventArgs e)
        {
            loadCollections();
        }

        private void collectionSearchTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                loadCollections();
            }
        }

        private void controlTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (controlTabs.SelectedTab == browseTab && !showingSaved && collectionsList.Items.Count == 0)
            {
                loadCollections();
            }
        }

        private void loadCollections()
        {
            if (browseTypeDropDown.SelectedItem == null)
            {
                return;
            }

            string category = browseTypeDropDown.SelectedItem.ToString();
            List<ArchiveHandler.ArchiveCollection> collections = ArchiveHandler.GetCollections(getMediaType(category), category, sortDropDown.SelectedIndex == 1, collectionSearchTxt.Text);
            if (collections == null)
            {
                return;
            }

            showingSaved = false;
            showCollections(collections);
        }

        private void savedBtn_Click(object sender, EventArgs e)
        {
            showingSaved = true;
            showCollections(readSavedCollections());
        }

        // Keeps the selected collection, and reloads its items, when it is still listed.
        private void showCollections(List<ArchiveHandler.ArchiveCollection> collections)
        {
            ArchiveHandler.ArchiveCollection previous = collectionsList.SelectedItem as ArchiveHandler.ArchiveCollection;
            int reselect = -1;

            fillingCollections = true;
            collectionsList.BeginUpdate();
            collectionsList.Items.Clear();
            foreach (ArchiveHandler.ArchiveCollection collection in collections)
            {
                int index = collectionsList.Items.Add(collection);
                if (reselect < 0 && previous != null && collection.identifier == previous.identifier)
                {
                    reselect = index;
                }
            }
            collectionsList.EndUpdate();
            if (reselect < 0 && collectionsList.Items.Count > 0)
            {
                reselect = 0;
            }
            collectionsList.SelectedIndex = reselect;
            fillingCollections = false;

            updateCollectionButton();
            if (reselect >= 0)
            {
                showCollectionItems();
            }
            else
            {
                browsePane.ShowResults(new List<ArchiveHandler.ArchiveItem>(), "");
            }
        }

        private void collectionsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fillingCollections)
            {
                return;
            }

            updateCollectionButton();
            showCollectionItems();
        }

        private void collectionsList_MouseMove(object sender, MouseEventArgs e)
        {
            int index = collectionsList.IndexFromPoint(e.Location);
            if (index == collectionTipIndex)
            {
                return;
            }

            collectionTipIndex = index;
            string title = "";
            if (index >= 0)
            {
                title = collectionsList.Items[index].ToString();
                if (TextRenderer.MeasureText(title, collectionsList.Font).Width <= collectionsList.ClientSize.Width)
                {
                    title = "";
                }
            }

            collectionTip.Active = false;
            collectionTip.SetToolTip(collectionsList, title);
            collectionTip.Active = title.Length > 0;
        }

        private void collectionsList_MouseLeave(object sender, EventArgs e)
        {
            collectionTipIndex = -1;
            collectionTip.Active = false;
            collectionTip.SetToolTip(collectionsList, "");
        }

        private void showCollectionItems()
        {
            ArchiveHandler.ArchiveCollection collection = collectionsList.SelectedItem as ArchiveHandler.ArchiveCollection;
            if (collection == null)
            {
                return;
            }

            browsePane.ShowResults(ArchiveHandler.GetCollectionItems(collection.identifier, getMediaType(collection.category)), collection.category);
        }

        private void collectionBtn_Click(object sender, EventArgs e)
        {
            ArchiveHandler.ArchiveCollection collection = collectionsList.SelectedItem as ArchiveHandler.ArchiveCollection;
            if (collection != null && readSavedCollections().FindIndex(item => item.identifier == collection.identifier) >= 0)
            {
                removeSelectedCollection();
            }
            else
            {
                saveSelectedCollection();
            }
        }

        private void saveSelectedCollection()
        {
            ArchiveHandler.ArchiveCollection collection = collectionsList.SelectedItem as ArchiveHandler.ArchiveCollection;
            List<ArchiveHandler.ArchiveCollection> saved = readSavedCollections();
            if (collection == null || saved.FindIndex(item => item.identifier == collection.identifier) >= 0)
            {
                return;
            }

            saved.Add(collection);
            writeSavedCollections(saved);
            updateCollectionButton();
        }

        private void removeSelectedCollection()
        {
            ArchiveHandler.ArchiveCollection collection = collectionsList.SelectedItem as ArchiveHandler.ArchiveCollection;
            if (collection == null)
            {
                return;
            }

            List<ArchiveHandler.ArchiveCollection> saved = readSavedCollections();
            int index = saved.FindIndex(item => item.identifier == collection.identifier);
            if (index < 0)
            {
                return;
            }

            saved.RemoveAt(index);
            writeSavedCollections(saved);

            if (showingSaved)
            {
                fillingCollections = true;
                collectionsList.Items.Remove(collection);
                fillingCollections = false;
            }
            updateCollectionButton();
        }

        private void updateCollectionButton()
        {
            ArchiveHandler.ArchiveCollection collection = collectionsList.SelectedItem as ArchiveHandler.ArchiveCollection;
            bool saved = collection != null && readSavedCollections().FindIndex(item => item.identifier == collection.identifier) >= 0;
            collectionBtn.Text = saved ? "&Remove Collection" : "&Save Collection";
            collectionBtn.Enabled = collection != null;
        }

        // One collection per line: identifier, title, and Type label, separated by tabs.
        private static List<ArchiveHandler.ArchiveCollection> readSavedCollections()
        {
            List<ArchiveHandler.ArchiveCollection> saved = new List<ArchiveHandler.ArchiveCollection>();
            foreach (string line in (Properties.Settings.Default.SavedCollections ?? "").Split('\n'))
            {
                string[] fields = line.TrimEnd('\r').Split('\t');
                if (fields.Length != 3 || fields[0].Length == 0 || fields[2].Length == 0)
                {
                    continue;
                }

                ArchiveHandler.ArchiveCollection collection = new ArchiveHandler.ArchiveCollection();
                collection.identifier = fields[0];
                collection.title = fields[1];
                collection.category = fields[2];
                saved.Add(collection);
            }
            return saved;
        }

        private static void writeSavedCollections(List<ArchiveHandler.ArchiveCollection> saved)
        {
            StringBuilder lines = new StringBuilder();
            foreach (ArchiveHandler.ArchiveCollection collection in saved)
            {
                string title = collection.title.Replace('\t', ' ').Replace('\r', ' ').Replace('\n', ' ');
                lines.Append(collection.identifier).Append('\t').Append(title).Append('\t').Append(collection.category).Append('\n');
            }
            Properties.Settings.Default.SavedCollections = lines.ToString();
            Properties.Settings.Default.Save();
        }

        // Results grid and preview. Search and Browse each pass their own controls.
        private sealed class ItemPane
        {
            private readonly MainForm owner;
            private readonly DataGridView grid;
            private readonly PictureBox preview;
            private readonly TextBox description;
            private readonly Label creatorLbl;
            private readonly Label publishedLbl;
            private readonly Label topicLbl;
            private readonly Button reviewBtn;
            private readonly Button downloadBtn;
            private readonly Button zipBtn;
            private readonly BindingSource results = new BindingSource();

            public ItemPane(MainForm owner, DataGridView grid, PictureBox preview, TextBox description, Label creatorLbl, Label publishedLbl, Label topicLbl, Button reviewBtn, Button downloadBtn, Button zipBtn)
            {
                this.owner = owner;
                this.grid = grid;
                this.preview = preview;
                this.description = description;
                this.creatorLbl = creatorLbl;
                this.publishedLbl = publishedLbl;
                this.topicLbl = topicLbl;
                this.reviewBtn = reviewBtn;
                this.downloadBtn = downloadBtn;
                this.zipBtn = zipBtn;

                grid.DataSource = results;
                grid.CellFormatting += grid_CellFormatting;
                grid.SelectionChanged += grid_SelectionChanged;
                grid.CellDoubleClick += grid_CellDoubleClick;
                grid.KeyDown += grid_KeyDown;
                preview.LoadCompleted += preview_LoadCompleted;
                reviewBtn.Click += reviewBtn_Click;
                downloadBtn.Click += downloadBtn_Click;
                zipBtn.Click += zipBtn_Click;
            }

            // Copies this pane's controls onto another tab. Call before the form is shown,
            // while both tabs still have their designer size, so anchors line up.
            public ItemPane CloneInto(Control parent)
            {
                DataGridView gridCopy = new DataGridView();
                ((ISupportInitialize)gridCopy).BeginInit();
                gridCopy.AllowUserToAddRows = grid.AllowUserToAddRows;
                gridCopy.AllowUserToDeleteRows = grid.AllowUserToDeleteRows;
                gridCopy.AllowUserToOrderColumns = grid.AllowUserToOrderColumns;
                gridCopy.AllowUserToResizeColumns = grid.AllowUserToResizeColumns;
                gridCopy.AllowUserToResizeRows = grid.AllowUserToResizeRows;
                gridCopy.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle(grid.AlternatingRowsDefaultCellStyle);
                gridCopy.AutoGenerateColumns = false;
                gridCopy.BackgroundColor = grid.BackgroundColor;
                gridCopy.ColumnHeadersHeightSizeMode = grid.ColumnHeadersHeightSizeMode;
                gridCopy.MultiSelect = grid.MultiSelect;
                gridCopy.ReadOnly = grid.ReadOnly;
                gridCopy.RowHeadersVisible = grid.RowHeadersVisible;
                gridCopy.RowHeadersWidthSizeMode = grid.RowHeadersWidthSizeMode;
                gridCopy.SelectionMode = grid.SelectionMode;
                foreach (DataGridViewColumn column in grid.Columns)
                {
                    gridCopy.Columns.Add((DataGridViewColumn)column.Clone());
                }
                CopyLayout(grid, gridCopy, parent);
                ((ISupportInitialize)gridCopy).EndInit();

                PictureBox previewCopy = new PictureBox();
                ((ISupportInitialize)previewCopy).BeginInit();
                previewCopy.BorderStyle = preview.BorderStyle;
                previewCopy.SizeMode = preview.SizeMode;
                previewCopy.Image = Properties.Resources.placeholder;
                previewCopy.TabStop = false;
                CopyLayout(preview, previewCopy, parent);
                ((ISupportInitialize)previewCopy).EndInit();

                TextBox descriptionCopy = new TextBox();
                descriptionCopy.Multiline = description.Multiline;
                descriptionCopy.ScrollBars = description.ScrollBars;
                CopyLayout(description, descriptionCopy, parent);

                return new ItemPane(owner, gridCopy, previewCopy, descriptionCopy,
                    CopyLayout(creatorLbl, new Label(), parent),
                    CopyLayout(publishedLbl, new Label(), parent),
                    CopyLayout(topicLbl, new Label(), parent),
                    CopyButton(reviewBtn, parent),
                    CopyButton(downloadBtn, parent),
                    CopyButton(zipBtn, parent));
            }

            private static Button CopyButton(Button source, Control parent)
            {
                Button copy = new Button();
                copy.UseVisualStyleBackColor = source.UseVisualStyleBackColor;
                return CopyLayout(source, copy, parent);
            }

            private static T CopyLayout<T>(T source, T copy, Control parent) where T : Control
            {
                copy.AutoSize = source.AutoSize;
                copy.Anchor = source.Anchor;
                copy.Margin = source.Margin;
                copy.Bounds = source.Bounds;
                copy.Text = source.Text;
                copy.Enabled = source.Enabled;
                copy.TabIndex = source.TabIndex;
                parent.Controls.Add(copy);
                return copy;
            }

            // A new list per load, so a header sort from the last results is not carried over.
            public void ShowResults(List<ArchiveHandler.ArchiveItem> items, string category)
            {
                ClearPreview();
                if (items == null)
                {
                    items = new List<ArchiveHandler.ArchiveItem>();
                }

                foreach (ArchiveHandler.ArchiveItem item in items)
                {
                    item.category = category;
                }
                results.DataSource = new SortableBindingList<ArchiveHandler.ArchiveItem>(items);
            }

            private void ClearPreview()
            {
                description.Text = "";

                creatorLbl.Text = "Creator: ";
                publishedLbl.Text = "Published: ";
                topicLbl.Text = "Topic: ";

                preview.Image = Properties.Resources.placeholder;
            }

            private ArchiveHandler.ArchiveItem SelectedItem()
            {
                if (grid.SelectedRows.Count == 0)
                {
                    return null;
                }
                return (ArchiveHandler.ArchiveItem)grid.SelectedRows[0].DataBoundItem;
            }

            private void grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
            {
                if (grid.Columns[e.ColumnIndex].DataPropertyName != "size" || e.Value == null)
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

            private void grid_SelectionChanged(object sender, EventArgs e)
            {
                ArchiveHandler.ArchiveItem item = SelectedItem();
                if (item == null)
                {
                    reviewBtn.Enabled = false;
                    downloadBtn.Enabled = false;
                    zipBtn.Enabled = false;
                    return;
                }

                description.Text = item.description;

                preview.CancelAsync();
                preview.ImageLocation = "http://archive.org/download/" + item.identifier + "/__ia_thumb.jpg";
                creatorLbl.Text = "Creator: " + item.creator;
                publishedLbl.Text = "Published: " + item.date;
                topicLbl.Text = "Topic: " + item.topic;

                downloadBtn.Enabled = true;
                reviewBtn.Enabled = true;

                zipBtn.Enabled = item.size < 40L * 1024 * 1024 * 1024;
            }

            private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
            {
                // Do nothing if header clicked.
                if (e.RowIndex == -1)
                    return;

                downloadBtn_Click(this, new EventArgs());
            }

            private void grid_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    downloadBtn_Click(this, new EventArgs());
                }
            }

            private void preview_LoadCompleted(object sender, AsyncCompletedEventArgs e)
            {
                if (e.Error != null)
                {
                    preview.Image = Properties.Resources.placeholder;
                }
            }

            private void downloadBtn_Click(object sender, EventArgs e)
            {
                ArchiveHandler.ArchiveItem item = SelectedItem();
                if (item != null)
                {
                    owner.showDownloadsIfQueued(delegate
                    {
                        DownloadForm downloadForm = new DownloadForm(item.identifier, item.category, owner.downloadHandler);
                        downloadForm.ShowDialog();
                    });
                }
            }

            private void reviewBtn_Click(object sender, EventArgs e)
            {
                ArchiveHandler.ArchiveItem item = SelectedItem();
                if (item == null)
                {
                    return;
                }
                ReviewForm reviewForm = new ReviewForm(item.identifier);
                reviewForm.ShowDialog();
            }

            private void zipBtn_Click(object sender, EventArgs e)
            {
                ArchiveHandler.ArchiveItem item = SelectedItem();
                if (item != null)
                {
                    string itemIdentifier = item.identifier;
                    string fileName = itemIdentifier + ".zip";
                    Uri URL = new Uri("http://archive.org/compress/" + Uri.EscapeDataString(itemIdentifier));
                    owner.showDownloadsIfQueued(delegate
                    {
                        owner.downloadHandler.addDownload(URL, itemIdentifier, fileName);
                    });
                }
            }
        }
    }
}
