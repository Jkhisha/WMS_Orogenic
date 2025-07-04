using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;

namespace WareHouseMVC.Controllers
{
    public class AutoZoneSuggentionsController : BaseController
    {
        UnitOfWork repo = new UnitOfWork();

        //
        // GET: /AutoZoneSuggentions/

        public ViewResult Index()
        {
            return View(repo.AutoZoneSuggentionRepository.AllIncluding());
        }

        //
        // GET: /AutoZoneSuggentions/Details/5

        public ViewResult Details(long id)
        {
            AutoZoneSuggention autozonesuggention = repo.AutoZoneSuggentionRepository.Find(id);
            return View(autozonesuggention);
        }

        //
        // GET: /AutoZoneSuggentions/Create

        public ActionResult Create()
        {
            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
            return View();
        }

        //
        // POST: /AutoZoneSuggentions/Create

        [HttpPost]
        public ActionResult Create(AutoZoneSuggention autozonesuggention)
        {
            if (ModelState.IsValid)
            {
                repo.AutoZoneSuggentionRepository.InsertOrUpdate(autozonesuggention);
                repo.AutoZoneSuggentionRepository.Save();
                return RedirectToAction("Index");
            }


            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
            return View(autozonesuggention);
        }

        //
        // GET: /AutoZoneSuggentions/Edit/5

        public ActionResult Edit(long id)
        {
            AutoZoneSuggention autozonesuggention = repo.AutoZoneSuggentionRepository.Find(id);
            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
            return View(autozonesuggention);
        }

        //
        // POST: /AutoZoneSuggentions/Edit/5

        [HttpPost]
        public ActionResult Edit(AutoZoneSuggention autozonesuggention)
        {
            if (ModelState.IsValid)
            {
                repo.AutoZoneSuggentionRepository.InsertOrUpdate(autozonesuggention);
                repo.AutoZoneSuggentionRepository.Save();
                return RedirectToAction("Index");
            }
            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
            return View(autozonesuggention);
        }

        //
        // GET: /AutoZoneSuggentions/Delete/5

        public ActionResult Delete(long id)
        {
            AutoZoneSuggention autozonesuggention = repo.AutoZoneSuggentionRepository.Find(id);
            return View(autozonesuggention);
        }

        //
        // POST: /AutoZoneSuggentions/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            repo.AutoZoneSuggentionRepository.Delete(id);
            repo.AutoZoneSuggentionRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            //if (disposing)
            //{
            //    context.Dispose();
            //}
            base.Dispose(disposing);
        }
    }
}