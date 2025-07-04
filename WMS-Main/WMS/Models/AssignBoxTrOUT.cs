using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class AssignBoxTrOUT
    {
        public long AssignBoxTrOUTId { get; set; }
        
        public virtual TransmittalOUT TransmittalOUT { get; set; }
        public long TransmittalOUTId { get; set; }

        public virtual TransmittalOUTStatus TransmittalOUTStatus { get; set; }
        public long TransmittalOUTStatusId { get; set; }

        public virtual Warehouse Warehouse { get; set; }
        public long WarehouseID { get; set; }
        public string WarehouseName { get; set; }

        public long[] ItemIds { get; set; }
        public virtual Item Item { get; set; }
        public long ItemId { get; set; }

        public string BoxNo { get; set; }
        public DateTime? DestructionPeriod { get; set; }
        public DateTime AssignDate { get; set; }

        public string BoxNameFile { get; set; }
        public string FileNumber { get; set; }
        public string ReferrenceNo { get; set; }
        public string RingNo { get; set; }
        public string AccountNo { get; set; }

        


    }
}