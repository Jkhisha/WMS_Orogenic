using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class InvoiceFieldsViewModel
    {
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string Amount { get; set; }
    }
}