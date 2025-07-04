using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class ClientMenuSetUP
    {
        public long ClientMenuSetUPID { get; set; }
        public string MenuName { get; set; }
        public long ClientId { get; set; }
    }
}