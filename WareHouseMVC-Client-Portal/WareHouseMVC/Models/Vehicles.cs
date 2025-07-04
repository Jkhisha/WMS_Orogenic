using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WareHouseMVC.Models
{
    public class Vehicle
    {
        public long VehicleId { get; set; }


        [Required]
        [Display(Name = "Vehicle type")]
        public string Type { get; set; }


        [Required]
        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; set; }

        public virtual TransportVendor TransportVendor { get; set; }

        [Required]
        [Display(Name = "Transport Vendor name")]
        public virtual long TransportVendorId { get; set; }


    }
}