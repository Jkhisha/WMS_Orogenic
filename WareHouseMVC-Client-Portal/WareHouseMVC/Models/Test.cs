using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WareHouseMVC.Models
{
    public class Test
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TestID { get; set; }

        public string TestName { get; set; }
        public decimal Amount { get; set; }
        public bool Status { get; set; }
        public bool IsRemoved { get; set; }
    }
}