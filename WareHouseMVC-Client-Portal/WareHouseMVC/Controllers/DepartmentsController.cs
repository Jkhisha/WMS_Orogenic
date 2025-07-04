using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;
using PagedList;

namespace WareHouseMVC.Controllers
{
    public class DepartmentsController : BaseController
    {


        //
        // GET: /Departments/




		 UnitOfWork repo = new UnitOfWork();
         public IPagedList<Department> pagedList { get; set; }
         long clientId = 0;
      

        //long clientId= System.Web.HttpContext.Current.Session["ClientId"]
      

        [Authorize]
        public ViewResult Index(int? page)
        {
            int pageSize = 50;
            int pageNumber = (page ?? 1);


            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }

            List<Department> DepartmentList = new List<Department>();
            pagedList = new PagedList<Department>(DepartmentList, pageNumber, pageSize);

           // pagedList = repo.DepartmentRepository.GetAllPagedList(pageNumber, pageSize);
            pagedList = repo.DepartmentRepository.GetAllPagedListByClientId(clientId,pageNumber, pageSize);

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
            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }

            ViewBag.ClientName = repo.ClientRepository.Find(clientId).ClientName;
            ViewBag.ClientId = clientId;

			//ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            return View();
        } 

        //
        // POST: /Departments/Create

        [HttpPost]
        public ActionResult Create(Department department)
        {
            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }

            if (ModelState.IsValid) {

                repo.DepartmentRepository.InsertOrUpdate(department);
                repo.DepartmentRepository.Save();
                return RedirectToAction("Index");
            } else {

                ViewBag.ClientName = repo.ClientRepository.Find(clientId).ClientName;
                ViewBag.ClientId = clientId;
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
            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }

            ViewBag.ClientName = repo.ClientRepository.Find(clientId).ClientName;
            ViewBag.ClientId = clientId;
			//ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
             return View(repo.DepartmentRepository.Find(id));
        }

        //
        // POST: /Departments/Edit/5

        [HttpPost]
        public ActionResult Edit(Department department)
        {
            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }


            if (ModelState.IsValid) {
                repo.DepartmentRepository.InsertOrUpdate(department);
                repo.DepartmentRepository.Save();
                return RedirectToAction("Index");
            } else {

                ViewBag.ClientName = repo.ClientRepository.Find(clientId).ClientName;
                ViewBag.ClientId = clientId;

				//ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
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

