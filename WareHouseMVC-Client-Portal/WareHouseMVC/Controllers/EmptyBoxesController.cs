using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Configuration;
using WareHouseMVC.HelperClasses;
using System.Drawing;
using System.IO;
using PagedList;
using WareHouseMVC.Models;
using System.Configuration;


namespace WareHouseMVC.Controllers
{
    public class EmptyBoxesController : BaseController
    {
       // private WareHouseMVCContext context = new WareHouseMVCContext();
        UnitOfWork repo = new UnitOfWork();

        long clientId = 0;
        Guid userId = (Guid)System.Web.HttpContext.Current.Session["UserId"];

        //
        // GET: /EmptyBoxes/

        public ViewResult Index()
        {
            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }

            Department dept = repo.DepartmentRepository.GetDepartmentByUser(userId);
            long departmentId = dept.DepartmentID;

            string subDepartment = repo.DepartmentRepository.GetSubDepartmentName(userId);

            return View(repo.EmptyBoxRepository.GetFilteredByDept(clientId, departmentId, subDepartment).ToList());

        }

        public ViewResult EmptyBoxClient() {

            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }

            List<EmptyBox> allBoxes = new List<EmptyBox>();
            allBoxes = repo.EmptyBoxRepository.GetByClientId(clientId);
            return View("Index",allBoxes);

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

        //
        // GET: /EmptyBoxes/Details/5

        public ViewResult Details(long id)
        {
            EmptyBox emptybox = repo.EmptyBoxRepository.Find(id);
            return View(emptybox);
        }


        //
        // GET: /EmptyBoxes/Create

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


            EmptyBox emptyBox = new EmptyBox();
            emptyBox.EmptyBoxNo = "This No will be generated";
            emptyBox.RecuisitionDate = DateTime.Now;
            emptyBox.Department = repo.DepartmentRepository.GetDepartmentByUser(userId);
            emptyBox.DepartmentID = emptyBox.Department.DepartmentID;
            ViewBag.DeptName = emptyBox.Department.DepartmentName;

            emptyBox.DeliverTo = repo.UserRepository.GetByUserId(userId).Username;
            emptyBox.Address = emptyBox.Department.DepartmentAddress;

