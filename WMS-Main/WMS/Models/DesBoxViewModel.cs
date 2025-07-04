using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class DesBoxViewModel
    {
        public long ClientID { get; set; }
        public long DepartmentID { get; set; }
        public List<Item> SearchResults { get; set; }
    }
}