using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;

namespace WareHouseMVC.Controllers
{
    public class BillsController : Controller
    {
        //
        // GET: /Bills/


        UnitOfWork repo = new UnitOfWork();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MonthlyBoxStatement()
        {
            List<Client> _clientList = new List<Client>();
            _clientList = repo.ClientRepository.GetAll();

            ViewBag.ClientList = _clientList;
            BoxStatementViewModel _boxStatement = new BoxStatementViewModel();
            return View(_boxStatement);
        }

        [HttpPost]
        public ActionResult MonthlyBoxStatement(BoxStatementViewModel _boxStatement)
        {
            if (ModelState.IsValid)
            {
                long _clientID = _boxStatement.ClientID;
                DateTime _month = _boxStatement.Month;
                double _rate = _boxStatement.Rate;
                int _reportType = _boxStatement.ReportType.Value;

                return RedirectToAction("BoxStatement", "Reports", new { clientID = _clientID, month = _month, rate = _rate, reportType = _reportType });

            }
            else
            {
                return RedirectToAction("MonthlyBoxStatement");
            }


        }


        public ActionResult MonthlyEmptyBoxStatement()
        {
            List<Client> _clientList = new List<Client>();
            _clientList = repo.ClientRepository.GetAll();

            ViewBag.ClientList = _clientList;
            BoxStatementViewModel _boxStatement = new BoxStatementViewModel();
            return View(_boxStatement);
        }

        [HttpPost]
        public ActionResult MonthlyEmptyBoxStatement(BoxStatementViewModel _boxStatement)
        {
            if (ModelState.IsValid)
            {
                long _clientID = _boxStatement.ClientID;
                DateTime _month = _boxStatement.Month;
                double _rate = _boxStatement.Rate;
                int _reportType = _boxStatement.ReportType.Value;

                return RedirectToAction("EmptyBoxStatement", "Reports", new { clientID = _clientID, month = _month, rate = _rate, reportType = _reportType });

            }
            else
            {
                return RedirectToAction("MonthlyEmptyBoxStatement");
            }


        }

        public ActionResult MonthlyBoxINStatement()
        {
            List<Client> _clientList = new List<Client>();
            _clientList = repo.ClientRepository.GetAll();

            ViewBag.ClientList = _clientList;
            BoxStatementViewModel _boxStatement = new BoxStatementViewModel();
            return View(_boxStatement);
        }

        [HttpPost]
        public ActionResult MonthlyBoxINStatement(BoxStatementViewModel _boxStatement)
        {
            if (ModelState.IsValid)
            {
                long _clientID = _boxStatement.ClientID;
                DateTime _month = _boxStatement.Month;
                double _rate = _boxStatement.Rate;
                long _baseBox = _boxStatement.BaseBoxNo;
                double _extraRate = _boxStatement.ExtraRate;
                int _reportType = _boxStatement.ReportType.Value;
                return RedirectToAction("BoxINStatement", "Reports", new { clientID = _clientID, month = _month, rate = _rate, baseBox = _baseBox, extraRate = _extraRate, reportType = _reportType });

            }
            else
            {
                return RedirectToAction("MonthlyBoxINStatement");
            }


        }

        public ActionResult MonthlyBoxOUTStatement()
        {
            List<Client> _clientList = new List<Client>();
            _clientList = repo.ClientRepository.GetAll();

            ViewBag.ClientList = _clientList;
            BoxStatementViewModel _boxStatement = new BoxStatementViewModel();
            return View(_boxStatement);
        }

        [HttpPost]
        public ActionResult MonthlyBoxOUTStatement(BoxStatementViewModel _boxStatement)
        {
            if (ModelState.IsValid)
            {
                long _clientID = _boxStatement.ClientID;
                DateTime _month = _boxStatement.Month;
                double _rate = _boxStatement.Rate;
                long _baseBox = _boxStatement.BaseBoxNo;
                double _extraRate = _boxStatement.ExtraRate;
                int _reportType = _boxStatement.ReportType.Value;
                return RedirectToAction("BoxIOUTStatement", "Reports", new { clientID = _clientID, month = _month, rate = _rate, baseBox = _baseBox, extraRate = _extraRate, reportType = _reportType });

            }
            else
            {
                return RedirectToAction("MonthlyBoxOUTStatement");
            }
        }


