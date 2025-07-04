using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{
    public class EmptyBoxRepository : IEmptyBoxRepository
    {
        WareHouseMVCContext context;

        public EmptyBoxRepository()
            : this(new WareHouseMVCContext())
        {

        }
        public EmptyBoxRepository(WareHouseMVCContext context)
        {

            this.context = context;
        }



        public IQueryable<EmptyBox> All
        {
            get { return context.EmptyBoxes; }
        }

        public IQueryable<EmptyBox> AllIncluding(params Expression<Func<EmptyBox, object>>[] includeProperties)
        {
            IQueryable<EmptyBox> query = context.EmptyBoxes;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public EmptyBox Find(long id)
        {
            return context.EmptyBoxes.Find(id);
        }

        public void InsertOrUpdate(EmptyBox emptybox)
        {
            if (emptybox.EmptyBoxId == default(long))
            {
                // New entity
                context.EmptyBoxes.Add(emptybox);
            }
            else
            {
                // Existing entity
                context.Entry(emptybox).State = EntityState.Modified;
            }
        }

        public EmptyBoxBarcode FindEmptyBoxBarcodeByBarcodeText(string barcodeText)
        {
            return context.EmptyBoxeBarcodes.FirstOrDefault(e => e.BarCodeText == barcodeText);
        }
        public void InsertOrUpdateEmptyBoxBarcode(EmptyBoxBarcode emptyboxBarcode)
        {
            if (emptyboxBarcode.Id == default(long))
            {
                // New entity
                context.EmptyBoxeBarcodes.Add(emptyboxBarcode);
            }
            else
            {
                // Existing entity
                context.Entry(emptyboxBarcode).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var emptybox = context.EmptyBoxes.Find(id);
            context.EmptyBoxes.Remove(emptybox);
        }

        public void DeleteEmptyBoxBarcode(long id)
        {
            var emptybox = context.EmptyBoxeBarcodes.Where(e => e.EmptyBoxId == id).ToList();
            context.EmptyBoxeBarcodes.RemoveRange(emptybox);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public List<EmptyBox> GetListBtClientIDandDate(long clientID, DateTime monthStart, DateTime monthEnd)
        {
            return context.EmptyBoxes.Where(e => e.ClientID == clientID && e.RecuisitionDate >= monthStart && e.RecuisitionDate <= monthEnd).ToList();
        }


        public List<EmptyBoxViewModel> EmptyBoxViewModelList()
        {
            //this is used only to help in adding the dataset of type employee to the report definition
            return null;
        }







        public ReportViewModelForEmptyBox GetEmptyBoxReport(List<EmptyBoxViewModel> emptyBoxViewList, long clientID, DateTime month, int reportType)
        {
            var _emptyBoxList = new List<object>();


            foreach (var item in emptyBoxViewList)
            {
                _emptyBoxList.Add(item);

            }

            DateTime monthStart = month;
            DateTime monthEnd = month.AddMonths(1).AddDays(-1);


            var reportViewModel = new ReportViewModelForEmptyBox()
            {
                FileName = "~/ReportsHolder/EmptyBoxStatement.rdlc",
                Name = "Statistical Report",
                ReportDate = DateTime.Now.ToShortDateString(),
                ReportTitle = "Delivery Order Empty Cartoons Statement for the month of ",
                HostName = GetHostInfo(1),
                HostAddress = GetHostInfo(2),
                ClientName = GetClientName(clientID),
                Month = month.ToString("Y"),
                MonthStart = monthStart.ToString("dd-MM-yyyy"),
                MonthEnd = monthEnd.ToString("dd-MM-yyyy"),


                ReportLanguage = "en-US",

                Format = GetReportType(reportType),//ReportViewModelForEmptyBox.ReportFormat.PDF,
                // ViewAsAttachment = false,

            };
            reportViewModel.ReportDataSets.Add(new ReportViewModelForEmptyBox.ReportDataSet() { DataSetData = _emptyBoxList.ToList(), DatasetName = "DataSet1" });


            return reportViewModel;




        }

        private ReportViewModelForEmptyBox.ReportFormat GetReportType(int reportType)
        {
            if (reportType == 1)
                return ReportViewModelForEmptyBox.ReportFormat.PDF;
            if (reportType == 2)
                return ReportViewModelForEmptyBox.ReportFormat.Excel;
            if (reportType == 3)
                return ReportViewModelForEmptyBox.ReportFormat.Word;

            return ReportViewModelForEmptyBox.ReportFormat.PDF;
        }

        private string GetClientName(long clientID)
        {
            string _temp = "None";

            Client _client = new Client();

            _client = context.Clients.Where(c => c.ClientID == clientID).FirstOrDefault();
            if (_client != null)
            {
                _temp = _client.ClientName;
            }

            return _temp;
        }

        private string GetHostInfo(int flag)
        {
            string _temp = "None";

            HostInformation _host = new HostInformation();

            _host = context.HostInformations.FirstOrDefault();
            if (_host != null)
            {
                if (flag == 1)
                {
                    _temp = _host.Name;
                }

                if (flag == 2)
                {
                    _temp = _host.Address;
                }
            }


            return _temp;
        }
         

        public ReportViewModelForEmptyBoxRecuisition GetEmptyBoxRequisitionRpt(long empId)
        {
            var _emptyBoxList = new List<object>();

            EmptyBox emptyBox = context.EmptyBoxes.Where(e => e.EmptyBoxId == empId).FirstOrDefault();
            _emptyBoxList.Add(emptyBox);

            var UserPrinting = HttpContext.Current.User.Identity.Name;

            var reportViewModel = new ReportViewModelForEmptyBoxRecuisition()
            {
                FileName = "~/ReportsHolder/EmptyBoxReport.rdlc",
                Name = "Statistical Report",
                ReportDate = DateTime.Now,
                ReportTitle = "EmptyBox Requisition Form ",
                ClientName = emptyBox.Client.ClientName,
                ClientAtt = emptyBox.Client.Att,
                ClientPosition = emptyBox.Client.Position,
                ClientCell = emptyBox.Client.Cell,
                ClientTell = emptyBox.Client.Tell,
                ClientFax = emptyBox.Client.Fax,
                ClientEmail = emptyBox.Client.Email,

                HostName = GetHostinfoFull(1),
                HostAddress = GetHostinfoFull(2),
                HostAtt = GetHostinfoFull(3),
                HostPosition = GetHostinfoFull(4),
                HostCell = GetHostinfoFull(5),
                HostTell = GetHostinfoFull(6),
                HostFax = GetHostinfoFull(7),
                HostEmail = GetHostinfoFull(8),

                Department = emptyBox.Department.DepartmentName,
                DepartmentCode = GetDeptCode(emptyBox.Department.DepartmentCode),
                UserNamPrinting = UserPrinting,
                // ContactPerson = trIn.ContactPerson.ContactPersonName,
                HandOverByName = GetHandOverBy(emptyBox, 0),
                HandOverByAddress = GetHandOverBy(emptyBox, 1),
                HandOverByDate = emptyBox.RecuisitionDate.ToShortDateString(),
                // Status = GetStatus(trIn.Status),//trIn.Status.ToString(),
                ReceivedByName = emptyBox.DeliverTo,
                ReceivedByAddress = emptyBox.Address,
                ReceivedByPosition = emptyBox.Position,
                ReceivedByDate = emptyBox.RecuisitionDate.ToShortDateString(),
                EmptyBoxDate = emptyBox.RecuisitionDate.ToShortDateString(),
                //ClientReqRef = GetClientReq(trIn.ClientRequestreference),//  trIn.ClientRequestreference,
                //IssuedBy = trIn.ContactPerson.ContactPersonName,
                //Year=year,
                //ProjectCode = GetProjectCode(trIn),
                Qty = emptyBox.NoofBoxes.ToString(),
                Unit = "Box",
                Item = "Box",
                BoxNoArr = "Empty Boxes",
                EmptyboxNo = emptyBox.EmptyBoxNo,
                ReportLanguage = "en-US",

                Format = ReportViewModelForEmptyBoxRecuisition.ReportFormat.PDF,
                // ViewAsAttachment = false,

            };
            reportViewModel.ReportDataSets.Add(new ReportViewModelForEmptyBoxRecuisition.ReportDataSet() { DataSetData = _emptyBoxList.ToList(), DatasetName = "DataSet1" });


            return reportViewModel;
        }

        private string GetHandOverBy(EmptyBox emptyBox, int p)
        {

            string _text = "None";
            if (p == 0)
            {
                _text = emptyBox.ORBLOperator.Name;
                if (_text == null)
                {
                    _text = "None";
                }
            }

            else if (p == 1)
            {
                _text = emptyBox.ORBLOperator.Address;
                if (_text == null)
                {
                    _text = "None";
                }
            }



            else
            {
            }

            return _text;
        }

        private string GetDeptCode(string p)
        {
            string _code = "None";
            if (p != string.Empty)
            {
                if (p != null)
                {
                    _code = p;
                }
            }

            return _code;
        }

        private string GetHostinfoFull(int p)
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

    public interface IEmptyBoxRepository : IDisposable
    {
        IQueryable<EmptyBox> All { get; }
        IQueryable<EmptyBox> AllIncluding(params Expression<Func<EmptyBox, object>>[] includeProperties);
        EmptyBox Find(long id);
        void InsertOrUpdate(EmptyBox emptybox);
        void Delete(long id);
        void Save();
    }
}