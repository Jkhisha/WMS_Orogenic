using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class BoxINStatementViewModelRepository : IBoxINStatementViewModelRepository
    {
         WareHouseMVCContext context;

        public BoxINStatementViewModelRepository()
            : this(new WareHouseMVCContext())
        {

        }
        public BoxINStatementViewModelRepository(WareHouseMVCContext context)
        {

            this.context = context;
        }


        public List<BoxInStatementViewModel> AllBoxINList()
        {
            //this is used only to help in adding the dataset of type employee to the report definition
            return null;
        }


        public void Dispose()
        {
            context.Dispose();
        }


        public  ReportViewModelForBoxINStatement GetBoxINReport(List<BoxInStatementViewModel> boxINList, long clientID, DateTime month,int reportType)
        {

            var _boxINList = new List<object>();


            foreach (var item in boxINList)
            {
                _boxINList.Add(item);

            }

            DateTime monthStart = month;
            DateTime monthEnd = month.AddMonths(1).AddDays(-1);


            var reportViewModel = new ReportViewModelForBoxINStatement()
            {
                FileName = "~/ReportsHolder/BoxINStatement.rdlc",
                Name = "Statistical Report",
                ReportDate = DateTime.Now.ToShortDateString(),
                ReportTitle = "BOX IN Statement for the month of ",
                HostName = GetHostInfo(1),
                HostAddress = GetHostInfo(2),
                ClientName = GetClientName(clientID),
                Month = month.ToString("Y"),
                MonthStart = monthStart.ToString("dd-MM-yyyy"),
                MonthEnd = monthEnd.ToString("dd-MM-yyyy"),


                ReportLanguage = "en-US",

                Format = GetReportFormat(reportType),
                //ReportViewModelForBoxINStatement.ReportFormat.PDF,
                // ViewAsAttachment = false,

            };
            reportViewModel.ReportDataSets.Add(new ReportViewModelForBoxINStatement.ReportDataSet() { DataSetData = _boxINList.ToList(), DatasetName = "DataSet1" });


            return reportViewModel;



        }

        private ReportViewModelForBoxINStatement.ReportFormat GetReportFormat(int reportType)
        {
            if (reportType == 1)
                return ReportViewModelForBoxINStatement.ReportFormat.PDF;
            if (reportType == 2)
                return ReportViewModelForBoxINStatement.ReportFormat.Excel;
            if (reportType == 3)
                return ReportViewModelForBoxINStatement.ReportFormat.Word;

            return ReportViewModelForBoxINStatement.ReportFormat.PDF;
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
    public interface IBoxINStatementViewModelRepository : IDisposable
    {
    }
}