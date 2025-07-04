using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class ChangeLocation
    {
        public long ChangeLocationId { get; set; }
        public long ItemId { get; set; }
        public string Location { get; set; }
        public DateTime AssignDate { get; set; }

    }
}