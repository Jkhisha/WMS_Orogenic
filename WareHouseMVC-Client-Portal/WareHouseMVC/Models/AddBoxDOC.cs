using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class AddBoxDOC
    {
        
            public int AddBoxDOCId { get; set; }
            public string BoxName { get; set; }
            public string BoxNo { get; set; }
            public string FileName { get; set; }
            public string TransmittalNo { get; set; }
            public string DeptName { get; set; }
            public string SubDeptName { get; set; }
            public DateTime date { get; set; }

    }
}