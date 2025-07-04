using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WareHouseMVC.Models
{
    public class DestructionHistory
    {
        public long DestructionHistoryId { get; set; }
        public virtual Item Item { get; set; }
        public long ItemId { get; set; }

        public string DestroyedBy { get; set; }
        public DateTime DestructionDate { get; set; }
        public string BoxName { get; set; }
        public string BoxNumber { get; set; }
        public string BoxYear { get; set; }


       


    }
}
