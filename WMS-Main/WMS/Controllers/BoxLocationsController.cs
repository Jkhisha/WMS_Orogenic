using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;
using System.Configuration;
using System.IO;
using System.Data.OleDb;
using PagedList;
using System.Net;

namespace WareHouseMVC.Controllers
{
    public class BoxLocationsController : Controller
    {
        UnitOfWork repo = new UnitOfWork();

        public IPagedList<AssignBox> pagedList { get; set; }  
          

        //
        // GET: /BoxLocations/

        public ViewResult Index()
        {
            return View(repo.BoxLocationRepository.AllIncluding(boxlocation => boxlocation.AssignBox).ToList());
        }

        //
        // GET: /BoxLocations/Details/5

        public ViewResult Details(long id)
        {
            BoxLocation boxlocation = repo.BoxLocationRepository.Find(id);
            return View(boxlocation);
        }

        //
        // GET: /BoxLocations/Create

        public ActionResult Create()
        {
            ViewBag.PossibleWareHouses = repo.WarehouseRepository.AllIncluding();
            ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
            ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
            ViewBag.PossibleTrains = repo.TrainRepository.AllIncluding();
            ViewBag.PossibleRacks = repo.RackRepository.AllIncluding();
            ViewBag.PossibleLevels = repo.LevelRepository.AllIncluding();
            ViewBag.PossibleHeights = repo.HeightRepository.AllIncluding();
            ViewBag.PossibleColumns = repo.ColumnRepository.AllIncluding();
            ViewBag.PossibleRows = repo.RowRepository.AllIncluding();
            return View();
        }

        //
        // POST: /BoxLocations/Create

        [HttpPost]
        public ActionResult Create(BoxLocation boxlocation)
        {
            if (ModelState.IsValid)
            {
                
                repo.BoxLocationRepository.InsertOrUpdate(boxlocation);
                repo.BoxLocationRepository.Save();
                return RedirectToAction("Index");


            }

            ViewBag.PossibleWareHouses = repo.WarehouseRepository.AllIncluding();
            ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
            ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
            ViewBag.PossibleTrains = repo.TrainRepository.AllIncluding();
            ViewBag.PossibleRacks = repo.RackRepository.AllIncluding();
            ViewBag.PossibleLevels = repo.LevelRepository.AllIncluding();
            ViewBag.PossibleHeights = repo.HeightRepository.AllIncluding();
            ViewBag.PossibleColumns = repo.ColumnRepository.AllIncluding();
            ViewBag.PossibleRows = repo.RowRepository.AllIncluding();
            return View(boxlocation);
        }

        //
        // GET: /BoxLocations/Edit/5

        public ActionResult Edit(long id)
        {
            BoxLocation boxlocation = repo.BoxLocationRepository.Find(id);
            ViewBag.PossibleWareHouses = repo.WarehouseRepository.AllIncluding();
            ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
            ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
            ViewBag.PossibleTrains = repo.TrainRepository.AllIncluding();
            ViewBag.PossibleRacks = repo.RackRepository.AllIncluding();
            ViewBag.PossibleLevels = repo.LevelRepository.AllIncluding();
            ViewBag.PossibleHeights = repo.HeightRepository.AllIncluding();
            ViewBag.PossibleColumns = repo.ColumnRepository.AllIncluding();
            ViewBag.PossibleRows = repo.RowRepository.AllIncluding();
            return View(boxlocation);
        }

        //
        // POST: /BoxLocations/Edit/5

        [HttpPost]
        public ActionResult Edit(BoxLocation boxlocation)
        {
            if (ModelState.IsValid)
            {
                repo.BoxLocationRepository.InsertOrUpdate(boxlocation);
                repo.BoxLocationRepository.Save();
                return RedirectToAction("Index");
            }
            ViewBag.PossibleWareHouses = repo.WarehouseRepository.AllIncluding();
            ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
            ViewBag.PossibleZones = repo.ZoneRepository.AllIncluding();
            ViewBag.PossibleTrains = repo.TrainRepository.AllIncluding();
            ViewBag.PossibleRacks = repo.RackRepository.AllIncluding();
            ViewBag.PossibleLevels = repo.LevelRepository.AllIncluding();
            ViewBag.PossibleHeights = repo.HeightRepository.AllIncluding();
            ViewBag.PossibleColumns = repo.ColumnRepository.AllIncluding();
            ViewBag.PossibleRows = repo.RowRepository.AllIncluding();
            return View(boxlocation);
        }


        public ActionResult BoxSummary()
        {
            ViewBag.ClientList = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.AllIncluding();
            return View();
        }

        [HttpPost]
        public ActionResult BoxSummary(BoxSummaryViewModel model)
        {


            ViewBag.ClientList = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(model.ClientID);// AllIncluding();

            return View();
        }




        public ActionResult PrintBoxList(long clientID, long deptID, DateTime startDate, DateTime endDate, int reportType)
        {

            long _clientID = clientID;
            long _deptID = deptID;
            DateTime _startDate = DateTime.Now;
            DateTime _endDate = DateTime.Now;

            if (startDate != DateTime.MinValue)
            {
                _startDate =startDate;
            }
            if (endDate != DateTime.MinValue)
            {
                _endDate = endDate;
            }


            List<AssignBox> listofAllBoxes = new List<AssignBox>();
            List<AssignBox> finalList = new List<AssignBox>();

            listofAllBoxes = repo.AssignBoxRepository.GetByClientIdAndDepartmentId(_clientID, _deptID);


            foreach (AssignBox item in listofAllBoxes)
            {
                if (_startDate.ToShortDateString() != DateTime.Now.ToShortDateString() && _endDate.ToShortDateString() != DateTime.Now.ToShortDateString())
                {

                    if (item.AssignDate.Date >= _startDate.Date && item.AssignDate.Date <= _endDate.Date)
                    {

                        List<BoxLocation> _boxLocationList = repo.BoxLocationRepository.GetListByAssignBoxId(item.AssignBoxId);
                        item.BoxLocation = _boxLocationList;


                        finalList.Add(item);
                    }


                }

                else
                {
                    List<BoxLocation> _boxLocationList = repo.BoxLocationRepository.GetListByAssignBoxId(item.AssignBoxId);
                    item.BoxLocation = _boxLocationList;



                    finalList.Add(item);
                }


            }


            TempData["finalList"] = finalList;
            return RedirectToAction("BoxSearchReport", "Reports", new { clientID = clientID, deptID = deptID, rptType = reportType });

        }

        public ActionResult PrintBoxListWithOutDept(long clientID, DateTime startDate, DateTime endDate, int reportType)
        {

            long _clientID = clientID;
            DateTime _startDate = DateTime.Now;
            DateTime _endDate = DateTime.Now;

            if (startDate != DateTime.MinValue)
            {
                _startDate = startDate;
            }
            if (endDate != DateTime.MinValue)
            {
                _endDate = endDate;
            }


            List<AssignBox> listofAllBoxes = new List<AssignBox>();
            List<AssignBox> finalList = new List<AssignBox>();

            listofAllBoxes = repo.AssignBoxRepository.GetByClientId(_clientID);


            foreach (AssignBox item in listofAllBoxes)
            {
                if (_startDate.ToShortDateString() != DateTime.Now.ToShortDateString() && _endDate.ToShortDateString() != DateTime.Now.ToShortDateString())
                {

                    if (item.AssignDate.Date >= _startDate.Date && item.AssignDate.Date <= _endDate.Date)
                    {

                        List<BoxLocation> _boxLocationList = repo.BoxLocationRepository.GetListByAssignBoxId(item.AssignBoxId);
                        item.BoxLocation = _boxLocationList;


                        finalList.Add(item);
                    }


                }

                else
                {
                    List<BoxLocation> _boxLocationList = repo.BoxLocationRepository.GetListByAssignBoxId(item.AssignBoxId);
                    item.BoxLocation = _boxLocationList;



                    finalList.Add(item);
                }


            }


            TempData["finalList"] = finalList;
            return RedirectToAction("BoxSearchReport", "Reports", new { clientID = clientID, deptID = 0, rptType = reportType });

        }

        public ActionResult PrintBoxListFile(long clientID, long deptID, DateTime startDate, DateTime endDate, int reportType)
        {


            long _clientID = clientID;
            long _deptID = deptID;
            DateTime _startDate = DateTime.Now;
            DateTime _endDate = DateTime.Now;

            if (startDate != DateTime.MinValue)
            {
                _startDate = startDate;
            }
            if (endDate != DateTime.MinValue)
            {
                _endDate = endDate;
            }


            List<AssignBox> listofAllBoxes = new List<AssignBox>();
            List<AssignBox> finalList = new List<AssignBox>();

            listofAllBoxes = repo.AssignBoxRepository.GetByClientIdAndDepartmentIdFile(_clientID, _deptID);


            foreach (AssignBox item in listofAllBoxes)
            {
                if (_startDate.ToShortDateString() != DateTime.Now.ToShortDateString() && _endDate.ToShortDateString() != DateTime.Now.ToShortDateString())
                {

                    if (item.AssignDate.Date >= _startDate.Date && item.AssignDate.Date <= _endDate.Date)
                    {

                        List<BoxLocation> _boxLocationList = repo.BoxLocationRepository.GetListByAssignBoxId(item.AssignBoxId);
                        item.BoxLocation = _boxLocationList;


                        finalList.Add(item);
                    }


                }

                else
                {
                    List<BoxLocation> _boxLocationList = repo.BoxLocationRepository.GetListByAssignBoxId(item.AssignBoxId);
                    item.BoxLocation = _boxLocationList;



                    finalList.Add(item);
                }


            }


            TempData["finalList"] = finalList;
            return RedirectToAction("BoxSearchReportFile", "Reports", new { clientID = clientID, deptID = deptID, rptType = reportType });

        }

