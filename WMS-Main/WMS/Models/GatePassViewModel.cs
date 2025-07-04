using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class GatePassViewModel
    {
        public double  Qty { get; set; }
        public string  WareHouseName { get; set; }
        public string  TransferTo { get; set; }
        public string BoxNo { get; set; }

        
    }
}