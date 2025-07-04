using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;
using Microsoft.Reporting.WebForms;

namespace WareHouseMVC.Controllers
{
    public class ReportsController : BaseController
    {
        UnitOfWork repo = new UnitOfWork();
        long clientId = 0;


        #region Report Header Section

        public ActionResult BoxMovement()
        {
            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }

            ViewBag.ClientName = repo.ClientRepository.Find(clientId).ClientName;
            ViewBag.ClientId = clientId;

            BoxSearchViewModel model = new BoxSearchViewModel();
            ViewBag.ClientList = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(clientId);

            return View(model);
        }

        [HttpPost]
        public ActionResult BoxMovement(BoxSearchViewModel model)
        {
            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }

            //if (ModelState.IsValid)
            //{
            if (string.IsNullOrEmpty(model.BoxName) || string.IsNullOrEmpty(model.BoxName) || model.DepartmentID == 0 || model.DepartmentID == -1 || model.BoxName == null)
            //if (model.DepartmentID == 0 || model.DepartmentID == -1)
            {
                ViewBag.flag = "1";
                ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(clientId);
                return View();
            }
            else
            {
                ViewBag.ClientList = repo.ClientRepository.AllIncluding();
                ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(model.ClientID);

                Client _client = new Client();
                _client = repo.ClientRepository.Find(model.ClientID);

                Department _dept = new Department();
                _dept = repo.DepartmentRepository.GetByClientIDandDepId(model.ClientID, model.DepartmentID.Value);


                Item item = new Item();
                item = repo.ItemRepository.GetByCliendIDandDeptIdandBoxNameandBoxNo(model.ClientID, model.DepartmentID.Value, model.BoxName, model.BoxNo);

                if (item != null)
                {
                    List<ChangeLocation> locationHistory = new List<ChangeLocation>();
                    locationHistory = repo.ChangeLocationRepository.GetListByItemId(item.ItemId);
                    TempData["LocationHistory"] = locationHistory;

                    Session["LocationHistory"] = locationHistory;
                    return RedirectToAction("BoxMovementReport", "Reports", new { clientName = _client.ClientName, deptName = _dept.DepartmentName, boxName = model.BoxName, boxNo = model.BoxNo, format = model.ReportType });
                }
                else
                {
                    ViewBag.flag = "2";
                    ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(clientId);
                    return View();
                }
            }
            //}

