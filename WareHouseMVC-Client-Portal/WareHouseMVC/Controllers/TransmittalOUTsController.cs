using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;
using System.IO;
using System.Configuration;
using System.Globalization;
using PagedList;

namespace WareHouseMVC.Controllers
{
    public class TransmittalOUTsController : BaseController
    {
        UnitOfWork repo = new UnitOfWork();
        public IPagedList<TransmittalOUT> pagedList { get; set; }
        WareHouseMVC.HelperClasses.MailHelper mailHelper = new HelperClasses.MailHelper();
        long clientId = 0;
        Guid userId = Guid.Empty;

        //
        // GET: /TransmittalOUTs/

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

            Department dept = repo.DepartmentRepository.GetDepartmentByUser(userId);
            long departmentId = dept.DepartmentID;

            string subDepartment = repo.DepartmentRepository.GetSubDepartmentName(userId);

            int pageSize = 50;
            int pageNumber = (page ?? 1);

            List<TransmittalOUT> transmittalOUTList = new List<TransmittalOUT>();
            pagedList = new PagedList<TransmittalOUT>(transmittalOUTList, pageNumber, pageSize);

            pagedList = repo.TransmittalOUTRepository.GetPagesListFilteredByDept(clientId, departmentId, subDepartment, pageNumber, pageSize);
            return View(pagedList); //Include(transmittalout => transmittalout.HandOverBy)
        }

        public ViewResult IndexForBarcode(int? page)
        {
            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }


            ViewBag.ClientName = repo.ClientRepository.Find(clientId).ClientName;
            ViewBag.ClientId = clientId;


            int pageSize = 50;
            int pageNumber = (page ?? 1);

            List<TransmittalOUT> transmittalOUTList = new List<TransmittalOUT>();
            pagedList = new PagedList<TransmittalOUT>(transmittalOUTList, pageNumber, pageSize);