        public ActionResult LocationAssign(long trID)
        {
            User _user = repo.UserRepository.GetUserByUserName(HttpContext.User.Identity.Name);
            long _wID = 0;
            if (_user.WarehouseID.HasValue)
            {
                _wID = _user.WarehouseID.Value;
            }


            TransmittalIN _tempTransmittalIN = repo.TransmittalINRepository.Find(trID);

            List<AssignBox> listofBoxes = new List<AssignBox>();

            listofBoxes = repo.AssignBoxRepository.GetByTrInIdAndWidandStatus(trID, _wID, Convert.ToInt64(EnumHelper.Status.Barcode_Verified));

            List<Row> rowList = repo.RowRepository.GetByEmptyStatusAndCount(false, _wID, listofBoxes.Count, _tempTransmittalIN.ClientID);

         

            if (listofBoxes.Count() == rowList.Count())
            {
                int i = 0;
                foreach (AssignBox item in listofBoxes)
                {
                    #region BoxLocation Update
                    BoxLocation _boxLocation = repo.BoxLocationRepository.GetByAssignBoxId(item.AssignBoxId);
                    if (_boxLocation == null)
                    {
                        _boxLocation = new BoxLocation();
                    }


                    _boxLocation.AssignBoxId = item.AssignBoxId;
                    _boxLocation.RowID = rowList[i].RowID;
                    _boxLocation.Row = repo.RowRepository.Find(rowList[i].RowID);

                    _boxLocation.ColumnID = rowList[i].ColumnID;
                    _boxLocation.Column = repo.ColumnRepository.Find(rowList[i].ColumnID);

                    _boxLocation.HeightID = rowList[i].HeightID;
                    _boxLocation.Height = repo.HeightRepository.Find(rowList[i].HeightID);

                    _boxLocation.LevelID = rowList[i].LevelID;
                    _boxLocation.Level = repo.LevelRepository.Find(rowList[i].LevelID);

                    _boxLocation.RackID = rowList[i].RackID;
                    _boxLocation.Rack = repo.RackRepository.Find(rowList[i].RackID);

                    _boxLocation.TrainID = rowList[i].TrainID;
                    _boxLocation.Train = repo.TrainRepository.Find(rowList[i].TrainID);

                    _boxLocation.ZoneID = rowList[i].ZoneID;
                    _boxLocation.Zone = repo.ZoneRepository.Find(rowList[i].ZoneID);

                    _boxLocation.FloorID = rowList[i].FloorID;
                    _boxLocation.Floor = repo.FloorRepository.Find(rowList[i].FloorID);

                    _boxLocation.WareHouseID = rowList[i].WarehouseID;
                    _boxLocation.Warehouse = repo.WarehouseRepository.Find(rowList[i].WarehouseID);
                    _boxLocation.IsLeatest = true;
                    _boxLocation.CurrentStatus = "In WareHouse";

                    repo.BoxLocationRepository.InsertOrUpdate(_boxLocation);
                    repo.BoxLocationRepository.Save();

                    #endregion

                    #region Row Update
                    Row _row = repo.RowRepository.Find(rowList[i].RowID);
                    _row.IsAssigned = true;

                    repo.RowRepository.InsertOrUpdate(_row);
                    repo.RowRepository.Save();

                    #endregion

                    #region AssignBox Update
                    AssignBox _assignBox = repo.AssignBoxRepository.Find(item.AssignBoxId);

                    List<BoxLocation> _boxLocationList = repo.BoxLocationRepository.GetListByAssignBoxId(item.AssignBoxId);
                    _assignBox.BoxLocation = _boxLocationList;
                    _assignBox.TransmittalINStatusId = repo.TransmittalINStatusRepository.Find(Convert.ToInt64(EnumHelper.Status.Location_Assigned)).TransmittalINStatusId;
                    repo.AssignBoxRepository.InsertOrUpdate(_assignBox);
                    repo.AssignBoxRepository.Save();


                    #endregion

                    i++;

                    #region Transmittal Update

                    AssignBox IsPending = repo.AssignBoxRepository.GetStatusByTrID(item.TransmittalINId, item.WarehouseID, Convert.ToInt64(EnumHelper.Status.Barcode_Verified));
                    if (IsPending == null)
                    {
                        TransmittalIN trIN = repo.TransmittalINRepository.Find(item.TransmittalINId);

                        trIN.TransmittalINStatusId = repo.TransmittalINStatusRepository.Find(Convert.ToInt64(EnumHelper.Status.Location_Assigned)).TransmittalINStatusId;
                        repo.TransmittalINRepository.InsertOrUpdate(trIN);
                        repo.TransmittalINRepository.Save();
                    }



                    #endregion

                    #region maintaining Audit-Trail

                    ChangeLocation location = new ChangeLocation();
                    location.AssignDate = DateTime.Now;
                    location.ItemId = item.ItemId;

                    location.Location = GetLocationString(_boxLocation);
                    repo.ChangeLocationRepository.InsertOrUpdate(location);
                    repo.ChangeLocationRepository.Save();


                    #endregion

                }
            }

            else
            {

                int i = 0;
                foreach (AssignBox item in listofBoxes)
                {
                    #region BoxLocation Update
                    BoxLocation _boxLocation = repo.BoxLocationRepository.GetByAssignBoxId(item.AssignBoxId);
                    if (_boxLocation == null)
                    {
                        _boxLocation = new BoxLocation();
                    }


                    _boxLocation.AssignBoxId = item.AssignBoxId;


                    _boxLocation.Pallet = repo.PalletRepository.GetBywareHouseIDPallet(_wID);
                    _boxLocation.PalletId = repo.PalletRepository.GetBywareHouseIDPallet(_wID).PalletId;
                    _boxLocation.ZoneID = repo.PalletRepository.GetBywareHouseIDPallet(_wID).ZoneID;

                    _boxLocation.FloorID = repo.PalletRepository.GetBywareHouseIDPallet(_wID).FloorID;

                    _boxLocation.WareHouseID = _wID;
                    _boxLocation.Warehouse = repo.WarehouseRepository.Find(_wID);
                    _boxLocation.IsLeatest = true;
                    _boxLocation.CurrentStatus = "In WareHouse";

                    repo.BoxLocationRepository.InsertOrUpdate(_boxLocation);
                    repo.BoxLocationRepository.Save();

                    #endregion

                    

                    #region AssignBox Update
                    AssignBox _assignBox = repo.AssignBoxRepository.Find(item.AssignBoxId);

                    List<BoxLocation> _boxLocationList = repo.BoxLocationRepository.GetListByAssignBoxId(item.AssignBoxId);
                    _assignBox.BoxLocation = _boxLocationList;
                    _assignBox.TransmittalINStatusId = repo.TransmittalINStatusRepository.Find(Convert.ToInt64(EnumHelper.Status.Location_Assigned)).TransmittalINStatusId;
                    repo.AssignBoxRepository.InsertOrUpdate(_assignBox);
                    repo.AssignBoxRepository.Save();


                    #endregion

                    //i++;

                    #region Transmittal Update

                    AssignBox IsPending = repo.AssignBoxRepository.GetStatusByTrID(item.TransmittalINId, item.WarehouseID, Convert.ToInt64(EnumHelper.Status.Barcode_Verified));
                    if (IsPending == null)
                    {
                        TransmittalIN trIN = repo.TransmittalINRepository.Find(item.TransmittalINId);

                        trIN.TransmittalINStatusId = repo.TransmittalINStatusRepository.Find(Convert.ToInt64(EnumHelper.Status.Location_Assigned)).TransmittalINStatusId;
                        repo.TransmittalINRepository.InsertOrUpdate(trIN);
                        repo.TransmittalINRepository.Save();
                    }



                    #endregion

                    #region maintaining Audit-Trail

                    ChangeLocation location = new ChangeLocation();
                    location.AssignDate = DateTime.Now;
                    location.ItemId = item.ItemId;

                    location.Location = GetLocationString(_boxLocation);
                    repo.ChangeLocationRepository.InsertOrUpdate(location);
                    repo.ChangeLocationRepository.Save();


                    #endregion

                }




                //return RedirectToAction("ShowBoxesEmptySpace", "AssignBoxes", new { trID = trID, type = 1 });
            }



            return RedirectToAction("ViewLocation", new { trID = trID });
            //return View(listofBoxesforLocation);
        }

        private string GetLocationString(BoxLocation _boxLocation)
        {
            string _location = string.Empty;

            _location = _boxLocation.Warehouse.WarehouseName + "-";
            _location = _location +repo.FloorRepository.Find(_boxLocation.FloorID).FloorName + "-";
            _location = _location + repo.ZoneRepository.Find(_boxLocation.ZoneID).ZoneName + "-";
            if (_boxLocation.Pallet == null)
            {
                _location = _location + _boxLocation.Train.TrainName + "-";
                _location = _location + _boxLocation.Rack.RackName + "-";
                _location = _location + _boxLocation.Level.LevelName + "-";
                _location = _location + _boxLocation.Height.HeightName + "-";
                _location = _location + _boxLocation.Column.ColumnName + "-";
                _location = _location + _boxLocation.Row.RowName;
            }

            else
            {
                _location = _location + _boxLocation.Pallet.PalletName;// +"-";
            }
            return _location;
        }





