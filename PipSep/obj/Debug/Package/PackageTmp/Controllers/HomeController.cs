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

			ViewBag.registrations = bool.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings.Get("AreRegistrationsAllowed"));
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

		[ChildActionOnly]
		public ActionResult GetEmergency()
		{
			if (bool.Parse(WebConfigurationManager.AppSettings.Get("IsEmergency")))
			{
				ViewBag.Text = WebConfigurationManager.AppSettings.Get("EmergencyText");
				return PartialView();
			}
			else
				return new EmptyResult();
		}

		[ChildActionOnly]
		public ActionResult GetNav()
		{
			var pages = db.Pages.Where(r => r.IsMenu == true).ToList();
			return PartialView(pages);
		}
    }
}
