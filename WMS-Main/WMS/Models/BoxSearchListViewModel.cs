using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;

namespace WareHouseMVC.Models
{
    public class BoxSearchListViewModel
    {
        public long AssignBoxId { get; set; }

        public virtual TransmittalIN TransmittalIN { get; set; }
        public long TransmittalINId { get; set; }

        public long[] ItemIds { get; set; }
        public virtual Warehouse Warehouse { get; set; }


        [Display(Name = "Warehouse name")]
        public long WarehouseID { get; set; }


        public string WarehouseName { get; set; }

        public virtual Item Item { get; set; }
        public long ItemId { get; set; }

        public string BoxName { get; set; }
        public string BoxNo { get; set; }
        public string Year { get; set; }

        public DateTime? DestructionPeriod { get; set; }

        public DateTime AssignDate { get; set; }

        public bool IsLatest { get; set; }

        public virtual TransmittalINStatus TransmittalINStatus { get; set; }

        public long TransmittalINStatusId { get; set; }

        public long? BarCodeId { get; set; }

        public int CurrentStatus { get; set; }
       

        public virtual List<BoxLocation> BoxLocation { get; set; }

        public string TrOutDate { get; set; }

    }
}