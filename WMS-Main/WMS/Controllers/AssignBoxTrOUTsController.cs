using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;
using System.Globalization;
using PagedList;
using System.IO;
using System.Configuration;
using System.Data.OleDb;

namespace WareHouseMVC.Controllers
{
    public class AssignBoxTrOUTsController : Controller
    {
        //private WareHouseMVCContext context = new WareHouseMVCContext();
        UnitOfWork repo = new UnitOfWork();

        public IPagedList<AssignBoxTrOUT> pagedList { get; set; }  
        //
        // GET: /AssignBoxTrOUTs/

        public ViewResult Index(string TransmittalOutNo)
        {


            List<long> TrIDList = new List<long>();
            List<AssignBoxTrViewModel> AssignViewList = new List<AssignBoxTrViewModel>();

            User _user = repo.UserRepository.GetUserByUserName(HttpContext.User.Identity.Name);
            long _wID = 0;
            if (_user.WarehouseID.HasValue)
            {
                _wID = _user.WarehouseID.Value;
            }


            //TrIDList = repo.AssignBoxTrOUTRepository.GetByByWidandStatus(_wID);
            TrIDList = repo.AssignBoxTrOUTRepository.GetCustomTOUTID(_wID, 100, TransmittalOutNo);

            foreach (long trID in TrIDList)
            {
                AssignBoxTrViewModel viewModel = new AssignBoxTrViewModel();
                viewModel.TransmittalOUTId = trID;
                viewModel.TotalBox = repo.AssignBoxTrOUTRepository.GetByTrInIdCount(trID);
                viewModel.TransmittalNo = repo.TransmittalOUTRepository.Find(trID).TransmittalNo;
                viewModel.Status = repo.AssignBoxTrOUTRepository.GetByTrInIdStatus(trID);
                viewModel.TransmittalDate = repo.TransmittalOUTRepository.Find(trID).TransmittalDate;
                //  = repo.AssignBoxRepository.GetByTrID(trID);


                AssignViewList.Add(viewModel);
            }
            List<AssignBoxTrViewModel> AssignViewList2 = AssignViewList.OrderByDescending(d => d.TransmittalDate).ToList();


            return View(AssignViewList2);

            //return View(repo.AssignBoxTrOUTRepository.AllIncluding(assignboxtrout => assignboxtrout.TransmittalOUT).Include(assignboxtrout => assignboxtrout.TransmittalOUTStatus).Include(assignboxtrout => assignboxtrout.Warehouse).Include(assignboxtrout => assignboxtrout.Item).ToList());
        }

        //
        // GET: /AssignBoxTrOUTs/Details/5


        public ActionResult ShowBoxes(int? page,long trID)
        {

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            User _user = repo.UserRepository.GetUserByUserName(HttpContext.User.Identity.Name);
            long _wID = 0;
            if (_user.WarehouseID.HasValue)
            {
                _wID = _user.WarehouseID.Value;
            }


            // List<AssignBox> assignBoxList = repo.AssignBoxRepository.GetByTrInId(trID);
            List<AssignBoxTrOUT> assignBoxList = new List<AssignBoxTrOUT>();
            pagedList = new PagedList<AssignBoxTrOUT>(assignBoxList, pageNumber, pageSize);

            pagedList = repo.AssignBoxTrOUTRepository.GetByTrInIdandWIDPagedList(trID, _wID,pageNumber,pageSize);
           // List<AssignBoxTrOUT> assignBoxList = repo.AssignBoxTrOUTRepository.GetByTrInId(trID);
            ViewBag.trID = trID;
            foreach (AssignBoxTrOUT item in assignBoxList)
            {
                ViewBag.Show = item.TransmittalOUTStatusId;

            }

            TransmittalOUT trOUT = repo.TransmittalOUTRepository.Find(trID);


            if (trOUT.Type == "File")
            {

                ViewBag.File = "1";
            }
            if (trOUT.Type == "Box")
            {

                ViewBag.File = "0";
            }


            ViewBag.Flag = trOUT.TransmittalOUTStatusId;

            return View(pagedList);
        }

