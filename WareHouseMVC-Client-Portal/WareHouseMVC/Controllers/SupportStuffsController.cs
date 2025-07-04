using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;

namespace WareHouseMVC.Controllers
{
    public class SupportStuffsController : BaseController
    {


        //
        // GET: /SupportStuffs/




		 UnitOfWork repo = new UnitOfWork();


        public ViewResult Index()
        {
            return View(repo.SupportStuffRepository.AllIncluding(sf=>sf.ORBLDepartment).ToList());
        }

        //
        // GET: /SupportStuffs/Details/5

        public ViewResult Details(long id)
        {
            return View(repo.SupportStuffRepository.Find(id));
        }


        public ActionResult AddSupportStuff(int dpid)
        {
            ViewBag.dPID = dpid;
            ViewBag.PossibleORBLDepartments = repo.ORBLDepartmentRepository.AllIncluding();
            return View("Create");
        }

        //
        // GET: /SupportStuffs/Create

        public ActionResult Create()
        {
			ViewBag.PossibleORBLDepartments = repo.ORBLDepartmentRepository.AllIncluding();
            return View();
        } 

        //
        // POST: /SupportStuffs/Create

        [HttpPost]
        public ActionResult Create(SupportStuff supportstuff)
        {
            if (ModelState.IsValid) {

                repo.SupportStuffRepository.InsertOrUpdate(supportstuff);
                repo.SupportStuffRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleORBLDepartments = repo.ORBLDepartmentRepository.AllIncluding();
				return View();
			}
        }
        
        //
        // GET: /SupportStuffs/Edit/5
 
        public ActionResult Edit(long id)
        {
			ViewBag.PossibleORBLDepartments = repo.ORBLDepartmentRepository.AllIncluding();
             return View(repo.SupportStuffRepository.Find(id));
        }

        //
        // POST: /SupportStuffs/Edit/5

        [HttpPost]
        public ActionResult Edit(SupportStuff supportstuff)
        {
            if (ModelState.IsValid) {
                repo.SupportStuffRepository.InsertOrUpdate(supportstuff);
                repo.SupportStuffRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleORBLDepartments = repo.ORBLDepartmentRepository.AllIncluding();
				return View();
			}
        }

        //
        // GET: /SupportStuffs/Delete/5
 
        public ActionResult Delete(long id)
        {
            return View(repo.SupportStuffRepository.Find(id));
        }

        //
        // POST: /SupportStuffs/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            repo.SupportStuffRepository.Delete(id);
            repo.SupportStuffRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
          if (disposing)
			 {

			/*	                orbldepartmentRepository.Dispose();
				                supportstuffRepository.Dispose();
				 */
			repo.Dispose();

            }
			
            base.Dispose(disposing);
        }
    }
}

