using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;

namespace WareHouseMVC.Models
{
    public class BoxSheetRepository : IBoxSheetRepository
    {
         WareHouseMVCContext context;

         public  BoxSheetRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public BoxSheetRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }

         public void Dispose()
         {
             context.Dispose();
         }
         public List<BoxStatementSheetViewModel> BoxStatementSheetList()
         {
             //this is used only to help in adding the dataset of type employee to the report definition
             return null;
         }



         public ReportViewModelForBoxSheet GetBoxSheetReport(List<BoxStatementSheetViewModel> _boxSheet, long clientID, DateTime month, int reportType)
         {
             var _boxSheetList = new List<object>();
            

            foreach (var item in _boxSheet)
            {
                _boxSheetList.Add(item);
                
            }


             var reportViewModel = new ReportViewModelForBoxSheet()
             {
                 FileName = "~/ReportsHolder/BoxStatement.rdlc",
                 Name = "Statistical Report",
                 ReportDate = DateTime.Now.ToShortDateString(),
                 ReportTitle = "Box Statement for the month of ",
                 HostName=GetHostInfo(1),
                 HostAddress = GetHostInfo(2),
                 ClientName=GetClientName(clientID),
                 Month=month.ToString("Y"),

                 //ClientName = trIn.Client.ClientName,
                 //Department = trIn.Department.DepartmentName,
                 //DepartmentCode = trIn.Department.DepartmentCode,
                 //UserNamPrinting = UserPrinting,
                 //TransmittalNo = _transmittalNo,
                 //PrintDate = DateTime.Now.ToShortDateString(),
                 // ContactPerson = trIn.ContactPerson.ContactPersonName,
                 //HandOverByName = GetHandOverBy(trIn, 0),
                 //HandOverByAddress = GetHandOverBy(trIn, 1),
                 //HandOverByDate = GetHandOverBy(trIn, 2),
                 //// Status = GetStatus(trIn.Status),//trIn.Status.ToString(),
                 //ReceivedByName = GetReceivedBy(trIn, 0),
                 //ReceivedByAddress = GetReceivedBy(trIn, 1),
                 //ReceivedByDate = GetReceivedBy(trIn, 2),
                 //TransmittalDate = trIn.TransmittalDate.ToShortDateString(),
                 //ClientReqRef = trIn.ClientRequestreference,
                 //IssuedBy = trIn.ContactPerson.ContactPersonName,
                 //Year = year,
                 //ProjectCode = GetProjectCode(trIn),
                 //Qty = qtyCount.ToString(),
                 //Unit = "Box",
                 //Item = "Box",
                 //BoxNoArr = GetBoxs(trIn),
                 //TransmittalNo = trIn.TransmittalNo,
                 ReportLanguage = "en-US",

                 Format = GetReportType(reportType),//ReportViewModelForBoxSheet.ReportFormat.PDF,
                // ViewAsAttachment = false,

             };
             reportViewModel.ReportDataSets.Add(new ReportViewModelForBoxSheet.ReportDataSet() { DataSetData = _boxSheetList.ToList(), DatasetName = "DataSet1" });


             return reportViewModel;
         }

         private ReportViewModelForBoxSheet.ReportFormat GetReportType(int reportType)
         {
             if (reportType == 1)
                 return ReportViewModelForBoxSheet.ReportFormat.PDF;
             if (reportType == 2)
                 return ReportViewModelForBoxSheet.ReportFormat.Excel;
             if (reportType == 3)
                 return ReportViewModelForBoxSheet.ReportFormat.Word;

             return ReportViewModelForBoxSheet.ReportFormat.PDF;
         }

         private string GetClientName(long clientID)
         {
             string _temp = "None";

             Client _client = new Client();

             _client = context.Clients.Where(c=>c.ClientID==clientID).FirstOrDefault();
             if (_client != null)
             {
                 _temp = _client.ClientName;
             }

             return _temp;
         }

         private string GetHostInfo( int flag)
         {
             string _temp = "None";

             HostInformation _host = new HostInformation();

             _host = context.HostInformations.FirstOrDefault();
             if (_host != null)
             {
                 if (flag == 1)
                 {
                     _temp = _host.Name;
                 }

                 if (flag == 2)
                 {
                     _temp = _host.Address;
                 }
             }


             return _temp;
         }
    }

    public interface IBoxSheetRepository : IDisposable
    {
       
    }
}