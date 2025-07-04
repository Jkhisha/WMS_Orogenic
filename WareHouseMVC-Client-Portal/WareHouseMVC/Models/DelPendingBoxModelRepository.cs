using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Globalization;

namespace WareHouseMVC.Models
{ 
    public class DelPendingBoxModelRepository : IDelPendingBoxModelRepository
    {
        WareHouseMVCContext context;

         public  DelPendingBoxModelRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public DelPendingBoxModelRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<DelPendingBoxModel> All
        {
            get { return context.DelPendingBoxModels; }
        }

        public IQueryable<DelPendingBoxModel> AllIncluding(params Expression<Func<DelPendingBoxModel, object>>[] includeProperties)
        {
            IQueryable<DelPendingBoxModel> query = context.DelPendingBoxModels;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public DelPendingBoxModel Find(long id)
        {
            return context.DelPendingBoxModels.Find(id);
        }

        public void InsertOrUpdate(DelPendingBoxModel delpendingboxmodel)
        {
            if (delpendingboxmodel.DelPendingBoxModelId == default(long)) {
                // New entity
                context.DelPendingBoxModels.Add(delpendingboxmodel);
            } else {
                // Existing entity
                context.Entry(delpendingboxmodel).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var delpendingboxmodel = context.DelPendingBoxModels.Find(id);
            context.DelPendingBoxModels.Remove(delpendingboxmodel);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

        public  List<DelPendingBoxModel> GetAllDeliveryPending()
        {
            return context.DelPendingBoxModels.ToList();
        }

        public  DelPendingBoxModel GetByTrOutNo(string trOutNo)
        {
            return context.DelPendingBoxModels.Where(d => d.TransmittalOutNo == trOutNo).FirstOrDefault();
        }

        public  List<DelPendingBoxModel> GetByClientIDandDeptIDandDateRange(long clientID, long? deptID, DateTime monthStart, DateTime monthEnd)
        {
            List<DelPendingBoxModel> list = new List<DelPendingBoxModel>();

            if (deptID.HasValue)
            {
                list = context.DelPendingBoxModels.Where(t => t.ClientID == clientID && t.DepatID == deptID.Value && t.TransmittalOutDate >= monthStart && t.TransmittalOutDate <= monthEnd).ToList();
            }
            else
            {
                list = context.DelPendingBoxModels.Where(t => t.ClientID == clientID && t.TransmittalOutDate >= monthStart && t.TransmittalOutDate <= monthEnd).ToList();
            }
            return list;
        }

        public List<DelPendingBoxModel> GetDelPendingBoxReport()
        {
            return null;
        }

        public ReportViewModelForDelPendingBox DelPending(List<DelPendingBoxModel> finalList, long clientID, DateTime month, int format)
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

            var reportViewModel = new ReportViewModelForDelPendingBox()
            {
                FileName = "~/ReportsHolder/DelPendingBox.rdlc",
                Name = "Box OUT Report",
                ReportDate = DateTime.Now,
                ReportTitle = "Box OUT Report",
                Month = monthName,
                ClientName = clientName,

                HostName = GetHostinfo(1),
                HostAddress = GetHostinfo(2),

                ReportLanguage = "en-US",

                Format = GetFormatDelPendingBox(format),
                ViewAsAttachment = false,

            };
            reportViewModel.ReportDataSets.Add(new ReportViewModelForDelPendingBox.ReportDataSet() { DataSetData = itemList.ToList(), DatasetName = "DataSet1" });


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

        private ReportViewModelForDelPendingBox.ReportFormat GetFormatDelPendingBox(int rptType)
        {
            if (rptType == 1)
                return ReportViewModelForDelPendingBox.ReportFormat.PDF;

            if (rptType == 2)
                return ReportViewModelForDelPendingBox.ReportFormat.Excel;



            return 0;
        }

        public  DelPendingBoxModel GetBytrOutNoandClientIdandDeptIdandBoxNo(string trOutNo, long clientId, long deptId, string boxNo)
        {
            return context.DelPendingBoxModels.Where(d => d.BoxNo == boxNo && d.TransmittalOutNo == trOutNo && d.ClientID == clientId && d.DepatID == deptId).FirstOrDefault();
        }
    }

    public interface IDelPendingBoxModelRepository : IDisposable
    {
        IQueryable<DelPendingBoxModel> All { get; }
        IQueryable<DelPendingBoxModel> AllIncluding(params Expression<Func<DelPendingBoxModel, object>>[] includeProperties);
        DelPendingBoxModel Find(long id);
        void InsertOrUpdate(DelPendingBoxModel delpendingboxmodel);
        void Delete(long id);
        void Save();
    }
}