namespace SoftwareShelf_Desktop
{
    partial class DownloadForm
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
            this.selectAllBtn = new System.Windows.Forms.Button();
            this.selectNoneBtn = new System.Windows.Forms.Button();
            this.downloadSelectedBtn = new System.Windows.Forms.Button();
            this.filesListBox = new System.Windows.Forms.CheckedListBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.filterTxt = new System.Windows.Forms.TextBox();
            this.filterLbl = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // selectAllBtn
            // 
            this.selectAllBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectAllBtn.Location = new System.Drawing.Point(0, 0);
            this.selectAllBtn.Margin = new System.Windows.Forms.Padding(0);
            this.selectAllBtn.Name = "selectAllBtn";
            this.selectAllBtn.Size = new System.Drawing.Size(153, 23);
            this.selectAllBtn.TabIndex = 0;
            this.selectAllBtn.Text = "Select &All";
            this.selectAllBtn.UseVisualStyleBackColor = true;
            this.selectAllBtn.Click += new System.EventHandler(this.selectAllBtn_Click);
            // 
            // selectNoneBtn
            // 
            this.selectNoneBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectNoneBtn.Location = new System.Drawing.Point(156, 0);
            this.selectNoneBtn.Margin = new System.Windows.Forms.Padding(0);
            this.selectNoneBtn.Name = "selectNoneBtn";
            this.selectNoneBtn.Size = new System.Drawing.Size(154, 23);
            this.selectNoneBtn.TabIndex = 1;
            this.selectNoneBtn.Text = "Select &None";
            this.selectNoneBtn.UseVisualStyleBackColor = true;
            this.selectNoneBtn.Click += new System.EventHandler(this.selectNoneBtn_Click);
            // 
            // downloadSelectedBtn
            // 
            this.downloadSelectedBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.downloadSelectedBtn.Location = new System.Drawing.Point(12, 222);
            this.downloadSelectedBtn.Name = "downloadSelectedBtn";
            this.downloadSelectedBtn.Size = new System.Drawing.Size(310, 23);
            this.downloadSelectedBtn.TabIndex = 4;
            this.downloadSelectedBtn.Text = "&Download Selected";
            this.downloadSelectedBtn.UseVisualStyleBackColor = true;
            this.downloadSelectedBtn.Click += new System.EventHandler(this.downloadSelectedBtn_Click);
            // 
            // filesListBox
            // 
            this.filesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filesListBox.CheckOnClick = true;
            this.filesListBox.FormattingEnabled = true;
            this.filesListBox.IntegralHeight = false;
            this.filesListBox.Location = new System.Drawing.Point(12, 42);
            this.filesListBox.Name = "filesListBox";
            this.filesListBox.ScrollAlwaysVisible = true;
            this.filesListBox.Size = new System.Drawing.Size(310, 151);
            this.filesListBox.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.selectAllBtn, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.selectNoneBtn, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 196);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(310, 23);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // filterTxt
            // 
            this.filterTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filterTxt.Location = new System.Drawing.Point(47, 12);
            this.filterTxt.Name = "filterTxt";
            this.filterTxt.Size = new System.Drawing.Size(275, 20);
            this.filterTxt.TabIndex = 1;
            this.filterTxt.TextChanged += new System.EventHandler(this.filterTxt_TextChanged);
            // 
            // filterLbl
            // 
            this.filterLbl.AutoSize = true;
            this.filterLbl.Location = new System.Drawing.Point(9, 15);
            this.filterLbl.Name = "filterLbl";
            this.filterLbl.Size = new System.Drawing.Size(32, 13);
            this.filterLbl.TabIndex = 0;
            this.filterLbl.Text = "&Filter:";
            // 
            // DownloadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 253);
            this.Controls.Add(this.filterLbl);
            this.Controls.Add(this.filterTxt);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.filesListBox);
            this.Controls.Add(this.downloadSelectedBtn);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(168, 157);
            this.Name = "DownloadForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Download Items";
            this.Load += new System.EventHandler(this.DownloadForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button selectAllBtn;
        private System.Windows.Forms.Button selectNoneBtn;
        private System.Windows.Forms.Button downloadSelectedBtn;
        private System.Windows.Forms.CheckedListBox filesListBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox filterTxt;
        private System.Windows.Forms.Label filterLbl;
    }
}