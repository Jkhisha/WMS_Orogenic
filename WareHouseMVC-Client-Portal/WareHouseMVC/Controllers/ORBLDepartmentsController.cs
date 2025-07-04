using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;

namespace WareHouseMVC.Controllers
{
    public class ORBLDepartmentsController : BaseController
    {


        //
        // GET: /ORBLDepartments/




		 UnitOfWork repo = new UnitOfWork();


        public ViewResult Index()
        {
            return View(repo.ORBLDepartmentRepository.AllIncluding());
        }

        //
        // GET: /ORBLDepartments/Details/5

        public ViewResult Details(long id)
        {
            return View(repo.ORBLDepartmentRepository.Find(id));
        }

        //
        // GET: /ORBLDepartments/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /ORBLDepartments/Create

        [HttpPost]
        public ActionResult Create(ORBLDepartment orbldepartment)
        {
            if (ModelState.IsValid) {

                repo.ORBLDepartmentRepository.InsertOrUpdate(orbldepartment);
                repo.ORBLDepartmentRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /ORBLDepartments/Edit/5
 
        public ActionResult Edit(long id)
        {
             return View(repo.ORBLDepartmentRepository.Find(id));
        }

        //
        // POST: /ORBLDepartments/Edit/5

        [HttpPost]
        public ActionResult Edit(ORBLDepartment orbldepartment)
        {
            if (ModelState.IsValid) {
                repo.ORBLDepartmentRepository.InsertOrUpdate(orbldepartment);
                repo.ORBLDepartmentRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /ORBLDepartments/Delete/5
 
        public ActionResult Delete(long id)
        {
            return View(repo.ORBLDepartmentRepository.Find(id));
        }

        //
        // POST: /ORBLDepartments/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            repo.ORBLDepartmentRepository.Delete(id);
            repo.ORBLDepartmentRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
          if (disposing)
			 {

			/*	                orbldepartmentRepository.Dispose();
				 */
			repo.Dispose();

            }
			
            base.Dispose(disposing);
        }
    }
}

