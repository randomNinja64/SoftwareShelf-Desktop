namespace SoftwareShelf_Desktop
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.searchTxtBox = new System.Windows.Forms.TextBox();
            this.searchBtn = new System.Windows.Forms.Button();
            this.controlTabs = new System.Windows.Forms.TabControl();
            this.searchTab = new System.Windows.Forms.TabPage();
            this.zipBtn = new System.Windows.Forms.Button();
            this.downloadButton = new System.Windows.Forms.Button();
            this.resultsGrid = new System.Windows.Forms.DataGridView();
            this.resultName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resultSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.identifier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Downloads = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resultDescription = new System.Windows.Forms.TextBox();
            this.resultPreview = new System.Windows.Forms.PictureBox();
            this.downloadTab = new System.Windows.Forms.TabPage();
            this.threadsNum = new System.Windows.Forms.NumericUpDown();
            this.threadLbl = new System.Windows.Forms.Label();
            this.boostChk = new System.Windows.Forms.CheckBox();
            this.openDownloadsBtn = new System.Windows.Forms.Button();
            this.cancelDlButton = new System.Windows.Forms.Button();
            this.downloadsDataGridView = new System.Windows.Forms.DataGridView();
            this.DownloadUrl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Speed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.downloadIdentifier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Running = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DownloadProgress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DownloadedBytes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.downloadTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.multiThreadBytesDownloaded = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dlDirLabel = new System.Windows.Forms.Label();
            this.dlDirTxtBox = new System.Windows.Forms.TextBox();
            this.setDirBtn = new System.Windows.Forms.Button();
            this.progressTimer = new System.Windows.Forms.Timer(this.components);
            this.controlTabs.SuspendLayout();
            this.searchTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultsGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultPreview)).BeginInit();
            this.downloadTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.threadsNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.downloadsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // searchTxtBox
            // 
            this.searchTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTxtBox.Location = new System.Drawing.Point(12, 12);
            this.searchTxtBox.Name = "searchTxtBox";
            this.searchTxtBox.Size = new System.Drawing.Size(639, 20);
            this.searchTxtBox.TabIndex = 0;
            this.searchTxtBox.TextChanged += new System.EventHandler(this.searchTxtBox_TextChanged);
            this.searchTxtBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchTxtBox_KeyDown);
            // 
            // searchBtn
            // 
            this.searchBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchBtn.Location = new System.Drawing.Point(657, 11);
            this.searchBtn.Name = "searchBtn";
            this.searchBtn.Size = new System.Drawing.Size(76, 22);
            this.searchBtn.TabIndex = 1;
            this.searchBtn.Text = "&Search";
            this.searchBtn.UseVisualStyleBackColor = true;
            this.searchBtn.Click += new System.EventHandler(this.searchBtn_Click);
            // 
            // controlTabs
            // 
            this.controlTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.controlTabs.Controls.Add(this.searchTab);
            this.controlTabs.Controls.Add(this.downloadTab);
            this.controlTabs.Location = new System.Drawing.Point(12, 39);
            this.controlTabs.Name = "controlTabs";
            this.controlTabs.SelectedIndex = 0;
            this.controlTabs.Size = new System.Drawing.Size(721, 366);
            this.controlTabs.TabIndex = 2;
            // 
            // searchTab
            // 
            this.searchTab.Controls.Add(this.zipBtn);
            this.searchTab.Controls.Add(this.downloadButton);
            this.searchTab.Controls.Add(this.resultsGrid);
            this.searchTab.Controls.Add(this.resultDescription);
            this.searchTab.Controls.Add(this.resultPreview);
            this.searchTab.Location = new System.Drawing.Point(4, 22);
            this.searchTab.Name = "searchTab";
            this.searchTab.Padding = new System.Windows.Forms.Padding(3);
            this.searchTab.Size = new System.Drawing.Size(713, 340);
            this.searchTab.TabIndex = 0;
            this.searchTab.Text = "Search Results";
            this.searchTab.UseVisualStyleBackColor = true;
            this.searchTab.Click += new System.EventHandler(this.searchTab_Click);
            // 
            // zipBtn
            // 
            this.zipBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.zipBtn.Enabled = false;
            this.zipBtn.Location = new System.Drawing.Point(658, 307);
            this.zipBtn.Name = "zipBtn";
            this.zipBtn.Size = new System.Drawing.Size(49, 27);
            this.zipBtn.TabIndex = 3;
            this.zipBtn.Text = "&ZIP";
            this.zipBtn.UseVisualStyleBackColor = true;
            this.zipBtn.Click += new System.EventHandler(this.zipBtn_Click);
            // 
            // downloadButton
            // 
            this.downloadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.downloadButton.Enabled = false;
            this.downloadButton.Location = new System.Drawing.Point(511, 307);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(144, 27);
            this.downloadButton.TabIndex = 2;
            this.downloadButton.Text = "&Download";
            this.downloadButton.UseVisualStyleBackColor = true;
            this.downloadButton.Click += new System.EventHandler(this.downloadButton_Click);
            // 
            // resultsGrid
            // 
            this.resultsGrid.AllowUserToAddRows = false;
            this.resultsGrid.AllowUserToDeleteRows = false;
            this.resultsGrid.AllowUserToOrderColumns = true;
            this.resultsGrid.AllowUserToResizeColumns = false;
            this.resultsGrid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.resultsGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.resultsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultsGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.resultsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resultsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.resultName,
            this.resultSize,
            this.identifier,
            this.description,
            this.Downloads});
            this.resultsGrid.Location = new System.Drawing.Point(6, 6);
            this.resultsGrid.MultiSelect = false;
            this.resultsGrid.Name = "resultsGrid";
            this.resultsGrid.ReadOnly = true;
            this.resultsGrid.RowHeadersVisible = false;
            this.resultsGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.resultsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.resultsGrid.Size = new System.Drawing.Size(499, 328);
            this.resultsGrid.TabIndex = 0;
            this.resultsGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.resultsGrid_CellContentClick);
            this.resultsGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.resultsGrid_CellDoubleClick);
            this.resultsGrid.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.resultsGrid_RowEnter);
            this.resultsGrid.SelectionChanged += new System.EventHandler(this.resultsGrid_SelectionChanged);
            this.resultsGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.resultsGrid_KeyDown);
            // 
            // resultName
            // 
            this.resultName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.resultName.HeaderText = "Name";
            this.resultName.MinimumWidth = 120;
            this.resultName.Name = "resultName";
            this.resultName.ReadOnly = true;
            // 
            // resultSize
            // 
            this.resultSize.HeaderText = "Size (KiB)";
            this.resultSize.Name = "resultSize";
            this.resultSize.ReadOnly = true;
            // 
            // identifier
            // 
            this.identifier.HeaderText = "Identifier";
            this.identifier.Name = "identifier";
            this.identifier.ReadOnly = true;
            this.identifier.Visible = false;
            // 
            // description
            // 
            this.description.HeaderText = "Description";
            this.description.Name = "description";
            this.description.ReadOnly = true;
            this.description.Visible = false;
            // 
            // Downloads
            // 
            this.Downloads.HeaderText = "Downloads";
            this.Downloads.Name = "Downloads";
            this.Downloads.ReadOnly = true;
            // 
            // resultDescription
            // 
            this.resultDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultDescription.Location = new System.Drawing.Point(511, 208);
            this.resultDescription.Multiline = true;
            this.resultDescription.Name = "resultDescription";
            this.resultDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.resultDescription.Size = new System.Drawing.Size(196, 93);
            this.resultDescription.TabIndex = 1;
            // 
            // resultPreview
            // 
            this.resultPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.resultPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.resultPreview.Location = new System.Drawing.Point(511, 6);
            this.resultPreview.Name = "resultPreview";
            this.resultPreview.Size = new System.Drawing.Size(196, 196);
            this.resultPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.resultPreview.TabIndex = 1;
            this.resultPreview.TabStop = false;
            // 
            // downloadTab
            // 
            this.downloadTab.Controls.Add(this.threadsNum);
            this.downloadTab.Controls.Add(this.threadLbl);
            this.downloadTab.Controls.Add(this.boostChk);
            this.downloadTab.Controls.Add(this.openDownloadsBtn);
            this.downloadTab.Controls.Add(this.cancelDlButton);
            this.downloadTab.Controls.Add(this.downloadsDataGridView);
            this.downloadTab.Controls.Add(this.dlDirLabel);
            this.downloadTab.Controls.Add(this.dlDirTxtBox);
            this.downloadTab.Controls.Add(this.setDirBtn);
            this.downloadTab.Location = new System.Drawing.Point(4, 22);
            this.downloadTab.Name = "downloadTab";
            this.downloadTab.Padding = new System.Windows.Forms.Padding(3);
            this.downloadTab.Size = new System.Drawing.Size(713, 340);
            this.downloadTab.TabIndex = 1;
            this.downloadTab.Text = "Downloads";
            this.downloadTab.UseVisualStyleBackColor = true;
            // 
            // threadsNum
            // 
            this.threadsNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.threadsNum.Location = new System.Drawing.Point(393, 314);
            this.threadsNum.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.threadsNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.threadsNum.Name = "threadsNum";
            this.threadsNum.Size = new System.Drawing.Size(45, 20);
            this.threadsNum.TabIndex = 5;
            this.threadsNum.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.threadsNum.ValueChanged += new System.EventHandler(this.threadsNum_ValueChanged);
            // 
            // threadLbl
            // 
            this.threadLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.threadLbl.AutoSize = true;
            this.threadLbl.Location = new System.Drawing.Point(389, 299);
            this.threadLbl.Name = "threadLbl";
            this.threadLbl.Size = new System.Drawing.Size(49, 13);
            this.threadLbl.TabIndex = 4;
            this.threadLbl.Text = "&Threads:";
            // 
            // boostChk
            // 
            this.boostChk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.boostChk.AutoSize = true;
            this.boostChk.Location = new System.Drawing.Point(337, 316);
            this.boostChk.Margin = new System.Windows.Forms.Padding(0);
            this.boostChk.Name = "boostChk";
            this.boostChk.Size = new System.Drawing.Size(50, 17);
            this.boostChk.TabIndex = 3;
            this.boostChk.Text = "&Aria2";
            this.boostChk.UseVisualStyleBackColor = true;
            this.boostChk.CheckedChanged += new System.EventHandler(this.boostChk_CheckedChanged);
            this.boostChk.Click += new System.EventHandler(this.boostChk_Click);
            // 
            // openDownloadsBtn
            // 
            this.openDownloadsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.openDownloadsBtn.Location = new System.Drawing.Point(444, 312);
            this.openDownloadsBtn.Name = "openDownloadsBtn";
            this.openDownloadsBtn.Size = new System.Drawing.Size(98, 23);
            this.openDownloadsBtn.TabIndex = 6;
            this.openDownloadsBtn.Text = "&Open Downloads";
            this.openDownloadsBtn.UseVisualStyleBackColor = true;
            this.openDownloadsBtn.Click += new System.EventHandler(this.openDownloadsBtn_Click);
            // 
            // cancelDlButton
            // 
            this.cancelDlButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelDlButton.Enabled = false;
            this.cancelDlButton.Location = new System.Drawing.Point(630, 312);
            this.cancelDlButton.Name = "cancelDlButton";
            this.cancelDlButton.Size = new System.Drawing.Size(76, 23);
            this.cancelDlButton.TabIndex = 8;
            this.cancelDlButton.Text = "&Cancel";
            this.cancelDlButton.UseVisualStyleBackColor = true;
            this.cancelDlButton.Click += new System.EventHandler(this.cancelDlButton_Click);
            // 
            // downloadsDataGridView
            // 
            this.downloadsDataGridView.AllowUserToAddRows = false;
            this.downloadsDataGridView.AllowUserToDeleteRows = false;
            this.downloadsDataGridView.AllowUserToOrderColumns = true;
            this.downloadsDataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.downloadsDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.downloadsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.downloadsDataGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.downloadsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.downloadsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DownloadUrl,
            this.Speed,
            this.downloadIdentifier,
            this.fileName,
            this.Running,
            this.DownloadProgress,
            this.DownloadedBytes,
            this.downloadTime,
            this.multiThreadBytesDownloaded});
            this.downloadsDataGridView.Location = new System.Drawing.Point(6, 6);
            this.downloadsDataGridView.MultiSelect = false;
            this.downloadsDataGridView.Name = "downloadsDataGridView";
            this.downloadsDataGridView.ReadOnly = true;
            this.downloadsDataGridView.RowHeadersVisible = false;
            this.downloadsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.downloadsDataGridView.Size = new System.Drawing.Size(701, 290);
            this.downloadsDataGridView.TabIndex = 0;
            this.downloadsDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.downloadsDataGridView_CellContentClick);
            this.downloadsDataGridView.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.downloadsDataGridView_RowEnter);
            this.downloadsDataGridView.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.downloadsDataGridView_RowLeave);
            this.downloadsDataGridView.SelectionChanged += new System.EventHandler(this.downloadsDataGridView_SelectionChanged);
            // 
            // DownloadUrl
            // 
            this.DownloadUrl.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DownloadUrl.DataPropertyName = "DownloadUrl";
            this.DownloadUrl.HeaderText = "File Name";
            this.DownloadUrl.Name = "DownloadUrl";
            this.DownloadUrl.ReadOnly = true;
            // 
            // Speed
            // 
            this.Speed.DataPropertyName = "downloadSpeed";
            this.Speed.HeaderText = "Speed (Avg.)";
            this.Speed.Name = "Speed";
            this.Speed.ReadOnly = true;
            // 
            // downloadIdentifier
            // 
            this.downloadIdentifier.DataPropertyName = "downloadIdentifier";
            this.downloadIdentifier.HeaderText = "downloadIdentifier";
            this.downloadIdentifier.Name = "downloadIdentifier";
            this.downloadIdentifier.ReadOnly = true;
            this.downloadIdentifier.Visible = false;
            // 
            // fileName
            // 
            this.fileName.DataPropertyName = "fileName";
            this.fileName.HeaderText = "fileName";
            this.fileName.Name = "fileName";
            this.fileName.ReadOnly = true;
            this.fileName.Visible = false;
            // 
            // Running
            // 
            this.Running.DataPropertyName = "Running";
            this.Running.HeaderText = "Running";
            this.Running.Name = "Running";
            this.Running.ReadOnly = true;
            this.Running.Visible = false;
            // 
            // DownloadProgress
            // 
            this.DownloadProgress.DataPropertyName = "DownloadProgress";
            this.DownloadProgress.HeaderText = "Progress";
            this.DownloadProgress.Name = "DownloadProgress";
            this.DownloadProgress.ReadOnly = true;
            // 
            // DownloadedBytes
            // 
            this.DownloadedBytes.DataPropertyName = "downloadedBytes";
            this.DownloadedBytes.HeaderText = "DownloadedBytes";
            this.DownloadedBytes.Name = "DownloadedBytes";
            this.DownloadedBytes.ReadOnly = true;
            this.DownloadedBytes.Visible = false;
            // 
            // downloadTime
            // 
            this.downloadTime.DataPropertyName = "downloadTime";
            this.downloadTime.HeaderText = "downloadTime";
            this.downloadTime.Name = "downloadTime";
            this.downloadTime.ReadOnly = true;
            this.downloadTime.Visible = false;
            // 
            // multiThreadBytesDownloaded
            // 
            this.multiThreadBytesDownloaded.DataPropertyName = "multiThreadBytesDownloaded";
            this.multiThreadBytesDownloaded.HeaderText = "multiThreadBytesDownloaded";
            this.multiThreadBytesDownloaded.Name = "multiThreadBytesDownloaded";
            this.multiThreadBytesDownloaded.ReadOnly = true;
            this.multiThreadBytesDownloaded.Visible = false;
            // 
            // dlDirLabel
            // 
            this.dlDirLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dlDirLabel.AutoSize = true;
            this.dlDirLabel.Location = new System.Drawing.Point(3, 299);
            this.dlDirLabel.Name = "dlDirLabel";
            this.dlDirLabel.Size = new System.Drawing.Size(108, 13);
            this.dlDirLabel.TabIndex = 1;
            this.dlDirLabel.Text = "&Downloads Directory:";
            // 
            // dlDirTxtBox
            // 
            this.dlDirTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dlDirTxtBox.BackColor = System.Drawing.SystemColors.Window;
            this.dlDirTxtBox.Location = new System.Drawing.Point(6, 314);
            this.dlDirTxtBox.Name = "dlDirTxtBox";
            this.dlDirTxtBox.ReadOnly = true;
            this.dlDirTxtBox.Size = new System.Drawing.Size(325, 20);
            this.dlDirTxtBox.TabIndex = 2;
            this.dlDirTxtBox.TextChanged += new System.EventHandler(this.dlDirTxtBox_TextChanged);
            // 
            // setDirBtn
            // 
            this.setDirBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.setDirBtn.Location = new System.Drawing.Point(548, 312);
            this.setDirBtn.Name = "setDirBtn";
            this.setDirBtn.Size = new System.Drawing.Size(76, 23);
            this.setDirBtn.TabIndex = 7;
            this.setDirBtn.Text = "&Browse...";
            this.setDirBtn.UseVisualStyleBackColor = true;
            this.setDirBtn.Click += new System.EventHandler(this.setDirBtn_Click);
            // 
            // progressTimer
            // 
            this.progressTimer.Interval = 500;
            this.progressTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(745, 417);
            this.Controls.Add(this.controlTabs);
            this.Controls.Add(this.searchBtn);
            this.Controls.Add(this.searchTxtBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(563, 378);
            this.Name = "MainForm";
            this.Text = "SoftwareShelf Desktop 1.3.1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.controlTabs.ResumeLayout(false);
            this.searchTab.ResumeLayout(false);
            this.searchTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultsGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultPreview)).EndInit();
            this.downloadTab.ResumeLayout(false);
            this.downloadTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.threadsNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.downloadsDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox searchTxtBox;
        private System.Windows.Forms.Button searchBtn;
        private System.Windows.Forms.TabControl controlTabs;
        private System.Windows.Forms.TabPage searchTab;
        private System.Windows.Forms.TabPage downloadTab;
        private System.Windows.Forms.TextBox resultDescription;
        private System.Windows.Forms.PictureBox resultPreview;
        private System.Windows.Forms.DataGridView resultsGrid;
        private System.Windows.Forms.Button downloadButton;
        private System.Windows.Forms.Button setDirBtn;
        private System.Windows.Forms.Label dlDirLabel;
        private System.Windows.Forms.TextBox dlDirTxtBox;
        private System.Windows.Forms.DataGridView downloadsDataGridView;
        public System.Windows.Forms.Timer progressTimer;
        private System.Windows.Forms.Button cancelDlButton;
        private System.Windows.Forms.Button openDownloadsBtn;
        public System.Windows.Forms.CheckBox boostChk;
        private System.Windows.Forms.DataGridViewTextBoxColumn DownloadUrl;
        private System.Windows.Forms.DataGridViewTextBoxColumn Speed;
        private System.Windows.Forms.DataGridViewTextBoxColumn downloadIdentifier;
        private System.Windows.Forms.DataGridViewTextBoxColumn fileName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Running;
        private System.Windows.Forms.DataGridViewTextBoxColumn DownloadProgress;
        private System.Windows.Forms.DataGridViewTextBoxColumn DownloadedBytes;
        private System.Windows.Forms.DataGridViewTextBoxColumn downloadTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn multiThreadBytesDownloaded;
        private System.Windows.Forms.DataGridViewTextBoxColumn resultName;
        private System.Windows.Forms.DataGridViewTextBoxColumn resultSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn identifier;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Downloads;
        private System.Windows.Forms.Button zipBtn;
        private System.Windows.Forms.NumericUpDown threadsNum;
        private System.Windows.Forms.Label threadLbl;
    }
}