        public ActionResult Invoice()
        {
            List<Client> _clientList = new List<Client>();
            _clientList = repo.ClientRepository.GetAll();

            ViewBag.ClientList = _clientList;
            InvoiceViewModel _invoice = new InvoiceViewModel();
            return View(_invoice);
        }

        [HttpPost]
        public ActionResult Invoice(InvoiceViewModel model)
        {
            long PreviousMonthClosingBox = 0;
            long CurrentMonthTransmittalIN = 0;
            long CurrentMonthTransmittalOut = 0;
            long ClosingBalanceCurrentMonth = 0;

            if (model.ClientID != 0 && model.ClientID != -1)
            {
                if (model.Month != default(DateTime))
                {
                    Client _client = new Client();
                    _client = repo.ClientRepository.Find(model.ClientID);
                    ViewBag.ClientName = _client.ClientName;
                    ViewBag.Month = model.Month.ToString("y");
                    ViewBag.InvoiceMonth = model.Month;
                    ViewBag.InvoiceNo = GetInvoiceNo();
                    ViewBag.ClientId = model.ClientID;

                    ClientBillingInfo billingInfo = new ClientBillingInfo();
                    billingInfo = repo.ClientBillingInfoRepository.GetByClientId(model.ClientID);

                    if (billingInfo != null)
                    {
                        ViewBag.BankName = billingInfo.BankName;
                        ViewBag.BankAddress = billingInfo.BankAddress;
                        ViewBag.BankTel = billingInfo.BankTel;
                        ViewBag.BankAcc = billingInfo.BankACC;
                        ViewBag.BranchName = billingInfo.BranchName;
                        ViewBag.SwiftCode = billingInfo.BankSwiftCode;
                    }

                    #region Get Box Bill From System

                    DateTime previousMonth = model.Month.AddMonths(-1);
                    InvoiceViewModel _previousMonth = repo.InvoiceViewModelRepository.GetByClientandMonth(model.ClientID, previousMonth);
                    if (_previousMonth != null)
                    {
                        PreviousMonthClosingBox = _previousMonth.ClosingBalance.Value;
                    }

                    DateTime monthStart = model.Month;
                    DateTime monthEnd = model.Month.AddMonths(1).AddDays(-1);


                    List<AssignBox> assignBoxListIN = new List<AssignBox>();
                    assignBoxListIN = repo.AssignBoxRepository.GetByClientIDandDateRange(model.ClientID, monthStart, monthEnd);
                    //List<TransmittalIN> TransmittalInList = new List<TransmittalIN>();
                    //TransmittalInList = repo.TransmittalINRepository.GetByClientIDandDateRange(model.ClientID, monthStart, monthEnd);

                    //foreach (var trIns in TransmittalInList)
                    //{
                    CurrentMonthTransmittalIN = CurrentMonthTransmittalIN + assignBoxListIN.Count;// trIns.Items.Count;

                    // }

                    //List<TransmittalOUT> TransmittalOUTList = new List<TransmittalOUT>();
                    //TransmittalOUTList = repo.TransmittalOUTRepository.GetByClientIDandDateRange(model.ClientID, monthStart, monthEnd);

                    List<AssignBoxTrOUT> assignBoxListOUT = new List<AssignBoxTrOUT>();
                    assignBoxListOUT = repo.AssignBoxTrOUTRepository.GetByClientIDandDateRange(model.ClientID, monthStart, monthEnd);



                    //foreach (var trOUts in TransmittalOUTList)
                    //{
                    CurrentMonthTransmittalOut = CurrentMonthTransmittalOut + assignBoxListOUT.Count;// trOUts.Items.Count;

                    //  }


                    ClosingBalanceCurrentMonth = PreviousMonthClosingBox + (CurrentMonthTransmittalIN - CurrentMonthTransmittalOut);
                    model.ClosingBalance = ClosingBalanceCurrentMonth;
                    model.TransmittalINBalance = CurrentMonthTransmittalIN;
                    model.TransmittalOUTBalance = CurrentMonthTransmittalOut;

                    ViewBag.TransmittalINBal = CurrentMonthTransmittalIN;
                    ViewBag.TransmittalOutBal = CurrentMonthTransmittalOut;
                    ViewBag.ClosingBal = ClosingBalanceCurrentMonth;


                    ViewBag.ClosingBalancPreviousMonthe = "Closing Balance of Month " + previousMonth.ToString("y") + " ---(" + PreviousMonthClosingBox.ToString() + ")";
                    ViewBag.CurrentMonthTransmittalIN = "Add Transmittal In From Current Month " + model.Month.ToString("y") + " ---(" + CurrentMonthTransmittalIN.ToString() + ")";
                    ViewBag.CurrentMonthTransmittalOut = "Less Transmittal Out From Month " + model.Month.ToString("y") + " ---(" + CurrentMonthTransmittalOut.ToString() + ")";
                    ViewBag.ClosingBalance = "Closing for Month " + model.Month.ToString("y");




                    #endregion




                    return View("InvoiceView", model);

                }
                else
                {
                }


            }
            else
            {

            }

            return RedirectToAction("Invoice");
        }

