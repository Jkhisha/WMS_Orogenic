using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;

namespace WareHouseMVC.Models
{
    public class ReportViewModelForBarCode
    {

        public enum ReportFormat { PDF = 1, Word = 2, Excel = 3 }
        public ReportViewModelForBarCode()
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
        public string TransmittalNo { get; set; }
        public string PrintDate { get; set; }
        public string BarCodeText { get; set; }

        


        //an helper class to store the data for each report data set
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
                    case ReportViewModelForBarCode.ReportFormat.Word: return ".doc";
                    case ReportViewModelForBarCode.ReportFormat.Excel: return ".xls";
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

           // seting the partameters for the report
            //localReport.SetParameters(new ReportParameter("ClientName", this.ClientName));
            //localReport.SetParameters(new ReportParameter("Department", this.Department));
            //// localReport.SetParameters(new ReportParameter("ContactPerson", this.ContactPerson));
            //localReport.SetParameters(new ReportParameter("Status", this.Status));
            //localReport.SetParameters(new ReportParameter("HandOverByName", this.HandOverByName));
            //localReport.SetParameters(new ReportParameter("HandOverByAddress", this.HandOverByAddress));
            //localReport.SetParameters(new ReportParameter("HandOverByDate", this.HandOverByDate));
            //localReport.SetParameters(new ReportParameter("ReceivedByName", this.ReceivedByName));
            //localReport.SetParameters(new ReportParameter("ReceivedByAddress", this.ReceivedByAddress));
            //localReport.SetParameters(new ReportParameter("ReceivedByDate", this.ReceivedByDate));
            localReport.SetParameters(new ReportParameter("ReportTitle", this.ReportTitle));
            localReport.SetParameters(new ReportParameter("UserNamPrinting", this.UserNamPrinting));
            localReport.SetParameters(new ReportParameter("TransmittalNo", this.TransmittalNo));
            localReport.SetParameters(new ReportParameter("PrintDate", this.PrintDate));
            //localReport.SetParameters(new ReportParameter("BoxNoArr", this.BoxNoArr));
            //localReport.SetParameters(new ReportParameter("Year", this.Year));
            //localReport.SetParameters(new ReportParameter("DepartmentCode", this.DepartmentCode));
            //localReport.SetParameters(new ReportParameter("ProjectCode", this.ProjectCode));
            //localReport.SetParameters(new ReportParameter("Qty", this.Qty));
            //localReport.SetParameters(new ReportParameter("Unit", this.Unit));
            //localReport.SetParameters(new ReportParameter("Item", this.Item));
            //localReport.SetParameters(new ReportParameter("TransmittalNo", this.TransmittalNo));



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