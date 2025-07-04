using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WareHouseMVC.Models
{
    public class EmptyBox
    {
        public long EmptyBoxId { get; set; }
        public string EmptyBoxNo { get; set; }
        public long NoofBoxes { get; set; }
        public DateTime RecuisitionDate { get; set; }

        public virtual Client Client { get; set; }
        [Display(Name = "Client Name")]
        public long ClientID { get; set; }


        public virtual Department Department { get; set; }
        [Display(Name = "Department")]
        public long DepartmentID { get; set; }

        public string SubDepartment { get; set; }
        public virtual ORBLOperator ORBLOperator { get; set; }
        public long ORBLOperatorId { get; set; }
        public string OrblOperatorAddress { get; set; }

        public string DeliverTo { get; set; }
        public string Position { get; set; }
        public string Address { get; set; }

        public virtual ICollection<EmptyBoxBarcode> EmptyBoxBarcodes { get; set; }

    }
}