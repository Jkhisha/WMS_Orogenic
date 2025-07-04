using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;

namespace WareHouseMVC.Controllers
{   
    public class RegionsController : Controller
    {


        //
        // GET: /Regions/




		 UnitOfWork repo = new UnitOfWork();


        public ViewResult Index()
        {
            return View(repo.RegionRepository.AllIncluding());
        }

        //
        // GET: /Regions/Details/5

        public ViewResult Details(long id)
        {
            return View(repo.RegionRepository.Find(id));
        }

        //
        // GET: /Regions/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Regions/Create

        [HttpPost]
        public ActionResult Create(Region region)
        {
            if (ModelState.IsValid) {

                repo.RegionRepository.InsertOrUpdate(region);
                repo.RegionRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /Regions/Edit/5
 
        public ActionResult Edit(long id)
        {
             return View(repo.RegionRepository.Find(id));
        }

        //
        // POST: /Regions/Edit/5

        [HttpPost]
        public ActionResult Edit(Region region)
        {
            if (ModelState.IsValid) {
                repo.RegionRepository.InsertOrUpdate(region);
                repo.RegionRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /Regions/Delete/5
 
        public ActionResult Delete(long id)
        {
            return View(repo.RegionRepository.Find(id));
        }

        //
        // POST: /Regions/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            repo.RegionRepository.Delete(id);
            repo.RegionRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
          if (disposing)
			 {

			/*	                regionRepository.Dispose();
				 */
			repo.Dispose();

            }
			
            base.Dispose(disposing);
        }
    }
}

