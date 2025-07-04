using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;

namespace WareHouseMVC.Controllers
{
    public class LoginTrailsController : BaseController
    {

        UnitOfWork repo = new UnitOfWork();


        //
        // GET: /LoginTrails/

        public ViewResult Index()
        {
            List<LoginTrail> allLoginlst = new List<LoginTrail>();
            allLoginlst = repo.LoginTrailRepository.GetAll();
            return View(allLoginlst);
        }

        //
        // GET: /LoginTrails/Details/5

        public ViewResult Details(long id)
        {
            return View(repo.LoginTrailRepository.Find(id));
        }

        //
        // GET: /LoginTrails/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /LoginTrails/Create

        [HttpPost]
        public ActionResult Create(LoginTrail logintrail)
        {
            if (ModelState.IsValid)
            {
                repo.LoginTrailRepository.InsertOrUpdate(logintrail);
                repo.LoginTrailRepository.Save();
                return RedirectToAction("Index");  
            }

            return View(logintrail);
        }
        
        //
        // GET: /LoginTrails/Edit/5
 
        public ActionResult Edit(long id)
        {
            return View(repo.LoginTrailRepository.Find(id));
        }

        //
        // POST: /LoginTrails/Edit/5

        [HttpPost]
        public ActionResult Edit(LoginTrail logintrail)
        {
            if (ModelState.IsValid)
            {

                repo.LoginTrailRepository.InsertOrUpdate(logintrail);
                repo.LoginTrailRepository.Save();
                return RedirectToAction("Index");
            }
            return View(logintrail);
        }

        //
        // GET: /LoginTrails/Delete/5
 
        public ActionResult Delete(long id)
        {
            return View(repo.LoginTrailRepository.Find(id));
        }

        //
        // POST: /LoginTrails/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            repo.LoginTrailRepository.Delete(id);
            repo.LoginTrailRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}