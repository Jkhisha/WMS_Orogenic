using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class TransmittalOUTAuditTrail
    {
        public long TransmittalOUTAuditTrailId { get; set; }
        public string TransmittalOUTNo { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string CreateTime { get; set; }
        public string CreatorIP { get; set; }
        public string BoxOutBy { get; set; }
        public DateTime BoxOutDate { get; set; }
        public string BoxOutTime { get; set; }
        public string BoxOutIP { get; set; }

    }
}