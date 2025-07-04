using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class SessionValue
    {
        public long SessionValueId { get; set; }

        public long WarehouseIDd { get; set; }
        public string WarehouseName { get; set; }
        public string BarcodeServerAddress { get; set; }
    }
}