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
					thing.Object = item["id"];
					thing.Type = 1;
					thing.UserId = i;
					thing.Gender = db.ApplicationCVs.Single(r => r.UserName == facebookAcceptedPeople[i]).Gender;
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
					thing.Object = item["id"];
					thing.Type = 2;
					thing.UserId = i;
					db.CommonThings.Add(thing);

					//Save for ViewBag

					pair.UserName = facebookAcceptedPeople[i];
					pair.Pages.Add(item["name"]);

				}

				bag.Add(pair);
				db.SaveChanges();
			}

			ViewBag.data = bag;

			return View();
		}

		//
		// GET: /PeopleSeparator/DisplayMatrix
		public ActionResult DisplayMatrix()
		{
			List<CommonThings> things = db.CommonThings.ToList();
			int numberPeople = db.ConnectedAccounts.Count();
			int[,] matrix = new int[numberPeople, numberPeople];
			for (int i = 0; i < numberPeople; ++i)
			{
				for (int j = i + 1; j < numberPeople; ++j)
				{
					List<CommonThings> first = things.Where(r => r.UserId == i).ToList();
					List<CommonThings> second = things.Where(r => r.UserId == j).ToList();
					matrix[i, j] = matrix[j, i] = PipSep.Models.Helper.CountIntersections(first, second);
				}

			}

			ViewBag.matrix = matrix;
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
		// GET: /PeopleSeparator/DisplayData
		[HttpPost]
		public ActionResult DisplayData(FormCollection form)
		{
			//Get form
			bool mixed;
			if (form.GetValue("type").ToString() == "Mixed")
				mixed = true;
			else
				mixed = false;

			List<int> groups = new List<int>();
			for (int i = 0; i < form.Count - 1; ++i)
			{
				string str = form.GetValue("group" + i).AttemptedValue;
				groups.Add(int.Parse(str));
			}
			groups.Sort();

			//THIS IS IT
			List<List<int>> eurika = new List<List<int>>();


			List<CommonThings> things = db.CommonThings.ToList();
			int numberPeople = db.ConnectedAccounts.Count();
			List<PairThingsValue> list = new List<PairThingsValue>();
			//Make a list with tuples
			for (int i = 0; i < numberPeople; ++i)
			{
				for (int j = i + 1; j < numberPeople; ++j)
				{
					List<CommonThings> first = things.Where(r => r.UserId == i).ToList();
					List<CommonThings> second = things.Where(r => r.UserId == j).ToList();
					PairThingsValue pair = new PairThingsValue();
					pair.First = i;
					pair.Second = j;
					pair.Value = PipSep.Models.Helper.CountIntersections(first, second);
					if (first[0].Gender == second[0].Gender)
						pair.CommonGender = true;
					else
						pair.CommonGender = false;
					list.Add(pair);
				}
			}
			while (list.Count != 0)
			{
				//get max
				PairThingsValue max = new PairThingsValue();
				max.Value = 0;
				foreach (var i in list)
				{
					if (max.Value < i.Value && i.CommonGender == true)
					{
						max = i;
					}
				}
				list.Remove(max);

				List<int> myGroup = new List<int>();
				myGroup.Add(max.First);
				myGroup.Add(max.Second);

				if (groups.First() == 2)
				{
					eurika.Add(myGroup);
				}
				else
				{
					while (groups.First() != myGroup.Count)
					{
						List<PairThingsValue> newList = new List<PairThingsValue>();
						foreach(var addedPerson in myGroup){
						}
					}
				}
			}

			return View();

		}
	}
}