        [HttpPost]
        public ActionResult CheckStatus(HttpPostedFileBase filename,int?page,long trID)
        {


            int pageSize = 10;
            int pageNumber = (page ?? 1);

            TransmittalOUT trOUT = repo.TransmittalOUTRepository.Find(trID);

            if (trOUT.Type == "File")
            {

                ViewBag.File = "1";
            }
            if (trOUT.Type == "Box")
            {

                ViewBag.File = "0";
            }

            ViewBag.Flag = trOUT.TransmittalOUTStatusId;
            ViewBag.Flag2 = 1;

            User _user = repo.UserRepository.GetUserByUserName(HttpContext.User.Identity.Name);
            long _wID = 0;
            if (_user.WarehouseID.HasValue)
            {
                _wID = _user.WarehouseID.Value;
            }

            List<Item> _itemsFound = new List<Item>();
            List<Item> _itemsNotFound = new List<Item>();
            List<long> _itemIdsforIN = new List<long>();
            List<long> _itemIdsforOUT = new List<long>();
            List<AssignBoxTrOUT> _assignboxTrList = repo.AssignBoxTrOUTRepository.GetByTrInIdAndStatus(trID, Convert.ToInt64(EnumHelper.TrOUTStatus.TrOUT_Generated));


            foreach (AssignBoxTrOUT item in _assignboxTrList)
            {
                _itemIdsforOUT.Add(item.ItemId);
            }

            List<AssignBox> _assignBoxList = new List<AssignBox>();


            if (filename != null && filename.ContentLength > 0)
            {
                // extract only the fielname
                string _name = Path.GetFileName(filename.FileName);
                _name = DateTime.Now.Ticks.ToString() + _name;
                //// store the file inside ~/App_Data/uploads folder
                //var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                var path = Path.Combine(Server.MapPath("~/Content/Images/") + ConfigurationManager.AppSettings["UploadedDocument"].ToString() + "XLUpload/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }


                filename.SaveAs(Path.Combine(path, _name));
                string Extension = Path.GetExtension(filename.FileName);

                path = path + _name;

               _assignBoxList = Import_To_Grid(path, Extension,_wID);

                try
                {
                    System.IO.File.Delete(path);
                }
                catch (Exception e)
                {
                }

            }


           // List<AssignBox> _assignBoxList = XLProcessing();
                
               // repo.AssignBoxRepository.GetByByWidandTrStatus(_wID, Convert.ToInt64(EnumHelper.Status.WareHouse_Assigned));

            foreach (AssignBox item in _assignBoxList)
            {
                _itemIdsforIN.Add(item.ItemId);

            }

            foreach (long itemID in _itemIdsforOUT)
            {
                if (_itemIdsforIN.Contains(itemID))
                {
                    Item _item = repo.ItemRepository.Find(itemID);
                    _itemsFound.Add(_item);
                }
                else
                {
                    Item _item2 = repo.ItemRepository.Find(itemID);
                    _itemsNotFound.Add(_item2);
                }


            }


            ViewBag.FoundItems = _itemsFound;
            ViewBag.NotFoundItems = _itemsNotFound;

            List<AssignBoxTrOUT> assignBoxList = new List<AssignBoxTrOUT>();
            pagedList = new PagedList<AssignBoxTrOUT>(assignBoxList, pageNumber, pageSize);
            
            

            pagedList = repo.AssignBoxTrOUTRepository.GetByTrInIdPagedList(trID, pageNumber, pageSize);

            ViewBag.trID = trID;
            foreach (AssignBoxTrOUT item in pagedList)
            {
                ViewBag.Show = item.TransmittalOUTStatusId;

            }

            ViewBag.Pager = "1";

            return View("ShowBoxes", pagedList);
        }

        private List<AssignBox> Import_To_Grid(string FilePath, string Extension,long wID)
        {

            List<AssignBox> list = new List<AssignBox>();

            string conStr = "";

            switch (Extension)
            {

                case ".xls": //Excel 97-03

                    conStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FilePath + ";Extended Properties=Excel 8.0;Persist Security Info=False";

                    break;

                case ".xlsx": //Excel 07

                    conStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath + ";Extended Properties=Excel 12.0;Persist Security Info=False";

                    break;

            }





            conStr = String.Format(conStr, FilePath);

            OleDbConnection connExcel = new OleDbConnection(conStr);


            OleDbCommand cmdExcel = new OleDbCommand();

            OleDbDataAdapter oda = new OleDbDataAdapter();

            DataTable dt = new DataTable();

            cmdExcel.Connection = connExcel;



            //Get the name of First Sheet

            connExcel.Open();

            DataTable dtExcelSchema;

            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();





            //Read Data from First Sheet

            //connExcel.Open();

            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";

            oda.SelectCommand = cmdExcel;

            oda.Fill(dt);
            if (dt.Rows.Count <= 0)
            {
                SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";

                oda.SelectCommand = cmdExcel;

                oda.Fill(dt);
            }
            //connExcel.Close();
            connExcel.Close();



            try
            {
              list=  SaveData(dt,wID);
               
            }
            catch (Exception ex)
            {
               
                // ShowSuccessResult("invalid file formate");
            }

            return list;
        }