            string subDept = repo.DepartmentRepository.GetSubDepartmentName(userId);
            emptyBox.SubDepartment = string.IsNullOrWhiteSpace(subDept) ? "" : subDept;
            ViewBag.SubDept = emptyBox.SubDepartment;

            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(clientId).Where(dept => !dept.DepartmentName.Equals("superadmin", StringComparison.OrdinalIgnoreCase)).ToList();
            ViewBag.PossibleOperator = repo.ORBLOperatorRepository.AllIncluding();
            ViewBag.EmptyBoxNo = "This No will be generated";
            return View(emptyBox);
        } 

        //
        // POST: /EmptyBoxes/Create

        [HttpPost]
        public ActionResult Create(EmptyBox emptybox)
        {
            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }

            if (System.Web.HttpContext.Current.Session["UserId"] != null)
            {
                userId = (Guid)System.Web.HttpContext.Current.Session["UserId"];
            }

            ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(clientId).Where(dept => !dept.DepartmentName.Equals("superadmin", StringComparison.OrdinalIgnoreCase)).ToList();
            ViewBag.DeptName = repo.DepartmentRepository.GetDepartmentByUser(userId).DepartmentName;
            var subDept = repo.DepartmentRepository.GetSubDepartmentName(userId);
            ViewBag.SubDept = string.IsNullOrWhiteSpace(subDept) ? "" : subDept;

            ViewBag.ClientName = repo.ClientRepository.Find(clientId).ClientName;
            ViewBag.ClientId = clientId;

            emptybox.EmptyBoxNo = "demo";

            if (ModelState.IsValid)
            {
                repo.EmptyBoxRepository.InsertOrUpdate(emptybox);
                repo.EmptyBoxRepository.Save();
                long id = emptybox.EmptyBoxId;

                emptybox.EmptyBoxNo = "ORBL-EMPBX-Cl-" + DateTime.Now.Year.ToString() + "-" + id.ToString();
                repo.EmptyBoxRepository.InsertOrUpdate(emptybox);
                repo.EmptyBoxRepository.Save();

                if (emptybox.NoofBoxes > 0)
                {
                    BarCodeRepository barcodeRef = new BarCodeRepository();
                    for (int i = 0; i < emptybox.NoofBoxes; i++)
                    {


                        BarCode BarCodeObj = barcodeRef.GetBarcodeTxtEmptyBoxes(emptybox.EmptyBoxId);

                        EmptyBoxBarcode BarcodeBox = new EmptyBoxBarcode()
                        {
                            BarCodeText = BarCodeObj.BoxName,
                            BarCodeId = BarCodeObj.BarCodeId,
                            EmptyBoxId = emptybox.EmptyBoxId
                        };
                        repo.EmptyBoxRepository.InsertOrUpdateEmptyBoxBarcode(BarcodeBox);
                        repo.EmptyBoxRepository.Save();

                    }

                }
                SendMailEmptyBoxClientMail(clientId);

                ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
                ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(clientId);
                ViewBag.PossibleOperator = repo.ORBLOperatorRepository.AllIncluding();
                ViewBag.Flag = 1;
                return View(emptybox);
            }

            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(clientId);
            ViewBag.PossibleOperator = repo.ORBLOperatorRepository.AllIncluding();
            ViewBag.Flag = 0;
            return View(emptybox);
        }
        
        //
        // GET: /EmptyBoxes/Edit/5
        //SHUVO
        private void SendMailEmptyBoxClientMail(long clientId)
        {
            WareHouseMVC.HelperClasses.MailHelper mailHelper = new HelperClasses.MailHelper();

            #region Mail Method for Admin

            string MailSubject = "New Empty Box Requisition has been created on WHMS Client Portal!";
            string MailBody = "Dear Admin,<br/>";
            MailBody = MailBody + "An user has created a new Empty Box Requisition on WHMS Client Portal. <br/>";
            MailBody = MailBody + "Here is the Empty Box Requisition URL : http://119.148.18.172/EmptyBoxes/EmptyBoxClient <br/>";
            MailBody = MailBody + "<br/> Thank You.<br/>" + System.Environment.NewLine;

            string adminMail = ConfigurationManager.AppSettings["AdminMailAddress"].ToString();
            string ccMail = ConfigurationManager.AppSettings["CCMailAddress"].ToString();

            mailHelper.SendMail(adminMail, MailSubject, MailBody, ccMail);
            #endregion

            #region Mail Method for User

            string MailSubjectUser = "Empty Box Requisition has been submitted.";
            string MailBodyUser = "Dear User,<br/>";
            MailBodyUser += "You have successfully created a new Empty Box Requisition on WHMS Client Portal. <br/>";
            MailBodyUser += "You can track the status of your requisition by visiting the following URL: http://119.148.18.172/EmptyBoxes/EmptyBoxClient <br/>";
            MailBodyUser += "<br/> Thank you for using WHMS.<br/>" + System.Environment.NewLine;

            Guid userId = (Guid)System.Web.HttpContext.Current.Session["UserId"];
            string UserMail = repo.UserRepository.GetByUserId(userId).Email;

            mailHelper.SendMail(UserMail, MailSubjectUser, MailBodyUser);
            #endregion
        }


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

            EmptyBox emptybox = repo.EmptyBoxRepository.Find(id);
            ViewBag.id = id;

            ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(clientId).Where(dept => !dept.DepartmentName.Equals("superadmin", StringComparison.OrdinalIgnoreCase)).ToList();
            ViewBag.DeptName = repo.DepartmentRepository.GetDepartmentByUser(userId).DepartmentName;
            var subDept = repo.DepartmentRepository.GetSubDepartmentName(userId);
            ViewBag.SubDept = string.IsNullOrWhiteSpace(subDept) ? "" : subDept;
            ViewBag.PossibleOperator = repo.ORBLOperatorRepository.AllIncluding();
            ViewBag.ClientName = repo.ClientRepository.Find(clientId).ClientName;
            ViewBag.ClientId = clientId;
            ViewBag.IsEdit = 1;

            return View(emptybox);

        }

        //
        // POST: /EmptyBoxes/Edit/5

        [HttpPost]
        public ActionResult Edit(EmptyBox emptybox)
        {
            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }

            if (System.Web.HttpContext.Current.Session["UserId"] != null)
            {
                userId = (Guid)System.Web.HttpContext.Current.Session["UserId"];
            }

            ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(clientId).Where(dept => !dept.DepartmentName.Equals("superadmin", StringComparison.OrdinalIgnoreCase)).ToList();
            ViewBag.DeptName = repo.DepartmentRepository.GetDepartmentByUser(userId).DepartmentName;
            var subDept = repo.DepartmentRepository.GetSubDepartmentName(userId);
            ViewBag.SubDept = string.IsNullOrWhiteSpace(subDept) ? "" : subDept;
            ViewBag.PossibleOperator = repo.ORBLOperatorRepository.AllIncluding();
            ViewBag.ClientName = repo.ClientRepository.Find(clientId).ClientName;
            ViewBag.ClientId = clientId;
            ViewBag.IsEdit = 1;

            EmptyBox ebox = repo.EmptyBoxRepository.Find(emptybox.EmptyBoxId);
            emptybox.EmptyBoxNo = ebox.EmptyBoxNo;

            repo.EmptyBoxRepository.InsertOrUpdate(emptybox);
            repo.EmptyBoxRepository.Save();

            if (emptybox.NoofBoxes > 0)
            {
                BarCodeRepository barcodeRef = new BarCodeRepository();
                for (int i = 0; i < emptybox.NoofBoxes; i++)
                {
                    BarCode BarCodeObj = barcodeRef.GetBarcodeTxtEmptyBoxes(emptybox.EmptyBoxId);

                    EmptyBoxBarcode BarcodeBox = new EmptyBoxBarcode()
                    {
                        BarCodeText = BarCodeObj.BoxName,
                        BarCodeId = BarCodeObj.BarCodeId,
                        EmptyBoxId = emptybox.EmptyBoxId
                    };
                    repo.EmptyBoxRepository.InsertOrUpdateEmptyBoxBarcode(BarcodeBox);
                    repo.EmptyBoxRepository.Save();

                }

            }

            return View(emptybox);
        }

        //
        // GET: /EmptyBoxes/Delete/5
 
        public ActionResult Delete(long id)
        {
            EmptyBox emptybox = repo.EmptyBoxRepository.Find(id);
            return View(emptybox);
        }

        //
        // POST: /EmptyBoxes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            EmptyBox emptybox = repo.EmptyBoxRepository.Find(id);
            repo.EmptyBoxRepository.DeleteEmptyBoxBarcode(id);
            repo.EmptyBoxRepository.Save();
            repo.EmptyBoxRepository.Delete(id);
            repo.EmptyBoxRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            //if (disposing) {
            //    context.Dispose();
            //}
            base.Dispose(disposing);
        }
    }
}