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
    public class TransmittalOUTAuditTrailsController : Controller
    {
        UnitOfWork repo = new UnitOfWork();

        //
        // GET: /TransmittalOUTAuditTrails/

        public ViewResult Index()
        {
            return View(repo.TransmittalOUTAuditTrailRepository.AllIncluding().OrderByDescending(o=>o.CreateDate));
        }

        //
        // GET: /TransmittalOUTAuditTrails/Details/5

        public ViewResult Details(long id)
        {
            return View(repo.TransmittalOUTAuditTrailRepository.Find(id));
        }

        //
        // GET: /TransmittalOUTAuditTrails/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /TransmittalOUTAuditTrails/Create

        [HttpPost]
        public ActionResult Create(TransmittalOUTAuditTrail transmittaloutaudittrail)
        {
            if (ModelState.IsValid)
            {
                repo.TransmittalOUTAuditTrailRepository.InsertOrUpdate(transmittaloutaudittrail);
                repo.TransmittalOUTAuditTrailRepository.Save();
                return RedirectToAction("Index");  
            }

            return View(transmittaloutaudittrail);
        }
        
        //
        // GET: /TransmittalOUTAuditTrails/Edit/5
 
        public ActionResult Edit(long id)
        {
            return View(repo.TransmittalOUTAuditTrailRepository.Find(id));
        }

        //
        // POST: /TransmittalOUTAuditTrails/Edit/5

        [HttpPost]
        public ActionResult Edit(TransmittalOUTAuditTrail transmittaloutaudittrail)
        {
            if (ModelState.IsValid)
            {
                repo.TransmittalOUTAuditTrailRepository.InsertOrUpdate(transmittaloutaudittrail);
                repo.TransmittalOUTAuditTrailRepository.Save();
                return RedirectToAction("Index");
            }
            return View(transmittaloutaudittrail);
        }

        //
        // GET: /TransmittalOUTAuditTrails/Delete/5
 
        public ActionResult Delete(long id)
        {
            return View(repo.TransmittalOUTAuditTrailRepository.Find(id));
        }

        //
        // POST: /TransmittalOUTAuditTrails/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            repo.TransmittalOUTAuditTrailRepository.Delete(id);
            repo.TransmittalOUTAuditTrailRepository.Save();
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