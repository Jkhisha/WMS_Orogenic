using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;
using System.Web.Security;
using PagedList;
using System.IO;
using System.Configuration;

namespace WareHouseMVC.Controllers
{
    public class ClientsController : BaseController
    {


        //
        // GET: /Clients/




        UnitOfWork repo = new UnitOfWork();
        public IPagedList<AssignBox> pagedList { get; set; }

        [Authorize(Roles = "Admin")]
        public ViewResult Index()
        {
            return View(repo.ClientRepository.AllIncluding(Client => Client.SupportStuff).OrderByDescending(c => c.ClientName).ToList());
        }


        public ActionResult ClientTransmittalIN()
        {
            List<TransmittalIN> allTrIns = new List<TransmittalIN>();

            long _clientID = 0;
            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                _clientID = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }

            allTrIns = repo.TransmittalINRepository.GetByClientID(_clientID);
            ViewBag.allTrIns = allTrIns;

            return View();
        }

        public ActionResult ClientBoxes(int? page, string txtBoxNo, string txtBoxNname)
        {
            int pageSize = 50;
            int pageNumber = (page ?? 1);

            long _clientID = 0;
            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                _clientID = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }


            List<AssignBox> listofAllBoxes = new List<AssignBox>();
            pagedList = new PagedList<AssignBox>(listofAllBoxes, pageNumber, pageSize);


            if (!String.IsNullOrEmpty(txtBoxNo) && !String.IsNullOrEmpty(txtBoxNname))
            {
                pagedList = repo.AssignBoxRepository.GetByClientIdPagedListwithBoxNoandName(_clientID,txtBoxNo,txtBoxNname, pageNumber, pageSize);
            
            }

            else if (String.IsNullOrEmpty(txtBoxNo) && !String.IsNullOrEmpty(txtBoxNname))
            {
                pagedList = repo.AssignBoxRepository.GetByClientIdPagedListwithBoxName(_clientID,txtBoxNname, pageNumber, pageSize);
            }
            else if (!String.IsNullOrEmpty(txtBoxNo) && String.IsNullOrEmpty(txtBoxNname))
            {
                pagedList = repo.AssignBoxRepository.GetByClientIdPagedListwithBoxNo(_clientID, txtBoxNo, pageNumber, pageSize);
            }

            else
            {
                pagedList = repo.AssignBoxRepository.GetByClientIdPagedList(_clientID, pageNumber, pageSize);
            }

            ViewBag.flag = 1;
            ViewBag.AllBoxes = pagedList;

            ViewBag.txtBoxNo = txtBoxNo;
            ViewBag.txtBoxNname = txtBoxNname;

