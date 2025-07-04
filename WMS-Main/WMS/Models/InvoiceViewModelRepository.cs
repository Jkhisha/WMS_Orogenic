using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{
    public class InvoiceViewModelRepository : IInvoiceViewModelRepository
    {
        WareHouseMVCContext context;

        public InvoiceViewModelRepository()
            : this(new WareHouseMVCContext())
        {

        }
        public InvoiceViewModelRepository(WareHouseMVCContext context)
        {

            this.context = context;
        }



        public IQueryable<InvoiceViewModel> All
        {
            get { return context.InvoiceViewModels; }
        }

        public IQueryable<InvoiceViewModel> AllIncluding(params Expression<Func<InvoiceViewModel, object>>[] includeProperties)
        {
            IQueryable<InvoiceViewModel> query = context.InvoiceViewModels;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public InvoiceViewModel Find(long id)
        {
            return context.InvoiceViewModels.Find(id);
        }

        public void InsertOrUpdate(InvoiceViewModel invoiceviewmodel)
        {
            if (invoiceviewmodel.InvoiceViewModelId == default(long))
            {
                // New entity
                context.InvoiceViewModels.Add(invoiceviewmodel);
            }
            else
            {
                // Existing entity
                context.Entry(invoiceviewmodel).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var invoiceviewmodel = context.InvoiceViewModels.Find(id);
            context.InvoiceViewModels.Remove(invoiceviewmodel);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public InvoiceViewModel GetByClientandMonth(long clientId, DateTime previousMonth)
        {
            return context.InvoiceViewModels.Where(i => i.ClientID == clientId && i.Month == previousMonth).FirstOrDefault();
        }

        public List<InvoiceViewModel> Items()
        {
            //this is used only to help in adding the dataset of type employee to the report definition
            return null;
        }
        public List<InvoiceFieldsViewModel> InvoiceFiledList()
        {
            //this is used only to help in adding the dataset of type employee to the report definition
            return null;
        }

        public ReportViewModelForInvoice GetInvoiceReport(InvoiceViewModel model,List<InvoiceFieldsViewModel> filedList)
        {


               var iTemList = new List<object>();
            iTemList.Add(model);
            var iTemList2 = new List<object>();
            foreach (var item in filedList)
            {
                iTemList2.Add(item);
                
            }

            long PreviousMonthClosingBox=0;

            DateTime previousMonth = model.Month.AddMonths(-1);
            InvoiceViewModel _previousMonth = context.InvoiceViewModels.Where(i => i.ClientID == model.ClientID && i.Month == previousMonth).FirstOrDefault();
            if (_previousMonth != null)
            {
                PreviousMonthClosingBox = _previousMonth.ClosingBalance.Value;
            }

           

            var reportViewModel = new ReportViewModelForInvoice()
            {
                FileName = "~/ReportsHolder/Invoice.rdlc",
                Name = "Statistical Report",
                ReportDate = DateTime.Now,
                ReportTitle = "Invoice ",


                ClientName = GetClient(model,1),
                ClientAtt = GetClientAtt(model.ClientID),
                ClientPosition = GetClientPos(model.ClientID),
                ClientCell = GetClient(model, 4),
                ClientTell = GetClient(model, 5),
                ClientFax = GetClient(model, 6),
                ClientEmail = GetClient(model, 7),

                HostName = GetHostinfo(1),
                HostAddress = GetHostinfo(2),
                HostAtt = GetHostinfo(3),
                HostPosition = GetHostinfo(4),
                HostCell = GetHostinfo(5),
                HostTell = GetHostinfo(6),
                HostFax = GetHostinfo(7),
                HostEmail = GetHostinfo(8),
                



                  InvoiceNo =model.InvoiceNo,
                  InvoiceDate =model.InvoiceDate.ToShortDateString(),
                  InvoiceMonth=model.Month.ToString("m"),

                ReservationNo = GetTextValidate(model.RevisionNo),
                  ReservationDate =GetDateValidate(model.RevisionDate), 
                  PONo = GetTextValidate(model.PurchaseOrderNo.ToString()),
                  PODate =GetDateValidate(model.PurchaseOrderDate),
                  VATRegNo = GetTextValidate(model.VATRegNo),
                  GRDate = GetDateValidate(model.GRDate),
                  Note= GetTextValidate(model.Note),

                  BankName=model.BankName,
                  BankAddress=model.BankAddress,
                  BankAcc=model.BankACC,
                  BankTel=model.BankTel,
                  BranchName=model.BranchName,
                  SWIFT=model.SwiftCode,

                  ClosingBalancePreviousMonth= "Closing Balance of Month " + previousMonth.ToString("y") + " ---(" + PreviousMonthClosingBox.ToString() + ")",
                  CurrentMonthTransmittalIN = "Add Transmittal In From Current Month " + model.Month.ToString("y") + " ---(" + model.ClosingBalance.ToString() + ")",
                  CurrentMonthTransmittalOut = "Less Transmittal Out From Month " + model.Month.ToString("y") + " ---(" + model.TransmittalOUTBalance.ToString() + ")",
                  ClosingBalanceCurrentMonth="Closing for Month " + model.Month.ToString("y"),
             
                ReportLanguage = "en-US",

                Format = ReportViewModelForInvoice.ReportFormat.PDF,
                ViewAsAttachment = false,

            };
            reportViewModel.ReportDataSets.Add(new ReportViewModelForInvoice.ReportDataSet() { DataSetData = iTemList.ToList(), DatasetName = "DataSet1" });
            reportViewModel.ReportDataSets.Add(new ReportViewModelForInvoice.ReportDataSet() { DataSetData = iTemList2.ToList(), DatasetName = "DataSet2" });


            return reportViewModel;
        }

        private string GetClientPos(long clientId)
        {
            string returnText = "N/A";
            ClientBillingInfo client = new ClientBillingInfo();
            client = context.ClientBillingInfoes.Where(c => c.ClientID == clientId).FirstOrDefault();

            if (!string.IsNullOrEmpty(client.ClientAttPosition))
                returnText = client.ClientAttPosition;

            return returnText;
        }

        private string GetClientAtt(long clientId)
        {
            string returnText="N/A";
            ClientBillingInfo client = new ClientBillingInfo();
            client = context.ClientBillingInfoes.Where(c => c.ClientID == clientId).FirstOrDefault();

            if (!string.IsNullOrEmpty(client.ClientAtt))
                returnText = client.ClientAtt;

            return returnText;


        }

        private string GetDateValidate(DateTime dateTime)
        {
            string returnText = "N/A";
            if (dateTime != DateTime.MinValue)
                returnText = dateTime.ToShortDateString();

            return returnText;
        }

        private string GetTextValidate(string text)
        {
            string returnText = "N/A";
            if (!string.IsNullOrEmpty(text))
                returnText = text;

            return returnText;
        }

        private string GetClient(InvoiceViewModel model, int p)
        {



            string _info = "-";

            Client _hostInfo = new Client();
            _hostInfo = context.Clients.Where(c => c.ClientID == model.ClientID).FirstOrDefault();

            if (p == 1)
            {
                _info = _hostInfo.ClientName;
            }
            if (p == 2)
            {
                _info = _hostInfo.Att;
            }
            if (p == 3)
            {
                _info = _hostInfo.Position;
            }
            if (p == 4)
            {
                _info = _hostInfo.Cell;
            }
            if (p == 5)
            {
                _info = _hostInfo.Tell;
            }
            if (p == 6)
            {
                _info = _hostInfo.Fax;
            }
            if (p == 7)
            {
                _info = _hostInfo.Email;
            }


            return _info;
        }

        private string GetClientName(InvoiceViewModel model)
        {
            return context.Clients.Where(c => c.ClientID == model.ClientID).FirstOrDefault().ClientName;
        }

        private string GetHostinfo(int p)
        {


            string _info = "-";

            HostInformation _hostInfo = new HostInformation();
            _hostInfo = context.HostInformations.FirstOrDefault();

            if (p == 1)
            {
                _info = _hostInfo.Name;
            }
            if (p == 2)
            {
                _info = _hostInfo.Address;
            }
            if (p == 3)
            {
                _info = _hostInfo.Att;
            }
            if (p == 4)
            {
                _info = _hostInfo.Position;
            }
            if (p == 5)
            {
                _info = _hostInfo.Cell;
            }
            if (p == 6)
            {
                _info = _hostInfo.Teliphone;
            }
            if (p == 7)
            {
                _info = _hostInfo.Fax;
            }
            if (p == 8)
            {
                _info = _hostInfo.Email;
            }

            return _info;
        }


    }

    public interface IInvoiceViewModelRepository : IDisposable
    {
        IQueryable<InvoiceViewModel> All { get; }
        IQueryable<InvoiceViewModel> AllIncluding(params Expression<Func<InvoiceViewModel, object>>[] includeProperties);
        InvoiceViewModel Find(long id);
        void InsertOrUpdate(InvoiceViewModel invoiceviewmodel);
        void Delete(long id);
        void Save();
    }
}