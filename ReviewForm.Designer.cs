namespace SoftwareShelf_Desktop
{
    partial class ReviewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReviewForm));
            this.reviewsTxt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // reviewsTxt
            // 
            this.reviewsTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reviewsTxt.BackColor = System.Drawing.SystemColors.Window;
            this.reviewsTxt.Location = new System.Drawing.Point(12, 12);
            this.reviewsTxt.Multiline = true;
            this.reviewsTxt.Name = "reviewsTxt";
            this.reviewsTxt.ReadOnly = true;
            this.reviewsTxt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.reviewsTxt.Size = new System.Drawing.Size(320, 357);
            this.reviewsTxt.TabIndex = 0;
            // 
            // ReviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 381);
            this.Controls.Add(this.reviewsTxt);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(300, 400);
            this.Name = "ReviewForm";
            this.ShowIcon = false;
            this.Text = "Reviews";
            this.Load += new System.EventHandler(this.ReviewForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox reviewsTxt;
    }
}