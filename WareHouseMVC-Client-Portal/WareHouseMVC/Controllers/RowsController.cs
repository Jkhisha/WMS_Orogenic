using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;

namespace WareHouseMVC.Controllers
{
    public class RowsController : BaseController
    {


        //
        // GET: /Rows/




		 UnitOfWork repo = new UnitOfWork();


        public ViewResult Index()
        {
          
            return View(repo.RowRepository.AllIncluding(row => row.Column, row => row.Column.Height, row => row.Column.Height.Level, row => row.Column.Height.Level.Rack, row => row.Column.Height.Level.Rack.Train, row => row.Column.Height.Level.Rack.Train.Zone, row => row.Column.Height.Level.Rack.Train.Zone.Floor, row => row.Column.Height.Level.Rack.Train.Zone.Floor.Warehouse));
        }

        //
        // GET: /Rows/Details/5

        public ViewResult Details(long id)
        {
            return View(repo.RowRepository.Find(id));
        }

        //


        public ActionResult CreateWithColumn(int clid)
        {

            ViewBag.cLID = clid;
            Column cl = new Column();
            cl = repo.ColumnRepository.Find(clid);

            ViewBag.hGID = cl.HeightID;
            ViewBag.lVID = cl.LevelID;
            ViewBag.rCID = cl.RackID;
            ViewBag.tRID = cl.TrainID;
            ViewBag.zNID = cl.ZoneID;
            ViewBag.fLID = cl.FloorID;
            ViewBag.wHID = cl.WarehouseID;

            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
            ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
            ViewBag.PossibleTrains = repo.TrainRepository.AllIncluding();
            ViewBag.PossibleRacks = repo.RackRepository.AllIncluding();
            ViewBag.PossibleLevels = repo.LevelRepository.AllIncluding();
            ViewBag.PossibleHeights = repo.HeightRepository.AllIncluding();
            ViewBag.PossibleHeights = repo.HeightRepository.AllIncluding();
            ViewBag.PossibleColumns = repo.ColumnRepository.AllIncluding();


            return View("Create");
        }





        // GET: /Rows/Create

        public ActionResult Create()
        {
			ViewBag.PossibleColumns = repo.ColumnRepository.AllIncluding();
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
        // POST: /Rows/Create

        [HttpPost]
        public ActionResult Create(Row row)
        {
            if (ModelState.IsValid) {

                repo.RowRepository.InsertOrUpdate(row);
                repo.RowRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleColumns = repo.ColumnRepository.AllIncluding();
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
        // GET: /Rows/Edit/5
 
        public ActionResult Edit(long id)
        {
			ViewBag.PossibleColumns = repo.ColumnRepository.AllIncluding();
			ViewBag.PossibleHeights = repo.HeightRepository.AllIncluding();
			ViewBag.PossibleLevels = repo.LevelRepository.AllIncluding();
			ViewBag.PossibleRacks = repo.RackRepository.AllIncluding();
			ViewBag.PossibleTrains = repo.TrainRepository.AllIncluding();
			ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
			ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
			ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
             return View(repo.RowRepository.Find(id));
        }

        //
        // POST: /Rows/Edit/5

        [HttpPost]
        public ActionResult Edit(Row row)
        {
            if (ModelState.IsValid) {
                repo.RowRepository.InsertOrUpdate(row);
                repo.RowRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleColumns = repo.ColumnRepository.AllIncluding();
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
        // GET: /Rows/Delete/5
 
        public ActionResult Delete(long id)
        {
            return View(repo.RowRepository.Find(id));
        }

        //
        // POST: /Rows/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            repo.RowRepository.Delete(id);
            repo.RowRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
          /*  if (disposing)
			 {

				                columnRepository.Dispose();
				                heightRepository.Dispose();
				                levelRepository.Dispose();
				                rackRepository.Dispose();
				                trainRepository.Dispose();
				                zoneRepository.Dispose();
				                floorRepository.Dispose();
				                warehouseRepository.Dispose();
				                rowRepository.Dispose();
				            }*/
			//repo.Dispose();
            base.Dispose(disposing);
        }
    }
}

