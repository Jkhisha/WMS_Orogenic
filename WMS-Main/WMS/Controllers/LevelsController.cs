using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;
using PagedList;

namespace WareHouseMVC.Controllers
{   
    public class LevelsController : Controller
    {


        //
        // GET: /Levels/




		 UnitOfWork repo = new UnitOfWork();

         public ViewResult Index(int? page)
         {

             int pageSize = 15;
             int pageNumber = (page ?? 1);
             ViewBag.NoofLevel = repo.LevelRepository.All.Count();
             return View(repo.LevelRepository.AllIncluding(level => level.Rack, level => level.Rack.Train, level => level.Rack.Train.Zone, level => level.Rack.Train.Zone.Floor, level => level.Rack.Train.Zone.Floor.Warehouse).OrderBy(o => o.LevelName).ToPagedList(pageNumber, pageSize));

         }

        //public ViewResult Index()
        //{
        //    return View(repo.LevelRepository.AllIncluding(level => level.Rack, level => level.Rack.Train, level => level.Rack.Train.Zone, level => level.Rack.Train.Zone.Floor, level => level.Rack.Train.Zone.Floor.Warehouse));
        //}

        //
        // GET: /Levels/Details/5

        public ViewResult Details(long id)
        {
            return View(repo.LevelRepository.Find(id));
        }

        //
        // GET: /Levels/Create





        public ActionResult CreateWithRC(int rcid)
        {

                  ViewBag.rCID = rcid;
                  Rack rc = new Rack();
                  rc = repo.RackRepository.Find(rcid);

                  ViewBag.tRID = rc.TrainID;
                  ViewBag.zNID = rc.ZoneID;
                  ViewBag.fLID = rc.FloorID;
                  ViewBag.wHID = rc.WarehouseID;

                ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
                ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
                ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
                ViewBag.PossibleTrains = repo.TrainRepository.AllIncluding();
                ViewBag.PossibleRacks = repo.RackRepository.AllIncluding();


            
            return View("Create");
        }




        public ActionResult Create()
        {
			ViewBag.PossibleRacks = repo.RackRepository.AllIncluding();
			ViewBag.PossibleTrains = repo.TrainRepository.AllIncluding();
			ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
			ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
			ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
           
            return View();
        } 

        //
        // POST: /Levels/Create

        [HttpPost]
        public ActionResult Create(Level level)
        {
            if (ModelState.IsValid) {

                repo.LevelRepository.InsertOrUpdate(level);
                repo.LevelRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleRacks = repo.RackRepository.AllIncluding();
				ViewBag.PossibleTrains = repo.TrainRepository.AllIncluding();
				ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
				ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
				ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
				return View();
			}
        }
        
        //
        // GET: /Levels/Edit/5
 
        public ActionResult Edit(long id)
        {
			ViewBag.PossibleRacks = repo.RackRepository.AllIncluding();
			ViewBag.PossibleTrains = repo.TrainRepository.AllIncluding();
			ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
			ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
			ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
             return View(repo.LevelRepository.Find(id));
        }

        //
        // POST: /Levels/Edit/5

        [HttpPost]
        public ActionResult Edit(Level level)
        {
            if (ModelState.IsValid) {
                repo.LevelRepository.InsertOrUpdate(level);
                repo.LevelRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleRacks = repo.RackRepository.AllIncluding();
				ViewBag.PossibleTrains = repo.TrainRepository.AllIncluding();
				ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
				ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
				ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
				return View();
			}
        }

        //
        // GET: /Levels/Delete/5
 
        public ActionResult Delete(long id)
        {
            return View(repo.LevelRepository.Find(id));
        }

        //
        // POST: /Levels/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            repo.LevelRepository.Delete(id);
            repo.LevelRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
          /*  if (disposing)
			 {

				                rackRepository.Dispose();
				                trainRepository.Dispose();
				                zoneRepository.Dispose();
				                floorRepository.Dispose();
				                warehouseRepository.Dispose();
				                levelRepository.Dispose();
				            }*/
			
            base.Dispose(disposing);
        }
    }
}

