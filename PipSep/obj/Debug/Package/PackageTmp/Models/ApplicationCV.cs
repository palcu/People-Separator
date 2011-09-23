using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel;

namespace PipSep.Models
{
	//[Bind(Exclude = "Id,UserName")]
	[Bind(Exclude = "Gender")]
	public class ApplicationCV
	{
		//[ScaffoldColumn(false)]
		public int Id { get; set; }

		//[ScaffoldColumn(false)]
		public string UserName { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[StringLength(1000, ErrorMessage = "Maximum length is 1000 characters.")]
		[DataType(DataType.MultilineText)]
		[DisplayName("Why do you want to take part in this camp ?")]
		public string QuestionWhy { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[StringLength(1000, ErrorMessage = "Maximum length is 1000 characters.")]
		[DataType(DataType.MultilineText)]
		[DisplayName("Why you should be selected to take part at this camp ?")]
		public string QuestionWhyYou { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[StringLength(1000, ErrorMessage = "Maximum length is 1000 characters.")]
		[DataType(DataType.MultilineText)]
		[DisplayName("Write about you... like a CV.")]
		public string QuestionCV { get; set; }

		public bool IsAccepted { get; set; }

		[ScaffoldColumn(false)]
		public string WhoConfirmedIt { get; set; }

		[ScaffoldColumn(false)]
		public bool HasBeenVerified { get; set; }

		[DisplayName("Flag")]
		public bool HasBeenFlagged { get; set; }

		[ScaffoldColumn(false)]
		public bool Gender { get; set; }

	}


}