            return View(pagedList);

        }

        public ActionResult ClientTransmittalOUT()
        {
            List<TransmittalOUT> allTrOuts = new List<TransmittalOUT>();

            long _clientID = 0;
            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                _clientID = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }

            allTrOuts = repo.TransmittalOUTRepository.GetByClientID(_clientID);
            ViewBag.allTrOuts = allTrOuts;

            return View();
        }

        public ActionResult ClientAllBoxes()
        {
            return RedirectToAction("BoxLocationsAll", "BoxLocations");
        }


        //
        // GET: /Clients/Details/5

        public ViewResult Details(long id)
        {
            return View(repo.ClientRepository.Find(id));
        }

        //
        // GET: /Clients/Create

        public ActionResult Create()
        {
            ViewBag.PossibleStuff = repo.SupportStuffRepository.AllIncluding();
            return View();
        }

        //
        // POST: /Clients/Create

        [HttpPost]
        public ActionResult Create(Client client)
        {
            if (ModelState.IsValid)
            {
                if (client.Email == null || client.Email == string.Empty)
                    client.Email = "demo@demo.com";

                repo.ClientRepository.InsertOrUpdate(client);
                repo.ClientRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.PossibleStuff = repo.SupportStuffRepository.AllIncluding();
                return View();
            }
        }

        //
        // GET: /Clients/Edit/5

        public ActionResult Edit(long id)
        {
            ViewBag.PossibleStuff = repo.SupportStuffRepository.AllIncluding();
            return View(repo.ClientRepository.Find(id));
        }

        //
        // POST: /Clients/Edit/5

        [HttpPost]
        public ActionResult Edit(Client client)
        {
            if (ModelState.IsValid)
            {
                repo.ClientRepository.InsertOrUpdate(client);
                repo.ClientRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.PossibleStuff = repo.SupportStuffRepository.AllIncluding();
                return View();
            }
        }

        //
        // GET: /Clients/Delete/5

        public ActionResult Delete(long id)
        {
            return View(repo.ClientRepository.Find(id));
        }

        public ActionResult CreateClientUser()
        {
            ViewBag.allClient = repo.ClientRepository.GetAll();
            RegisterModelForClient register = new RegisterModelForClient();
            ViewBag.role = "Client";
            ViewBag.allUserClient = repo.ClientRepository.GetAllUserClient();
            List<string> themeList = new List<string>();
            themeList.Add("default");
            themeList.Add("orange");
            themeList.Add("dark");
            ViewBag.themeList = themeList;

            return View(register);

        }

        [HttpPost]
        public ActionResult CreateClientUser(RegisterModelForClient model, HttpPostedFileBase filenameFile)
        {
            string _viewName = string.Empty;
            bool IsSuccess = false;
            string password=System.Web.Security.Membership.GeneratePassword(8,2);
            if (ModelState.IsValid)
            {
                // Attempt to register the user

                Client _client = new Client();
                _client = repo.ClientRepository.Find(model.ClientID);


                MembershipCreateStatus createStatus;


                Membership.CreateUser(model.UserName, password, _client.Email, null, null, true, null, out createStatus);




              
                if (createStatus == MembershipCreateStatus.Success)
                {
                    // FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    // return RedirectToAction("Index", "Home");
                    ViewBag.successMessage = "User Created Successfully.";


                    foreach (string role in model.RoleNames)
                    {
                        Roles.AddUserToRole(model.UserName, role);
                    }





                    _client.UserName = model.UserName;
                    MembershipUser user = Membership.GetUser(model.UserName);
                    _client.UserId = new Guid(user.ProviderUserKey.ToString());




                    repo.ClientRepository.InsertOrUpdate(_client);
                    repo.ClientRepository.Save();


                    User _user = new User();
                    _user = repo.UserRepository.GetByUserId(new Guid(user.ProviderUserKey.ToString()));

                    try
                    {

                        _user.FirstName = _client.ClientName;
                        _user.BannerText = model.BannerText;
                        _user.ThemeName = model.ThemeName;

                        repo.UserRepository.InsertOrUpdate(_user);
                        repo.UserRepository.Save();

                        try
                        {

                            if (Request.Files[0].ContentType.ToString().ToLower().Contains("image"))
                            {
                                var fileName = Path.GetFileName(Request.Files[0].FileName);



                                var path = Path.Combine(Server.MapPath("~/Content/Images/") + ConfigurationManager.AppSettings["UploadedDocument"].ToString() + "ClientLogo/" + _client.ClientName.ToString() + "/");
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }

                                Request.Files[0].SaveAs(Path.Combine(path, fileName));
                                _user.LogoUrl = "Content/Images/" + ConfigurationManager.AppSettings["UploadedDocument"].ToString() + "ClientLogo/" + _client.ClientName.ToString() + "/" + fileName;

                            }

                            repo.UserRepository.InsertOrUpdate(_user);
                            repo.UserRepository.Save();
                        }

                        catch
                        {
                        }
                    
                    }
                    catch
                    {
                    }






                    _viewName = "CreateClientUser";
                    IsSuccess = true;


                }
                else
                {
                    ViewBag.successMessage = ErrorCodeToString(createStatus);
                    _viewName = "CreateClientUser";

                }



            }
            ViewBag.allClient = repo.ClientRepository.GetAll();
            ViewBag.allUserClient = repo.ClientRepository.GetAllUserClient();
            ViewBag.role = "Client";
            ViewBag.Password = password;

            List<string> themeList = new List<string>();
            themeList.Add("default");
            themeList.Add("orange");
            themeList.Add("dark");
            ViewBag.themeList = themeList;

            return View(_viewName, model);

        }

        public ActionResult DeleteClientUser(long id)
        {
            try
            {
                Client _client = new Client();
                _client = repo.ClientRepository.Find(id);

                MembershipUser user = Membership.GetUser(_client.UserName);
                Membership.DeleteUser(user.UserName);

                _client.UserName = null;
                repo.ClientRepository.InsertOrUpdate(_client);
                repo.ClientRepository.Save();

                ViewBag.successMessage = "User Deleted Successfully.";
            }
            catch
            {
                ViewBag.successMessage = "User does not Delete.";
            }

            ViewBag.allClient = repo.ClientRepository.GetAll();
            RegisterModelForClient register = new RegisterModelForClient();
            ViewBag.role = "Client";
            ViewBag.allUserClient = repo.ClientRepository.GetAllUserClient();
            List<string> themeList = new List<string>();
            themeList.Add("default");
            themeList.Add("orange");
            themeList.Add("dark");
            ViewBag.themeList = themeList;

            return View("CreateClientUser", register);
        }

        public ActionResult DepartmentTransfer()
        {
           
            ViewBag.PossibleClients = repo.ClientRepository.GetAll();
            ViewBag.possibleDepts = repo.DepartmentRepository.GetAllPagedList();
            ChangeDepartmentViewModel model = new ChangeDepartmentViewModel();
            model.itemList = new List<Item>();

            return View(model);
        }

        [HttpPost]
        public ActionResult DepartmentTransfer(ChangeDepartmentViewModel model)
        {

            if (ModelState.IsValid)
            {
                if (model.ItemIds != null)
                {


                    if (model.ClientId != 0)
                    {
                        if (model.OldDeptId != 0 && model.OldDeptId != -1)
                        {
                            if (model.NewDeptId != 0 && model.NewDeptId != -1)
                            {

                                Department newDept = new Department();
                                newDept = repo.DepartmentRepository.Find(model.NewDeptId);

                             

                                foreach (long item in model.ItemIds)
                                {
                                    Item _item = new Item();
                                    _item = repo.ItemRepository.Find(item);
                                    _item.DepartmentID = newDept.DepartmentID;
                                    _item.Department = newDept;
                                    repo.ItemRepository.InsertOrUpdate(_item);
                                    repo.ItemRepository.Save();

                                    AssignBox _assignBox = new AssignBox();
                                    _assignBox = repo.AssignBoxRepository.GetByItemID(item);
                                    if (_assignBox != null)
                                    {
                                        BarCode _barcode = new BarCode();
                                        _barcode = repo.BarCodeRepository.GetByAssignBoxId(_assignBox.AssignBoxId);
                                        if (_barcode != null)
                                        {
                                            _barcode.DeptName = newDept.DepartmentName;
                                            repo.BarCodeRepository.InsertOrUpdate(_barcode);
                                            repo.BarCodeRepository.Save();
                                        }
                                    }


                                    ViewBag.PossibleClients = repo.ClientRepository.GetAll();
                                    ViewBag.possibleDepts = repo.DepartmentRepository.GetAllPagedList();
                                    ViewBag.Flag = "1";

                                    model.itemList = new List<Item>();

                                    return View(model);

                                    
                                }


                               


                            }

                        }
                    }
                }

            }

            ViewBag.PossibleClients = repo.ClientRepository.GetAll();
            ViewBag.possibleDepts = repo.DepartmentRepository.GetAllPagedList();
            ViewBag.Flag = "0";
           
            model.itemList = new List<Item>();

            return View(model);
        }


        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion

        //
        // POST: /Clients/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            repo.ClientRepository.Delete(id);
            repo.ClientRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

                //clientRepository.Dispose();
                repo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

