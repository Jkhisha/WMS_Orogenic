using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class ClientUser
    {
        public long ClientUserId { get; set; }
        public long ClientId { get; set; }
        public string ClientName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public Guid UserId { get; set; }
        public long? DepartmentID { get; set; }
        public string SubDepartment { get; set; }
        public bool IsFirstLogin { get; set; } = false;
    }
}