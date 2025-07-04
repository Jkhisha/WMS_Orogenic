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
    public class TransmittalINsController : BaseController
    {
        // private WareHouseMVCContext context = new WareHouseMVCContext();
        UnitOfWork repo = new UnitOfWork();
        public IPagedList<TransmittalIN> pagedList { get; set; }
        long clientId = 0;
        Guid userId = Guid.Empty;

        WareHouseMVC.HelperClasses.MailHelper mailHelper = new HelperClasses.MailHelper();
        //
        // GET: /TransmittalINs/

        public ViewResult Index(int? page)
        {
            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }

            if (System.Web.HttpContext.Current.Session["UserId"] != null)
            {
                userId = (Guid)System.Web.HttpContext.Current.Session["UserId"];
            }

            int pageSize = 50;
            int pageNumber = (page ?? 1);

            long departmentId = 0;
            Department dept = repo.DepartmentRepository.GetDepartmentByUser(userId);
            if (dept != null)
            {
                departmentId = dept.DepartmentID;
            }

            string subDepartment = repo.DepartmentRepository.GetSubDepartmentName(userId);

            List<TransmittalIN> transmittalINList = new List<TransmittalIN>();
            pagedList = new PagedList<TransmittalIN>(transmittalINList, pageNumber, pageSize);

            // pagedList = repo.TransmittalINRepository.GetAllPagedList(pageNumber, pageSize);
            pagedList = repo.TransmittalINRepository.GetByClientIDPagedList(clientId, departmentId, subDepartment, pageNumber, pageSize);
            //SendMailDestructionMail(clientId);
            return View(pagedList);

        }


        private void SendMailDestructionMail(long clientId)
        {
            #region Mail Method

            string MailSubject = "Upcoming Destruction Period Boxes";
            string MailBody = "Dear Client,</br>";
            MailBody = MailBody + "Please see the Attachment for upcoming Destruction Period Box List. </br>";
            MailBody = MailBody + "</br> Thank You.</br>" + System.Environment.NewLine;

            Client client = new Client();
            client = repo.ClientRepository.Find(clientId);



            mailHelper.SendMailAttachment(client.Email, MailSubject, MailBody);
            #endregion
        }

        //SHUVO
        private void SendMailNewTransmitINMail(long clientId)
        {
            #region Mail Method

            string MailSubject = "New Transmittal-IN has been created on WHMS Client Portal!";
            string MailBody = "Dear Admin,<br/>";
            MailBody = MailBody + "An User has created a new Transmittal-IN on WHMS Client Portal. <br/>";
            MailBody = MailBody + "Here is the Transmittal-IN URL : http://119.148.18.172/warehouse/TransmittalINs <br/>";
            MailBody = MailBody + "<br/> Thank You.<br/>" + System.Environment.NewLine;

            Client client = new Client();
            client = repo.ClientRepository.Find(clientId);

            mailHelper.SendMail(client.Email, MailSubject, MailBody);
            #endregion
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

        [HttpGet]
        public JsonResult GetSubDepartment(long deptId)
        {
            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }
            
            var subDepartments = repo.DepartmentRepository.GetAllSubDepartments(clientId, deptId).ToList();

            return Json(subDepartments, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetContactPerson(long deptId)
        {
            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }

            var contactPerson = repo.ContactPersonRepository.GetByClientAndDepartment(clientId, deptId);

            return Json(contactPerson, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /TransmittalINs/Create

        public ActionResult Create()
        {
            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }

            if (System.Web.HttpContext.Current.Session["UserId"] != null)
            {
                userId = (Guid)System.Web.HttpContext.Current.Session["UserId"];
            }

            ViewBag.ClientName = repo.ClientRepository.Find(clientId).ClientName;
            ViewBag.ClientId = clientId;
            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(clientId).Where(dept => !dept.DepartmentName.Equals("superadmin", StringComparison.OrdinalIgnoreCase)).ToList();

            TransmittalIN transmittalin = new TransmittalIN();
            transmittalin.TransmittalNo = "This No will be generated";
            transmittalin.CreateDate = DateTime.Now;
            transmittalin.TransmittalDate = DateTime.Now;
            transmittalin.Department = repo.DepartmentRepository.GetDepartmentByUser(userId);
            transmittalin.DepartmentID = transmittalin.Department.DepartmentID;
            ViewBag.DeptName = transmittalin.Department.DepartmentName;

            string subDept = repo.DepartmentRepository.GetSubDepartmentName(userId);
            transmittalin.SubDepartment = string.IsNullOrWhiteSpace(subDept) ? "" : subDept;
            ViewBag.SubDept = transmittalin.SubDepartment;

            TransmittalINStatus status = repo.TransmittalINStatusRepository.GetFirst();
            transmittalin.TransmittalINStatusId = status.TransmittalINStatusId;

            return View(transmittalin);
        }

        //
        // POST: /TransmittalINs/Create

        [HttpPost]
        public ActionResult Create(TransmittalIN transmittalin)
        {
            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }

            ViewBag.ClientName = repo.ClientRepository.Find(clientId).ClientName;
            ViewBag.ClientId = clientId;

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



                transmittalin.TransmittalNo = "ORBL-CL-" + DateTime.Now.Year.ToString() + "-" + id.ToString();
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
                //SendMailNewTransmitINMail(clientId);

                //return RedirectToAction("Edit", new { id = id });
                return RedirectToRoute(new { controller = "HandOverBies", action = "HandOverBy", trId = id });

            }


            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(clientId);
            ViewBag.PossibleContactPerson = repo.ContactPersonRepository.GetByClientId(clientId);
            ViewBag.PossibleProjects = repo.ProjectRepository.GetByClientId(clientId);

            var selectList = new List<SelectListItem>();
            foreach (WareHouseMVC.Models.EnumHelper.Status satus in Enum.GetValues(typeof(WareHouseMVC.Models.EnumHelper.Status)))
            {
                selectList.Add(new SelectListItem() { Value = ((int)satus).ToString(), Text = satus.ToString() });
            }

            ViewBag.Status = selectList;

            return View(transmittalin);
        }


        // GET: /TransmittalINs/Edit/5

        public ActionResult Edit(long id)
        {
            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }

            if (System.Web.HttpContext.Current.Session["UserId"] != null)
            {
                userId = (Guid)System.Web.HttpContext.Current.Session["UserId"];
            }

            TransmittalIN transmittalin = repo.TransmittalINRepository.Find(id);
            ViewBag.id = id;
            ViewBag.ClientName = transmittalin.Client;
            ViewBag.ClientId = transmittalin.ClientID;
            ViewBag.CreateDate = transmittalin.CreateDate;
            ViewBag.TransmittalINStatusId = transmittalin.TransmittalINStatusId;

            ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(clientId).Where(dept => !dept.DepartmentName.Equals("superadmin", StringComparison.OrdinalIgnoreCase)).ToList();
            ViewBag.DeptName = repo.DepartmentRepository.GetDepartmentByUser(userId).DepartmentName;
            var subDept = repo.DepartmentRepository.GetSubDepartmentName(userId);
            ViewBag.SubDept = string.IsNullOrWhiteSpace(subDept) ? "" : subDept;

            return View(transmittalin);
        }

        //
        // POST: /TransmittalINs/Edit/5

        [HttpPost]
        public ActionResult Edit(TransmittalIN transmittalin)
        {

            TransmittalIN trin = repo.TransmittalINRepository.Find(transmittalin.TransmittalINId);
            transmittalin.TransmittalNo = trin.TransmittalNo;
            transmittalin.TransmittalINStatus = trin.TransmittalINStatus;
            transmittalin.TransmittalINStatusId = trin.TransmittalINStatusId;
            transmittalin.CreateDate = trin.CreateDate;
            transmittalin.Type = trin.Type;

            repo.TransmittalINRepository.InsertOrUpdate(transmittalin);
            repo.TransmittalINRepository.Save();
            return RedirectToRoute(new { controller = "HandOverBies", action = "HandOverBy", trId = trin.TransmittalINId });
        }


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