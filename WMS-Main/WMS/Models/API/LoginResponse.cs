using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models.API
{
    public class LoginResponse
    {
        public bool IsValid { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
    }
}