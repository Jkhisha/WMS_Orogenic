using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WareHouseMVC.Models
{
    public class TransportVendor
    {

        public long TransportVendorId { get; set; }

        [Required]
        [Display(Name = "Vendor name")]
        public string Name { get; set; }


        [Display(Name = "Vendor Code")]
        public string Code { get; set; }


        [Display(Name = "Contact No")]
        public string ContactNo { get; set; }

        public virtual List<Vehicle> Vehicles { get; set; }
    }
}