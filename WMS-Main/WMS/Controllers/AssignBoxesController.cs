using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;
using System.Configuration;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using PagedList;
using System.Data.OleDb;
using WareHouseMVC.HelperClasses;

namespace WareHouseMVC.Controllers
{
    public class AssignBoxesController : Controller
    {
        // private WareHouseMVCContext context = new WareHouseMVCContext();
        UnitOfWork repo = new UnitOfWork();
        public IPagedList<AssignBox> pagedList { get; set; }


        // GET: /AssignBoxes/
        public ViewResult Index(string TransmittalINNo)
        {

            List<long> TrIDList = new List<long>();
            List<AssignBox> AssignBoxList = new List<AssignBox>();
            List<AssignBoxViewModel> AssignViewList = new List<AssignBoxViewModel>();

            User _user = repo.UserRepository.GetUserByUserName(HttpContext.User.Identity.Name);
            long _wID = 0;
            if (_user.WarehouseID.HasValue)
            {
                _wID = _user.WarehouseID.Value;
            }

            //TrIDList = repo.AssignBoxRepository.GetByByWidandStatus(_wID);
            //TrIDList = repo.AssignBoxRepository.GetCustomTINID(_wID, 100);
            TrIDList = repo.AssignBoxRepository.GetCustomTINID(_wID, 100, TransmittalINNo);

            foreach (long trID in TrIDList)
            {
                AssignBoxViewModel viewModel = new AssignBoxViewModel();
                viewModel.TransmittalINId = trID;
                viewModel.TotalBox = repo.AssignBoxRepository.GetByTrInIdCount(trID);
                viewModel.TransmittalNo = repo.TransmittalINRepository.Find(trID).TransmittalNo;
                viewModel.Status = repo.AssignBoxRepository.GetByTrInIdSTatus(trID);
                viewModel.TransmittalDate = repo.TransmittalINRepository.Find(trID).TransmittalDate;
                //  = repo.AssignBoxRepository.GetByTrID(trID);

                AssignViewList.Add(viewModel);
            }

            return View(AssignViewList.OrderByDescending(d => d.TransmittalDate).ToList());
        }


        //GET: /AssignBoxes/Details/5
        public ActionResult ShowBoxes(int? page, long trID)
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

            List<AssignBox> assignBoxList = new List<AssignBox>();
            pagedList = new PagedList<AssignBox>(assignBoxList, pageNumber, pageSize);

            pagedList = repo.AssignBoxRepository.GetByTrInIdandWIDPagedList(trID, _wID, pageSize, pageNumber);
            ViewBag.trID = trID;

            TransmittalIN trIn = repo.TransmittalINRepository.Find(trID);

            long wID = 0;
            foreach (AssignBox item in pagedList)
            {
                ViewBag.Show = item.TransmittalINStatusId;
                ViewBag.WareHouse = item.WarehouseName;
                wID = item.WarehouseID;
            }


            ViewBag.TransmittalNo = repo.TransmittalINRepository.Find(trID).TransmittalNo;
            ViewBag.ClientName = repo.TransmittalINRepository.Find(trID).Client.ClientName;
            ViewBag.Department = repo.TransmittalINRepository.Find(trID).Department.DepartmentName;
            ViewBag.TransmittalDate = repo.TransmittalINRepository.Find(trID).TransmittalDate.ToShortDateString();

            string _emptySpace = repo.RowRepository.GetAllEmptySpace(wID);
            ViewBag.EmptySpace = _emptySpace;

            if (trIn.Type == "File")
            {
                ViewBag.File = "1";
            }
            if (trIn.Type == "Box")
            {
                ViewBag.File = "0";
            }


            // =repo.AssignBoxRepository.AllIncluding(assignbox => assignbox.TransmittalIN).Include(assignbox => assignbox.Warehouse).Include(assignbox => assignbox.Item).Include(assignbox => assignbox.TransmittalINStatus).Include(assignbox => assignbox.BoxLocation).Where(x=>x.TransmittalINId==trID).ToList();
            return View(pagedList);
        }



        public ActionResult ShowBoxesEmptySpace(long trID, int type)
        {
            //  = new List<AssignBox>();
            List<AssignBox> assignBoxList = repo.AssignBoxRepository.GetByTrInId(trID);
            ViewBag.trID = trID;
            long wID = 0;
            foreach (AssignBox item in assignBoxList)
            {
                ViewBag.Show = item.TransmittalINStatusId;

                ViewBag.WareHouse = item.WarehouseName;
                wID = item.WarehouseID;
            }

            if (type == 1)
            {
                ViewBag.EmptyStatus = 1;
            }

            if (type == 2)
            {
                ViewBag.EmptyStatus = 2;
            }

            ViewBag.TransmittalNo = repo.TransmittalINRepository.Find(trID).TransmittalNo;
            ViewBag.ClientName = repo.TransmittalINRepository.Find(trID).Client.ClientName;
            ViewBag.Department = repo.TransmittalINRepository.Find(trID).Department.DepartmentName;
            ViewBag.TransmittalDate = repo.TransmittalINRepository.Find(trID).TransmittalDate.ToShortDateString();




            string _emptySpace = repo.RowRepository.GetAllEmptySpace(wID);
            ViewBag.EmptySpace = _emptySpace;


            // =repo.AssignBoxRepository.AllIncluding(assignbox => assignbox.TransmittalIN).Include(assignbox => assignbox.Warehouse).Include(assignbox => assignbox.Item).Include(assignbox => assignbox.TransmittalINStatus).Include(assignbox => assignbox.BoxLocation).Where(x=>x.TransmittalINId==trID).ToList();
            return View("ShowBoxes", assignBoxList);
        }


