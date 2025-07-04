using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class DelPendingBoxModel
    {
        public long DelPendingBoxModelId { get; set; }
        public string BoxName { get; set; }
        public string BoxNo { get; set; }
        public string Year { get; set; }
        public string TransmittalOutNo { get; set; }
        public string ClientName { get; set; }
        public string Department { get; set; }
        public long ClientID {get;set;}
        public long DepatID { get; set; }
        public DateTime TransmittalOutDate { get; set; }
    }
}