        public ActionResult BoxPallet(long trID)
        {
            User _user = repo.UserRepository.GetUserByUserName(HttpContext.User.Identity.Name);
            long _wID = 0;
            if (_user.WarehouseID.HasValue)
            {
                _wID = _user.WarehouseID.Value;
            }


            TransmittalIN _tempTransmittalIN = repo.TransmittalINRepository.Find(trID);

            List<AssignBox> listofBoxes = new List<AssignBox>();

            listofBoxes = repo.AssignBoxRepository.GetByTrInIdAndWidandStatus(trID, _wID, Convert.ToInt64(EnumHelper.Status.Barcode_Assigned));

            Pallet pallet = repo.PalletRepository.GetPalletList(false, _wID, listofBoxes.Count, _tempTransmittalIN.ClientID);

            if (pallet != null)
            {

                foreach (AssignBox item in listofBoxes)
                {
                    #region BoxLocation Update
                    BoxLocation _boxLocation = repo.BoxLocationRepository.GetByAssignBoxId(item.AssignBoxId);
                    if (_boxLocation == null)
                    {
                        _boxLocation = new BoxLocation();
                    }


                    _boxLocation.AssignBoxId = item.AssignBoxId;
                    //_boxLocation.RowID = -1;
                    //_boxLocation.Row = null;

                    //_boxLocation.ColumnID = -1;
                    //_boxLocation.Column = null;

                    //_boxLocation.HeightID = -1;
                    //_boxLocation.Height = null;
                    //_boxLocation.LevelID = -1;
                    //_boxLocation.Level = null;

                    //_boxLocation.RackID = -1;
                    //_boxLocation.Rack = null;

                    //_boxLocation.TrainID = -1;
                    //_boxLocation.Train = null;

                    _boxLocation.ZoneID = pallet.ZoneID;
                    _boxLocation.Zone = repo.ZoneRepository.Find(pallet.ZoneID);

                    _boxLocation.FloorID = pallet.FloorID;
                    _boxLocation.Floor = repo.FloorRepository.Find(pallet.FloorID);

                    _boxLocation.WareHouseID = pallet.WarehouseID;
                    _boxLocation.Warehouse = repo.WarehouseRepository.Find(pallet.WarehouseID);
                    _boxLocation.PalletId = pallet.PalletId;
                    _boxLocation.Pallet = pallet;

                    _boxLocation.IsLeatest = true;
                    _boxLocation.CurrentStatus = "In WareHouse";

                    repo.BoxLocationRepository.InsertOrUpdate(_boxLocation);
                    repo.BoxLocationRepository.Save();

                    #endregion



                    #region AssignBox Update
                    AssignBox _assignBox = repo.AssignBoxRepository.Find(item.AssignBoxId);

                    List<BoxLocation> _boxLocationList = repo.BoxLocationRepository.GetListByAssignBoxId(item.AssignBoxId);
                    _assignBox.BoxLocation = _boxLocationList;
                    _assignBox.TransmittalINStatusId = repo.TransmittalINStatusRepository.Find(Convert.ToInt64(EnumHelper.Status.Location_Assigned)).TransmittalINStatusId;
                    repo.AssignBoxRepository.InsertOrUpdate(_assignBox);
                    repo.AssignBoxRepository.Save();


                    #endregion



                    #region Transmittal Update

                    AssignBox IsPending = repo.AssignBoxRepository.GetStatusByTrID(item.TransmittalINId, item.WarehouseID, Convert.ToInt64(EnumHelper.Status.Barcode_Assigned));
                    if (IsPending == null)
                    {
                        TransmittalIN trIN = repo.TransmittalINRepository.Find(item.TransmittalINId);

                        trIN.TransmittalINStatusId = repo.TransmittalINStatusRepository.Find(Convert.ToInt64(EnumHelper.Status.Location_Assigned)).TransmittalINStatusId;
                        repo.TransmittalINRepository.InsertOrUpdate(trIN);
                        repo.TransmittalINRepository.Save();
                    }



                    #endregion


                    #region maintaining Audit-Trail

                    ChangeLocation location = new ChangeLocation();
                    location.AssignDate = DateTime.Now;
                    location.ItemId = item.ItemId;

                    location.Location = GetLocationStringForPallet(_boxLocation);
                    repo.ChangeLocationRepository.InsertOrUpdate(location);
                    repo.ChangeLocationRepository.Save();


                    #endregion

                }
            }
            else
            {
                return RedirectToAction("ShowBoxesEmptySpace", "AssignBoxes", new { trID = trID, type = 2 });
            }

            return RedirectToAction("ViewLocation", new { trID = trID });
        }

        private string GetLocationStringForPallet(BoxLocation _boxLocation)
        {

            string _location = string.Empty;

            _location = _boxLocation.Warehouse.WarehouseName + "-";
            _location = _location + _boxLocation.Floor.FloorName + "-";
            _location = _location + _boxLocation.Zone.ZoneName + "-";
            _location = _location + _boxLocation.Pallet.PalletName;


            return _location;
        }

        public ActionResult ViewLocation(long trID,int? page)
        {

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            List<AssignBox> listofBoxesforLocation = new List<AssignBox>();
            pagedList = new PagedList<AssignBox>(listofBoxesforLocation, pageNumber, pageSize);

            listofBoxesforLocation = repo.AssignBoxRepository.GetByTrInId(trID);

            pagedList = repo.AssignBoxRepository.GetByTrIdPlist(trID,pageNumber, pageSize);

            ViewBag.trID = trID;
            ViewBag.pager = "1";
            return View("LocationAssign", pagedList);
        }

        public ActionResult BoxLocations(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            User _user = repo.UserRepository.GetUserByUserName(HttpContext.User.Identity.Name);
            long _wID = 0;
            if (_user.WarehouseID.HasValue)
            {
                _wID = _user.WarehouseID.Value;
            }

            List<AssignBox> listofBoxesforLocation = new List<AssignBox>();
            pagedList = new PagedList<AssignBox>(listofBoxesforLocation, pageNumber, pageSize);
            pagedList = repo.AssignBoxRepository.GetByByWidandTrStatusPagedList(_wID, Convert.ToInt64(EnumHelper.Status.Location_Assigned), pageNumber, pageSize);

            ViewBag.Filter = "1";

            if (TempData["Flag"] != null)
                ViewBag.Flag = TempData["Flag"].ToString();

            return View("LocationAssign", pagedList);
        }

        public ActionResult BoxLocationsAll(int? page, string txtBoxNo, string txtBoxNname)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            if (txtBoxNo == "/")
            {
                txtBoxNo = string.Empty;
            }
            if (txtBoxNname == "/")
            {
                txtBoxNname = string.Empty;
            }


            List<AssignBox> listofBoxesforLocation = new List<AssignBox>();
            pagedList = new PagedList<AssignBox>(listofBoxesforLocation, pageNumber, pageSize);


            if (!String.IsNullOrEmpty(txtBoxNo) && !String.IsNullOrEmpty(txtBoxNname))
            {
                if (System.Web.HttpContext.Current.User.IsInRole("Client"))
                {
                    long _clientID = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);

                    // pagedList = repo.AssignBoxRepository.GetByByTrStatusAndClientId(Convert.ToInt64(EnumHelper.Status.Location_Assigned), _clientID).OrderBy(o => o.AssignDate).ToPagedList(pageNumber, pageSize);
                    pagedList = repo.AssignBoxRepository.GetByByTrStatusAndClientIdPagedListFilter(Convert.ToInt64(EnumHelper.Status.Location_Assigned),txtBoxNo,txtBoxNname, _clientID, pageNumber, pageSize);
                }
                else
                {

                    pagedList = repo.AssignBoxRepository.GetByByTrStatusPListFilter(Convert.ToInt64(EnumHelper.Status.Location_Assigned), txtBoxNo, txtBoxNname, pageNumber, pageSize);

                }
            }

