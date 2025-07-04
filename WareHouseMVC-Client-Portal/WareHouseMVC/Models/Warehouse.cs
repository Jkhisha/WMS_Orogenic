using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WareHouseMVC.Models
{
    public class Warehouse
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long WarehouseID { get; set; }

        [Required]
        [Display(Name = "Warehouse name")]
        public string WarehouseName { get; set; }

        [Display(Name = "Warehouse code")]
        public string WarehouseCode { get; set; }
    }
}