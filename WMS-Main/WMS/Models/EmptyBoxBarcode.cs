using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class EmptyBoxBarcode
    {
        public long Id { get; set; }
        public long EmptyBoxId { get; set; }
        public long BarCodeId { get; set; }
        public string BarCodeText { get; set; }

    }
}