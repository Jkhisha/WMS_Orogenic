using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WareHouseMVC.Models
{
    public class Item
    {

        public long ItemId { get; set; }


        public string ItemName { get; set; }

        public string Description { get; set; }
        public string Category { get; set; }

        public string Quantity { get; set; }
        public string Year { get; set; }


        public string Unit { get; set; }
        [Required]
        public string BoxNo { get; set; }

        public DateTime? DestructionPeriod { get; set; }

        public virtual Client Client { get; set; }
        public long ClientID { get; set; }

        public virtual Department Department { get; set; }
        public long DepartmentID { get; set; }
        public string SubDepartment { get; set; }

        public long? ProjectId { get; set; }

        public int IsNew { get; set; }

        //File Level Entry

        public string FileBoxName { get; set; }
        public string FileNumber { get; set; }
        public string FileDescription { get; set; }
        public string ReferrenceNo { get; set; }
        public string RingNo { get; set; }
        public string AccountNo { get; set; }
        public string Locations { get; set; }
        public string Comments { get; set; }

        //File Level Entry


        [DefaultValue(false)]
        public bool IsArchieve { get; set; }
        public int? InCount { get; set; }
        public int? OutCount { get; set; }

        public List<TransmittalIN> TransmittalINs { get; set; }
        public List<TransmittalOUT> TrasmittalOUTs { get; set; }
        public int IsDelete { get; set; }

        public string BarCodeText { get; set; }
        public bool IsLegalHold { get; set; }
        public string DocPath { get; set; }

    }
}