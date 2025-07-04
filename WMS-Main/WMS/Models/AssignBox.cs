using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WareHouseMVC.Models
{
    public class AssignBox
    {

        public long AssignBoxId { get; set; }
        
        public virtual TransmittalIN TransmittalIN {get;set;}
        public long TransmittalINId { get; set; }

        public long[]  ItemIds { get; set; }
        public virtual Warehouse Warehouse { get; set; }


        [Display(Name = "Warehouse name")]
        public long WarehouseID { get; set; }


        public string  WarehouseName { get; set; }

        public virtual Item Item { get; set; }
        public long ItemId { get; set; }

        public string BoxName { get; set; }
        public string BoxNo { get; set; }
        public string Year { get; set; }


        public string BoxNameFile { get; set; }
        public string FileNumber { get; set; }
        public string ReferrenceNo { get; set; }
        public string RingNo { get; set; }
        public string AccountNo { get; set; }




        public DateTime? DestructionPeriod { get; set; }

        public DateTime AssignDate { get; set; }

        public bool  IsLatest { get; set; }

        public virtual TransmittalINStatus TransmittalINStatus { get; set; }

        public long TransmittalINStatusId { get; set; }

        public long? BarCodeId { get; set; }

        public string BarCodeText { get; set; }

        public int CurrentStatus { get; set; }
        public string Category { get; set; }

        public virtual List<BoxLocation> BoxLocation { get; set; }
        public bool IsLegalHold { get; set; }
        public string DocPath { get; set; }
        public string ClientBarCode { get; set; }

    }
}