using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;
using System.Web.Security;
using System.Configuration;

namespace WareHouseMVC.Controllers
{
    public class HomeController : Controller
    {

        UnitOfWork repo = new UnitOfWork();

        public ActionResult Index()
        {

            SessionManager.SessionValueItem.BarcodeServerAddress = Server.MapPath("~/Content/Images/" + ConfigurationManager.AppSettings["BarCodes"].ToString()) ;//


          //  FormsAuthentication.SignOut();

            //Roles.CreateRole("Client");
            //Roles.AddUserToRole("brac", "Client");
            if (System.Web.HttpContext.Current.User.IsInRole("Client"))
            {
                Client _client = new Client();

                MembershipUser user = Membership.GetUser(System.Web.HttpContext.Current.User.Identity.Name);

                //_client = repo.ClientRepository.GetByUserID(new Guid(user.ProviderUserKey.ToString()));

                System.Web.HttpContext.Current.Session["ClientId"] = _client.ClientID;
                return RedirectToAction("ClientBoxes", "Clients");
            }
            else
            {





                ViewBag.Message = "Welcome to ASP.NET MVC!";
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("LogOn", "Users");
                }
                else
                {

                    List<Warehouse> wHList = repo.WarehouseRepository.GetAllList();
                    ViewBag.TotWareHouse = wHList.Count;

                    string[] wName = new string[wHList.Count];
                    string[] boxNo = new string[wHList.Count];

                    int i = 0;
                    foreach (var item in wHList)
                    {
                        int totalBox = 0;
                        string tempwName = string.Empty;
                        string tempBxNo = string.Empty;

                        int rowList = repo.RowRepository.GetByStatusandWIDCount(item.WarehouseID);
                        totalBox = totalBox + rowList;

                        int boxlocation = repo.BoxLocationRepository.GetBywIDandPalletStatusCount(item.WarehouseID);
                        totalBox = totalBox + boxlocation;

                        tempwName = item.WarehouseName;
                        tempBxNo = totalBox.ToString();

                        wName[i] = tempwName;
                        boxNo[i] = tempBxNo;
                        i++;


                    }

                    ViewBag.wName = wName;
                    ViewBag.boxNo = boxNo;




                    string[] trInCount = new string[10];
                    string[] trOutCount = new string[10];


                    for (int j = 1; j <= 10; j++)
                    {
                        int flag = 10 - i;

                        DateTime flagDate = DateTime.Now.AddDays(-flag);
                        List<TransmittalIN> trInList = new List<TransmittalIN>();
                        trInList = repo.TransmittalINRepository.GetAllByDate(flagDate);
                        trInCount[j - 1] = trInList.Count.ToString();



                        List<TransmittalOUT> trOutList = new List<TransmittalOUT>();
                        trOutList = repo.TransmittalOUTRepository.GetAllByDate(flagDate);
                        trOutCount[j - 1] = trOutList.Count.ToString();
                    }
                    //Muna

                    List<Item> wItemDestructionPeriod = repo.ItemRepository.GetByNextOneMonth();
                    ViewBag.DestructionPeriod = wItemDestructionPeriod;
                    List<Item> wItemDestructionPeriodNull = repo.ItemRepository.GetByNextOneMonthNull();
                    ViewBag.DestructionPeriodNULL = wItemDestructionPeriodNull;

                    //
                    ViewBag.StartDate = DateTime.Now.AddDays(-9).ToShortDateString();
                    ViewBag.EndDate = DateTime.Now.ToShortDateString();
                    ViewBag.TrInCount = trInCount;
                    ViewBag.TrOutCount = trOutCount;


                    List<TransmittalIN> allTransmittalInList = new List<TransmittalIN>();
                    allTransmittalInList = repo.TransmittalINRepository.GetAllLocationNA();
                    ViewBag.allTransmittalInList = allTransmittalInList;

                    List<DelPendingBoxModel> allDeliveryPendingList = new List<DelPendingBoxModel>();
                    allDeliveryPendingList = repo.DelPendingBoxModelRepository.GetAllDeliveryPending();
                    ViewBag.allDeliveryPendingList = allDeliveryPendingList;

                    List<DelPendingBoxModelFile> allDeliveryPendingFileList = new List<DelPendingBoxModelFile>();
                    allDeliveryPendingFileList = repo.DelPendingBoxModelFileRepository.GetAllDeliveryPendingList();
                    ViewBag.allDeliveryPendingListFile = allDeliveryPendingFileList;

                }
            }
            return View();
        }

