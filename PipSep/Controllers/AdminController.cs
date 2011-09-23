using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PipSep.Models;
using System.Web.Configuration;
using System.Web.Security;

namespace TechFriendCamp.Controllers
{
	//[Authorize(Roles="Administrator")]
	public class AdminController : Controller
	{
		//Database
		PipSepDb db = new PipSepDb();

		//
		// GET: /Admin/

		public ActionResult Index()
		{
			ViewBag.CVsSent = db.ApplicationCVs.Count();
			ViewBag.CVsApproved = db.ApplicationCVs.Count(r => r.IsAccepted == true);
			return View();
		}

		public ActionResult Emergency()
		{
			ViewBag.IsEmergency = bool.Parse(WebConfigurationManager.AppSettings.Get("IsEmergency"));
			ViewBag.EmergencyText = WebConfigurationManager.AppSettings.Get("EmergencyText");
			return View();
		}

		[HttpPost]
		public ActionResult Emergency(FormCollection form)
		{
			WebConfigurationManager.AppSettings.Set("IsEmergency", form["IsEmergency"].Contains("true").ToString());
			WebConfigurationManager.AppSettings.Set("EmergencyText", form["EmergencyText"]);
			return RedirectToAction("Emergency", "Admin");
		}

		public ActionResult IndexContent()
		{
			ViewBag.AreRegistrationsAllowed = bool.Parse(WebConfigurationManager.AppSettings.Get("AreRegistrationsAllowed"));
			ViewBag.IndexContent = Server.HtmlDecode(WebConfigurationManager.AppSettings.Get("IndexContent"));
			return View();
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult IndexContent(FormCollection form)
		{
			WebConfigurationManager.AppSettings.Set("AreRegistrationsAllowed", form["AreRegistrationsAllowed"].Contains("true").ToString());
			WebConfigurationManager.AppSettings.Set("IndexContent", Server.HtmlDecode(form["IndexContent"]));
			return RedirectToAction("IndexContent", "Admin");
		}
	}
}
