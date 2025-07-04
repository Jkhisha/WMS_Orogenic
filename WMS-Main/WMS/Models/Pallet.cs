using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WareHouseMVC.Models
{
    public class Pallet
    {
        public long PalletId { get; set; }

        [Required]
        [Display(Name = "Pallet Name")]
        public string PalletName { get; set; }


        public virtual Zone Zone { get; set; }

        [Required]
        [Display(Name = "Zone name")]
        public long ZoneID { get; set; }


        public virtual Floor Floor { get; set; }

        [Required]
        [Display(Name = "Floor name")]
        public long FloorID { get; set; }

        public virtual Warehouse Warehouse { get; set; }

        [Required]
        [Display(Name = "Warehouse name")]
        public virtual long WarehouseID { get; set; }
    }
}