            return View(model);
        }

        public ActionResult DestructionPeriod()
        {
            UpcomingDestructionViewModel model = new UpcomingDestructionViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult DestructionPeriod(UpcomingDestructionViewModel model)
        {
            List<DesPeriodReportViewModel> desList = new List<DesPeriodReportViewModel>();

            List<Item> itemList = repo.ItemRepository.GetDestructionPeriodByMonthCount(model.MonthCount);
            foreach (Item item in itemList)
            {
                DesPeriodReportViewModel desModel = new DesPeriodReportViewModel();

                desModel.BoxName = item.ItemName;
                desModel.BoxNo = item.BoxNo;
                desModel.Client = item.Client.ClientName;
                desModel.Dept = item.Department.DepartmentName;
                desModel.DesPeriod = item.DestructionPeriod;
                desModel.Unit = item.Unit;
                desModel.Year = item.Year;

                desList.Add(desModel);

            }

            TempData["itemList"] = desList;
            Session["itemList"] = desList;
            return RedirectToAction("DestructionPeriodReport", "Reports", new { monthCount = model.MonthCount, format = model.ReportType });

        }

        #endregion



        #region Transmittal IN Method

        public ActionResult PrintAsPDF(int trID)
        {
            int format = 1;
            return RedirectToAction("PrintTransmittalIN", "Reports", new { trID = trID, type = format });
        }

        public ActionResult PrintAsExcel(int trID)
        {
            int format = 2;
            return RedirectToAction("PrintTransmittalIN", "Reports", new { trID = trID, type = format });
        }

        public ActionResult PrintAsDoc(int trID)
        {
            int format = 3;
            return RedirectToAction("PrintTransmittalIN", "Reports", new { trID = trID, type = format });
        }

        public ActionResult PrintTransmittalIN(int trID, int type)
        {
            //geting repot data from the business object
            var reportViewModel = repo.TransmittalINRepository.GetTransmittalINReport(trID, type);

            var renderedBytes = reportViewModel.RenderReport();

            if (reportViewModel.ViewAsAttachment)
                Response.AddHeader("content-disposition", reportViewModel.ReporExportFileName);
            return File(renderedBytes, reportViewModel.LastmimeType);
        }

        #endregion

        #region Transmittal OUT Method

        public ActionResult PrintAsPDFTrOUT(long trID)
        {

            int format = 1;
            return RedirectToAction("PrintTransmittalOUT", "Reports", new { trID = trID, type = format });
        }

        public ActionResult PrintAsExcelTrOUT(long trID)
        {

            int format = 2;
            return RedirectToAction("PrintTransmittalOUT", "Reports", new { trID = trID, type = format });
        }

        public ActionResult PrintAsDocTrOUT(long trID)
        {
            int format = 3;
            return RedirectToAction("PrintTransmittalOUT", "Reports", new { trID = trID, type = format });
        }


        public ActionResult PrintTransmittalOUT(long trID, int type)
        {
            //geting repot data from the business object
            var reportViewModel = repo.TransmittalOUTRepository.GetTransmittalOUTReport(trID, type);

            var renderedBytes = reportViewModel.RenderReport();

            if (reportViewModel.ViewAsAttachment)
                Response.AddHeader("content-disposition", reportViewModel.ReporExportFileName);
            return File(renderedBytes, reportViewModel.LastmimeType);

        }


        #endregion

        #region GatePass Print Method

        public ActionResult PrintAsPDFGatePass(int trID)
        {
            int format = 1;
            return RedirectToAction("PrintGatePass", "Reports", new { trID = trID, type = format });
        }

        public ActionResult PrintAsExcelGatePass(int trID)
        {
            int format = 2;
            return RedirectToAction("PrintGatePass", "Reports", new { trID = trID, type = format });
        }

        public ActionResult PrintAsDocGatePass(int trID)
        {
            int format = 3;
            return RedirectToAction("PrintGatePass", "Reports", new { trID = trID, type = format });
        }

        public ActionResult PrintGatePass(int trID, int type)
        {
            //geting repot data from the business object
            var reportViewModel = repo.TransmittalOUTRepository.GetGatePassReport(trID, type);

            var renderedBytes = reportViewModel.RenderReport();

            if (reportViewModel.ViewAsAttachment)
                Response.AddHeader("content-disposition", reportViewModel.ReporExportFileName);
            return File(renderedBytes, reportViewModel.LastmimeType);
        }


        #endregion

        #region Barcode Print Method

        public ActionResult PrintBarCode(long trID, int? page)
        {
            int pageNumber = (page ?? 1);
            User _user = repo.UserRepository.GetUserByUserName(HttpContext.User.Identity.Name);
            long _wID = 0;
            if (_user.WarehouseID.HasValue)
            {
                _wID = _user.WarehouseID.Value;
            }


            var reportViewModel = repo.BarCodeRepository.GetBarCodeReport(trID, _wID, Convert.ToInt64(EnumHelper.Status.Barcode_Assigned), pageNumber);

            var renderedBytes = reportViewModel.RenderReport();

            if (reportViewModel.ViewAsAttachment)
                Response.AddHeader("content-disposition", reportViewModel.ReporExportFileName);
            return File(renderedBytes, reportViewModel.LastmimeType);
        }

        [HttpPost]
        public ActionResult PrintEmptyBoxBarCode(List<string> barcodes)
        {
            // If needed, create a list of EmptyBoxBarcode objects from the barcode strings
            var barcodeList = barcodes.Select(barcodeText => new EmptyBoxBarcode
            {
                Id = 0,  // Set default or meaningful values if needed
                EmptyBoxId = 0,  // Set default or meaningful values if needed
                BarCodeId = 0,  // Set default or meaningful values if needed
                BarCodeText = barcodeText
            }).ToList();

            // Now pass this list to your repository method
            var reportViewModel = repo.BarCodeRepository.GetEmptyBoxBarCodeReport(barcodeList);

            var renderedBytes = reportViewModel.RenderReport();

            if (reportViewModel.ViewAsAttachment)
                Response.AddHeader("content-disposition", reportViewModel.ReporExportFileName);

            return File(renderedBytes, reportViewModel.LastmimeType);
        }

        #endregion

        #region Box Statement Method

        public ActionResult BoxStatement(long clientID, DateTime month, double rate, int reportType)
        {
            List<BoxStatementSheetViewModel> _boxSheet = new List<BoxStatementSheetViewModel>();

            if (Session["boxSheet"] != null)
            {

                _boxSheet = Session["boxSheet"] as List<BoxStatementSheetViewModel>;
                Session["boxSheet"] = null;
            }
            else
            {


                List<Item> _itemList = repo.ItemRepository.GetByClientID(clientID);
                List<Item> _itemListMonthFiltered = new List<Item>();
                List<AssignBox> _assignBoxList = new List<AssignBox>();

                DateTime monthStart = month;
                DateTime monthEnd = month.AddMonths(1).AddDays(-1);


                List<AssignBox> assignLst = new List<AssignBox>();
                assignLst = repo.AssignBoxRepository.GetByClientMonthStatu(clientID, month, (int)EnumHelper.Status.Pending);

                //foreach (Item item in _itemList)
                //{
                //    AssignBox _assignBox = repo.AssignBoxRepository.GetByItemIDandMonthWithStatus(item.ItemId, month, (int)EnumHelper.Status.Received_at_WareHouse);

                //    if (_assignBox != null)
                //    {
                //        _itemListMonthFiltered.Add(item);

                //        if (_assignBox.AssignDate >= monthStart && _assignBox.AssignDate <= monthEnd)
                //        {
                //            _assignBoxList.Add(_assignBox);
                //        }
                //    }

                //}


                foreach (AssignBox item in assignLst)
                {
                    _itemListMonthFiltered.Add(item.Item);

                }

                _assignBoxList = assignLst.Where(a => a.AssignDate >= monthStart && a.AssignDate <= monthEnd).ToList();







                var wordList = _itemListMonthFiltered;

                var grouped = wordList.Where(a => a.ClientID == clientID).GroupBy(s => new { department = s.DepartmentID, client = s.ClientID }).Select(g => new { g.Key, Count = g.Count() });

                foreach (var item in grouped)
                {
                    BoxStatementSheetViewModel _box = new BoxStatementSheetViewModel();

                    _box.Department = repo.DepartmentRepository.Find(item.Key.department).DepartmentName;
                    _box.NewBox = _assignBoxList.Where(a => a.Item.DepartmentID == item.Key.department).ToList().Count;// GetNewBox(_assignBoxList, item.Key.department);
                    _box.Rate = rate;
                    _box.TotalBox = item.Count;
                    _box.NoofBox = _box.TotalBox - _box.NewBox;
                    _box.TotalAmount = rate * (_box.TotalBox);

                    _boxSheet.Add(_box);
                }

                Session["boxSheet"] = _boxSheet;
            }

            var reportViewModel = repo.BoxSheetRepository.GetBoxSheetReport(_boxSheet, clientID, month, reportType);
            var renderedBytes = reportViewModel.RenderReport();
            return File(renderedBytes, reportViewModel.LastmimeType);
        }
        public int GetNewBox(List<AssignBox> _assignBoxList, long deptID)
        {
            int _boxCount = 0;
            foreach (AssignBox item in _assignBoxList)
            {
                Item _item = new Item();
                _item = repo.ItemRepository.Find(item.ItemId);

                if (_item.DepartmentID == deptID)
                {
                    _boxCount++;
                }
            }

            return _boxCount;
        }

        #endregion

        #region EmptyBox Statement Method

        public ActionResult EmptyBoxStatement(long clientID, DateTime month, double rate, int reportType)
        {
            DateTime monthStart = month;
            DateTime monthEnd = month.AddMonths(1).AddDays(-1);
            List<EmptyBox> emptyBoxList = new List<EmptyBox>();
            List<EmptyBoxViewModel> emptyBoxViewList = new List<EmptyBoxViewModel>();

            emptyBoxList = repo.EmptyBoxRepository.GetListBtClientIDandDate(clientID, monthStart, monthEnd);

            foreach (EmptyBox item in emptyBoxList)
            {
                EmptyBoxViewModel _item = new EmptyBoxViewModel();

                _item.Date = item.RecuisitionDate.ToString("dd-MM-yyyy");
                _item.DeliverTo = item.DeliverTo + "(" + item.Address + ")";
                _item.Quantity = item.NoofBoxes;
                _item.Time = 1;
                _item.Rate = rate;
                _item.Amount = (item.NoofBoxes * rate);
                emptyBoxViewList.Add(_item);
            }



            var reportViewModel = repo.EmptyBoxRepository.GetEmptyBoxReport(emptyBoxViewList, clientID, month, reportType);
            var renderedBytes = reportViewModel.RenderReport();
            return File(renderedBytes, reportViewModel.LastmimeType);



        }
        #endregion

        #region Box IN Statement Method

        public ActionResult BoxINStatement(long clientID, DateTime month, double rate, long baseBox, double extraRate, int reportType)
        {
            DateTime monthStart = month;
            DateTime monthEnd = month.AddMonths(1).AddDays(-1);

            List<BoxInStatementViewModel> boxINList = new List<BoxInStatementViewModel>();

            List<TransmittalIN> allTransmittalList = new List<TransmittalIN>();
            allTransmittalList = repo.TransmittalINRepository.GetByClientIDandDateRange(clientID, monthStart, monthEnd);

            //start from tomorrow
            foreach (TransmittalIN item in allTransmittalList)
            {
                BoxInStatementViewModel boxIN = new BoxInStatementViewModel();

                boxIN.Date = item.TransmittalDate.ToString("dd-MM-yyyy");
                boxIN.Department = repo.DepartmentRepository.Find(item.DepartmentID).DepartmentName;

                List<AssignBox> _assignBoxList = new List<AssignBox>();
                _assignBoxList = repo.AssignBoxRepository.GetByTransmittalINID(item.TransmittalINId);
                if (_assignBoxList.Count > 0)
                {

                    boxIN.Quantity = _assignBoxList.Count();
                    boxIN.BoxNo = GetBoxName(_assignBoxList);
                    boxIN.Month = month.ToString("Y");
                    boxIN.Time = 1;

                    boxIN.Rate = rate;
                    boxIN.Extra = GetExtra(baseBox, boxIN.Quantity);
                    boxIN.Amount = GetAmount(boxIN.Quantity, boxIN.Extra, rate, extraRate);

                    boxINList.Add(boxIN);
                }
            }

            var reportViewModel = repo.BoxINStatementViewModelRepository.GetBoxINReport(boxINList, clientID, month, reportType);
            var renderedBytes = reportViewModel.RenderReport();
            return File(renderedBytes, reportViewModel.LastmimeType);


        }

        private double GetAmount(long quantity, long extra, double rate, double extraRate)
        {
            double _amount = 0;

            if (extra > 0)
            {
                _amount = rate + (extra * extraRate);
            }
            else
            {
                _amount = rate;
            }

            return _amount;
        }

        private long GetExtra(long baseBox, long quantity)
        {
            long p = 0;
            if (quantity > baseBox)
            {
                p = quantity - baseBox;
            }

            return p;
        }

        private string GetBoxName(List<AssignBox> _assignBoxList)
        {
            string _boxNo = string.Empty;
            int i = 0;
            foreach (AssignBox item in _assignBoxList)
            {
                string trimedBoxNo = item.BoxNo.TrimStart('0');

                if (i == 0)
                {
                    _boxNo = _boxNo + item.BoxName + "-" + trimedBoxNo + "(" + item.Year + ")" + ", ";
                    i++;
                }
                else
                {
                    _boxNo = _boxNo + trimedBoxNo + "(" + item.Year + ")" + ", ";
                }

            }

            return _boxNo;
        }

        #endregion

        #region Box OUT Statement Method

        public ActionResult BoxIOUTStatement(long clientID, DateTime month, double rate, long baseBox, double extraRate, int reportType)
        {
            DateTime monthStart = month;
            DateTime monthEnd = month.AddMonths(1).AddDays(-1);
            List<BoxOutStatementViewModel> boxOUTList = new List<BoxOutStatementViewModel>();

            List<TransmittalOUT> allTransmittalList = new List<TransmittalOUT>();
            allTransmittalList = repo.TransmittalOUTRepository.GetByClientIDandDateRange(clientID, monthStart, monthEnd);

            foreach (TransmittalOUT item in allTransmittalList)
            {
                BoxOutStatementViewModel boxOUT = new BoxOutStatementViewModel();
                boxOUT.Date = item.TransmittalDate.ToString("dd-MM-yyyy");
                boxOUT.Department = repo.DepartmentRepository.Find(item.DepartmentID).DepartmentName;

                List<AssignBoxTrOUT> _assignBoxList = new List<AssignBoxTrOUT>();
                _assignBoxList = repo.AssignBoxTrOUTRepository.GetByTrInId(item.TransmittalOUTId);

                if (_assignBoxList.Count > 0)
                {

                    boxOUT.Quantity = _assignBoxList.Count();
                    boxOUT.BoxNo = GetBoxNameForOUT(_assignBoxList);
                    boxOUT.Month = month.ToString("Y");
                    boxOUT.Time = 1;

                    boxOUT.Rate = rate;
                    boxOUT.Extra = GetExtra(baseBox, boxOUT.Quantity);
                    boxOUT.Amount = GetAmount(boxOUT.Quantity, boxOUT.Extra, rate, extraRate);

                    boxOUTList.Add(boxOUT);
                }
            }

            var reportViewModel = repo.BoxOutStatementViewModelRepository.GetBoxOUTReport(boxOUTList, clientID, month, reportType);
            var renderedBytes = reportViewModel.RenderReport();
            return File(renderedBytes, reportViewModel.LastmimeType);


        }

        private string GetBoxNameForOUT(List<AssignBoxTrOUT> _assignBoxList)
        {
            string _boxNo = string.Empty;
            int i = 0;


            foreach (AssignBoxTrOUT item in _assignBoxList)
            {
                Item _item = new Item();
                _item = repo.ItemRepository.Find(item.ItemId);

                string trimedBoxNo = _item.BoxNo.TrimStart('0');

                if (i == 0)
                {
                    _boxNo = _boxNo + _item.ItemName + "-" + trimedBoxNo + "(" + _item.Year + ")" + ", ";
                    i++;
                }
                else
                {
                    _boxNo = _boxNo + trimedBoxNo + "(" + _item.Year + ")" + ", ";
                }

            }

            return _boxNo;
        }
        #endregion

        #region Invoice Method
        public ActionResult InvoiceStatement()
        {

            var model = new InvoiceViewModel();
            List<InvoiceFieldsViewModel> fildList = new List<InvoiceFieldsViewModel>();
            if (TempData["Invoice"] != null)
            {
                model = TempData["Invoice"] as InvoiceViewModel;
                fildList = TempData["InvoiceFiledList"] as List<InvoiceFieldsViewModel>;

            }
            else
            {
                model = Session["InvoiceModel"] as InvoiceViewModel;
                fildList = Session["InvoiceFiledList"] as List<InvoiceFieldsViewModel>;
            }


            var reportViewModel = repo.InvoiceViewModelRepository.GetInvoiceReport(model, fildList);
            var renderedBytes = reportViewModel.RenderReport();
            return File(renderedBytes, reportViewModel.LastmimeType);

        }
        #endregion

        #region Box Search Report

        public ActionResult BoxSearchReport(long clientId, long deptID, int rptType)
        {



            var finalList2 = TempData["finalList"] as List<AssignBox>;
            var finalList = TempData["finalList"] as List<AssignBox>;
            if (finalList2 != null)
            {
                Session["finalList"] = TempData["finalList"] as List<AssignBox>;

            }

            else
            {
                finalList = Session["finalList"] as List<AssignBox>;
                Session["finalList"] = null;

            }





            var reportViewModel = repo.BoxLocationRepository.GetBoxSearchReport(finalList, clientId, deptID, rptType);
            var renderedBytes = reportViewModel.RenderReport();
            return File(renderedBytes, reportViewModel.LastmimeType);


        }

        public ActionResult BoxSearchReportFile(long clientId, long deptID, int rptType)
        {
            var finalList = TempData["finalList"] as List<AssignBox>;

            var reportViewModel = repo.BoxLocationRepository.GetBoxSearchReportFile(finalList, clientId, deptID, rptType);
            var renderedBytes = reportViewModel.RenderReport();
            return File(renderedBytes, reportViewModel.LastmimeType);


        }


        #endregion

        #region EmptyBox Report
        public ActionResult PrintEmptyBox(long empId)
        {
            //geting repot data from the business object
            var reportViewModel = repo.EmptyBoxRepository.GetEmptyBoxRequisitionRpt(empId);

            var renderedBytes = reportViewModel.RenderReport();

            if (reportViewModel.ViewAsAttachment)
                Response.AddHeader("content-disposition", reportViewModel.ReporExportFileName);
            return File(renderedBytes, reportViewModel.LastmimeType);

        }

        #endregion

        #region Box Movement Report

        public ActionResult BoxMovementReport(string clientName, string deptName, string boxName, string boxNo, int format)
        {
            var finalList = new List<ChangeLocation>();

            if (TempData["LocationHistory"] != null)
            {
                finalList = TempData["LocationHistory"] as List<ChangeLocation>;
            }
            else
            {
                finalList = Session["LocationHistory"] as List<ChangeLocation>;
            }



            var reportViewModel = repo.ChangeLocationRepository.GetBoxMovementReport(finalList, clientName, deptName, boxName, boxNo, format);
            var renderedBytes = reportViewModel.RenderReport();
            return File(renderedBytes, reportViewModel.LastmimeType);

        }
        #endregion

        #region Upcoming Destruction Period Report

        public ActionResult DestructionPeriodReport(int monthCount, int format)
        {
            var finalList = new List<DesPeriodReportViewModel>();

            if (TempData["itemList"] != null)
            {
                finalList = TempData["itemList"] as List<DesPeriodReportViewModel>;
            }
            else
            {
                finalList = Session["itemList"] as List<DesPeriodReportViewModel>;
            }


            var reportViewModel = repo.ItemRepository.UpcomingDestructionPeriodReport(finalList, monthCount, format);
            var renderedBytes = reportViewModel.RenderReport();
            return File(renderedBytes, reportViewModel.LastmimeType);

        }


        [HttpGet]
        public ActionResult DestructionPeriodReportAttachment(long clientId, int monthCount, int format)
        {
            var desList = new List<DesPeriodReportViewModel>();

            List<Item> itemList = repo.ItemRepository.GetDestructionPeriodByMonthCountAndClientId(monthCount, clientId);
            foreach (Item item in itemList)
            {
                DesPeriodReportViewModel desModel = new DesPeriodReportViewModel();

                desModel.BoxName = item.ItemName;
                desModel.BoxNo = item.BoxNo;
                desModel.Client = item.Client.ClientName;
                desModel.Dept = item.Department.DepartmentName;
                desModel.DesPeriod = item.DestructionPeriod;
                desModel.Unit = item.Unit;
                desModel.Year = item.Year;

                desList.Add(desModel);

            }


            var reportViewModel = repo.ItemRepository.UpcomingDestructionPeriodReport(desList, monthCount, format);
            var renderedBytes = reportViewModel.RenderReport();
            return File(renderedBytes, reportViewModel.LastmimeType);

        }

        #endregion

        #region BoxIn Report Method

        public ActionResult BoxINReport(long clientID, DateTime month, int format)
        {
            var finalList = new List<BoxINReportViewModel>();

            if (TempData["ViewModelList"] != null)
            {
                finalList = TempData["ViewModelList"] as List<BoxINReportViewModel>;
            }
            else
            {
                finalList = Session["BoxINReport"] as List<BoxINReportViewModel>;
            }


            var reportViewModel = repo.ItemRepository.BoxInReport(finalList, clientID, month, format);
            var renderedBytes = reportViewModel.RenderReport();
            return File(renderedBytes, reportViewModel.LastmimeType);

        }

        #endregion

        #region BoxOUT Report Method


        public ActionResult BoxOUTReport(long clientID, DateTime month, int format)
        {
            var finalList = new List<BoxINReportViewModel>();

            if (TempData["ViewModelList"] != null)
            {
                finalList = TempData["ViewModelList"] as List<BoxINReportViewModel>;
            }
            else
            {
                finalList = Session["BoxOUTReport"] as List<BoxINReportViewModel>;
            }

            var reportViewModel = repo.ItemRepository.BoxOUTReport(finalList, clientID, month, format);
            var renderedBytes = reportViewModel.RenderReport();
            return File(renderedBytes, reportViewModel.LastmimeType);

        }

        #endregion

        #region Delivery Pending Report Method

        public ActionResult BoxPending(long clientID, DateTime month, int format)
        {
            var finalList = new List<DelPendingBoxModel>();


            if (TempData["ViewModelList"] != null)
            {
                finalList = TempData["ViewModelList"] as List<DelPendingBoxModel>;
            }
            else
            {
                finalList = Session["BoxPending"] as List<DelPendingBoxModel>;
            }


            var reportViewModel = repo.DelPendingBoxModelRepository.DelPending(finalList, clientID, month, format);
            var renderedBytes = reportViewModel.RenderReport();
            return File(renderedBytes, reportViewModel.LastmimeType);

        }


        #endregion

        #region Barcode Mapping Report

        public ActionResult BarcodeMappingRpt(long trID, int? page)
        {

            int pageNumber = (page ?? 1);
            var reportViewModel = repo.BarcodeMappingRepository.MappingReport(trID, pageNumber);
            var renderedBytes = reportViewModel.RenderReport();
            return File(renderedBytes, reportViewModel.LastmimeType);
        }


        #endregion

        #region Report Section For previous Barcode



        public ActionResult PreviousBarcode()
        {
            var finalList = new List<BarCode>();

            if (TempData["BarcodeList"] != null)
            {
                finalList = TempData["BarcodeList"] as List<BarCode>;
            }
            else
            {
                finalList = Session["BarcodeList"] as List<BarCode>;
            }



            var reportViewModel = repo.BarCodeRepository.GetBarCodeReportPrevious(finalList);
            var renderedBytes = reportViewModel.RenderReport();
            if (reportViewModel.ViewAsAttachment)
                Response.AddHeader("content-disposition", reportViewModel.ReporExportFileName);
            return File(renderedBytes, reportViewModel.LastmimeType);
        }

        public ActionResult PreviousBarcodeMapping()
        {
            var finalList = new List<BarCode>();

            if (TempData["BarcodeList"] != null)
            {
                finalList = TempData["BarcodeList"] as List<BarCode>;
            }
            else
            {
                finalList = Session["BarcodeList"] as List<BarCode>;
            }

            var reportViewModel = repo.BarcodeMappingRepository.MappingReportPrevious(finalList);
            var renderedBytes = reportViewModel.RenderReport();
            return File(renderedBytes, reportViewModel.LastmimeType);

        }



        #endregion

        #region Inventory Report

        public ActionResult InventoryReport()
        {

            var finalList = new List<InventoryViewModel>();

            if (TempData["inventoryLst"] != null)
            {
                finalList = TempData["inventoryLst"] as List<InventoryViewModel>;
            }
            else
            {
                finalList = Session["inventoryLst"] as List<InventoryViewModel>;
            }

            string ClientName = Session["ClientName"].ToString();
            string WareHouseName = Session["WareHouseName"].ToString();

            var reportViewModel = repo.AssignBoxRepository.InventoryReportMethod(finalList, ClientName, WareHouseName);
            var renderedBytes = reportViewModel.RenderReport();
            return File(renderedBytes, reportViewModel.LastmimeType);


        }

        #endregion

    }
}