            pagedList = repo.TransmittalOUTRepository.GetByClientIDPagesList(clientId, pageNumber, pageSize);
            return View(pagedList); //Include(transmittalout => transmittalout.HandOverBy)
        }

        public ActionResult ViewBoxBarcodes(long transMIttalOUTId)
        {
            TransmittalOUT _out = new TransmittalOUT();
            _out = repo.TransmittalOUTRepository.Find(transMIttalOUTId);

            List<TransmittalOUTBArcodeViewModel> list = new List<TransmittalOUTBArcodeViewModel>();

            foreach (var item in _out.Items)
            {
                TransmittalOUTBArcodeViewModel model = new TransmittalOUTBArcodeViewModel();
                model.BoxName = item.ItemName;
                model.BoxNo = item.BoxNo;
                model.ClientName = item.Client.ClientName;
                model.Dept = item.Department.DepartmentName;
                model.BarCodeText = item.BoxNo;
                list.Add(model);
                
            }
            return View(list);
        }

        private string GetBarcodeText(Item item)
        {
            string rtnText = string.Empty;
            AssignBox _box = new AssignBox();
            _box = repo.AssignBoxRepository.GetByItemID(item.ItemId);

            rtnText = (_box.AssignBoxId * 5000).ToString();
            return rtnText;

        }


        //
        // GET: /TransmittalOUTs/Details/5

        public ViewResult Details(long id)
        {
            TransmittalOUT transmittalout = repo.TransmittalOUTRepository.Find(id);
            return View(transmittalout);
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
        // GET: /TransmittalOUTs/Create

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

            ViewBag.PossibleTransmittalOUTStatus = repo.TransmittalOUTStatusRepository.AllIncluding();
            TransmittalOUT transmittalOut = new TransmittalOUT();
            transmittalOut.TransmittalNo = "This No will be generated";
            transmittalOut.CreateDate = DateTime.Now;
            transmittalOut.TransmittalDate = DateTime.Now;
            transmittalOut.Department = repo.DepartmentRepository.GetDepartmentByUser(userId);
            transmittalOut.DepartmentID = transmittalOut.Department.DepartmentID;
            ViewBag.DeptName = transmittalOut.Department.DepartmentName;

            string subDept = repo.DepartmentRepository.GetSubDepartmentName(userId);
            transmittalOut.SubDepartment = string.IsNullOrWhiteSpace(subDept) ? "" : subDept;
            ViewBag.SubDept = transmittalOut.SubDepartment;

            transmittalOut.IsPermanent = true;
            transmittalOut.IsUrgent = false;

            ViewBag.TransmittalNo = "This No will be generated";
            TransmittalOUTStatus status = repo.TransmittalOUTStatusRepository.GetFirst();
            transmittalOut.TransmittalOUTStatusId = status.TransmittalOUTStatusId;
            return View(transmittalOut);
        }

        //
        // POST: /TransmittalOUTs/Create

        [HttpPost]
        public ActionResult Create(TransmittalOUT transmittalout)
        {
            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }

            if (System.Web.HttpContext.Current.Session["UserId"] != null)
            {
                userId = (Guid)System.Web.HttpContext.Current.Session["UserId"];
            }

            transmittalout.TransmittalNo = "demo";
            transmittalout.CreateDate = DateTime.Now;

            TransmittalOUTStatus status = repo.TransmittalOUTStatusRepository.GetFirst();
            transmittalout.TransmittalOUTStatusId = status.TransmittalOUTStatusId;

            string transmittalTime = Request.Form["TransmittalTime"];
            TimeSpan time = TimeSpan.Parse(transmittalTime);
            transmittalout.TransmittalDate = transmittalout.TransmittalDate.Date.Add(time);

            string rearchiveTime = Request.Form["RearchiveTime"];
            if (!string.IsNullOrWhiteSpace(transmittalout.RearchiveDate.ToString()) && !string.IsNullOrWhiteSpace(rearchiveTime))
            {
                TimeSpan time2 = TimeSpan.Parse(rearchiveTime);
                transmittalout.RearchiveDate = transmittalout.TransmittalDate.Date.Add(time2);
            }

            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleTransmittalOUTStatus = repo.TransmittalOUTStatusRepository.AllIncluding();

            ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(clientId).Where(dept => !dept.DepartmentName.Equals("superadmin", StringComparison.OrdinalIgnoreCase)).ToList();
            ViewBag.DeptName = repo.DepartmentRepository.GetDepartmentByUser(userId).DepartmentName;
            var subDept = repo.DepartmentRepository.GetSubDepartmentName(userId);
            ViewBag.SubDept = string.IsNullOrWhiteSpace(subDept) ? "" : subDept;

            if (ModelState.IsValid)
            {
                transmittalout.CreateDate = DateTime.Now;
                transmittalout.FileUrl = "";

                if (transmittalout.IsFile == true)
                {
                    transmittalout.Type = "File";
                }
                if (transmittalout.IsFile == false)
                {
                    transmittalout.Type = "Box";
                }


                repo.TransmittalOUTRepository.InsertOrUpdate(transmittalout);
                repo.TransmittalOUTRepository.Save();
                long id = transmittalout.TransmittalOUTId;

                transmittalout.TransmittalNo = "ORBL-TrOut-CL-" + DateTime.Now.Year.ToString() + "-" + id.ToString();
                repo.TransmittalOUTRepository.InsertOrUpdate(transmittalout);
                repo.TransmittalINRepository.Save();



                //Audit Train

                HelperClasses.AuditTrailHelperClass auditHelper = new HelperClasses.AuditTrailHelperClass();
                TransmittalOUTAuditTrail auditTrail = new TransmittalOUTAuditTrail();
                auditTrail.CreateBy = User.Identity.Name;
                auditTrail.CreatorIP = auditHelper.LocalIPAddress();
                auditTrail.CreateDate = DateTime.Now;
                auditTrail.CreateTime = DateTime.Now.ToString("h:mm tt", CultureInfo.InvariantCulture);
                auditTrail.BoxOutDate = DateTime.Now;
                auditTrail.TransmittalOUTNo = transmittalout.TransmittalNo;
                repo.TransmittalOUTAuditTrailRepository.InsertOrUpdate(auditTrail);
                repo.TransmittalOUTAuditTrailRepository.Save();


                //Audit Train


                if (Request.Files[0].ContentType.ToString().ToLower().Contains("image"))
                {
                    var fileName = Path.GetFileName(Request.Files[0].FileName);



                    var path = Path.Combine(Server.MapPath("~/Content/Images/") + ConfigurationManager.AppSettings["UploadedDocument"].ToString() + "TransmittalOUT/" + transmittalout.TransmittalOUTId.ToString() + "/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    Request.Files[0].SaveAs(Path.Combine(path, fileName));
                    transmittalout.FileUrl = "Content/Images/" + ConfigurationManager.AppSettings["UploadedDocument"].ToString() + "TransmittalOUT/" + transmittalout.TransmittalOUTId.ToString() + "/" + fileName;
                    repo.TransmittalOUTRepository.InsertOrUpdate(transmittalout);
                    repo.TransmittalINRepository.Save();


                }

                //SendMailNewTransmitOutMail(clientId, transmittalout.TransmittalNo);
                //return RedirectToAction("Edit", new { id = id });

                return RedirectToRoute(new { controller = "HandOverBies", action = "HandOverByForTrOUT", trId = id });

            }

            return View(transmittalout);
        }
        //SHUVO
        private void SendMailNewTransmitOutMail(long clientId, string transmittaloutNumber)
        {
            #region Mail Method

            string MailSubject = "New Transmittal-OUT has been created on WHMS Client Portal!";
            string MailBody = "Dear Admin,<br/>";
            MailBody = MailBody + "User has created a new Transmittal-OUT on WHMS Client Portal. <br/>";
            MailBody = MailBody + "Transmittal No is : " + transmittaloutNumber + ". <br/>";
            MailBody = MailBody + "Here is the Transmittal-OUT URL : http://119.148.18.172/warehouse/TransmittalOUTs <br/>";
            MailBody = MailBody + "<br/> Thank You.<br/>" + System.Environment.NewLine;

            Client client = new Client();
            client = repo.ClientRepository.Find(clientId);

            mailHelper.SendMail(client.Email, MailSubject, MailBody);
            #endregion
        }
        //
        // GET: /TransmittalOUTs/Edit/5

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

            TransmittalOUT trout = repo.TransmittalOUTRepository.Find(id);
            ViewBag.id = id;
            ViewBag.ClientName = trout.Client;
            ViewBag.ClientId = trout.ClientID;
            ViewBag.CreateDate = trout.CreateDate;
            ViewBag.TransmittalINStatusId = trout.TransmittalOUTStatusId;

            ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(clientId).Where(dept => !dept.DepartmentName.Equals("superadmin", StringComparison.OrdinalIgnoreCase)).ToList();
            ViewBag.DeptName = repo.DepartmentRepository.GetDepartmentByUser(userId).DepartmentName;
            var subDept = repo.DepartmentRepository.GetSubDepartmentName(userId);
            ViewBag.SubDept = string.IsNullOrWhiteSpace(subDept) ? "" : subDept;

            return View(trout);
        }

        //
        // POST: /TransmittalOUTs/Edit/5

        [HttpPost]
        public ActionResult Edit(TransmittalOUT transmittalout)
        {
            TransmittalOUT trout = repo.TransmittalOUTRepository.Find(transmittalout.TransmittalOUTId);
            transmittalout.TransmittalNo = trout.TransmittalNo;
            transmittalout.CreateDate = trout.CreateDate;
            TransmittalOUTStatus status = trout.TransmittalOUTStatus;
            transmittalout.TransmittalOUTStatusId = trout.TransmittalOUTStatusId;

            repo.TransmittalOUTRepository.InsertOrUpdate(transmittalout);
            repo.TransmittalOUTRepository.Save();

            return RedirectToRoute(new { controller = "HandOverBies", action = "HandOverByForTrOUT", trId = trout.TransmittalOUTId });
        }


        public ActionResult DoneTrOUT(long trID)
        {

            TransmittalOUT trOUT = new TransmittalOUT();
            trOUT = repo.TransmittalOUTRepository.Find(trID);

            if (trOUT.Type == "File")
            {




                bool IsXL = false;
                List<AssignBox> finalList = (List<AssignBox>)Session["AssignBoxListForXLFile"];

                try
                {
                    IsXL = Convert.ToBoolean(Session["IsXLUploadFile"]);
                }
                catch
                {
                    IsXL = false;
                }

                if (IsXL == true)
                {

                    TransmittalOUT _trOUT2 = new TransmittalOUT();
                    _trOUT2 = repo.TransmittalOUTRepository.Find(trID);

                    foreach (AssignBox item2 in finalList)
                    {
                        Item _item = new Item();
                        _item = repo.ItemRepository.Find(item2.ItemId);
                        if (!_trOUT2.Items.Contains(_item))
                        {
                            DelPendingBoxModelFile model = new DelPendingBoxModelFile();
                            model.BoxNo = _item.BoxNo;
                            model.BoxNameFile=_item.FileBoxName;
                            model.FileNumber=_item.FileNumber;
                            model.ReferrenceNo=_item.ReferrenceNo;
                            model.RingNo=_item.RingNo;
                            model.AccountNo=_item.AccountNo;


                            model.ClientName = repo.ClientRepository.Find(_item.ClientID).ClientName;
                            model.Department = repo.DepartmentRepository.Find(_item.DepartmentID).DepartmentName;
                            model.TransmittalOutDate = _trOUT2.TransmittalDate;
                            model.TransmittalOutNo = _trOUT2.TransmittalNo;
                            model.Year = _item.Year;

                            repo.DelPendingBoxModelFileRepository.InsertOrUpdate(model);
                            repo.DelPendingBoxModelFileRepository.Save();
                        }
                    }


                    Session["IsXLUploadFile"] = false;

                }




                TransmittalOUT _trOUT = new TransmittalOUT();
                _trOUT = repo.TransmittalOUTRepository.Find(trID);

                foreach (Item item in _trOUT.Items)
                {
                    AssignBoxTrOUT _assignBoxTrOUT = repo.AssignBoxTrOUTRepository.GetByTrIDandItemId(trID, item.ItemId);
                    if (_assignBoxTrOUT == null)
                    {
                        _assignBoxTrOUT = new AssignBoxTrOUT();
                    }


                    _assignBoxTrOUT.AssignDate = _trOUT.TransmittalDate;
                    _assignBoxTrOUT.BoxNo = item.BoxNo;

                    _assignBoxTrOUT.BoxNameFile = item.FileBoxName;
                    _assignBoxTrOUT.FileNumber = item.FileNumber;
                    _assignBoxTrOUT.ReferrenceNo = item.ReferrenceNo;
                    _assignBoxTrOUT.RingNo = item.RingNo;
                    _assignBoxTrOUT.AccountNo = item.AccountNo;


                    _assignBoxTrOUT.DestructionPeriod = item.DestructionPeriod;
                    _assignBoxTrOUT.ItemId = item.ItemId;
                    _assignBoxTrOUT.TransmittalOUTId = _trOUT.TransmittalOUTId;
                    _assignBoxTrOUT.TransmittalOUTStatusId = repo.TransmittalOUTStatusRepository.Find(Convert.ToInt64(EnumHelper.TrOUTStatus.TrOUT_Generated)).TransmittalOUTStatusId;
                    _assignBoxTrOUT.WarehouseID = repo.AssignBoxRepository.GetByItemIdandStatus(item.ItemId, Convert.ToInt64(EnumHelper.Status.WareHouse_Assigned)).WarehouseID;
                    _assignBoxTrOUT.WarehouseName = repo.AssignBoxRepository.GetByItemIdandStatus(item.ItemId, Convert.ToInt64(EnumHelper.Status.WareHouse_Assigned)).WarehouseName;
                    repo.AssignBoxTrOUTRepository.InsertOrUpdate(_assignBoxTrOUT);
                    repo.AssignBoxTrOUTRepository.Save();

                }

                _trOUT.TransmittalOUTStatusId = repo.TransmittalOUTStatusRepository.Find(Convert.ToInt64(EnumHelper.TrOUTStatus.TrOUT_Generated)).TransmittalOUTStatusId;
                repo.TransmittalOUTRepository.InsertOrUpdate(_trOUT);
                repo.TransmittalOUTRepository.Save();


                #region Admin Mail Method
                string MailSubject = "New Transmittal-OUT has been created on WHMS Client Portal!";
                string MailBody = "Dear Admin,</br>";
                MailBody = MailBody + "An User has created a new Transmittal-OUT on WHMS Client Portal..Transmittal No is :" + _trOUT.TransmittalNo.ToString() + " </br>";
                MailBody = MailBody + "Here is the Transmittal-OUT URL : http://119.148.18.172/warehouse/TransmittalOUTs";
                MailBody = MailBody + "</br> Thank You.</br>" + System.Environment.NewLine;

                string adminMail = ConfigurationManager.AppSettings["AdminMailAddress"].ToString();
                string ccMail = ConfigurationManager.AppSettings["CCMailAddress"].ToString();

                mailHelper.SendMail(adminMail, MailSubject, MailBody, ccMail);
                #endregion

                #region User Mail
                string UserMailSubject = "Your Transmittal-OUT has been successfully created!";
                string UserMailBody = "Dear User,</br>";
                UserMailBody += "You have successfully created a new Transmittal-OUT on the WHMS Client Portal. The Transmittal No is: " + _trOUT.TransmittalNo.ToString() + " </br>";
                UserMailBody += "</br> Thank You.</br>" + System.Environment.NewLine;

                mailHelper.SendMail(_trOUT.Client.Email, UserMailSubject, UserMailBody);
                #endregion

            }

            if (trOUT.Type == "Box")
            {




                bool IsXL = false;
                List<AssignBox> finalList = (List<AssignBox>)Session["AssignBoxListForXL"];

                try
                {
                    IsXL = Convert.ToBoolean(Session["IsXLUpload"]);
                }
                catch
                {
                    IsXL = false;
                }

                if (IsXL == true)
                {

                    TransmittalOUT _trOUT2 = new TransmittalOUT();
                    _trOUT2 = repo.TransmittalOUTRepository.Find(trID);

                    foreach (AssignBox item2 in finalList)
                    {
                        Item _item = new Item();
                        _item = repo.ItemRepository.Find(item2.ItemId);
                        if (!_trOUT2.Items.Contains(_item))
                        {
                            DelPendingBoxModel model = new DelPendingBoxModel();
                            model.BoxName = _item.ItemName;
                            model.BoxNo = _item.BoxNo;
                            model.ClientName = repo.ClientRepository.Find(_item.ClientID).ClientName;
                            model.Department = repo.DepartmentRepository.Find(_item.DepartmentID).DepartmentName;
                            model.TransmittalOutDate = _trOUT2.TransmittalDate;
                            model.TransmittalOutNo = _trOUT2.TransmittalNo;
                            model.Year = _item.Year;
                            model.DepatID = _item.DepartmentID;
                            model.ClientID = _item.ClientID;

                            repo.DelPendingBoxModelRepository.InsertOrUpdate(model);
                            repo.DelPendingBoxModelRepository.Save();
                        }
                    }


                    Session["IsXLUpload"] = false;

                }



                TransmittalOUT _trOUT = new TransmittalOUT();
                _trOUT = repo.TransmittalOUTRepository.Find(trID);

                foreach (Item item in _trOUT.Items)
                {
                    AssignBoxTrOUT _assignBoxTrOUT = repo.AssignBoxTrOUTRepository.GetByTrIDandItemId(trID, item.ItemId);
                    if (_assignBoxTrOUT == null)
                    {
                        _assignBoxTrOUT = new AssignBoxTrOUT();
                    }


                    _assignBoxTrOUT.AssignDate = _trOUT.TransmittalDate;
                    _assignBoxTrOUT.BoxNo = item.BoxNo;
                    _assignBoxTrOUT.DestructionPeriod = item.DestructionPeriod;
                    _assignBoxTrOUT.ItemId = item.ItemId;
                    _assignBoxTrOUT.TransmittalOUTId = _trOUT.TransmittalOUTId;
                    _assignBoxTrOUT.TransmittalOUTStatusId = repo.TransmittalOUTStatusRepository.Find(Convert.ToInt64(EnumHelper.TrOUTStatus.TrOUT_Generated)).TransmittalOUTStatusId;
                    _assignBoxTrOUT.WarehouseID = repo.AssignBoxRepository.GetByItemIdandStatus(item.ItemId, Convert.ToInt64(EnumHelper.Status.WareHouse_Assigned)).WarehouseID;
                    _assignBoxTrOUT.WarehouseName = repo.AssignBoxRepository.GetByItemIdandStatus(item.ItemId, Convert.ToInt64(EnumHelper.Status.WareHouse_Assigned)).WarehouseName;
                    repo.AssignBoxTrOUTRepository.InsertOrUpdate(_assignBoxTrOUT);
                    repo.AssignBoxTrOUTRepository.Save();

                }

                _trOUT.TransmittalOUTStatusId = repo.TransmittalOUTStatusRepository.Find(Convert.ToInt64(EnumHelper.TrOUTStatus.TrOUT_Generated)).TransmittalOUTStatusId;
                repo.TransmittalOUTRepository.InsertOrUpdate(_trOUT);
                repo.TransmittalOUTRepository.Save();

                #region Mail Method

                string MailSubject = "New Transmittal-OUT has been created on WHMS Client Portal!";
                string MailBody = "Dear Admin,</br>";
                MailBody = MailBody + "An User has created a new Transmittal-OUT on WHMS Client Portal.Transmittal No is :"+_trOUT.TransmittalNo.ToString()+" </br>";
                MailBody = MailBody + "Here is the Transmittal-OUT URL : http://119.148.18.172/warehouse/TransmittalOUTs";
                MailBody = MailBody + "</br> Thank You.</br>" + System.Environment.NewLine;





                mailHelper.SendMail(_trOUT.Client.Email, MailSubject, MailBody);
                #endregion
            }

            return RedirectToAction("Preview", new { trid = trID });



        }

        public ActionResult Preview(long trid)
        {

            TransmittalOUT _trOUTForView = new TransmittalOUT();
            _trOUTForView = repo.TransmittalOUTRepository.Find(trid);

            ViewBag.TransmittalNo = _trOUTForView.TransmittalNo;
            ViewBag.TransmittalRefNo = _trOUTForView.ClientRequestreference;
            ViewBag.TransmittalType = "Transmittal OUT";
            ViewBag.TransmittalId = _trOUTForView.TransmittalOUTId;
            ViewBag.TransmittalDate = _trOUTForView.TransmittalDate.ToShortDateString();
            ViewBag.ClientName = repo.ClientRepository.Find(_trOUTForView.ClientID).ClientName;
            ViewBag.Department = repo.DepartmentRepository.Find(_trOUTForView.DepartmentID).DepartmentName;

            ViewBag.AllItems = _trOUTForView.Items;

            if (_trOUTForView.Type == "File")
            {

                ViewBag.File = "1";
            }
            if (_trOUTForView.Type == "Box")
            {

                ViewBag.File = "0";
            }


            return View(_trOUTForView);
        }


        //
        // GET: /TransmittalOUTs/Delete/5

        public ActionResult Delete(long id)
        {
            TransmittalOUT transmittalout = repo.TransmittalOUTRepository.Find(id);
            return View(transmittalout);
        }

        //
        // POST: /TransmittalOUTs/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            TransmittalOUT transmittalout = repo.TransmittalOUTRepository.Find(id);
            repo.TransmittalOUTRepository.Delete(id);
            repo.TransmittalOUTRepository.Save();
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