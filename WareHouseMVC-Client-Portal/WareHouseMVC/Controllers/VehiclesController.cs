using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;

namespace WareHouseMVC.Controllers
{
    public class VehiclesController : BaseController
    {


        //
        // GET: /Vehicles/




		 UnitOfWork repo = new UnitOfWork();


        public ViewResult Index()
        {
            return View(repo.VehicleRepository.AllIncluding(vh=>vh.TransportVendor));
        }

        //
        // GET: /Vehicles/Details/5

        public ViewResult Details(long id)
        {
            return View(repo.VehicleRepository.Find(id));
        }

        //
        // GET: /Vehicles/Create

        public ActionResult Create()
        {
			ViewBag.PossibleTransportVendors = repo.TransportVendorRepository.AllIncluding();
            return View();
        } 

        //
        // POST: /Vehicles/Create

        [HttpPost]
        public ActionResult Create(Vehicle vehicle)
        {
            if (ModelState.IsValid) {

                repo.VehicleRepository.InsertOrUpdate(vehicle);
                repo.VehicleRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleTransportVendors = repo.TransportVendorRepository.AllIncluding();
				return View();
			}
        }
        
        //
        // GET: /Vehicles/Edit/5
 
        public ActionResult Edit(long id)
        {
			ViewBag.PossibleTransportVendors = repo.TransportVendorRepository.AllIncluding();
             return View(repo.VehicleRepository.Find(id));
        }

        //
        // POST: /Vehicles/Edit/5

        [HttpPost]
        public ActionResult Edit(Vehicle vehicle)
        {
            if (ModelState.IsValid) {
                repo.VehicleRepository.InsertOrUpdate(vehicle);
                repo.VehicleRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleTransportVendors = repo.TransportVendorRepository.AllIncluding();
				return View();
			}
        }

        //
        // GET: /Vehicles/Delete/5
 
        public ActionResult Delete(long id)
        {
            return View(repo.VehicleRepository.Find(id));
        }

        //
        // POST: /Vehicles/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            repo.VehicleRepository.Delete(id);
            repo.VehicleRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
          if (disposing)
			 {

			/*	                transportvendorRepository.Dispose();
				                vehicleRepository.Dispose();
				 */
			repo.Dispose();

            }
			
            base.Dispose(disposing);
        }
    }
}

