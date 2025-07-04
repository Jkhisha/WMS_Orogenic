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
    public class WarehousesController : BaseController
    {
      //  private WareHouseMVCContext context = new WareHouseMVCContext();
        UnitOfWork repo = new UnitOfWork();
        //
        // GET: /Warehouses/

        public ViewResult Index()
        {
            ViewBag.NoofWarehouse = repo.WarehouseRepository.All.Count();
            return View(repo.WarehouseRepository.AllIncluding());

        }

        //
        // GET: /Warehouses/Details/5

        public ViewResult Details(long id)
        {
            return View(repo.WarehouseRepository.Find(id));
        }

        //
        // GET: /Warehouses/Create

        public ActionResult Create()
        {
            ViewBag.NoofWarehouse = repo.WarehouseRepository.All.Count();
            return View();
        } 

        //
        // POST: /Warehouses/Create

        [HttpPost]
        public ActionResult Create(Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                repo.WarehouseRepository.InsertOrUpdate(warehouse);
                repo.WarehouseRepository.Save();
                ViewBag.NoofWarehouse = repo.WarehouseRepository.All.Count();
                return RedirectToAction("Index");  
            }
            ViewBag.NoofWarehouse = repo.WarehouseRepository.All.Count();
            return View(warehouse);
        }
        
        //
        // GET: /Warehouses/Edit/5
 
        public ActionResult Edit(long id)
        {
            return View(repo.WarehouseRepository.Find(id));
        }

        //
        // POST: /Warehouses/Edit/5

        [HttpPost]
        public ActionResult Edit(Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                repo.WarehouseRepository.InsertOrUpdate(warehouse);
                repo.WarehouseRepository.Save();
                return RedirectToAction("Index");
            }
            return View(warehouse);
        }

        //
        // GET: /Warehouses/Delete/5
 
        public ActionResult Delete(long id)
        {
            return View(repo.WarehouseRepository.Find(id));
        }

        //
        // POST: /Warehouses/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            repo.WarehouseRepository.Delete(id);
            repo.WarehouseRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            //if (disposing) {
            //    context.Dispose();
            //}
            repo.Dispose(); 
            base.Dispose(disposing);

        }
    }
}