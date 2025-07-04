using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;
using System.IO;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using System.Drawing;
using WareHouseMVC.HelperClasses;

namespace WareHouseMVC.Controllers
{
    public class XLUploadsController : BaseController
    {
        UnitOfWork repo = new UnitOfWork();
        long intCounr = 0;

        public ActionResult XLUpload()
        {
            if (TempData["Flag"] != null)
            {
                bool _flag = Convert.ToBoolean(TempData["Flag"]);

                if (_flag == true)
                    ViewBag.Flag = 1;
                if (_flag == false)
                    ViewBag.Flag = 0;
            }
            if (Session["TotalDataUploaded"] != null)
            {
                ViewBag.Count = Session["TotalDataUploaded"].ToString();
                Session["TotalDataUploaded"] = null;
            }

            return View();
        }

        [HttpPost]
        public ActionResult XLUploadwithFile(HttpPostedFileBase filename)
        {
            if (Session["TotalDataUploaded"] != null)
            {
                Session["TotalDataUploaded"] = null;
            }

            // Verify that the user selected a file
            if (filename != null && filename.ContentLength > 0)
            {
                // extract only the fielname
                string _name = Path.GetFileName(filename.FileName);
                _name = DateTime.Now.Ticks.ToString() + _name;
                //// store the file inside ~/App_Data/uploads folder
                //var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                var path = Path.Combine(Server.MapPath("~/Content/Images/") + ConfigurationManager.AppSettings["UploadedDocument"].ToString() + "XLUpload/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }


                filename.SaveAs(Path.Combine(path, _name));
                string Extension = Path.GetExtension(filename.FileName);

                path = path + _name;

                bool flag = Import_To_Grid(path, Extension);

                try
                {
                    System.IO.File.Delete(path);
                }
                catch (Exception e)
                {
                }

                TempData["Flag"] = flag;
            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("XLUpload", "XLUploads");



        }

        [HttpPost]
        public ActionResult XLUploadwithFileForFile(HttpPostedFileBase filenameFile)
        {
            if (Session["TotalDataUploaded"] != null)
            {
                Session["TotalDataUploaded"] = null;
            }
            // Verify that the user selected a file
            if (filenameFile != null && filenameFile.ContentLength > 0)
            {
                // extract only the fielname
                string _name = Path.GetFileName(filenameFile.FileName);
                _name = DateTime.Now.Ticks.ToString() + _name;
                //// store the file inside ~/App_Data/uploads folder
                //var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                var path = Path.Combine(Server.MapPath("~/Content/Images/") + ConfigurationManager.AppSettings["UploadedDocument"].ToString() + "XLUpload/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }


                filenameFile.SaveAs(Path.Combine(path, _name));
                string Extension = Path.GetExtension(filenameFile.FileName);

                path = path + _name;

                bool flag = Import_To_GridFile(path, Extension);

                try
                {
                    System.IO.File.Delete(path);
                }
                catch (Exception e)
                {
                }

                TempData["Flag"] = flag;
            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("XLUpload", "XLUploads");



        }

        [HttpPost]
        public ActionResult XLUploadwithFileForPalletBox(HttpPostedFileBase filenamePallet)
        {
            if (Session["TotalDataUploaded"] != null)
            {
                Session["TotalDataUploaded"] = null;
            }
            // Verify that the user selected a file
            if (filenamePallet != null && filenamePallet.ContentLength > 0)
            {
                // extract only the fielname
                string _name = Path.GetFileName(filenamePallet.FileName);
                _name = DateTime.Now.Ticks.ToString() + _name;
                //// store the file inside ~/App_Data/uploads folder
                //var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                var path = Path.Combine(Server.MapPath("~/Content/Images/") + ConfigurationManager.AppSettings["UploadedDocument"].ToString() + "XLUpload/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }


                filenamePallet.SaveAs(Path.Combine(path, _name));
                string Extension = Path.GetExtension(filenamePallet.FileName);

                path = path + _name;

                bool flag = Import_To_GridPallet(path, Extension);

                try
                {
                    System.IO.File.Delete(path);
                }
                catch (Exception e)
                {
                }

                TempData["Flag"] = flag;
            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("XLUpload", "XLUploads");



        }



        private bool Import_To_Grid(string FilePath, string Extension)
        {
            bool flag = false;

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
                SaveData(dt);
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                // ShowSuccessResult("invalid file formate");

                ErrorLog _error = new ErrorLog();
                _error.ErrorMsg = ex.Message;
                _error.ErrorTime = DateTime.Now;
                _error.ErrorType = "XL Upload Box";
                repo.ErrorLogRepository.InsertOrUpdate(_error);
                repo.ErrorLogRepository.Save();

            }

            return flag;

        }

        private bool Import_To_GridFile(string FilePath, string Extension)
        {
            bool flag = false;

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
                SaveDataFile(dt);
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                // ShowSuccessResult("invalid file formate");
                ErrorLog _error = new ErrorLog();
                _error.ErrorMsg = ex.Message;
                _error.ErrorTime = DateTime.Now;
                _error.ErrorType = "XL Upload File";
                repo.ErrorLogRepository.InsertOrUpdate(_error);
                repo.ErrorLogRepository.Save();
            }

            return flag;

        }

        private bool Import_To_GridPallet(string FilePath, string Extension)
        {
            bool flag = false;

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
                SaveDataPallet(dt);
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;

                ErrorLog _error = new ErrorLog();
                _error.ErrorMsg = ex.Message;
                _error.ErrorTime = DateTime.Now;
                _error.ErrorType = "XL Upload Box Pallet";
                repo.ErrorLogRepository.InsertOrUpdate(_error);
                repo.ErrorLogRepository.Save();
                // ShowSuccessResult("invalid file formate");
            }

            return flag;

        }