        public ActionResult Receive(long id)
        {
            AssignBox assignBox = repo.AssignBoxRepository.Find(id);
            assignBox.TransmittalINStatusId = repo.TransmittalINStatusRepository.Find(Convert.ToInt64(EnumHelper.Status.Received_at_WareHouse)).TransmittalINStatusId;

            repo.AssignBoxRepository.InsertOrUpdate(assignBox);
            repo.AssignBoxRepository.Save();


            AssignBox IsPending = repo.AssignBoxRepository.GetStatusByTrID(assignBox.TransmittalINId, assignBox.WarehouseID, Convert.ToInt64(EnumHelper.Status.WareHouse_Assigned));
            if (IsPending == null)
            {
                TransmittalIN trIN = repo.TransmittalINRepository.Find(assignBox.TransmittalINId);

                trIN.TransmittalINStatusId = repo.TransmittalINStatusRepository.Find(Convert.ToInt64(EnumHelper.Status.Received_at_WareHouse)).TransmittalINStatusId;

                repo.TransmittalINRepository.InsertOrUpdate(trIN);
                repo.TransmittalINRepository.Save();


            }

            else
            {
                ViewBag.Show = 2;
            }


            return RedirectToAction("ShowBoxes", new { trID = assignBox.TransmittalINId });
        }

