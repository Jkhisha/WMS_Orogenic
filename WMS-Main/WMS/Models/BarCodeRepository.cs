using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Configuration;
using WareHouseMVC.HelperClasses;
using System.Drawing;
using System.IO;
using PagedList;

namespace WareHouseMVC.Models
{
    public class BarCodeRepository : IBarCodeRepository
    {
        WareHouseMVCContext context;
        public IPagedList<AssignBox> pagedList { get; set; }
        public BarCodeRepository()
            : this(new WareHouseMVCContext())
        {

        }

        public BarCodeRepository(WareHouseMVCContext context)
        {

            this.context = context;
        }

        UnitOfWork repo = new UnitOfWork();

        public IQueryable<BarCode> All
        {
            get { return context.BarCodes; }
        }

        public IQueryable<BarCode> AllIncluding(params Expression<Func<BarCode, object>>[] includeProperties)
        {
            IQueryable<BarCode> query = context.BarCodes;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public BarCode Find(long id)
        {
            return context.BarCodes.Find(id);
        }

        public void InsertOrUpdate(BarCode barcode)
        {
            if (barcode.BarCodeId == default(long))
            {
                // New entity
                context.BarCodes.Add(barcode);
            }
            else
            {
                // Existing entity
                context.Entry(barcode).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var barcode = context.BarCodes.Find(id);
            context.BarCodes.Remove(barcode);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public BarCode GetByAssignBoxId(long assignBoxId)
        {
            return context.BarCodes.Where(b => b.AssignBoxId == assignBoxId).FirstOrDefault();
        }

        public List<BarCode> BarCodeList()
        {
            //this is used only to help in adding the dataset of type employee to the report definition
            return null;
        }

        public BarCode GetBarcodeTxtEmptyBoxes(long EmtBxId)
        {
            BarCode _existingBarCode = new BarCode();
            repo.BarCodeRepository.InsertOrUpdate(_existingBarCode);
            repo.BarCodeRepository.Save();
            long id = _existingBarCode.BarCodeId;

            string BarCodeText = (id * 5000 + 10).ToString();

            Image myimg = Code128Rendering.MakeBarcodeImage(BarCodeText, 1, true);
            byte[] fileContents = imageToByteArray(myimg);


            string _relativePath = "~/Content/Images/" + ConfigurationManager.AppSettings["BarCodes"].ToString() + "AssignBox-" + EmtBxId.ToString();
            var path = "D:\\WHMS2\\Content\\Images\\BarCodes\\" + "AssignBox-" + id.ToString();

            Directory.CreateDirectory(path);

            string filePath = path + ".Bmp";

            System.IO.File.WriteAllBytes(filePath, fileContents);
            string _reportField = string.Empty;

            EmptyBox Box = context.EmptyBoxes.Find(EmtBxId);


            _existingBarCode.ImagePathRel = _relativePath + ".Bmp";
            _existingBarCode.ImagePathAbs = "file://" + filePath;
            _existingBarCode.BoxNo = BarCodeText;
            _existingBarCode.Year = Box.RecuisitionDate.Year.ToString();
            _existingBarCode.BoxName = BarCodeText;
            _existingBarCode.ClientName = Box.Client.ClientName;
            _existingBarCode.DeptName = Box.Department.DepartmentName;
            _reportField = "Box Name :" + _existingBarCode.BoxName + System.Environment.NewLine;
            _reportField = _reportField + "Box No :" + _existingBarCode.BoxNo + System.Environment.NewLine;
            _reportField = _reportField + "Client :" + _existingBarCode.ClientName + System.Environment.NewLine + "Department :" + _existingBarCode.DeptName + System.Environment.NewLine;

            _existingBarCode.ReportFiled = _reportField;
            repo.BarCodeRepository.InsertOrUpdate(_existingBarCode);
            repo.BarCodeRepository.Save();

            return _existingBarCode;


        }
         
        public ReportViewModelForBarCode GetEmptyBoxBarCodeReport(string empBarcode)
        {
            var _barCodeList = new List<object>();
            List<string> barcodeList = empBarcode?.Split(',').ToList();
            foreach (var item in barcodeList)
            {
                var emptyBox = repo.EmptyBoxRepository.FindEmptyBoxBarcodeByBarcodeText(item);
                BarCode emptyBoxBarcode = context.BarCodes.FirstOrDefault(e => e.BarCodeId == emptyBox.BarCodeId);
                _barCodeList.Add(emptyBoxBarcode);

            }
            var UserPrinting = HttpContext.Current.User.Identity.Name;

            var reportViewModel = new ReportViewModelForBarCode()
            {
                FileName = "~/ReportsHolder/BarCode.rdlc",
                Name = "Statistical Report",
                ReportTitle = "Barcode Images",
                UserNamPrinting = UserPrinting,
                TransmittalNo = " ",
                PrintDate = DateTime.Now.ToString(),
                BarCodeText = "barcode",
                ReportLanguage = "en-US",

                Format = ReportViewModelForBarCode.ReportFormat.PDF,
                ViewAsAttachment = false,

            };
            reportViewModel.ReportDataSets.Add(new ReportViewModelForBarCode.ReportDataSet() { DataSetData = _barCodeList.ToList(), DatasetName = "DataSet1" });


            return reportViewModel;

        }

        public ReportViewModelForBarCode GetBarCodeReport(long TrID, long wID, long statusID, int? page)
        {

            string _transmittalNo = repo.TransmittalINRepository.Find(TrID).TransmittalNo;


            int pageSize = 50;
            int pageNumber = (page ?? 1);

            var _barCodeList = new List<object>();
            List<AssignBox> assignBox = new List<AssignBox>();


            pagedList = new PagedList<AssignBox>(assignBox, pageNumber, pageSize);

            pagedList = repo.AssignBoxRepository.GetByTrIdandWidandTrStsPagedList(TrID, wID, statusID, pageSize, pageNumber);



            // assignBox = repo.AssignBoxRepository.GetByTrIdandWidandTrSts(TrID, wID, statusID);
            // assignBox = 
            string barcodeText = " ";
            foreach (AssignBox item in pagedList)
            {

                #region BarCode Insert or Update for emptyboxes
                
                if (item.BarCodeText != null && item.BarCodeId != null)
                {
                    BarCode emptyBoxBarcode = context.BarCodes.FirstOrDefault(e => e.BarCodeId == item.BarCodeId);
                    if (emptyBoxBarcode != null)
                    {
                        emptyBoxBarcode.AssignBoxId = item.AssignBoxId;
                        emptyBoxBarcode.ItemId = item.ItemId;
                        barcodeText = item.BarCodeText;
                        context.Entry(emptyBoxBarcode).State = EntityState.Modified;
                        context.SaveChanges();

                        BarcodeMapping _mapping = context.BarcodeMappings.FirstOrDefault(b => b.TransmittalNo == item.TransmittalIN.TransmittalNo && b.BarCodeId == emptyBoxBarcode.BarCodeId && b.ItemId == item.ItemId);
                        if (_mapping == null)
                            _mapping = new BarcodeMapping();

                        _mapping.BarCodeId = emptyBoxBarcode.BarCodeId;
                        _mapping.BoxName = item.BoxName;
                        _mapping.BoxNo = item.BoxNo;
                        _mapping.ClientName = emptyBoxBarcode.ClientName;
                        _mapping.DeptName = emptyBoxBarcode.DeptName;
                        _mapping.ImagePath = emptyBoxBarcode.ImagePathAbs;
                        _mapping.ItemId = item.ItemId;
                        _mapping.TransmittalNo = item.TransmittalIN.TransmittalNo;
                        _mapping.Year = item.Year;

                        repo.BarcodeMappingRepository.InsertOrUpdate(_mapping);
                        repo.BarcodeMappingRepository.Save();

                        _barCodeList.Add(emptyBoxBarcode);
                    }
                }
                else
                {
                    string BarCodeText = GetBarcodeText(item.AssignBoxId);
                    //barcodeText = BarCodeText;
                    //Image myimg = Code128Rendering.MakeBarcodeImage(BarCodeText, 2, false);
                    Image myimg = Code128Rendering.MakeBarcodeImage(BarCodeText, 1, true);
                    byte[] fileContents = imageToByteArray(myimg);

                    
                    string _relativePath = "~/Content/Images/" + ConfigurationManager.AppSettings["BarCodes"].ToString() + "AssignBox-" + item.AssignBoxId.ToString();
                    var path = "D:\\WHMS2\\Content\\Images\\BarCodes\\" + "AssignBox-" + item.AssignBoxId.ToString();

                    Directory.CreateDirectory(path);

                    string filePath = path + ".Bmp";

                    System.IO.File.WriteAllBytes(filePath, fileContents);


                    BarCode _existingBarCode = repo.BarCodeRepository.GetByAssignBoxId(item.AssignBoxId);//.ite.GetByAssignBoxId(item.AssignBoxId);
                    string _reportField = string.Empty;

                    if (_existingBarCode == null)
                    {
                        _existingBarCode = new BarCode();


                    }
                    _existingBarCode.AssignBoxId = item.AssignBoxId;
                    _existingBarCode.ImagePathRel = _relativePath + ".Bmp";
                    _existingBarCode.ImagePathAbs = "file://" + filePath;
                    _existingBarCode.BoxNo = item.BoxNo;
                    _existingBarCode.Year = item.Year;
                    _existingBarCode.BoxName = item.BoxName;
                    _existingBarCode.ClientName = item.Item.Client.ClientName;
                    _existingBarCode.DeptName = item.Item.Department.DepartmentName;
                    _reportField = "Box Name :" + _existingBarCode.BoxName + System.Environment.NewLine;
                    _reportField = _reportField + "Box No :" + _existingBarCode.BoxNo + System.Environment.NewLine;
                    _reportField = _reportField + "Client :" + _existingBarCode.ClientName + System.Environment.NewLine + "Department :" + _existingBarCode.DeptName + System.Environment.NewLine;

                    _existingBarCode.ReportFiled = _reportField;
                    _existingBarCode.ItemId = item.ItemId;
                    repo.BarCodeRepository.InsertOrUpdate(_existingBarCode);
                    repo.BarCodeRepository.Save();

                    //InsertOrUpdate(_existingBarCode);
                    //Save();

                    AssignBox _box = new AssignBox();
                    _box = repo.AssignBoxRepository.Find(item.AssignBoxId);// context.AssignBoxes.Where(a => a.AssignBoxId == item.AssignBoxId).FirstOrDefault();
                    _box.BarCodeId = _existingBarCode.BarCodeId;
                    repo.AssignBoxRepository.InsertOrUpdate(_box);
                    repo.AssignBoxRepository.Save();


                    #endregion


                    #region BarcodeMapping Table Insertion

                    BarcodeMapping _mapping = new BarcodeMapping();
                    _mapping = context.BarcodeMappings.Where(b => b.TransmittalNo == item.TransmittalIN.TransmittalNo && b.BarCodeId == _existingBarCode.BarCodeId && b.ItemId == item.ItemId).FirstOrDefault();

                    if (_mapping == null)
                        _mapping = new BarcodeMapping();

                    _mapping.BarCodeId = _existingBarCode.BarCodeId;
                    _mapping.BoxName = item.BoxName;
                    _mapping.BoxNo = item.BoxNo;
                    _mapping.ClientName = _existingBarCode.ClientName;
                    _mapping.DeptName = _existingBarCode.DeptName;
                    _mapping.ImagePath = _existingBarCode.ImagePathAbs;
                    _mapping.ItemId = item.ItemId;
                    _mapping.TransmittalNo = item.TransmittalIN.TransmittalNo;
                    _mapping.Year = item.Year;

                    repo.BarcodeMappingRepository.InsertOrUpdate(_mapping);
                    repo.BarcodeMappingRepository.Save();

                    #endregion

                    _barCodeList.Add(_existingBarCode);
                }



            }


            //Assuming the person printing the report is me
            var UserPrinting = HttpContext.Current.User.Identity.Name;

            var reportViewModel = new ReportViewModelForBarCode()
            {
                FileName = "~/ReportsHolder/BarCode.rdlc",
                Name = "Statistical Report",
                //  ReportDate = DateTime.Now,
                ReportTitle = "Barcode Images",
                //ClientName = trIn.Client.ClientName,
                //Department = trIn.Department.DepartmentName,
                //DepartmentCode = trIn.Department.DepartmentCode,
                UserNamPrinting = UserPrinting,
                TransmittalNo = _transmittalNo,
                PrintDate = DateTime.Now.ToString(),
                BarCodeText = barcodeText,
                // ContactPerson = trIn.ContactPerson.ContactPersonName,
                //HandOverByName = GetHandOverBy(trIn, 0),
                //HandOverByAddress = GetHandOverBy(trIn, 1),
                //HandOverByDate = GetHandOverBy(trIn, 2),
                //// Status = GetStatus(trIn.Status),//trIn.Status.ToString(),
                //ReceivedByName = GetReceivedBy(trIn, 0),
                //ReceivedByAddress = GetReceivedBy(trIn, 1),
                //ReceivedByDate = GetReceivedBy(trIn, 2),
                //TransmittalDate = trIn.TransmittalDate.ToShortDateString(),
                //ClientReqRef = trIn.ClientRequestreference,
                //IssuedBy = trIn.ContactPerson.ContactPersonName,
                //Year = year,
                //ProjectCode = GetProjectCode(trIn),
                //Qty = qtyCount.ToString(),
                //Unit = "Box",
                //Item = "Box",
                //BoxNoArr = GetBoxs(trIn),
                //TransmittalNo = trIn.TransmittalNo,
                ReportLanguage = "en-US",

                Format = ReportViewModelForBarCode.ReportFormat.PDF,
                ViewAsAttachment = false,

            };
            reportViewModel.ReportDataSets.Add(new ReportViewModelForBarCode.ReportDataSet() { DataSetData = _barCodeList.ToList(), DatasetName = "DataSet1" });


            return reportViewModel;


        }

        private byte[] imageToByteArray(Image myimg)
        {
            using (var ms = new MemoryStream())
            {
                myimg.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
        }

        private dynamic GetBarcodeText(long assignBoxId)
        {
            string barcodeTxt = string.Empty;
            AssignBox aBox = new AssignBox();


            aBox = repo.AssignBoxRepository.Find(assignBoxId);

            long itemId = aBox.ItemId;




            //var itemIdFormatted= (itemId * 5000).ToString();
            //var clientName = "";
            //var clientBoxId = "";

            return (itemId * 5000).ToString();
            //return new { itemId = itemIdFormatted, Name = clientName, BoxId = clientBoxId };

        }

        public ReportViewModelForBarCode GetBarCodeReportPrevious(List<BarCode> finalList)
        {
            var _barCodeList = new List<object>();
            foreach (BarCode item in finalList)
            {
                _barCodeList.Add(item);

            }

            var UserPrinting = HttpContext.Current.User.Identity.Name;
            string _transmittalNo = "Demo";

            var reportViewModel = new ReportViewModelForBarCode()
            {
                FileName = "~/ReportsHolder/BarCode.rdlc",
                Name = "Statistical Report",
                //  ReportDate = DateTime.Now,
                ReportTitle = "Barcode Images",
                //ClientName = trIn.Client.ClientName,
                //Department = trIn.Department.DepartmentName,
                //DepartmentCode = trIn.Department.DepartmentCode,
                UserNamPrinting = UserPrinting,
                TransmittalNo = _transmittalNo,
                PrintDate = DateTime.Now.ToString(),
                BarCodeText = "newbarcode here ",
                // ContactPerson = trIn.ContactPerson.ContactPersonName,
                //HandOverByName = GetHandOverBy(trIn, 0),
                //HandOverByAddress = GetHandOverBy(trIn, 1),
                //HandOverByDate = GetHandOverBy(trIn, 2),
                //// Status = GetStatus(trIn.Status),//trIn.Status.ToString(),
                //ReceivedByName = GetReceivedBy(trIn, 0),
                //ReceivedByAddress = GetReceivedBy(trIn, 1),
                //ReceivedByDate = GetReceivedBy(trIn, 2),
                //TransmittalDate = trIn.TransmittalDate.ToShortDateString(),
                //ClientReqRef = trIn.ClientRequestreference,
                //IssuedBy = trIn.ContactPerson.ContactPersonName,
                //Year = year,
                //ProjectCode = GetProjectCode(trIn),
                //Qty = qtyCount.ToString(),
                //Unit = "Box",
                //Item = "Box",
                //BoxNoArr = GetBoxs(trIn),
                //TransmittalNo = trIn.TransmittalNo,
                ReportLanguage = "en-US",

                Format = ReportViewModelForBarCode.ReportFormat.PDF,
                ViewAsAttachment = false,

            };
            reportViewModel.ReportDataSets.Add(new ReportViewModelForBarCode.ReportDataSet() { DataSetData = _barCodeList.ToList(), DatasetName = "DataSet1" });


            return reportViewModel;



        }
    }

    public interface IBarCodeRepository : IDisposable
    {
        IQueryable<BarCode> All { get; }
        IQueryable<BarCode> AllIncluding(params Expression<Func<BarCode, object>>[] includeProperties);
        BarCode Find(long id);
        void InsertOrUpdate(BarCode barcode);
        void Delete(long id);
        void Save();
    }
}