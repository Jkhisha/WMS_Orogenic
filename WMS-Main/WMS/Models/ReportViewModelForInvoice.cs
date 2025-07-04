using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;

namespace WareHouseMVC.Models
{
    public class ReportViewModelForInvoice
    {

          public enum ReportFormat { PDF = 1, Word = 2, Excel = 3 }
          public ReportViewModelForInvoice()
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
          public string ClosingBalancePreviousMonth { get; set; }
          public string CurrentMonthTransmittalIN { get; set; }
          public string CurrentMonthTransmittalOut { get; set; }
          public string ClosingBalanceCurrentMonth { get; set; }

          public string HostName { get; set; }
          public string HostAddress { get; set; }
          public string HostAtt { get; set; }
          public string HostPosition { get; set; }
          public string HostCell { get; set; }
          public string HostTell { get; set; }
          public string HostFax { get; set; }
          public string HostEmail { get; set; }

          public string ClientName { get; set; }
          public string ClientAtt { get; set; }
          public string ClientPosition { get; set; }
          public string ClientCell { get; set; }
          public string ClientTell { get; set; }
          public string ClientFax { get; set; }
          public string ClientEmail { get; set; }


          #region Invoice Options
          public string InvoiceNo { get; set; }
          public string InvoiceDate { get; set; }
          public string InvoiceMonth { get; set; }

          public string ReservationNo { get; set; }
          public string ReservationDate { get; set; }
          public string PONo { get; set; }
          public string PODate { get; set; }
          public string VATRegNo { get; set; }
          public string GRDate { get; set; }
          public string Note { get; set; }

          public string BankName { get; set; }
          public string BankAddress { get; set; }
          public string BankAcc { get; set; }
          public string BankTel { get; set; }
          public string BranchName { get; set; }
          public string SWIFT { get; set; }



          #endregion

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
                      case ReportViewModelForInvoice.ReportFormat.Word: return ".doc";
                      case ReportViewModelForInvoice.ReportFormat.Excel: return ".xls";
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
              localReport.SetParameters(new ReportParameter("ReportTitle", this.ReportTitle));
              //localReport.SetParameters(new ReportParameter("ReportDate", this.ReportDate.ToShortDateString()));

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



              localReport.SetParameters(new ReportParameter("InvoiceNo", this.InvoiceNo));
              localReport.SetParameters(new ReportParameter("InvoiceDate", this.InvoiceDate));
              localReport.SetParameters(new ReportParameter("InvoiceMonth", this.InvoiceMonth));
              localReport.SetParameters(new ReportParameter("ReservationNo", this.ReservationNo));
              localReport.SetParameters(new ReportParameter("ReservationDate", this.ReservationDate));
              localReport.SetParameters(new ReportParameter("PONo", this.PONo));
              localReport.SetParameters(new ReportParameter("PODate", this.PODate));
              localReport.SetParameters(new ReportParameter("VATRegNo", this.VATRegNo));
              localReport.SetParameters(new ReportParameter("GRDate", this.GRDate));
              localReport.SetParameters(new ReportParameter("BankName", this.BankName));
              localReport.SetParameters(new ReportParameter("BankAddress", this.BankAddress));
              localReport.SetParameters(new ReportParameter("BankAcc", this.BankAcc));
              localReport.SetParameters(new ReportParameter("BankTel", this.BankTel));
              localReport.SetParameters(new ReportParameter("BranchName", this.BranchName));
              localReport.SetParameters(new ReportParameter("SWIFT", this.SWIFT));
              localReport.SetParameters(new ReportParameter("Note", this.Note));
              
              

              localReport.SetParameters(new ReportParameter("ClosingBalancePreviousMonth", this.ClosingBalancePreviousMonth));
              localReport.SetParameters(new ReportParameter("CurrentMonthTransmittalIN", this.CurrentMonthTransmittalIN));
              localReport.SetParameters(new ReportParameter("CurrentMonthTransmittalOut", this.CurrentMonthTransmittalOut));
              localReport.SetParameters(new ReportParameter("ClosingBalanceCurrentMonth", this.ClosingBalanceCurrentMonth));
              
              
              



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