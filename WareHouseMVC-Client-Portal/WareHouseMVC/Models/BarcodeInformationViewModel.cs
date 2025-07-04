using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class BarcodeInformationViewModel
    {
        public string BoxName { get; set; }
        public string BoxNo { get; set; }
        public string Year { get; set; }
        public string Client { get; set; }
        public string Dept { get; set; }
        public string DestructionDate { get; set; }
        public string isLegal { get; set; }
        public string category { get; set; }
        public string titleOfContent { get; set; }
        public string CurrentStatus { get; set; }
        public string TransmittalStatus { get; set; }
        public string ReceivedBy { get; set; }
        public string BarcodeVerifiedBy { get; set; }
    }
}