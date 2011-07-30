using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Configuration;
using Facebook.Web;
using PipSep.Models;

namespace PipSep.Controllers
{
    public class HomeController : Controller
    {
		//Database
		PipSepDb db = new PipSepDb();

        public ActionResult Index()
        {
			ViewBag.Content = WebConfigurationManager.AppSettings.Get("IndexContent");

			var loggedUser = db.ApplicationCVs.SingleOrDefault(r => r.UserName == HttpContext.User.Identity.Name);
			if (loggedUser == null)
				ViewBag.CV = true;

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

		public ActionResult Profile()
		{
			if (FacebookWebContext.Current.IsAuthenticated())
			{
				var client = new FacebookWebClient();

				dynamic me = client.Get("me");
				ViewBag.Friends = me;
				ViewBag.ShouldConnect = false;
			}
			else
			{
				ViewBag.ShouldConnect = true;
			}
			return View();
		}
    }
}
