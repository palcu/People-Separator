using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PipSep.Models;
using Facebook.Web;

namespace PipSep.Controllers
{
    public class PeopleSeparatorController : Controller
    {
		//Database
		PipSepDb db = new PipSepDb();

        //
        // GET: /PeopleSeparator/

        public ActionResult Index()
        {
			//People with no facebook
			List<string> acceptedPeople = db.ApplicationCVs.Where(r => r.IsAccepted == true).Select(r => r.UserName).ToList();
			List<string> facebookPeople = db.ConnectedAccounts.Select(r => r.AccountUserName).ToList();

			List<string> facebookAcceptedPeople = new List<string>();
			foreach (var item in facebookPeople)
			{
				if (acceptedPeople.Contains(item))
					facebookAcceptedPeople.Add(item);
			}

			List<string> noFacebook = new List<string>();
			foreach (var item in acceptedPeople)
			{
				if (!facebookAcceptedPeople.Contains(item))
					noFacebook.Add(item);
			}

			ViewBag.Facebook = facebookAcceptedPeople;
			ViewBag.NoFacebook = noFacebook;
			ViewBag.FacebookCount = facebookAcceptedPeople.Count;
			ViewBag.NoFacebookCount = noFacebook.Count;
			ViewBag.Total = ViewBag.FacebookCount + ViewBag.NoFacebookCount;

			return View();
		}

		//
		// GET: /PeopleSeparator/GetData
		public ActionResult GetData()
		{
			//First find people
			List<string> acceptedPeople = db.ApplicationCVs.Where(r => r.IsAccepted == true).Select(r => r.UserName).ToList();
			List<string> facebookPeople = db.ConnectedAccounts.Select(r => r.AccountUserName).ToList();

			List<string> facebookAcceptedPeople = new List<string>();
			foreach (var item in facebookPeople)
			{
				if (acceptedPeople.Contains(item))
					facebookAcceptedPeople.Add(item);
			}

			List<string> noFacebook = new List<string>();
			foreach (var item in acceptedPeople)
			{
				if (!facebookAcceptedPeople.Contains(item))
					noFacebook.Add(item);
			}

			List<PairStringList> bag = new List<PairStringList>();
			//Get those stinky things

			db.Database.ExecuteSqlCommand("DELETE FROM CommonThings");

			for (int i = 0; i < facebookAcceptedPeople.Count; i++)
			{
				string userName = facebookAcceptedPeople[i];
				ConnectedAccounts account = db.ConnectedAccounts.Single(r => r.AccountUserName == userName);
				var client = new FacebookWebClient(account.AccesToken);
				dynamic friends = client.Get("me/friends");
				dynamic pages = client.Get("me/likes");

				PairStringList pair = new PairStringList();

				foreach (var item in friends["data"])
				{
					//Save in Table
					CommonThings thing = new CommonThings();
					thing.UserName = facebookAcceptedPeople[i];
					thing.Object = item["name"];
					thing.Type = 1;
					db.CommonThings.Add(thing);

					//Save for ViewBag
					
					pair.UserName = facebookAcceptedPeople[i];
					pair.Friends.Add(item["name"]);
					
				}

				foreach (var item in pages["data"])
				{
					//Save in Table
					CommonThings thing = new CommonThings();
					thing.UserName = facebookAcceptedPeople[i];
					thing.Object = item["name"];
					thing.Type = 2;
					db.CommonThings.Add(thing);

					//Save for ViewBag
					
					pair.UserName = facebookAcceptedPeople[i];
					pair.Pages.Add(item["name"]);
					
				}

				bag.Add(pair);
			}

			ViewBag.data = bag;

			return View();
		}

		//
		// GET: /PeopleSeparator/Settings
		public ActionResult Settings()
		{
			List<string> acceptedPeople = db.ApplicationCVs.Where(r => r.IsAccepted == true).Select(r => r.UserName).ToList();
			ViewBag.Total = acceptedPeople.Count;

			return View();
		}

		//
		// GET: /PeopleSeparator/Settings
		/*[HttpPost]
		public ActionResult Settings()
		{
			return View();
		}*/
    }
}
