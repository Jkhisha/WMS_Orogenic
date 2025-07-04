using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class ChangeDepartmentViewModel
    {
        public long ClientId { get; set; }
        public long OldDeptId { get; set; }
        public long NewDeptId { get; set; }
        public long[] ItemIds { get; set; }
        public virtual List<Item> itemList { get; set; }


    }
}