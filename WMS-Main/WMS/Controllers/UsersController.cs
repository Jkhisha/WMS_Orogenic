using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;
using System.Web.Security;
using System.Net;
using System.Net.Sockets;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;

namespace WareHouseMVC.Controllers
{
    public class UsersController : Controller
    {
        UnitOfWork repo = new UnitOfWork();


        public ActionResult EditProfile()
        {
            using (UnitOfWork unit = new UnitOfWork())
            {

                User user = new User();

                user = unit.UserRepository.GetUserByUserName(User.Identity.Name);

                ViewBag.FirstName = user.FirstName;
                ViewBag.LastName = user.LastName;
                ViewBag.Email = user.Email;
                ViewBag.Mobile = user.MobileNumber;
                return View();
            }
        }

        public ActionResult UpdatePassword(string userName)
        {
            using (UnitOfWork unit = new UnitOfWork())
            {

                User user = new User();

                user = unit.UserRepository.GetUserByUserName(userName);

                ViewBag.UserName = user.Username;
                ViewBag.Email = user.Email;
                return View();
            }
        }


        [HttpPost]
        public ActionResult UpdatePassword(UpdatePasswordModel m)
        {

            if (ModelState.IsValid)
            {
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(m.UserName, true /* userIsOnline */);


                    changePasswordSucceeded = currentUser.ChangePassword(m.OldPassword, m.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {

                    ViewBag.successMessage = "Password Updated Successfully.";
                    User user = new User();

                    user = repo.UserRepository.GetUserByUserName(m.UserName);

                    ViewBag.UserName = user.Username;
                    ViewBag.Email = user.Email;
                    return View();
                }
                else
                {
                    //ModelState.AddModelError("", "");
                    ViewBag.successMessage = "The current password is incorrect or the new password is invalid.";


                }
            }




            User user2 = new User();

            user2 = repo.UserRepository.GetUserByUserName(m.UserName);

            ViewBag.UserName = user2.Username;
            ViewBag.Email = user2.Email;




            return View(m);
        }

        public ActionResult ResetPassword(string userName)
        {
            using (UnitOfWork unit = new UnitOfWork())
            {
                User user = new User();

                user = unit.UserRepository.GetUserByUserName(userName);

                ViewBag.UserName = user.Username;
                ViewBag.Email = user.Email;
                return View();
            }
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel m)
        {
            User finalUser = new User();
            List<Role> userRoles = new List<Role>();

            if (ModelState.IsValid)
            {

                #region Old Code Badhon Vai
                bool changePasswordSucceeded = false;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(m.UserName, false);
                    Client _cliet = new Client();
                    ClientUser cUserNew = new ClientUser();
                    cUserNew = repo.ClientRepository.GetClientByUserName(m.UserName);
                    // 
                    if (cUserNew != null)
                    {
                        _cliet = repo.ClientRepository.Find(cUserNew.ClientId);
                    }


                    User _user = new User();
                    _user = repo.UserRepository.GetByEmail(currentUser.Email);



                    userRoles = _user.Roles.ToList();
                    long warehouseID = 0;
                    string BannerText = _user.BannerText;
                    string firstname = _user.FirstName;
                    string mail = _user.Email;
                    string lastname = _user.LastName;
                    string mobileNumber = _user.MobileNumber;
                    string themename = _user.ThemeName;
                    string logourl = _user.LogoUrl;
                    string warehousename = _user.WarehouseName;
                    if (_user.WarehouseID != null)
                    {
                        warehouseID = _user.WarehouseID.Value;
                    }



                    Membership.DeleteUser(m.UserName);

                    MembershipCreateStatus createStatus;
                    Membership.CreateUser(m.UserName, m.NewPassword, mail, null, null, true, null, out createStatus);


                    User _userNew = new User();
                    _userNew = repo.UserRepository.GetByEmail(currentUser.Email);
                    _userNew.BannerText = BannerText;
                    _userNew.FirstName = firstname;
                    _userNew.LastName = lastname;
                    _userNew.MobileNumber = mobileNumber;
                    _userNew.ThemeName = themename;
                    _userNew.LogoUrl = logourl;
                    _userNew.WarehouseName = warehousename;
                    _userNew.WarehouseID = warehouseID;

                    repo.UserRepository.InsertOrUpdate(_userNew);
                    repo.UserRepository.Save();

                    foreach (Role role in userRoles)
                    {
                        Roles.AddUserToRole(m.UserName, role.RoleName);
                        if (role.RoleName == "Client")
                        {
                            List<ClientUser> cUserLst = new List<ClientUser>();

                            MembershipUser user = Membership.GetUser(m.UserName);
                            ClientUser cUser = new ClientUser();

                            //_cliet = repo.ClientRepository.GetByUserName(m.UserName);

                            cUser.UserName = user.UserName;
                            cUser.UserId = new Guid(user.ProviderUserKey.ToString());
                            cUser.ClientId = _cliet.ClientID;
                            cUser.ClientName = _cliet.ClientName;
                            cUser.Email = user.Email;
                            repo.ClientRepository.InsertcUser(cUser);
                            repo.ClientRepository.Save();

                            cUserLst = repo.ClientRepository.GetAllcUserBycId(_cliet.ClientID);



                            _cliet.ClientUserList = cUserLst;
                            repo.ClientRepository.InsertOrUpdate(_cliet);
                            repo.ClientRepository.Save();

                        }
                    }



                    changePasswordSucceeded = true;
                }

                catch (Exception ex)
                {
                    changePasswordSucceeded = false;
                }


                if (changePasswordSucceeded)
                {

                    ViewBag.successMessage = "Password Reset Successfully.";
                    User user = new User();

                    user = repo.UserRepository.GetUserByUserName(m.UserName);

                    ViewBag.UserName = user.Username;
                    ViewBag.Email = user.Email;
                    return View();
                }
                else
                {
                    //ModelState.AddModelError("", "");
                    ViewBag.successMessage = "The new password is invalid.";


                }

                #endregion
            }




            User user2 = new User();

            user2 = repo.UserRepository.GetUserByUserName(m.UserName);

            ViewBag.UserName = user2.Username;
            ViewBag.Email = user2.Email;




            return View(m);
        }


