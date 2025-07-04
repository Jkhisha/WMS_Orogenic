using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WareHouseMVC.Models
{
    public class ContactPerson
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ContactPersontID { get; set; }

        [Required]
        [Display(Name = "Contact Person name")]
        public string ContactPersonName { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Position")]
        public string Position { get; set; }


        public virtual Client Client { get; set; }

        [Required]
        [Display(Name = "Client name")]
        public long ClientID { get; set; }


        public virtual Department Department { get; set; }

        [Required]
        [Display(Name = "Department name")]
        public long DepartmentID { get; set; }

    }
}