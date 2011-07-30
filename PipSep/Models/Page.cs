using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Security;

namespace PipSep.Models
{
    [Bind(Exclude = "DateCreated,Author")]
    public class Page
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        [StringLength(16, ErrorMessage = "The title must have maximum 16 characters")]
        public string Title { get; set; }

        private string encodedContent { get; set; }

        [DataType(DataType.MultilineText)]
        public string Content
        {
            get
            {
                return HttpUtility.HtmlDecode(encodedContent);
            }
            set
            {
                encodedContent = value;
            }
        }

        [DisplayName("Appears in menu")]
        public bool IsMenu { get; set; }

        [DisplayName("Appears on front page blog")]
        public bool IsBlog { get; set; }

        [DisplayName("Page has comments available")]
        public bool IsComment { get; set; }

        [ScaffoldColumn(false)]
        public DateTime DateCreated { get; set; }

		[ScaffoldColumn(false)]
		public string Author { get; set; }

        [DisplayName("Displays first in blog")]
        public bool IsSticky { get; set; }

		public static Page AddDateCreatedAndAuthor(Page page)
		{
			page.DateCreated = DateTime.Now;
			if (HttpContext.Current.User.Identity.IsAuthenticated)
				page.Author = Membership.GetUser().UserName;
			else
				page.Author = "NoAuthor";
			return page;
		}
    }
}