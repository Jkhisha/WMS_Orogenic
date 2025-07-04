using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WareHouseMVC.Models
{
    public class ORBLDepartment
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ORBLDepartmentID { get; set; }

        [Required]
        [Display(Name = "Department name")]
        public string ORBLDepartmentName { get; set; }

        [Display(Name = "Department code")]
        public string ORBLDepartmentCode { get; set; }
    }
}