using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using PagedList;

namespace WareHouseMVC.Models
{
    public class TransmittalINRepository : ITransmittalINRepository
    {
        WareHouseMVCContext context;

        public TransmittalINRepository()
            : this(new WareHouseMVCContext())
        {

        }
        public TransmittalINRepository(WareHouseMVCContext context)
        {

            this.context = context;
        }



        public IQueryable<TransmittalIN> All
        {
            get { return context.TransmittalINs; }
        }

        public IQueryable<TransmittalIN> AllIncluding(params Expression<Func<TransmittalIN, object>>[] includeProperties)
        {
            IQueryable<TransmittalIN> query = context.TransmittalINs;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public TransmittalIN Find(long id)
        {
            return context.TransmittalINs.Find(id);
        }

        public void InsertOrUpdate(TransmittalIN transmittalin)
        {
            if (transmittalin.TransmittalINId == default(long))
            {
                context.TransmittalINs.Add(transmittalin);
            }
            else
            {
                var existingEntity = context.TransmittalINs.Find(transmittalin.TransmittalINId);

                if (existingEntity != null)
                {
                    context.Entry(existingEntity).CurrentValues.SetValues(transmittalin);
                }
                else
                {
                    context.TransmittalINs.Attach(transmittalin);
                    context.Entry(transmittalin).State = EntityState.Modified;
                }
            }
        }


        public void Delete(long id)
        {
            var transmittalin = context.TransmittalINs.Find(id);
            context.TransmittalINs.Remove(transmittalin);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public TransmittalIN GetTrIN(long trId)
        {
            return context.TransmittalINs.Where(t => t.TransmittalINId == trId).FirstOrDefault();
        }

        public List<TransmittalIN> GetTrINList(long _trID)
        {
            return context.TransmittalINs.Where(t => t.TransmittalINId == _trID).ToList();
        }

        public List<TransmittalIN> TransmittalIN()
        {
            //this is used only to help in adding the dataset of type employee to the report definition
            return null;
        }

        public List<Item> Items()
        {
            //this is used only to help in adding the dataset of type employee to the report definition
            return null;
        }

        public ReportViewModel GetTransmittalINReport(int trID, int type)
        {
            var trINList = new List<object>();
            var itemList = new List<object>();

            string year = string.Empty;

            int qtyCount = 0;

            TransmittalIN trIn = context.TransmittalINs.Where(tr => tr.TransmittalINId == trID).FirstOrDefault();
            trINList.Add(trIn);


            foreach (var item in trIn.Items)
            {
                // year = item.Year;
                qtyCount++;
                itemList.Add(item);
            }
            //Assuming the person printing the report is me
            var UserPrinting = HttpContext.Current.User.Identity.Name;


            string _clientFax = "N/A";
            try
            {
                _clientFax = trIn.Client.Fax.ToString();
            }
            catch
            {
            }

            string clientCell = trIn.Client.Cell;
            if (string.IsNullOrEmpty(trIn.Client.Cell))
            {
                clientCell = "N/A";
            }

            string clientTell = trIn.Client.Tell;
            if (string.IsNullOrEmpty(trIn.Client.Tell))
            {
                clientTell = "N/A";
            }





            var reportViewModel = new ReportViewModel()
            {
                FileName = "~/ReportsHolder/TransmittalIN.rdlc",
                Name = "Statistical Report",
                ReportDate = DateTime.Now,
                ReportTitle = "Transmittal Form - IN",



                ClientName = trIn.Client.ClientName,
                ClientAtt = trIn.Client.Att,
                ClientPosition = trIn.Client.Position,
                ClientCell = clientCell,
                ClientTell = clientTell,
                ClientFax = _clientFax,
                ClientEmail = trIn.Client.Email,

                HostName = GetHostinfo(1),
                HostAddress = GetHostinfo(2),
                HostAtt = GetHostinfo(3),
                HostPosition = GetHostinfo(4),
                HostCell = GetHostinfo(5),
                HostTell = GetHostinfo(6),
                HostFax = GetHostinfo(7),
                HostEmail = GetHostinfo(8),

                Department = trIn.Department.DepartmentName,
                ClientDept=trIn.Client.Department,
                DepartmentCode = GetDeptCode(trIn.Department.DepartmentCode),
                UserNamPrinting = UserPrinting,
                // ContactPerson = trIn.ContactPerson.ContactPersonName,
                HandOverByName = GetHandOverBy(trIn, 0),
                HandOverByAddress = GetHandOverBy(trIn, 1),
                HandOverByDate = GetHandOverBy(trIn, 2),
                // Status = GetStatus(trIn.Status),//trIn.Status.ToString(),
                ReceivedByName = GetReceivedBy(trIn, 0),
                ReceivedByAddress = GetReceivedBy(trIn, 1),
                ReceivedByDate = GetReceivedBy(trIn, 2),
                TransmittalDate = trIn.TransmittalDate.ToShortDateString(),
                ClientReqRef = GetClientReq(trIn.ClientRequestreference),//  trIn.ClientRequestreference,
                IssuedBy = trIn.ContactPerson.ContactPersonName,
                IssuedByPosition=trIn.ContactPerson.Position,
                //Year=year,
                ProjectCode = GetProjectCode(trIn),
                Qty = qtyCount.ToString(),
                Unit =trIn.Type,
                Item = trIn.Type,
                ArchieveItem = GetArchiveItem(trIn.TotalArchieveItem.ToString(), trIn.Items.Count),
                BoxNoArr = GetBoxs(trIn),
                TransmittalNo = trIn.TransmittalNo,
                ReportLanguage = "en-US",

                Format = GetFormat(type),
                ViewAsAttachment = false,

            };
            reportViewModel.ReportDataSets.Add(new ReportViewModel.ReportDataSet() { DataSetData = trINList.ToList(), DatasetName = "DataSet1" });
            reportViewModel.ReportDataSets.Add(new ReportViewModel.ReportDataSet() { DataSetData = itemList.ToList(), DatasetName = "DataSet2" });


            return reportViewModel;
        }

       



        private string GetArchiveItem(string reArchive, int TotalItem)
        {
            string tempFinal = string.Empty;

            int reArchItem = Convert.ToInt32(reArchive);
            int newArchiveItem = TotalItem - reArchItem;

            tempFinal = newArchiveItem.ToString() + " / " + reArchItem.ToString();
            return tempFinal;
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
        /*
        private string GetBoxs(Models.TransmittalIN trIn)
        {
            string _boxNo = string.Empty;

            int i = 0;

            if (trIn.Type == "File")
            {

                foreach (var item in trIn.Items)
                {

                    string trimedBoxNo = item.BoxNo.TrimStart('0');

                    if (i == 0)
                    {
                        if (item.Year == null || item.Year == string.Empty)
                        {
                            _boxNo = _boxNo + trimedBoxNo + "-" + item.FileBoxName + "-"+ item.FileNumber+ "-" +item.RingNo+" , ";
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
            if (trIn.Type == "Box")
            {


                foreach (var item in trIn.Items)
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
        */

    private string GetBoxs(Models.TransmittalIN trIn)
{
    string _boxNo = string.Empty;
    int i = 0;
    if (trIn.Type == "File")
    {
        foreach (var item in trIn.Items)
        {
            string trimedBoxNo = item.BoxNo.TrimStart('0');
            if (i == 0)
            {
                // Include department name for files
                if (item.Year == null || item.Year == string.Empty)
                {
                    _boxNo = _boxNo + item.ItemName + "-" + trimedBoxNo + ", ";
                }
                else
                {
                    _boxNo = _boxNo + item.ItemName + "-" + trimedBoxNo + "(" + item.Year + "), ";
                }
                i++;
            }
            else
            {
                if (item.Year == null || item.Year == string.Empty)
                {
                    _boxNo = _boxNo + item.ItemName + "-" + trimedBoxNo + ", ";
                }
                else
                {
                    _boxNo = _boxNo + item.ItemName + "-" + trimedBoxNo + "(" + item.Year + "), ";
                }
            }
        }
    }
    if (trIn.Type == "Box")
    {
        foreach (var item in trIn.Items)
        {
            string trimedBoxNo = item.BoxNo.TrimStart('0');
            if (i == 0)
            {
                if (item.Year == null || item.Year == string.Empty)
                {
                    // Ensure department name is included for boxes
                    _boxNo = _boxNo + item.ItemName + "-" + trimedBoxNo + ", ";
                }
                else
                {
                    _boxNo = _boxNo + item.ItemName + "-" + trimedBoxNo + "(" + item.Year + "), ";
                }
                i++;
            }
            else
            {
                if (item.Year == null || item.Year == string.Empty)
                {
                    _boxNo = _boxNo + item.ItemName + "-" + trimedBoxNo + ", ";
                }
                else
                {
                    _boxNo = _boxNo + item.ItemName + "-" + trimedBoxNo + "(" + item.Year + "), ";
                }
            }
        }
    }
    return _boxNo;
}

        private string GetProjectCode(Models.TransmittalIN trIn)
        {
            string projectCode = "None";
            if (trIn.ProjectId != null)
            {
                if (trIn.ProjectId != -1)
                {

                    int id = Convert.ToInt32(trIn.ProjectId);
                    Project _project = context.Projects.Find(id);

                    projectCode = _project.ProjectCode;
                }
            }
            return projectCode;
        }

        private string GetStatus(int p)
        {
            string _text = string.Empty;

            if (p == 1)
                _text = "Done";
            else if (p == 0)
                _text = "Pending";

            return _text;

        }

        private string GetReceivedBy(Models.TransmittalIN trIn, int p)
        {

            string _text = "None";
            if (trIn.ReceivedBy.Count > 0)
            {
                if (p == 0)
                {
                    _text = trIn.ReceivedBy[0].Name;
                    if (_text == null)
                    {
                        _text = "None";
                    }
                }

                else if (p == 1)
                {
                    _text = trIn.ReceivedBy[0].Address;
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

        private string GetHandOverBy(Models.TransmittalIN trIn, int p)
        {
            string _text = "None";
            if (trIn.HandOverBy.Count > 0)
            {
                if (p == 0)
                {
                    _text = trIn.HandOverBy[0].Name;
                    if (_text == null)
                    {
                        _text = "None";
                    }
                }

                else if (p == 1)
                {
                    _text = trIn.HandOverBy[0].Address;
                    if (_text == null)
                    {
                        _text = "None";
                    }
                }

                else if (p == 2)
                {
                    _text = trIn.HandOverBy[0].Date.ToShortDateString();
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

        private ReportViewModel.ReportFormat GetFormat(int type)
        {
            if (type == 1)
                return ReportViewModel.ReportFormat.PDF;

            if (type == 2)
                return ReportViewModel.ReportFormat.Excel;

            if (type == 3)
                return ReportViewModel.ReportFormat.Word;

            return 0;
        }



        public List<TransmittalIN> GetByClientIDandDateRange(long clientID, DateTime monthStart, DateTime monthEnd)
        {
            return context.TransmittalINs.Where(t => t.ClientID == clientID && t.TransmittalDate >= monthStart && t.TransmittalDate <= monthEnd).ToList();
        }

        public List<TransmittalIN> GetAll()
        {
            return context.TransmittalINs.ToList();
        }

        public TransmittalIN GetByName(string tempTransmittalNo)
        {
            return context.TransmittalINs.Where(t => t.TransmittalNo == tempTransmittalNo).FirstOrDefault();
        }

        public long GetMaxID()
        {
            long id = 0;

            try
            {
                id = context.TransmittalINs.Max(t => t.TransmittalINId);// db.Users.Max(u => u.UserId);
            }
            catch
            {
                id = 1;
            }

            return id;
        }

        public  List<TransmittalIN> GetAllDateFilter()
        {
            DateTime startDate = DateTime.Now.AddDays(-10);
            DateTime endDate = DateTime.Now;

            return context.TransmittalINs.Where(t => t.TransmittalDate >= startDate && t.TransmittalDate <= endDate).ToList();

        }

        public List<TransmittalIN> GetAllByDate(DateTime flagDate)
        {
            return context.TransmittalINs.Where(t => t.TransmittalDate == flagDate).ToList();
        }

        public  List<TransmittalIN> GetAllLocationNA()
        {
            return context.TransmittalINs.Where(t => t.TransmittalINStatusId > 1 && t.TransmittalINStatusId <= 4).ToList();
        }

        public  List<TransmittalIN> GetByClientID(long _clientID)
        {
            return context.TransmittalINs.Where(t => t.ClientID == _clientID).ToList();
        }

        public  PagedList.IPagedList<Models.TransmittalIN> GetAllPagedList(int pageNumber, int pageSize)
        {
            return context.TransmittalINs.OrderByDescending(a => a.TransmittalNo).ToPagedList(pageNumber, pageSize);
        }

        public  List<TransmittalIN> GetByClientIDandDeptIDandDateRange(long clientID, long? deptID, DateTime monthStart, DateTime monthEnd)
        {
            List<TransmittalIN> list = new List<TransmittalIN>();

            if (deptID.HasValue)
            {
                list = context.TransmittalINs.Where(t => t.ClientID == clientID && t.DepartmentID == deptID.Value && t.TransmittalDate >= monthStart && t.TransmittalDate <= monthEnd).ToList();
            }
            else
            {
                list = context.TransmittalINs.Where(t => t.ClientID == clientID  && t.TransmittalDate >= monthStart && t.TransmittalDate <= monthEnd).ToList();
            }
            return list;
        }

        public PagedList.IPagedList<Models.TransmittalIN> GetByClientIDPagedList(long clientId, long departmentId, string subDepartment, int pageNumber, int pageSize)
        {
            var departmentName = context.Departments.FirstOrDefault(d => d.DepartmentID == departmentId).DepartmentName;
            if(!string.Equals(departmentName, "SuperAdmin", StringComparison.OrdinalIgnoreCase))
            {
                return context.TransmittalINs
                    .Where(c => c.ClientID == clientId
                                && c.DepartmentID == departmentId
                                && (subDepartment == null || c.SubDepartment == subDepartment))
                    .OrderByDescending(a => a.TransmittalINId)
                    .ToPagedList(pageNumber, pageSize);
            }
            else
            {
                return context.TransmittalINs
                    .Where(c => c.ClientID == clientId)
                    .OrderByDescending(a => a.TransmittalINId)
                    .ToPagedList(pageNumber, pageSize);
            }

        }

    }

    public interface ITransmittalINRepository : IDisposable
    {
        IQueryable<TransmittalIN> All { get; }
        IQueryable<TransmittalIN> AllIncluding(params Expression<Func<TransmittalIN, object>>[] includeProperties);
        TransmittalIN Find(long id);
        void InsertOrUpdate(TransmittalIN transmittalin);
        TransmittalIN GetTrIN(long trId);
        List<TransmittalIN> GetTrINList(long _trID);
        void Delete(long id);
        void Save();
    }
}