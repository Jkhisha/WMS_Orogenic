using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;

namespace WareHouseMVC.Controllers
{   
    public class TestsController : Controller
    {


        //
        // GET: /Tests/




		 UnitOfWork repo = new UnitOfWork();


        public ViewResult Index()
        {
            return View(repo.TestRepository.AllIncluding());
        }

        //
        // GET: /Tests/Details/5

        public ViewResult Details(long id)
        {
            return View(repo.TestRepository.Find(id));
        }

        //
        // GET: /Tests/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Tests/Create

        [HttpPost]
        public ActionResult Create(Test test)
        {
            if (ModelState.IsValid) {

                repo.TestRepository.InsertOrUpdate(test);
                repo.TestRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /Tests/Edit/5
 
        public ActionResult Edit(long id)
        {
             return View(repo.TestRepository.Find(id));
        }

        //
        // POST: /Tests/Edit/5

        [HttpPost]
        public ActionResult Edit(Test test)
        {
            if (ModelState.IsValid) {
                repo.TestRepository.InsertOrUpdate(test);
                repo.TestRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /Tests/Delete/5
 
        public ActionResult Delete(long id)
        {
            return View(repo.TestRepository.Find(id));
        }

        //
        // POST: /Tests/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            repo.TestRepository.Delete(id);
            repo.TestRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
          /*  if (disposing)
			 {

				                testRepository.Dispose();
				            }*/
			repo.Dispose();
            base.Dispose(disposing);
        }
    }
}

