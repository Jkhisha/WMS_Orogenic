using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;

namespace WareHouseMVC.Controllers
{
    public class RacksController : BaseController
    {


        //
        // GET: /Racks/




		 UnitOfWork repo = new UnitOfWork();


        public ViewResult Index()
        {
            return View(repo.RackRepository.AllIncluding(Rack=>Rack.Train,Rack=>Rack.Train.Zone,Rack=>Rack.Train.Zone.Floor,Rack=>Rack.Train.Zone.Floor.Warehouse));
            
        }

        //
        // GET: /Racks/Details/5

        public ViewResult Details(long id)
        {
            return View(repo.RackRepository.Find(id));
        }

        //
        // GET: /Racks/Create

        public ActionResult Create()
        {
			ViewBag.PossibleTrains = repo.TrainRepository.AllIncluding();
            ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
            ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            return View();
        }




        public ActionResult CreateWithTR(int trid)
        {

                  ViewBag.tRID = trid;
                  Train tr = new Train();
                  tr = repo.TrainRepository.Find(trid);


                  ViewBag.zNID = tr.ZoneID;
                  ViewBag.fLID = tr.FloorID;
                  ViewBag.wHID = tr.WarehouseID;

                ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
                ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
                ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
                ViewBag.PossibleTrains = repo.TrainRepository.AllIncluding();

            
            return View("Create");
        }




        //
        // POST: /Racks/Create

        [HttpPost]
        public ActionResult Create(Rack rack)
        {
            if (ModelState.IsValid) {

                repo.RackRepository.InsertOrUpdate(rack);
                repo.RackRepository.Save();
                return RedirectToAction("Index");
            } 
            else {
                ViewBag.PossibleTrains = repo.TrainRepository.AllIncluding();
                ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
                ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
                ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
				return View();
			}
        }
        
        //
        // GET: /Racks/Edit/5
 
        public ActionResult Edit(long id)
        {
            ViewBag.PossibleTrains = repo.TrainRepository.AllIncluding();
            ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
            ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
             return View(repo.RackRepository.Find(id));
        }

        //
        // POST: /Racks/Edit/5

        [HttpPost]
        public ActionResult Edit(Rack rack)
        {
            if (ModelState.IsValid) {
                repo.RackRepository.InsertOrUpdate(rack);
                repo.RackRepository.Save();
                return RedirectToAction("Index");
            } else {
                ViewBag.PossibleTrains = repo.TrainRepository.AllIncluding();
                ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
                ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
                ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
				return View();
			}
        }

        //
        // GET: /Racks/Delete/5
 
        public ActionResult Delete(long id)
        {
            return View(repo.RackRepository.Find(id));
        }

        //
        // POST: /Racks/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            repo.RackRepository.Delete(id);
            repo.RackRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
          /*  if (disposing)
			 {

				                trainRepository.Dispose();
				                zoneRepository.Dispose();
				                floorRepository.Dispose();
				                warehouseRepository.Dispose();
				                rackRepository.Dispose();
				            }*/
			repo.Dispose();
            base.Dispose(disposing);
        }
    }
}

