using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WareHouseMVC.Models
{
    public class Driver
    {
        public long DriverId { get; set; }


        [Required]
        [Display(Name = "Driver Name")]
        public string Name { get; set; }


        [Required]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        public virtual TransportVendor TransportVendor { get; set; }

        [Required]
        [Display(Name = "Transport Vendor name")]
        public virtual long TransportVendorId { get; set; }
    }
}