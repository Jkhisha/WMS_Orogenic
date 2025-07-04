using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class BoxStatementViewModel
    {

        public long ClientID { get; set; }
        public DateTime Month { get; set; }
        public double  Rate { get; set; }
        public long BaseBoxNo { get; set; }
        public double ExtraRate { get; set; }
        public int? ReportType { get; set; }

    }
}