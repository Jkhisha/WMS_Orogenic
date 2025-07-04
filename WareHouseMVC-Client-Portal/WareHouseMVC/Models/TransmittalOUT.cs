using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WareHouseMVC.Models
{
    public class TransmittalOUT
    {
        public long TransmittalOUTId { get; set; }



        [Required]
        [Display(Name = "Transmittal No")]
        public string TransmittalNo { get; set; }


        [Display(Name = "Transmittal Date")]
        public DateTime TransmittalDate { get; set; }


        [Display(Name = "Client Request reference")]
        public string ClientRequestreference { get; set; }

        public DateTime CreateDate { get; set; }


        public virtual Client Client { get; set; }

        [Required]
        [Display(Name = "Client Name")]
        public long ClientID { get; set; }



        public virtual Department Department { get; set; }

        [Required]
        [Display(Name = "Department Name")]
        public long DepartmentID { get; set; }

        public string SubDepartment { get; set; }

        [Display(Name = "Project Name")]
        public long? ProjectId { get; set; }


        public virtual ContactPerson ContactPerson { get; set; }

        [Required]
        [Display(Name = "Issued By ")]
        public long ContactPersontID { get; set; }


        //public virtual ORBLOperator ORBLOperator { get; set; }
        //public long ORBLOperatorId { get; set; }
        //public string OrblOperatorAddress { get; set; }


        public string FileUrl { get; set; }


        public virtual TransmittalOUTStatus TransmittalOUTStatus { get; set; }
        public long TransmittalOUTStatusId { get; set; }
        public string Type { get; set; }
        public bool IsFile { get; set; }

        public bool? IsUrgent { get; set; }
        public bool? IsPermanent { get; set; }
        public DateTime? RearchiveDate { get; set; }

        public virtual List<Item> Items { get; set; }

        public virtual List<ReceivedBy> ReceivedBy { get; set; }
        public virtual List<ORBLOperator> HandOverBy { get; set; }

    }
}