using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace WareHouseMVC.Models
{
    public class BoxSearchViewModel
    {
        public int? Page { get; set; }
        public long ClientID { get; set; }
        public long? DepartmentID { get; set; }
        public string SubDepartment { get; set;  }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string BoxName { get; set; }
        public string BoxNo { get; set; }
        public List<AssignBox> ListItems { get; set; }
        public IPagedList<AssignBox> SearchResults { get; set; }
        public int? ReportType { get; set; }
        public bool IsLegal { get; set; }



    }
}