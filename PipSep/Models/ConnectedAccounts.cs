
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PipSep.Models
{
	public class ConnectedAccounts
	{
		public int Id { get; set; }
		public long FacebookId { get; set; }
		public string AccountUserName { get; set; }
		public string AccesToken { get; set; }
	}
}