        private List<AssignBox> SaveData(DataTable dt,long _wID)
        {
            List<AssignBox> list = new List<AssignBox>();
            foreach (DataRow dr in dt.Rows)
            {
                #region Get All Values From XL
                string BarcodeText = dr["Barcode Text"].ToString();

              //  long AssignBoxId = Convert.ToInt64(BarcodeText) / 5000;



                long itemId = Convert.ToInt64(BarcodeText) / 5000; //TODO
                AssignBox _assignBox = new AssignBox();
               
                List<AssignBox> aBoxList = new List<AssignBox>();

                aBoxList = repo.AssignBoxRepository.CheckForBarcodeSingle(itemId);//.AssignBoxes.Where(a => a.ItemId == _itemID)

                long aId = aBoxList[0].AssignBoxId;



                _assignBox = repo.AssignBoxRepository.GetByWidandTrStatus(aId, _wID, Convert.ToInt64(EnumHelper.Status.WareHouse_Assigned));

                if (_assignBox != null)
                    list.Add(_assignBox);
                #endregion
            }

            return list;
        }

      


        public ActionResult ApprovedTrOUT(int? page,long trID)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            TransmittalOUT trOUT = repo.TransmittalOUTRepository.Find(trID);

            if (trOUT.Type == "File")
            {

                ViewBag.File = "1";
            }
            if (trOUT.Type == "Box")
            {

                ViewBag.File = "0";
            }


            trOUT.TransmittalOUTStatusId = repo.TransmittalOUTStatusRepository.Find(Convert.ToInt64(EnumHelper.TrOUTStatus.Approved_at_WareHouse)).TransmittalOUTStatusId;
            repo.TransmittalOUTRepository.InsertOrUpdate(trOUT);
            repo.TransmittalOUTRepository.Save();
            ViewBag.Flag = trOUT.TransmittalOUTStatusId;

            List<AssignBoxTrOUT> assignBoxList = new List<AssignBoxTrOUT>();
            pagedList = new PagedList<AssignBoxTrOUT>(assignBoxList, pageNumber, pageSize);
            pagedList = repo.AssignBoxTrOUTRepository.GetByTrInIdPagedList(trID, pageNumber, pageSize);
            ViewBag.trID = trID;
            foreach (AssignBoxTrOUT item in pagedList)
            {
                item.TransmittalOUTStatusId = repo.TransmittalOUTStatusRepository.Find(Convert.ToInt64(EnumHelper.TrOUTStatus.Approved_at_WareHouse)).TransmittalOUTStatusId;
                repo.AssignBoxTrOUTRepository.InsertOrUpdate(item);
                repo.AssignBoxTrOUTRepository.Save();
                ViewBag.Show = item.TransmittalOUTStatusId;

            }


            ViewBag.Pager = "3";
            return View("ShowBoxes", pagedList);
        }

        public ActionResult GatePass(int? page,long trID)
        {

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            TransmittalOUT trOUT = repo.TransmittalOUTRepository.Find(trID);

            if (trOUT.Type == "File")
            {

                ViewBag.File = "1";
            }
            if (trOUT.Type == "Box")
            {

                ViewBag.File = "0";
            }


            trOUT.TransmittalOUTStatusId = repo.TransmittalOUTStatusRepository.Find(Convert.ToInt64(EnumHelper.TrOUTStatus.Gate_Pass_Generated)).TransmittalOUTStatusId;
            repo.TransmittalOUTRepository.InsertOrUpdate(trOUT);
            repo.TransmittalOUTRepository.Save();
            ViewBag.Flag = trOUT.TransmittalOUTStatusId;


           // List<AssignBoxTrOUT> assignBoxList = repo.AssignBoxTrOUTRepository.GetByTrInId(trID);
            List<AssignBoxTrOUT> assignBoxList = new List<AssignBoxTrOUT>();
            pagedList = new PagedList<AssignBoxTrOUT>(assignBoxList, pageNumber, pageSize);
            pagedList = repo.AssignBoxTrOUTRepository.GetByTrInIdPagedList(trID, pageNumber, pageSize);


            ViewBag.trID = trID;
            foreach (AssignBoxTrOUT item in pagedList)
            {
                item.TransmittalOUTStatusId = repo.TransmittalOUTStatusRepository.Find(Convert.ToInt64(EnumHelper.TrOUTStatus.Gate_Pass_Generated)).TransmittalOUTStatusId;
                repo.AssignBoxTrOUTRepository.InsertOrUpdate(item);
                repo.AssignBoxTrOUTRepository.Save();
                ViewBag.Show = item.TransmittalOUTStatusId;

            }

            ViewBag.Pager = "4";

            return View("ShowBoxes", pagedList);

        }