        [HttpPost]
        public ActionResult GenerateInvoice(InvoiceViewModel model, string Month)
        {

            #region SaveInvoice

            InvoiceViewModel newModel = new InvoiceViewModel();
            newModel = repo.InvoiceViewModelRepository.GetByClientandMonth(model.ClientID, Convert.ToDateTime(Month));
            if (newModel == null)
            {
                newModel = new InvoiceViewModel();
            }

            newModel.BankACC = model.BankACC;
            newModel.BankAddress = model.BankAddress;
            newModel.BankName = model.BankName;
            newModel.BankTel = model.BankTel;
            newModel.BranchName = model.BranchName;
            newModel.Client = model.Client;
            newModel.ClientID = model.ClientID;
            newModel.ClosingBalance = model.ClosingBalance;

            if (string.IsNullOrEmpty(model.ContactNo))
                newModel.ContactNo = "N/A";
            else
            newModel.ContactNo = model.ContactNo;

            newModel.FileTransferDes = model.FileTransferDes;
            newModel.FileTransferPrice = model.FileTransferPrice;
            newModel.FileTransferQty = model.FileTransferQty;
            if (model.GRDate == DateTime.MinValue)
                model.GRDate = DateTime.Now;
            newModel.GRDate = model.GRDate;


            if (string.IsNullOrEmpty(model.GRNo))
                newModel.GRNo = "N/A";
            else
            newModel.GRNo = model.GRNo;

            newModel.InvoiceDate = DateTime.Now;//datemodel.InvoiceDate;
            newModel.InvoiceNo = model.InvoiceNo;
            // newModel.InvoiceViewModelId = model.InvoiceViewModelId;
            newModel.MagneticTapeDes = model.MagneticTapeDes;
            newModel.MagneticTapePrice = model.MagneticTapePrice;
            newModel.MagneticTapeQty = model.MagneticTapeQty;
            newModel.Month = model.Month;

            if (string.IsNullOrEmpty(model.Note))
                newModel.Note = "N/A";
            else
                newModel.Note = model.Note;

            newModel.OthersDes = model.OthersDes;
            newModel.OthersPrice = model.OthersPrice;
            newModel.OthersQty = model.OthersQty;


            if (string.IsNullOrEmpty(model.PaymentTerm))
                newModel.PaymentTerm = "N/A";
            else
            newModel.PaymentTerm = model.PaymentTerm;

            newModel.PhotocopyDes = model.PhotocopyDes;
            newModel.PhotocopyPrice = model.PhotocopyPrice;
            newModel.PhotocopyQty = model.PhotocopyQty;
            if (model.PurchaseOrderDate == DateTime.MinValue)
                model.PurchaseOrderDate = DateTime.Now;
            newModel.PurchaseOrderDate = model.PurchaseOrderDate;

            if (string.IsNullOrEmpty(model.PurchaseOrderNo))
                newModel.PurchaseOrderNo = "N/A";
            else
            newModel.PurchaseOrderNo = model.PurchaseOrderNo;


            if (model.RevisionDate == DateTime.MinValue)
                model.RevisionDate = DateTime.Now;
            newModel.RevisionDate = model.RevisionDate;

            if (string.IsNullOrEmpty(model.RevisionNo))
                newModel.RevisionNo = "N/A";// model.RevisionNo;

            else
                newModel.RevisionNo = model.RevisionNo;


           


            newModel.ScanningDes = model.ScanningDes;
            newModel.ScanningPrice = model.ScanningPrice;
            newModel.ScanningQty = model.ScanningQty;
            newModel.SwiftCode = model.SwiftCode;

            if (string.IsNullOrEmpty(model.TAXRegNo))
                newModel.TAXRegNo = "N/A";
            else
            newModel.TAXRegNo = model.TAXRegNo;
            newModel.TotalAmount = model.TotalAmount;
            newModel.TotalFinalAmount = model.TotalFinalAmount;
            newModel.TotalPayable = model.TotalPayable;
            newModel.TransmittalINBalance = model.TransmittalINBalance;
            newModel.TransmittalOUTBalance = model.TransmittalOUTBalance;
            newModel.TransportationEmptyBoxDes = model.TransportationEmptyBoxDes;
            newModel.TransportationEmptyBoxPrice = model.TransportationEmptyBoxPrice;

            newModel.TransportationEmptyBoxQty = model.TransportationEmptyBoxQty;
            newModel.TransportationEmptyCartoonDes = model.TransportationEmptyCartoonDes;
            newModel.TransportationEmptyCartoonPrice = model.TransportationEmptyCartoonPrice;
            newModel.TransportationEmptyCartoonQty = model.TransportationEmptyCartoonQty;
            newModel.TransportationInChrgeDes = model.TransportationInChrgeDes;
            newModel.TransportationInChrgePrice = model.TransportationInChrgePrice;
            newModel.TransportationInChrgeQty = model.TransportationInChrgeQty;
            newModel.TransportationOUTChrgeDes = model.TransportationOUTChrgeDes;
            newModel.TransportationOUTChrgePrice = model.TransportationOUTChrgePrice;
            newModel.TransportationOUTChrgeQty = model.TransportationOUTChrgeQty;
            newModel.UnitPrice = model.UnitPrice;
            newModel.VAT = model.VAT;


            if (string.IsNullOrEmpty(model.VATRegNo))
                newModel.VATRegNo = "N/A";
            else

            newModel.VATRegNo = model.VATRegNo;
            newModel.WasteSampleDes = model.WasteSampleDes;

            newModel.WasteSamplePrice = model.WasteSamplePrice;
            newModel.WasteSampleQty = model.WasteSampleQty;
            newModel.VATCalculatedAmount = model.VATCalculatedAmount;

            repo.InvoiceViewModelRepository.InsertOrUpdate(newModel);
            repo.InvoiceViewModelRepository.Save();

            
            #endregion

            #region Fill List

            List<InvoiceFieldsViewModel> fildList = new List<InvoiceFieldsViewModel>();

            if (model.TransportationInChrgePrice.Value != 0 && model.TransportationInChrgeQty.Value != 0)
            {
                InvoiceFieldsViewModel fieldModel = new InvoiceFieldsViewModel();
                fieldModel.ItemName = "Transportation In Charge";
                fieldModel.Description = model.TransportationInChrgeDes;
                fieldModel.Quantity = model.TransportationInChrgeQty.Value.ToString();
                fieldModel.Amount = model.TransportationInChrgePrice.Value.ToString();
                fildList.Add(fieldModel);

            }

            if (model.TransportationOUTChrgePrice.Value != 0 && model.TransportationOUTChrgeQty.Value != 0)
            {
                InvoiceFieldsViewModel fieldModel = new InvoiceFieldsViewModel();
                fieldModel.ItemName = "Transportation OUT Charge";
                fieldModel.Description = model.TransportationOUTChrgeDes;
                fieldModel.Quantity = model.TransportationOUTChrgeQty.Value.ToString();
                fieldModel.Amount = model.TransportationOUTChrgePrice.Value.ToString();
                fildList.Add(fieldModel);

            }

            if (model.TransportationEmptyBoxPrice.Value != 0 && model.TransportationEmptyBoxQty.Value != 0)
            {
                InvoiceFieldsViewModel fieldModel = new InvoiceFieldsViewModel();
                fieldModel.ItemName = "Transportation Empty Box Charge";
                fieldModel.Description = model.TransportationEmptyBoxDes;
                fieldModel.Quantity = model.TransportationEmptyBoxQty.Value.ToString();
                fieldModel.Amount = model.TransportationEmptyBoxPrice.Value.ToString();
                fildList.Add(fieldModel);

            }

            if (model.TransportationEmptyCartoonPrice.Value != 0 && model.TransportationEmptyCartoonQty.Value != 0)
            {
                InvoiceFieldsViewModel fieldModel = new InvoiceFieldsViewModel();
                fieldModel.ItemName = "Empty Cartoon";
                fieldModel.Description = model.TransportationEmptyCartoonDes;
                fieldModel.Quantity = model.TransportationEmptyCartoonQty.Value.ToString();
                fieldModel.Amount = model.TransportationEmptyCartoonPrice.Value.ToString();
                fildList.Add(fieldModel);

            }

            if (model.FileTransferPrice.Value != 0 && model.FileTransferQty.Value != 0)
            {
                InvoiceFieldsViewModel fieldModel = new InvoiceFieldsViewModel();
                fieldModel.ItemName = "File Transfer / Movement";
                fieldModel.Description = model.FileTransferDes;
                fieldModel.Quantity = model.FileTransferQty.Value.ToString();
                fieldModel.Amount = model.FileTransferPrice.Value.ToString();
                fildList.Add(fieldModel);

            }

            if (model.PhotocopyPrice.Value != 0 && model.PhotocopyQty.Value != 0)
            {
                InvoiceFieldsViewModel fieldModel = new InvoiceFieldsViewModel();
                fieldModel.ItemName = "Photocopy";
                fieldModel.Description = model.PhotocopyDes;
                fieldModel.Quantity = model.PhotocopyQty.Value.ToString();
                fieldModel.Amount = model.PhotocopyPrice.Value.ToString();
                fildList.Add(fieldModel);

            }

            if (model.WasteSamplePrice.Value != 0 && model.WasteSampleQty.Value != 0)
            {
                InvoiceFieldsViewModel fieldModel = new InvoiceFieldsViewModel();
                fieldModel.ItemName = "Archiving of washed sample";
                fieldModel.Description = model.WasteSampleDes;
                fieldModel.Quantity = model.WasteSampleQty.Value.ToString();
                fieldModel.Amount = model.WasteSamplePrice.Value.ToString();
                fildList.Add(fieldModel);

            }

            if (model.MagneticTapePrice.Value != 0 && model.MagneticTapeQty.Value != 0)
            {
                InvoiceFieldsViewModel fieldModel = new InvoiceFieldsViewModel();
                fieldModel.ItemName = "Magnetic Tapes";
                fieldModel.Description = model.MagneticTapeDes;
                fieldModel.Quantity = model.MagneticTapeQty.Value.ToString();
                fieldModel.Amount = model.MagneticTapePrice.Value.ToString();
                fildList.Add(fieldModel);

            }

            if (model.ScanningPrice.Value != 0 && model.ScanningQty.Value != 0)
            {
                InvoiceFieldsViewModel fieldModel = new InvoiceFieldsViewModel();
                fieldModel.ItemName = "Imaging / Scanning";
                fieldModel.Description = model.ScanningDes;
                fieldModel.Quantity = model.ScanningQty.Value.ToString();
                fieldModel.Amount = model.ScanningPrice.Value.ToString();
                fildList.Add(fieldModel);

            }

            if (model.OthersPrice.Value != 0 && model.OthersQty.Value != 0)
            {
                InvoiceFieldsViewModel fieldModel = new InvoiceFieldsViewModel();
                fieldModel.ItemName = "Others";
                fieldModel.Description = model.OthersDes;
                fieldModel.Quantity = model.OthersQty.Value.ToString();
                fieldModel.Amount = model.OthersPrice.Value.ToString();
                fildList.Add(fieldModel);

            }



            #endregion

            TempData["Invoice"] = newModel;
            TempData["InvoiceFiledList"] = fildList;
            Session["InvoiceModel"] = TempData["Invoice"] as InvoiceViewModel;
            Session["InvoiceFiledList"] = TempData["InvoiceFiledList"] as List<InvoiceFieldsViewModel>;
            return RedirectToAction("InvoiceStatement", "Reports");
        }

        private string GetInvoiceNo()
        {
            return "ORBL-Inv201503001";
        }
    }
}