        private string GenerateSalt()
        {
            var buf = new byte[16];
            (new RNGCryptoServiceProvider()).GetBytes(buf);
            return Convert.ToBase64String(buf);
        }

        private string EncodePassword(string pass, string salt)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(pass);
            byte[] src = Convert.FromBase64String(salt);
            byte[] dst = new byte[src.Length + bytes.Length];
            byte[] inArray = null;
            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
            HashAlgorithm algorithm = HashAlgorithm.Create("SHA3");
            inArray = algorithm.ComputeHash(dst);
            return Convert.ToBase64String(inArray);
        }


        public string EncodePassword(string pass, MembershipPasswordFormat passwordFormat, string salt)
        {
            byte[] numArray;
            byte[] numArray1;
            string base64String;

            if (passwordFormat == MembershipPasswordFormat.Hashed)
            {
                byte[] bytes = Encoding.Unicode.GetBytes(pass);
                byte[] numArray2 = Convert.FromBase64String(salt);
                byte[] numArray3;

                // Hash password
                HashAlgorithm hashAlgorithm = HashAlgorithm.Create(Membership.HashAlgorithmType);

                if (hashAlgorithm as KeyedHashAlgorithm == null)
                {
                    numArray1 = new byte[numArray2.Length + bytes.Length];
                    Buffer.BlockCopy(numArray2, 0, numArray1, 0, numArray2.Length);
                    Buffer.BlockCopy(bytes, 0, numArray1, numArray2.Length, bytes.Length);
                    numArray3 = hashAlgorithm.ComputeHash(numArray1);
                }
                else
                {
                    KeyedHashAlgorithm keyedHashAlgorithm = (KeyedHashAlgorithm)hashAlgorithm;

                    if (keyedHashAlgorithm.Key.Length != numArray2.Length)
                    {

                        if (keyedHashAlgorithm.Key.Length >= numArray2.Length)
                        {
                            numArray = new byte[keyedHashAlgorithm.Key.Length];
                            int num = 0;
                            while (true)
                            {
                                if (!(num < numArray.Length))
                                {
                                    break;
                                }
                                int num1 = Math.Min(numArray2.Length, numArray.Length - num);
                                Buffer.BlockCopy(numArray2, 0, numArray, num, num1);
                                num = num + num1;
                            }
                            keyedHashAlgorithm.Key = numArray;
                        }
                        else
                        {
                            numArray = new byte[keyedHashAlgorithm.Key.Length];
                            Buffer.BlockCopy(numArray2, 0, numArray, 0, numArray.Length);
                            keyedHashAlgorithm.Key = numArray;
                        }
                    }
                    else
                    {
                        keyedHashAlgorithm.Key = numArray2;
                    }
                    numArray3 = keyedHashAlgorithm.ComputeHash(bytes);
                }

                base64String = Convert.ToBase64String(numArray3);
            }
            else if (passwordFormat == MembershipPasswordFormat.Encrypted)
            {
                throw new NotImplementedException("Encrypted password method is not supported.");
            }
            else
            {
                base64String = pass;
            }

            return base64String;
        }


