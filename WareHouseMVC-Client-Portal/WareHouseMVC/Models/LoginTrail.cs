using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WareHouseMVC.Models
{
    public class LoginTrail
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LoginTrainId { get; set; }
        public string LoginBy { get; set; }
        public DateTime LoginDate { get; set; }
        public string LoginTime { get; set; }
        public string LocalIP { get; set; }
        

    }
}