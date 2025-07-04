using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;

namespace WareHouseMVC.Controllers
{
    public class ORBLOperatorsController : BaseController
    {
        private WareHouseMVCContext context = new WareHouseMVCContext();

        //
        // GET: /ORBLOperators/

        public ViewResult Index()
        {
            return View(context.ORBLOperators.ToList());
        }

        //
        // GET: /ORBLOperators/Details/5

        public ViewResult Details(long id)
        {
            ORBLOperator orbloperator = context.ORBLOperators.Single(x => x.ORBLOperatorId == id);
            return View(orbloperator);
        }

        //
        // GET: /ORBLOperators/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /ORBLOperators/Create

        [HttpPost]
        public ActionResult Create(ORBLOperator orbloperator)
        {
            if (ModelState.IsValid)
            {
                context.ORBLOperators.Add(orbloperator);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(orbloperator);
        }
        
        //
        // GET: /ORBLOperators/Edit/5
 
        public ActionResult Edit(long id)
        {
            ORBLOperator orbloperator = context.ORBLOperators.Single(x => x.ORBLOperatorId == id);
            return View(orbloperator);
        }

        //
        // POST: /ORBLOperators/Edit/5

        [HttpPost]
        public ActionResult Edit(ORBLOperator orbloperator)
        {
            if (ModelState.IsValid)
            {
                context.Entry(orbloperator).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(orbloperator);
        }

        //
        // GET: /ORBLOperators/Delete/5
 
        public ActionResult Delete(long id)
        {
            ORBLOperator orbloperator = context.ORBLOperators.Single(x => x.ORBLOperatorId == id);
            return View(orbloperator);
        }

        //
        // POST: /ORBLOperators/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            ORBLOperator orbloperator = context.ORBLOperators.Single(x => x.ORBLOperatorId == id);
            context.ORBLOperators.Remove(orbloperator);
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