using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WareHouseMVC.Models
{
    public class Client
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ClientID { get; set; }

        [Required]
        [Display(Name = "Client name")]
        public string ClientName { get; set; }

        [Required]
        [Display(Name = "Client address")]
        public string ClientAddress { get; set; }

        public virtual SupportStuff SupportStuff { get; set; }

        [Required]
        [Display(Name = "SupportStuffe")]
        public long SupportStuffID { get; set; }

        public string  Att { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public string Cell { get; set; }
        public string Tell { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public Guid UserId { get; set; }

        public List<ClientUser> ClientUserList { get; set; }


        
    }
}