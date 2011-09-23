using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PipSep.Models;
using Facebook.Web;
using System.Web.Configuration;

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

            //db.Database.ExecuteSqlCommand("DELETE FROM CommonThings");

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
                    //db.SaveChanges();

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
                    //db.SaveChanges();

                    //Save for ViewBag

                    pair.UserName = facebookAcceptedPeople[i];
                    pair.Pages.Add(item["name"]);

                }

                bag.Add(pair);
                db.SaveChanges();
            }

            ViewBag.data = bag;
            db.SaveChanges();

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
        
        //EXCUSE ME
        public ActionResult ExcuseMe()
        {
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

            //The start of algorithm
            List<ApplicationCV> acceptedPeople = db.ApplicationCVs.Where(r => r.IsAccepted == true).ToList();
            List<ConnectedAccounts> facebookAccounts = db.ConnectedAccounts.ToList();

            //Remove unaccepted facebook accounts
            List<ConnectedAccounts> toRemove = new List<ConnectedAccounts>();
            foreach (var i in facebookAccounts)
                if (!acceptedPeople.Select(r => r.UserName).Contains(i.AccountUserName))
                    toRemove.Add(i);
            foreach (var i in toRemove)
                facebookAccounts.Remove(i);

            //Generate list of number of common things
            List<CommonThings> common = new List<CommonThings>();
            List<PairAccountsValue> listValue = new List<PairAccountsValue>();

            for (int i = 0; i < facebookAccounts.Count; i++)
            {
                for (int j = i+1; j < facebookAccounts.Count; j++)
                {
                    string first = facebookAccounts[i].AccountUserName;
                    string second = facebookAccounts[j].AccountUserName;

                    //List<CommonThings> firstCommon = common.Where(r => r.UserName == first).ToList();
                    //List<CommonThings> secondCommon = common.Where(r => r.UserName == second).ToList();

                    List<CommonThings> firstCommon = db.CommonThings.Where(r => r.UserName == first).ToList();
                    List<CommonThings> secondCommon = db.CommonThings.Where(r => r.UserName == second).ToList();

                    int nCommon = PipSep.Models.Helper.CountIntersections(firstCommon, secondCommon);
                    PairAccountsValue pair = new PairAccountsValue();
                    pair.First = acceptedPeople.SingleOrDefault(r => r.UserName == first);
                    pair.Second = acceptedPeople.SingleOrDefault(r => r.UserName == second);
                    pair.Value = nCommon;
                    listValue.Add(pair);
                }
            }

            //Add those 0 fuckers to the list
            for (int i = 0; i < facebookAccounts.Count; i++)
            {
                for (int j = 0; j < acceptedPeople.Count; j++)
                {
                    if (facebookAccounts.Where(r => r.AccountUserName == acceptedPeople[j].UserName).ToList().Count == 0) //test if facebook is in accepted
                    {
                        PairAccountsValue pair = new PairAccountsValue();

                        string first = facebookAccounts[i].AccountUserName;
                        string second = acceptedPeople[j].UserName;

                        pair.First = acceptedPeople.SingleOrDefault(r => r.UserName == first);
                        pair.Second = acceptedPeople[j];
                        pair.Value = 0;
                        listValue.Add(pair);
                    }
                }
            }

            //And and those fuckers with a little common thing
            for (int i = 0; i < acceptedPeople.Count; i++)
            {
                for (int j = i+1; j < acceptedPeople.Count; j++)
                {
                    if (facebookAccounts.Where(r => r.AccountUserName == acceptedPeople[i].UserName).ToList().Count == 0 && facebookAccounts.Where(r => r.AccountUserName == acceptedPeople[j].UserName).ToList().Count == 0) //test if facebook is in accepted
                    {
                        PairAccountsValue pair = new PairAccountsValue();

                        pair.First = acceptedPeople[i];
                        pair.Second = acceptedPeople[j];
                        pair.Value = 5;
                        listValue.Add(pair);
                    }
                }
            }

            List<List<ApplicationCV>> result = new List<List<ApplicationCV>>();

            //sort list
            bool ok = true;
            while (ok)
            {
                ok = false;
                for (int i = 1; i < listValue.Count; i++)
                {
                    if (listValue[i-1].Value < listValue[i].Value)
                    {
                        ok = true;
                        PairAccountsValue aux = listValue[i];
                        listValue[i] = listValue[i-1];
                        listValue[i-1] = aux;
                    }
                }
            }

            if (!mixed)
            {
                ReturnedThing results = MainAlgorithm.GetResults(listValue, groups, acceptedPeople);
                ViewBag.results = results.Groups;
                ViewBag.remained = results.Outside;
            }
            else
            {
                ReturnedThing male = MainAlgorithm.GetResults(listValue.Where(r => r.First.Gender == true && r.Second.Gender == true).ToList(), groups, acceptedPeople);
                int groupsCompleted = male.Groups.Count;
                groups.RemoveRange(0, groupsCompleted);
                ReturnedThing female = MainAlgorithm.GetResults(listValue.Where(r => r.First.Gender == true && r.Second.Gender == true).ToList(), groups, acceptedPeople);
                ViewBag.results = male.Groups.Intersect(female.Groups);
                ViewBag.remained = male.Outside.Intersect(female.Outside);
            }
            
            /*
            //first facebook accounts
            while (listValue.Count != 0 && count++<100)
            {
                PairAccountsValue first = listValue[0];
                //test if same gender

                if ((listValue[0].First.Gender && listValue[0].Second.Gender) == !mixed)
                {
                    List<ApplicationCV> l = new List<ApplicationCV>();
                    l.Add(listValue[0].First);
                    l.Add(listValue[0].Second);
                    acceptedPeople.Remove(listValue[0].First);
                    acceptedPeople.Remove(listValue[0].Second);
                    facebookAccounts.RemoveAll(r => r.AccountUserName == listValue[0].First.UserName);
                    facebookAccounts.RemoveAll(r => r.AccountUserName == listValue[0].Second.UserName);
                }
                    listValue.RemoveAt(0);
            }


            //if there are remaining fb accounts
            if (facebookAccounts.Count != 0)
            {
                List<ApplicationCV> l = new List<ApplicationCV>();
                foreach (var i in facebookAccounts)
                {
                    ApplicationCV cv = acceptedPeople.Single(r => r.UserName == i.AccountUserName);
                    l.Add(cv);
                    acceptedPeople.Remove(cv);
                }
                result.Add(l);
            }

            //if there are remaining accounts
            if (acceptedPeople.Count != 0)
            {
                List<ApplicationCV> l = new List<ApplicationCV>();
                if (acceptedPeople.Count != 0)
                {
                    l.Add(acceptedPeople[0]);
                    acceptedPeople.RemoveAt(0);
                }
                if (acceptedPeople.Count != 0)
                {
                    l.Add(acceptedPeople[0]);
                    acceptedPeople.RemoveAt(0);
                }
                result.Add(l);
            }

            ViewBag.result = result;*/

            /*if (!mixed)
            {
                int cycles=0;
                while (acceptedPeople.Count>0 && cycles++ < 100)
                {
                    List<ApplicationCV> newGroup = new List<ApplicationCV>();
                    PairAccountsValue max = new PairAccountsValue();

                    //Search for maximum
                    while (facebookAccounts.Count != 0)
                    {
                        //remove if different genders and still in list
                        max.Value = 0;
                        foreach (var i in listValue)
                        {
                            if (i.Value > max.Value)
                                max = i;
                        }

                        if (max.First.Gender != max.Second.Gender && acceptedPeople.Contains(max.First) && acceptedPeople.Contains(max.Second))
                        {
                            listValue.Remove(max);
                        }
                        else
                        {
                            acceptedPeople.Remove(max.First);
                            acceptedPeople.Remove(max.Second);
                            facebookAccounts.RemoveAll(r => r.AccountUserName == max.First.UserName);
                            facebookAccounts.RemoveAll(r => r.AccountUserName == max.Second.UserName);
                            break;

                        }
                    }

                    //Add to new group
                    newGroup.Add(max.First);
                    newGroup.Add(max.Second);

                    //Search until group is full
                    while (newGroup.Count != groups[0])
                    {
                        List<AccountCost> list = new List<AccountCost>();
                        int maxCost = 0;
                        ApplicationCV maxCV = new ApplicationCV();
                        foreach (var i in facebookAccounts)
                        {
                            ApplicationCV j = acceptedPeople.SingleOrDefault(r => r.UserName == i.AccountUserName);
                            int cost = 0;
                            foreach (var item in newGroup)
                            {
                                if (j.Gender != item.Gender)
                                    continue;
                                int value = listValue.Single(r => (r.First == item && r.Second == j) || (r.First == j && r.Second == item)).Value;
                                cost += value;
                            }
                            if (cost > maxCost)
                            {
                                maxCost = cost;
                                maxCV = j;
                            }
                        }
                        newGroup.Add(maxCV);
                        facebookAccounts.RemoveAll(r => r.AccountUserName == maxCV.UserName);
                        acceptedPeople.Remove(maxCV);
                    }

                    //If we don't have any fb accounts, add one
                    newGroup.Add(acceptedPeople[0]);
                    acceptedPeople.RemoveAt(0);

                    //If we didn't have enough facebook accounts, complete with other
                    if (newGroup.Count != groups[0])
                    {
                        foreach (var item in acceptedPeople)
                        {
                            if (newGroup.Count == groups[0])
                            {
                                break;
                            }
                            if (item.Gender == newGroup[0].Gender)
                            {
                                newGroup.Add(item);
                                continue;
                            }
                        }
                    }

                    result.Add(newGroup);
                }
            }
            else
            {
            }*/


            /*
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
			}*/

			return View();

		}
	}
}