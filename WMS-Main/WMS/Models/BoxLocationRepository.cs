using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class BoxLocationRepository : IBoxLocationRepository
    {
        WareHouseMVCContext context;

         public  BoxLocationRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public BoxLocationRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<BoxLocation> All
        {
            get { return context.BoxLocations; }
        }

        public IQueryable<BoxLocation> AllIncluding(params Expression<Func<BoxLocation, object>>[] includeProperties)
        {
            IQueryable<BoxLocation> query = context.BoxLocations;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }



        public List<TransmittalINDTO> FindAllBoxData(DateTime startDate, DateTime endDate, int clientId)
        {
            var transmittalData = (from transmittal in context.TransmittalINs
                                   where transmittal.TransmittalDate >= startDate && transmittal.TransmittalDate <= endDate
                                   //&& transmittal.DepartmentID == deptId && (transmittal.SubDepartment == subDeptId || transmittal.SubDepartment == null)
                                   select new TransmittalINDTO
                                   {
                                       TransmittalNo = transmittal.TransmittalNo,
                                       TransmittalDate = transmittal.TransmittalDate,
                                       TransmittalStatus = context.TransmittalINStatus.Where(e => e.TransmittalINStatusId == transmittal.TransmittalINStatusId).FirstOrDefault().StatusName,
                                       Items = (from item in transmittal.Items
                                                select new ItemDTO
                                                {
                                                    BoxNo = item.BoxNo,
                                                    ItemName = item.ItemName,
                                                    BoxDestruction = item.DestructionPeriod,
                                                    DepartmentName = context.Departments.Where(d => d.DepartmentID == item.DepartmentID).FirstOrDefault().DepartmentName,
                                                    SubDeptName = string.IsNullOrEmpty(item.SubDepartment) ? "NA" : item.SubDepartment,
                                                    Description = item.Description,
                                                    Legalhold = item.IsLegalHold ? "yes" : "No"
                                                }).ToList()
                                   }).ToList();

            return transmittalData;
        }
        public BoxLocation Find(long id)
        {
            return context.BoxLocations.Find(id);
        }

        public void InsertOrUpdate(BoxLocation boxlocation)
        {
            if (boxlocation.BoxLocationId == default(long)) {
                // New entity
                context.BoxLocations.Add(boxlocation);
            } else {
                // Existing entity
                context.Entry(boxlocation).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var boxlocation = context.BoxLocations.Find(id);
            context.BoxLocations.Remove(boxlocation);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

        public BoxLocation GetByAssignBoxId(long assignBoxId)
        {
            return context.BoxLocations.Where(b => b.AssignBoxId == assignBoxId).FirstOrDefault();
        }

        public List<BoxLocation> GetListByAssignBoxId(long assignBoxId)
        {
            return context.BoxLocations.Where(b => b.AssignBoxId == assignBoxId).ToList();
        }

        public List<AssignBox> GetAllItems()
        {
            //this is used only to help in adding the dataset of type employee to the report definition
            return null;
        }

        public List<AssignBox> GetAllItemsFile()
        {
            //this is used only to help in adding the dataset of type employee to the report definition
            return null;
        }

        public ReportViewModelForBoxSearch GetBoxSearchReport(List<AssignBox> finalList, long clientId, long deptID, int rptType)
        {
            var itemList = new List<object>();

          

            foreach (var item in finalList)
            {
                //year = item.Year;
                itemList.Add(item);
            }
            //Assuming the person printing the report is me
            var UserPrinting = HttpContext.Current.User.Identity.Name;


            var reportViewModel = new ReportViewModelForBoxSearch()
            {
                FileName = "~/ReportsHolder/BoxSearch.rdlc",
                Name = "Statistical Report",
                ReportDate = DateTime.Now,
                ReportTitle = "Box Search Statement",


                ClientName = context.Clients.Find(clientId).ClientName,


                HostName = GetHostinfo(1),
                HostAddress = GetHostinfo(2),

                Department = GetDept(clientId,deptID),
                ReportLanguage = "en-US",

                Format = GetFormat(rptType),
                ViewAsAttachment = false,

            };
            reportViewModel.ReportDataSets.Add(new ReportViewModelForBoxSearch.ReportDataSet() { DataSetData = itemList.ToList(), DatasetName = "DataSet1" });


            return reportViewModel;
        }

        private ReportViewModelForBoxSearch.ReportFormat GetFormat(int rptType)
        {
            if (rptType == 1)
                return ReportViewModelForBoxSearch.ReportFormat.PDF;

            if (rptType == 2)
                return ReportViewModelForBoxSearch.ReportFormat.Excel;

           

            return 0;
        }

        private string GetDept(long clientId, long deptID)
        {
            string deptName = "N/A";
            if (deptID != 0)
            {
                deptName = context.Departments.Where(d => d.ClientID == clientId && d.DepartmentID == deptID).FirstOrDefault().DepartmentName;
            }

            return deptName;
            
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


        public  List<BoxLocation> GetBywIDandPalletStatus(long wID)
        {
            return context.BoxLocations.Where(b=>b.WareHouseID==wID && b.IsPallet==true && b.CurrentStatus=="In WareHouse").ToList();
        }

        public int GetBywIDandPalletStatusCount(long wID)
        {

            return context.BoxLocations.Where(b => b.WareHouseID == wID && b.IsPallet == true && b.CurrentStatus == "In WareHouse").Count();
        }
        public ReportViewModelForBoxSearch GetBoxSearchReportFile(List<AssignBox> finalList, long clientId, long deptID, int rptType)
        {

            var itemList = new List<object>();

            foreach (var item in finalList)
            {
                //year = item.Year;
                itemList.Add(item);
            }
            //Assuming the person printing the report is me
            var UserPrinting = HttpContext.Current.User.Identity.Name;


            var reportViewModel = new ReportViewModelForBoxSearch()
            {
                FileName = "~/ReportsHolder/BoxSearchFile.rdlc",
                Name = "Statistical Report",
                ReportDate = DateTime.Now,
                ReportTitle = "File Search Statement",


                ClientName = context.Clients.Find(clientId).ClientName,


                HostName = GetHostinfo(1),
                HostAddress = GetHostinfo(2),

                Department = GetDept(clientId, deptID),
                ReportLanguage = "en-US",

                Format = GetFormat(rptType),
                ViewAsAttachment = false,

            };
            reportViewModel.ReportDataSets.Add(new ReportViewModelForBoxSearch.ReportDataSet() { DataSetData = itemList.ToList(), DatasetName = "DataSet1" });


            return reportViewModel;
        }
    }

    public interface IBoxLocationRepository : IDisposable
    {
        IQueryable<BoxLocation> All { get; }
        IQueryable<BoxLocation> AllIncluding(params Expression<Func<BoxLocation, object>>[] includeProperties);
        BoxLocation Find(long id);
        void InsertOrUpdate(BoxLocation boxlocation);
        void Delete(long id);
        void Save();
    }
}