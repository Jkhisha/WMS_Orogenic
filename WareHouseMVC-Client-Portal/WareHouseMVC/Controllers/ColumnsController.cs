using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;

namespace WareHouseMVC.Controllers
{
    public class ColumnsController : BaseController
    {


        //
        // GET: /Columns/




		 UnitOfWork repo = new UnitOfWork();


        public ViewResult Index()
        {
           

            return View(repo.ColumnRepository.AllIncluding(column => column.Height, column => column.Height.Level, column => column.Height.Level.Rack, column => column.Height.Level.Rack.Train, column => column.Height.Level.Rack.Train.Zone, column => column.Height.Level.Rack.Train.Zone.Floor, column => column.Height.Level.Rack.Train.Zone.Floor.Warehouse));
        }

        //
        // GET: /Columns/Details/5

        public ViewResult Details(long id)
        {
            return View(repo.ColumnRepository.Find(id));
        }

        //
        // GET: /Columns/Create

        public ActionResult Create()
        {
			ViewBag.PossibleHeights = repo.HeightRepository.AllIncluding();
			ViewBag.PossibleLevels = repo.LevelRepository.AllIncluding();
			ViewBag.PossibleRacks = repo.RackRepository.AllIncluding();
			ViewBag.PossibleTrains = repo.TrainRepository.AllIncluding();
			ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
			ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
			ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            return View();
        } 

        //
        // POST: /Columns/Create

        [HttpPost]
        public ActionResult Create(Column column)
        {
            if (ModelState.IsValid) {

                repo.ColumnRepository.InsertOrUpdate(column);
                repo.ColumnRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleHeights = repo.HeightRepository.AllIncluding();
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
        // GET: /Columns/Edit/5
 
        public ActionResult Edit(long id)
        {
			ViewBag.PossibleHeights = repo.HeightRepository.AllIncluding();
			ViewBag.PossibleLevels = repo.LevelRepository.AllIncluding();
			ViewBag.PossibleRacks = repo.RackRepository.AllIncluding();
			ViewBag.PossibleTrains = repo.TrainRepository.AllIncluding();
			ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
			ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
			ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
             return View(repo.ColumnRepository.Find(id));
        }

        //
        // POST: /Columns/Edit/5

        [HttpPost]
        public ActionResult Edit(Column column)
        {
            if (ModelState.IsValid) {
                repo.ColumnRepository.InsertOrUpdate(column);
                repo.ColumnRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleHeights = repo.HeightRepository.AllIncluding();
				ViewBag.PossibleLevels = repo.LevelRepository.AllIncluding();
				ViewBag.PossibleRacks = repo.RackRepository.AllIncluding();
				ViewBag.PossibleTrains = repo.TrainRepository.AllIncluding();
				ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
				ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
				ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
				return View();
			}
        }


        public ActionResult CreateWithHG(int hgid)
        {

            ViewBag.hGID = hgid;
            Height hg = new Height();
            hg = repo.HeightRepository.Find(hgid);

            ViewBag.lVID = hg.LevelID;
            ViewBag.rCID = hg.RackID;
            ViewBag.tRID = hg.TrainID;
            ViewBag.zNID = hg.ZoneID;
            ViewBag.fLID = hg.FloorID;
            ViewBag.wHID = hg.WarehouseID;

            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
            ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
            ViewBag.PossibleTrains = repo.TrainRepository.AllIncluding();
            ViewBag.PossibleRacks = repo.RackRepository.AllIncluding();
            ViewBag.PossibleLevels = repo.LevelRepository.AllIncluding();
            ViewBag.PossibleHeights = repo.HeightRepository.AllIncluding();


            return View("Create");
        }


        //
        // GET: /Columns/Delete/5
 
        public ActionResult Delete(long id)
        {
            return View(repo.ColumnRepository.Find(id));
        }

        //
        // POST: /Columns/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            repo.ColumnRepository.Delete(id);
            repo.ColumnRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
          /*  if (disposing)
			 {

				                heightRepository.Dispose();
				                levelRepository.Dispose();
				                rackRepository.Dispose();
				                trainRepository.Dispose();
				                zoneRepository.Dispose();
				                floorRepository.Dispose();
				                warehouseRepository.Dispose();
				                columnRepository.Dispose();
				            }*/
			//repo.Dispose();
            base.Dispose(disposing);
        }
    }
}

