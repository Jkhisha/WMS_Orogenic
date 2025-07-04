using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseMVC.Models;

namespace WareHouseMVC
{
    public static class SessionManager
    {
        public static string ItemSessionString = "Session.Items";
        public static SessionValue SessionValueItem
        {

            get
            {
                if (null == System.Web.HttpContext.Current.Session["Session.Items"])
                {

                    System.Web.HttpContext.Current.Session["Session.Items"] = new SessionValue();

                }


                return (SessionValue)System.Web.HttpContext.Current.Session["Session.Items"];


            }

            set { System.Web.HttpContext.Current.Session["Session.Items"] = value; }
        }
    }
}