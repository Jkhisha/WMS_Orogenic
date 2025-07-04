using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;

namespace WareHouseMVC.Controllers
{
    public class UploadChangeLcationXLXController : Controller
    {
        UnitOfWork repo = new UnitOfWork();
        //
        // GET: /UploadChangeLcationXLX/

        public ActionResult Index()
        {
            return View();
        }
        private List<AssignBox> Import_To_Grid(string FilePath, string Extension, long wID)
        {

            List<AssignBox> list = new List<AssignBox>();

            string conStr = "";

            switch (Extension)
            {

                case ".xls": //Excel 97-03

                    conStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FilePath + ";Extended Properties=Excel 8.0;Persist Security Info=False";

                    break;

                case ".xlsx": //Excel 07

                    conStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath + ";Extended Properties=Excel 12.0;Persist Security Info=False";

                    break;

            }





            conStr = String.Format(conStr, FilePath);

            OleDbConnection connExcel = new OleDbConnection(conStr);


            OleDbCommand cmdExcel = new OleDbCommand();

            OleDbDataAdapter oda = new OleDbDataAdapter();

            DataTable dt = new DataTable();

            cmdExcel.Connection = connExcel;



            //Get the name of First Sheet

            connExcel.Open();

            DataTable dtExcelSchema;

            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();





            //Read Data from First Sheet

            //connExcel.Open();

            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";

            oda.SelectCommand = cmdExcel;

            oda.Fill(dt);
            if (dt.Rows.Count <= 0)
            {
                SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";

                oda.SelectCommand = cmdExcel;

                oda.Fill(dt);
            }
            //connExcel.Close();
            connExcel.Close();



            try
            {
                list = SaveData(dt, wID);

            }
            catch (Exception ex)
            {

                // ShowSuccessResult("invalid file formate");
            }

            return list;
        }

        private List<AssignBox> SaveData(DataTable dt, long _wID)
        {
            List<AssignBox> list = new List<AssignBox>();
            foreach (DataRow dr in dt.Rows)
            {
                #region Get All Values From XL
                string BarcodeText = dr["Barcode Text"].ToString();

                //  long AssignBoxId = Convert.ToInt64(BarcodeText) / 5000;



                long itemId = Convert.ToInt64(BarcodeText) / 5000; //TODO
                AssignBox _assignBox = new AssignBox();

                List<AssignBox> aBoxList = new List<AssignBox>();

                aBoxList = repo.AssignBoxRepository.CheckForBarcodeSingle(itemId);//.AssignBoxes.Where(a => a.ItemId == _itemID)

                long aId = aBoxList[0].AssignBoxId;



                _assignBox = repo.AssignBoxRepository.GetByWidandTrStatus(aId, _wID, Convert.ToInt64(EnumHelper.Status.WareHouse_Assigned));

                if (_assignBox != null)
                    list.Add(_assignBox);
                #endregion
            }

            return list;
        }

    }
}
