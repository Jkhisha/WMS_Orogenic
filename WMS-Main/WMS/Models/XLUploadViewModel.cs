using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class XLUploadViewModel
    {
        [Required]
        public long ClientId1 { get; set; }
        [Required]
        public long DeptId1 { get; set; }
        [Required]
        public long ClientId2 { get; set; }
        [Required]
        public long DeptId2 { get; set; }
        [Required]
        public long ClientId3 { get; set; }
        [Required]
        public long DeptId3 { get; set; }
    }
}