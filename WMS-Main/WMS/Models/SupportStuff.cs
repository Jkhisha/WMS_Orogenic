using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WareHouseMVC.Models
{
    public class SupportStuff
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SupportStuffID { get; set; }

        [Required]
        [Display(Name = "SupportStuff name")]
        public string SupportStuffName { get; set; }

        [Display(Name = "SupportStuff code")]
        public string SupportStuffCode { get; set; }

        [Required]
        [Display(Name = "SupportStuff MObileNo")]
        public string SupportStuffMobileNo { get; set; }

        public virtual ORBLDepartment ORBLDepartment { get; set; }

        [Required]
        [Display(Name = "ORBLDepartment name")]
        public long ORBLDepartmentID { get; set; }

    }
}