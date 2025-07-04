using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class AssignBoxViewModel
    {
        public long TransmittalINId { get; set; }
        public int TotalBox { get; set; }
        public string  TransmittalNo { get; set; }
        public string Status { get; set; }
        public DateTime TransmittalDate { get; set; }
    }
}