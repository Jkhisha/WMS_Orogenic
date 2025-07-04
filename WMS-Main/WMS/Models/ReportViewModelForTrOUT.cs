using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;

namespace WareHouseMVC.Models
{
    public class ReportViewModelForTrOUT
    {

          public enum ReportFormat { PDF = 1, Word = 2, Excel = 3 }
          public ReportViewModelForTrOUT()
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

        #region Parameters for TransmittalOUT

        public string ClientName { get; set; }
        public string Department { get; set; }
        public string ClientDept { get; set; }
        public string ContactPerson { get; set; }
        public string Status { get; set; }
        public string HandOverByName { get; set; }
        public string HandOverByAddress { get; set; }
        public string HandOverByDate { get; set; }
        public string ReceivedByName { get; set; }
        public string ReceivedByAddress { get; set; }
        public string ReceivedByDate { get; set; }
        public string TransmittalDate { get; set; }
        public string ClientReqRef { get; set; }
        public string IssuedBy { get; set; }
        public string IssuedByPosition { get; set; }
        public string BoxNoArr { get; set; }
        public string Year { get; set; }
        public string DepartmentCode { get; set; }
        public string ProjectCode { get; set; }
        public string Qty { get; set; }
        public string Unit { get; set; }
        public string Item { get; set; }
        public string TransmittalNo { get; set; }

        public string ClientAtt { get; set; }
        public string ClientPosition { get; set; }
        public string ClientCell { get; set; }
        public string ClientTell { get; set; }
        public string ClientFax { get; set; }
        public string ClientEmail { get; set; }

        public string HostName { get; set; }
        public string HostAddress { get; set; }
        public string HostAtt { get; set; }
        public string HostPosition { get; set; }
        public string HostCell { get; set; }
        public string HostTell { get; set; }
        public string HostFax { get; set; }
        public string HostEmail { get; set; }





        #endregion


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
                    case ReportViewModelForTrOUT.ReportFormat.Word: return ".doc";
                    case ReportViewModelForTrOUT.ReportFormat.Excel: return ".xls";
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
            localReport.SetParameters(new ReportParameter("Department", this.Department));
            localReport.SetParameters(new ReportParameter("ClientDept", this.ClientDept));
            // localReport.SetParameters(new ReportParameter("ContactPerson", this.ContactPerson));
            //localReport.SetParameters(new ReportParameter("Status", this.Status));
            localReport.SetParameters(new ReportParameter("HandOverByName", this.HandOverByName));
            localReport.SetParameters(new ReportParameter("HandOverByAddress", this.HandOverByAddress));
            localReport.SetParameters(new ReportParameter("HandOverByDate", this.HandOverByDate));
            localReport.SetParameters(new ReportParameter("ReceivedByName", this.ReceivedByName));
            localReport.SetParameters(new ReportParameter("ReceivedByAddress", this.ReceivedByAddress));
            localReport.SetParameters(new ReportParameter("ReceivedByDate", this.ReceivedByDate));
            localReport.SetParameters(new ReportParameter("ReportTitle", this.ReportTitle));
            localReport.SetParameters(new ReportParameter("UserNamPrinting", this.UserNamPrinting));
            localReport.SetParameters(new ReportParameter("TransmittalDate", this.TransmittalDate));
            localReport.SetParameters(new ReportParameter("ClientReqRef", this.ClientReqRef));
            localReport.SetParameters(new ReportParameter("IssuedBy", this.IssuedBy));
            localReport.SetParameters(new ReportParameter("IssuedByPosition", this.IssuedByPosition));
            localReport.SetParameters(new ReportParameter("BoxNoArr", this.BoxNoArr));
            //localReport.SetParameters(new ReportParameter("Year", this.Year));
            localReport.SetParameters(new ReportParameter("DepartmentCode", this.DepartmentCode));
            localReport.SetParameters(new ReportParameter("ProjectCode", this.ProjectCode));
            localReport.SetParameters(new ReportParameter("Qty", this.Qty));
            localReport.SetParameters(new ReportParameter("Unit", this.Unit));
            localReport.SetParameters(new ReportParameter("Item", this.Item));
            localReport.SetParameters(new ReportParameter("TransmittalNo", this.TransmittalNo));

            localReport.SetParameters(new ReportParameter("AttClient", this.ClientAtt));
            localReport.SetParameters(new ReportParameter("PositionClient", this.ClientPosition));
            localReport.SetParameters(new ReportParameter("CellClient", this.ClientCell));
            localReport.SetParameters(new ReportParameter("TellClient", this.ClientTell));
            localReport.SetParameters(new ReportParameter("FAXClient", this.ClientFax));
            localReport.SetParameters(new ReportParameter("EmailClient", this.ClientEmail));

            localReport.SetParameters(new ReportParameter("HostName", this.HostName));
            localReport.SetParameters(new ReportParameter("HostAddress", this.HostAddress));
            localReport.SetParameters(new ReportParameter("HostAtt", this.HostAtt));
            localReport.SetParameters(new ReportParameter("HostPosition", this.HostPosition));
            localReport.SetParameters(new ReportParameter("HostCell", this.HostCell));
            localReport.SetParameters(new ReportParameter("HostTell", this.HostTell));
            localReport.SetParameters(new ReportParameter("HostFAX", this.HostFax));
            localReport.SetParameters(new ReportParameter("HostEmail", this.HostEmail));



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