        public ActionResult ReceiveAll(long TrID)
        {
            User _user = repo.UserRepository.GetUserByUserName(HttpContext.User.Identity.Name);
            long _wID = 0;
            if (_user.WarehouseID.HasValue)
            {
                _wID = _user.WarehouseID.Value;
            }
            List<AssignBox> assignBoxList = new List<AssignBox>();
            assignBoxList = repo.AssignBoxRepository.GetByTrInIdAndWid(TrID, _wID);

            //long wHouseCapacity = repo.RowRepository.GetFreeCapacity(_wID);

            //if (wHouseCapacity >= assignBoxList.Count)
            //{

            foreach (AssignBox item in assignBoxList)
            {
                item.TransmittalINStatusId = repo.TransmittalINStatusRepository.Find(Convert.ToInt64(EnumHelper.Status.Received_at_WareHouse)).TransmittalINStatusId;

                repo.AssignBoxRepository.InsertOrUpdate(item);
                repo.AssignBoxRepository.Save();

                ViewBag.Show = item.TransmittalINStatusId;
                AssignBox IsPending = repo.AssignBoxRepository.GetStatusByTrID(item.TransmittalINId, item.WarehouseID, Convert.ToInt64(EnumHelper.Status.WareHouse_Assigned));
                if (IsPending == null)
                {
                    TransmittalIN trIN = repo.TransmittalINRepository.Find(item.TransmittalINId);

                    trIN.TransmittalINStatusId = repo.TransmittalINStatusRepository.Find(Convert.ToInt64(EnumHelper.Status.Received_at_WareHouse)).TransmittalINStatusId;
                    repo.TransmittalINRepository.InsertOrUpdate(trIN);
                    repo.TransmittalINRepository.Save();
                }
            }


            //Audit Train

            HelperClasses.AuditTrailHelperClass auditHelper = new HelperClasses.AuditTrailHelperClass();
            TransmittalIN transmittalin = repo.TransmittalINRepository.Find(TrID);

            TransmittalINAuditTrail auditTrail = new TransmittalINAuditTrail();
            auditTrail = repo.TransmittalINAuditTrailRepository.GetByTransmittalNo(transmittalin.TransmittalNo);

            if (auditTrail != null)
            {

                auditTrail.BoxReceivedBy = User.Identity.Name;
                auditTrail.BoxReceivedByIP = auditHelper.LocalIPAddress();
                auditTrail.BoxReceivedByDate = DateTime.Now;
                auditTrail.BoxReceivedByTime = DateTime.Now.ToString("h:mm tt", CultureInfo.InvariantCulture);
                repo.TransmittalINAuditTrailRepository.InsertOrUpdate(auditTrail);
                repo.TransmittalINAuditTrailRepository.Save();
            }

            //Audit Train
            //}

            //else
            //{
            //    TempData["emptyError"] = "NoEmptySpace";
            //}


            return RedirectToAction("ShowBoxes", new { trID = TrID });
        }

         
        public ActionResult SetBarcodeAll(long TrID)
        {
            SessionManager.SessionValueItem.BarcodeServerAddress = Server.MapPath("~/Content/Images/" + ConfigurationManager.AppSettings["BarCodes"].ToString());//

            User _user = repo.UserRepository.GetUserByUserName(HttpContext.User.Identity.Name);
            long _wID = 0;
            if (_user.WarehouseID.HasValue)
            {
                _wID = _user.WarehouseID.Value;
            }

            List<AssignBox> assignBoxList = new List<AssignBox>();
            assignBoxList = repo.AssignBoxRepository.GetByTrInIdAndWidandStatus(TrID, _wID, Convert.ToInt64(EnumHelper.Status.Received_at_WareHouse));

            foreach (AssignBox item in assignBoxList)
            {


                AssignBox _assignBox = repo.AssignBoxRepository.Find(item.AssignBoxId);

                //  _assignBox.BarCodeId = _existingBarCode.BarCodeId;
                _assignBox.TransmittalINStatusId = repo.TransmittalINStatusRepository.Find(Convert.ToInt64(EnumHelper.Status.Barcode_Assigned)).TransmittalINStatusId;

                repo.AssignBoxRepository.InsertOrUpdate(_assignBox);
                repo.AssignBoxRepository.Save();

                ViewBag.Show = _assignBox.TransmittalINStatusId;




                AssignBox IsPending = repo.AssignBoxRepository.GetStatusByTrID(item.TransmittalINId, item.WarehouseID, Convert.ToInt64(EnumHelper.Status.Received_at_WareHouse));
                if (IsPending == null)
                {
                    TransmittalIN trIN = repo.TransmittalINRepository.Find(item.TransmittalINId);

                    trIN.TransmittalINStatusId = repo.TransmittalINStatusRepository.Find(Convert.ToInt64(EnumHelper.Status.Barcode_Assigned)).TransmittalINStatusId;
                    repo.TransmittalINRepository.InsertOrUpdate(trIN);
                    repo.TransmittalINRepository.Save();
                }

            }




            return RedirectToAction("ShowBoxes", new { trID = TrID });
        }

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
        }



        [HttpPost]
        public ActionResult VerifyBarcode(HttpPostedFileBase filename, long TrID)
        {

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

                bool flag = Import_To_Grid(path, Extension, TrID);

                try
                {
                    System.IO.File.Delete(path);
                }
                catch (Exception e)
                {
                }


                TransmittalIN transmittalin = repo.TransmittalINRepository.Find(TrID);

                TransmittalINAuditTrail auditTrail = new TransmittalINAuditTrail();
                auditTrail = repo.TransmittalINAuditTrailRepository.GetByTransmittalNo(transmittalin.TransmittalNo);

                if (auditTrail != null)
                {

                    auditTrail.BarcodeVerifiedBy = User.Identity.Name;
                    repo.TransmittalINAuditTrailRepository.InsertOrUpdate(auditTrail);
                    repo.TransmittalINAuditTrailRepository.Save();
                }

                TempData["Flag"] = flag;
            }


            return RedirectToAction("ShowBoxes", new { trID = TrID });
        }

        private bool Import_To_Grid(string FilePath, string Extension, long trID)
        {

            bool flag = false;

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
                flag = SaveData(dt, trID);
            }
            catch (Exception ex)
            {
                flag = false;
                // ShowSuccessResult("invalid file formate");
            }

            return flag;
        }

        private bool SaveData(DataTable dt, long TrID)
        {
            List<Item> itemList = new List<Item>();
            bool flag = false;
            foreach (DataRow dr in dt.Rows)
            {
                try
                {

                    #region Get All Values From XL
                    string BarcodeID = dr["Barcode Text"].ToString();

                    long itemId = Convert.ToInt64(BarcodeID) / 5000; //TODO
                    AssignBox _assignBox = new AssignBox();
                    List<AssignBox> aBoxList = new List<AssignBox>();

                    aBoxList = repo.AssignBoxRepository.CheckForBarcodeSingle(itemId);//.AssignBoxes.Where(a => a.ItemId == _itemID)

                    long aId = aBoxList[0].AssignBoxId;
                    // _assignBox = repo.AssignBoxRepository.Find(aId);//(_item.ItemId, Convert.ToInt64(EnumHelper.Status.Barcode_Assigned));




                    _assignBox = repo.AssignBoxRepository.Find(aId);

                    _assignBox.TransmittalINStatusId = repo.TransmittalINStatusRepository.Find(Convert.ToInt64(EnumHelper.Status.Barcode_Verified)).TransmittalINStatusId;

                    repo.AssignBoxRepository.InsertOrUpdate(_assignBox);
                    repo.AssignBoxRepository.Save();

                    ViewBag.Show = _assignBox.TransmittalINStatusId;
                    AssignBox IsPending = repo.AssignBoxRepository.GetStatusByTrID(_assignBox.TransmittalINId, _assignBox.WarehouseID, Convert.ToInt64(EnumHelper.Status.Barcode_Assigned));
                    if (IsPending == null)
                    {
                        TransmittalIN trIN = repo.TransmittalINRepository.Find(_assignBox.TransmittalINId);

                        trIN.TransmittalINStatusId = repo.TransmittalINStatusRepository.Find(Convert.ToInt64(EnumHelper.Status.Barcode_Verified)).TransmittalINStatusId;
                        repo.TransmittalINRepository.InsertOrUpdate(trIN);
                        repo.TransmittalINRepository.Save();
                    }



                    #endregion

                    flag = true;
                }
                catch
                {
                    flag = false;
                }
            }

            return flag;
        }

        public ActionResult BarcodeMappingMethod(long TrID, int? page)
        {
            int pageNumber = (page ?? 1);
            TransmittalIN _trIN = new TransmittalIN();
            _trIN = repo.TransmittalINRepository.Find(TrID);
            BarcodeMapping _mappingTbl = new BarcodeMapping();
            _mappingTbl = repo.BarcodeMappingRepository.GetByTransmittalNo(_trIN.TransmittalNo);

            if (_mappingTbl == null)
            {

                #region New Entry If Not Exists

                User _user = repo.UserRepository.GetUserByUserName(HttpContext.User.Identity.Name);
                long _wID = 0;
                if (_user.WarehouseID.HasValue)
                {
                    _wID = _user.WarehouseID.Value;
                }

                List<AssignBox> assignBoxList = new List<AssignBox>();
                assignBoxList = repo.AssignBoxRepository.GetByTrInIdAndWid(TrID, _wID);
                repo.BarCodeRepository.GetBarCodeReport(TrID, _wID, Convert.ToInt64(EnumHelper.Status.Barcode_Assigned), page);

                foreach (AssignBox item in assignBoxList)
                {

                    #region BarcodeMapping Table Insertion

                    BarCode _existingBarCode = repo.BarCodeRepository.GetByAssignBoxId(item.AssignBoxId);


                    BarcodeMapping _mapping = new BarcodeMapping();
                    _mapping = repo.BarcodeMappingRepository.GetByTransmittalNoandBarcodeIdandItemId(item.TransmittalIN.TransmittalNo, _existingBarCode.BarCodeId, item.ItemId);

                    if (_mapping == null)
                        _mapping = new BarcodeMapping();

                    _mapping.BarCodeId = _existingBarCode.BarCodeId;
                    _mapping.BoxName = item.BoxName;
                    _mapping.BoxNo = item.BoxNo;
                    _mapping.ClientName = _existingBarCode.ClientName;
                    _mapping.DeptName = _existingBarCode.DeptName;
                    _mapping.ImagePath = _existingBarCode.ImagePathAbs;
                    _mapping.ItemId = item.ItemId;
                    _mapping.TransmittalNo = item.TransmittalIN.TransmittalNo;
                    _mapping.Year = item.Year;

                    repo.BarcodeMappingRepository.InsertOrUpdate(_mapping);
                    repo.BarcodeMappingRepository.Save();

                    #endregion
                }

                #endregion

                #region Forward To Report


                return RedirectToAction("BarcodeMappingRpt", "Reports", new { trID = TrID, page = pageNumber });



                #endregion

            }
            else
            {
                #region Forward To Report

                return RedirectToAction("BarcodeMappingRpt", "Reports", new { trID = TrID, page = pageNumber });

                #endregion
            }


            return null;
        }

        private string GetBarcodeText(long assignBoxId)
        {
            string barcodeTxt = string.Empty;
            AssignBox aBox = new AssignBox();
            aBox = repo.AssignBoxRepository.Find(assignBoxId);
            return (aBox.ItemId * 5000).ToString();
        }
        public string GetBarcodeTxtEmptyBoxes(long? id)
        {
            //BarCode _existingBarCode = new BarCode();
            //repo.BarCodeRepository.InsertOrUpdate(_existingBarCode);
            //repo.BarCodeRepository.Save();
            //long id = _existingBarCode.BarCodeId;

            string BarCodeText = (id * 5000 + 10).ToString();
            return BarCodeText;
        }

            public Image byteArrayToImage(byte[] fileContents)
        {
            MemoryStream ms = new MemoryStream(fileContents);
            Image returnImage = Image.FromStream(ms);

            return returnImage;
        }

        public ActionResult PreviousBarcode()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PreviousBarcode(HttpPostedFileBase filename, int ReportType)
        {

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
                List<BarCode> BarcodeList = new List<BarCode>();

                BarcodeList = Import_To_Grid(path, Extension);


                try
                {
                    System.IO.File.Delete(path);
                }
                catch (Exception e)
                {
                }

                TempData["BarcodeList"] = BarcodeList;
                Session["BarcodeList"] = BarcodeList;


                if (ReportType == 1)
                {

                    return RedirectToAction("PreviousBarcode", "Reports");
                }

                else if (ReportType == 2)
                {
                    return RedirectToAction("PreviousBarcodeMapping", "Reports");
                }

            }
            return View();
        }

        private List<BarCode> Import_To_Grid(string FilePath, string Extension)
        {
            bool flag = false;

            List<BarCode> barcodeList = new List<BarCode>();

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
                barcodeList = SaveData(dt);
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                // ShowSuccessResult("invalid file formate");

                ErrorLog _error = new ErrorLog();
                _error.ErrorMsg = ex.Message;
                _error.ErrorTime = DateTime.Now;
                _error.ErrorType = "XL Upload Box";
                repo.ErrorLogRepository.InsertOrUpdate(_error);
                repo.ErrorLogRepository.Save();

            }

            return barcodeList;
        }

        private List<BarCode> SaveData(DataTable dt)
        {
            List<BarCode> barcodeList = new List<BarCode>();
            List<AssignBox> assignBoxList = new List<AssignBox>();

            foreach (DataRow dr in dt.Rows)
            {
                #region Get All Values From XL
                string BoxName = dr["Box Name"].ToString().Trim();
                string BoxNo2 = dr["Box No"].ToString().Trim();
                string Year = dr["Year"].ToString().Trim();
                string ClientName = dr["Client Name"].ToString().Trim();
                string DeptName = dr["Department Name"].ToString().Trim();

                string BoxNo = FormatBoxNo(BoxNo2);

                #endregion

                try
                {

                    Client _client = new Client();
                    _client = repo.ClientRepository.GetByClientName(ClientName);

                    if (_client != null)
                    {
                        Department _dept = new Department();
                        _dept = repo.DepartmentRepository.GetByClientIDandDeptName(_client.ClientID, DeptName);

                        if (_dept != null)
                        {
                            Item _item = new Item();
                            _item = repo.ItemRepository.GetByClientIdDeptIdBoxNoBoxNameYear(_client.ClientID, _dept.DepartmentID, BoxNo, BoxName, Year);

                            if (_item != null)
                            {
                                AssignBox _assignBox = new AssignBox();
                                List<AssignBox> aBoxList = new List<AssignBox>();

                                aBoxList = repo.AssignBoxRepository.CheckForBarcode(_item.ItemId, Convert.ToInt64(EnumHelper.Status.Barcode_Assigned));//.AssignBoxes.Where(a => a.ItemId == _itemID)

                                long aId = aBoxList[0].AssignBoxId;
                                _assignBox = repo.AssignBoxRepository.Find(aId);//(_item.ItemId, Convert.ToInt64(EnumHelper.Status.Barcode_Assigned));
                                //TODO:

                                if (_assignBox != null)
                                {
                                    assignBoxList.Add(_assignBox);
                                }
                            }


                        }

                    }
                }
                catch (Exception ex)
                {
                    ErrorLog _error = new ErrorLog();
                    _error.ErrorMsg = ex.ToString();
                    _error.ErrorTime = DateTime.Now;
                    _error.ErrorType = "PreviousBarcode";
                    repo.ErrorLogRepository.InsertOrUpdate(_error);
                    repo.ErrorLogRepository.Save();
                }


            }

            if (assignBoxList.Count > 0)
            {
                foreach (AssignBox item in assignBoxList)
                {
                    string BarCodeText;
                    #region BarCode Insert or Update
                    if (item.BarCodeText != null)
                    {
                         BarCodeText = GetBarcodeTxtEmptyBoxes(item.BarCodeId);
                    }
                    else
                    {
                         BarCodeText = GetBarcodeText(item.AssignBoxId);
                    }
                    Image myimg = Code128Rendering.MakeBarcodeImage(BarCodeText, 2, false);
                    byte[] fileContents = imageToByteArray(myimg);


                    string _relativePath = "~/Content/Images/" + ConfigurationManager.AppSettings["BarCodes"].ToString() + "AssignBox-" + item.AssignBoxId.ToString();
                    var path = "D:\\WHMS2\\Content\\Images\\BarCodes\\ " + "AssignBox-" + item.AssignBoxId.ToString();

                    Directory.CreateDirectory(path);


                    string filePath = path + ".Bmp";

                    System.IO.File.WriteAllBytes(filePath, fileContents);



                    BarCode _existingBarCode = repo.BarCodeRepository.GetByAssignBoxId(item.AssignBoxId);//.ite.GetByAssignBoxId(item.AssignBoxId);
                    string _reportField = string.Empty;

                    if (_existingBarCode == null)
                    {
                        _existingBarCode = new BarCode();


                    }
                    _existingBarCode.AssignBoxId = item.AssignBoxId;
                    _existingBarCode.ImagePathRel = _relativePath + ".Bmp";
                    _existingBarCode.ImagePathAbs = "file://" + filePath;
                    _existingBarCode.BoxNo = item.BoxNo;
                    _existingBarCode.Year = item.Year;
                    _existingBarCode.BoxName = item.BoxName;
                    _existingBarCode.ClientName = repo.ClientRepository.Find(item.Item.ClientID).ClientName; ;
                    _existingBarCode.DeptName = repo.DepartmentRepository.Find(item.Item.DepartmentID).DepartmentName; //item.Item.Department.DepartmentName;
                    _reportField = "Box Name :" + _existingBarCode.BoxName + System.Environment.NewLine;
                    _reportField = _reportField + "Box No :" + _existingBarCode.BoxNo + System.Environment.NewLine;
                    _reportField = _reportField + "Client :" + _existingBarCode.ClientName + System.Environment.NewLine + "Department :" + _existingBarCode.DeptName + System.Environment.NewLine;

                    _existingBarCode.ReportFiled = _reportField;
                    _existingBarCode.ItemId = item.ItemId;
                    repo.BarCodeRepository.InsertOrUpdate(_existingBarCode);
                    repo.BarCodeRepository.Save();
                    //InsertOrUpdate(_existingBarCode);
                    //Save();

                    AssignBox _box = new AssignBox();
                    _box = repo.AssignBoxRepository.Find(item.AssignBoxId);// context.AssignBoxes.Where(a => a.AssignBoxId == item.AssignBoxId).FirstOrDefault();
                    _box.BarCodeId = _existingBarCode.BarCodeId;
                    repo.AssignBoxRepository.InsertOrUpdate(_box);
                    repo.AssignBoxRepository.Save();


                    #endregion


                    #region BarcodeMapping Table Insertion

                    BarcodeMapping _mapping = new BarcodeMapping();

                    _mapping = repo.BarcodeMappingRepository.GetByTransmittalNoandBarcodeIdandItemId(item.TransmittalIN.TransmittalNo, _existingBarCode.BarCodeId, item.ItemId);

                    if (_mapping == null)
                        _mapping = new BarcodeMapping();

                    _mapping.BarCodeId = _existingBarCode.BarCodeId;
                    _mapping.BoxName = item.BoxName;
                    _mapping.BoxNo = item.BoxNo;
                    _mapping.ClientName = _existingBarCode.ClientName;
                    _mapping.DeptName = _existingBarCode.DeptName;
                    _mapping.ImagePath = _existingBarCode.ImagePathAbs;
                    _mapping.ItemId = item.ItemId;
                    _mapping.TransmittalNo = item.TransmittalIN.TransmittalNo;
                    _mapping.Year = item.Year;

                    repo.BarcodeMappingRepository.InsertOrUpdate(_mapping);
                    repo.BarcodeMappingRepository.Save();

                    #endregion


                    barcodeList.Add(_existingBarCode);

                }
            }




            return barcodeList;
        }

        private string FormatBoxNo(string BoxNo2)
        {
            string retrnText = BoxNo2;

            if (BoxNo2.Length == 10)
            {
                retrnText = BoxNo2;
            }
            if (BoxNo2.Length < 10)
            {
                retrnText = retrnText.PadLeft(10, '0');
            }

            return retrnText;
        }


        private string GetFormattedInteger(long amount, int padder)
        {
            string finalAmount = amount.ToString();

            if (finalAmount.Length < padder)
            {
                finalAmount = finalAmount.PadLeft(padder, '0');
            }

            return finalAmount;
        }

        public ActionResult InventoryReport()
        {
            ViewBag.ClientList = repo.ClientRepository.GetAll();
            ViewBag.WareHouseList = repo.WarehouseRepository.GetAllList();

            return View();
        }

        [HttpPost]
        public ActionResult InventoryReport(InventoryModel model)
        {
            if (model.ClientID == 0 || model.WarhouseID == 0)
            {
                ViewBag.Flag = "1";
                ViewBag.ClientList = repo.ClientRepository.GetAll();
                ViewBag.WareHouseList = repo.WarehouseRepository.GetAllList();
                return View();
            }
            else
            {
                List<InventoryViewModel> inventoryLst = new List<InventoryViewModel>();
                inventoryLst = repo.AssignBoxRepository.GetInventoryListByClientIDandWareHouseID(model.ClientID, model.WarhouseID);

                TempData["inventoryLst"] = inventoryLst;
                Session["inventoryLst"] = inventoryLst;

                Session["ClientName"] = repo.ClientRepository.Find(model.ClientID).ClientName;
                Session["WareHouseName"] = repo.WarehouseRepository.Find(model.WarhouseID).WarehouseName;

                return RedirectToAction("InventoryReport", "Reports");
            }

            return null;

        }

        public ViewResult Details(long id)
        {
            AssignBox assignbox = repo.AssignBoxRepository.Find(id);
            return View(assignbox);
        }

        //
        // GET: /AssignBoxes/Create

        public ActionResult Create()
        {
            ViewBag.PossibleTransmittalINs = repo.TransmittalINRepository.AllIncluding();
            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            ViewBag.PossibleItems = repo.ItemRepository.AllIncluding();
            ViewBag.PossibleTransmittalINStatus = repo.TransmittalINStatusRepository.AllIncluding();
            return View();
        }

        //
        // POST: /AssignBoxes/Create

        [HttpPost]
        public ActionResult Create(AssignBox assignbox)
        {
            if (ModelState.IsValid)
            {
                repo.AssignBoxRepository.InsertOrUpdate(assignbox);
                repo.AssignBoxRepository.Save();
                return RedirectToAction("Index");
            }

            ViewBag.PossibleTransmittalINs = repo.TransmittalINRepository.AllIncluding();
            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            ViewBag.PossibleItems = repo.ItemRepository.AllIncluding();
            ViewBag.PossibleTransmittalINStatus = repo.TransmittalINStatusRepository.AllIncluding();
            return View(assignbox);
        }

        //
        // GET: /AssignBoxes/Edit/5

        public ActionResult Edit(long id)
        {
            AssignBox assignbox = repo.AssignBoxRepository.Find(id);
            ViewBag.PossibleTransmittalINs = repo.TransmittalINRepository.AllIncluding();
            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            ViewBag.PossibleItems = repo.ItemRepository.AllIncluding();
            ViewBag.PossibleTransmittalINStatus = repo.TransmittalINStatusRepository.AllIncluding();
            return View(assignbox);
        }

        //
        // POST: /AssignBoxes/Edit/5

        [HttpPost]
        public ActionResult Edit(AssignBox assignbox)
        {
            if (ModelState.IsValid)
            {
                repo.AssignBoxRepository.InsertOrUpdate(assignbox);
                repo.AssignBoxRepository.Save();
                return RedirectToAction("Index");
            }
            ViewBag.PossibleTransmittalINs = repo.TransmittalINRepository.AllIncluding();
            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            ViewBag.PossibleItems = repo.ItemRepository.AllIncluding();
            ViewBag.PossibleTransmittalINStatus = repo.TransmittalINStatusRepository.AllIncluding();
            return View(assignbox);
        }

        //
        // GET: /AssignBoxes/Delete/5

        public ActionResult Delete(long id)
        {
            AssignBox assignbox = repo.AssignBoxRepository.Find(id);
            return View(assignbox);
        }

        //
        // POST: /AssignBoxes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            AssignBox assignbox = repo.AssignBoxRepository.Find(id);
            repo.AssignBoxRepository.Delete(id);
            repo.AssignBoxRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            //if (disposing) {
            //    context.Dispose();
            //}
            base.Dispose(disposing);
        }
        #region Duplicate Box

        public ActionResult CheckDuplicate()
        {
            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.AllIncluding();

            return View();
        }

        [HttpPost]
        public ActionResult CheckDuplicate(CheckDuplicateViewModel model)
        {


            if (Request.Form["btnDelete"] != null)
            {

                Session["IDS"] = model;

                return RedirectToAction("DeleteBoxAll");
            }


            if (model.ClientID != -1 && model.DepartmentID != -1)
            {
                List<AssignBox> assignBoxList = new List<AssignBox>();
                assignBoxList = repo.AssignBoxRepository.GetListOfDuplicateByClientIdandDeptId(model.ClientID, model.DepartmentID);

                List<DuplicateBoxViewModel> duplicateList = new List<DuplicateBoxViewModel>();
                List<DuplicateBoxViewModel> singleList = new List<DuplicateBoxViewModel>();

                var query = assignBoxList.GroupBy(i => new { i.BoxName, i.BoxNo })
              .Where(g => g.Count() > 1)
              .Select(y => new { Element = y.Key, Counter = y.Count() })
              .ToList();



                foreach (var item in query)
                {

                    List<AssignBox> dupLiList = new List<AssignBox>();

                    dupLiList = repo.AssignBoxRepository.GetListByBoxNameBoxNo(item.Element.BoxName, item.Element.BoxNo);

                    string bxNo = "0";
                    foreach (AssignBox box in dupLiList)
                    {
                        DuplicateBoxViewModel modelDuplicate = new DuplicateBoxViewModel();
                        DuplicateBoxViewModel modelSingle = new DuplicateBoxViewModel();
                        if (box.Item.Unit == "File")
                        {
                            try
                            {
                                modelDuplicate.FileBoxName = box.BoxNameFile.ToString();
                            }
                            catch
                            {
                            }
                            try
                            {
                                modelDuplicate.BoxNo = box.BoxNo.ToString();
                                modelDuplicate.FileNumber = box.FileNumber.ToString();
                            }
                            catch
                            {
                            }
                            try
                            {
                                modelDuplicate.ReferrenceNo = box.ReferrenceNo.ToString();
                            }
                            catch
                            {
                            }
                            try
                            {

                                modelDuplicate.RingNo = box.RingNo.ToString();
                            }
                            catch
                            {
                            }

                            try
                            {
                                modelDuplicate.AccountNo = box.AccountNo.ToString();
                            }
                            catch
                            {
                            }

                            try
                            {
                                modelDuplicate.Year = box.Year.ToString();
                            }
                            catch
                            {

                            }
                            modelDuplicate.Unit = "File";
                        }
                        else
                        {

                            try
                            {

                                modelDuplicate.BoxName = box.BoxName.ToString();
                                modelSingle.BoxName = box.BoxName.ToString();
                            }
                            catch
                            {
                            }
                            try
                            {
                                modelDuplicate.BoxNo = box.BoxNo.ToString();
                                modelSingle.BoxNo = box.BoxNo.ToString();
                            }
                            catch
                            {
                            }

                            modelDuplicate.Unit = "Box";
                            modelSingle.Unit = "Box";

                            try
                            {
                                modelDuplicate.Year = box.Year.ToString();
                                modelSingle.Year = box.Year.ToString();
                            }
                            catch
                            {
                                modelDuplicate.Year = "";
                                modelSingle.Year = "";
                            }
                        }
                        modelDuplicate.AssignBoxId = box.AssignBoxId;//.ToString();
                        modelDuplicate.TransmittalINStatusId = box.TransmittalINStatusId;
                        modelDuplicate.ItemId = box.Item.ItemId;
                        modelSingle.AssignBoxId = box.AssignBoxId;//.ToString();
                        modelSingle.TransmittalINStatusId = box.TransmittalINStatusId;
                        modelSingle.ItemId = box.Item.ItemId;

                        modelDuplicate.TransmittalINNo = repo.TransmittalINRepository.Find(box.TransmittalINId).TransmittalNo;
                        modelSingle.TransmittalINNo = repo.TransmittalINRepository.Find(box.TransmittalINId).TransmittalNo;
                        if (box.BoxLocation.Count > 0)
                        {
                            // modelDuplicate.BoxCurrentStatus = box.BoxLocation;
                        }

                        duplicateList.Add(modelDuplicate);

                        if (box.BoxNo.ToString() != bxNo)
                        {
                            singleList.Add(modelSingle);
                            bxNo = box.BoxNo.ToString();
                        }
                    }





                }

                foreach (var item in singleList)
                {
                    DeleteDup(item.AssignBoxId);
                }


                duplicateList = new List<DuplicateBoxViewModel>();
                foreach (var item in query)
                {

                    List<AssignBox> dupLiList = new List<AssignBox>();

                    dupLiList = repo.AssignBoxRepository.GetListByBoxNameBoxNo(item.Element.BoxName, item.Element.BoxNo);

                    string bxNo = "0";
                    foreach (AssignBox box in dupLiList)
                    {
                        DuplicateBoxViewModel modelDuplicate = new DuplicateBoxViewModel();
                        DuplicateBoxViewModel modelSingle = new DuplicateBoxViewModel();
                        if (box.Item.Unit == "File")
                        {
                            try
                            {
                                modelDuplicate.FileBoxName = box.BoxNameFile.ToString();
                            }
                            catch
                            {
                            }
                            try
                            {
                                modelDuplicate.BoxNo = box.BoxNo.ToString();
                                modelDuplicate.FileNumber = box.FileNumber.ToString();
                            }
                            catch
                            {
                            }
                            try
                            {
                                modelDuplicate.ReferrenceNo = box.ReferrenceNo.ToString();
                            }
                            catch
                            {
                            }
                            try
                            {

                                modelDuplicate.RingNo = box.RingNo.ToString();
                            }
                            catch
                            {
                            }

                            try
                            {
                                modelDuplicate.AccountNo = box.AccountNo.ToString();
                            }
                            catch
                            {
                            }

                            try
                            {
                                modelDuplicate.Year = box.Year.ToString();
                            }
                            catch
                            {

                            }
                            modelDuplicate.Unit = "File";
                        }
                        else
                        {

                            try
                            {

                                modelDuplicate.BoxName = box.BoxName.ToString();
                                modelSingle.BoxName = box.BoxName.ToString();
                            }
                            catch
                            {
                            }
                            try
                            {
                                modelDuplicate.BoxNo = box.BoxNo.ToString();
                                modelSingle.BoxNo = box.BoxNo.ToString();
                            }
                            catch
                            {
                            }

                            modelDuplicate.Unit = "Box";
                            modelSingle.Unit = "Box";

                            try
                            {
                                modelDuplicate.Year = box.Year.ToString();
                                modelSingle.Year = box.Year.ToString();
                            }
                            catch
                            {
                                modelDuplicate.Year = "";
                                modelSingle.Year = "";
                            }
                        }
                        modelDuplicate.AssignBoxId = box.AssignBoxId;//.ToString();
                        modelDuplicate.TransmittalINStatusId = box.TransmittalINStatusId;
                        modelDuplicate.ItemId = box.Item.ItemId;
                        modelSingle.AssignBoxId = box.AssignBoxId;//.ToString();
                        modelSingle.TransmittalINStatusId = box.TransmittalINStatusId;
                        modelSingle.ItemId = box.Item.ItemId;

                        modelDuplicate.TransmittalINNo = repo.TransmittalINRepository.Find(box.TransmittalINId).TransmittalNo;
                        modelSingle.TransmittalINNo = repo.TransmittalINRepository.Find(box.TransmittalINId).TransmittalNo;
                        if (box.BoxLocation.Count > 0)
                        {
                            // modelDuplicate.BoxCurrentStatus = box.BoxLocation;
                        }

                        duplicateList.Add(modelDuplicate);

                        if (box.BoxNo.ToString() != bxNo)
                        {
                            singleList.Add(modelSingle);
                            bxNo = box.BoxNo.ToString();
                        }
                    }





                }




                model.boxList = duplicateList;



            }
            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.AllIncluding();

            return View(model);
        }

        public ActionResult DeleteBox(long id)
        {
            BarCode barcode = new BarCode();
            barcode = repo.BarCodeRepository.GetByAssignBoxId(id);
            try
            {
                repo.BarCodeRepository.Delete(barcode.BarCodeId);
                repo.BarCodeRepository.Save();

                BoxLocation location = new BoxLocation();
                location = repo.BoxLocationRepository.GetByAssignBoxId(id);

                repo.BoxLocationRepository.Delete(location.BoxLocationId);
                repo.BoxLocationRepository.Save();

                AssignBox assignBox = new AssignBox();
                assignBox = repo.AssignBoxRepository.Find(id);
                long itemId = assignBox.ItemId;

                repo.AssignBoxRepository.Delete(id);
                repo.AssignBoxRepository.Save();


                Item item = new Item();
                item = repo.ItemRepository.Find(itemId);
                repo.ItemRepository.Delete(item.ItemId);
                repo.ItemRepository.Save();


                AssignBoxTrOUT trOut = new AssignBoxTrOUT();
                trOut = repo.AssignBoxTrOUTRepository.GetByItemId(assignBox.ItemId);
                repo.AssignBoxTrOUTRepository.Delete(trOut.AssignBoxTrOUTId);
                repo.AssignBoxTrOUTRepository.Save();




            }
            catch
            {
            }


            ViewBag.message = "1";
            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.AllIncluding();

            return View("CheckDuplicate");
        }

        public void DeleteDup(long id)
        {
            long itemId = 0;
            AssignBox assignBox = new AssignBox();
            BarCode barcode = new BarCode();
            barcode = repo.BarCodeRepository.GetByAssignBoxId(id);
            try
            {
                try
                {
                    using (UnitOfWork repo1 = new UnitOfWork())
                    {
                        repo1.BarCodeRepository.Delete(barcode.BarCodeId);
                        repo1.BarCodeRepository.Save();
                    }

                }
                catch
                { }

                try
                {
                    using (UnitOfWork repo1 = new UnitOfWork())
                    {
                        BoxLocation location = new BoxLocation();
                        location = repo.BoxLocationRepository.GetByAssignBoxId(id);

                        repo1.BoxLocationRepository.Delete(location.BoxLocationId);
                        repo1.BoxLocationRepository.Save();
                    }
                }
                catch
                {

                }

                try
                {
                    using (UnitOfWork repo1 = new UnitOfWork())
                    {

                        assignBox = repo.AssignBoxRepository.Find(id);
                        itemId = assignBox.ItemId;

                        repo1.AssignBoxRepository.Delete(id);
                        repo1.AssignBoxRepository.Save();
                    }
                }
                catch
                {

                }
                try
                {
                    using (UnitOfWork repo1 = new UnitOfWork())
                    {

                        Item item = new Item();
                        item = repo.ItemRepository.Find(itemId);
                        repo1.ItemRepository.Delete(item.ItemId);
                        repo1.ItemRepository.Save();
                    }
                }
                catch
                { }


                try
                {
                    using (UnitOfWork repo1 = new UnitOfWork())
                    {
                        AssignBoxTrOUT trOut = new AssignBoxTrOUT();
                        trOut = repo.AssignBoxTrOUTRepository.GetByItemId(assignBox.ItemId);
                        repo1.AssignBoxTrOUTRepository.Delete(trOut.AssignBoxTrOUTId);
                        repo1.AssignBoxTrOUTRepository.Save();
                    }
                }
                catch
                {

                }



            }
            catch (Exception ex)
            {
            }
        }

        public ActionResult DeleteBoxAll()
        {
            CheckDuplicateViewModel model = new CheckDuplicateViewModel();
            if (Session["IDS"] != null)
            {
                model = (CheckDuplicateViewModel)Session["IDS"];

                if (model.AssignBoxIds.Count() > 0)
                {
                    foreach (long id in model.AssignBoxIds)
                    {
                        try
                        {
                            BarCode barcode = new BarCode();
                            barcode = repo.BarCodeRepository.GetByAssignBoxId(id);

                            repo.BarCodeRepository.Delete(barcode.BarCodeId);
                            repo.BarCodeRepository.Save();
                        }
                        catch (Exception ex)
                        {
                            ErrorLog log = new ErrorLog();
                            log.ErrorMsg = ex.ToString();
                            log.ErrorTime = DateTime.Now;
                            repo.ErrorLogRepository.InsertOrUpdate(log);
                            repo.ErrorLogRepository.Save();
                        }
                        try
                        {
                            BoxLocation location = new BoxLocation();
                            location = repo.BoxLocationRepository.GetByAssignBoxId(id);

                            repo.BoxLocationRepository.Delete(location.BoxLocationId);
                            repo.BoxLocationRepository.Save();
                        }
                        catch (Exception ex)
                        {
                            ErrorLog log = new ErrorLog();
                            log.ErrorMsg = ex.ToString();
                            log.ErrorTime = DateTime.Now;
                            repo.ErrorLogRepository.InsertOrUpdate(log);
                            repo.ErrorLogRepository.Save();
                        }

                        try
                        {
                            AssignBox assignBox = new AssignBox();
                            assignBox = repo.AssignBoxRepository.Find(id);
                            long itemId = assignBox.ItemId;

                            repo.AssignBoxRepository.Delete(id);
                            repo.AssignBoxRepository.Save();


                            Item item = new Item();
                            item = repo.ItemRepository.Find(itemId);
                            repo.ItemRepository.Delete(item.ItemId);
                            repo.ItemRepository.Save();


                            AssignBoxTrOUT trOut = new AssignBoxTrOUT();
                            trOut = repo.AssignBoxTrOUTRepository.GetByItemId(assignBox.ItemId);
                            repo.AssignBoxTrOUTRepository.Delete(trOut.AssignBoxTrOUTId);
                            repo.AssignBoxTrOUTRepository.Save();



                        }
                        catch (Exception ex)
                        {
                            ErrorLog log = new ErrorLog();
                            log.ErrorMsg = ex.ToString();
                            log.ErrorTime = DateTime.Now;
                            repo.ErrorLogRepository.InsertOrUpdate(log);
                            repo.ErrorLogRepository.Save();
                        }



                    }

                    ViewBag.message = "1";
                    ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
                    ViewBag.PossibleDepartments = repo.DepartmentRepository.AllIncluding();

                    return View("CheckDuplicate");
                }
                else
                {
                    ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
                    ViewBag.PossibleDepartments = repo.DepartmentRepository.AllIncluding();

                    return View("CheckDuplicate");
                }





            }
            else
            {
                //ViewBag.message = "1";
                ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
                ViewBag.PossibleDepartments = repo.DepartmentRepository.AllIncluding();

                return View("CheckDuplicate");
            }


            ViewBag.message = "1";
            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.AllIncluding();

            return View("CheckDuplicate");
        }

        #endregion
    }
}