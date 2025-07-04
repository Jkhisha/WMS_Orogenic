using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using PagedList;
using System.Globalization;

namespace WareHouseMVC.Models
{ 
    public class ItemRepository : IItemRepository
    {
        WareHouseMVCContext context;
        public IPagedList<Item> pagedListItem { get; set; }
         public  ItemRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public ItemRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<Item> All
        {
            get { return context.Items; }
        }

        public IQueryable<Item> AllIncluding(params Expression<Func<Item, object>>[] includeProperties)
        {
            IQueryable<Item> query = context.Items;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Item Find(long id)
        {
            return context.Items.Find(id);
        }

        public void InsertOrUpdate(Item item)
        {
            if (item.ItemId == default(long)) {
                // New entity
                context.Items.Add(item);
            } else {
                // Existing entity
                context.Entry(item).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var item = context.Items.Find(id);
            context.Items.Remove(item);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

        public  List<Item> GetByCliendIDandDeptIdandProjectId(long _clientId, long _deptId, long _projectId)
        {
            List<Item> items = new List<Item>();
            if (_projectId == -1)
            {
                items = context.Items.Where(i => i.ClientID == _clientId && i.Unit == "Box").ToList();//&& i.DepartmentID == _deptId
            }
            else
            {
                items = context.Items.Where(i => i.ClientID == _clientId && i.ProjectId == _projectId && i.Unit == "Box").ToList();//&& i.DepartmentID == _deptId
            }
            return items;

            
        }

        public List<Item> GetByCliendIDandDeptId(long _clientId, long _deptId)
        {
            List<Item> items = new List<Item>();
            items = context.Items.Where(i => i.ClientID == _clientId && i.Unit == "Box").ToList();// && i.DepartmentID == _deptIdc
            return items;
        }

        public void AddRange(List<AddBoxDOC> boxes)
        {
            context.AddBoxDOC.AddRange(boxes);
            context.SaveChanges();
        }
        public void AddAllItems(List<Item> items)
        {
            context.Items.AddRange(items);
            context.SaveChanges();
        }


        public List<Item> GetByCliendIDandDeptIdTransfer(long _clientId, long _deptId)
        {
            List<Item> items = new List<Item>();
            items = context.Items.Where(i => i.ClientID == _clientId && i.DepartmentID == _deptId).ToList();// && i.DepartmentID == _deptIdc
            return items;
        }

        public  Item CheckArchieve(string _boxNo, string _itemName, string _year, long _clientID, long _departmentID)
        {
            return context.Items.Where(i => i.BoxNo == _boxNo && i.ItemName == _itemName && i.Year == _year && i.ClientID == _clientID && i.DepartmentID == _departmentID).FirstOrDefault();
        }

        public  List<Item> GetByClientID(long clientID)
        {
            return context.Items.Where(i => i.ClientID == clientID).ToList();
        }

        //public  List<Controllers.ItemsController> Get(List<Item> _itemListMonthFiltered)
        //{

        //}

        public List<Item> GetByNextOneMonth()
        {
            DateTime todayDate = (DateTime.Now).Date;
            DateTime NextDate = todayDate.AddDays(30);
            return context.Items.Where(i=>i.DestructionPeriod >= todayDate && i.DestructionPeriod <= NextDate).Take(50).ToList();
        }

        public long getTrinIdbyTrinNo(string trNo)
        {
            return context.TransmittalINs.Where(e => e.TransmittalNo == trNo).FirstOrDefault().TransmittalINId;
        }
        public List<Item> GetByNextOneMonthNull()
        {
            return context.Items.Where(i => i.DestructionPeriod == null).Take(50).ToList();
        }

        public AddBoxDOC getAllAddboxDoc (string docName , string deptName)
        {
            var res = context.AddBoxDOC.Where(e => e.FileName == docName && e.DeptName == deptName).FirstOrDefault();
            return res;
        }
        public List<Item> GetByCliendIDandDeptIdandProjectIdFile(long _clientId, long _deptId, long _projectId)
        {
            List<Item> items = new List<Item>();
            if (_projectId == -1)
            {
                items = context.Items.Where(i => i.ClientID == _clientId && i.Unit=="File").ToList();//&& i.DepartmentID == _deptId
            }
            else
            {
                items = context.Items.Where(i => i.ClientID == _clientId && i.ProjectId == _projectId && i.Unit == "File").ToList();//&& i.DepartmentID == _deptId
            }
            return items;
        }

        public List<Item> GetByCliendIDandDeptIdFile(long _clientId, long _deptId)
        {
            List<Item> items = new List<Item>();
            items = context.Items.Where(i => i.ClientID == _clientId && i.Unit == "File").ToList();// && i.DepartmentID == _deptIdc
            return items;
        }

        public PagedList.IPagedList<Item> GetByCliendIDandDeptIdandProjectIdFilePagedList(long _clientId, long _deptId, long _projectId, int pageNumber, int pageSize)
        {
            List<Item> itemList = new List<Item>();
            pagedListItem = new PagedList<Item>(itemList, pageNumber, pageSize);
            if (_projectId == -1)
            {
                pagedListItem = context.Items.Where(i => i.ClientID == _clientId && i.Unit == "File").OrderBy(o=>o.ItemName).ToPagedList(pageNumber,pageSize);//.ToList();//&& i.DepartmentID == _deptId
            }
            else
            {
                pagedListItem = context.Items.Where(i => i.ClientID == _clientId && i.ProjectId == _projectId && i.Unit == "File").OrderBy(o => o.ItemName).ToPagedList(pageNumber, pageSize);//&& i.DepartmentID == _deptId
            }
            return pagedListItem;
        }

        public IPagedList<Item> GetByCliendIDandDeptIdFilePagedList(long _clientId, long _deptId, int pageNumber, int pageSize)
        {
            return context.Items.Where(i => i.ClientID == _clientId && i.Unit == "File").OrderBy(o => o.ItemName).ToPagedList(pageNumber, pageSize);//&& i.DepartmentID == _deptId// && i.DepartmentID == _deptIdc
          
        }

        public IPagedList<Item> GetByCliendIDandDeptIdandProjectIdPagedList(long _clientId, long _deptId, long _projectId, int pageNumber, int pageSize)
        {
            List<Item> items = new List<Item>();
            pagedListItem = new PagedList<Item>(items, pageNumber, pageSize);
            if (_projectId == -1)
            {
                pagedListItem = context.Items.Where(i => i.ClientID == _clientId && i.Unit == "Box").OrderBy(o => o.ItemName).ToPagedList(pageNumber, pageSize);//&& i.DepartmentID == _deptId
            }
            else
            {
                pagedListItem = context.Items.Where(i => i.ClientID == _clientId && i.ProjectId == _projectId && i.Unit == "Box").OrderBy(o => o.ItemName).ToPagedList(pageNumber, pageSize);//&& i.DepartmentID == _deptId
            }
            return pagedListItem;
        }

        public  IPagedList<Item> GetByCliendIDandDeptIdPagedList(long _clientId, long _deptId, int pageNumber, int pageSize)
        {


            return context.Items.Where(i => i.ClientID == _clientId && i.Unit == "Box").OrderBy(o => o.ItemName).ToPagedList(pageNumber, pageSize);// && i.DepartmentID == _deptIdc
            
        
        }

        public  Item GetByCliendIDandDeptIdandBoxNameandBoxNo(long clientId, long depId, string boxName, string boxNo)
        {
            return context.Items.Where(b => b.ClientID == clientId && b.DepartmentID == depId && b.ItemName == boxName && b.BoxNo == boxNo).FirstOrDefault();
        }

        public  List<Item> GetDestructionPeriodByMonthCount(int monthCount)
        {
            DateTime monthStart = DateTime.Now;
            DateTime monthEnd = monthStart.AddMonths(monthCount).AddDays(-1);

            return context.Items.Where(t => t.DestructionPeriod >= monthStart && t.DestructionPeriod <= monthEnd).ToList();
        }

        public List<DesPeriodReportViewModel> GetAllDesPeriod()
        {
            return null;
        }

        public ReportViewModelForDestructionPeriod UpcomingDestructionPeriodReport(List<DesPeriodReportViewModel> finalList, int monthCount, int format)
        {
            var itemList = new List<object>();

            foreach (var item in finalList)
            {
                //year = item.Year;
                itemList.Add(item);
            }
            //Assuming the person printing the report is me
            var UserPrinting = HttpContext.Current.User.Identity.Name;
            string monTH = monthCount.ToString() + " Month";

            var reportViewModel = new ReportViewModelForDestructionPeriod()
            {
                FileName = "~/ReportsHolder/DestructionPeriodReport.rdlc",
                Name = "Statistical Report",
                ReportDate = DateTime.Now,
                ReportTitle = "Upcoming Destruction Period",
                MonthPeriod=monTH,

                HostName = GetHostinfo(1),
                HostAddress = GetHostinfo(2),

                ReportLanguage = "en-US",

                Format = GetFormat(format),
                ViewAsAttachment = false,

            };
            reportViewModel.ReportDataSets.Add(new ReportViewModelForDestructionPeriod.ReportDataSet() { DataSetData = itemList.ToList(), DatasetName = "DataSet1" });


            return reportViewModel;
        }

        private ReportViewModelForDestructionPeriod.ReportFormat GetFormat(int rptType)
        {
            if (rptType == 1)
                return ReportViewModelForDestructionPeriod.ReportFormat.PDF;

            if (rptType == 2)
                return ReportViewModelForDestructionPeriod.ReportFormat.Excel;



            return 0;
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

        public  Item GetByClientIdDeptIdBoxNoBoxNameYear(long clientID, long deptId, string boxNo, string boxName, string year)
        {
            return context.Items.Where(i => i.ClientID == clientID && i.DepartmentID == deptId && i.BoxNo == boxNo && i.ItemName == boxName && i.Year == year).FirstOrDefault();
        }

        public List<BoxINReportViewModel> GetBoxInReport()
        {
            return null;
        }

        public List<BoxINReportViewModel> GetBoxOUTReport()
        {
            return null;
        }

        public ReportViewModelForBoxIN BoxInReport(List<BoxINReportViewModel> finalList, long clientID, DateTime month,int format)
        {
            var itemList = new List<object>();

            foreach (var item in finalList)
            {
                //year = item.Year;
                itemList.Add(item);
            }
            //Assuming the person printing the report is me
            var UserPrinting = HttpContext.Current.User.Identity.Name;
            string monTH = month.Month.ToString();

            string monthName = month.ToString("MMMM", CultureInfo.InvariantCulture);

            string clientName = context.Clients.Find(clientID).ClientName;

            var reportViewModel = new ReportViewModelForBoxIN()
            {
                FileName = "~/ReportsHolder/BoxINReport.rdlc",
                Name = "Box In Report",
                ReportDate = DateTime.Now,
                ReportTitle = "Box In Report",
                Month = monthName,
                ClientName=clientName,

                HostName = GetHostinfo(1),
                HostAddress = GetHostinfo(2),

                ReportLanguage = "en-US",

                Format = GetFormatBoxIn(format),
                ViewAsAttachment = false,

            };
            reportViewModel.ReportDataSets.Add(new ReportViewModelForBoxIN.ReportDataSet() { DataSetData = itemList.ToList(), DatasetName = "DataSet1" });


            return reportViewModel;
        }

        private ReportViewModelForBoxIN.ReportFormat GetFormatBoxIn(int rptType)
        {
            if (rptType == 1)
                return ReportViewModelForBoxIN.ReportFormat.PDF;

            if (rptType == 2)
                return ReportViewModelForBoxIN.ReportFormat.Excel;



            return 0;
        }

        public ReportViewModelForBoxOUT BoxOUTReport(List<BoxINReportViewModel> finalList, long clientID, DateTime month, int format)
        {

            var itemList = new List<object>();

            foreach (var item in finalList)
            {
                //year = item.Year;
                itemList.Add(item);
            }
            //Assuming the person printing the report is me
            var UserPrinting = HttpContext.Current.User.Identity.Name;
            string monTH = month.Month.ToString();

            string monthName = month.ToString("MMMM", CultureInfo.InvariantCulture);

            string clientName = context.Clients.Find(clientID).ClientName;

            var reportViewModel = new ReportViewModelForBoxOUT()
            {
                FileName = "~/ReportsHolder/BoxOUTReport.rdlc",
                Name = "Box OUT Report",
                ReportDate = DateTime.Now,
                ReportTitle = "Box OUT Report",
                Month = monthName,
                ClientName = clientName,

                HostName = GetHostinfo(1),
                HostAddress = GetHostinfo(2),

                ReportLanguage = "en-US",

                Format = GetFormatBoxOUT(format),
                ViewAsAttachment = false,

            };
            reportViewModel.ReportDataSets.Add(new ReportViewModelForBoxOUT.ReportDataSet() { DataSetData = itemList.ToList(), DatasetName = "DataSet1" });


            return reportViewModel;
        }

        private ReportViewModelForBoxOUT.ReportFormat GetFormatBoxOUT(int rptType)
        {
            if (rptType == 1)
                return ReportViewModelForBoxOUT.ReportFormat.PDF;

            if (rptType == 2)
                return ReportViewModelForBoxOUT.ReportFormat.Excel;



            return 0;
        }



        public  List<Item> GetByDeptId(long OldDeptId)
        {
            return context.Items.Where(i => i.DepartmentID == OldDeptId).ToList();
        }

        public  List<Item> GetAllUpcoming(long clientId)
        {
            DateTime monthStart = DateTime.Now;
            DateTime monthEnd = DateTime.Now.AddMonths(1).AddDays(-1);


           return context.Items.Where(t => t.ClientID == clientId && t.DestructionPeriod >= monthStart && t.DestructionPeriod <= monthEnd).ToList();
            
        }

        public List<Item> GetAllDesitems(long clientId)
        {
            DateTime monthStart = DateTime.Now;
            DateTime monthEnd = DateTime.Now.AddMonths(1).AddDays(-1);


            return context.Items.Where(t => t.ClientID == clientId && t.DestructionPeriod >= monthStart && t.DestructionPeriod <= monthEnd).ToList();
        }

        public  List<Item> GetAll(long clientId)
        {
            return context.Items.Where(t => t.ClientID == clientId).ToList();
        }

        internal List<Item> GetByNextOneMonthByClient(long clientId)
        {
            DateTime todayDate = (DateTime.Now).Date;
            DateTime NextDate = todayDate.AddDays(30);
            return context.Items.Where(i => i.DestructionPeriod >= todayDate && i.DestructionPeriod <= NextDate && i.ClientID == clientId).Take(50).ToList();
        }

        internal List<Item> GetByNextOneMonthByClientandDept(long clientId, long deptId)
        {
            DateTime todayDate = (DateTime.Now).Date;
            DateTime NextDate = todayDate.AddDays(30);
            return context.Items.Where(i => i.DestructionPeriod >= todayDate && i.DestructionPeriod <= NextDate && i.ClientID == clientId && i.DepartmentID == deptId).Take(50).ToList();
        }

      

        public Item GetByCliendIDandDeptIdandBoxNameandBoxNoAndYear(long clientId, long depId, string boxName, string boxNo, string year)
        {


            List<Item> allBox = new List<Item>();
            allBox = context.Items.Where(i => i.ClientID == clientId && i.DepartmentID == depId).ToList();

            Item box = new Item();

            if (!string.IsNullOrEmpty(year))
            {
                box = allBox.Where(s => s.BoxNo.Trim() == boxNo.Trim() && s.ItemName.Trim() == boxName.Trim() && s.Year == year.Trim()).FirstOrDefault();
            }
            else
            {
                box = allBox.Where(s => s.BoxNo.Trim() == boxNo.Trim() && s.ItemName.Trim() == boxName.Trim()).FirstOrDefault();
            }

            return box;


            // return context.Items.Where(b => b.ClientID == clientId && b.DepartmentID == depId && b.ItemName == boxName && b.BoxNo == boxNo && b.Year==year).FirstOrDefault();
        }

        internal List<Item> GetDestructionPeriodByMonthCountAndClientId(int monthCount, long clientId)
        {
            DateTime monthStart = DateTime.Now;
            DateTime monthEnd = monthStart.AddMonths(monthCount).AddDays(-1);

            return context.Items.Where(t => t.DestructionPeriod >= monthStart && t.DestructionPeriod <= monthEnd && t.ClientID==clientId).ToList();
        }
    }

    public interface IItemRepository : IDisposable
    {
        IQueryable<Item> All { get; }
        IQueryable<Item> AllIncluding(params Expression<Func<Item, object>>[] includeProperties);
        Item Find(long id);
        void InsertOrUpdate(Item item);
        void Delete(long id);
        void Save();
    }
}