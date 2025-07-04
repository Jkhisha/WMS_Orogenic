using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class AutoZoneSuggention
    {
        public long AutoZoneSuggentionId { get; set; }
        public long ClientID { get; set; }
        public long ZoneID { get; set; }
        public virtual Client Client { get; set; }
        public virtual Zone Zone { get; set; }
    }
}