using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class DesPeriodReportViewModel
    {
        public string Client { get; set; }
        public string Dept { get; set; }
        public string BoxName { get; set; }
        public string BoxNo { get; set; }
        public DateTime? DesPeriod { get; set; }
        public string Year { get; set; }
        public string Unit { get; set; }

    }
}