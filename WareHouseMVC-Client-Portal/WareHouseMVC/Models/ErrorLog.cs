using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class ErrorLog
    {
        public long ErrorLogId { get; set; }
        public string ErrorType { get; set; }
        public string ErrorMsg { get; set; }
        public DateTime ErrorTime { get; set; }

    }
}