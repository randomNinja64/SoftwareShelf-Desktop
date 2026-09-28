using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SoftwareShelf_Desktop
{
    public partial class ReviewForm : Form
    {
        public string itemIdentifier;
        public ReviewForm()
        {
            InitializeComponent();
        }

        public ReviewForm(string identifier)
        {
            itemIdentifier = identifier;
            InitializeComponent();
        }

        private void ReviewForm_Load(object sender, EventArgs e)
        {
            this.ActiveControl = null; // No control gets focus on load.

            List<Review> reviews = ArchiveHandler.GetReviews(itemIdentifier);

            if (reviews != null)
            {
                foreach (Review review in reviews)
                {

                    reviewsTxt.Text += review.ToString() + Environment.NewLine + Environment.NewLine;
                }
            }
            else
            {
                reviewsTxt.Text = "No reviews found for this item.";
            }

            reviewsTxt.SelectionStart = reviewsTxt.TextLength;
            reviewsTxt.SelectionLength = 0;

        }
    }
}
