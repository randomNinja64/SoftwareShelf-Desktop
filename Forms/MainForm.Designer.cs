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
            this.downloadTab = new System.Windows.Forms.TabPage();
            this.torrentChk = new System.Windows.Forms.CheckBox();
            this.threadsNum = new System.Windows.Forms.NumericUpDown();
            this.threadLbl = new System.Windows.Forms.Label();
            this.boostChk = new System.Windows.Forms.CheckBox();
            this.openDownloadsBtn = new System.Windows.Forms.Button();
            this.cancelDlButton = new System.Windows.Forms.Button();
            this.dlDirLabel = new System.Windows.Forms.Label();
            this.dlDirTxtBox = new System.Windows.Forms.TextBox();
            this.setDirBtn = new System.Windows.Forms.Button();
            this.downloadsDataGridView = new System.Windows.Forms.DataGridView();
            this.DownloadUrl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Speed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DownloadProgress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.searchTab = new System.Windows.Forms.TabPage();
            this.topicInfoLbl = new System.Windows.Forms.Label();
            this.publishedInfoLbl = new System.Windows.Forms.Label();
            this.creatorInfoLbl = new System.Windows.Forms.Label();
            this.searchPanel = new System.Windows.Forms.Panel();
            this.latestBtn = new System.Windows.Forms.Button();
            this.topicTxt = new System.Windows.Forms.TextBox();
            this.typeDropDown = new System.Windows.Forms.ComboBox();
            this.topicLbl = new System.Windows.Forms.Label();
            this.categoryLbl = new System.Windows.Forms.Label();
            this.searchBtn = new System.Windows.Forms.Button();
            this.creatorTxt = new System.Windows.Forms.TextBox();
            this.queryLbl = new System.Windows.Forms.Label();
            this.creatorLbl = new System.Windows.Forms.Label();
            this.searchTxtBox = new System.Windows.Forms.TextBox();
            this.pubYrLbl = new System.Windows.Forms.Label();
            this.yearTxt = new System.Windows.Forms.TextBox();
            this.zipBtn = new System.Windows.Forms.Button();
            this.downloadButton = new System.Windows.Forms.Button();
            this.resultDescription = new System.Windows.Forms.TextBox();
            this.resultsGrid = new System.Windows.Forms.DataGridView();
            this.resultName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.avgRating = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resultSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Downloads = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.controlTabs = new System.Windows.Forms.TabControl();
            this.reviewButton = new System.Windows.Forms.Button();
            this.resultPreview = new System.Windows.Forms.PictureBox();
            this.browseTab = new System.Windows.Forms.TabPage();
            this.browsePanel = new System.Windows.Forms.Panel();
            this.collectionSearchLbl = new System.Windows.Forms.Label();
            this.collectionSearchTxt = new System.Windows.Forms.TextBox();
            this.collectionSearchBtn = new System.Windows.Forms.Button();
            this.collectionsLbl = new System.Windows.Forms.Label();
            this.collectionsList = new System.Windows.Forms.ListBox();
            this.collectionBtn = new System.Windows.Forms.Button();
            this.sortLbl = new System.Windows.Forms.Label();
            this.sortDropDown = new System.Windows.Forms.ComboBox();
            this.browseTypeLbl = new System.Windows.Forms.Label();
            this.browseTypeDropDown = new System.Windows.Forms.ComboBox();
            this.savedBtn = new System.Windows.Forms.Button();
            this.downloadTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.threadsNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.downloadsDataGridView)).BeginInit();
            this.searchTab.SuspendLayout();
            this.searchPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultsGrid)).BeginInit();
            this.controlTabs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultPreview)).BeginInit();
            this.browseTab.SuspendLayout();
            this.browsePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // downloadTab
            // 
            this.downloadTab.Controls.Add(this.torrentChk);
            this.downloadTab.Controls.Add(this.threadsNum);
            this.downloadTab.Controls.Add(this.threadLbl);
            this.downloadTab.Controls.Add(this.boostChk);
            this.downloadTab.Controls.Add(this.openDownloadsBtn);
            this.downloadTab.Controls.Add(this.cancelDlButton);
            this.downloadTab.Controls.Add(this.dlDirLabel);
            this.downloadTab.Controls.Add(this.dlDirTxtBox);
            this.downloadTab.Controls.Add(this.setDirBtn);
            this.downloadTab.Controls.Add(this.downloadsDataGridView);
            this.downloadTab.Location = new System.Drawing.Point(4, 22);
            this.downloadTab.Name = "downloadTab";
            this.downloadTab.Padding = new System.Windows.Forms.Padding(3);
            this.downloadTab.Size = new System.Drawing.Size(906, 425);
            this.downloadTab.TabIndex = 2;
            this.downloadTab.Text = "Downloads";
            this.downloadTab.UseVisualStyleBackColor = true;
            this.downloadTab.Resize += new System.EventHandler(this.tab_Resize);
            // 
            // torrentChk
            // 
            this.torrentChk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.torrentChk.AutoSize = true;
            this.torrentChk.Checked = true;
            this.torrentChk.CheckState = System.Windows.Forms.CheckState.Checked;
            this.torrentChk.Enabled = false;
            this.torrentChk.Location = new System.Drawing.Point(483, 400);
            this.torrentChk.Margin = new System.Windows.Forms.Padding(0);
            this.torrentChk.Name = "torrentChk";
            this.torrentChk.Size = new System.Drawing.Size(106, 17);
            this.torrentChk.TabIndex = 4;
            this.torrentChk.Text = "&Process Torrents";
            this.torrentChk.UseVisualStyleBackColor = true;
            this.torrentChk.CheckedChanged += new System.EventHandler(this.torrentChk_CheckedChanged);
            // 
            // threadsNum
            // 
            this.threadsNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.threadsNum.Enabled = false;
            this.threadsNum.Location = new System.Drawing.Point(593, 398);
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
            this.threadsNum.TabIndex = 6;
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
            this.threadLbl.Location = new System.Drawing.Point(590, 380);
            this.threadLbl.Name = "threadLbl";
            this.threadLbl.Size = new System.Drawing.Size(49, 13);
            this.threadLbl.TabIndex = 5;
            this.threadLbl.Text = "&Threads:";
            // 
            // boostChk
            // 
            this.boostChk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.boostChk.AutoSize = true;
            this.boostChk.Location = new System.Drawing.Point(429, 400);
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
            this.openDownloadsBtn.Location = new System.Drawing.Point(642, 396);
            this.openDownloadsBtn.Name = "openDownloadsBtn";
            this.openDownloadsBtn.Size = new System.Drawing.Size(98, 23);
            this.openDownloadsBtn.TabIndex = 7;
            this.openDownloadsBtn.Text = "&Open Downloads";
            this.openDownloadsBtn.UseVisualStyleBackColor = true;
            this.openDownloadsBtn.Click += new System.EventHandler(this.openDownloadsBtn_Click);
            // 
            // cancelDlButton
            // 
            this.cancelDlButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelDlButton.Enabled = false;
            this.cancelDlButton.Location = new System.Drawing.Point(824, 396);
            this.cancelDlButton.Name = "cancelDlButton";
            this.cancelDlButton.Size = new System.Drawing.Size(76, 23);
            this.cancelDlButton.TabIndex = 9;
            this.cancelDlButton.Text = "&Cancel";
            this.cancelDlButton.UseVisualStyleBackColor = true;
            this.cancelDlButton.Click += new System.EventHandler(this.cancelDlButton_Click);
            // 
            // dlDirLabel
            // 
            this.dlDirLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dlDirLabel.AutoSize = true;
            this.dlDirLabel.Location = new System.Drawing.Point(3, 380);
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
            this.dlDirTxtBox.Location = new System.Drawing.Point(6, 398);
            this.dlDirTxtBox.Name = "dlDirTxtBox";
            this.dlDirTxtBox.ReadOnly = true;
            this.dlDirTxtBox.Size = new System.Drawing.Size(419, 20);
            this.dlDirTxtBox.TabIndex = 2;
            // 
            // setDirBtn
            // 
            this.setDirBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.setDirBtn.Location = new System.Drawing.Point(744, 396);
            this.setDirBtn.Name = "setDirBtn";
            this.setDirBtn.Size = new System.Drawing.Size(76, 23);
            this.setDirBtn.TabIndex = 8;
            this.setDirBtn.Text = "&Browse...";
            this.setDirBtn.UseVisualStyleBackColor = true;
            this.setDirBtn.Click += new System.EventHandler(this.setDirBtn_Click);
            // 
            // downloadsDataGridView
            // 
            this.downloadsDataGridView.AllowUserToAddRows = false;
            this.downloadsDataGridView.AllowUserToDeleteRows = false;
            this.downloadsDataGridView.AllowUserToOrderColumns = true;
            this.downloadsDataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.downloadsDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.downloadsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.downloadsDataGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.downloadsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.downloadsDataGridView.AutoGenerateColumns = false;
            this.downloadsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DownloadUrl,
            this.Speed,
            this.DownloadProgress});
            this.downloadsDataGridView.Location = new System.Drawing.Point(6, 7);
            this.downloadsDataGridView.MultiSelect = false;
            this.downloadsDataGridView.Name = "downloadsDataGridView";
            this.downloadsDataGridView.ReadOnly = true;
            this.downloadsDataGridView.RowHeadersVisible = false;
            this.downloadsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.downloadsDataGridView.Size = new System.Drawing.Size(894, 370);
            this.downloadsDataGridView.TabIndex = 0;
            this.downloadsDataGridView.SelectionChanged += new System.EventHandler(this.downloadsDataGridView_SelectionChanged);
            // 
            // DownloadUrl
            // 
            this.DownloadUrl.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DownloadUrl.DataPropertyName = "fileName";
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
            // DownloadProgress
            // 
            this.DownloadProgress.DataPropertyName = "DownloadProgress";
            this.DownloadProgress.HeaderText = "Progress";
            this.DownloadProgress.Name = "DownloadProgress";
            this.DownloadProgress.ReadOnly = true;
            // 
            // searchTab
            // 
            this.searchTab.Controls.Add(this.reviewButton);
            this.searchTab.Controls.Add(this.topicInfoLbl);
            this.searchTab.Controls.Add(this.publishedInfoLbl);
            this.searchTab.Controls.Add(this.creatorInfoLbl);
            this.searchTab.Controls.Add(this.searchPanel);
            this.searchTab.Controls.Add(this.zipBtn);
            this.searchTab.Controls.Add(this.downloadButton);
            this.searchTab.Controls.Add(this.resultDescription);
            this.searchTab.Controls.Add(this.resultPreview);
            this.searchTab.Controls.Add(this.resultsGrid);
            this.searchTab.Location = new System.Drawing.Point(4, 22);
            this.searchTab.Name = "searchTab";
            this.searchTab.Padding = new System.Windows.Forms.Padding(3);
            this.searchTab.Size = new System.Drawing.Size(906, 425);
            this.searchTab.TabIndex = 0;
            this.searchTab.Text = "Search";
            this.searchTab.UseVisualStyleBackColor = true;
            this.searchTab.Resize += new System.EventHandler(this.tab_Resize);
            // 
            // topicInfoLbl
            // 
            this.topicInfoLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.topicInfoLbl.AutoSize = true;
            this.topicInfoLbl.Location = new System.Drawing.Point(704, 243);
            this.topicInfoLbl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.topicInfoLbl.Name = "topicInfoLbl";
            this.topicInfoLbl.Size = new System.Drawing.Size(37, 13);
            this.topicInfoLbl.TabIndex = 4;
            this.topicInfoLbl.Text = "Topic:";
            // 
            // publishedInfoLbl
            // 
            this.publishedInfoLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.publishedInfoLbl.AutoSize = true;
            this.publishedInfoLbl.Location = new System.Drawing.Point(704, 225);
            this.publishedInfoLbl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.publishedInfoLbl.Name = "publishedInfoLbl";
            this.publishedInfoLbl.Size = new System.Drawing.Size(56, 13);
            this.publishedInfoLbl.TabIndex = 3;
            this.publishedInfoLbl.Text = "Published:";
            // 
            // creatorInfoLbl
            // 
            this.creatorInfoLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.creatorInfoLbl.AutoSize = true;
            this.creatorInfoLbl.Location = new System.Drawing.Point(704, 207);
            this.creatorInfoLbl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.creatorInfoLbl.Name = "creatorInfoLbl";
            this.creatorInfoLbl.Size = new System.Drawing.Size(47, 13);
            this.creatorInfoLbl.TabIndex = 2;
            this.creatorInfoLbl.Text = "Creator: ";
            // 
            // searchPanel
            // 
            this.searchPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.searchPanel.BackColor = System.Drawing.SystemColors.Control;
            this.searchPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchPanel.Controls.Add(this.latestBtn);
            this.searchPanel.Controls.Add(this.topicTxt);
            this.searchPanel.Controls.Add(this.typeDropDown);
            this.searchPanel.Controls.Add(this.topicLbl);
            this.searchPanel.Controls.Add(this.categoryLbl);
            this.searchPanel.Controls.Add(this.searchBtn);
            this.searchPanel.Controls.Add(this.creatorTxt);
            this.searchPanel.Controls.Add(this.queryLbl);
            this.searchPanel.Controls.Add(this.creatorLbl);
            this.searchPanel.Controls.Add(this.searchTxtBox);
            this.searchPanel.Controls.Add(this.pubYrLbl);
            this.searchPanel.Controls.Add(this.yearTxt);
            this.searchPanel.Location = new System.Drawing.Point(6, 7);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.Size = new System.Drawing.Size(196, 411);
            this.searchPanel.TabIndex = 0;
            // 
            // latestBtn
            // 
            this.latestBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.latestBtn.Location = new System.Drawing.Point(5, 355);
            this.latestBtn.Name = "latestBtn";
            this.latestBtn.Size = new System.Drawing.Size(184, 22);
            this.latestBtn.TabIndex = 10;
            this.latestBtn.Text = "&Latest Items for Type";
            this.latestBtn.UseVisualStyleBackColor = true;
            this.latestBtn.Click += new System.EventHandler(this.latestBtn_Click);
            // 
            // topicTxt
            // 
            this.topicTxt.Location = new System.Drawing.Point(5, 112);
            this.topicTxt.Name = "topicTxt";
            this.topicTxt.Size = new System.Drawing.Size(184, 20);
            this.topicTxt.TabIndex = 5;
            this.topicTxt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchTxtBox_KeyDown);
            // 
            // typeDropDown
            // 
            this.typeDropDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.typeDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typeDropDown.FormattingEnabled = true;
            this.typeDropDown.Items.AddRange(new object[] {
            "All",
            "Audio",
            "Books",
            "Images",
            "Movies",
            "Software"});
            this.typeDropDown.Location = new System.Drawing.Point(5, 328);
            this.typeDropDown.Name = "typeDropDown";
            this.typeDropDown.Size = new System.Drawing.Size(184, 21);
            this.typeDropDown.TabIndex = 9;
            // 
            // topicLbl
            // 
            this.topicLbl.AutoSize = true;
            this.topicLbl.Location = new System.Drawing.Point(5, 93);
            this.topicLbl.Margin = new System.Windows.Forms.Padding(3);
            this.topicLbl.Name = "topicLbl";
            this.topicLbl.Size = new System.Drawing.Size(37, 13);
            this.topicLbl.TabIndex = 4;
            this.topicLbl.Text = "T&opic:";
            // 
            // categoryLbl
            // 
            this.categoryLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.categoryLbl.AutoSize = true;
            this.categoryLbl.Location = new System.Drawing.Point(5, 309);
            this.categoryLbl.Margin = new System.Windows.Forms.Padding(3);
            this.categoryLbl.Name = "categoryLbl";
            this.categoryLbl.Size = new System.Drawing.Size(34, 13);
            this.categoryLbl.TabIndex = 8;
            this.categoryLbl.Text = "&Type:";
            // 
            // searchBtn
            // 
            this.searchBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.searchBtn.Location = new System.Drawing.Point(5, 383);
            this.searchBtn.Name = "searchBtn";
            this.searchBtn.Size = new System.Drawing.Size(184, 22);
            this.searchBtn.TabIndex = 11;
            this.searchBtn.Text = "&Search";
            this.searchBtn.UseVisualStyleBackColor = true;
            this.searchBtn.Click += new System.EventHandler(this.searchBtn_Click);
            // 
            // creatorTxt
            // 
            this.creatorTxt.Location = new System.Drawing.Point(5, 67);
            this.creatorTxt.Name = "creatorTxt";
            this.creatorTxt.Size = new System.Drawing.Size(184, 20);
            this.creatorTxt.TabIndex = 3;
            this.creatorTxt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchTxtBox_KeyDown);
            // 
            // queryLbl
            // 
            this.queryLbl.AutoSize = true;
            this.queryLbl.Location = new System.Drawing.Point(5, 3);
            this.queryLbl.Margin = new System.Windows.Forms.Padding(3);
            this.queryLbl.Name = "queryLbl";
            this.queryLbl.Size = new System.Drawing.Size(51, 13);
            this.queryLbl.TabIndex = 0;
            this.queryLbl.Text = "&Keyword:";
            // 
            // creatorLbl
            // 
            this.creatorLbl.AutoSize = true;
            this.creatorLbl.Location = new System.Drawing.Point(5, 48);
            this.creatorLbl.Margin = new System.Windows.Forms.Padding(3);
            this.creatorLbl.Name = "creatorLbl";
            this.creatorLbl.Size = new System.Drawing.Size(44, 13);
            this.creatorLbl.TabIndex = 2;
            this.creatorLbl.Text = "&Creator:";
            // 
            // searchTxtBox
            // 
            this.searchTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTxtBox.Location = new System.Drawing.Point(5, 22);
            this.searchTxtBox.Name = "searchTxtBox";
            this.searchTxtBox.Size = new System.Drawing.Size(184, 20);
            this.searchTxtBox.TabIndex = 1;
            this.searchTxtBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchTxtBox_KeyDown);
            // 
            // pubYrLbl
            // 
            this.pubYrLbl.AutoSize = true;
            this.pubYrLbl.Location = new System.Drawing.Point(5, 138);
            this.pubYrLbl.Margin = new System.Windows.Forms.Padding(3);
            this.pubYrLbl.Name = "pubYrLbl";
            this.pubYrLbl.Size = new System.Drawing.Size(87, 13);
            this.pubYrLbl.TabIndex = 6;
            this.pubYrLbl.Text = "&Publication Year:";
            // 
            // yearTxt
            // 
            this.yearTxt.Location = new System.Drawing.Point(5, 157);
            this.yearTxt.Name = "yearTxt";
            this.yearTxt.Size = new System.Drawing.Size(59, 20);
            this.yearTxt.TabIndex = 7;
            this.yearTxt.TextChanged += new System.EventHandler(this.yearTxt_TextChanged);
            this.yearTxt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchTxtBox_KeyDown);
            // 
            // zipBtn
            // 
            this.zipBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.zipBtn.Enabled = false;
            this.zipBtn.Location = new System.Drawing.Point(849, 391);
            this.zipBtn.Name = "zipBtn";
            this.zipBtn.Size = new System.Drawing.Size(51, 27);
            this.zipBtn.TabIndex = 7;
            this.zipBtn.Text = "&ZIP";
            this.zipBtn.UseVisualStyleBackColor = true;
            // 
            // downloadButton
            // 
            this.downloadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.downloadButton.Enabled = false;
            this.downloadButton.Location = new System.Drawing.Point(704, 391);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(139, 27);
            this.downloadButton.TabIndex = 6;
            this.downloadButton.Text = "&Download";
            this.downloadButton.UseVisualStyleBackColor = true;
            // 
            // resultDescription
            // 
            this.resultDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultDescription.Location = new System.Drawing.Point(704, 261);
            this.resultDescription.Multiline = true;
            this.resultDescription.Name = "resultDescription";
            this.resultDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.resultDescription.Size = new System.Drawing.Size(196, 93);
            this.resultDescription.TabIndex = 5;
            // 
            // resultsGrid
            // 
            this.resultsGrid.AllowUserToAddRows = false;
            this.resultsGrid.AllowUserToDeleteRows = false;
            this.resultsGrid.AllowUserToOrderColumns = true;
            this.resultsGrid.AllowUserToResizeColumns = false;
            this.resultsGrid.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.resultsGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.resultsGrid.AutoGenerateColumns = false;
            this.resultsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultsGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.resultsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resultsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.resultName,
            this.avgRating,
            this.resultSize,
            this.Downloads});
            this.resultsGrid.Location = new System.Drawing.Point(208, 7);
            this.resultsGrid.MultiSelect = false;
            this.resultsGrid.Name = "resultsGrid";
            this.resultsGrid.ReadOnly = true;
            this.resultsGrid.RowHeadersVisible = false;
            this.resultsGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.resultsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.resultsGrid.Size = new System.Drawing.Size(490, 411);
            this.resultsGrid.TabIndex = 1;
            // 
            // resultName
            // 
            this.resultName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.resultName.DataPropertyName = "title";
            this.resultName.HeaderText = "Name";
            this.resultName.MinimumWidth = 120;
            this.resultName.Name = "resultName";
            this.resultName.ReadOnly = true;
            // 
            // avgRating
            // 
            this.avgRating.DataPropertyName = "avgRating";
            this.avgRating.FillWeight = 20F;
            this.avgRating.HeaderText = "Rating";
            this.avgRating.MaxInputLength = 10;
            this.avgRating.Name = "avgRating";
            this.avgRating.ReadOnly = true;
            this.avgRating.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.avgRating.Width = 45;
            // 
            // resultSize
            // 
            this.resultSize.DataPropertyName = "size";
            this.resultSize.HeaderText = "Size";
            this.resultSize.MinimumWidth = 10;
            this.resultSize.Name = "resultSize";
            this.resultSize.ReadOnly = true;
            // 
            // Downloads
            // 
            this.Downloads.DataPropertyName = "downloads";
            this.Downloads.HeaderText = "Downloads";
            this.Downloads.Name = "Downloads";
            this.Downloads.ReadOnly = true;
            // 
            // controlTabs
            // 
            this.controlTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.controlTabs.Controls.Add(this.searchTab);
            this.controlTabs.Controls.Add(this.browseTab);
            this.controlTabs.Controls.Add(this.downloadTab);
            this.controlTabs.Location = new System.Drawing.Point(10, 10);
            this.controlTabs.Margin = new System.Windows.Forms.Padding(1);
            this.controlTabs.Name = "controlTabs";
            this.controlTabs.SelectedIndex = 0;
            this.controlTabs.Size = new System.Drawing.Size(914, 451);
            this.controlTabs.TabIndex = 0;
            this.controlTabs.SelectedIndexChanged += new System.EventHandler(this.controlTabs_SelectedIndexChanged);
            // 
            // reviewButton
            // 
            this.reviewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.reviewButton.Enabled = false;
            this.reviewButton.Location = new System.Drawing.Point(704, 359);
            this.reviewButton.Name = "reviewButton";
            this.reviewButton.Size = new System.Drawing.Size(196, 27);
            this.reviewButton.TabIndex = 8;
            this.reviewButton.Text = "&Reviews";
            this.reviewButton.UseVisualStyleBackColor = true;
            // 
            // resultPreview
            // 
            this.resultPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.resultPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.resultPreview.Image = global::SoftwareShelf_Desktop.Properties.Resources.placeholder;
            this.resultPreview.Location = new System.Drawing.Point(704, 7);
            this.resultPreview.Name = "resultPreview";
            this.resultPreview.Size = new System.Drawing.Size(196, 196);
            this.resultPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.resultPreview.TabIndex = 1;
            this.resultPreview.TabStop = false;
            // 
            // browseTab
            // 
            this.browseTab.Controls.Add(this.browsePanel);
            this.browseTab.Location = new System.Drawing.Point(4, 22);
            this.browseTab.Name = "browseTab";
            this.browseTab.Padding = new System.Windows.Forms.Padding(3);
            this.browseTab.Size = new System.Drawing.Size(906, 425);
            this.browseTab.TabIndex = 1;
            this.browseTab.Text = "Browse";
            this.browseTab.UseVisualStyleBackColor = true;
            this.browseTab.Resize += new System.EventHandler(this.tab_Resize);
            // 
            // browsePanel
            // 
            this.browsePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.browsePanel.BackColor = System.Drawing.SystemColors.Control;
            this.browsePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.browsePanel.Controls.Add(this.collectionSearchLbl);
            this.browsePanel.Controls.Add(this.collectionSearchTxt);
            this.browsePanel.Controls.Add(this.collectionSearchBtn);
            this.browsePanel.Controls.Add(this.collectionsLbl);
            this.browsePanel.Controls.Add(this.collectionsList);
            this.browsePanel.Controls.Add(this.collectionBtn);
            this.browsePanel.Controls.Add(this.sortLbl);
            this.browsePanel.Controls.Add(this.sortDropDown);
            this.browsePanel.Controls.Add(this.browseTypeLbl);
            this.browsePanel.Controls.Add(this.browseTypeDropDown);
            this.browsePanel.Controls.Add(this.savedBtn);
            this.browsePanel.Location = new System.Drawing.Point(6, 7);
            this.browsePanel.Name = "browsePanel";
            this.browsePanel.Size = new System.Drawing.Size(196, 411);
            this.browsePanel.TabIndex = 0;
            // 
            // collectionSearchLbl
            // 
            this.collectionSearchLbl.AutoSize = true;
            this.collectionSearchLbl.Location = new System.Drawing.Point(5, 3);
            this.collectionSearchLbl.Margin = new System.Windows.Forms.Padding(3);
            this.collectionSearchLbl.Name = "collectionSearchLbl";
            this.collectionSearchLbl.Size = new System.Drawing.Size(99, 13);
            this.collectionSearchLbl.TabIndex = 7;
            this.collectionSearchLbl.Text = "&Collection Search:";
            // 
            // collectionSearchTxt
            // 
            this.collectionSearchTxt.Location = new System.Drawing.Point(5, 22);
            this.collectionSearchTxt.Name = "collectionSearchTxt";
            this.collectionSearchTxt.Size = new System.Drawing.Size(184, 20);
            this.collectionSearchTxt.TabIndex = 0;
            this.collectionSearchTxt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.collectionSearchTxt_KeyDown);
            // 
            // collectionSearchBtn
            // 
            this.collectionSearchBtn.Location = new System.Drawing.Point(5, 48);
            this.collectionSearchBtn.Name = "collectionSearchBtn";
            this.collectionSearchBtn.Size = new System.Drawing.Size(184, 22);
            this.collectionSearchBtn.TabIndex = 1;
            this.collectionSearchBtn.Text = "Searc&h";
            this.collectionSearchBtn.UseVisualStyleBackColor = true;
            this.collectionSearchBtn.Click += new System.EventHandler(this.collectionSearchBtn_Click);
            // 
            // collectionsLbl
            // 
            this.collectionsLbl.AutoSize = true;
            this.collectionsLbl.Location = new System.Drawing.Point(5, 76);
            this.collectionsLbl.Margin = new System.Windows.Forms.Padding(3);
            this.collectionsLbl.Name = "collectionsLbl";
            this.collectionsLbl.Size = new System.Drawing.Size(66, 13);
            this.collectionsLbl.TabIndex = 8;
            this.collectionsLbl.Text = "Collectio&ns:";
            // 
            // collectionsList
            // 
            this.collectionsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.collectionsList.FormattingEnabled = true;
            this.collectionsList.IntegralHeight = false;
            this.collectionsList.Location = new System.Drawing.Point(5, 95);
            this.collectionsList.Name = "collectionsList";
            this.collectionsList.Size = new System.Drawing.Size(184, 162);
            this.collectionsList.TabIndex = 2;
            this.collectionsList.SelectedIndexChanged += new System.EventHandler(this.collectionsList_SelectedIndexChanged);
            this.collectionsList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.collectionsList_MouseMove);
            this.collectionsList.MouseLeave += new System.EventHandler(this.collectionsList_MouseLeave);
            // 
            // collectionBtn
            // 
            this.collectionBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.collectionBtn.Enabled = false;
            this.collectionBtn.Location = new System.Drawing.Point(5, 355);
            this.collectionBtn.Name = "collectionBtn";
            this.collectionBtn.Size = new System.Drawing.Size(184, 22);
            this.collectionBtn.TabIndex = 5;
            this.collectionBtn.Text = "&Save Collection";
            this.collectionBtn.UseVisualStyleBackColor = true;
            this.collectionBtn.Click += new System.EventHandler(this.collectionBtn_Click);
            // 
            // sortLbl
            // 
            this.sortLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sortLbl.AutoSize = true;
            this.sortLbl.Location = new System.Drawing.Point(5, 263);
            this.sortLbl.Margin = new System.Windows.Forms.Padding(3);
            this.sortLbl.Name = "sortLbl";
            this.sortLbl.Size = new System.Drawing.Size(29, 13);
            this.sortLbl.TabIndex = 3;
            this.sortLbl.Text = "S&ort:";
            // 
            // sortDropDown
            // 
            this.sortDropDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sortDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sortDropDown.FormattingEnabled = true;
            this.sortDropDown.Items.AddRange(new object[] {
            "Latest",
            "Top"});
            this.sortDropDown.SelectedIndex = 1;
            this.sortDropDown.Location = new System.Drawing.Point(5, 282);
            this.sortDropDown.Name = "sortDropDown";
            this.sortDropDown.Size = new System.Drawing.Size(184, 21);
            this.sortDropDown.TabIndex = 3;
            this.sortDropDown.SelectedIndexChanged += new System.EventHandler(this.browseFilter_Changed);
            // 
            // browseTypeLbl
            // 
            this.browseTypeLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.browseTypeLbl.AutoSize = true;
            this.browseTypeLbl.Location = new System.Drawing.Point(5, 309);
            this.browseTypeLbl.Margin = new System.Windows.Forms.Padding(3);
            this.browseTypeLbl.Name = "browseTypeLbl";
            this.browseTypeLbl.Size = new System.Drawing.Size(34, 13);
            this.browseTypeLbl.TabIndex = 5;
            this.browseTypeLbl.Text = "&Type:";
            // 
            // browseTypeDropDown
            // 
            this.browseTypeDropDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.browseTypeDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.browseTypeDropDown.FormattingEnabled = true;
            this.browseTypeDropDown.Items.AddRange(new object[] {
            "Audio",
            "Books",
            "Images",
            "Movies",
            "Software"});
            this.browseTypeDropDown.SelectedIndex = 4;
            this.browseTypeDropDown.Location = new System.Drawing.Point(5, 328);
            this.browseTypeDropDown.Name = "browseTypeDropDown";
            this.browseTypeDropDown.Size = new System.Drawing.Size(184, 21);
            this.browseTypeDropDown.TabIndex = 4;
            this.browseTypeDropDown.SelectedIndexChanged += new System.EventHandler(this.browseFilter_Changed);
            // 
            // savedBtn
            // 
            this.savedBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.savedBtn.Location = new System.Drawing.Point(5, 383);
            this.savedBtn.Name = "savedBtn";
            this.savedBtn.Size = new System.Drawing.Size(184, 22);
            this.savedBtn.TabIndex = 6;
            this.savedBtn.Text = "Sa&ved Collections";
            this.savedBtn.UseVisualStyleBackColor = true;
            this.savedBtn.Click += new System.EventHandler(this.savedBtn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 471);
            this.Controls.Add(this.controlTabs);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(563, 500);
            this.Name = "MainForm";
            this.Text = "SoftwareShelf Desktop 1.7.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.downloadTab.ResumeLayout(false);
            this.downloadTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.threadsNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.downloadsDataGridView)).EndInit();
            this.searchTab.ResumeLayout(false);
            this.searchTab.PerformLayout();
            this.searchPanel.ResumeLayout(false);
            this.searchPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultsGrid)).EndInit();
            this.controlTabs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.resultPreview)).EndInit();
            this.browseTab.ResumeLayout(false);
            this.browsePanel.ResumeLayout(false);
            this.browsePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabPage downloadTab;
        private System.Windows.Forms.NumericUpDown threadsNum;
        private System.Windows.Forms.Label threadLbl;
        public System.Windows.Forms.CheckBox boostChk;
        private System.Windows.Forms.Button openDownloadsBtn;
        private System.Windows.Forms.Button cancelDlButton;
        private System.Windows.Forms.Label dlDirLabel;
        private System.Windows.Forms.TextBox dlDirTxtBox;
        private System.Windows.Forms.Button setDirBtn;
        private System.Windows.Forms.DataGridView downloadsDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn DownloadUrl;
        private System.Windows.Forms.DataGridViewTextBoxColumn Speed;
        private System.Windows.Forms.DataGridViewTextBoxColumn DownloadProgress;
        private System.Windows.Forms.TabPage searchTab;
        private System.Windows.Forms.Panel searchPanel;
        private System.Windows.Forms.TextBox topicTxt;
        private System.Windows.Forms.ComboBox typeDropDown;
        private System.Windows.Forms.Label topicLbl;
        private System.Windows.Forms.Label categoryLbl;
        private System.Windows.Forms.Button searchBtn;
        private System.Windows.Forms.TextBox creatorTxt;
        private System.Windows.Forms.Label queryLbl;
        private System.Windows.Forms.Label creatorLbl;
        private System.Windows.Forms.TextBox searchTxtBox;
        private System.Windows.Forms.Label pubYrLbl;
        private System.Windows.Forms.TextBox yearTxt;
        private System.Windows.Forms.Button zipBtn;
        private System.Windows.Forms.Button downloadButton;
        private System.Windows.Forms.TextBox resultDescription;
        private System.Windows.Forms.PictureBox resultPreview;
        private System.Windows.Forms.DataGridView resultsGrid;
        private System.Windows.Forms.TabControl controlTabs;
        private System.Windows.Forms.Label creatorInfoLbl;
        private System.Windows.Forms.Label topicInfoLbl;
        private System.Windows.Forms.Label publishedInfoLbl;
        private System.Windows.Forms.Button latestBtn;
        private System.Windows.Forms.DataGridViewTextBoxColumn resultName;
        private System.Windows.Forms.DataGridViewTextBoxColumn avgRating;
        private System.Windows.Forms.DataGridViewTextBoxColumn resultSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn Downloads;
        public System.Windows.Forms.CheckBox torrentChk;
        private System.Windows.Forms.Button reviewButton;
        private System.Windows.Forms.TabPage browseTab;
        private System.Windows.Forms.Panel browsePanel;
        private System.Windows.Forms.Label collectionSearchLbl;
        private System.Windows.Forms.TextBox collectionSearchTxt;
        private System.Windows.Forms.Button collectionSearchBtn;
        private System.Windows.Forms.Label collectionsLbl;
        private System.Windows.Forms.ListBox collectionsList;
        private System.Windows.Forms.Button collectionBtn;
        private System.Windows.Forms.Label sortLbl;
        private System.Windows.Forms.ComboBox sortDropDown;
        private System.Windows.Forms.Label browseTypeLbl;
        private System.Windows.Forms.ComboBox browseTypeDropDown;
        private System.Windows.Forms.Button savedBtn;
    }
}

