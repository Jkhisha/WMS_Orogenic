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
    public class ProjectsController : BaseController
    {
        //private WareHouseMVCContext context = new WareHouseMVCContext();

        UnitOfWork repo = new UnitOfWork();

        //
        // GET: /Projects/

        public ViewResult Index()
        {
            return View(repo.ProjectRepository.AllIncluding(project => project.Department,project=>project.Department.Client).ToList());
        }

        //
        // GET: /Projects/Details/5

        public ViewResult Details(long id)
        {
            Project project = repo.ProjectRepository.Find(id);
            return View(project);
        }

        //
        // GET: /Projects/Create

        public ActionResult Create()
        {
            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.AllIncluding();
            return View();
        } 

        //
        // POST: /Projects/Create

        [HttpPost]
        public ActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                repo.ProjectRepository.InsertOrUpdate(project);
                repo.ProjectRepository.Save();
                return RedirectToAction("Index");  
            }

            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(project.ClientID);
            return View(project);
        }
        
        //
        // GET: /Projects/Edit/5
 
        public ActionResult Edit(long id)
        {
            Project project = repo.ProjectRepository.Find(id);
            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.AllIncluding();
            return View(project);
        }

        //
        // POST: /Projects/Edit/5

        [HttpPost]
        public ActionResult Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                repo.ProjectRepository.InsertOrUpdate(project);
                repo.ProjectRepository.Save();
                return RedirectToAction("Index");
            }
            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(project.ClientID);
            return View(project);
        }


        public ActionResult AddProject(int dpid)
        {
            ViewBag.dpID = dpid;
            Department dp = new Department();
            dp = repo.DepartmentRepository.Find(dpid);
            ViewBag.clID = dp.ClientID;

            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.AllIncluding();
            return View("Create");
        }




        //
        // GET: /Projects/Delete/5
 
        public ActionResult Delete(long id)
        {
            Project project = repo.ProjectRepository.Find(id);
            return View(project);
        }

        //
        // POST: /Projects/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            Project project = repo.ProjectRepository.Find(id);
            repo.ProjectRepository.Delete(id);
            repo.ProjectRepository.Save();
            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.AllIncluding();
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