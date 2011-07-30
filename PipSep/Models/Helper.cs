using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PipSep.Models
{
	public class Helper
	{

	}

	public class PairStringList
	{
		public string UserName { get; set; }
		public List<string> Friends = new List<string>();
		public List<string> Pages = new List<string>();
	}
}