using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WareHouseMVC
{
    /// <summary>
    /// Summary description for XMLTest
    /// </summary>
    [WebService(Namespace = "http://mainserver/wms/XMLTest")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class XMLTest : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string cblTest(string cardNoActual, string password, string username)
        {

            //string _tempEmail = "mrromel@hotmail.com";
            string accountName = "success";






            return accountName;
        }
    }
}
