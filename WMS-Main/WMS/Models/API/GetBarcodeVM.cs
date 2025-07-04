using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models.API
{
    public class GetBarcodeVM
    {
        public string Message { get; set; }
        public bool IsValid { get; set; }
        public string UserId { get; set; }
        public TrBarCodeMobileResponse Model { get; set; }

    }
}