            else if (String.IsNullOrEmpty(txtBoxNo) && !String.IsNullOrEmpty(txtBoxNname))
            {

                if (System.Web.HttpContext.Current.User.IsInRole("Client"))
                {
                    long _clientID = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);

                    // pagedList = repo.AssignBoxRepository.GetByByTrStatusAndClientId(Convert.ToInt64(EnumHelper.Status.Location_Assigned), _clientID).OrderBy(o => o.AssignDate).ToPagedList(pageNumber, pageSize);
                    pagedList = repo.AssignBoxRepository.GetByByTrStatusAndClientIdPagedListFilterOnlyName(Convert.ToInt64(EnumHelper.Status.Location_Assigned), txtBoxNname, _clientID, pageNumber, pageSize);
                }
                else
                {

                    pagedList = repo.AssignBoxRepository.GetByByTrStatusPListFilterOnlyName(Convert.ToInt64(EnumHelper.Status.Location_Assigned), txtBoxNname, pageNumber, pageSize);

                }
            }

            else if (!String.IsNullOrEmpty(txtBoxNo) && String.IsNullOrEmpty(txtBoxNname))
            {
                if (System.Web.HttpContext.Current.User.IsInRole("Client"))
                {
                    long _clientID = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);

                    // pagedList = repo.AssignBoxRepository.GetByByTrStatusAndClientId(Convert.ToInt64(EnumHelper.Status.Location_Assigned), _clientID).OrderBy(o => o.AssignDate).ToPagedList(pageNumber, pageSize);
                    pagedList = repo.AssignBoxRepository.GetByByTrStatusAndClientIdPagedListFilterOnlyNo(Convert.ToInt64(EnumHelper.Status.Location_Assigned), txtBoxNo, _clientID, pageNumber, pageSize);
                }
                else
                {

                    pagedList = repo.AssignBoxRepository.GetByByTrStatusPListFilterOnlyNo(Convert.ToInt64(EnumHelper.Status.Location_Assigned), txtBoxNo, pageNumber, pageSize);

                }
            }

            else
            {
                if (System.Web.HttpContext.Current.User.IsInRole("Client"))
                {
                    long _clientID = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);

                    // pagedList = repo.AssignBoxRepository.GetByByTrStatusAndClientId(Convert.ToInt64(EnumHelper.Status.Location_Assigned), _clientID).OrderBy(o => o.AssignDate).ToPagedList(pageNumber, pageSize);
                    pagedList = repo.AssignBoxRepository.GetByByTrStatusAndClientIdPagedList(Convert.ToInt64(EnumHelper.Status.Location_Assigned), _clientID, pageNumber, pageSize);
                }
                else
                {

                    pagedList = repo.AssignBoxRepository.GetByByTrStatusPList(Convert.ToInt64(EnumHelper.Status.Location_Assigned), pageNumber, pageSize);

                }
            }


           

            ViewBag.Filter = "2";
            ViewBag.txtBoxNo=txtBoxNo;
            ViewBag.txtBoxNname = txtBoxNname;

            if (TempData["Flag"] != null)
                ViewBag.Flag = TempData["Flag"].ToString();
            return View("LocationAssign", pagedList);
        }

        public ActionResult FileLocationsAll(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            List<AssignBox> listofBoxesforLocation = new List<AssignBox>();
            pagedList = new PagedList<AssignBox>(listofBoxesforLocation, pageNumber, pageSize);

            if (System.Web.HttpContext.Current.User.IsInRole("Client"))
            {
                long _clientID = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);

                pagedList = repo.AssignBoxRepository.GetByByTrStatusAndClientIdFilePList(Convert.ToInt64(EnumHelper.Status.Location_Assigned), _clientID, pageNumber, pageSize);
            }
            else
            {

                pagedList = repo.AssignBoxRepository.GetByByTrStatusFilePList(Convert.ToInt64(EnumHelper.Status.Location_Assigned),pageNumber,pageSize);

            }

            ViewBag.Filter = "3";

            if (TempData["Flag"] != null)
                ViewBag.Flag = TempData["Flag"].ToString();
            return View("LocationAssignFile", pagedList);
        }

        

        public ActionResult FileSearch()
        {
            BoxSearchViewModel model = new BoxSearchViewModel();
            ViewBag.ClientList = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.AllIncluding();
            //model.ListItems = repo.AssignBoxRepository.GetAllFile();
            model.ListItems = repo.AssignBoxRepository.GetCustomeFileForPageLoad(100);

            List<BoxSearchListViewModelFile> bosSearchList = new List<BoxSearchListViewModelFile>();

            //List<AssignBox> assignBoxList = repo.AssignBoxRepository.GetAllFile();
            List<AssignBox> assignBoxList = repo.AssignBoxRepository.GetCustomeFileForPageLoad(100);

            foreach (AssignBox item in assignBoxList)
            {
                BoxSearchListViewModelFile boxSearch = new BoxSearchListViewModelFile();
                boxSearch.AssignBoxId = item.AssignBoxId;
                boxSearch.AssignDate = item.AssignDate;
                boxSearch.BarCodeId = item.BarCodeId;
                boxSearch.BoxLocation = item.BoxLocation;
                boxSearch.BoxNo = item.BoxNo;
                boxSearch.BookNumber = item.BoxNameFile;
                boxSearch.FileNumber = item.FileNumber;
                boxSearch.ReferrenceNo = item.ReferrenceNo;
                boxSearch.RingNo = item.RingNo;
                boxSearch.AccountNo = item.AccountNo;




                boxSearch.CurrentStatus = item.CurrentStatus;
                boxSearch.DestructionPeriod = item.DestructionPeriod;
                boxSearch.IsLatest = item.IsLatest;
                boxSearch.Item = item.Item;
                boxSearch.ItemId = item.ItemId;
                boxSearch.ItemIds = item.ItemIds;
                boxSearch.TransmittalIN = item.TransmittalIN;
                boxSearch.TransmittalINId = item.TransmittalINId;
                boxSearch.TransmittalINStatus = item.TransmittalINStatus;
                boxSearch.TransmittalINStatusId = item.TransmittalINStatusId;
                boxSearch.TrOutDate = repo.AssignBoxTrOUTRepository.GetLatestByItemId(item.ItemId);
                boxSearch.Warehouse = item.Warehouse;
                boxSearch.WarehouseID = item.WarehouseID;
                boxSearch.WarehouseName = item.WarehouseName;
                boxSearch.Year = item.Year;
                bosSearchList.Add(boxSearch);

            }


            ViewBag.AllBoxes = bosSearchList;
            ViewBag.flag = 0;
            return View(model);
        }

        [HttpPost]
        public ActionResult FileSearch(BoxSearchViewModel model)
        {
            long _clientID = model.ClientID;
            long _deptID = 0; 
            if (model.DepartmentID != null)
            {
                _deptID = model.DepartmentID.Value;
            }
            DateTime _startDate = DateTime.Now;
            DateTime _endDate = DateTime.Now;
            string fileNo = model.FileNo;
            if (model.StartDate != DateTime.MinValue)
            {
                _startDate = model.StartDate;
            }
            if (model.EndDate != DateTime.MinValue)
            {
                _endDate = model.EndDate;
            }


            List<AssignBox> listofAllBoxes = new List<AssignBox>();
            List<AssignBox> finalList = new List<AssignBox>();

            if (string.IsNullOrEmpty(fileNo))
            {
                listofAllBoxes = repo.AssignBoxRepository.GetByClientIdAndDepartmentIdFile(_clientID, _deptID);


                foreach (AssignBox item in listofAllBoxes)
                {
                    if (_startDate.ToShortDateString() != DateTime.Now.ToShortDateString() && _endDate.ToShortDateString() != DateTime.Now.ToShortDateString())
                    {

                        if (item.AssignDate.Date >= _startDate.Date && item.AssignDate.Date <= _endDate.Date)
                        {

                            List<BoxLocation> _boxLocationList = repo.BoxLocationRepository.GetListByAssignBoxId(item.AssignBoxId);
                            item.BoxLocation = _boxLocationList;


                            finalList.Add(item);
                        }


                    }

                    else
                    {
                        List<BoxLocation> _boxLocationList = repo.BoxLocationRepository.GetListByAssignBoxId(item.AssignBoxId);
                        item.BoxLocation = _boxLocationList;



                        finalList.Add(item);
                    }


                }
            }
            else
            {
                finalList = repo.AssignBoxRepository.GetByClientIdAndFileNo(_clientID, _deptID,fileNo);

            }

            ViewBag.ClientList = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(model.ClientID);
            ViewBag.flag = 1;


            List<BoxSearchListViewModelFile> bosSearchList = new List<BoxSearchListViewModelFile>();


            foreach (AssignBox item in finalList)
            {
                BoxSearchListViewModelFile boxSearch = new BoxSearchListViewModelFile();
                boxSearch.AssignBoxId = item.AssignBoxId;
                boxSearch.AssignDate = item.AssignDate;
                boxSearch.BarCodeId = item.BarCodeId;
                boxSearch.BoxLocation = item.BoxLocation;
                boxSearch.BoxNo = item.BoxNo;
                boxSearch.BookNumber = item.BoxNameFile;
                boxSearch.FileNumber = item.FileNumber;
                boxSearch.ReferrenceNo = item.ReferrenceNo;
                boxSearch.RingNo = item.RingNo;
                boxSearch.AccountNo = item.AccountNo;


                boxSearch.CurrentStatus = item.CurrentStatus;
                boxSearch.DestructionPeriod = item.DestructionPeriod;
                boxSearch.IsLatest = item.IsLatest;
                boxSearch.Item = item.Item;
                boxSearch.ItemId = item.ItemId;
                boxSearch.ItemIds = item.ItemIds;
                boxSearch.TransmittalIN = item.TransmittalIN;
                boxSearch.TransmittalINId = item.TransmittalINId;
                boxSearch.TransmittalINStatus = item.TransmittalINStatus;
                boxSearch.TransmittalINStatusId = item.TransmittalINStatusId;
                boxSearch.TrOutDate = repo.AssignBoxTrOUTRepository.GetLatestByItemId(item.ItemId);
                boxSearch.Warehouse = item.Warehouse;
                boxSearch.WarehouseID = item.WarehouseID;
                boxSearch.WarehouseName = item.WarehouseName;
                boxSearch.Year = item.Year;
                bosSearchList.Add(boxSearch);

            }


            ViewBag.AllBoxes = bosSearchList;
            model.ListItems = finalList;





            return View(model);

        }


        public bool IsDate(Object obj)
        {
            string strDate = obj.ToString();
            try
            {
                DateTime dt = DateTime.Parse(strDate);
                if ((dt.Month != System.DateTime.Now.Month) || (dt.Day < 1 && dt.Day > 31) || dt.Year != System.DateTime.Now.Year)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }

        public ActionResult BoxSearch(int? page)
        {
            int pageSize = 50;
            int pageNumber = (page ?? 1);

            BoxSearchViewModel model = new BoxSearchViewModel();
            ViewBag.ClientList = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.AllIncluding().OrderBy(o => o.DepartmentName);
            // model.ListItems = repo.AssignBoxRepository.GetAll();

            // List<BoxSearchListViewModel> bosSearchList = new List<BoxSearchListViewModel>();



            IPagedList<AssignBox> pagedListMother = repo.AssignBoxRepository.GetAllPlist(pageNumber, pageSize);
            model.SearchResults = pagedListMother;
            ViewBag.flag = 0;
            return View(model);
        }

        [HttpGet]
        public ActionResult BoxSearch(BoxSearchViewModel model)
        {

            int pageIndex = model.Page ?? 1;
            int pageSize = 50;
            int pageNumber = pageIndex;
            //}

            long _clientID = model.ClientID;
            long _deptID;
            if (model.DepartmentID.HasValue && model.DepartmentID.Value!=-1)
            {
                _deptID = model.DepartmentID.Value;
            }
            else
            {
                _deptID = 0;
            }
            DateTime _startDate = DateTime.Now;
            DateTime _endDate = DateTime.Now;

            if (model.StartDate != DateTime.MinValue)
            {
                _startDate = model.StartDate;
            }
            if (model.EndDate != DateTime.MinValue)
            {
                _endDate = model.EndDate;
            }


            string _boxName = model.BoxName;
            string _boxNo = model.BoxNo;


            //IPagedList<AssignBox> listofAllBoxes = new List<AssignBox>();
            List<AssignBox> finalList = new List<AssignBox>();
            IPagedList<AssignBox> listofAllBoxes =listofAllBoxes = repo.AssignBoxRepository.GetByClientIdAndDepartmentIdPList(_clientID, _deptID, model.StartDate, model.EndDate,_boxName,_boxNo, pageNumber, pageSize);
            








            foreach (AssignBox item in listofAllBoxes)
            {
                if (_startDate.ToShortDateString() != DateTime.Now.ToShortDateString() && _endDate.ToShortDateString() != DateTime.Now.ToShortDateString())
                {

                    if (item.AssignDate.Date >= _startDate.Date && item.AssignDate.Date <= _endDate.Date)
                    {

                        List<BoxLocation> _boxLocationList = repo.BoxLocationRepository.GetListByAssignBoxId(item.AssignBoxId);
                        item.BoxLocation = _boxLocationList;


                      //  finalList.Add(item);
                    }


                }

                else
                {
                    List<BoxLocation> _boxLocationList = repo.BoxLocationRepository.GetListByAssignBoxId(item.AssignBoxId);
                    item.BoxLocation = _boxLocationList;


                   
                    //finalList.Add(item);
                }


            }

            

            ViewBag.ClientList = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(model.ClientID).OrderBy(o => o.DepartmentName); 
            ViewBag.flag = 1;
            model.SearchResults = listofAllBoxes;

            ViewBag.BoxNo = model.BoxNo;
            ViewBag.BoxName = model.BoxName;

            return View(model);


        }

        public ActionResult BoxXLSearch()
        {
            BoxSearchViewModel model = new BoxSearchViewModel();
            ViewBag.ClientList = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.AllIncluding();
            model.ListItems = repo.AssignBoxRepository.GetAllBox();

            List<BoxSearchListViewModel> bosSearchList = new List<BoxSearchListViewModel>();

            List<AssignBox> assignBoxList = repo.AssignBoxRepository.GetAllBox();

            foreach (AssignBox item in assignBoxList)
            {
                BoxSearchListViewModel boxSearch = new BoxSearchListViewModel();
                boxSearch.AssignBoxId = item.AssignBoxId;
                boxSearch.AssignDate = item.AssignDate;
                boxSearch.BarCodeId = item.BarCodeId;
                boxSearch.BoxLocation = item.BoxLocation;
                boxSearch.BoxName = item.BoxName;
                boxSearch.BoxNo = item.BoxNo;
                boxSearch.CurrentStatus = item.CurrentStatus;
                boxSearch.DestructionPeriod = item.DestructionPeriod;
                boxSearch.IsLatest = item.IsLatest;
                boxSearch.Item = item.Item;
                boxSearch.ItemId = item.ItemId;
                boxSearch.ItemIds = item.ItemIds;
                boxSearch.TransmittalIN = item.TransmittalIN;
                boxSearch.TransmittalINId = item.TransmittalINId;
                boxSearch.TransmittalINStatus = item.TransmittalINStatus;
                boxSearch.TransmittalINStatusId = item.TransmittalINStatusId;
                boxSearch.TrOutDate = repo.AssignBoxTrOUTRepository.GetLatestByItemId(item.ItemId);
                boxSearch.Warehouse = item.Warehouse;
                boxSearch.WarehouseID = item.WarehouseID;
                boxSearch.WarehouseName = item.WarehouseName;
                boxSearch.Year = item.Year;
                bosSearchList.Add(boxSearch);

            }


            ViewBag.AllBoxes = bosSearchList;
            ViewBag.flag = 0;
            return View(model);
        }

        [HttpPost]
        public ActionResult BoxXLSearch(HttpPostedFileBase filename)
        {
            BoxSearchViewModel model = new BoxSearchViewModel();
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

                DataTable xlTable = Import_To_Grid(path, Extension);

                List<AssignBox> finalList = new List<AssignBox>();
                foreach (DataRow dr in xlTable.Rows)
                {
                    string boxName = dr["Box Name"].ToString();
                    string boxNo = dr["Box No"].ToString();
                    string year = dr["Year"].ToString();
                    string clientName = dr["Client Name"].ToString();
                    string departmentName = dr["Department Name"].ToString();

                    Client objclient = new Client();
                    objclient = repo.ClientRepository.GetByClientName(clientName);
                    Department objdepartment = new Department();
                    objdepartment = repo.DepartmentRepository.GetByDeptName(departmentName);
                    AssignBox objAssignBox = new AssignBox();
                    //objAssignBox = repo.AssignBoxRepository.GetByClientIdAndDepartmentId(objclient.ClientID,objdepartment.DepartmentID);
                    //Item objItem = repo.ItemRepository.

                    if (objdepartment.Client.ClientName.Trim() == clientName)
                    //if (objclient != null && objdepartment != null)
                    {
                        AssignBox assignBox = new AssignBox();
                        if (year == "" || year == null || year == string.Empty)
                        {
                            assignBox = repo.AssignBoxRepository.GetByClientIdandDeptIDwithoutYear(objdepartment.DepartmentID, objdepartment.ClientID, boxName.Trim(), boxNo.Trim());
                        }
                        else
                        {
                            assignBox = repo.AssignBoxRepository.GetByClientIdandDeptID(objdepartment.DepartmentID, objdepartment.ClientID, boxName.Trim(), boxNo.Trim(), year.Trim());
                        }

                        if (assignBox != null)
                        {
                            Item _item = new Item();

                            _item = repo.ItemRepository.Find(assignBox.ItemId);
                            List<BoxLocation> _boxLocationList = repo.BoxLocationRepository.GetListByAssignBoxId(assignBox.AssignBoxId);
                            assignBox.BoxLocation = _boxLocationList;
                            finalList.Add(assignBox);
                        }

                    }


                    //dept not found


                }
                List<BoxSearchListViewModel> bosSearchList = new List<BoxSearchListViewModel>();


                foreach (AssignBox item in finalList)
                {
                    BoxSearchListViewModel boxSearch = new BoxSearchListViewModel();
                    boxSearch.AssignBoxId = item.AssignBoxId;
                    boxSearch.AssignDate = item.AssignDate;
                    boxSearch.BarCodeId = item.BarCodeId;
                    boxSearch.BoxLocation = item.BoxLocation;
                    boxSearch.BoxName = item.BoxName;
                    boxSearch.BoxNo = item.BoxNo;
                    boxSearch.CurrentStatus = item.CurrentStatus;
                    boxSearch.DestructionPeriod = item.DestructionPeriod;
                    boxSearch.IsLatest = item.IsLatest;
                    boxSearch.Item = item.Item;
                    boxSearch.ItemId = item.ItemId;
                    boxSearch.ItemIds = item.ItemIds;
                    boxSearch.TransmittalIN = item.TransmittalIN;
                    boxSearch.TransmittalINId = item.TransmittalINId;
                    boxSearch.TransmittalINStatus = item.TransmittalINStatus;
                    boxSearch.TransmittalINStatusId = item.TransmittalINStatusId;
                    boxSearch.TrOutDate = repo.AssignBoxTrOUTRepository.GetLatestByItemId(item.ItemId);
                    boxSearch.Warehouse = item.Warehouse;
                    boxSearch.WarehouseID = item.WarehouseID;
                    boxSearch.WarehouseName = item.WarehouseName;
                    boxSearch.Year = item.Year;
                    bosSearchList.Add(boxSearch);

                }
                try
                {
                    System.IO.File.Delete(path);
                }
                catch (Exception e)
                {
                }

                model.ListItems = finalList;
                ViewBag.AllBoxes = bosSearchList;
            }
            // redirect back to the index action to show the form once again
            return View(model);
        }


        public ActionResult FileXLSearch()
        {
            BoxSearchViewModel model = new BoxSearchViewModel();
            ViewBag.ClientList = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.AllIncluding();
            //model.ListItems = repo.AssignBoxRepository.GetAllFile();
            model.ListItems = repo.AssignBoxRepository.GetCustomeFileForPageLoad(10);

            List<FileSearchListViewModel> bosSearchList = new List<FileSearchListViewModel>();

            //List<AssignBox> assignBoxList = repo.AssignBoxRepository.GetAllFile();
            List<AssignBox> assignBoxList = repo.AssignBoxRepository.GetCustomeFileForPageLoad(10);

            foreach (AssignBox item in assignBoxList)
            {
                FileSearchListViewModel boxSearch = new FileSearchListViewModel();
                boxSearch.AssignBoxId = item.AssignBoxId;
                boxSearch.AssignDate = item.AssignDate;
                boxSearch.BarCodeId = item.BarCodeId;
                boxSearch.BoxLocation = item.BoxLocation;

                boxSearch.BoxNo = item.BoxNo;
                boxSearch.BookNumber = item.BoxNameFile;
                boxSearch.FileNumber = item.FileNumber;
                boxSearch.ReferrenceNo = item.ReferrenceNo;
                boxSearch.RingNo = item.RingNo;
                boxSearch.AccountNo = item.AccountNo;


                boxSearch.CurrentStatus = item.CurrentStatus;
                boxSearch.DestructionPeriod = item.DestructionPeriod;
                boxSearch.IsLatest = item.IsLatest;
                boxSearch.Item = item.Item;
                boxSearch.ItemId = item.ItemId;
                boxSearch.ItemIds = item.ItemIds;
                boxSearch.TransmittalIN = item.TransmittalIN;
                boxSearch.TransmittalINId = item.TransmittalINId;
                boxSearch.TransmittalINStatus = item.TransmittalINStatus;
                boxSearch.TransmittalINStatusId = item.TransmittalINStatusId;
                boxSearch.TrOutDate = repo.AssignBoxTrOUTRepository.GetLatestByItemId(item.ItemId);
                boxSearch.Warehouse = item.Warehouse;
                boxSearch.WarehouseID = item.WarehouseID;
                boxSearch.WarehouseName = item.WarehouseName;
                boxSearch.Year = item.Year;
                bosSearchList.Add(boxSearch);

            }


            ViewBag.AllBoxes = bosSearchList;
            ViewBag.flag = 0;
            return View(model);
        }

        [HttpPost]
        public ActionResult FileXLSearch(HttpPostedFileBase filename)
        {

            BoxSearchViewModel model = new BoxSearchViewModel();
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

                DataTable xlTable = Import_To_Grid(path, Extension);

                List<AssignBox> finalList = new List<AssignBox>();
                foreach (DataRow dr in xlTable.Rows)
                {
                    string boxNo = dr["BoxNo"].ToString();

                    string bookNo = dr["BoxName"].ToString();
                    string fileNo = dr["FileNumber"].ToString();
                    string refNo = dr["ReferrenceNumber"].ToString();
                    string ringNo = dr["RingNumber"].ToString();
                    string accNo = dr["AccountNumber"].ToString();

                    string year = dr["Year"].ToString();
                    string clientName = dr["ClientName"].ToString();
                    string departmentName = dr["Department"].ToString();

                    Client objclient = new Client();
                    objclient = repo.ClientRepository.GetByClientName(clientName);
                    Department objdepartment = new Department();
                    objdepartment = repo.DepartmentRepository.GetByDeptName(departmentName);
                    AssignBox objAssignBox = new AssignBox();
                    //objAssignBox = repo.AssignBoxRepository.GetByClientIdAndDepartmentId(objclient.ClientID,objdepartment.DepartmentID);
                    //Item objItem = repo.ItemRepository.

                    if (objdepartment.Client.ClientName.Trim() == clientName)
                    //if (objclient != null && objdepartment != null)
                    {
                        AssignBox assignBox = new AssignBox();
                        if (year == "" || year == null || year == string.Empty)
                        {
                            assignBox = repo.AssignBoxRepository.GetByClientIdandDeptIDwithoutYearFile(objdepartment.DepartmentID, objdepartment.ClientID, boxNo.Trim(), bookNo.Trim(), fileNo.Trim(), refNo.Trim(), ringNo.Trim(), accNo.Trim());
                        }
                        else
                        {
                            assignBox = repo.AssignBoxRepository.GetByClientIdandDeptIDFile(objdepartment.DepartmentID, objdepartment.ClientID, boxNo.Trim(), bookNo.Trim(), fileNo.Trim(), refNo.Trim(), ringNo.Trim(), accNo.Trim(),year.Trim());
                        }

                        if (assignBox != null)
                        {
                            Item _item = new Item();

                            _item = repo.ItemRepository.Find(assignBox.ItemId);
                            List<BoxLocation> _boxLocationList = repo.BoxLocationRepository.GetListByAssignBoxId(assignBox.AssignBoxId);
                            assignBox.BoxLocation = _boxLocationList;
                            finalList.Add(assignBox);
                        }

                    }


                    //dept not found


                }
                List<FileSearchListViewModel> bosSearchList = new List<FileSearchListViewModel>();


                foreach (AssignBox item in finalList)
                {
                    FileSearchListViewModel boxSearch = new FileSearchListViewModel();
                    boxSearch.AssignBoxId = item.AssignBoxId;
                    boxSearch.AssignDate = item.AssignDate;
                    boxSearch.BarCodeId = item.BarCodeId;
                    boxSearch.BoxLocation = item.BoxLocation;

                    boxSearch.BoxNo = item.BoxNo;
                    boxSearch.BookNumber = item.BoxNameFile;
                    boxSearch.FileNumber = item.FileNumber;
                    boxSearch.ReferrenceNo = item.ReferrenceNo;
                    boxSearch.RingNo = item.RingNo;
                    boxSearch.AccountNo = item.AccountNo;


                    boxSearch.CurrentStatus = item.CurrentStatus;
                    boxSearch.DestructionPeriod = item.DestructionPeriod;
                    boxSearch.IsLatest = item.IsLatest;
                    boxSearch.Item = item.Item;
                    boxSearch.ItemId = item.ItemId;
                    boxSearch.ItemIds = item.ItemIds;
                    boxSearch.TransmittalIN = item.TransmittalIN;
                    boxSearch.TransmittalINId = item.TransmittalINId;
                    boxSearch.TransmittalINStatus = item.TransmittalINStatus;
                    boxSearch.TransmittalINStatusId = item.TransmittalINStatusId;
                    boxSearch.TrOutDate = repo.AssignBoxTrOUTRepository.GetLatestByItemId(item.ItemId);
                    boxSearch.Warehouse = item.Warehouse;
                    boxSearch.WarehouseID = item.WarehouseID;
                    boxSearch.WarehouseName = item.WarehouseName;
                    boxSearch.Year = item.Year;
                    bosSearchList.Add(boxSearch);

                }
                try
                {
                    System.IO.File.Delete(path);
                }
                catch (Exception e)
                {
                }

                model.ListItems = finalList;
                ViewBag.AllBoxes = bosSearchList;
            }
            // redirect back to the index action to show the form once again
            return View(model);
        }

        private DataTable Import_To_Grid(string FilePath, string Extension)
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
                SheetName = dtExcelSchema.Rows[1]["TABLE_NAME"].ToString();
                cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";

                oda.SelectCommand = cmdExcel;

                oda.Fill(dt);
            }
            //connExcel.Close();
            connExcel.Close();




            return dt;

        }

        public ActionResult ViewBoxHistory(long id)
        {
            AssignBox box = new AssignBox();
            box = repo.AssignBoxRepository.Find(id);


            Item item = new Item();
            item = repo.ItemRepository.Find(box.ItemId);
            ViewBag.Client = repo.ClientRepository.Find(item.ClientID).ClientName;
            ViewBag.Dept = repo.DepartmentRepository.Find(item.DepartmentID).DepartmentName;
            ViewBag.BoxName = item.ItemName;
            ViewBag.BoxNo = item.BoxNo;
            ViewBag.Year = item.Year;

            if (item.ProjectId.HasValue)
            {
                if (item.ProjectId.Value != -1)
                {
                    ViewBag.Project = repo.ProjectRepository.Find(item.ProjectId.Value).ProjectName;
                }
                else
                {
                    ViewBag.Project = "None";
                }

            }
            else
            {
                ViewBag.Project = "None";
            }

            List<ChangeLocation> locationHistory = new List<ChangeLocation>();
            locationHistory = repo.ChangeLocationRepository.GetListByItemId(item.ItemId);
            return PartialView(locationHistory);
        }


        // public ActionResult PrintBoxList()

        public ActionResult UpdatePeriod(long ItemId, long clientID, long deptID)
        {
            Item item = repo.ItemRepository.Find(ItemId);
            ViewBag.ClientName = repo.ClientRepository.Find(item.ClientID).ClientName;
            ViewBag.Department = repo.DepartmentRepository.Find(item.DepartmentID).DepartmentName;
            if (item.ProjectId.HasValue)
            {
                if (item.ProjectId != -1)
                {
                    ViewBag.Project = repo.ProjectRepository.Find(item.ProjectId.Value).ProjectName;
                }
                else
                {
                    ViewBag.Project = "None";
                }
            }
            else
            {
                ViewBag.Project = "None";
            }

            ViewBag.BoxName = item.ItemName;
            if (item.DestructionPeriod.HasValue)
            {
                ViewBag.Period = item.DestructionPeriod.Value.ToShortDateString();
            }
            ViewBag.BoxNo = item.BoxNo;
            ViewBag.Year = item.Year;
            ViewBag.Id = item.ItemId;
            ViewBag.Flag = false;
            return View(item);
        }

        [HttpPost]
        public ActionResult UpdatePeriod(Item model)
        {
            Item item = new Item();
            if (model.ItemId != null)
            {
                item = repo.ItemRepository.Find(model.ItemId);


            }
            AuditTrailCP auditCp = new AuditTrailCP();
            auditCp.ChangingDate = DateTime.Now;
            auditCp.NewBoxYear = model.DestructionPeriod.ToString();
            if (item.DestructionPeriod.HasValue)
                auditCp.PreBoxYear = item.DestructionPeriod.Value.ToString();
            auditCp.UserName = User.Identity.Name;
            repo.AuditTrailCPRepository.InsertOrUpdate(auditCp);
            repo.AuditTrailCPRepository.Save();



            item.DestructionPeriod = model.DestructionPeriod;

            repo.ItemRepository.InsertOrUpdate(item);
            repo.ItemRepository.Save();

            AssignBox assignBox = new AssignBox();
            assignBox = repo.AssignBoxRepository.GetByItemID(item.ItemId);
            if (assignBox != null)
            {
                assignBox.DestructionPeriod = item.DestructionPeriod;

                repo.AssignBoxRepository.InsertOrUpdate(assignBox);
                repo.AssignBoxRepository.Save();
            }


            AssignBoxTrOUT assignBoxTrOUT = new AssignBoxTrOUT();
            assignBoxTrOUT = repo.AssignBoxTrOUTRepository.GetByItemId(item.ItemId);
            if (assignBoxTrOUT != null)
            {
                assignBoxTrOUT.DestructionPeriod = item.DestructionPeriod;

                repo.AssignBoxTrOUTRepository.InsertOrUpdate(assignBoxTrOUT);
                repo.AssignBoxTrOUTRepository.Save();
            }


            



            ViewBag.Flag = true;

            ViewBag.ClientName = repo.ClientRepository.Find(item.ClientID).ClientName;
            ViewBag.Department = repo.DepartmentRepository.Find(item.DepartmentID).DepartmentName;
            if (item.ProjectId.HasValue)
            {
                if (item.ProjectId != -1)
                {
                    ViewBag.Project = repo.ProjectRepository.Find(item.ProjectId.Value).ProjectName;
                }
                else
                {
                    ViewBag.Project = "None";
                }
            }
            else
            {
                ViewBag.Project = "None";
            }

            ViewBag.BoxName = item.ItemName;

            ViewBag.BoxNo = item.BoxNo;
            ViewBag.Year = item.Year;
            ViewBag.Id = item.ItemId;
            return View(item);
        }

        public ActionResult ViewLocationModal(long id)
        {

            AssignBox box = new AssignBox();
            box = repo.AssignBoxRepository.Find(id);
            List<BoxLocation> _boxLocationList = repo.BoxLocationRepository.GetListByAssignBoxId(box.AssignBoxId);
            box.BoxLocation = _boxLocationList;

            Item item = new Item();
            item = repo.ItemRepository.Find(box.ItemId);
            ViewBag.Client = repo.ClientRepository.Find(item.ClientID).ClientName;
            ViewBag.Dept = repo.DepartmentRepository.Find(item.DepartmentID).DepartmentName;

            if (item.ProjectId.HasValue)
            {
                if (item.ProjectId.Value != -1)
                {
                    ViewBag.Project = repo.ProjectRepository.Find(item.ProjectId.Value).ProjectName;
                }
                else
                {
                    ViewBag.Project = "None";
                }

            }
            else
            {
                ViewBag.Project = "None";
            }




            return PartialView("PartialLocationView", box);
        }
        //
        // GET: /BoxLocations/Delete/5


        public ActionResult ChangeLocationModal(long AssignBoxId, long BoxLocationId, string filter)
        {
            if (TempData["Flag"] != null)
                ViewBag.Flag = TempData["Flag"].ToString();
            BoxLocation location = repo.BoxLocationRepository.Find(BoxLocationId);

            ViewBag.WareHouseID = location.WareHouseID;
            ViewBag.FloorID = location.FloorID;
            ViewBag.ZoneID = location.ZoneID;
            ViewBag.TrainID = location.TrainID;
            ViewBag.RackID = location.RackID;
            ViewBag.LevelID = location.LevelID;
            ViewBag.HeightID = location.HeightID;
            ViewBag.ColumnID = location.ColumnID;
            ViewBag.RowID = location.RowID;
            ViewBag.PalletID = location.PalletId;
            if (location.RowID.HasValue)
            {
                ViewBag.preRowID = location.RowID.Value.ToString();
            }
            ViewBag.Filter = filter;

            ViewBag.AssignBoxID = AssignBoxId;
            ViewBag.BoxLocationId = BoxLocationId;

            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            ViewBag.PossibleFloors = repo.FloorRepository.allList(location.WareHouseID);
            ViewBag.PossibleZones = repo.ZoneRepository.allList(location.FloorID);
            ViewBag.PossibleTrains = repo.TrainRepository.allList(location.ZoneID);
            ViewBag.PossibleRacks = repo.RackRepository.allList(location.TrainID);
            ViewBag.PossibleLevels = repo.LevelRepository.allList(location.RackID);
            ViewBag.PossibleHeights = repo.HeightRepository.allList(location.LevelID);
            ViewBag.PossibleColumns = repo.ColumnRepository.allList(location.HeightID);
            ViewBag.PossibleRows = repo.RowRepository.allList(location.ColumnID);
            ViewBag.PossiblePallets = repo.PalletRepository.allList(location.ZoneID);

            return View("ChangeLocation", location);

        }

        [HttpPost]
        public ActionResult ChangeLocationModal(BoxLocation location, string preRowID, string Filter)
        {

            if (location.RowID != null && location.RowID != -1 && location.IsPallet == false)
            {
                if ((location.ColumnID != null && location.ColumnID != -1) && (location.FloorID != null && location.FloorID != -1) && (location.HeightID != null && location.HeightID != -1) && (location.LevelID != null && location.LevelID != -1) && (location.RackID != null && location.RackID != -1) && (location.TrainID != null && location.TrainID != -1) && (location.WareHouseID != null && location.WareHouseID != -1) && (location.ZoneID != null && location.ZoneID != -1))
                {
                    #region BoxLocation Update


                    location.Row = repo.RowRepository.Find(location.RowID.Value);
                    location.Column = repo.ColumnRepository.Find(location.ColumnID.Value);
                    location.Height = repo.HeightRepository.Find(location.HeightID.Value);
                    location.Level = repo.LevelRepository.Find(location.LevelID.Value);
                    location.Rack = repo.RackRepository.Find(location.RackID.Value);
                    location.Train = repo.TrainRepository.Find(location.TrainID.Value);
                    location.Zone = repo.ZoneRepository.Find(location.ZoneID);
                    location.Floor = repo.FloorRepository.Find(location.FloorID);
                    location.Warehouse = repo.WarehouseRepository.Find(location.WareHouseID);
                    location.IsLeatest = true;
                    location.CurrentStatus = "In WareHouse";

                    repo.BoxLocationRepository.InsertOrUpdate(location);
                    repo.BoxLocationRepository.Save();

                    #endregion

                    #region Row Update

                    #region Previous Row Clear

                    if (preRowID != null && preRowID != "")
                    {
                        Row preRow = repo.RowRepository.Find(Convert.ToInt64(preRowID));

                        preRow.IsAssigned = false;
                        repo.RowRepository.InsertOrUpdate(preRow);
                        repo.RowRepository.Save();
                    }

                    #endregion
                    Row _row = new Row();
                    _row = repo.RowRepository.Find(location.RowID.Value);
                    _row.IsAssigned = true;

                    repo.RowRepository.InsertOrUpdate(_row);
                    repo.RowRepository.Save();



                    #endregion

                    #region AssignBox Update
                    AssignBox _assignBox = repo.AssignBoxRepository.Find(location.AssignBoxId);

                    //List<BoxLocation> _boxLocationList = repo.BoxLocationRepository.GetListByAssignBoxId(location.AssignBoxId);
                    //_assignBox.BoxLocation = _boxLocationList;
                    // _assignBox.TransmittalINStatusId = repo.TransmittalINStatusRepository.Find(Convert.ToInt64(EnumHelper.Status.Location_Assigned)).TransmittalINStatusId;

                    _assignBox.WarehouseName = location.Warehouse.WarehouseName;
                    _assignBox.WarehouseID = location.WareHouseID;

                    repo.AssignBoxRepository.InsertOrUpdate(_assignBox);
                    repo.AssignBoxRepository.Save();


                    #endregion

                    #region maintaining Audit-Trail

                    ChangeLocation chnglocation = new ChangeLocation();
                    chnglocation.AssignDate = DateTime.Now;
                    chnglocation.ItemId = _assignBox.ItemId;

                    chnglocation.Location = GetLocationString(location);
                    repo.ChangeLocationRepository.InsertOrUpdate(chnglocation);
                    repo.ChangeLocationRepository.Save();


                    #endregion


                    TempData["Flag"] = "0";

                    if (Filter == "1")
                    {
                        return RedirectToAction("BoxLocations");
                    }
                     if (Filter == "2")
                    {
                        return RedirectToAction("BoxLocationsAll");
                    }

                     if (Filter == "3")
                    {
                        return RedirectToAction("FileLocationsAll");
                    }

                    // TODO : Return Success Msg
                }
                else
                {
                    TempData["Flag"] = "3";
                    return RedirectToAction("ChangeLocationModal", new { AssignBoxId = location.AssignBoxId, BoxLocationId = location.BoxLocationId, filter = Filter });

                    // TODO : Return Validation;
                }
            }

            else if (location.IsPallet == true)
            {
                if ((location.WareHouseID != null && location.WareHouseID != -1) && (location.FloorID != null && location.FloorID != -1) && (location.ZoneID != null && location.ZoneID != -1))
                {

                    Pallet pallet = repo.PalletRepository.GetPalletList(false, location.WareHouseID, 1, repo.ItemRepository.Find(repo.AssignBoxRepository.Find(location.AssignBoxId).ItemId).ClientID);

                    if (pallet != null)
                    {


                        #region BoxLocation Update

                        location.Zone = repo.ZoneRepository.Find(location.ZoneID);
                        location.Floor = repo.FloorRepository.Find(location.FloorID);
                        location.Warehouse = repo.WarehouseRepository.Find(location.WareHouseID);

                        location.ColumnID = null;
                        location.HeightID = null;
                        location.IsPallet = true;
                        location.LevelID = null;
                        location.RackID = null;
                        location.RowID = null;
                        location.TrainID = null;


                        location.PalletId = pallet.PalletId;
                        location.Pallet = pallet;

                        location.IsLeatest = true;
                        location.CurrentStatus = "In WareHouse";

                        repo.BoxLocationRepository.InsertOrUpdate(location);
                        repo.BoxLocationRepository.Save();

                        #endregion

                        #region AssignBox Update
                        AssignBox _assignBox = repo.AssignBoxRepository.Find(location.AssignBoxId);

                        //List<BoxLocation> _boxLocationList = repo.BoxLocationRepository.GetListByAssignBoxId(location.AssignBoxId);
                        //_assignBox.BoxLocation = _boxLocationList;
                        //_assignBox.TransmittalINStatusId = repo.TransmittalINStatusRepository.Find(Convert.ToInt64(EnumHelper.Status.Location_Assigned)).TransmittalINStatusId;
                        _assignBox.WarehouseName = location.Warehouse.WarehouseName;
                        _assignBox.WarehouseID = location.WareHouseID;
                        repo.AssignBoxRepository.InsertOrUpdate(_assignBox);
                        repo.AssignBoxRepository.Save();


                        #endregion

                        #region maintaining Audit-Trail

                        ChangeLocation chnglocation = new ChangeLocation();
                        chnglocation.AssignDate = DateTime.Now;
                        chnglocation.ItemId = _assignBox.ItemId;

                        chnglocation.Location = GetLocationStringForPallet(location);
                        repo.ChangeLocationRepository.InsertOrUpdate(chnglocation);
                        repo.ChangeLocationRepository.Save();


                        #endregion


                        TempData["Flag"] = "1";

                        if (Filter == "1")
                        {
                            return RedirectToAction("BoxLocations");
                        }
                        if (Filter == "2")
                        {
                            return RedirectToAction("BoxLocationsAll");
                        }

                        if (Filter == "3")
                        {
                            return RedirectToAction("FileLocationsAll");
                        }
                        // TODO : return Success Msg
                    }

                    else
                    {
                        TempData["Flag"] = "6";
                        return RedirectToAction("ChangeLocationModal", new { AssignBoxId = location.AssignBoxId, BoxLocationId = location.BoxLocationId, filter = Filter });

                        //TODO : Return Pallent Null msg
                    }

                }
                else
                {
                    TempData["Flag"] = "4";
                    return RedirectToAction("ChangeLocationModal", new { AssignBoxId = location.AssignBoxId, BoxLocationId = location.BoxLocationId, filter = Filter });
                    // TODO : Return Validation
                }
            }
            else
            {
                TempData["Flag"] = "5";
                return RedirectToAction("ChangeLocationModal", new { AssignBoxId = location.AssignBoxId, BoxLocationId = location.BoxLocationId, filter = Filter });



                //TODO : Return Validation
            }

            return null;
        }

        public ActionResult Delete(long id)
        {
            BoxLocation boxlocation = repo.BoxLocationRepository.Find(id);
            return View(boxlocation);
        }

        //
        // POST: /BoxLocations/Delete/5



        #region Report

        public ActionResult BoxInOut()
        {

            ViewBag.ClientList = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.AllIncluding();
            BoxInOutSearchFilter model = new BoxInOutSearchFilter();
            return View(model);
        }


        [HttpPost]
        public ActionResult BoxInOut(BoxInOutSearchFilter model)
        {
            DateTime monthStart=model.Month;
            DateTime monthEnd = model.Month.AddMonths(1).AddDays(-1);


            #region BoxIN
            if (model.Service == 1)
            {

                List<TransmittalIN> trINList = new List<TransmittalIN>();
                List<AssignBox> assignBoxList = new List<AssignBox>();
                assignBoxList = repo.AssignBoxRepository.GetByClientIDandDtIDanMmonthStartandMonthEnd(model.ClientID, model.DepartmentID, monthStart, monthEnd); 
                //trINList = repo.TransmittalINRepository.GetByClientIDandDeptIDandDateRange(model.ClientID, model.DepartmentID, monthStart, monthEnd);
                List<BoxINReportViewModel> viewModelList = new List<BoxINReportViewModel>();

                foreach (AssignBox item in assignBoxList)
                {
                    BoxINReportViewModel ob = new BoxINReportViewModel();
                        ob.BoxName = item.Item.ItemName;
                        ob.BoxNo = item.BoxNo;
                        ob.Date = item.AssignDate.ToShortDateString();
                        ob.Department = item.Item.Department.DepartmentName;
                        ob.Year = item.Year;
                        viewModelList.Add(ob);

                    

                }

                TempData["ViewModelList"] = viewModelList;
                Session["BoxINReport"] = viewModelList;
                return RedirectToAction("BoxINReport", "Reports", new { clientID = model.ClientID, month = model.Month, format = model.ReportType.Value });
            }

            #endregion

            #region BoxOUT

            else
            {

                List<TransmittalOUT> trOUTList = new List<TransmittalOUT>();
                List<AssignBoxTrOUT> assignBoxList = new List<AssignBoxTrOUT>();
                assignBoxList = repo.AssignBoxTrOUTRepository.GetByClientIDandDtIDanMmonthStartandMonthEnd(model.ClientID, model.DepartmentID, monthStart, monthEnd); 

                trOUTList = repo.TransmittalOUTRepository.GetByClientIDandDeptIDandDateRange(model.ClientID, model.DepartmentID, monthStart, monthEnd);
                List<BoxINReportViewModel> viewModelList = new List<BoxINReportViewModel>();


                foreach (TransmittalOUT trOUT in trOUTList)
                {
                    foreach (Item item in trOUT.Items)
                    {
                        BoxINReportViewModel ob = new BoxINReportViewModel();
                        ob.BoxName = item.ItemName;
                        ob.BoxNo = item.BoxNo;
                        ob.Date = trOUT.TransmittalDate.ToShortDateString();
                        ob.Department = item.Department.DepartmentName;
                        ob.Year = item.Year;
                        viewModelList.Add(ob);

                    }

                }

                TempData["ViewModelList"] = viewModelList;
                Session["BoxOUTReport"] = viewModelList;
                return RedirectToAction("BoxOUTReport", "Reports", new { clientID = model.ClientID,month = model.Month, format = model.ReportType.Value  });

            }

            #endregion
            return null;
        }


        public ActionResult BoxPending()
        {
            ViewBag.ClientList = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.AllIncluding();
            DelPendingSearchFilter model = new DelPendingSearchFilter();
            return View(model);
        }

        [HttpPost]
        public ActionResult BoxPending(DelPendingSearchFilter model)
        {
            DateTime monthStart = model.Month;
            DateTime monthEnd = model.Month.AddMonths(1).AddDays(-1);

            List<DelPendingBoxModel> list = new List<DelPendingBoxModel>();

            list=repo.DelPendingBoxModelRepository.GetByClientIDandDeptIDandDateRange(model.ClientID, model.DepartmentID, monthStart, monthEnd);

            TempData["ViewModelList"] = list;
            Session["BoxPending"] = list;
            return RedirectToAction("BoxPending", "Reports", new { clientID = model.ClientID, month = model.Month, format = model.ReportType.Value });

        }

        #endregion



        #region Box Info By Barcode

        public ActionResult BoxInfoByBarcode()
        {
            return View();
        }


        [HttpPost]
        public ActionResult BoxInfoByBarcode(BarcodeInfoSearchViewModel model)
        {
            if(!string.IsNullOrEmpty(model.BarcodeID))
            {

                try
                {
                    long itemId = Convert.ToInt64(model.BarcodeID) / 5000;

                    AssignBox _assignBox = new AssignBox();
                    _assignBox = repo.AssignBoxRepository.GetByItemID(itemId);

                    if (_assignBox != null)
                    {
                        ViewBag.flag = "1";

                        BarcodeInformationViewModel outputModel = new BarcodeInformationViewModel();

                        outputModel.BoxName = _assignBox.BoxName;
                        outputModel.BoxNo = _assignBox.BoxNo;
                        outputModel.Year = _assignBox.Year;
                        outputModel.Client = repo.ItemRepository.Find(_assignBox.ItemId).Client.ClientName; ;
                        outputModel.Dept = repo.ItemRepository.Find(_assignBox.ItemId).Department.DepartmentName;



                        if (_assignBox.DestructionPeriod.HasValue)
                            outputModel.DestructionDate = String.Format("{0:dddd, MMMM d, yyyy}", _assignBox.DestructionPeriod.Value);  
                         else
                            outputModel.DestructionDate = "N/A";

                        outputModel.TransmittalStatus=repo.TransmittalINStatusRepository.Find(_assignBox.TransmittalINStatusId).StatusName;

                        if(_assignBox.CurrentStatus==1)
                            outputModel.CurrentStatus="Box available at warehouse";
                        else
                            outputModel.CurrentStatus = "Box is not available at warehouse";

                        ViewBag.outputModel = outputModel;

                        BoxLocation _boxLocation = new BoxLocation();
                        _boxLocation = repo.BoxLocationRepository.GetByAssignBoxId(_assignBox.AssignBoxId);

                        ViewBag.BoxLocation = _boxLocation;

                        List<ChangeLocation> _locationHistory = new List<ChangeLocation>();
                        _locationHistory = repo.ChangeLocationRepository.GetListByItemId(_assignBox.ItemId);
                        ViewBag.LocationHistory = _locationHistory;

                    }
                    else
                    {

                        ViewBag.flag = "0";
                    }

                }
                catch
                {
                    ViewBag.flag = "0";
                }
            }

            else
            {
                ViewBag.flag="0";
            }

            return View(model);
        }

        #endregion



        #region Box Destruction Section

        [HttpGet]
        public ActionResult BoxDestruction(BoxDestructionViewModel model)
        {

            int pageIndex = model.Page ?? 1;
            int pageSize = 10;
            int pageNumber = pageIndex;
            //}

            long _clientID = model.ClientID;
            long _deptID;
            if (model.DepartmentID.HasValue && model.DepartmentID.Value != -1)
            {
                _deptID = model.DepartmentID.Value;
            }
            else
            {
                _deptID = 0;
            }
            DateTime _startDate = DateTime.Now;
            DateTime _endDate = DateTime.Now;

            if (model.StartDate != DateTime.MinValue)
            {
                _startDate = model.StartDate;
            }
            if (model.EndDate != DateTime.MinValue)
            {
                _endDate = model.EndDate;
            }


            string _boxName = model.BoxName;
            string _boxNo = model.BoxNo;


            //IPagedList<AssignBox> listofAllBoxes = new List<AssignBox>();
            List<AssignBox> finalList = new List<AssignBox>();
            IPagedList<AssignBox> listofAllBoxes = listofAllBoxes = repo.AssignBoxRepository.GetByClientIdAndDepartmentIdPList(_clientID, _deptID, model.StartDate, model.EndDate, _boxName, _boxNo, pageNumber, pageSize);


            foreach (AssignBox item in listofAllBoxes)
            {
                if (_startDate.ToShortDateString() != DateTime.Now.ToShortDateString() && _endDate.ToShortDateString() != DateTime.Now.ToShortDateString())
                {

                    if (item.AssignDate.Date >= _startDate.Date && item.AssignDate.Date <= _endDate.Date)
                    {

                        List<BoxLocation> _boxLocationList = repo.BoxLocationRepository.GetListByAssignBoxId(item.AssignBoxId);
                        item.BoxLocation = _boxLocationList;


                        //  finalList.Add(item);
                    }


                }

                else
                {
                    List<BoxLocation> _boxLocationList = repo.BoxLocationRepository.GetListByAssignBoxId(item.AssignBoxId);
                    item.BoxLocation = _boxLocationList;



                    //finalList.Add(item);
                }


            }



            ViewBag.ClientList = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(model.ClientID).OrderBy(o => o.DepartmentName);
            ViewBag.flag = 1;
            model.SearchResults = listofAllBoxes;

            ViewBag.BoxNo = model.BoxNo;
            ViewBag.BoxName = model.BoxName;

            return View(model);

        }


        #endregion


        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            BoxLocation boxlocation = repo.BoxLocationRepository.Find(id);
            repo.BoxLocationRepository.Delete(id);
            repo.BoxLocationRepository.Save();
            return RedirectToAction("Index");
        }

        public JsonResult GetClients()
        {
            var clients = repo.ClientRepository.GetAll();
            return Json(clients, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult ExportData()
        //{
        //    ViewBag.allClient = repo.ClientRepository.GetAll();

        //    return View();
        //}

        [HttpGet]
        public ActionResult ExportData(DateTime startDate, DateTime endDate , int clientId)
        {
            
            var userId = (Guid)System.Web.HttpContext.Current.Session["UserId"];
            var subDeptName = repo.DepartmentRepository.GetSubDepartmentName(userId);
            var user = repo.UserRepository.GetByUserId(userId);
            var clientUser = repo.ClientRepository.GetClientUserByUserName(user.Username);


            var data = repo.BoxLocationRepository.FindAllBoxData(startDate, endDate, clientId);

            // Define file name and path
            string fileName = $"DataExport_{DateTime.Now.ToString("yyyyMMdd")}.xlsx";
            string relativePath = "~/Content/BoxInfoXL/";
            string folderPath = Server.MapPath(relativePath);

            // Ensure the directory exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = Path.Combine(folderPath, fileName);

            // Remove existing file if exists
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            // Use the ACE.OLEDB provider to work with Excel
            using (var excelConnection = new OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};Extended Properties='Excel 12.0 Xml;HDR=YES;';"))
            {
                excelConnection.Open();

                // Create a new Excel sheet and define the schema
                string createTableQuery = @"
            CREATE TABLE [Sheet1] (
            TransmittalNo NVARCHAR(255),
            TransmittalDate DATE,
            BoxNo NVARCHAR(255),
            ItemName NVARCHAR(255),
            Status NVARCHAR(255),
            BoxDestruction DATE,
            Description NVARCHAR(255),
            Legalhold NVARCHAR(3),
            DepartmentName NVARCHAR(255),
            SubDeptName NVARCHAR(255)
            )";

                using (var cmdCreateTable = new OleDbCommand(createTableQuery, excelConnection))
                {
                    cmdCreateTable.ExecuteNonQuery();
                }

                // Insert data into the Excel sheet
                foreach (var transmittal in data)
                {
                    foreach (var item in transmittal.Items)
                    {
                        string insertQuery = @"
                INSERT INTO [Sheet1] 
                (TransmittalNo, TransmittalDate, BoxNo, ItemName, Status, BoxDestruction, Description, Legalhold,DepartmentName,SubDeptName) 
                VALUES (?, ?, ?, ?, ?, ?, ?, ?,?,?)";

                        using (var cmdInsert = new OleDbCommand(insertQuery, excelConnection))
                        {
                            // Handle null values appropriately
                            cmdInsert.Parameters.AddWithValue("@TransmittalNo", string.IsNullOrEmpty(transmittal.TransmittalNo) ? DBNull.Value : (object)transmittal.TransmittalNo);
                            cmdInsert.Parameters.AddWithValue("@TransmittalDate", transmittal.TransmittalDate);
                            cmdInsert.Parameters.AddWithValue("@BoxNo", string.IsNullOrEmpty(item.BoxNo) ? DBNull.Value : (object)item.BoxNo);
                            cmdInsert.Parameters.AddWithValue("@ItemName", string.IsNullOrEmpty(item.ItemName) ? DBNull.Value : (object)item.ItemName);
                            cmdInsert.Parameters.AddWithValue("@Status", string.IsNullOrEmpty(transmittal.TransmittalStatus) ? DBNull.Value : (object)transmittal.TransmittalStatus);
                            cmdInsert.Parameters.AddWithValue("@BoxDestruction", item.BoxDestruction == null ? (object)DBNull.Value : item.BoxDestruction);
                            cmdInsert.Parameters.AddWithValue("@Description", string.IsNullOrEmpty(item.Description) ? DBNull.Value : (object)item.Description);
                            cmdInsert.Parameters.AddWithValue("@Legalhold", string.IsNullOrEmpty(item.Legalhold) ? DBNull.Value : (object)item.Legalhold);
                            cmdInsert.Parameters.AddWithValue("@DepartmentName", string.IsNullOrEmpty(item.DepartmentName) ? DBNull.Value : (object)item.DepartmentName);
                            cmdInsert.Parameters.AddWithValue("@SubDeptName", string.IsNullOrEmpty(item.SubDeptName) ? DBNull.Value : (object)item.SubDeptName);

                            cmdInsert.ExecuteNonQuery();
                        }
                    }
                }
            }

            // Return the generated Excel file for download
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        protected override void Dispose(bool disposing)
        {
            //if (disposing) {
            //    context.Dispose();
            //}
            base.Dispose(disposing);
        }
    }
}