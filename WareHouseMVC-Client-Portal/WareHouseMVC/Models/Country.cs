using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WareHouseMVC.Models
{
    public class Country
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CountryID { get; set; }

        public string CountryName { get; set; }
        public bool Status { get; set; }
        public bool IsRemoved { get; set; }

    }
}