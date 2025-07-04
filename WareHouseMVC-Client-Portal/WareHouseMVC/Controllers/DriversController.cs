using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;

namespace WareHouseMVC.Controllers
{
    public class DriversController : BaseController
    {


        //
        // GET: /Drivers/




		 UnitOfWork repo = new UnitOfWork();


        public ViewResult Index()
        {
            return View(repo.DriverRepository.AllIncluding(Driver=>Driver.TransportVendor));
        }

        //
        // GET: /Drivers/Details/5

        public ViewResult Details(long id)
        {
            return View(repo.DriverRepository.Find(id));
        }

        //
        // GET: /Drivers/Create

        public ActionResult Create()
        {
			ViewBag.PossibleTransportVendors = repo.TransportVendorRepository.AllIncluding();
            return View();
        } 

        //
        // POST: /Drivers/Create

        [HttpPost]
        public ActionResult Create(Driver driver)
        {
            if (ModelState.IsValid) {

                repo.DriverRepository.InsertOrUpdate(driver);
                repo.DriverRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleTransportVendors = repo.TransportVendorRepository.AllIncluding();
				return View();
			}
        }
        
        //
        // GET: /Drivers/Edit/5
 
        public ActionResult Edit(long id)
        {
			ViewBag.PossibleTransportVendors = repo.TransportVendorRepository.AllIncluding();
             return View(repo.DriverRepository.Find(id));
        }

        //
        // POST: /Drivers/Edit/5

        [HttpPost]
        public ActionResult Edit(Driver driver)
        {
            if (ModelState.IsValid) {
                repo.DriverRepository.InsertOrUpdate(driver);
                repo.DriverRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleTransportVendors = repo.TransportVendorRepository.AllIncluding();
				return View();
			}
        }

        //
        // GET: /Drivers/Delete/5
 
        public ActionResult Delete(long id)
        {
            return View(repo.DriverRepository.Find(id));
        }

        //
        // POST: /Drivers/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            repo.DriverRepository.Delete(id);
            repo.DriverRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
          if (disposing)
			 {

			/*	                transportvendorRepository.Dispose();
				                driverRepository.Dispose();
				 */
			repo.Dispose();

            }
			
            base.Dispose(disposing);
        }
    }
}

