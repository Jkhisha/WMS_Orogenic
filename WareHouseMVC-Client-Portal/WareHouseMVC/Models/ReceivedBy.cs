using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class ReceivedBy
    {

        public long ReceivedById { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }
        public List<TransmittalOUT> TrasmittalOUTs { get; set; }
    }
}