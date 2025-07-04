using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;

namespace WareHouseMVC.Controllers
{
    public class ContactPersonsController : BaseController
    {


        //
        // GET: /ContactPersons/




		 UnitOfWork repo = new UnitOfWork();
         long clientId = 0;

        public ViewResult Index()
        {
             if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }

            
             ViewBag.ClientName = repo.ClientRepository.Find(clientId).ClientName;
             ViewBag.ClientId = clientId;


            List<ContactPerson> allContactPersons = new List<ContactPerson>();
            allContactPersons=repo.ContactPersonRepository.GetByClientId(clientId);// AllIncluding(cp=>cp.Department, cp=>cp.Department.Client).OrderByDescending(c=>c.ContactPersontID).ToList());

            return View(allContactPersons);
        }

        //
        // GET: /ContactPersons/Details/5

        public ViewResult Details(long id)
        {
            return View(repo.ContactPersonRepository.Find(id));
        }

        //
        // GET: /ContactPersons/Create

        public ActionResult Create()
        {
            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }


            ViewBag.ClientName = repo.ClientRepository.Find(clientId).ClientName;
            ViewBag.ClientId = clientId;

			ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
			ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(clientId);
            return View();
        } 

        //
        // POST: /ContactPersons/Create

        [HttpPost]
        public ActionResult Create(ContactPerson contactperson)
        {

            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }


            ViewBag.ClientName = repo.ClientRepository.Find(clientId).ClientName;
            ViewBag.ClientId = clientId;

            if (ModelState.IsValid) {

                repo.ContactPersonRepository.InsertOrUpdate(contactperson);
                repo.ContactPersonRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
                ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(clientId);
				return View();
			}
        }


        public ActionResult AddCP(int dpid)
        {
            ViewBag.dpID = dpid;
            Department dp = new Department();
            dp = repo.DepartmentRepository.Find(dpid);
            ViewBag.clID= dp.ClientID;

            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }


            ViewBag.ClientName = repo.ClientRepository.Find(clientId).ClientName;
            ViewBag.ClientId = clientId;

            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(clientId);
            return View("Create");
        }


        //
        // GET: /ContactPersons/Edit/5
 
        public ActionResult Edit(long id)
        {
            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }


            ViewBag.ClientName = repo.ClientRepository.Find(clientId).ClientName;
            ViewBag.ClientId = clientId;

			ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(clientId);
             return View(repo.ContactPersonRepository.Find(id));
        }

        //
        // POST: /ContactPersons/Edit/5

        [HttpPost]
        public ActionResult Edit(ContactPerson contactperson)
        {
            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }


            ViewBag.ClientName = repo.ClientRepository.Find(clientId).ClientName;
            ViewBag.ClientId = clientId;

            if (ModelState.IsValid) {
                repo.ContactPersonRepository.InsertOrUpdate(contactperson);
                repo.ContactPersonRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
                ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(clientId);
				return View();
			}
        }

        //
        // GET: /ContactPersons/Delete/5
 
        public ActionResult Delete(long id)
        {
            return View(repo.ContactPersonRepository.Find(id));
        }

        //
        // POST: /ContactPersons/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            repo.ContactPersonRepository.Delete(id);
            repo.ContactPersonRepository.Save();

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

