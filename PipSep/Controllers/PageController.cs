using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PipSep.Models;

namespace PipSep.Controllers
{ 
    public class PageController : Controller
    {
        private PipSepDb db = new PipSepDb();

        //
        // GET: /Page/

        public ViewResult Index()
        {
            return View(db.Pages.ToList());
        }

        //
        // GET: /Page/Details/5

        public ViewResult Details(int id)
        {
            Page page = db.Pages.Find(id);
            return View(page);
        }

        //
        // GET: /Page/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Page/Create

        [HttpPost]
		[ValidateInput(false)]
        public ActionResult Create(Page page)
        {
			page = Page.AddDateCreatedAndAuthor(page);

            if (ModelState.IsValid)
            {
                db.Pages.Add(page);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(page);
        }
        
        //
        // GET: /Page/Edit/5
 
        public ActionResult Edit(int id)
        {
            Page page = db.Pages.Find(id);
            return View(page);
        }

        //
        // POST: /Page/Edit/5

        [HttpPost]
		[ValidateInput(false)]
        public ActionResult Edit(Page page)
        {
			page = Page.AddDateCreatedAndAuthor(page);

            if (ModelState.IsValid)
            {
                db.Entry(page).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(page);
        }

        //
        // GET: /Page/Delete/5
 
        public ActionResult Delete(int id)
        {
            Page page = db.Pages.Find(id);
            return View(page);
        }

        //
        // POST: /Page/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Page page = db.Pages.Find(id);
            db.Pages.Remove(page);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

		//
		// GET: /Page/View/5

		public ActionResult View(int id)
		{
			Page page = db.Pages.Find(id);
			return View(page);
		}
    }
}