        public ActionResult IndexForClient()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogOn", "Users");
            }
            else
            {

            }

            return View();

        }

        public ActionResult RenderWareHouse()
        {
            List<Warehouse> wareHouseList = new List<Warehouse>();
            List<WareHousePartialViewModel> _viewModelList = new List<WareHousePartialViewModel>();

            wareHouseList = repo.WarehouseRepository.GetAllList();

            foreach (Warehouse item in wareHouseList)
            {
                WareHousePartialViewModel model = new WareHousePartialViewModel();
                model.ID = item.WarehouseID;
                model.WarehouseName = item.WarehouseName;
                _viewModelList.Add(model);

            }
            ViewBag.PossibleWarehouses = _viewModelList;

            User _user = repo.UserRepository.GetUserByUserName(HttpContext.User.Identity.Name);

            if (_user != null)
            {
                if (_user.WarehouseID.HasValue)
                {
                    Warehouse wareHouse = new Warehouse();
                    wareHouse = repo.WarehouseRepository.Find(_user.WarehouseID.Value);
                    ViewBag.Wid = _user.WarehouseID.Value;
                }
            }


            string _tempUrl = Request.Url.AbsoluteUri.ToString();

            int a = _tempUrl.Split('/').Length - 1;



            return PartialView("_WareHousePartial");

        }

        public ActionResult ChangeWareHouse(long ID)
        {
            User _user = repo.UserRepository.GetUserByUserName(HttpContext.User.Identity.Name);
            _user.WarehouseID = ID;
            _user.WarehouseName = repo.WarehouseRepository.Find(ID).WarehouseName;

            repo.UserRepository.InsertOrUpdate(_user);
            repo.UserRepository.Save();
            return Redirect(Request.UrlReferrer.ToString());

        }

        public ActionResult SetWareHouse()
        {
            //List<Warehouse> wareHouseList = new List<Warehouse>();
            List<User> userListforModel = new List<User>();
            List<User> userList = new List<Models.User>();

            userList = repo.UserRepository.GetAll();
            string _adminRole = "Admin";
            string _warehouseManagerRole = "Warehouse Manager";

            Role _roleAdmin = repo.RoleRepository.GetByRoleName(_adminRole);
            Role _roleWManager = repo.RoleRepository.GetByRoleName(_warehouseManagerRole);

            foreach (User item in userList)
            {
                if (item.Roles.Contains(_roleAdmin) || item.Roles.Contains(_roleWManager))
                {
                    userListforModel.Add(item);

                }

            }

            ViewBag.PossibleUsers = userListforModel;


            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            return View();
        }

        [HttpPost]
        public ActionResult SetWareHouse(User model)
        {
            if (model.WarehouseID != null && model.UserId != new Guid())
            {
                User _user = repo.UserRepository.GetByUserId(model.UserId);
                _user.WarehouseID = model.WarehouseID.Value;
                _user.WarehouseName = repo.WarehouseRepository.Find(model.WarehouseID.Value).WarehouseName;

                repo.UserRepository.InsertOrUpdate(_user);
                repo.UserRepository.Save();



                ViewBag.Flag = 1;

            }

            else
            {
                ViewBag.Flag = 0;

            }

            List<User> userListforModel = new List<User>();
            List<User> userList = new List<Models.User>();

            userList = repo.UserRepository.GetAll();
            string _adminRole = "Admin";
            string _warehouseManagerRole = "Warehouse Manager";

            Role _roleAdmin = repo.RoleRepository.GetByRoleName(_adminRole);
            Role _roleWManager = repo.RoleRepository.GetByRoleName(_warehouseManagerRole);

            foreach (User item in userList)
            {
                if (item.Roles.Contains(_roleAdmin) || item.Roles.Contains(_roleWManager))
                {
                    userListforModel.Add(item);

                }

            }

            ViewBag.PossibleUsers = userListforModel;
            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Denied()
        {
            return View();
        }
    }
}
