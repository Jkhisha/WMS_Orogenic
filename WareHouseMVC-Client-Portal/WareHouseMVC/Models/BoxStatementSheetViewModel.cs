using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class BoxStatementSheetViewModel
    {
        public string Department { get; set; }
        public int NoofBox { get; set; }
        public int NewBox { get; set; }
        public int TotalBox { get; set; }
        public double Rate { get; set; }
        public double TotalAmount { get; set; }

    }
}