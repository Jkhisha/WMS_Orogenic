using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class InvoiceViewModel
    {

        public long InvoiceViewModelId { get; set; }
        public DateTime Month { get; set; }

        public virtual Client Client { get; set; }
        public long ClientID { get; set; }

        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }

        public string RevisionNo { get; set; }
        public DateTime RevisionDate { get; set; }
        public string PurchaseOrderNo { get; set; }
        public DateTime PurchaseOrderDate { get; set; }

        public string ContactNo { get; set; }
        public string PaymentTerm { get; set; }
        public DateTime GRDate { get; set; }
        public string GRNo { get; set; }

        public double? TotalAmount { get; set; }
        public double? TotalFinalAmount { get; set; }
        public double? VAT { get; set; }
        public double? VATCalculatedAmount { get; set; }
        public double? TotalPayable { get; set; }
        public string Note { get; set; }
        public double? UnitPrice { get; set; }

        public string VATRegNo { get; set; }
        public string TAXRegNo { get; set; }

        public long? ClosingBalance { get; set; }
        public long? TransmittalINBalance { get; set; }
        public long? TransmittalOUTBalance { get; set; }

        #region Data Item Fields

        public string TransportationInChrgeDes { get; set; }
        public long? TransportationInChrgeQty { get; set; }
        public double? TransportationInChrgePrice { get; set; }


        public string TransportationOUTChrgeDes { get; set; }
        public long? TransportationOUTChrgeQty { get; set; }
        public double? TransportationOUTChrgePrice { get; set; }

        public string TransportationEmptyBoxDes { get; set; }
        public long? TransportationEmptyBoxQty { get; set; }
        public double? TransportationEmptyBoxPrice { get; set; }

        public string TransportationEmptyCartoonDes { get; set; }
        public long? TransportationEmptyCartoonQty { get; set; }
        public double? TransportationEmptyCartoonPrice { get; set; }

        public string FileTransferDes { get; set; }
        public long? FileTransferQty { get; set; }
        public double? FileTransferPrice { get; set; }

        public string PhotocopyDes { get; set; }
        public long? PhotocopyQty { get; set; }
        public double? PhotocopyPrice { get; set; }

        public string WasteSampleDes { get; set; }
        public long? WasteSampleQty { get; set; }
        public double? WasteSamplePrice { get; set; }

        public string MagneticTapeDes { get; set; }
        public long? MagneticTapeQty { get; set; }
        public double? MagneticTapePrice { get; set; }

        public string ScanningDes { get; set; }
        public long? ScanningQty { get; set; }
        public double? ScanningPrice { get; set; }

        public string OthersDes { get; set; }
        public long? OthersQty { get; set; }
        public double? OthersPrice { get; set; }



        #endregion

        #region Bank Info

        public string BankName { get; set; }
        public string BankAddress { get; set; }
        public string BankTel { get; set; }
        public string BankACC { get; set; }
        public string BranchName { get; set; }
        public string SwiftCode { get; set; }

       
        #endregion




    }
}