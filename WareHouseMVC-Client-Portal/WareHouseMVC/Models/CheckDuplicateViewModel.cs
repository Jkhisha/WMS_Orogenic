using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class CheckDuplicateViewModel
    {
        public long ClientID { get; set; }
        public long DepartmentID { get; set; }
        public virtual List<DuplicateBoxViewModel> boxList { get; set; }
    }
}