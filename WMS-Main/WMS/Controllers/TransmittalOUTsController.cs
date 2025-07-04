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
    public class TransmittalOUTsController : Controller
    {
        UnitOfWork repo = new UnitOfWork();
        public IPagedList<TransmittalOUT> pagedList { get; set; }  
        //
        // GET: /TransmittalOUTs/

        public ViewResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            List<TransmittalOUT> transmittalOUTList = new List<TransmittalOUT>();
            pagedList = new PagedList<TransmittalOUT>(transmittalOUTList, pageNumber, pageSize);

            pagedList = repo.TransmittalOUTRepository.GetAllPagedList(pageNumber, pageSize);
            return View(pagedList); //Include(transmittalout => transmittalout.HandOverBy)
        }

        public ViewResult IndexForBarcode(int? page,string trOUTNumber)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            List<TransmittalOUT> transmittalOUTList = new List<TransmittalOUT>();
            pagedList = new PagedList<TransmittalOUT>(transmittalOUTList, pageNumber, pageSize);

            if (!string.IsNullOrEmpty(trOUTNumber))
            {
                pagedList = repo.TransmittalOUTRepository.GetListByTrNo(pageNumber, pageSize, trOUTNumber);
            }
            else
            {
                pagedList = repo.TransmittalOUTRepository.GetAllPagedList(pageNumber, pageSize);
            }

            ViewBag.ClientList = repo.ClientRepository.AllIncluding();
            ViewBag.trOUTNumber = trOUTNumber;

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
                model.BarCodeText = GetBarcodeText(item);
                list.Add(model);
                
            }
            return View(list);
        }

        private string GetBarcodeText(Item item)
        {
            string rtnText = string.Empty;
            //AssignBox _box = new AssignBox();
            //_box = repo.AssignBoxRepository.GetByItemID(item.ItemId);

            rtnText = (item.ItemId * 5000).ToString();
            return rtnText;

        }


        //
        // GET: /TransmittalOUTs/Details/5

        public ViewResult Details(long id)
        {
            TransmittalOUT transmittalout = repo.TransmittalOUTRepository.Find(id);
            return View(transmittalout);
        }

        //
        // GET: /TransmittalOUTs/Create

        public ActionResult Create()
        {
            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.AllIncluding();
            ViewBag.PossibleContactPerson = repo.ContactPersonRepository.AllIncluding();
            ViewBag.PossibleProjects = repo.ProjectRepository.AllIncluding();
            ViewBag.PossibleTransmittalOUTStatus = repo.TransmittalOUTStatusRepository.AllIncluding();
            TransmittalOUT transmittalOut = new TransmittalOUT();
            transmittalOut.TransmittalNo = "This No will be generated";
            transmittalOut.CreateDate = DateTime.Now;

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
            transmittalout.TransmittalNo = "demo";
            transmittalout.CreateDate = DateTime.Now;

            TransmittalOUTStatus status = repo.TransmittalOUTStatusRepository.GetFirst();
            transmittalout.TransmittalOUTStatusId = status.TransmittalOUTStatusId;


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

                transmittalout.TransmittalNo = "ORBL-TrOut-" + DateTime.Now.Year.ToString() + "-" + id.ToString();
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
                return RedirectToAction("Edit", new { id = id });
            }

            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleContactPerson = repo.ContactPersonRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(transmittalout.ClientID);
            ViewBag.PossibleProjects = repo.ProjectRepository.AllIncluding();
            ViewBag.PossibleTransmittalOUTStatus = repo.TransmittalOUTStatusRepository.AllIncluding();
            return View(transmittalout);
        }

        //
        // GET: /TransmittalOUTs/Edit/5

        public ActionResult Edit(long id)
        {
            TransmittalOUT transmittalout = repo.TransmittalOUTRepository.Find(id);
            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleContactPerson = repo.ContactPersonRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.AllIncluding();
            ViewBag.PossibleProjects = repo.ProjectRepository.AllIncluding();
            ViewBag.PossibleTransmittalOUTStatus = repo.TransmittalOUTStatusRepository.AllIncluding();

            ViewBag.id = id;
            return View(transmittalout);
        }

        //
        // POST: /TransmittalOUTs/Edit/5

        [HttpPost]
        public ActionResult Edit(TransmittalOUT transmittalout)
        {
            if (ModelState.IsValid)
            {
                repo.TransmittalOUTRepository.InsertOrUpdate(transmittalout);
                repo.TransmittalOUTRepository.Save();
                return RedirectToAction("Edit", new { id = transmittalout.TransmittalOUTId });
            }
            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleContactPerson = repo.ContactPersonRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(transmittalout.ClientID);
            ViewBag.PossibleProjects = repo.ProjectRepository.AllIncluding();
            ViewBag.PossibleTransmittalOUTStatus = repo.TransmittalOUTStatusRepository.AllIncluding();
            return View(transmittalout);
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