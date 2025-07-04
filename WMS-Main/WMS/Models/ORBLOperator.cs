using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class ORBLOperator
    {
        public long ORBLOperatorId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public virtual List<TransmittalOUT> TransmittalOUT { get; set; }
        public virtual List<TransmittalIN> TransmittalIN { get; set; }
    }
}