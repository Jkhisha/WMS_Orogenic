using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;

namespace WareHouseMVC.Controllers
{
    public class HeightsController : BaseController
    {


        //
        // GET: /Heights/




		 UnitOfWork repo = new UnitOfWork();


        public ViewResult Index()
        {
           // return View(repo.HeightRepository.AllIncluding());
            return View(repo.HeightRepository.AllIncluding(height => height.Level, height => height.Level.Rack, height => height.Level.Rack.Train, height => height.Level.Rack.Train.Zone, height => height.Level.Rack.Train.Zone.Floor, height => height.Level.Rack.Train.Zone.Floor.Warehouse));
        }

        //
        // GET: /Heights/Details/5

        public ViewResult Details(long id)
        {
            return View(repo.HeightRepository.Find(id));
        }

        //







        public ActionResult CreateWithLV(int lvid)
        {

                 ViewBag.lVID = lvid;
                 Level lv = new Level();
                 lv = repo.LevelRepository.Find(lvid);

                 ViewBag.rCID = lv.RackID;
                 ViewBag.tRID = lv.TrainID;
                 ViewBag.zNID = lv.ZoneID;
                 ViewBag.fLID = lv.FloorID;
                 ViewBag.wHID = lv.WarehouseID;

                ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
                ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
                ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
                ViewBag.PossibleTrains = repo.TrainRepository.AllIncluding();
                ViewBag.PossibleRacks = repo.RackRepository.AllIncluding();
                ViewBag.PossibleLevels = repo.LevelRepository.AllIncluding();

            
            return View("Create");
        }





        // GET: /Heights/Create

        public ActionResult Create()
        {
			ViewBag.PossibleLevels = repo.LevelRepository.AllIncluding();
			ViewBag.PossibleRacks = repo.RackRepository.AllIncluding();
			ViewBag.PossibleTrains = repo.TrainRepository.AllIncluding();
			ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
			ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
			ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            return View();
        } 

        //
        // POST: /Heights/Create

        [HttpPost]
        public ActionResult Create(Height height)
        {
            if (ModelState.IsValid) {

                repo.HeightRepository.InsertOrUpdate(height);
                repo.HeightRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleLevels = repo.LevelRepository.AllIncluding();
				ViewBag.PossibleRacks = repo.RackRepository.AllIncluding();
				ViewBag.PossibleTrains = repo.TrainRepository.AllIncluding();
				ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
				ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
				ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
				return View();
			}
        }
        
        //
        // GET: /Heights/Edit/5
 
        public ActionResult Edit(long id)
        {
			ViewBag.PossibleLevels = repo.LevelRepository.AllIncluding();
			ViewBag.PossibleRacks = repo.RackRepository.AllIncluding();
			ViewBag.PossibleTrains = repo.TrainRepository.AllIncluding();
			ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
			ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
			ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
             return View(repo.HeightRepository.Find(id));
        }

        //
        // POST: /Heights/Edit/5

        [HttpPost]
        public ActionResult Edit(Height height)
        {
            if (ModelState.IsValid) {
                repo.HeightRepository.InsertOrUpdate(height);
                repo.HeightRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleLevels = repo.LevelRepository.AllIncluding();
				ViewBag.PossibleRacks = repo.RackRepository.AllIncluding();
				ViewBag.PossibleTrains = repo.TrainRepository.AllIncluding();
				ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
				ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
				ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
				return View();
			}
        }

        //
        // GET: /Heights/Delete/5
 
        public ActionResult Delete(long id)
        {
            return View(repo.HeightRepository.Find(id));
        }

        //
        // POST: /Heights/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            repo.HeightRepository.Delete(id);
            repo.HeightRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
          /*  if (disposing)
			 {

				                levelRepository.Dispose();
				                rackRepository.Dispose();
				                trainRepository.Dispose();
				                zoneRepository.Dispose();
				                floorRepository.Dispose();
				                warehouseRepository.Dispose();
				                heightRepository.Dispose();
				            }*/
			repo.Dispose();
            base.Dispose(disposing);
        }
    }
}

