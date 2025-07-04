using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class TransmittalOUTBArcodeViewModel
    {
        public string BoxName { get; set; }
        public string BoxNo { get; set; }
        public string Year { get; set; }
        public string ClientName { get; set; }
        public string Dept { get; set; }
        public string BarCodeText { get; set; }
    }
}