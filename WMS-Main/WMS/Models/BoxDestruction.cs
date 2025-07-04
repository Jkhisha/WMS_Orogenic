using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class BoxDestruction
    {
        public long BoxDestructionId { get; set; }

        public virtual Client Client { get; set; }
        public long ClientID { get; set; }
        public string ClientName { get; set; }
        public virtual Department Department { get; set; }
        public long DepartmentID { get; set; }
        public string DepartmentName { get; set; }

        public virtual Item Item { get; set; }
        public long ItemId { get; set; }

        public string BoxName { get; set; }
        public string BoxNumner { get; set; }
        public string BoxYear { get; set; }


        public DateTime DestructionDate { get; set; }
        public DateTime RequestDate { get; set; }
        public int CurrentStatus { get; set; }
        public string RequestBy { get; set; }
        public string DestructedBy { get; set; }




    }
}