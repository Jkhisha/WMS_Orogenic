using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class BoxOutStatementViewModelRepository : IBoxOutStatementViewModelRepository
    {
          WareHouseMVCContext context;

        public BoxOutStatementViewModelRepository()
            : this(new WareHouseMVCContext())
        {

        }
        public BoxOutStatementViewModelRepository(WareHouseMVCContext context)
        {

            this.context = context;
        }

        public List<BoxOutStatementViewModel> AllBoxOUTList()
        {
            //this is used only to help in adding the dataset of type employee to the report definition
            return null;
        }


        public void Dispose()
        {
            context.Dispose();
        }


        public ReportViewModelForBoxOutStatement GetBoxOUTReport(List<BoxOutStatementViewModel> boxOUTList, long clientID, DateTime month, int reportType)
        {
            var _boxOUTList = new List<object>();


            foreach (var item in boxOUTList)
            {
                _boxOUTList.Add(item);

            }

            DateTime monthStart = month;
            DateTime monthEnd = month.AddMonths(1).AddDays(-1);

            var reportViewModel = new ReportViewModelForBoxOutStatement()
            {
                FileName = "~/ReportsHolder/BOXOutStatement.rdlc",
                Name = "Statistical Report",
                ReportDate = DateTime.Now.ToShortDateString(),
                ReportTitle = "BOX OUT Statement for the month of ",
                HostName = GetHostInfo(1),
                HostAddress = GetHostInfo(2),
                ClientName = GetClientName(clientID),
                Month = month.ToString("Y"),
                MonthStart = monthStart.ToString("dd-MM-yyyy"),
                MonthEnd = monthEnd.ToString("dd-MM-yyyy"),


                ReportLanguage = "en-US",

                Format = GetReportFormat(reportType),// ReportViewModelForBoxOutStatement.ReportFormat.PDF,
                // ViewAsAttachment = false,

            };
            reportViewModel.ReportDataSets.Add(new ReportViewModelForBoxOutStatement.ReportDataSet() { DataSetData = _boxOUTList.ToList(), DatasetName = "DataSet1" });


            return reportViewModel;
        }

        private ReportViewModelForBoxOutStatement.ReportFormat GetReportFormat(int reportType)
        {
            if (reportType == 1)
                return ReportViewModelForBoxOutStatement.ReportFormat.PDF;
            if (reportType == 2)
                return ReportViewModelForBoxOutStatement.ReportFormat.Excel;
            if (reportType == 3)
                return ReportViewModelForBoxOutStatement.ReportFormat.Word;

            return ReportViewModelForBoxOutStatement.ReportFormat.PDF;
        }

        private string GetClientName(long clientID)
        {
            string _temp = "None";

            Client _client = new Client();

            _client = context.Clients.Where(c => c.ClientID == clientID).FirstOrDefault();
            if (_client != null)
            {
                _temp = _client.ClientName;
            }

            return _temp;
        }

        private string GetHostInfo(int flag)
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

    public interface IBoxOutStatementViewModelRepository : IDisposable
    {
    }
}