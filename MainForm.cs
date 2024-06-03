using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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

            // Check if turboboost exists in config and add it, set to true if not
            #pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
            if (Properties.Settings.Default.TurboBooster == null)
            {
                Properties.Settings.Default.TurboBooster = true;
            }

            // If a download path hasn't been set, prompt the user for one
            if (Properties.Settings.Default.DownloadPath == "")
            {
                if (setDownloadPath() == 1) 
                {
                    Application.Exit();
                }
                
            }

            // If the number of threads hasn't been set in config, add it and set it to 4
            if (Properties.Settings.Default.DLThreads == null)
            {
                Properties.Settings.Default.DLThreads = 4;
            }
            #pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'


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

            // Set boostChk based on TurboBooster boolean
            boostChk.Checked = Properties.Settings.Default.TurboBooster;

            // Set DLThreads Number
            threadsNum.Value = Properties.Settings.Default.DLThreads;

        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            // Switch To Search Tab
            controlTabs.SelectedTab = searchTab;

            // Clear Search Results
            resultsGrid.Rows.Clear();

            // Perform Search With ArchiveHandler
            List<ArchiveHandler.ArchiveItem> results = ArchiveHandler.Search(searchTxtBox.Text);

            // If results is null, break
            if (results == null)
            {
                return;
            }

            // Add results to resultsGrid if results is not empty
            if (results.Count > 0)
            {
                foreach (ArchiveHandler.ArchiveItem result in results)
                {
                    resultsGrid.Rows.Add(result.title, result.size, result.identifier, result.description, result.downloads);
                }
            }
        }

        private void searchTxtBox_TextChanged(object sender, EventArgs e)
        {

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
            resultDescription.Text = resultsGrid.Rows[e.RowIndex].Cells[3].Value.ToString();

            if (e.RowIndex != -1)
            {
                // Get identifier of selected item
                string identifier = resultsGrid.Rows[e.RowIndex].Cells[2].Value.ToString();

                // Download thumbnail
                resultPreview.ImageLocation = "http://archive.org/download/" + identifier + "/__ia_thumb.jpg";

                // Enable the download button
                downloadButton.Enabled = true;

                // Check the size of the item
                long sizeInKiB = Convert.ToInt64(resultsGrid.Rows[e.RowIndex].Cells[1].Value);
                double sizeInGB = sizeInKiB / (1024.0 * 1024.0); // Convert KiB to GB

                // Enable or disable the zip button based on the size
                if (sizeInGB < 50)
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
                
                DownloadForm downloadForm = new DownloadForm(resultsGrid.SelectedRows[0].Cells[2].Value.ToString(), downloadHandler, progressTimer);
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

        private void dlDirTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void clrInactiveBtn_Click(object sender, EventArgs e)
        {
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
        private void searchTab_Click(object sender, EventArgs e)
        {

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
                // Abort the selected download
                downloadHandler.Abort(downloadHandler.Downloads[downloadsDataGridView.SelectedRows[0].Index]);
            }
        }

        private void downloadsDataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            cancelDlButton.Enabled = true;
        }

        private void downloadsDataGridView_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
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
                }
            }

            // Kill aria2c processes
            Process[] aria2cProcesses = Process.GetProcessesByName("aria2c");
            foreach (Process aria2cProcess in aria2cProcesses)
            {
                aria2cProcess.Kill();
            }
        }

        private void resultsGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Do nothing if header clicked.
            if (e.RowIndex == -1)
                return;
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

        private void downloadsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void boostChk_CheckedChanged(object sender, EventArgs e)
        {
            //If checked, enable the TURBO BOOSTER!!!!
            if (boostChk.Checked)
            {
                Properties.Settings.Default.TurboBooster = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.TurboBooster = false;
                Properties.Settings.Default.Save();
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
                string itemIdentifier = resultsGrid.SelectedRows[0].Cells[2].Value.ToString();

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
    }
    }