        private void SaveData(DataTable dt)
        {
            List<Item> itemList = new List<Item>();

            foreach (DataRow dr in dt.Rows)
            {
                intCounr++;
                #region Get All Values From XL
                string wareHouseName = dr["Ware House Name "].ToString();
                string floorName = dr["Floor Name"].ToString();
                string zoneName = dr["Zone Name"].ToString();
                string trainName = dr["Train Name"].ToString();
                string rackName = dr["Rack Name"].ToString();
                string levelName = dr["Lavel Name"].ToString();
                string heightName = dr["Height Name"].ToString();
                string columnName = dr["Column Name"].ToString();
                string rowName = dr["Row Name"].ToString();


                string transmittalNo = dr["TransmittallN No"].ToString();
                string clientName = dr["Client Name"].ToString();
                string department = dr["Department Name"].ToString();
                string project = dr["Project"].ToString();
                string contactPerson = dr["IssuedBy"].ToString();

                string clientReqRef = dr["Cliente Ref No"].ToString();
                string habdoverBy = dr["Hand Over By"].ToString();
                string receivedBy = dr["Received By"].ToString();
                string boxName = dr["Box Name"].ToString();
                string boxNo = dr["Box No"].ToString();

                if (boxNo.Length < 1)
                {
                    boxNo = boxNo.PadLeft(10, '0');
                }

                string year = dr["Year"].ToString();
                try
                {

                    DateTime destructionDate = Convert.ToDateTime(dr["Distruction Date"].ToString());
                }
                catch
                {
                    DateTime destructionDate = DateTime.Now;
                }

                string supportStuff = dr["Client Supervisor"].ToString();
                string orblDept = dr["Supervisor Department"].ToString();
                #endregion

                #region Public Variables

                int deptFlag = 0;

                long _tempWarehouseID = 0;
                long _tempFloorID = 0;
                long _tempZoneID = 0;
                long _tempTrainID = 0;
                long _tempRackID = 0;
                long _tempLevelID = 0;
                long _tempHeightID = 0;
                long _tempColumnID = 0;
                long _tempRowID = 0;
                long _tempORBLDeptID = 0;
                long _tempSupportStuffID = 0;
                long _tempClientID = 0;
                long _tempDepartmentID = 0;
                long _tempContactPersonID = 0;
                #endregion

                #region WareHouse Setup Module

                Warehouse _warehouse = repo.WarehouseRepository.GetByName(wareHouseName);
                if (_warehouse == null)
                {
                    _warehouse = new Warehouse();
                    _warehouse.WarehouseName = wareHouseName;
                    _warehouse.WarehouseCode = "";
                    repo.WarehouseRepository.InsertOrUpdate(_warehouse);
                    repo.Save();
                }
                _tempWarehouseID = _warehouse.WarehouseID;


                Floor _floor = repo.FloorRepository.GetByWidandFName(_tempWarehouseID, floorName);
                if (_floor == null)
                {
                    _floor = new Floor();
                    _floor.FloorName = floorName;
                    _floor.FloorCode = "";
                    _floor.WarehouseID = _tempWarehouseID;
                    repo.FloorRepository.InsertOrUpdate(_floor);
                    repo.FloorRepository.Save();
                }
                _tempFloorID = _floor.FloorID;


                Zone _zone = repo.ZoneRepository.GetBywIDandFIDandzoneName(_tempWarehouseID, _tempFloorID, zoneName);
                if (_zone == null)
                {
                    _zone = new Zone();
                    _zone.ZoneName = zoneName;
                    _zone.WarehouseID = _tempWarehouseID;
                    _zone.FloorID = _tempFloorID;
                    _zone.ZoneCode = "";
                    repo.ZoneRepository.InsertOrUpdate(_zone);
                    repo.ZoneRepository.Save();
                }
                _tempZoneID = _zone.ZoneID;


                Train _train = repo.TrainRepository.GetByWIDandFIDandZIDandTrainName(_tempWarehouseID, _tempFloorID, _tempZoneID, trainName);
                if (_train == null)
                {
                    _train = new Train();
                    _train.WarehouseID = _tempWarehouseID;
                    _train.FloorID = _tempFloorID;
                    _train.ZoneID = _tempZoneID;
                    _train.TrainCode = "";
                    _train.TrainName = trainName;
                    repo.TrainRepository.InsertOrUpdate(_train);
                    repo.TrainRepository.Save();
                }
                _tempTrainID = _train.TrainID;


                Rack _rack = repo.RackRepository.GetByWIDandFIDandZIDandTIDandRackName(_tempWarehouseID, _tempFloorID, _tempZoneID, _tempTrainID, rackName);
                if (_rack == null)
                {
                    _rack = new Rack();
                    _rack.RackCode = "";
                    _rack.RackName = rackName;
                    _rack.WarehouseID = _tempWarehouseID;
                    _rack.FloorID = _tempFloorID;
                    _rack.ZoneID = _tempZoneID;
                    _rack.TrainID = _tempTrainID;

                    repo.RackRepository.InsertOrUpdate(_rack);
                    repo.RackRepository.Save();
                }
                _tempRackID = _rack.RackID;


                Level _level = repo.LevelRepository.GetByWIDandFIDandZIDandTIDandRIDandLName(_tempWarehouseID, _tempFloorID, _tempZoneID, _tempTrainID, _tempRackID, levelName);
                if (_level == null)
                {
                    _level = new Level();
                    _level.LevelName = levelName;
                    _level.LevelCode = "";
                    _level.WarehouseID = _tempWarehouseID;
                    _level.FloorID = _tempFloorID;
                    _level.ZoneID = _tempZoneID;
                    _level.TrainID = _tempTrainID;
                    _level.RackID = _tempRackID;
                    repo.LevelRepository.InsertOrUpdate(_level);
                    repo.LevelRepository.Save();

                }
                _tempLevelID = _level.LevelID;


                Height _height = repo.HeightRepository.GetByLIDandHeightName(_tempLevelID, heightName);
                if (_height == null)
                {
                    _height = new Height();
                    _height.HeightName = heightName;
                    _height.HeightCode = "";
                    _height.WarehouseID = _tempWarehouseID;
                    _height.FloorID = _tempFloorID;
                    _height.ZoneID = _tempZoneID;
                    _height.TrainID = _tempTrainID;
                    _height.RackID = _tempRackID;
                    _height.LevelID = _tempLevelID;
                    repo.HeightRepository.InsertOrUpdate(_height);
                    repo.HeightRepository.Save();
                }
                _tempHeightID = _height.HeightID;


                Column _column = repo.ColumnRepository.GetByHIDandColumnName(_tempHeightID, columnName);
                if (_column == null)
                {
                    _column = new Column();
                    _column.ColumnName = columnName;
                    _column.ColumnCode = "";
                    _column.WarehouseID = _tempWarehouseID;
                    _column.FloorID = _tempFloorID;
                    _column.ZoneID = _tempZoneID;
                    _column.TrainID = _tempTrainID;
                    _column.RackID = _tempRackID;
                    _column.LevelID = _tempLevelID;
                    _column.HeightID = _tempHeightID;
                    repo.ColumnRepository.InsertOrUpdate(_column);
                    repo.ColumnRepository.Save();

                }
                _tempColumnID = _column.ColumnID;


                Row _row = repo.RowRepository.GetByCIDandRowName(_tempColumnID, rowName);
                if (_row == null)
                {
                    _row = new Row();
                    _row.RowName = rowName;
                    _row.RowCode = "";
                    _row.WarehouseID = _tempWarehouseID;
                    _row.FloorID = _tempFloorID;
                    _row.ZoneID = _tempZoneID;
                    _row.TrainID = _tempTrainID;
                    _row.RackID = _tempRackID;
                    _row.LevelID = _tempLevelID;
                    _row.HeightID = _tempHeightID;
                    _row.ColumnID = _tempColumnID;
                    repo.RowRepository.InsertOrUpdate(_row);
                    repo.RowRepository.Save();
                }
                else
                {
                    if (_row.IsAssigned == true)
                    {
                        _row = new Row();
                        _row.RowName = rowName;
                        _row.RowCode = "";
                        _row.WarehouseID = _tempWarehouseID;
                        _row.FloorID = _tempFloorID;
                        _row.ZoneID = _tempZoneID;
                        _row.TrainID = _tempTrainID;
                        _row.RackID = _tempRackID;
                        _row.LevelID = _tempLevelID;
                        _row.HeightID = _tempHeightID;
                        _row.ColumnID = _tempColumnID;
                        repo.RowRepository.InsertOrUpdate(_row);
                        repo.RowRepository.Save();
                    }

                }
                _tempRowID = _row.RowID;

                #endregion

                #region Client Setup Module

                #region ORBL Depts
                ORBLDepartment _orblDept = repo.ORBLDepartmentRepository.GetByName(orblDept);

                if (_orblDept == null)
                {
                    _orblDept = new ORBLDepartment();
                    _orblDept.ORBLDepartmentCode = "";
                    _orblDept.ORBLDepartmentName = orblDept;
                    repo.ORBLDepartmentRepository.InsertOrUpdate(_orblDept);
                    repo.ORBLDepartmentRepository.Save();
                }
                _tempORBLDeptID = _orblDept.ORBLDepartmentID;

                #endregion
                #region ORBLSupportStuff
                SupportStuff _supportStuff = repo.SupportStuffRepository.GetByNameandDeptID(supportStuff, _tempORBLDeptID);

                if (_supportStuff == null)
                {
                    _supportStuff = new SupportStuff();
                    _supportStuff.SupportStuffName = supportStuff;
                    _supportStuff.SupportStuffMobileNo = "01600000000";
                    _supportStuff.ORBLDepartmentID = _tempORBLDeptID;
                    _supportStuff.SupportStuffCode = "";
                    repo.SupportStuffRepository.InsertOrUpdate(_supportStuff);
                    repo.SupportStuffRepository.Save();
                }
                _tempSupportStuffID = _supportStuff.SupportStuffID;


                #endregion
                #region Project Setup

                Project _project = repo.ProjectRepository.GetByCIDandDeptIDandProjectName(_tempClientID, _tempDepartmentID, project);


                #endregion

                #region Client Setup

                Client _client = repo.ClientRepository.GetByClientName(clientName);

                if (_client == null)
                {
                    _client = new Client();
                    _client.ClientName = clientName;
                    _client.ClientAddress = "Initial Address";
                    _client.SupportStuffID = _tempSupportStuffID;
                    repo.ClientRepository.InsertOrUpdate(_client);
                    repo.ClientRepository.Save();
                }
                _tempClientID = _client.ClientID;

                Department _department = repo.DepartmentRepository.GetByClientIDandDeptName(_tempClientID, department);

                if (_department == null)
                {
                    deptFlag = 1;
                    _department = new Department();
                    _department.DepartmentName = department;
                    _department.DepartmentCode = "";
                    _department.DepartmentAddress = "";
                    _department.ClientID = _tempClientID;
                    repo.DepartmentRepository.InsertOrUpdate(_department);
                    repo.DepartmentRepository.Save();
                }
                _tempDepartmentID = _department.DepartmentID;

                ContactPerson _contactPerson = repo.ContactPersonRepository.GetByCIDandDeptIDandContactName(_tempClientID, _tempDepartmentID, contactPerson);

                if (_contactPerson == null)
                {
                    _contactPerson = new ContactPerson();
                    _contactPerson.ContactPersonName = contactPerson;
                    _contactPerson.DepartmentID = _tempDepartmentID;
                    _contactPerson.ClientID = _tempClientID;
                    _contactPerson.Address = "";
                    _contactPerson.Email = "";
                    _contactPerson.PhoneNumber = "01600000000";
                    repo.ContactPersonRepository.InsertOrUpdate(_contactPerson);
                    repo.ContactPersonRepository.Save();
                }
                _tempContactPersonID = _contactPerson.ContactPersontID;

                #endregion

                #endregion
                //Make a common TransmittalIN
                TransmittalIN trIn = new TransmittalIN();
                string trNo = "ORBL-2014-Demo";
                trIn = repo.TransmittalINRepository.GetByName(trNo);
                if (trIn == null)
                {
                    trIn = new TransmittalIN();



                    trIn.ClientID = _tempClientID;
                    trIn.ContactPersontID = _tempContactPersonID;
                    trIn.CreateDate = DateTime.Now;
                    trIn.DepartmentID = _tempDepartmentID;
                    trIn.IsFile = false;
                    trIn.TotalArchieveItem = 0;
                    trIn.TransmittalDate = DateTime.Now;
                    trIn.TransmittalNo = trNo;
                    trIn.Type = "Box";
                    trIn.TransmittalINStatusId = 5;


                    ORBLOperator _operator = repo.ORBLOperatorRepository.GetByName("Saiful");

                    //trout.ORBLOperatorId = handoverby.ORBLOperatorId;
                    //trout.OrblOperatorAddress = handoverby.Address;
                    List<ORBLOperator> operatorList = new List<ORBLOperator>();
                    operatorList.Add(_operator);

                    trIn.ReceivedBy = operatorList;

                    repo.TransmittalINRepository.InsertOrUpdate(trIn);
                    repo.TransmittalINRepository.Save();

                    List<TransmittalIN> trINList = new List<TransmittalIN>();
                    trINList = repo.TransmittalINRepository.GetTrINList(trIn.TransmittalINId);
                    HandOverBy _handOver = new HandOverBy();
                    _handOver.Name = habdoverBy;
                    _handOver.Address = "initial address";
                    _handOver.Date = DateTime.Now;
                    _handOver.TransmittalINs = trINList;


                    repo.HandOverByRepository.InsertOrUpdate(_handOver);
                    repo.HandOverByRepository.Save();
                }

                Item box = new Item();
                box.BoxNo = boxNo;
                box.ClientID = _tempClientID;
                box.Comments = "nothing";
                box.DepartmentID = _tempDepartmentID;
                box.IsArchieve = false;
                box.ItemName = boxName;
                box.Quantity = "1";
                box.Unit = "Box";
                box.Year = year;
                repo.ItemRepository.InsertOrUpdate(box);
                repo.ItemRepository.Save();

                itemList.Add(box);

                trIn.Items = itemList;
                repo.TransmittalINRepository.InsertOrUpdate(trIn);
                repo.TransmittalINRepository.Save();

                AssignBox assignBox = new AssignBox();

                assignBox.AssignDate = DateTime.Now;
                assignBox.BoxName = box.ItemName;
                assignBox.BoxNo = box.BoxNo;
                assignBox.CurrentStatus = 1;
                assignBox.ItemId = box.ItemId;
                assignBox.Year = box.Year;
                assignBox.TransmittalINId = trIn.TransmittalINId;
                assignBox.TransmittalINStatusId = 5;
                assignBox.WarehouseID = _tempWarehouseID;
                assignBox.WarehouseName = _warehouse.WarehouseName;
                repo.AssignBoxRepository.InsertOrUpdate(assignBox);
                repo.AssignBoxRepository.Save();

                string _boxNo = assignBox.BoxNo;
                char[] arr = new char[10] { 'a', 'b', 'c', 'd', 'e', 'A', 'B', 'C', 'D', 'E' };
                if (assignBox.BoxNo.Contains('(') && assignBox.BoxNo.Contains(')'))
                {
                    _boxNo = assignBox.BoxNo.Replace('(', '-').Replace(')', '-');
                }

                for (int i = 0; i < arr.Length; i++)
                {
                    if (assignBox.BoxNo.Contains(arr[i]))
                    {
                        _boxNo = assignBox.BoxNo.Replace(arr[i], '-');
                    }
                }
                //var barcode = new Code39BarCode()
                //{



                //    BarCodeText = GetBarcodeText(assignBox.AssignBoxId),
                //    BarCodeWeight = BarCodeWeight.Small,
                //    Height = 80,

                //};

                string BarCodeText = GetBarcodeText(assignBox.AssignBoxId);
                Image myimg = Code128Rendering.MakeBarcodeImage(BarCodeText, 2, false);
                byte[] fileContents = imageToByteArray(myimg);


                string _relativePath = "~/Content/Images/" + ConfigurationManager.AppSettings["BarCodes"].ToString() + "AssignBox-" + assignBox.AssignBoxId.ToString();
                var path = Server.MapPath("~/Content/Images/" + ConfigurationManager.AppSettings["BarCodes"].ToString() + "AssignBox-" + assignBox.AssignBoxId.ToString());
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //Image returnImage = byteArrayToImage(fileContents);

                string filePath = path + ".Bmp";

                System.IO.File.WriteAllBytes(filePath, fileContents);

                #region BarCode Insert or Update

                BarCode _existingBarCode = repo.BarCodeRepository.GetByAssignBoxId(assignBox.AssignBoxId);

                if (_existingBarCode == null)
                {
                    _existingBarCode = new BarCode();


                }
                _existingBarCode.AssignBoxId = assignBox.AssignBoxId;
                _existingBarCode.ImagePathRel = _relativePath + ".Bmp";
                _existingBarCode.ImagePathAbs = "file://" + filePath;
                _existingBarCode.BoxNo = assignBox.BoxNo;


                string _reportField = string.Empty;


                _existingBarCode.Year = assignBox.Year;
                _existingBarCode.BoxName = assignBox.BoxName;
                _existingBarCode.ClientName = assignBox.Item.Client.ClientName;
                _existingBarCode.DeptName = assignBox.Item.Department.DepartmentName;

                _reportField = "Box Name-" + _existingBarCode.BoxName + System.Environment.NewLine;
                _reportField = _reportField + "Box No-" + _existingBarCode.BoxNo + System.Environment.NewLine;
                _reportField = _reportField + "Client-" + _existingBarCode.ClientName + System.Environment.NewLine + "Department-" + _existingBarCode.DeptName + System.Environment.NewLine;

                _existingBarCode.ReportFiled = _reportField;



                repo.BarCodeRepository.InsertOrUpdate(_existingBarCode);
                repo.BarCodeRepository.Save();

                #endregion
                AssignBox _assignBox = repo.AssignBoxRepository.Find(assignBox.AssignBoxId);

                _assignBox.BarCodeId = _existingBarCode.BarCodeId;
                // _assignBox.TransmittalINStatusId = repo.TransmittalINStatusRepository.Find(Convert.ToInt64(EnumHelper.Status.Barcode_Assigned)).TransmittalINStatusId;

                repo.AssignBoxRepository.InsertOrUpdate(_assignBox);
                repo.AssignBoxRepository.Save();




                BoxLocation location = new BoxLocation();
                location.AssignBoxId = assignBox.AssignBoxId;
                location.ColumnID = _tempColumnID;
                location.CurrentStatus = "In WareHouse";
                location.FloorID = _tempFloorID;
                location.HeightID = _tempHeightID;
                location.IsPallet = false;
                location.LevelID = _tempLevelID;
                location.RackID = _tempRackID;
                location.RowID = _tempRowID;
                location.TrainID = _tempTrainID;
                location.WareHouseID = _tempWarehouseID;
                location.ZoneID = _tempZoneID;


                repo.BoxLocationRepository.InsertOrUpdate(location);
                repo.BoxLocationRepository.Save();

                Row newROw = repo.RowRepository.Find(_tempRowID);
                newROw.IsAssigned = true;
                repo.RowRepository.InsertOrUpdate(newROw);
                repo.RowRepository.Save();


                List<BoxLocation> _boxList = new List<BoxLocation>();
                _boxList.Add(location);

                assignBox.BoxLocation = _boxList;
                repo.AssignBoxRepository.InsertOrUpdate(assignBox);
                repo.AssignBoxRepository.Save();


            }

            Session["TotalDataUploaded"] = intCounr;
        }

