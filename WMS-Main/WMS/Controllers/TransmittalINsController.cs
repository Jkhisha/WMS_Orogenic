using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;
using WareHouseMVC.Common;
using System.IO;
using System.Configuration;
using System.Globalization;
using PagedList;

namespace WareHouseMVC.Controllers
{
    public class TransmittalINsController : Controller
    {
       // private WareHouseMVCContext context = new WareHouseMVCContext();
        UnitOfWork repo = new UnitOfWork();
        public IPagedList<TransmittalIN> pagedList { get; set; }  

        //
        // GET: /TransmittalINs/

        public ViewResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            List<TransmittalIN> transmittalINList = new List<TransmittalIN>();
            pagedList = new PagedList<TransmittalIN>(transmittalINList, pageNumber, pageSize);

            pagedList = repo.TransmittalINRepository.GetAllPagedList(pageNumber, pageSize);

            return View(pagedList);
           
        }

        //
        // GET: /TransmittalINs/Details/5

        public ViewResult Details(long id)
        {
            TransmittalIN transmittalin = repo.TransmittalINRepository.Find(id);


            ViewBag.id = id;
            TransmittalINStatus status = repo.TransmittalINStatusRepository.Find(transmittalin.TransmittalINStatusId);
            ViewBag.Status = status.StatusName;
            return View(transmittalin);


        }

        //
        // GET: /TransmittalINs/Create

        public ActionResult Create()
        {
            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.AllIncluding();
            ViewBag.PossibleContactPerson = repo.ContactPersonRepository.AllIncluding();
            ViewBag.PossibleProjects = repo.ProjectRepository.AllIncluding();
            TransmittalIN transmittalin = new TransmittalIN();

          transmittalin.TransmittalNo = "This No will be generated";
          transmittalin.CreateDate = DateTime.Now;

          TransmittalINStatus status = repo.TransmittalINStatusRepository.GetFirst();
          transmittalin.TransmittalINStatusId = status.TransmittalINStatusId;

            return View(transmittalin);
        }

        //
        // POST: /TransmittalINs/Create

        [HttpPost]
        public ActionResult Create(TransmittalIN transmittalin)
        {
            transmittalin.TransmittalNo = "demo";
           TransmittalINStatus status= repo.TransmittalINStatusRepository.GetFirst();
           transmittalin.TransmittalINStatusId = status.TransmittalINStatusId;


           if (transmittalin.DepartmentID != -1)
           {

              
                   transmittalin.CreateDate = DateTime.Now;
                   transmittalin.FileUrl = "";

                   if (transmittalin.IsFile == true)
                   {
                       transmittalin.Type = "File";
                   }
                   if (transmittalin.IsFile == false)
                   {
                       transmittalin.Type = "Box";
                   }


                   repo.TransmittalINRepository.InsertOrUpdate(transmittalin);
                   repo.TransmittalINRepository.Save();


              


                   long id = transmittalin.TransmittalINId;


                  
                   transmittalin.TransmittalNo = "ORBL-" + DateTime.Now.Year.ToString() + "-" + id.ToString();
                   repo.TransmittalINRepository.InsertOrUpdate(transmittalin);
                   repo.TransmittalINRepository.Save();


                   //Audit Train

                   HelperClasses.AuditTrailHelperClass auditHelper = new HelperClasses.AuditTrailHelperClass();
                   TransmittalINAuditTrail auditTrail = new TransmittalINAuditTrail();
                   auditTrail.CreateBy = User.Identity.Name;
                   auditTrail.CreateByIP = auditHelper.LocalIPAddress();
                   auditTrail.CreateDate = DateTime.Now;
                   auditTrail.BoxReceivedByDate = DateTime.Now;
                   auditTrail.CreateTime = DateTime.Now.ToString("h:mm tt", CultureInfo.InvariantCulture);
                   auditTrail.TransmittalINNo = transmittalin.TransmittalNo;
                   repo.TransmittalINAuditTrailRepository.InsertOrUpdate(auditTrail);
                   repo.TransmittalINAuditTrailRepository.Save();


                   //Audit Train
                   if (Request.Files[0].ContentType.ToString().ToLower().Contains("image"))
                   {
                       var fileName = Path.GetFileName(Request.Files[0].FileName);



                       var path = Path.Combine(Server.MapPath("~/Content/Images/") + ConfigurationManager.AppSettings["UploadedDocument"].ToString() + "TransmittalIN/" + transmittalin.TransmittalINId.ToString() + "/");
                       if (!Directory.Exists(path))
                       {
                           Directory.CreateDirectory(path);
                       }

                       Request.Files[0].SaveAs(Path.Combine(path, fileName));
                       transmittalin.FileUrl = "Content/Images/" + ConfigurationManager.AppSettings["UploadedDocument"].ToString() + "TransmittalIN/" + transmittalin.TransmittalINId.ToString() + "/" + fileName;
                       repo.TransmittalINRepository.InsertOrUpdate(transmittalin);
                       repo.TransmittalINRepository.Save();


                   }


                   return RedirectToAction("Edit", new { id = id });
               
           }


            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(transmittalin.ClientID);
            ViewBag.PossibleContactPerson = repo.ContactPersonRepository.AllIncluding();
            ViewBag.PossibleProjects = repo.ProjectRepository.AllIncluding();

            var selectList = new List<SelectListItem>();
            foreach (WareHouseMVC.Models.EnumHelper.Status satus in Enum.GetValues(typeof(WareHouseMVC.Models.EnumHelper.Status)))
            {
                selectList.Add(new SelectListItem() { Value = ((int)satus).ToString(), Text = satus.ToString() });
            }

            ViewBag.Status = selectList;
          
            return View(transmittalin);
        }

