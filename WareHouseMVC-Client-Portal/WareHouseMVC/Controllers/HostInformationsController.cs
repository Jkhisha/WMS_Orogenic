using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;

namespace WareHouseMVC.Controllers
{
    public class HostInformationsController : BaseController
    {
        private WareHouseMVCContext context = new WareHouseMVCContext();

        //
        // GET: /HostInformations/

        public ViewResult Index()
        {
            return View(context.HostInformations.ToList());
        }

        //
        // GET: /HostInformations/Details/5

        public ViewResult Details(long id)
        {
            HostInformation hostinformation = context.HostInformations.Single(x => x.HostInformationId == id);
            return View(hostinformation);
        }

        //
        // GET: /HostInformations/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /HostInformations/Create

        [HttpPost]
        public ActionResult Create(HostInformation hostinformation)
        {
            if (ModelState.IsValid)
            {
                context.HostInformations.Add(hostinformation);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(hostinformation);
        }
        
        //
        // GET: /HostInformations/Edit/5
 
        public ActionResult Edit(long id)
        {
            HostInformation hostinformation = context.HostInformations.Single(x => x.HostInformationId == id);
            return View(hostinformation);
        }

        //
        // POST: /HostInformations/Edit/5

        [HttpPost]
        public ActionResult Edit(HostInformation hostinformation)
        {
            if (ModelState.IsValid)
            {
                context.Entry(hostinformation).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hostinformation);
        }

        //
        // GET: /HostInformations/Delete/5
 
        public ActionResult Delete(long id)
        {
            HostInformation hostinformation = context.HostInformations.Single(x => x.HostInformationId == id);
            return View(hostinformation);
        }

        //
        // POST: /HostInformations/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            HostInformation hostinformation = context.HostInformations.Single(x => x.HostInformationId == id);
            context.HostInformations.Remove(hostinformation);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}