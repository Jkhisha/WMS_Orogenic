using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;
using PagedList;

namespace WareHouseMVC.Controllers
{   
    public class DepartmentsController : Controller
    {


        //
        // GET: /Departments/




		 UnitOfWork repo = new UnitOfWork();
         public IPagedList<Department> pagedList { get; set; }  

      


        public ViewResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            List<Department> DepartmentList = new List<Department>();
            pagedList = new PagedList<Department>(DepartmentList, pageNumber, pageSize);

            pagedList = repo.DepartmentRepository.GetAllPagedList(pageNumber, pageSize);

            return View(pagedList);

        }



        //
        // GET: /Departments/Details/5

        public ViewResult Details(long id)
        {
            return View(repo.DepartmentRepository.Find(id));
        }

        //
        // GET: /Departments/Create

        public ActionResult Create()
        {
			ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            return View();
        } 

        //
        // POST: /Departments/Create

        [HttpPost]
        public ActionResult Create(Department department)
        {
            if (ModelState.IsValid) {

                repo.DepartmentRepository.InsertOrUpdate(department);
                repo.DepartmentRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
				return View();
			}
        }


        public ActionResult AddDept(int clid)
        {
            ViewBag.clID = clid;


            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            return View("Create");
        }


        //
        // GET: /Departments/Edit/5
 
        public ActionResult Edit(long id)
        {
			ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
             return View(repo.DepartmentRepository.Find(id));
        }

        //
        // POST: /Departments/Edit/5

        [HttpPost]
        public ActionResult Edit(Department department)
        {
            if (ModelState.IsValid) {
                repo.DepartmentRepository.InsertOrUpdate(department);
                repo.DepartmentRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
				return View();
			}
        }

        //
        // GET: /Departments/Delete/5
 
        public ActionResult Delete(long id)
        {
            return View(repo.DepartmentRepository.Find(id));
        }

        //
        // POST: /Departments/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            repo.DepartmentRepository.Delete(id);
            repo.DepartmentRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
			 {
	repo.Dispose();
				               }
		
            base.Dispose(disposing);
        }
    }
}

