using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WareHouseMVC.Models
{
   public  class TrBarCodeMobileResponse
    {
       
        public string TrId { get; set; }
        public string TrDate { get; set; }
        public string Client { get; set; }
        public string Department { get; set; }
        public string Message { get; set; }
        public List<TrBarCodeModel> BarCodes { get; set; }

    }
}
