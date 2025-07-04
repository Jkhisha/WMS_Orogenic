using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Web.UI.WebControls;


namespace WareHouseMVC.Models
{
    public class EnumHelper
    {

        public EnumHelper()
        {
            
        }


        public enum Status
        {
            Pending = 1,
            WareHouse_Assigned=2,
            Received_at_WareHouse=3,
            Barcode_Assigned=4,
            Barcode_Verified = 5,
            Location_Assigned = 6,
            Location_Varified=319

        }

        public enum TrOUTStatus
        {
            Pending = 1,
            TrOUT_Generated=2,
            Approved_at_WareHouse = 3,
            Gate_Pass_Generated = 4,
            Box_Out_from_WareHouse = 5,
            Box_Not_Found=6
      
        }


        public enum IsNeworReArchive
        {
            ReArchive = 0,
            New
        }

     
    }
}