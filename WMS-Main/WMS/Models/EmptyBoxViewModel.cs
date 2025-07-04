using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class EmptyBoxViewModel
    {
        public string  Date { get; set; }
        public string DeliverTo { get; set; }
        public long Quantity { get; set; }
        public int Time { get; set; }
        public double Rate { get; set; }
        public double Amount { get; set; }


    }
}