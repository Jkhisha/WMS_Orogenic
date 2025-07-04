using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;

namespace WareHouseMVC.Controllers
{   
    public class ClientBillingInfoesController : Controller
    {


        //
        // GET: /ClientBillingInfoes/




		 UnitOfWork repo = new UnitOfWork();


        public ViewResult Index()
        {
            List<ClientBillingInfo> allLst = new List<ClientBillingInfo>();
            allLst = repo.ClientBillingInfoRepository.GetAll();

            return View(allLst);
        }

        //
        // GET: /ClientBillingInfoes/Details/5

        public ViewResult Details(long id)
        {
            return View(repo.ClientBillingInfoRepository.Find(id));
        }

        //
        // GET: /ClientBillingInfoes/Create

        public ActionResult Create()
        {
			ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            return View();
        } 

        //
        // POST: /ClientBillingInfoes/Create

        [HttpPost]
        public ActionResult Create(ClientBillingInfo clientbillinginfo)
        {
            if (ModelState.IsValid) {

                repo.ClientBillingInfoRepository.InsertOrUpdate(clientbillinginfo);
                repo.ClientBillingInfoRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
				return View();
			}
        }
        
        //
        // GET: /ClientBillingInfoes/Edit/5
 
        public ActionResult Edit(long id)
        {
			ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
             return View(repo.ClientBillingInfoRepository.Find(id));
        }

        //
        // POST: /ClientBillingInfoes/Edit/5

        [HttpPost]
        public ActionResult Edit(ClientBillingInfo clientbillinginfo)
        {
            if (ModelState.IsValid) {
                repo.ClientBillingInfoRepository.InsertOrUpdate(clientbillinginfo);
                repo.ClientBillingInfoRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
				return View();
			}
        }

        //
        // GET: /ClientBillingInfoes/Delete/5
 
        public ActionResult Delete(long id)
        {
            return View(repo.ClientBillingInfoRepository.Find(id));
        }

        //
        // POST: /ClientBillingInfoes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            repo.ClientBillingInfoRepository.Delete(id);
            repo.ClientBillingInfoRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
          if (disposing)
			 {

			/*	                clientRepository.Dispose();
				                clientbillinginfoRepository.Dispose();
				 */
			repo.Dispose();

            }
			
            base.Dispose(disposing);
        }
    }
}

