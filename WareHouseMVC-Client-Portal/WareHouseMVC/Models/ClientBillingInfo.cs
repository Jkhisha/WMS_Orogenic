using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class ClientBillingInfo
    {
        public long ClientBillingInfoId { get; set; }
        public string VATRegNo { get; set; }
        public string TAXRegNo { get; set; }
        public string BankName { get; set; }
        public string BankAddress { get; set; }
        public string BankTel { get; set; }
        public string BankACC { get; set; }
        public string BranchName { get; set; }
        public string BankSwiftCode { get; set; }
        public string ClientAtt { get; set; }
        public string ClientAttPosition { get; set; }

        public virtual Client Client { get; set; }
        public long ClientID { get; set; }
    }
}