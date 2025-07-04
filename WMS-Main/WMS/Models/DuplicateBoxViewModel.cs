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


        public string FileBoxName { get; set; }
        public string FileNumber { get; set; }
        public string ReferrenceNo { get; set; }
        public string RingNo { get; set; }
        public string AccountNo { get; set; }
        public string Unit { get; set; }

        public string TransmittalInStatus { get; set; }
        public string TransmittalINNo { get; set; }
        public string BoxCurrentStatus { get; set; }



    }
}