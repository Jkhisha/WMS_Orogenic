using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Reporting.WebForms;
using WareHouseMVC.Models;

namespace WareHouseMVC.Reports
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["ReportID"] != null)
                {

                    string reportID = Request.QueryString["ReportID"].ToString();
                    switch (reportID)
                    {
                        case "CustomersListReport":
                            ReportViewer1.Reset();
                            List<Test> dt = GetTestReport();
                            ReportViewer1.LocalReport.ReportPath = "Reports\\GeneralReport.rdlc";
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("NewDataSet", dt));

                            ReportViewer1.DataBind();
                            ReportViewer1.LocalReport.Refresh();
                            break;
                            default:
                            break;
                    }
                }
            }
        }

        private List<Test> GetTestReport()
        {

            using (UnitOfWork unit = new UnitOfWork())
            {
                List<Test> test = new List<Test>();
                //test = unit.TestRepository.GetAll();
               
                return test;
               
            }


        }
    }
}