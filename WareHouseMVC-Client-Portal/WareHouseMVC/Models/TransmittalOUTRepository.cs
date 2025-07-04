using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using PagedList;

namespace WareHouseMVC.Models
{ 
    public class TransmittalOUTRepository : ITransmittalOUTRepository
    {
        WareHouseMVCContext context;

         public  TransmittalOUTRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public TransmittalOUTRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<TransmittalOUT> All
        {
            get { return context.TransmittalOUTs; }
        }

        public IQueryable<TransmittalOUT> AllIncluding(params Expression<Func<TransmittalOUT, object>>[] includeProperties)
        {
            IQueryable<TransmittalOUT> query = context.TransmittalOUTs;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public TransmittalOUT Find(long id)
        {
            return context.TransmittalOUTs.Find(id);
        }

        public void InsertOrUpdate(TransmittalOUT transmittalout)
        {
            if (transmittalout.TransmittalOUTId == default(long))
            {
                context.TransmittalOUTs.Add(transmittalout);
            }
            else
            {
                var existingEntity = context.TransmittalOUTs.Find(transmittalout.TransmittalOUTId);

                if (existingEntity != null)
                {
                    context.Entry(existingEntity).CurrentValues.SetValues(transmittalout);
                }
                else
                {
                    context.TransmittalOUTs.Attach(transmittalout);
                    context.Entry(transmittalout).State = EntityState.Modified;
                }
            }
        }


        public void Delete(long id)
        {
            var transmittalout = context.TransmittalOUTs.Find(id);
            context.TransmittalOUTs.Remove(transmittalout);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

        public  List<Item> GetAllItemsByTrID(long trId)
        {
            return context.TransmittalOUTs.Where(t => t.TransmittalOUTId == trId).FirstOrDefault().Items;
        }



        public  List<TransmittalOUT> GetTrOUTList(long _trID)
        {
            return context.TransmittalOUTs.Where(t => t.TransmittalOUTId == _trID).ToList();
        }


        public List<TransmittalOUT> TransmittalOUTReportList()
        {
            //this is used only to help in adding the dataset of type employee to the report definition
            return null;
        }

        public List<Item> ItemsReportList()
        {
            //this is used only to help in adding the dataset of type employee to the report definition
            return null;
        }

        public List<GatePassViewModel> GatePassList()
        {
            //this is used only to help in adding the dataset of type employee to the report definition
            return null;
        }



        public ReportViewModelForTrOUT GetTransmittalOUTReport(long trID, int type)
        {
            var trOUTList = new List<object>();
            var itemList = new List<object>();

            string year = string.Empty;

            int qtyCount = 0;

            TransmittalOUT trOut = context.TransmittalOUTs.Where(tr => tr.TransmittalOUTId == trID).FirstOrDefault();
            trOUTList.Add(trOut);


            foreach (var item in trOut.Items)
            {
                //year = item.Year;
                qtyCount++;
                itemList.Add(item);
            }

            string _clientFax="N/A";
            try
            {
                _clientFax=trOut.Client.Fax.ToString();
            }
            catch
            {
            }
            
            //Assuming the person printing the report is me
            var UserPrinting = HttpContext.Current.User.Identity.Name;
            string clientCell = trOut.Client.Cell;
           if(string.IsNullOrEmpty (trOut.Client.Cell))
           {
               clientCell = "N/A";
            }

           string clientTell = trOut.Client.Tell;
           if (string.IsNullOrEmpty(trOut.Client.Tell))
           {
               clientTell = "N/A";
           }



          
            var reportViewModel = new ReportViewModelForTrOUT()
            {
                FileName = "~/ReportsHolder/TransmittalOUT.rdlc",
                Name = "Statistical Report",
                ReportDate = DateTime.Now,
                ReportTitle = "Transmittal Form - OUT",


                ClientName = trOut.Client.ClientName,
                ClientAtt = trOut.Client.Att,
                ClientPosition = trOut.Client.Position,
                ClientCell = clientCell,
                ClientTell = clientTell,
                ClientFax = _clientFax,
                ClientEmail = trOut.Client.Email,

                HostName = GetHostinfo(1),
                HostAddress = GetHostinfo(2),
                HostAtt = GetHostinfo(3),
                HostPosition = GetHostinfo(4),
                HostCell = GetHostinfo(5),
                HostTell = GetHostinfo(6),
                HostFax = GetHostinfo(7),
                HostEmail = GetHostinfo(8),

                Department = trOut.Department.DepartmentName,
                ClientDept=trOut.Client.Department,
                DepartmentCode = GetDeptCode( trOut.Department.DepartmentCode),
                UserNamPrinting = UserPrinting,
                // ContactPerson = trIn.ContactPerson.ContactPersonName,
                HandOverByName = GetHandOverBy(trOut, 0),
                HandOverByAddress = GetHandOverBy(trOut, 1),
                HandOverByDate = GetHandOverBy(trOut, 2),
                // Status = GetStatus(trIn.Status),//trIn.Status.ToString(),
                ReceivedByName = GetReceivedBy(trOut, 0),
                ReceivedByAddress = GetReceivedBy(trOut, 1),
                ReceivedByDate = GetReceivedBy(trOut, 2),
                TransmittalDate = trOut.TransmittalDate.ToShortDateString(),
                ClientReqRef = GetClientReq(trOut.ClientRequestreference),
                IssuedBy = trOut.ContactPerson.ContactPersonName,
                IssuedByPosition=trOut.ContactPerson.Position,
                IsUrgent = trOut.IsUrgent == true ? "Yes" : "No",
                //  Year = year,
                ProjectCode = GetProjectCode(trOut),
                Qty = qtyCount.ToString(),
                Unit = trOut.Type,
                Item = trOut.Type,
                BoxNoArr = GetBoxs(trOut),
                TransmittalNo = trOut.TransmittalNo,
                ReportLanguage = "en-US",

                Format = GetFormat(type),
                ViewAsAttachment = false,

            };
            reportViewModel.ReportDataSets.Add(new ReportViewModelForTrOUT.ReportDataSet() { DataSetData = trOUTList.ToList(), DatasetName = "DataSet1" });
            reportViewModel.ReportDataSets.Add(new ReportViewModelForTrOUT.ReportDataSet() { DataSetData = itemList.ToList(), DatasetName = "DataSet2" });


            return reportViewModel;
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

        private string GetClientReq(string p)
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

        private ReportViewModelForTrOUT.ReportFormat GetFormat(int type)
        {

            if (type == 1)
                return ReportViewModelForTrOUT.ReportFormat.PDF;

            if (type == 2)
                return ReportViewModelForTrOUT.ReportFormat.Excel;

            if (type == 3)
                return ReportViewModelForTrOUT.ReportFormat.Word;

            return 0;
        }

        private string GetBoxs(TransmittalOUT trOut)
        {
            string _boxNo = string.Empty;

            int i = 0;

            if (trOut.Type == "File")
            {

                foreach (var item in trOut.Items)
                {

                    string trimedBoxNo = item.BoxNo.TrimStart('0');

                    if (i == 0)
                    {
                        if (item.Year == null || item.Year == string.Empty)
                        {
                            _boxNo = _boxNo + trimedBoxNo + "-" + item.FileBoxName + "-" + item.FileNumber + "-" + item.RingNo + " , ";
                        }
                        else
                        {
                            _boxNo = _boxNo + trimedBoxNo + "-" + item.FileBoxName + "-" + item.FileNumber + "-" + item.RingNo + "(" + item.Year + ")" + ", ";
                        }
                        i++;
                    }
                    else
                    {
                        if (item.Year == null || item.Year == string.Empty)
                        {
                            _boxNo = _boxNo + trimedBoxNo + "-" + item.FileBoxName + "-" + item.FileNumber + "-" + item.RingNo + " , ";
                        }
                        else
                        {
                            _boxNo = _boxNo + trimedBoxNo + "-" + item.FileBoxName + "-" + item.FileNumber + "-" + item.RingNo + "(" + item.Year + ")" + ", ";
                        }
                    }

                }


            }
            if (trOut.Type == "Box")
            {


                foreach (var item in trOut.Items)
                {

                    string trimedBoxNo = item.BoxNo.TrimStart('0');

                    if (i == 0)
                    {
                        if (item.Year == null || item.Year == string.Empty)
                        {
                            _boxNo = _boxNo + item.ItemName + "-" + trimedBoxNo + ", ";
                        }
                        else
                        {
                            _boxNo = _boxNo + item.ItemName + "-" + trimedBoxNo + "(" + item.Year + ")" + ", ";
                        }
                        i++;
                    }
                    else
                    {
                        if (item.Year == null || item.Year == string.Empty)
                        {
                            _boxNo = _boxNo + trimedBoxNo + ", ";
                        }
                        else
                        {
                            _boxNo = _boxNo + trimedBoxNo + "(" + item.Year + ")" + ", ";
                        }
                    }

                }
            }

            return _boxNo;
        }

        private string GetProjectCode(TransmittalOUT trOut)
        {

            string projectCode = "None";
            int flag = 1;

            if (trOut.ProjectId == -1)
            {
                flag = 0;
            }

            else if (trOut.ProjectId == null)
            {
                flag = 0;
            }


            if (flag==1)
            {
                int id = Convert.ToInt32(trOut.ProjectId);
                Project _project = context.Projects.Find(id);

                projectCode = _project.ProjectCode;
            }
            return projectCode;
        }

        private string GetReceivedBy(TransmittalOUT trOut, int p)
        {

            string _text = "None";
            if (trOut.ReceivedBy.Count > 0)
            {
                if (p == 0)
                {
                    _text = trOut.ReceivedBy[0].Name;
                    if (_text == null)
                    {
                        _text = "None";
                    }
                }

                else if (p == 1)
                {
                    _text = trOut.ReceivedBy[0].Address;

                    if (_text == null)
                    {
                        _text = "None";
                    }
                }

                else if (p == 2)
                {
                    _text = trOut.ReceivedBy[0].Date.ToShortDateString();
                    if (_text == null)
                    {
                        _text = "None";
                    }
                }


            }

            else
            {
            }

            return _text;
        }

        private string GetHandOverBy(TransmittalOUT trOut, int p)
        {
            string _text = "None";


            if (trOut.HandOverBy.Count > 0)
            {
                if (p == 0)
                {
                    _text = trOut.HandOverBy[0].Name;
                    if (_text == null)
                    {
                        _text = "None";
                    }
                }

                else if (p == 1)
                {
                    _text = trOut.HandOverBy[0].Address;
                    if (_text == null)
                    {
                        _text = "None";
                    }
                }

                else if (p == 2)
                {
                    _text = DateTime.Now.ToShortDateString();
                    if (_text == null)
                    {
                        _text = "None";
                    }
                }


            }

            else
            {
            }

            return _text;
        }

        public  ReportViewModelForGatePass GetGatePassReport(int trID, int type)
        {

            var trOUTList = new List<object>();
            var itemList = new List<object>();
            var originalList = new List<object>();

          

            string year = string.Empty;

            int qtyCount = 0;

            var originalListw = context.AssignBoxTrOUTs.Where(a => a.TransmittalOUTId == trID).GroupBy(g => g.WarehouseName).ToList();


            foreach (var item in originalListw)
            {
                GatePassViewModel gatePass = new GatePassViewModel();
                string wName = item.Key.ToString();

                List<AssignBoxTrOUT> assigntrOUTList = new List<AssignBoxTrOUT>();
                assigntrOUTList = context.AssignBoxTrOUTs.Where(ar => ar.WarehouseName == wName && ar.TransmittalOUTId == trID).ToList();

                gatePass.WareHouseName = wName;
                gatePass.Qty = assigntrOUTList.Count();
                gatePass.BoxNo = GetBoxNos(assigntrOUTList);

                originalList.Add(gatePass);
            }



            TransmittalOUT trOut = context.TransmittalOUTs.Where(tr => tr.TransmittalOUTId == trID).FirstOrDefault();
            trOUTList.Add(trOut);


            foreach (var item in trOut.Items)
            {
                //year = item.Year;
                qtyCount++;
                itemList.Add(item);
            }
            //Assuming the person printing the report is me
            var UserPrinting = HttpContext.Current.User.Identity.Name;

            var reportViewModel = new ReportViewModelForGatePass()
            {
                FileName = "~/ReportsHolder/GatePass.rdlc",
                Name = "Statistical Report",
                ReportDate = DateTime.Now.ToShortDateString(),
                ReportTitle = "Gate Pass",
                HostName =  GetHostInfo(1),
                HostAddress = GetHostInfo(2),
                HostPhone = GetHostInfo(3),
                UserNamPrinting = UserPrinting,
               
                Qty = qtyCount.ToString(),
              
                BoxNoArr = GetBoxs(trOut),
                ReportLanguage = "en-US",

                Format = GetFormatforGatePass(type),
                ViewAsAttachment = false,

            };
            reportViewModel.ReportDataSets.Add(new ReportViewModelForGatePass.ReportDataSet() { DataSetData = trOUTList.ToList(), DatasetName = "DataSet1" });
            reportViewModel.ReportDataSets.Add(new ReportViewModelForGatePass.ReportDataSet() { DataSetData = itemList.ToList(), DatasetName = "DataSet2" });
            reportViewModel.ReportDataSets.Add(new ReportViewModelForGatePass.ReportDataSet() { DataSetData = originalList.ToList(), DatasetName = "DataSet3" });


            return reportViewModel;
        }

        private string GetBoxNos(List<AssignBoxTrOUT> assigntrOUTList)
        {
            string box=string.Empty;
            foreach (AssignBoxTrOUT item in assigntrOUTList)
            {
                string _tempItemName= context.Items.Where(i => i.ItemId == item.ItemId).FirstOrDefault().ItemName;
                string _tempItemNo = context.Items.Where(i => i.ItemId == item.ItemId).FirstOrDefault().BoxNo;
                string _tempItemYear = context.Items.Where(i => i.ItemId == item.ItemId).FirstOrDefault().Year;
                string _tempBookNo = context.Items.Where(i => i.ItemId == item.ItemId).FirstOrDefault().FileBoxName;
                string _tempFileNo = context.Items.Where(i => i.ItemId == item.ItemId).FirstOrDefault().FileNumber;
                string _tempRingNo = context.Items.Where(i => i.ItemId == item.ItemId).FirstOrDefault().RingNo;

                string trimedBoxNo = _tempItemNo.TrimStart('0');
                
                    if (_tempItemYear == null || _tempItemYear == string.Empty)
                    {
                        if ( (_tempBookNo == null || _tempBookNo == string.Empty) && (_tempFileNo==null || _tempFileNo==string.Empty) && (_tempRingNo==null || _tempRingNo==string.Empty) )
                        {

                            box = box + _tempItemName + "-" + trimedBoxNo + ", ";
                        }

                        else
                        {
                            box = box + trimedBoxNo + "-" + _tempBookNo + "-" + _tempFileNo + "-" + _tempRingNo + " , ";
                        }
                    }
                    else
                    {
                        if ((_tempBookNo == null || _tempBookNo == string.Empty) && (_tempFileNo == null || _tempFileNo == string.Empty) && (_tempRingNo == null || _tempRingNo == string.Empty))
                        {

                            box = box + _tempItemName + "-" + trimedBoxNo + "(" + _tempItemYear + ")" + ", ";
                        }
                        else
                        {
                            box = box + trimedBoxNo + "-" + _tempBookNo + "-" + _tempFileNo + "-" + _tempRingNo + "(" + _tempItemYear + ")" + ", ";
                        }
                    }

                  

            }

            return box;
        }

        private string GetHostInfo(int p)
        {
            string _info = "None";

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
                _info = _hostInfo.Teliphone;
            }

            return _info;
        }

        private ReportViewModelForGatePass.ReportFormat GetFormatforGatePass(int type)
        {

            if (type == 1)
                return ReportViewModelForGatePass.ReportFormat.PDF;

            if (type == 2)
                return ReportViewModelForGatePass.ReportFormat.Excel;

            if (type == 3)
                return ReportViewModelForGatePass.ReportFormat.Word;

            return 0;
        }

        public  List<TransmittalOUT> GetByClientIDandDateRange(long clientID, DateTime monthStart, DateTime monthEnd)
        {
            return context.TransmittalOUTs.Where(t => t.ClientID == clientID && t.TransmittalDate >= monthStart && t.TransmittalDate <= monthEnd).ToList();
        }

        public  List<TransmittalOUT> GetAllDateFilter()
        {
            DateTime startDate = DateTime.Now.AddDays(-10);
            DateTime endDate = DateTime.Now;

            return context.TransmittalOUTs.Where(t => t.TransmittalDate >= startDate && t.TransmittalDate <= endDate).ToList();

        }

        public  List<TransmittalOUT> GetAllByDate(DateTime flagDate)
        {
            return context.TransmittalOUTs.Where(t => t.TransmittalDate == flagDate).ToList();
        }

        public  List<TransmittalOUT> GetAllDeliveryPending()
        {
            return context.TransmittalOUTs.Where(t => t.TransmittalOUTStatusId > 1 && t.TransmittalOUTStatusId <= 4).ToList();
        }

        public  List<TransmittalOUT> GetByClientID(long _clientID)
        {
            return context.TransmittalOUTs.Where(t => t.ClientID == _clientID).ToList();
        }

        public  PagedList.IPagedList<TransmittalOUT> GetAllPagedList(int pageNumber, int pageSize)
        {
            return context.TransmittalOUTs.OrderByDescending(o => o.TransmittalNo).ToPagedList(pageNumber, pageSize);
        }

        public  List<TransmittalOUT> GetByClientIDandDeptIDandDateRange(long clientID, long? deptID, DateTime monthStart, DateTime monthEnd)
        {
            List<TransmittalOUT> list = new List<TransmittalOUT>();

            if (deptID.HasValue)
            {
                list = context.TransmittalOUTs.Where(t => t.ClientID == clientID && t.DepartmentID == deptID.Value && t.TransmittalDate >= monthStart && t.TransmittalDate <= monthEnd).ToList();
            }
            else
            {
                list = context.TransmittalOUTs.Where(t => t.ClientID == clientID && t.TransmittalDate >= monthStart && t.TransmittalDate <= monthEnd).ToList();
            }
            return list;
        }



        public IPagedList<TransmittalOUT> GetByClientIDPagesList(long clientId, int pageNumber, int pageSize)
        {
            return context.TransmittalOUTs.Where(c => c.ClientID == clientId).OrderByDescending(o => o.TransmittalNo).ToPagedList(pageNumber, pageSize);
        }

        public IPagedList<TransmittalOUT> GetPagesListFilteredByDept(long clientId, long departmentId, string subDepartment, int pageNumber, int pageSize)
        {
            var departmentName = context.Departments.FirstOrDefault(d => d.DepartmentID == departmentId).DepartmentName;
            if (!string.Equals(departmentName, "SuperAdmin", StringComparison.OrdinalIgnoreCase))
            {
                return context.TransmittalOUTs
                    .Where(c => c.ClientID == clientId
                                && c.DepartmentID == departmentId
                                && (subDepartment == null || c.SubDepartment == subDepartment))
                    .OrderByDescending(a => a.TransmittalOUTId)
                    .ToPagedList(pageNumber, pageSize);
            }
            else
            {
                return context.TransmittalOUTs
                    .Where(c => c.ClientID == clientId)
                    .OrderByDescending(a => a.TransmittalOUTId)
                    .ToPagedList(pageNumber, pageSize);
            }
        }
    }

    public interface ITransmittalOUTRepository : IDisposable
    {
        IQueryable<TransmittalOUT> All { get; }
        IQueryable<TransmittalOUT> AllIncluding(params Expression<Func<TransmittalOUT, object>>[] includeProperties);
        TransmittalOUT Find(long id);
        void InsertOrUpdate(TransmittalOUT transmittalout);
        void Delete(long id);
        void Save();
    }
}