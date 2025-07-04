using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WareHouseMVC.Models
{
    public class Project
    {
        public long ProjectId { get; set; }

        [Required]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "Project Code")]
        public string ProjectCode { get; set; }

        public virtual Client Client { get; set; }

        [Required]
        [Display(Name = "Client Name")]
        public long ClientID { get; set; }

        public virtual Department Department { get; set; }

        [Required]
        [Display(Name = "Department Name")]
        public long DepartmentID { get; set; }


    }
}