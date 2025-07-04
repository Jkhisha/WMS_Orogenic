using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class HostInformation
    {
        public long HostInformationId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Att { get; set; }
        public string Position { get; set; }
        public string Cell { get; set; }
        public string Teliphone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }


        #region SMTP Config

        public string AdminMailAddress { get; set; }
        public string SMTPServer { get; set; }
        public string SMPTPost { get; set; }
        public string SMTPEmail { get; set; }
        public string SmtpPassword { get; set; }

        #endregion
       


    }
}