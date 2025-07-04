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
    public class TrainsController : Controller
    {
        //private WareHouseMVCContext context = new WareHouseMVCContext();
        UnitOfWork repo = new UnitOfWork();

        //
        // GET: /Trains/

        public ViewResult Index()
        {
           // return View(context.Trains.Include(train => train.Zone).Include(train => train.Floor).Include(train => train.Warehouse).ToList());

            return View(repo.TrainRepository.AllIncluding(train => train.Zone, train => train.Zone.Floor, train => train.Zone.Floor.Warehouse));
        }

        //
        // GET: /Trains/Details/5

        public ViewResult Details(long id)
        {
           // Train train = context.Trains.Single(x => x.TrainID == id);
            return View(repo.TrainRepository.Find(id));
           // return View(train);
        }






        public ActionResult CreateWithZN(int znid)
        {
          
                ViewBag.zNID = znid;
                Zone zn = new Zone();
                zn = repo.ZoneRepository.Find(znid);
              
           
                ViewBag.fLID = zn.FloorID;
                ViewBag.wHID = zn.WarehouseID;

                ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
                ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
                ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();

            
            return View("Create");
        }




        //
        // GET: /Trains/Create

        public ActionResult Create()
        {
            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
            ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
            return View();
        } 

        //
        // POST: /Trains/Create

        [HttpPost]
        public ActionResult Create(Train train)
        {
            if (ModelState.IsValid)
            {
                repo.TrainRepository.InsertOrUpdate(train);
                repo.TrainRepository.Save();
                ViewBag.NoofZones = repo.ZoneRepository.All.Count();
                return RedirectToAction("Index");  
            }

            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
            ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
            return View(train);
        }
        
        //
        // GET: /Trains/Edit/5
 
        public ActionResult Edit(long id)
        {
           

            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
            ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
            return View(repo.TrainRepository.Find(id));

        }

        //
        // POST: /Trains/Edit/5

        [HttpPost]
        public ActionResult Edit(Train train)
        {
            if (ModelState.IsValid)
            {

                repo.TrainRepository.InsertOrUpdate(train);
                repo.TrainRepository.Save();
                return RedirectToAction("Index");
            }
            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
            ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
            return View(train);
        }

        //
        // GET: /Trains/Delete/5
 
        public ActionResult Delete(long id)
        {

            return View(repo.TrainRepository.Find(id));
        }

        //
        // POST: /Trains/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
           

            repo.TrainRepository.Delete(id);
            repo.TrainRepository.Save();
            return RedirectToAction("Index");
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