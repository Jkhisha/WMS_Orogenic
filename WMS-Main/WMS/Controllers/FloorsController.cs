using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;
using PagedList;

namespace WareHouseMVC.Controllers
{   
    public class FloorsController : Controller
    {
        private UnitOfWork repo = new UnitOfWork();
        //WarehouseRepository wareHouseRepo = new WarehouseRepository();

        //
        // GET: /Floors/

        //public ViewResult Index()
        //{
        //    ViewBag.NoofFloors = repo.FloorRepository.All.Count();
        //    return View(repo.FloorRepository.AllIncluding(floor => floor.Warehouse).ToList());
        //}

        public ViewResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.NoofFloors = repo.FloorRepository.All.Count();
            return View(repo.FloorRepository.AllIncluding(floor => floor.Warehouse).OrderBy(o=>o.FloorName).ToPagedList(pageNumber, pageSize));

            //int pageSize = 3;
            //int pageNumber = (page ?? 1);
            //return View(students.ToPagedList(pageNumber, pageSize));
        
        }

        //
        // GET: /Floors/Details/5

        public ViewResult Details(long id)
        {
            //Floor floor = context.Floors.Single(x => x.FloorID == id);
            return View(repo.FloorRepository.Find(id));
        }

        //
        // GET: /Floors/Create

        public ActionResult Create()
        {
            ViewBag.NoofFloors = repo.FloorRepository.All.Count();
            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            return View();
        }

        public ActionResult CreateWithWH(int wHid)
        {
            ViewBag.whID = wHid;

            ViewBag.NoofFloors = repo.FloorRepository.All.Count();
            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            return View("Create");
        } 

        //
        // POST: /Floors/Create

        [HttpPost]
        public ActionResult Create(Floor floor)
        {
            if (ModelState.IsValid)
            {
                repo.FloorRepository.InsertOrUpdate(floor);
                repo.Save();
                ViewBag.NoofFloors = repo.FloorRepository.All.Count();
                return RedirectToAction("Index");  
            }

            ViewBag.NoofFloors = repo.FloorRepository.All.Count();
            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            return View(floor);
        }
        
        //
        // GET: /Floors/Edit/5
 
        public ActionResult Edit(long id)
        {
            //Floor floor = context.Floors.Single(x => x.FloorID == id);
            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            return View(repo.FloorRepository.Find(id));
        }

        //
        // POST: /Floors/Edit/5

        [HttpPost]
        public ActionResult Edit(Floor floor)
        {
            if (ModelState.IsValid)
            {
                repo.FloorRepository.InsertOrUpdate(floor);
                repo.FloorRepository.Save();
                return RedirectToAction("Index");
            }
            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            return View(floor);
        }

        //
        // GET: /Floors/Delete/5
 
        public ActionResult Delete(long id)
        {
            return View(repo.FloorRepository.Find(id));
        }

        //
        // POST: /Floors/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            repo.FloorRepository.Delete(id);
            repo.FloorRepository.Save();
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