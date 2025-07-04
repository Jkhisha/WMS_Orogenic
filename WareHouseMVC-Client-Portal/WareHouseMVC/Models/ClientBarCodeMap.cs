using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class ClientBarCodeMap
    {
        [Key]
        public int Id { get; set; }
        public string Encoding { get; set; }
        public string OrogenicBarCodeText { get; set; }
        public string ClientBarCodeText { get; set; }
    }
}