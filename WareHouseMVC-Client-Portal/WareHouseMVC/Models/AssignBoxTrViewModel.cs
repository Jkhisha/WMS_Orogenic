using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class AssignBoxTrViewModel
    {
        public long TransmittalOUTId { get; set; }
        public int TotalBox { get; set; }
        public string TransmittalNo { get; set; }
        public string Status { get; set; }
        public DateTime TransmittalDate { get; set; }
    }
}