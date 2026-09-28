using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SoftwareShelf_Desktop
{

    public partial class MainForm : Form
    {
        // Create a new Download Handler
        DownloadHandler downloadHandler = new DownloadHandler();
        
        static MainForm _frmObj;
        public static MainForm frmObj
        {
            get { return _frmObj; }
            set { _frmObj = value; }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Set the form object
            frmObj = this;

            // If a download path hasn't been set, prompt the user for one
            if (Properties.Settings.Default.DownloadPath == "")
            {
                if (setDownloadPath() == 1) 
                {
                    Application.Exit();
                }
                
            }

            // Aria2 does not run below Windows XP. A copied settings file must not turn it back on.
            if (!supportsAria2())
            {
                Properties.Settings.Default.AriaMode = false;
            }

            // Save default settings
            Properties.Settings.Default.Save();

            // Place download path in appropriate control
            dlDirTxtBox.Text = Properties.Settings.Default.DownloadPath;

            // Set Sort Column of resultsGrid to Name
            resultsGrid.Sort(resultsGrid.Columns[0], System.ComponentModel.ListSortDirection.Ascending);

            // Setup Downloads Grid View
            // Create Binding Source For Download Manager
            BindingSource downloadBindingSource = new BindingSource
            {
                // Set the Binding Source to the Download Handler
                DataSource = downloadHandler.Downloads
            };
            // Set the Data Source of the Download Manager to the Binding Source
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
                // Set boostChk based on AriaMode boolean
                boostChk.Checked = Properties.Settings.Default.AriaMode;

                // If aria2 is enabled, allow torrentChk to be checked/unchecked, and threads to be changed
                if (boostChk.Checked)
                {
                    torrentChk.Enabled = true;
                    threadsNum.Enabled = true;
                }

                // Set torrentChk based on TorrentProcessing boolean
                torrentChk.Checked = Properties.Settings.Default.TorrentProcessing;

                // Set DLThreads Number
                threadsNum.Value = Properties.Settings.Default.DLThreads;
            }

            // Set type drop down
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
                case "All":
                    return string.Empty;
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
            // Clear Search Results
            resultsGrid.Rows.Clear();

            // Clear Description Text
            resultDescription.Text = "";

            // Clear labels on right side
            creatorInfoLbl.Text = "Creator: ";
            publishedInfoLbl.Text = "Published: ";
            topicInfoLbl.Text = "Topic: ";

            // Set image box image back to default
            resultPreview.Image = SoftwareShelf_Desktop.Properties.Resources.placeholder;
        }

        private void showResults(List<ArchiveHandler.ArchiveItem> results)
        {
            clearShownItems();
            if (results == null)
            {
                return;
            }

            foreach (ArchiveHandler.ArchiveItem result in results)
            {
                resultsGrid.Rows.Add(result.title, result.avgRating.ToString(), result.size, result.identifier, result.description, result.downloads, result.creator, result.date, result.topic);
            }
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            string mediaType = getMediaType(typeDropDown.SelectedItem.ToString());
            showResults(ArchiveHandler.Search(searchTxtBox.Text, mediaType, creatorTxt.Text, topicTxt.Text, yearTxt.Text));
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

        private void resultsGrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            // Set the description textbox's text to the description cell of the currently selected row
            resultDescription.Text = resultsGrid.Rows[e.RowIndex].Cells[4].Value.ToString();

            if (e.RowIndex != -1)
            {
                // Get identifier of selected item
                string identifier = resultsGrid.Rows[e.RowIndex].Cells[3].Value.ToString();

                // Download thumbnail
                resultPreview.ImageLocation = "http://archive.org/download/" + identifier + "/__ia_thumb.jpg";

                // Set creator label
                creatorInfoLbl.Text = "Creator: " + resultsGrid.Rows[e.RowIndex].Cells[6].Value.ToString();

                // Set date label
                publishedInfoLbl.Text = "Published: " + resultsGrid.Rows[e.RowIndex].Cells[7].Value.ToString();

                // Set topic label
                topicInfoLbl.Text = "Topic: " + resultsGrid.Rows[e.RowIndex].Cells[8].Value.ToString();

                // Enable the download button
                downloadButton.Enabled = true;

                // Enable the reviews button
                reviewButton.Enabled = true;

                // Check the size of the item
                long sizeInKiB = Convert.ToInt64(resultsGrid.Rows[e.RowIndex].Cells[2].Value);
                double sizeInGB = sizeInKiB / (1024.0 * 1024.0); // Convert KiB to GB

                // Enable or disable the zip button based on the size
                if (sizeInGB < 40)
                {
                    zipBtn.Enabled = true;
                }
                else
                {
                    zipBtn.Enabled = false;
                }
            }
        }

        private void downloadButton_Click(object sender, EventArgs e)
        {
            //If an item is selected, Open DownloadForm and pass in the selected item's identifier
            if (resultsGrid.SelectedRows.Count > 0)
            {
                // Enumerate items in Downloads tab before dialog
                int numDownloads = downloadHandler.Downloads.Count;
                
                DownloadForm downloadForm = new DownloadForm(resultsGrid.SelectedRows[0].Cells[3].Value.ToString(), downloadHandler, progressTimer);
                downloadForm.ShowDialog();

                // Change selected tab to downloads tab if items were downloaded
                if (downloadHandler.Downloads.Count > numDownloads)
                {
                    controlTabs.SelectedTab = downloadTab;
                }
                
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

            //Update Text Box
            dlDirTxtBox.Text = Properties.Settings.Default.DownloadPath;

            // Save Properties
            Properties.Settings.Default.Save();
        }

        private int setDownloadPath()
        {
            // Create a new folder browser dialog
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
            {
                // Set the description
                Description = "Please select path for downloaded files."
            };

            // Show the dialog
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                // Set the download path
                Properties.Settings.Default.DownloadPath = folderBrowserDialog.SelectedPath;
                return 0;
            }
            else
            {
                return 1;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            downloadsDataGridView.Refresh();
        }

        private void cancelDlButton_Click(object sender, EventArgs e)
        {
            // If no row is selected, disable cancel button and return
            if (downloadsDataGridView.SelectedRows.Count == 0)
            {
                cancelDlButton.Enabled = false;
                return;
            }
            
            // If a download is running, abort it and delete the file
            if (downloadHandler.Downloads.Count > 0)
            {
                // Retrieve the Download object bound to the selected row.
                Download selectedDownload = (Download)downloadsDataGridView.SelectedRows[0].DataBoundItem;

                // Abort the selected download
                downloadHandler.Abort(selectedDownload);
            }
        }

        private void downloadsDataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            cancelDlButton.Enabled = true;
        }

        private void downloadsDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            //If a row is selected, leave the cancel button enabled
            if (downloadsDataGridView.SelectedRows.Count > 0)
            {
                cancelDlButton.Enabled = true;
            }
            else
            {
                cancelDlButton.Enabled = false;
            }
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

            // Drop the queue and kill the Aria2 process this download started.
            downloadHandler.Shutdown();
        }

        private void resultsGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Do nothing if header clicked.
            if (e.RowIndex == -1)
                return;

            // If an item is clicked, run downloadBtn_Click
            downloadButton_Click(this, new EventArgs());
        }

        private void openDownloadsBtn_Click(object sender, EventArgs e)
        {
            // Open explorer to text in Downloads directory Box
            try
            {
                Process.Start("explorer.exe", @dlDirTxtBox.Text);
            } catch {
                MessageBox.Show("Error 31: Opening directory failed. Directory may not exist or permissions may be incorrect.");
            }
        }

        private void boostChk_CheckedChanged(object sender, EventArgs e)
        {
            if (!supportsAria2())
            {
                Properties.Settings.Default.AriaMode = false;
                Properties.Settings.Default.Save();
                torrentChk.Enabled = false;
                threadsNum.Enabled = false;
                if (boostChk.Checked)
                {
                    boostChk.Checked = false;
                }
                return;
            }

            //If checked, enable aria2
            if (boostChk.Checked)
            {
                Properties.Settings.Default.AriaMode = true;
                Properties.Settings.Default.Save();
                torrentChk.Enabled = true;
                threadsNum.Enabled = true;
            }
            else
            {
                Properties.Settings.Default.AriaMode = false;
                Properties.Settings.Default.Save();
                torrentChk.Enabled = false;
                threadsNum.Enabled = false;
            }
        }

        private void boostChk_Click(object sender, EventArgs e)
        {
            //If progress timer is running, do nothing
            if (progressTimer.Enabled)
            {
                //Reset checkbox
                boostChk.Checked = !boostChk.Checked;
            }
        }

        private void zipBtn_Click(object sender, EventArgs e)
        {
            //If an item is selected, Open DownloadForm and pass in the selected item's identifier
            if (resultsGrid.SelectedRows.Count > 0)
            {
                // Enumerate items in Downloads tab before dialog
                int numDownloads = downloadHandler.Downloads.Count;

                // Get identifier
                string itemIdentifier = resultsGrid.SelectedRows[0].Cells[3].Value.ToString();

                // Create a filename
                string fileName = itemIdentifier + ".zip";

                // Add download to download handler
                Uri URL = new Uri("http://archive.org/compress/" + itemIdentifier);

                //MessageBox.Show(URL.ToString());

                downloadHandler.addDownload(URL, itemIdentifier, fileName, progressTimer);

                // Change selected tab to downloads tab if items were downloaded
                if (downloadHandler.Downloads.Count > numDownloads)
                {
                    controlTabs.SelectedTab = downloadTab;
                }

            }
        }

        private void resultsGrid_SelectionChanged(object sender, EventArgs e)
        {
            // Check if any rows are selected
            if (resultsGrid.SelectedRows.Count == 0)
            {
                // Disable the review button
                reviewButton.Enabled = false;
                
                // Disable the download button if no rows are selected
                downloadButton.Enabled = false;

                // Disable the ZIP button
                zipBtn.Enabled = false;
            }
        }

        private void threadsNum_ValueChanged(object sender, EventArgs e)
        {
            // Save new value to settings
            Properties.Settings.Default.DLThreads = (int)threadsNum.Value;
            Properties.Settings.Default.Save();
        }

        private void yearTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control keys like Backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void yearTxt_TextChanged(object sender, EventArgs e)
        {
            if (yearTxt.Text.Length > 4)
            {
                // If the input exceeds 4 digits, truncate the text
                yearTxt.Text = yearTxt.Text.Substring(0, 4);
                // Move the cursor to the end of the text
                yearTxt.SelectionStart = yearTxt.Text.Length;
            }

            // Ensure the content is numeric
            if (!System.Text.RegularExpressions.Regex.IsMatch(yearTxt.Text, "^[0-9]*$"))
            {
                yearTxt.Text = "";
            }
        }

        private void searchTab_Resize(object sender, EventArgs e)
        {
            searchTab.Invalidate();
        }

        private void downloadTab_Resize(object sender, EventArgs e)
        {
            downloadTab.Invalidate();
        }

        private void latestBtn_Click(object sender, EventArgs e)
        {
            string mediaType = getMediaType(typeDropDown.SelectedItem.ToString());
            showResults(ArchiveHandler.GetLatestItems(mediaType));
        }

        private void resultPreview_LoadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            // Check if there was an error during the image load
            if (e.Error != null)
            {
                resultPreview.Image = SoftwareShelf_Desktop.Properties.Resources.placeholder;
            }
        }

        private void torrentChk_CheckedChanged(object sender, EventArgs e)
        {
            //If checked, enable torrent processing
            if (torrentChk.Checked)
            {
                Properties.Settings.Default.TorrentProcessing = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.TorrentProcessing = false;
                Properties.Settings.Default.Save();
            }
        }

        private void reviewButton_Click(object sender, EventArgs e)
        {
            ReviewForm reviewForm = new ReviewForm(resultsGrid.SelectedRows[0].Cells[3].Value.ToString());
            reviewForm.ShowDialog();
        }
    }
    }
