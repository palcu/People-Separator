using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PipSep.Models;
using Facebook.Web;
using Facebook.Web.Mvc;

namespace PipSep.Controllers
{
	public class FacebookController : Controller
	{
		//Database
		PipSepDb _db = new PipSepDb();

		//
		// GET: /Facebook/

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult LogOn()
		{
			if (FacebookWebContext.Current.IsAuthenticated())
			{
				long id = FacebookWebContext.Current.UserId;
				ConnectedAccounts user = _db.ConnectedAccounts.SingleOrDefault(r => r.FacebookId == id);
				if (user != null)
				{
					FormsAuthentication.SetAuthCookie(user.AccountUserName, true);
					
					return RedirectToAction("Index","Home");
				}
				else
				{
					return RedirectToAction("Register", "Account", new { FacebookId = id, info = 1 });
				}
			}
			else return View();
		}

		public ActionResult Profile()
		{
			if (FacebookWebContext.Current.IsAuthenticated())
			{
				var client = new FacebookWebClient();

				dynamic me = client.Get("me");
				ViewBag.Friends = me;
				ViewBag.ShouldConnect = false;
				return View();
			}
			else
			{
				ViewBag.ShouldConnect = true;
				return View();
			}
		}
	}
}
