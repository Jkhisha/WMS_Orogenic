using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class DuplicateBoxViewModel
    {
        public string BoxName { get; set; }
        public string BoxNo { get; set; }
        public string Year { get; set; }
        public long AssignBoxId { get; set; }
        public long ItemId { get; set; }
        public long TransmittalINStatusId { get; set; }

        public string TransmittalInStatus { get; set; }
        public string TransmittalINNo { get; set; }
        public string BoxCurrentStatus { get; set; }



    }
}