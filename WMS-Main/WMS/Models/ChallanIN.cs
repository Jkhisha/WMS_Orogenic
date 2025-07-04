using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WareHouseMVC.Models
{
    public class ChallanIN
    {
        public long ChallanINId { get; set; }

        public string ChallanBy { get; set; }
        public int Status { get; set; }
        public DateTime ChallanDate { get; set; }
        public long TransmittalINId { get; set; }

        public virtual Warehouse Warehouse { get; set; }
        public long WarehouseID { get; set; }



    }
}