        public ActionResult BoxOut(int? page,long trID)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            TransmittalOUT trOUT = repo.TransmittalOUTRepository.Find(trID);


            if (trOUT.Type == "File")
            {

                ViewBag.File = "1";
            }
            if (trOUT.Type == "Box")
            {

                ViewBag.File = "0";
            }


            trOUT.TransmittalOUTStatusId = repo.TransmittalOUTStatusRepository.Find(Convert.ToInt64(EnumHelper.TrOUTStatus.Box_Out_from_WareHouse)).TransmittalOUTStatusId;
            repo.TransmittalOUTRepository.InsertOrUpdate(trOUT);
            repo.TransmittalOUTRepository.Save();
            ViewBag.Flag = trOUT.TransmittalOUTStatusId;

            try
            {

                #region Update AssignBox Table,Row Table,Box Location

                foreach (Item item in trOUT.Items)
                {
                    AssignBox _assignBox = repo.AssignBoxRepository.GetByItemID(item.ItemId);

                    _assignBox.CurrentStatus = 0;
                    _assignBox.AssignDate = DateTime.Now;
                    repo.AssignBoxRepository.InsertOrUpdate(_assignBox);
                    repo.AssignBoxRepository.Save();

                    BoxLocation _boxLocation = new BoxLocation();
                    _boxLocation = repo.BoxLocationRepository.GetByAssignBoxId(_assignBox.AssignBoxId);

                    if (_boxLocation != null)
                    {
                        _boxLocation.CurrentStatus = "Box Out";
                        repo.BoxLocationRepository.InsertOrUpdate(_boxLocation);
                        repo.BoxLocationRepository.Save();

                        //Check Here For Pallet

                        if (_boxLocation.Row != null)
                        {

                            Row _row = new Row();

                            _row = repo.RowRepository.Find(_boxLocation.RowID.Value);
                            _row.IsAssigned = false;

                            repo.RowRepository.InsertOrUpdate(_row);
                            repo.RowRepository.Save();
                        }


                    }


                    DelPendingBoxModel delPending = new DelPendingBoxModel();
                    delPending = repo.DelPendingBoxModelRepository.GetBytrOutNoandClientIdandDeptIdandBoxNo(trOUT.TransmittalNo, item.ClientID, item.DepartmentID, item.BoxNo);

                    if (delPending != null)
                    {
                        repo.DelPendingBoxModelRepository.Delete(delPending.DelPendingBoxModelId);
                        repo.DelPendingBoxModelRepository.Save();
                    }


                }





                #endregion

            }
            catch (Exception ex)
            {
            }

           // List<AssignBoxTrOUT> assignBoxList = repo.AssignBoxTrOUTRepository.GetByTrInId(trID);

            List<AssignBoxTrOUT> assignBoxList = new List<AssignBoxTrOUT>();
            pagedList = new PagedList<AssignBoxTrOUT>(assignBoxList, pageNumber, pageSize);
            pagedList = repo.AssignBoxTrOUTRepository.GetByTrInIdPagedList(trID, pageNumber, pageSize);
            ViewBag.trID = trID;
            foreach (AssignBoxTrOUT item in pagedList)
            {
                item.TransmittalOUTStatusId = repo.TransmittalOUTStatusRepository.Find(Convert.ToInt64(EnumHelper.TrOUTStatus.Box_Out_from_WareHouse)).TransmittalOUTStatusId;
                repo.AssignBoxTrOUTRepository.InsertOrUpdate(item);
                repo.AssignBoxTrOUTRepository.Save();
                ViewBag.Show = item.TransmittalOUTStatusId;

            }



            //Audit Train