        //
        // GET: /TransmittalINs/Edit/5

        public ActionResult Edit(long id)
        {
            TransmittalIN transmittalin = repo.TransmittalINRepository.Find(id);
            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.AllIncluding();
            ViewBag.PossibleContactPerson = repo.ContactPersonRepository.AllIncluding();
            ViewBag.PossibleProjects = repo.ProjectRepository.AllIncluding();

            //var selectList = new List<SelectListItem>();
            //foreach (WareHouseMVC.Models.EnumHelper.Status satus in Enum.GetValues(typeof(WareHouseMVC.Models.EnumHelper.Status)))
            //{
            //    selectList.Add(new SelectListItem() { Value = ((int)satus).ToString(), Text = satus.ToString() });
            //}

            //ViewBag.Status = selectList;
            ViewBag.id = id;
            ViewBag.CreateDate = transmittalin.CreateDate;
            ViewBag.TransmittalINStatusId = transmittalin.TransmittalINStatusId;
            return View(transmittalin);
        }

        //
        // POST: /TransmittalINs/Edit/5

        [HttpPost]
        public ActionResult Edit(TransmittalIN transmittalin)
        {

            if (ModelState.IsValid)
            {
                repo.TransmittalINRepository.InsertOrUpdate(transmittalin);
                repo.TransmittalINRepository.Save();
                return RedirectToAction("Edit", new {id=transmittalin.TransmittalINId });
            }
            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(transmittalin.ClientID);
            ViewBag.PossibleContactPerson = repo.ContactPersonRepository.AllIncluding();
            ViewBag.PossibleProjects = repo.ProjectRepository.AllIncluding();


            return View(transmittalin);
        }

        //
        // GET: /TransmittalINs/Delete/5

        public ActionResult Delete(long id)
        {
            TransmittalIN transmittalin = repo.TransmittalINRepository.Find(id);
            return View(transmittalin);
        }

        //
        // POST: /TransmittalINs/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            TransmittalIN transmittalin = repo.TransmittalINRepository.Find(id);
            repo.TransmittalINRepository.Delete(id);
            repo.TransmittalINRepository.Save();
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