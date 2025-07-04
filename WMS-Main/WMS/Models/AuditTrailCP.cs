using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WareHouseMVC.Models
{
   public class AuditTrailCP
    {
        public long AuditTrailCPId { get; set; }
        public string UserName { get; set; }
        public DateTime ChangingDate { get; set; }
        public string PreBoxName { get; set; }
        public string PreBoxNumber { get; set; }
        public string PreBoxYear { get; set; }
        public string NewBoxName { get; set; }
        public string NewBoxNumber { get; set; }
        public string NewBoxYear { get; set; }
        public string PreComments { get; set; }
        public string NewComments { get; set; }
        public long? ClientId { get; set; }

    }
}
