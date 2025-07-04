using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WareHouseMVC.Models
{
    public class Floor
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FloorID { get; set; }

        [Required]
        [Display(Name = "Floor name")]
        public string FloorName { get; set; }

        [Display(Name = "Floor code")]
        public string FloorCode { get; set; }

        public virtual Warehouse Warehouse { get; set; }

        [Required]
        [Display(Name = "Warehouse name")]
        public long WarehouseID { get; set; }

    }
}