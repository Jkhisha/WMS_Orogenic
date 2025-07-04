using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class TransmittalINAuditTrail
    {
        public long TransmittalINAuditTrailId { get; set; }
        public string TransmittalINNo { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateTime { get; set; }
        public string CreateByIP { get; set; }
        public string BoxReceivedBy { get; set; }
        public DateTime BoxReceivedByDate { get; set; }
        public string BoxReceivedByTime { get; set; }
        public string BoxReceivedByIP { get; set; }
        public string BarcodeVerifiedBy { get; set; }

    }
}