            HelperClasses.AuditTrailHelperClass auditHelper = new HelperClasses.AuditTrailHelperClass();
            TransmittalOUT transmittalout = repo.TransmittalOUTRepository.Find(trID);

            TransmittalOUTAuditTrail auditTrail = new TransmittalOUTAuditTrail();
            auditTrail = repo.TransmittalOUTAuditTrailRepository.GetByTransmittalNo(transmittalout.TransmittalNo);

            if (auditTrail != null)
            {

                auditTrail.BoxOutBy = User.Identity.Name;
                auditTrail.BoxOutIP = auditHelper.LocalIPAddress();
                auditTrail.BoxOutDate = DateTime.Now;
                auditTrail.BoxOutTime = DateTime.Now.ToString("h:mm tt", CultureInfo.InvariantCulture);
                repo.TransmittalOUTAuditTrailRepository.InsertOrUpdate(auditTrail);
                repo.TransmittalOUTAuditTrailRepository.Save();
            }

            //Audit Train
            ViewBag.Pager = "4";
            return View("ShowBoxes", pagedList);
        }

        public ViewResult Details(long id)
        {
            AssignBoxTrOUT assignboxtrout = repo.AssignBoxTrOUTRepository.Find(id);
            return View(assignboxtrout);
        }

        //
        // GET: /AssignBoxTrOUTs/Create

        public ActionResult Create()
        {
            ViewBag.PossibleTransmittalOUTs = repo.TransmittalOUTRepository.AllIncluding();
            ViewBag.PossibleTransmittalOUTStatus = repo.TransmittalOUTStatusRepository.AllIncluding();
            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            ViewBag.PossibleItems = repo.ItemRepository.AllIncluding();
            return View();
        }

        //
        // POST: /AssignBoxTrOUTs/Create

        [HttpPost]
        public ActionResult Create(AssignBoxTrOUT assignboxtrout)
        {
            if (ModelState.IsValid)
            {
                repo.AssignBoxTrOUTRepository.InsertOrUpdate(assignboxtrout);
                repo.AssignBoxTrOUTRepository.Save();
                return RedirectToAction("Index");
            }

            ViewBag.PossibleTransmittalOUTs = repo.TransmittalOUTRepository.AllIncluding();
            ViewBag.PossibleTransmittalOUTStatus = repo.TransmittalOUTStatusRepository.AllIncluding();
            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            ViewBag.PossibleItems = repo.ItemRepository.AllIncluding();
            return View(assignboxtrout);
        }

        //
        // GET: /AssignBoxTrOUTs/Edit/5

        public ActionResult Edit(long id)
        {
            AssignBoxTrOUT assignboxtrout = repo.AssignBoxTrOUTRepository.Find(id);
            ViewBag.PossibleTransmittalOUTs = repo.TransmittalOUTRepository.AllIncluding();
            ViewBag.PossibleTransmittalOUTStatus = repo.TransmittalOUTStatusRepository.AllIncluding();
            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            ViewBag.PossibleItems = repo.ItemRepository.AllIncluding();
            return View(assignboxtrout);
        }

        //
        // POST: /AssignBoxTrOUTs/Edit/5

        [HttpPost]
        public ActionResult Edit(AssignBoxTrOUT assignboxtrout)
        {
            if (ModelState.IsValid)
            {
                repo.AssignBoxTrOUTRepository.InsertOrUpdate(assignboxtrout);
                repo.AssignBoxTrOUTRepository.Save();
                return RedirectToAction("Index");
            }
            ViewBag.PossibleTransmittalOUTs = repo.TransmittalOUTRepository.AllIncluding();
            ViewBag.PossibleTransmittalOUTStatus = repo.TransmittalOUTStatusRepository.AllIncluding();
            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            ViewBag.PossibleItems = repo.ItemRepository.AllIncluding();
            return View(assignboxtrout);
        }

        //
        // GET: /AssignBoxTrOUTs/Delete/5

        public ActionResult Delete(long id)
        {
            AssignBoxTrOUT assignboxtrout = repo.AssignBoxTrOUTRepository.Find(id);
            return View(assignboxtrout);
        }

        //
        // POST: /AssignBoxTrOUTs/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            AssignBoxTrOUT assignboxtrout = repo.AssignBoxTrOUTRepository.Find(id);
            repo.AssignBoxTrOUTRepository.Delete(id);
            repo.TransmittalOUTStatusRepository.Save();
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