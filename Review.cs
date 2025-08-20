using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftwareShelf_Desktop
{
    class Review
    {
        public string Title { get; set; }
        public string Stars { get; set; }
        public string Body { get; set; }
        public string Reviewer { get; set; }

        public override string ToString()
        {
            return $"{Title} - {Stars} Stars - {Reviewer}{Environment.NewLine}{Body}";
        }
    }
}
