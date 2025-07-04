using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class ChangeLocationRepository : IChangeLocationRepository
    {
        WareHouseMVCContext context;

         public  ChangeLocationRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public ChangeLocationRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<ChangeLocation> All
        {
            get { return context.ChangeLocations; }
        }

        public IQueryable<ChangeLocation> AllIncluding(params Expression<Func<ChangeLocation, object>>[] includeProperties)
        {
            IQueryable<ChangeLocation> query = context.ChangeLocations;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public ChangeLocation Find(long id)
        {
            return context.ChangeLocations.Find(id);
        }

        public void InsertOrUpdate(ChangeLocation changelocation)
        {
            if (changelocation.ChangeLocationId == default(long)) {
                // New entity
                context.ChangeLocations.Add(changelocation);
            } else {
                // Existing entity
                context.Entry(changelocation).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var changelocation = context.ChangeLocations.Find(id);
            context.ChangeLocations.Remove(changelocation);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

        public  List<ChangeLocation> GetListByItemId(long itemId)
        {
            return context.ChangeLocations.Where(c => c.ItemId == itemId).ToList();
        }

        public List<ChangeLocation> GetAllItemsBoxMovement()
        {
            //this is used only to help in adding the dataset of type employee to the report definition
            return null;
        }

        public ReportViewModelForBoxMovement GetBoxMovementReport(List<ChangeLocation> finalList, string clientName, string deptName, string boxName, string boxNo, int format)
        {
            var itemList = new List<object>();

            foreach (var item in finalList)
            {
                //year = item.Year;
                itemList.Add(item);
            }
            //Assuming the person printing the report is me
            var UserPrinting = HttpContext.Current.User.Identity.Name;

            var reportViewModel = new ReportViewModelForBoxMovement()
            {
                FileName = "~/ReportsHolder/BoxMovement.rdlc",
                Name = "Statistical Report",
                ReportDate = DateTime.Now,
                ReportTitle = "Box Movement Report",


                ClientName = clientName,


                HostName = GetHostinfo(1),
                HostAddress = GetHostinfo(2),
                BoxName=boxName,
                BoxNo=boxNo,
                Department =deptName,
                ReportLanguage = "en-US",

                Format = GetFormat(format),
                ViewAsAttachment = false,

            };
            reportViewModel.ReportDataSets.Add(new ReportViewModelForBoxMovement.ReportDataSet() { DataSetData = itemList.ToList(), DatasetName = "DataSet1" });


            return reportViewModel;
        }

        private ReportViewModelForBoxMovement.ReportFormat GetFormat(int format)
        {
            if (format == 1)
                return ReportViewModelForBoxMovement.ReportFormat.PDF;

            if (format == 2)
                return ReportViewModelForBoxMovement.ReportFormat.Excel;



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
    }

    public interface IChangeLocationRepository : IDisposable
    {
        IQueryable<ChangeLocation> All { get; }
        IQueryable<ChangeLocation> AllIncluding(params Expression<Func<ChangeLocation, object>>[] includeProperties);
        ChangeLocation Find(long id);
        void InsertOrUpdate(ChangeLocation changelocation);
        void Delete(long id);
        void Save();
    }
}