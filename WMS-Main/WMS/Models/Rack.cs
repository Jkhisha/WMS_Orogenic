using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WareHouseMVC.Models
{
    public class Rack
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RackID { get; set; }

        [Required]
        [Display(Name = "Rack name")]
        public string RackName { get; set; }

        [Display(Name = "Rack code")]
        public string RackCode { get; set; }


        public virtual Train Train { get; set; }

        [Required]
        [Display(Name = "Train name")]
        public long TrainID { get; set; }

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