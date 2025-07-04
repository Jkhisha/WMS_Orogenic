using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WareHouseMVC.Models
{
    public class Zone
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ZoneID { get; set; }

        [Required]
        [Display(Name = "Zone name")]
        public string ZoneName { get; set; }

        [Display(Name = "Zone code")]
        public string ZoneCode { get; set; }

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