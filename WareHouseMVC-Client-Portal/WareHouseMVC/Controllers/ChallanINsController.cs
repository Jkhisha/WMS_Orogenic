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
    public class ChallanINsController : BaseController
    {
        UnitOfWork repo = new UnitOfWork();

        //
        // GET: /ChallanINs/

        public ViewResult Index()
        {
            //return View(repo.ChallanINRepository.AllIncluding(challanin => challanin.Driver).Include(challanin => challanin.Vehicle).Include(challanin => challanin.TransportVendor).Include(challanin => challanin.TransmittalINs).ToList());
            return View();
        }

        //
        // GET: /ChallanINs/Details/5

        public ViewResult Details(long id)
        {
            ChallanIN challanin = repo.ChallanINRepository.Find(id);
            return View(challanin);
        }

        //
        // GET: /ChallanINs/Create

        public ActionResult Create()
        {
            ViewBag.PossibleTransmittalINs = repo.TransmittalINRepository.AllIncluding();
            ViewBag.PossibleDrivers = repo.DriverRepository.AllIncluding();
            ViewBag.PossibleVehicles = repo.VehicleRepository.AllIncluding() ;
            ViewBag.PossibleTransportVendors = repo.TransportVendorRepository.AllIncluding();
            return View();
        } 

        //
        // POST: /ChallanINs/Create

        [HttpPost]
        public ActionResult Create(ChallanIN challanin)
        {
            if (ModelState.IsValid)
            {
                repo.ChallanINRepository.InsertOrUpdate(challanin);
                repo.ChallanINRepository.Save();
                return RedirectToAction("Index");  
            }

            ViewBag.PossibleTransmittalINs = repo.TransmittalINRepository.AllIncluding();
            ViewBag.PossibleDrivers = repo.DriverRepository.AllIncluding();
            ViewBag.PossibleVehicles = repo.VehicleRepository.AllIncluding();
            ViewBag.PossibleTransportVendors = repo.TransportVendorRepository.AllIncluding();
            return View(challanin);
        }
        
        //
        // GET: /ChallanINs/Edit/5
 
        public ActionResult Edit(long id)
        {
            ChallanIN challanin = repo.ChallanINRepository.Find(id);
            ViewBag.PossibleTransmittalINs = repo.TransmittalINRepository.AllIncluding();
            ViewBag.PossibleDrivers = repo.DriverRepository.AllIncluding();
            ViewBag.PossibleVehicles = repo.VehicleRepository.AllIncluding();
            ViewBag.PossibleTransportVendors = repo.TransportVendorRepository.AllIncluding();
            return View(challanin);
        }

        //
        // POST: /ChallanINs/Edit/5

        [HttpPost]
        public ActionResult Edit(ChallanIN challanin)
        {
            if (ModelState.IsValid)
            {
                repo.ChallanINRepository.InsertOrUpdate(challanin);
                repo.ChallanINRepository.Save();
                return RedirectToAction("Index");
            }
            ViewBag.PossibleTransmittalINs = repo.TransmittalINRepository.AllIncluding();
            ViewBag.PossibleDrivers = repo.DriverRepository.AllIncluding();
            ViewBag.PossibleVehicles = repo.VehicleRepository.AllIncluding();
            ViewBag.PossibleTransportVendors = repo.TransportVendorRepository.AllIncluding();
            return View(challanin);
        }

        //
        // GET: /ChallanINs/Delete/5
 
        public ActionResult Delete(long id)
        {
            ChallanIN challanin = repo.ChallanINRepository.Find(id);
            return View(challanin);
        }

        //
        // POST: /ChallanINs/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            ChallanIN challanin = repo.ChallanINRepository.Find(id);
            repo.ChallanINRepository.InsertOrUpdate(challanin);
            repo.ChallanINRepository.Save();
            return RedirectToAction("Index");
        }



        public ActionResult AssignDriver(long trID, long wID)
        {

            return View();

        }





        protected override void Dispose(bool disposing)
        {
            //if (disposing) {
            //    context.Dispose();
            //}
            base.Dispose(disposing);
        }
    }
}