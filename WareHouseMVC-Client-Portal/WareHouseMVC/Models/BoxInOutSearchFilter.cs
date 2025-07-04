using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class BoxInOutSearchFilter
    {
        public long ClientID { get; set; }
        public long? DepartmentID { get; set; }
        public DateTime Month { get; set; }
        public int Service { get; set; }
        public int? ReportType { get; set; }
    }
}