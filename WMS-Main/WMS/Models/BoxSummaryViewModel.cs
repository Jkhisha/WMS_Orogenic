using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class BoxSummaryViewModel
    {
        public long ClientID { get; set; }
        public long DepartmentID { get; set; }
        public string Client { get; set; }
        public string Department { get; set; }
        public long TotalBox { get; set; }
        public int? ReportType { get; set; }
    }
}