        private byte[] imageToByteArray(Image myimg)
        {
            using (var ms = new MemoryStream())
            {
                myimg.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
        }

        private string GetBarcodeText(long assignBoxId)
        {
            return (assignBoxId * 5000).ToString();
        }

        private void SaveDataFile(DataTable dt)
        {
            List<Item> itemList = new List<Item>();
            long intcount = 0;

            foreach (DataRow dr in dt.Rows)
            {
                intcount++;
                #region Get All Values From XL
                string wareHouseName = dr["Ware House Name "].ToString();
                string floorName = dr["Floor Name"].ToString();
                string zoneName = dr["Zone Name"].ToString();
                string trainName = dr["Train Name"].ToString();
                string rackName = dr["Rack Name"].ToString();
                string levelName = dr["Lavel Name"].ToString();
                string heightName = dr["Height Name"].ToString();
                string columnName = dr["Column Name"].ToString();
                string rowName = dr["Row Name"].ToString();


                string clientName = dr["Client Name"].ToString();
                string department = dr["Department Name"].ToString();
                string project = dr["Project"].ToString();
                string contactPerson = dr["IssuedBy"].ToString();

                string transmittalNo = dr["TransmittallN No"].ToString();
                string clientReqRef = dr["Cliente Ref No"].ToString();
                string habdoverBy = dr["Hand Over By"].ToString();
                string receivedBy = dr["Received By"].ToString();

                string boxNo = dr["Box No"].ToString();

                if (boxNo.Length < 1)
                {
                    boxNo = boxNo.PadLeft(10, '0');
                }


                string bookNo = dr["BoxName"].ToString();
                string fileNo = dr["File No"].ToString();
                string refNo = dr["Referrence No"].ToString();
                string ringNo = dr["Ring No"].ToString();
                string accNo = dr["Account No"].ToString();
                string year = dr["Year"].ToString();
                try
                {

                    DateTime destructionDate = Convert.ToDateTime(dr["Distruction Date"].ToString());
                }
                catch
                {
                    DateTime destructionDate = DateTime.Now;
                }

                string supportStuff = dr["Client Supervisor"].ToString();
                string orblDept = dr["Supervisor Department"].ToString();
                #endregion

                #region Public Variables

                int deptFlag = 0;

                long _tempWarehouseID = 0;
                long _tempFloorID = 0;
                long _tempZoneID = 0;
                long _tempTrainID = 0;
                long _tempRackID = 0;
                long _tempLevelID = 0;
                long _tempHeightID = 0;
                long _tempColumnID = 0;
                long _tempRowID = 0;
                long _tempORBLDeptID = 0;
                long _tempSupportStuffID = 0;
                long _tempClientID = 0;
                long _tempDepartmentID = 0;
                long _tempContactPersonID = 0;
                #endregion

                #region WareHouse Setup Module

                Warehouse _warehouse = repo.WarehouseRepository.GetByName(wareHouseName);
                if (_warehouse == null)
                {
                    _warehouse = new Warehouse();
                    _warehouse.WarehouseName = wareHouseName;
                    _warehouse.WarehouseCode = "";
                    repo.WarehouseRepository.InsertOrUpdate(_warehouse);
                    repo.Save();
                }
                _tempWarehouseID = _warehouse.WarehouseID;


                Floor _floor = repo.FloorRepository.GetByWidandFName(_tempWarehouseID, floorName);
                if (_floor == null)
                {
                    _floor = new Floor();
                    _floor.FloorName = floorName;
                    _floor.FloorCode = "";
                    _floor.WarehouseID = _tempWarehouseID;
                    repo.FloorRepository.InsertOrUpdate(_floor);
                    repo.FloorRepository.Save();
                }
                _tempFloorID = _floor.FloorID;


                Zone _zone = repo.ZoneRepository.GetBywIDandFIDandzoneName(_tempWarehouseID, _tempFloorID, zoneName);
                if (_zone == null)
                {
                    _zone = new Zone();
                    _zone.ZoneName = zoneName;
                    _zone.WarehouseID = _tempWarehouseID;
                    _zone.FloorID = _tempFloorID;
                    _zone.ZoneCode = "";
                    repo.ZoneRepository.InsertOrUpdate(_zone);
                    repo.ZoneRepository.Save();
                }
                _tempZoneID = _zone.ZoneID;


                Train _train = repo.TrainRepository.GetByWIDandFIDandZIDandTrainName(_tempWarehouseID, _tempFloorID, _tempZoneID, trainName);
                if (_train == null)
                {
                    _train = new Train();
                    _train.WarehouseID = _tempWarehouseID;
                    _train.FloorID = _tempFloorID;
                    _train.ZoneID = _tempZoneID;
                    _train.TrainCode = "";
                    _train.TrainName = trainName;
                    repo.TrainRepository.InsertOrUpdate(_train);
                    repo.TrainRepository.Save();
                }
                _tempTrainID = _train.TrainID;


                Rack _rack = repo.RackRepository.GetByWIDandFIDandZIDandTIDandRackName(_tempWarehouseID, _tempFloorID, _tempZoneID, _tempTrainID, rackName);
                if (_rack == null)
                {
                    _rack = new Rack();
                    _rack.RackCode = "";
                    _rack.RackName = rackName;
                    _rack.WarehouseID = _tempWarehouseID;
                    _rack.FloorID = _tempFloorID;
                    _rack.ZoneID = _tempZoneID;
                    _rack.TrainID = _tempTrainID;

                    repo.RackRepository.InsertOrUpdate(_rack);
                    repo.RackRepository.Save();
                }
                _tempRackID = _rack.RackID;


                Level _level = repo.LevelRepository.GetByWIDandFIDandZIDandTIDandRIDandLName(_tempWarehouseID, _tempFloorID, _tempZoneID, _tempTrainID, _tempRackID, levelName);
                if (_level == null)
                {
                    _level = new Level();
                    _level.LevelName = levelName;
                    _level.LevelCode = "";
                    _level.WarehouseID = _tempWarehouseID;
                    _level.FloorID = _tempFloorID;
                    _level.ZoneID = _tempZoneID;
                    _level.TrainID = _tempTrainID;
                    _level.RackID = _tempRackID;
                    repo.LevelRepository.InsertOrUpdate(_level);
                    repo.LevelRepository.Save();

                }
                _tempLevelID = _level.LevelID;


                Height _height = repo.HeightRepository.GetByLIDandHeightName(_tempLevelID, heightName);
                if (_height == null)
                {
                    _height = new Height();
                    _height.HeightName = heightName;
                    _height.HeightCode = "";
                    _height.WarehouseID = _tempWarehouseID;
                    _height.FloorID = _tempFloorID;
                    _height.ZoneID = _tempZoneID;
                    _height.TrainID = _tempTrainID;
                    _height.RackID = _tempRackID;
                    _height.LevelID = _tempLevelID;
                    repo.HeightRepository.InsertOrUpdate(_height);
                    repo.HeightRepository.Save();
                }
                _tempHeightID = _height.HeightID;


                Column _column = repo.ColumnRepository.GetByHIDandColumnName(_tempHeightID, columnName);
                if (_column == null)
                {
                    _column = new Column();
                    _column.ColumnName = columnName;
                    _column.ColumnCode = "";
                    _column.WarehouseID = _tempWarehouseID;
                    _column.FloorID = _tempFloorID;
                    _column.ZoneID = _tempZoneID;
                    _column.TrainID = _tempTrainID;
                    _column.RackID = _tempRackID;
                    _column.LevelID = _tempLevelID;
                    _column.HeightID = _tempHeightID;
                    repo.ColumnRepository.InsertOrUpdate(_column);
                    repo.ColumnRepository.Save();

                }
                _tempColumnID = _column.ColumnID;


                Row _row = repo.RowRepository.GetByCIDandRowName(_tempColumnID, rowName);
                if (_row == null)
                {
                    _row = new Row();
                    _row.RowName = rowName;
                    _row.RowCode = "";
                    _row.WarehouseID = _tempWarehouseID;
                    _row.FloorID = _tempFloorID;
                    _row.ZoneID = _tempZoneID;
                    _row.TrainID = _tempTrainID;
                    _row.RackID = _tempRackID;
                    _row.LevelID = _tempLevelID;
                    _row.HeightID = _tempHeightID;
                    _row.ColumnID = _tempColumnID;
                    repo.RowRepository.InsertOrUpdate(_row);
                    repo.RowRepository.Save();
                }
                _tempRowID = _row.RowID;

                #endregion

                #region Client Setup Module

                #region ORBL Depts
                ORBLDepartment _orblDept = repo.ORBLDepartmentRepository.GetByName(orblDept);

                if (_orblDept == null)
                {
                    _orblDept = new ORBLDepartment();
                    _orblDept.ORBLDepartmentCode = "";
                    _orblDept.ORBLDepartmentName = orblDept;
                    repo.ORBLDepartmentRepository.InsertOrUpdate(_orblDept);
                    repo.ORBLDepartmentRepository.Save();
                }
                _tempORBLDeptID = _orblDept.ORBLDepartmentID;

                #endregion
                #region ORBLSupportStuff
                SupportStuff _supportStuff = repo.SupportStuffRepository.GetByNameandDeptID(supportStuff, _tempORBLDeptID);

                if (_supportStuff == null)
                {
                    _supportStuff = new SupportStuff();
                    _supportStuff.SupportStuffName = supportStuff;
                    _supportStuff.SupportStuffMobileNo = "01600000000";
                    _supportStuff.ORBLDepartmentID = _tempORBLDeptID;
                    _supportStuff.SupportStuffCode = "";
                    repo.SupportStuffRepository.InsertOrUpdate(_supportStuff);
                    repo.SupportStuffRepository.Save();
                }
                _tempSupportStuffID = _supportStuff.SupportStuffID;


                #endregion
                #region Project Setup

                Project _project = repo.ProjectRepository.GetByCIDandDeptIDandProjectName(_tempClientID, _tempDepartmentID, project);


                #endregion

                #region Client Setup

                Client _client = repo.ClientRepository.GetByClientName(clientName);

                if (_client == null)
                {
                    _client = new Client();
                    _client.ClientName = clientName;
                    _client.ClientAddress = "Initial Address";
                    _client.SupportStuffID = _tempSupportStuffID;
                    repo.ClientRepository.InsertOrUpdate(_client);
                    repo.ClientRepository.Save();
                }
                _tempClientID = _client.ClientID;

                Department _department = repo.DepartmentRepository.GetByClientIDandDeptName(_tempClientID, department);

                if (_department == null)
                {
                    deptFlag = 1;
                    _department = new Department();
                    _department.DepartmentName = department;
                    _department.DepartmentCode = "";
                    _department.DepartmentAddress = "";
                    _department.ClientID = _tempClientID;
                    repo.DepartmentRepository.InsertOrUpdate(_department);
                    repo.DepartmentRepository.Save();
                }
                _tempDepartmentID = _department.DepartmentID;

                ContactPerson _contactPerson = repo.ContactPersonRepository.GetByCIDandDeptIDandContactName(_tempClientID, _tempDepartmentID, contactPerson);

                if (_contactPerson == null)
                {
                    _contactPerson = new ContactPerson();
                    _contactPerson.ContactPersonName = contactPerson;
                    _contactPerson.DepartmentID = _tempDepartmentID;
                    _contactPerson.ClientID = _tempClientID;
                    _contactPerson.Address = "";
                    _contactPerson.Email = "";
                    _contactPerson.PhoneNumber = "01600000000";
                    repo.ContactPersonRepository.InsertOrUpdate(_contactPerson);
                    repo.ContactPersonRepository.Save();
                }
                _tempContactPersonID = _contactPerson.ContactPersontID;

                #endregion

                #endregion
                //Make a common TransmittalIN
                TransmittalIN trIn = new TransmittalIN();
                string trNo = "ORBL-2014-Demo.File";
                trIn = repo.TransmittalINRepository.GetByName(trNo);
                if (trIn == null)
                {
                    trIn = new TransmittalIN();



                    trIn.ClientID = _tempClientID;
                    trIn.ContactPersontID = _tempContactPersonID;
                    trIn.CreateDate = DateTime.Now;
                    trIn.DepartmentID = _tempDepartmentID;
                    trIn.IsFile = true;
                    trIn.TotalArchieveItem = 0;
                    trIn.TransmittalDate = DateTime.Now;
                    trIn.TransmittalNo = trNo;
                    trIn.Type = "File";
                    trIn.TransmittalINStatusId = 5;


                    ORBLOperator _operator = repo.ORBLOperatorRepository.GetByName("Saiful");

                    //trout.ORBLOperatorId = handoverby.ORBLOperatorId;
                    //trout.OrblOperatorAddress = handoverby.Address;
                    List<ORBLOperator> operatorList = new List<ORBLOperator>();
                    operatorList.Add(_operator);

                    trIn.ReceivedBy = operatorList;

                    repo.TransmittalINRepository.InsertOrUpdate(trIn);
                    repo.TransmittalINRepository.Save();

                    List<TransmittalIN> trINList = new List<TransmittalIN>();
                    trINList = repo.TransmittalINRepository.GetTrINList(trIn.TransmittalINId);
                    HandOverBy _handOver = new HandOverBy();
                    _handOver.Name = habdoverBy;
                    _handOver.Address = "initial address";
                    _handOver.Date = DateTime.Now;
                    _handOver.TransmittalINs = trINList;


                    repo.HandOverByRepository.InsertOrUpdate(_handOver);
                    repo.HandOverByRepository.Save();
                }

                Item box = new Item();
                box.BoxNo = boxNo;
                box.ClientID = _tempClientID;
                box.Comments = "nothing";
                box.DepartmentID = _tempDepartmentID;
                box.IsArchieve = false;
                box.FileBoxName = bookNo;
                box.FileNumber = fileNo;
                box.ReferrenceNo = refNo;
                box.RingNo = ringNo;
                box.AccountNo = accNo;
                box.Quantity = "1";
                box.Year = year;
                box.Unit = "File";
                repo.ItemRepository.InsertOrUpdate(box);
                repo.ItemRepository.Save();

                itemList.Add(box);

                trIn.Items = itemList;
                repo.TransmittalINRepository.InsertOrUpdate(trIn);
                repo.TransmittalINRepository.Save();

                AssignBox assignBox = new AssignBox();

                assignBox.AssignDate = DateTime.Now;




                assignBox.BoxNo = box.BoxNo;

                assignBox.BoxNameFile = box.FileBoxName;
                assignBox.FileNumber = box.FileNumber;
                assignBox.ReferrenceNo = box.ReferrenceNo;
                assignBox.RingNo = box.RingNo;
                assignBox.AccountNo = box.AccountNo;


                assignBox.CurrentStatus = 1;
                assignBox.ItemId = box.ItemId;
                assignBox.TransmittalINId = trIn.TransmittalINId;
                assignBox.TransmittalINStatusId = 5;
                assignBox.WarehouseID = _tempWarehouseID;
                assignBox.Year = box.Year;
                assignBox.WarehouseName = _warehouse.WarehouseName;
                repo.AssignBoxRepository.InsertOrUpdate(assignBox);
                repo.AssignBoxRepository.Save();

                string _boxNo = assignBox.BoxNo;
                char[] arr = new char[5] { 'a', 'b', 'c', 'd', 'e' };
                if (assignBox.BoxNo.Contains('(') && assignBox.BoxNo.Contains(')'))
                {
                    _boxNo = assignBox.BoxNo.Replace('(', '-').Replace(')', '-');
                }

                for (int i = 0; i < arr.Length; i++)
                {
                    if (assignBox.BoxNo.Contains(arr[i]))
                    {
                        _boxNo = assignBox.BoxNo.Replace(arr[i], '-');
                    }
                }
                //var barcode = new Code39BarCode()

                //{



                //    BarCodeText = GetBarcodeText(assignBox.AssignBoxId),
                //    BarCodeWeight = BarCodeWeight.Small,
                //    Height = 80,

                //};
                string BarCodeText = GetBarcodeText(assignBox.AssignBoxId);
                Image myimg = Code128Rendering.MakeBarcodeImage(BarCodeText, 2, false);
                byte[] fileContents = imageToByteArray(myimg);
                //byte[] fileContents = barcode.Generate();


                string _relativePath = "~/Content/Images/" + ConfigurationManager.AppSettings["BarCodes"].ToString() + "AssignBox-" + assignBox.AssignBoxId.ToString();
                var path = Server.MapPath("~/Content/Images/" + ConfigurationManager.AppSettings["BarCodes"].ToString() + "AssignBox-" + assignBox.AssignBoxId.ToString());
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //Image returnImage = byteArrayToImage(fileContents);

                string filePath = path + ".Bmp";

                System.IO.File.WriteAllBytes(filePath, fileContents);

                #region BarCode Insert or Update

                BarCode _existingBarCode = repo.BarCodeRepository.GetByAssignBoxId(assignBox.AssignBoxId);

                if (_existingBarCode == null)
                {
                    _existingBarCode = new BarCode();


                }
                _existingBarCode.AssignBoxId = assignBox.AssignBoxId;
                _existingBarCode.ImagePathRel = _relativePath + ".Bmp";
                _existingBarCode.ImagePathAbs = "file://" + filePath;
                _existingBarCode.BoxNo = assignBox.BoxNo;


                string _reportField = string.Empty;


                _existingBarCode.Year = assignBox.Year;
                _existingBarCode.BoxName = assignBox.BoxName;
                _existingBarCode.ClientName = assignBox.Item.Client.ClientName;
                _existingBarCode.DeptName = assignBox.Item.Department.DepartmentName;

                _reportField = "Box Name-" + _existingBarCode.BoxName + System.Environment.NewLine;
                _reportField = _reportField + "Box No-" + _existingBarCode.BoxNo + System.Environment.NewLine;
                _reportField = _reportField + "Client-" + _existingBarCode.ClientName + System.Environment.NewLine + "Department-" + _existingBarCode.DeptName + System.Environment.NewLine;

                _existingBarCode.ReportFiled = _reportField;



                repo.BarCodeRepository.InsertOrUpdate(_existingBarCode);
                repo.BarCodeRepository.Save();
                #endregion
                AssignBox _assignBox = repo.AssignBoxRepository.Find(assignBox.AssignBoxId);

                _assignBox.BarCodeId = _existingBarCode.BarCodeId;
                //  _assignBox.TransmittalINStatusId = repo.TransmittalINStatusRepository.Find(Convert.ToInt64(EnumHelper.Status.Barcode_Assigned)).TransmittalINStatusId;

                repo.AssignBoxRepository.InsertOrUpdate(_assignBox);
                repo.AssignBoxRepository.Save();


                BoxLocation location = new BoxLocation();
                location.AssignBoxId = assignBox.AssignBoxId;
                location.ColumnID = _tempColumnID;
                location.CurrentStatus = "In WareHouse";
                location.FloorID = _tempFloorID;
                location.HeightID = _tempHeightID;
                location.IsPallet = false;
                location.LevelID = _tempLevelID;
                location.RackID = _tempRackID;
                location.RowID = _tempRowID;
                location.TrainID = _tempTrainID;
                location.WareHouseID = _tempWarehouseID;
                location.ZoneID = _tempZoneID;


                repo.BoxLocationRepository.InsertOrUpdate(location);
                repo.BoxLocationRepository.Save();


                Row newROw = repo.RowRepository.Find(_tempRowID);
                newROw.IsAssigned = true;
                repo.RowRepository.InsertOrUpdate(newROw);
                repo.RowRepository.Save();

                List<BoxLocation> _boxList = new List<BoxLocation>();
                _boxList.Add(location);

                assignBox.BoxLocation = _boxList;
                repo.AssignBoxRepository.InsertOrUpdate(assignBox);
                repo.AssignBoxRepository.Save();


            }
            Session["TotalDataUploaded"] = intcount;
        }

        private void SaveDataPallet(DataTable dt)
        {
            List<Item> itemList = new List<Item>();
            long intcount = 0;
            foreach (DataRow dr in dt.Rows)
            {
                intcount++;
                #region Get All Values From XL
                string wareHouseName = dr["Ware House Name "].ToString();
                string floorName = dr["Floor Name"].ToString();
                string zoneName = dr["Zone Name"].ToString();
                string palletName = dr["Pallete Name"].ToString();

                string clientName = dr["Client Name"].ToString();
                string department = dr["Department Name"].ToString();
                string project = dr["Project"].ToString();
                string contactPerson = dr["IssuedBy"].ToString();

                string transmittalNo = dr["TransmittallN No"].ToString();


                string clientReqRef = dr["Cliente Ref No"].ToString();
                string habdoverBy = dr["Hand Over By"].ToString();
                string receivedBy = dr["Received By"].ToString();
                string boxName = dr["Box Name"].ToString();
                string boxNo = dr["Box No"].ToString();

                if (boxNo.Length < 10)
                {
                    boxNo = boxNo.PadLeft(10, '0');
                }






                string year = dr["Year"].ToString();
                try
                {

                    DateTime destructionDate = Convert.ToDateTime(dr["Distruction Date"].ToString());
                }
                catch
                {
                    DateTime destructionDate = DateTime.Now;
                }

                string supportStuff = dr["Client Supervisor"].ToString();
                string orblDept = dr["Supervisor Department"].ToString();
                #endregion

                #region Public Variables

                int deptFlag = 0;

                long _tempWarehouseID = 0;
                long _tempFloorID = 0;
                long _tempZoneID = 0;
                long _tempTrainID = 0;
                long _tempRackID = 0;
                long _tempLevelID = 0;
                long _tempHeightID = 0;
                long _tempColumnID = 0;
                long _tempRowID = 0;
                long _tempORBLDeptID = 0;
                long _tempSupportStuffID = 0;
                long _tempClientID = 0;
                long _tempDepartmentID = 0;
                long _tempContactPersonID = 0;
                long _tempPalletID = 0;
                #endregion

                #region WareHouse Setup Module

                Warehouse _warehouse = repo.WarehouseRepository.GetByName(wareHouseName);
                if (_warehouse == null)
                {
                    _warehouse = new Warehouse();
                    _warehouse.WarehouseName = wareHouseName;
                    _warehouse.WarehouseCode = "";
                    repo.WarehouseRepository.InsertOrUpdate(_warehouse);
                    repo.Save();
                }
                _tempWarehouseID = _warehouse.WarehouseID;


                Floor _floor = repo.FloorRepository.GetByWidandFName(_tempWarehouseID, floorName);
                if (_floor == null)
                {
                    _floor = new Floor();
                    _floor.FloorName = floorName;
                    _floor.FloorCode = "";
                    _floor.WarehouseID = _tempWarehouseID;
                    repo.FloorRepository.InsertOrUpdate(_floor);
                    repo.FloorRepository.Save();
                }
                _tempFloorID = _floor.FloorID;


                Zone _zone = repo.ZoneRepository.GetBywIDandFIDandzoneName(_tempWarehouseID, _tempFloorID, zoneName);
                if (_zone == null)
                {
                    _zone = new Zone();
                    _zone.ZoneName = zoneName;
                    _zone.WarehouseID = _tempWarehouseID;
                    _zone.FloorID = _tempFloorID;
                    _zone.ZoneCode = "";
                    repo.ZoneRepository.InsertOrUpdate(_zone);
                    repo.ZoneRepository.Save();
                }
                _tempZoneID = _zone.ZoneID;


                Pallet _pallet = repo.PalletRepository.GetByZoneIDPallet(_tempZoneID);
                if (_pallet == null)
                {
                    _pallet = new Pallet();
                    _pallet.WarehouseID = _tempWarehouseID;
                    _pallet.FloorID = _tempFloorID;
                    _pallet.ZoneID = _tempZoneID;

                    _pallet.PalletName = palletName;
                    repo.PalletRepository.InsertOrUpdate(_pallet);
                    repo.PalletRepository.Save();
                }
                _tempPalletID = _pallet.PalletId;



                #endregion

                #region Client Setup Module

                #region ORBL Depts
                ORBLDepartment _orblDept = repo.ORBLDepartmentRepository.GetByName(orblDept);

                if (_orblDept == null)
                {
                    _orblDept = new ORBLDepartment();
                    _orblDept.ORBLDepartmentCode = "";
                    _orblDept.ORBLDepartmentName = orblDept;
                    repo.ORBLDepartmentRepository.InsertOrUpdate(_orblDept);
                    repo.ORBLDepartmentRepository.Save();
                }
                _tempORBLDeptID = _orblDept.ORBLDepartmentID;

                #endregion
                #region ORBLSupportStuff
                SupportStuff _supportStuff = repo.SupportStuffRepository.GetByNameandDeptID(supportStuff, _tempORBLDeptID);

                if (_supportStuff == null)
                {
                    _supportStuff = new SupportStuff();
                    _supportStuff.SupportStuffName = supportStuff;
                    _supportStuff.SupportStuffMobileNo = "01600000000";
                    _supportStuff.ORBLDepartmentID = _tempORBLDeptID;
                    _supportStuff.SupportStuffCode = "";
                    repo.SupportStuffRepository.InsertOrUpdate(_supportStuff);
                    repo.SupportStuffRepository.Save();
                }
                _tempSupportStuffID = _supportStuff.SupportStuffID;


                #endregion
                #region Project Setup

                Project _project = repo.ProjectRepository.GetByCIDandDeptIDandProjectName(_tempClientID, _tempDepartmentID, project);


                #endregion

                #region Client Setup

                Client _client = repo.ClientRepository.GetByClientName(clientName);

                if (_client == null)
                {
                    _client = new Client();
                    _client.ClientName = clientName;
                    _client.ClientAddress = "Initial Address";
                    _client.SupportStuffID = _tempSupportStuffID;
                    repo.ClientRepository.InsertOrUpdate(_client);
                    repo.ClientRepository.Save();
                }
                _tempClientID = _client.ClientID;

                Department _department = repo.DepartmentRepository.GetByClientIDandDeptName(_tempClientID, department);

                if (_department == null)
                {
                    deptFlag = 1;
                    _department = new Department();
                    _department.DepartmentName = department;
                    _department.DepartmentCode = "";
                    _department.DepartmentAddress = "";
                    _department.ClientID = _tempClientID;
                    repo.DepartmentRepository.InsertOrUpdate(_department);
                    repo.DepartmentRepository.Save();
                }
                _tempDepartmentID = _department.DepartmentID;

                ContactPerson _contactPerson = repo.ContactPersonRepository.GetByCIDandDeptIDandContactName(_tempClientID, _tempDepartmentID, contactPerson);

                if (_contactPerson == null)
                {
                    _contactPerson = new ContactPerson();
                    _contactPerson.ContactPersonName = contactPerson;
                    _contactPerson.DepartmentID = _tempDepartmentID;
                    _contactPerson.ClientID = _tempClientID;
                    _contactPerson.Address = "";
                    _contactPerson.Email = "";
                    _contactPerson.PhoneNumber = "01600000000";
                    repo.ContactPersonRepository.InsertOrUpdate(_contactPerson);
                    repo.ContactPersonRepository.Save();
                }
                _tempContactPersonID = _contactPerson.ContactPersontID;

                #endregion

                #endregion
                //Make a common TransmittalIN
                TransmittalIN trIn = new TransmittalIN();
                string trNo = "ORBL-2014-Demo.Pallet";
                trIn = repo.TransmittalINRepository.GetByName(trNo);
                if (trIn == null)
                {
                    trIn = new TransmittalIN();



                    trIn.ClientID = _tempClientID;
                    trIn.ContactPersontID = _tempContactPersonID;
                    trIn.CreateDate = DateTime.Now;
                    trIn.DepartmentID = _tempDepartmentID;
                    trIn.IsFile = true;
                    trIn.TotalArchieveItem = 0;
                    trIn.TransmittalDate = DateTime.Now;
                    trIn.TransmittalNo = trNo;
                    trIn.Type = "Box";
                    trIn.TransmittalINStatusId = 5;


                    ORBLOperator _operator = repo.ORBLOperatorRepository.GetByName("Saiful");

                    //trout.ORBLOperatorId = handoverby.ORBLOperatorId;
                    //trout.OrblOperatorAddress = handoverby.Address;
                    List<ORBLOperator> operatorList = new List<ORBLOperator>();
                    operatorList.Add(_operator);

                    trIn.ReceivedBy = operatorList;

                    repo.TransmittalINRepository.InsertOrUpdate(trIn);
                    repo.TransmittalINRepository.Save();

                    List<TransmittalIN> trINList = new List<TransmittalIN>();
                    trINList = repo.TransmittalINRepository.GetTrINList(trIn.TransmittalINId);
                    HandOverBy _handOver = new HandOverBy();
                    _handOver.Name = habdoverBy;
                    _handOver.Address = "initial address";
                    _handOver.Date = DateTime.Now;
                    _handOver.TransmittalINs = trINList;


                    repo.HandOverByRepository.InsertOrUpdate(_handOver);
                    repo.HandOverByRepository.Save();
                }

                Item box = new Item();
                box.BoxNo = boxNo;
                box.ClientID = _tempClientID;
                box.Comments = "nothing";
                box.DepartmentID = _tempDepartmentID;
                box.IsArchieve = false;
                box.ItemName = boxName;
                box.Quantity = "1";
                box.Year = year;
                box.Unit = "Box";
                repo.ItemRepository.InsertOrUpdate(box);
                repo.ItemRepository.Save();

                itemList.Add(box);

                trIn.Items = itemList;
                repo.TransmittalINRepository.InsertOrUpdate(trIn);
                repo.TransmittalINRepository.Save();

                AssignBox assignBox = new AssignBox();

                assignBox.AssignDate = DateTime.Now;
                assignBox.BoxName = box.ItemName;
                assignBox.BoxNo = box.BoxNo;
                assignBox.CurrentStatus = 1;
                assignBox.ItemId = box.ItemId;
                assignBox.TransmittalINId = trIn.TransmittalINId;
                assignBox.TransmittalINStatusId = 5;
                assignBox.WarehouseID = _tempWarehouseID;
                assignBox.Year = box.Year;
                assignBox.WarehouseName = _warehouse.WarehouseName;
                repo.AssignBoxRepository.InsertOrUpdate(assignBox);
                repo.AssignBoxRepository.Save();

                string _boxNo = assignBox.BoxNo;
                char[] arr = new char[5] { 'a', 'b', 'c', 'd', 'e' };
                if (assignBox.BoxNo.Contains('(') && assignBox.BoxNo.Contains(')'))
                {
                    _boxNo = assignBox.BoxNo.Replace('(', '-').Replace(')', '-');
                }

                for (int i = 0; i < arr.Length; i++)
                {
                    if (assignBox.BoxNo.Contains(arr[i]))
                    {
                        _boxNo = assignBox.BoxNo.Replace(arr[i], '-');
                    }
                }
                //var barcode = new Code39BarCode()

                //{

                //    BarCodeText = GetBarcodeText(assignBox.AssignBoxId),
                //    BarCodeWeight = BarCodeWeight.Small,
                //    Height = 80,

                //};

                //// Generate the barcode
                //byte[] fileContents = barcode.Generate();

                string BarCodeText = GetBarcodeText(assignBox.AssignBoxId);
                Image myimg = Code128Rendering.MakeBarcodeImage(BarCodeText, 2, false);
                byte[] fileContents = imageToByteArray(myimg);


                string _relativePath = "~/Content/Images/" + ConfigurationManager.AppSettings["BarCodes"].ToString() + "AssignBox-" + assignBox.AssignBoxId.ToString();
                var path = Server.MapPath("~/Content/Images/" + ConfigurationManager.AppSettings["BarCodes"].ToString() + "AssignBox-" + assignBox.AssignBoxId.ToString());
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //Image returnImage = byteArrayToImage(fileContents);

                string filePath = path + ".Bmp";

                System.IO.File.WriteAllBytes(filePath, fileContents);

                #region BarCode Insert or Update

                BarCode _existingBarCode = repo.BarCodeRepository.GetByAssignBoxId(assignBox.AssignBoxId);

                if (_existingBarCode == null)
                {
                    _existingBarCode = new BarCode();


                }
                _existingBarCode.AssignBoxId = assignBox.AssignBoxId;
                _existingBarCode.ImagePathRel = _relativePath + ".Bmp";
                _existingBarCode.ImagePathAbs = "file://" + filePath;
                _existingBarCode.BoxNo = assignBox.BoxNo;


                string _reportField = string.Empty;


                _existingBarCode.Year = assignBox.Year;
                _existingBarCode.BoxName = assignBox.BoxName;
                _existingBarCode.ClientName = assignBox.Item.Client.ClientName;
                _existingBarCode.DeptName = assignBox.Item.Department.DepartmentName;

                _reportField = "Box Name-" + _existingBarCode.BoxName + System.Environment.NewLine;
                _reportField = _reportField + "Box No-" + _existingBarCode.BoxNo + System.Environment.NewLine;
                _reportField = _reportField + "Client-" + _existingBarCode.ClientName + System.Environment.NewLine + "Department-" + _existingBarCode.DeptName + System.Environment.NewLine;

                _existingBarCode.ReportFiled = _reportField;



                repo.BarCodeRepository.InsertOrUpdate(_existingBarCode);
                repo.BarCodeRepository.Save();
                #endregion
                AssignBox _assignBox = repo.AssignBoxRepository.Find(assignBox.AssignBoxId);

                _assignBox.BarCodeId = _existingBarCode.BarCodeId;
                //  _assignBox.TransmittalINStatusId = repo.TransmittalINStatusRepository.Find(Convert.ToInt64(EnumHelper.Status.Barcode_Assigned)).TransmittalINStatusId;

                repo.AssignBoxRepository.InsertOrUpdate(_assignBox);
                repo.AssignBoxRepository.Save();


                BoxLocation location = new BoxLocation();
                location.AssignBoxId = assignBox.AssignBoxId;

                location.CurrentStatus = "In WareHouse";
                location.FloorID = _tempFloorID;

                location.IsPallet = true;
                location.PalletId = _tempPalletID;

                location.WareHouseID = _tempWarehouseID;
                location.ZoneID = _tempZoneID;


                repo.BoxLocationRepository.InsertOrUpdate(location);
                repo.BoxLocationRepository.Save();

                List<BoxLocation> _boxList = new List<BoxLocation>();
                _boxList.Add(location);

                assignBox.BoxLocation = _boxList;
                repo.AssignBoxRepository.InsertOrUpdate(assignBox);
                repo.AssignBoxRepository.Save();

            }
            Session["TotalDataUploaded"] = intcount;
        }

        public ActionResult Limit()
        {
            foreach (AssignBox item in repo.AssignBoxRepository.GetAllBox())
            {
                BoxLocation _box = new BoxLocation();
                List<BoxLocation> boxList = new List<BoxLocation>();

                _box = repo.BoxLocationRepository.GetByAssignBoxId(item.AssignBoxId);
                boxList.Add(_box);
                item.BoxLocation = boxList;
                repo.AssignBoxRepository.InsertOrUpdate(item);
                repo.AssignBoxRepository.Save();

            }
            return View("Index", "Home");
        }

        public ActionResult SetBarcode()
        {

            //List<Warehouse> allWarehouse = new List<Warehouse>();
            //allWarehouse = repo.WarehouseRepository.GetAllList();

            //foreach (Warehouse item2 in allWarehouse)
            //{

                    List<AssignBox> assignBoxList = new List<AssignBox>();
                    //assignBoxList = repo.AssignBoxRepository.GetByTrInIdAndWidandStatusAll(290, item2.WarehouseID, Convert.ToInt64(EnumHelper.Status.Received_at_WareHouse));
                   // assignBoxList = repo.AssignBoxRepository.GetMyFilteredData(290, item2.WarehouseID, Convert.ToInt64(EnumHelper.Status.Received_at_WareHouse), 12255);
                    assignBoxList = repo.AssignBoxRepository.GetBytrStatusForBarcode(Convert.ToInt64(EnumHelper.Status.Barcode_Assigned));

                    foreach (AssignBox item in assignBoxList)
                    {
                       

                       

                        #region BarCode Insert or Update

                        BarCode _existingBarCode = repo.BarCodeRepository.GetByAssignBoxId(item.AssignBoxId);

                        if (_existingBarCode != null)
                        {
                            // _existingBarCode = new BarCode();
                            string BarCodeText = GetBarcodeText(item.AssignBoxId);
                            Image myimg = Code128Rendering.MakeBarcodeImage(BarCodeText, 2, false);
                            byte[] fileContents = imageToByteArray(myimg);


                            string _relativePath = "~/Content/Images/" + ConfigurationManager.AppSettings["BarCodes"].ToString() + "AssignBox-" + item.AssignBoxId.ToString();
                            var path =  Server.MapPath("~/Content/Images/" + ConfigurationManager.AppSettings["BarCodes"].ToString() + "AssignBox-" + item.AssignBoxId.ToString());
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            //Image returnImage = byteArrayToImage(fileContents);

                            string filePath = path + ".GIF";

                            System.IO.File.WriteAllBytes(filePath, fileContents);


                            _existingBarCode.AssignBoxId = item.AssignBoxId;
                            _existingBarCode.ImagePathRel = _relativePath + ".GIF";
                            _existingBarCode.ImagePathAbs = "file://" + filePath;
                            _existingBarCode.BoxNo = item.BoxNo;
                            _existingBarCode.Year = item.Year;
                            repo.BarCodeRepository.InsertOrUpdate(_existingBarCode);
                            repo.BarCodeRepository.Save();
                            AssignBox _assignBox = repo.AssignBoxRepository.Find(item.AssignBoxId);

                            _assignBox.BarCodeId = _existingBarCode.BarCodeId;
                            _assignBox.TransmittalINStatusId = repo.TransmittalINStatusRepository.Find(Convert.ToInt64(EnumHelper.Status.Barcode_Assigned)).TransmittalINStatusId;

                            repo.AssignBoxRepository.InsertOrUpdate(_assignBox);
                            repo.AssignBoxRepository.Save();

                            ViewBag.Show = _assignBox.TransmittalINStatusId;

                        #endregion


                            AssignBox IsPending = repo.AssignBoxRepository.GetStatusByTrID(item.TransmittalINId, item.WarehouseID, Convert.ToInt64(EnumHelper.Status.Received_at_WareHouse));
                            if (IsPending == null)
                            {
                                TransmittalIN trIN = repo.TransmittalINRepository.Find(item.TransmittalINId);

                                trIN.TransmittalINStatusId = repo.TransmittalINStatusRepository.Find(Convert.ToInt64(EnumHelper.Status.Barcode_Assigned)).TransmittalINStatusId;
                                repo.TransmittalINRepository.InsertOrUpdate(trIN);
                                repo.TransmittalINRepository.Save();
                            }
                        }

                    }
               // }

           // }


            return View();

        }

        public ActionResult DeleteAllError()
        {
            List<AssignBox> allAssigned = repo.AssignBoxRepository.GetByWareHouseId(186);

            foreach (AssignBox item in allAssigned)
            {
                //Item _item = new Item();
                //_item = repo.ItemRepository.Find(item.ItemId);

                BarCode barcode = new BarCode();
                barcode = repo.BarCodeRepository.GetByAssignBoxId(item.AssignBoxId);


                repo.BarCodeRepository.Delete(barcode.BarCodeId);
                repo.BarCodeRepository.Save();

                //repo.ItemRepository.Delete(item.ItemId);
                //repo.ItemRepository.Save();

                //BoxLocation box = new BoxLocation();
                //box = repo.BoxLocationRepository.GetByAssignBoxId(item.AssignBoxId);

                //repo.BoxLocationRepository.Delete(box.BoxLocationId);
                //repo.BoxLocationRepository.Save();
            }

            return null;
        }


        public ActionResult Barcode128()
        {

            Image myimg = Code128Rendering.MakeBarcodeImage("123456789",5, true);

            ViewBag.img = myimg;
            return View();
        }

    }
}