        [HttpPost]
        public ActionResult EditProfile(EditProfileModel m)
        {
            if (ModelState.IsValid)
            {
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);


                    changePasswordSucceeded = currentUser.ChangePassword(m.OldPassword, m.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    using (UnitOfWork unit = new UnitOfWork())
                    {
                        User user = new User();

                        user = unit.UserRepository.GetUserByUserName(User.Identity.Name);

                        user.FirstName = m.FirstName;
                        user.LastName = m.LastName;
                        user.MobileNumber = m.MobileNumber;
                        user.Email = m.Email;

                        unit.UserRepository.InsertOrUpdate(user);
                        unit.Save();

                    }
                    ViewBag.successMessage = "Profile Updated Successfully.";

                    return View();
                }
                else
                {
                    //ModelState.AddModelError("", "");
                    ViewBag.successMessage = "The current password is incorrect or the new password is invalid.";


                }
            }




            return View(m);
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }


        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    HelperClasses.AuditTrailHelperClass auditHelper = new HelperClasses.AuditTrailHelperClass();
                    LoginTrail loginModel = new LoginTrail();
                    loginModel.LoginBy = model.UserName;
                    loginModel.LoginDate = DateTime.Now;
                    loginModel.LoginTime = DateTime.Now.ToString("h:mm tt", CultureInfo.InvariantCulture);

                    loginModel.LocalIP = auditHelper.LocalIPAddress();

                    repo.LoginTrailRepository.InsertOrUpdate(loginModel);
                    repo.LoginTrailRepository.Save();
                    Session["UserId"] = repo.UserRepository.GetUserByUserName(model.UserName).UserId;

                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "✖ Incorrect username or password!");
                }
            }
            else
            {
                ModelState.AddModelError("", "✖ Incorrect username or password!");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff
        
        public ActionResult ForgotPassword(string userName)
        {
            User user = repo.UserRepository.GetUserByUserName(userName);
            string NewPassword = System.Web.Security.Membership.GeneratePassword(10, 2);
            String NewHashedPassword = Crypto.HashPassword(NewPassword);
            user.Password = NewHashedPassword;
            repo.UserRepository.InsertOrUpdate(user);
            repo.UserRepository.Save();

            ClientUser clientUser = repo.ClientRepository.GetClientUserByUserName(userName);
            clientUser.IsFirstLogin = true;
            repo.ClientRepository.InsertOrUpdateClientUser(clientUser);
            repo.ClientRepository.Save();
            TempData["successMessage"] = "Password has been successfully reset! for "+ userName + " to ";
            TempData["NewPassword"] = NewPassword;

            return   RedirectToAction("CreateClientUser", "Clients");
        }
        //public ActionResult ForgotPassword()
        //{
        //    return RedirectToAction("CreateClientUser", "Clients");
        //}
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}