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
    public class TransmittalINAuditTrailsController : Controller
    {

        UnitOfWork repo = new UnitOfWork();

        //
        // GET: /TransmittalINAuditTrails/

        public ViewResult Index()
        {
           
            return View(repo.TransmittalINAuditTrailRepository.AllIncluding().OrderByDescending(o=>o.CreateDate));
        }

        //
        // GET: /TransmittalINAuditTrails/Details/5

        public ViewResult Details(long id)
        {
            return View(repo.TransmittalINAuditTrailRepository.Find(id));
        }

        //
        // GET: /TransmittalINAuditTrails/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /TransmittalINAuditTrails/Create

        [HttpPost]
        public ActionResult Create(TransmittalINAuditTrail transmittalinaudittrail)
        {
            if (ModelState.IsValid)
            {
                repo.TransmittalINAuditTrailRepository.InsertOrUpdate(transmittalinaudittrail);
                repo.TransmittalINAuditTrailRepository.Save();
                return RedirectToAction("Index");  
            }

            return View(transmittalinaudittrail);
        }
        
        //
        // GET: /TransmittalINAuditTrails/Edit/5
 
        public ActionResult Edit(long id)
        {
            return View(repo.TransmittalINAuditTrailRepository.Find(id));
        }

        //
        // POST: /TransmittalINAuditTrails/Edit/5

        [HttpPost]
        public ActionResult Edit(TransmittalINAuditTrail transmittalinaudittrail)
        {
            if (ModelState.IsValid)
            {
                repo.TransmittalINAuditTrailRepository.InsertOrUpdate(transmittalinaudittrail);
                repo.TransmittalINAuditTrailRepository.Save();
                return RedirectToAction("Index");
            }
            return View(transmittalinaudittrail);
        }

        //
        // GET: /TransmittalINAuditTrails/Delete/5
 
        public ActionResult Delete(long id)
        {
            return View(repo.TransmittalINAuditTrailRepository.Find(id));
        }

        //
        // POST: /TransmittalINAuditTrails/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            repo.TransmittalINAuditTrailRepository.Delete(id);
            repo.TransmittalINAuditTrailRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}