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
    public class PalletsController : Controller
    {
        UnitOfWork repo = new UnitOfWork();

        //
        // GET: /Pallets/

        public ViewResult Index()
        {
            return View(repo.PalletRepository.AllIncluding(pallet => pallet.Zone).Include(pallet => pallet.Floor).Include(pallet => pallet.Warehouse).ToList());
        }

        //
        // GET: /Pallets/Details/5

        public ViewResult Details(long id)
        {
            Pallet pallet = repo.PalletRepository.Find(id);
            return View(pallet);
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
        // GET: /Pallets/Create

        public ActionResult Create()
        {
            ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
            ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            return View();
        } 

        //
        // POST: /Pallets/Create

        [HttpPost]
        public ActionResult Create(Pallet pallet)
        {
            if (ModelState.IsValid)
            {
                repo.PalletRepository.InsertOrUpdate(pallet);
                 repo.PalletRepository.Save();
                return RedirectToAction("Index");  
            }

            ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
            ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            return View(pallet);
        }
        
        //
        // GET: /Pallets/Edit/5
 
        public ActionResult Edit(long id)
        {
            Pallet pallet = repo.PalletRepository.Find(id);

            ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
            ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            return View(pallet);
        }

        //
        // POST: /Pallets/Edit/5

        [HttpPost]
        public ActionResult Edit(Pallet pallet)
        {
            if (ModelState.IsValid)
            {
                repo.PalletRepository.InsertOrUpdate(pallet);
                repo.PalletRepository.Save();
                return RedirectToAction("Index");
            }
            ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
            ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            return View(pallet);
        }

        //
        // GET: /Pallets/Delete/5
 
        public ActionResult Delete(long id)
        {
            Pallet pallet = repo.PalletRepository.Find(id);
            return View(pallet);
        }

        //
        // POST: /Pallets/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            Pallet pallet = repo.PalletRepository.Find(id);
            repo.PalletRepository.Delete(id);
            repo.PalletRepository.Save();
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