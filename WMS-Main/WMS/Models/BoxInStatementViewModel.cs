using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class BoxInStatementViewModel
    {
        public string Date { get; set; }
        public string Department { get; set; }
        public string Month { get; set; }
        public string BoxNo { get; set; }
        public long Quantity { get; set; }
        public long Extra { get; set; }
        public long Time { get; set; }
        public double Rate { get; set; }
        public double Amount { get; set; }

    }
}