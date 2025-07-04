using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using PagedList;

namespace WareHouseMVC.Models
{ 
    public class BarcodeMappingRepository : IBarcodeMappingRepository
    {
        WareHouseMVCContext context;
        public IPagedList<BarcodeMapping> pagedList { get; set; }  
         public  BarcodeMappingRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public BarcodeMappingRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<BarcodeMapping> All
        {
            get { return context.BarcodeMappings; }
        }

        public IQueryable<BarcodeMapping> AllIncluding(params Expression<Func<BarcodeMapping, object>>[] includeProperties)
        {
            IQueryable<BarcodeMapping> query = context.BarcodeMappings;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public BarcodeMapping Find(long id)
        {
            return context.BarcodeMappings.Find(id);
        }

        public void InsertOrUpdate(BarcodeMapping barcodemapping)
        {
            if (barcodemapping.BarcodeMappingId == default(long)) {
                // New entity
                context.BarcodeMappings.Add(barcodemapping);
            } else {
                // Existing entity
                context.Entry(barcodemapping).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var barcodemapping = context.BarcodeMappings.Find(id);
            context.BarcodeMappings.Remove(barcodemapping);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

        public  BarcodeMapping GetByTransmittalNoandBarcodeIdandItemId(string transmittalNo, long barcodeId, long itemId)
        {
            return context.BarcodeMappings.Where(b => b.TransmittalNo == transmittalNo && b.BarCodeId == barcodeId && b.ItemId == itemId).FirstOrDefault();
        }

        public  BarcodeMapping GetByTransmittalNo(string transmittalNo)
        {
            return context.BarcodeMappings.Where(b => b.TransmittalNo == transmittalNo).FirstOrDefault();
        }

        public List<BarcodeMappingViewModel> BarcodeMappingList()
        {
            return null;
        }
        public  ReportViewModelForBarcodeMapping MappingReport(long trID,int?page)
        {
            int pageSize = 50;
            int pageNumber = (page ?? 1);

            TransmittalIN _trIN=new TransmittalIN();
            _trIN = context.TransmittalINs.Where(t => t.TransmittalINId == trID).FirstOrDefault();

            List<BarcodeMapping> finalList = new List<BarcodeMapping>();

            pagedList = new PagedList<BarcodeMapping>(finalList, pageNumber, pageSize);

            pagedList = context.BarcodeMappings.Where(b => b.TransmittalNo == _trIN.TransmittalNo).OrderBy(o => o.BoxName).ToPagedList(pageNumber, pageSize);// repo.AssignBoxRepository.GetByTrIdandWidandTrStsPagedList(TrID, wID, statusID, pageSize, pageNumber);

          
            List<BarcodeMappingViewModel> lstBarcode = new List<BarcodeMappingViewModel>();

            foreach (BarcodeMapping item in pagedList)
            {
                BarcodeMappingViewModel model = new BarcodeMappingViewModel();
                model.BarCodeId = item.BarCodeId;
                model.BarcodeMappingId = item.BarcodeMappingId;
                model.BarcodeNo = (context.BarCodes.Where(d => d.BarCodeId == item.BarCodeId).FirstOrDefault().AssignBoxId * 5000).ToString();
                model.BoxName = item.BoxName;
                model.BoxNo = item.BoxNo;
                model.ClientName = item.ClientName;
                model.DeptName = item.DeptName;
                model.ImagePath = item.ImagePath;
                model.ItemId = item.ItemId;
                model.TransmittalNo = item.TransmittalNo;
                model.Year = item.Year;
                lstBarcode.Add(model);


                
            }

            var itemList = new List<object>();

            foreach (var item in lstBarcode)
            {
                //year = item.Year;
                itemList.Add(item);
            }
            //Assuming the person printing the report is me
            var UserPrinting = HttpContext.Current.User.Identity.Name;
            string ClientName = lstBarcode[0].ClientName;
            string DeptName = lstBarcode[0].DeptName;
            string TransmittalNo = _trIN.TransmittalNo;



            var reportViewModel = new ReportViewModelForBarcodeMapping()
            {
                FileName = "~/ReportsHolder/BarcodeMapping.rdlc",
                Name = "Barcode Mapping Table",
                ReportDate = DateTime.Now,
                ReportTitle = "Barcode Mapping Table",

                ClientName = ClientName,
                DepartmentName=DeptName,
                TransmittalNo=TransmittalNo,
                HostName = GetHostinfo(1),
                HostAddress = GetHostinfo(2),

                ReportLanguage = "en-US",

                Format = ReportViewModelForBarcodeMapping.ReportFormat.PDF,
                ViewAsAttachment = false,

            };
            reportViewModel.ReportDataSets.Add(new ReportViewModelForBarcodeMapping.ReportDataSet() { DataSetData = itemList.ToList(), DatasetName = "DataSet1" });


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

        public  ReportViewModelForBarcodeMappingPrevious MappingReportPrevious(List<BarCode> finalList)
        {
            List<BarcodeMappingViewModel> lstBarcode = new List<BarcodeMappingViewModel>();

            foreach (BarCode item in finalList)
            {
                BarcodeMapping _mapping = new BarcodeMapping();
                _mapping = context.BarcodeMappings.Where(m => m.BarCodeId == item.BarCodeId).FirstOrDefault();
                BarcodeMappingViewModel model = new BarcodeMappingViewModel();
                model.BarCodeId = item.BarCodeId;
                model.BarcodeMappingId = _mapping.BarcodeMappingId;
                model.BarcodeNo = (context.BarCodes.Where(d => d.BarCodeId == item.BarCodeId).FirstOrDefault().ItemId * 5000).ToString();
                model.BoxName = item.BoxName;
                model.BoxNo = item.BoxNo;
                model.ClientName = item.ClientName;
                model.DeptName = item.DeptName;
                model.ImagePath = _mapping.ImagePath;
                model.ItemId = _mapping.ItemId;
                model.TransmittalNo = _mapping.TransmittalNo;
                model.Year = item.Year;
                lstBarcode.Add(model);

            }

            var itemList = new List<object>();

            foreach (var item in lstBarcode)
            {
                //year = item.Year;
                itemList.Add(item);
            }
            //Assuming the person printing the report is me
            var UserPrinting = HttpContext.Current.User.Identity.Name;

            var reportViewModel = new ReportViewModelForBarcodeMappingPrevious()
            {
                FileName = "~/ReportsHolder/BarcodeMappingPrevious.rdlc",
                Name = "Barcode Mapping Table",
                ReportDate = DateTime.Now,
                ReportTitle = "Barcode Mapping Table",

             
                HostName = GetHostinfo(1),
                HostAddress = GetHostinfo(2),

                ReportLanguage = "en-US",

                Format = ReportViewModelForBarcodeMappingPrevious.ReportFormat.PDF,
                ViewAsAttachment = false,

            };
            reportViewModel.ReportDataSets.Add(new ReportViewModelForBarcodeMappingPrevious.ReportDataSet() { DataSetData = itemList.ToList(), DatasetName = "DataSet1" });


            return reportViewModel;

        }
    }

    public interface IBarcodeMappingRepository : IDisposable
    {
        IQueryable<BarcodeMapping> All { get; }
        IQueryable<BarcodeMapping> AllIncluding(params Expression<Func<BarcodeMapping, object>>[] includeProperties);
        BarcodeMapping Find(long id);
        void InsertOrUpdate(BarcodeMapping barcodemapping);
        void Delete(long id);
        void Save();
    }
}