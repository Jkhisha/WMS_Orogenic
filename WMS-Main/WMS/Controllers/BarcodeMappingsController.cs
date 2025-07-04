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
    public class BarcodeMappingsController : Controller
    {
        UnitOfWork repo = new UnitOfWork();

        //
        // GET: /BarcodeMappings/

        public ViewResult Index()
        {
            return View(repo.BarcodeMappingRepository.AllIncluding());
        }

        //
        // GET: /BarcodeMappings/Details/5

        public ViewResult Details(long id)
        {
            BarcodeMapping barcodemapping = repo.BarcodeMappingRepository.Find(id);// context.BarcodeMappings.Single(x => x.BarcodeMappingId == id);
            return View(barcodemapping);
        }

        //
        // GET: /BarcodeMappings/Create

        public ActionResult Create()
        {
            ViewBag.PossibleBarCodes = repo.BarCodeRepository.BarCodeList();// context.BarCodes;
            ViewBag.PossibleItems = repo.ItemRepository.AllIncluding();// context.Items;
            return View();
        } 

        //
        // POST: /BarcodeMappings/Create

        [HttpPost]
        public ActionResult Create(BarcodeMapping barcodemapping)
        {
            if (ModelState.IsValid)
            {
                repo.BarcodeMappingRepository.InsertOrUpdate(barcodemapping);// context.BarcodeMappings.Add(barcodemapping);
                repo.BarcodeMappingRepository.Save();// context.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.PossibleBarCodes = repo.BarCodeRepository.BarCodeList();// context.BarCodes;
            ViewBag.PossibleItems = repo.ItemRepository.AllIncluding();// context.Items;
            return View(barcodemapping);
        }
        
        //
        // GET: /BarcodeMappings/Edit/5
 
        public ActionResult Edit(long id)
        {
            BarcodeMapping barcodemapping = repo.BarcodeMappingRepository.Find(id);// context.BarcodeMappings.Single(x => x.BarcodeMappingId == id);
            ViewBag.PossibleBarCodes = repo.BarCodeRepository.BarCodeList();// context.BarCodes;
            ViewBag.PossibleItems = repo.ItemRepository.AllIncluding();// context.Items;
            return View(barcodemapping);
        }

        //
        // POST: /BarcodeMappings/Edit/5

        [HttpPost]
        public ActionResult Edit(BarcodeMapping barcodemapping)
        {
            if (ModelState.IsValid)
            {
                repo.BarcodeMappingRepository.InsertOrUpdate(barcodemapping);// context.Entry(barcodemapping).State = EntityState.Modified;
                repo.BarcodeMappingRepository.Save();// context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PossibleBarCodes = repo.BarCodeRepository.BarCodeList();// context.BarCodes;
            ViewBag.PossibleItems = repo.ItemRepository.AllIncluding();// context.Items;
            return View(barcodemapping);
        }
        
        //
        // GET: /BarcodeMappings/Delete/5
 
        public ActionResult Delete(long id)
        {
            BarcodeMapping barcodemapping = repo.BarcodeMappingRepository.Find(id);// context.BarcodeMappings.Single(x => x.BarcodeMappingId == id);
            return View(barcodemapping);
        }

        //
        // POST: /BarcodeMappings/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            BarcodeMapping barcodemapping = repo.BarcodeMappingRepository.Find(id);// context.BarcodeMappings.Single(x => x.BarcodeMappingId == id);
            repo.BarcodeMappingRepository.Delete(id);// context.BarcodeMappings.Remove(barcodemapping);
            repo.BarcodeMappingRepository.Save();// context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                repo.BarcodeMappingRepository.Dispose();// context.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ClientToOrogenicBarcodeView()
        {
            // Return empty view (15 rows will be generated client-side)
            return View();
        }
        private WareHouseMVCContext db = new WareHouseMVCContext();

        [HttpPost]
        public JsonResult GetOrogenicBarcode(string clientBarcode)
        {
            if (string.IsNullOrEmpty(clientBarcode))
                return Json(new { success = false, message = "Empty barcode." });

            var mapping = db.ClientBarCodeMaps
                .FirstOrDefault(x => x.ClientBarCodeText == clientBarcode);

            return Json(new
            {
                success = true,
                orogenicBarcode = mapping?.OrogenicBarCodeText
            });
        }
    }
}