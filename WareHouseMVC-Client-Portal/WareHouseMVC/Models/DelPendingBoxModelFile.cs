using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class DelPendingBoxModelFile
    {
        public long DelPendingBoxModelFileId { get; set; }
        public string BoxNo { get; set; }
        public string BoxNameFile { get; set; }
        public string FileNumber { get; set; }
        public string FileDescription { get; set; }
        public string ReferrenceNo { get; set; }
        public string RingNo { get; set; }
        public string AccountNo { get; set; }
        public string Year { get; set; }
        public string TransmittalOutNo { get; set; }
        public string ClientName { get; set; }
        public string Department { get; set; }
        public DateTime TransmittalOutDate { get; set; }
    }
}