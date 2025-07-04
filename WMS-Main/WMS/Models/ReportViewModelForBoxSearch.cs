using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;

namespace WareHouseMVC.Models
{
    public class ReportViewModelForBoxSearch
    {
        public enum ReportFormat { PDF = 1, Word = 2, Excel = 3 }
        public ReportViewModelForBoxSearch()
        {
            //initation for the data set holder
            ReportDataSets = new List<ReportDataSet>();
        }

        public string Name { get; set; }
        public string ReportLanguage { get; set; }
        public string FileName { get; set; }
        public string ReportTitle { get; set; }
        public DateTime ReportDate { get; set; }
        public string UserNamPrinting { get; set; }
        public List<ReportDataSet> ReportDataSets { get; set; }
        public ReportFormat Format { get; set; }
        public bool ViewAsAttachment { get; set; }
        public string PrintDate { get; set; }


        public string ClientName { get; set; }
        public string Department { get; set; }

        public string HostName { get; set; }
        public string HostAddress { get; set; }
        public string HostAtt { get; set; }
        public string HostPosition { get; set; }
        public string HostCell { get; set; }
        public string HostTell { get; set; }
        public string HostFax { get; set; }
        public string HostEmail { get; set; }


        public class ReportDataSet
        {
            public string DatasetName { get; set; }
            public List<object> DataSetData { get; set; }
        }


        public string ReporExportFileName
        {
            get
            {
                return string.Format("attachment; filename={0}.{1}", this.ReportTitle, ReporExportExtention);
            }
        }
        public string ReporExportExtention
        {
            get
            {
                switch (this.Format)
                {
                    case ReportViewModelForBoxSearch.ReportFormat.Word: return ".doc";
                    case ReportViewModelForBoxSearch.ReportFormat.Excel: return ".xls";
                    default:
                        return ".pdf";
                }
            }
        }

        public string LastmimeType
        {
            get
            {
                return mimeType;
            }
        }
        private string mimeType;

        public byte[] RenderReport()
        {
            //geting repot data from the business object

            //creating a new report and setting its path
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = System.Web.HttpContext.Current.Server.MapPath(this.FileName);

            //adding the reort datasets with there names
            foreach (var dataset in this.ReportDataSets)
            {
                ReportDataSource reportDataSource = new ReportDataSource(dataset.DatasetName, dataset.DataSetData);
                localReport.DataSources.Add(reportDataSource);
            }
            //enabeling external images
            localReport.EnableExternalImages = true;


            //seting the partameters for the report
            localReport.SetParameters(new ReportParameter("ClientName", this.ClientName));
            localReport.SetParameters(new ReportParameter("ClientDept", this.Department));
            // localReport.SetParameters(new ReportParameter("ContactPerson", this.ContactPerson));
            //localReport.SetParameters(new ReportParameter("Status", this.Status));
            localReport.SetParameters(new ReportParameter("ReportTitle", this.ReportTitle));
            localReport.SetParameters(new ReportParameter("ReportDate", this.ReportDate.ToShortDateString()));
            //localReport.SetParameters(new ReportParameter("Year", this.Year));

            localReport.SetParameters(new ReportParameter("HostName", this.HostName));
            localReport.SetParameters(new ReportParameter("HostAdd", this.HostAddress));



            ReportParameterInfoCollection parameters = localReport.GetParameters();
            //preparing to render the report

            string reportType = this.Format.ToString();

            string encoding;
            string fileNameExtension;

            //The DeviceInfo settings should be changed based on the reportType
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + this.Format.ToString() + "</OutputFormat>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            renderedBytes = localReport.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return renderedBytes;
        }
    }
}