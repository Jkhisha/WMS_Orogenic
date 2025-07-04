using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;

namespace WareHouseMVC.Controllers
{
    public class TransportVendorsController : BaseController
    {


        //
        // GET: /TransportVendors/




		 UnitOfWork repo = new UnitOfWork();


        public ViewResult Index()
        {
            return View(repo.TransportVendorRepository.AllIncluding(tv=>tv.Vehicles).ToList());
        }

        //
        // GET: /TransportVendors/Details/5

        public ViewResult Details(long id)
        {
            return View(repo.TransportVendorRepository.Find(id));
        }

        //
        // GET: /TransportVendors/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /TransportVendors/Create

        [HttpPost]
        public ActionResult Create(TransportVendor transportvendor)
        {
            if (ModelState.IsValid) {

                repo.TransportVendorRepository.InsertOrUpdate(transportvendor);
                repo.TransportVendorRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /TransportVendors/Edit/5
 
        public ActionResult Edit(long id)
        {
             return View(repo.TransportVendorRepository.Find(id));
        }

        //
        // POST: /TransportVendors/Edit/5

        [HttpPost]
        public ActionResult Edit(TransportVendor transportvendor)
        {
            if (ModelState.IsValid) {
                repo.TransportVendorRepository.InsertOrUpdate(transportvendor);
                repo.TransportVendorRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /TransportVendors/Delete/5
 
        public ActionResult Delete(long id)
        {
            return View(repo.TransportVendorRepository.Find(id));
        }

        //
        // POST: /TransportVendors/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            repo.TransportVendorRepository.Delete(id);
            repo.TransportVendorRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
          if (disposing)
			 {

			/*	                transportvendorRepository.Dispose();
				 */
			repo.Dispose();

            }
			
            base.Dispose(disposing);
        }
    }
}

