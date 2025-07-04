using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class BarcodeMappingViewModel
    {
        public long BarcodeMappingId { get; set; }
        public string TransmittalNo { get; set; }
        public long BarCodeId { get; set; }
        public long ItemId { get; set; }
        public string ClientName { get; set; }
        public string DeptName { get; set; }
        public string ImagePath { get; set; }

        public string BoxNo { get; set; }
        public string BoxName { get; set; }
        public string Year { get; set; }
        public string BarcodeNo { get; set; }
    }
}