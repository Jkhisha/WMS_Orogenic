using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;

namespace WareHouseMVC.Controllers
{   
    public class CountriesController : Controller
    {
        private WareHouseMVCContext context = new WareHouseMVCContext();

        //
        // GET: /Countries/

        public ViewResult Index()
        {
            return View(context.Countries.ToList());
        }

        //
        // GET: /Countries/Details/5

        public ViewResult Details(long id)
        {
            Country country = context.Countries.Single(x => x.CountryID == id);
            return View(country);
        }

        //
        // GET: /Countries/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Countries/Create

        [HttpPost]
        public ActionResult Create(Country country)
        {
            if (ModelState.IsValid)
            {
                context.Countries.Add(country);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(country);
        }
        
        //
        // GET: /Countries/Edit/5
 
        public ActionResult Edit(long id)
        {
            Country country = context.Countries.Single(x => x.CountryID == id);
            return View(country);
        }

        //
        // POST: /Countries/Edit/5

        [HttpPost]
        public ActionResult Edit(Country country)
        {
            if (ModelState.IsValid)
            {
                context.Entry(country).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(country);
        }

        //
        // GET: /Countries/Delete/5
 
        public ActionResult Delete(long id)
        {
            Country country = context.Countries.Single(x => x.CountryID == id);
            return View(country);
        }

        //
        // POST: /Countries/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            Country country = context.Countries.Single(x => x.CountryID == id);
            context.Countries.Remove(country);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}