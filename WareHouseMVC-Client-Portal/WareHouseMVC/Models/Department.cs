using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WareHouseMVC.Models
{
    public class Department
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long DepartmentID { get; set; }

        [Required]
        [Display(Name = "Department name")]
        public string DepartmentName { get; set; }

        [Display(Name = "Department code")]
        public string DepartmentCode{ get; set; }

        
        [Display(Name = "Department Address")]
        public string DepartmentAddress { get; set; }

        public virtual Client Client { get; set; }

        [Required]
        [Display(Name = "Client name")]
        public long ClientID { get; set; }

    }
}