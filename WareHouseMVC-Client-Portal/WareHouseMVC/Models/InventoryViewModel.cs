using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class InventoryViewModel
    {
        public long ClientID { get; set; }
        public long WareHouseID { get; set; }
        public string ClientName { get; set; }
        public string WareHouseName { get; set; }
        public string DeptName { get; set; }
        public long BoxTotal { get; set; }
    }
}