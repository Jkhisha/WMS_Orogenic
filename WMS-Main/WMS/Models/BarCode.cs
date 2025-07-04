using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class BarCode
    {
        public long BarCodeId { get; set; }
        public string ImagePathRel { get; set; }
        public virtual AssignBox AssignBox { get; set; }
        public long? AssignBoxId { get; set; }
        public string BoxNo { get; set; }
        public string Year { get; set; }
        public string ClientName { get; set; }
        public string DeptName { get; set; }
        public string BoxName { get; set; }
        public string ImagePathAbs { get; set; }
        public string ReportFiled { get; set; }
        public long? ItemId { get; set; }
        public virtual ICollection<EmptyBoxBarcode> EmptyBoxBarcodes { get; set; }
    }
}