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
                //check user as client
                if (Membership.ValidateUser(model.UserName, model.Password))
                {

                    if (System.Web.HttpContext.Current.User.IsInRole("Client"))
                    {

                        Client _client = new Client();
                        MembershipUser user = Membership.GetUser(System.Web.HttpContext.Current.User.Identity.Name);
                        if (user != null)
                        {
                            
                            long clientId = repo.ClientRepository.GetcUserByUserId(new Guid(user.ProviderUserKey.ToString())).ClientId;
                            
                            //_client = repo.ClientRepository.GetByUserID(new Guid(user.ProviderUserKey.ToString()));
                            System.Web.HttpContext.Current.Session["ClientId"] = clientId;

                            User _user = new User();
                            _user = repo.UserRepository.GetByUserId(new Guid(user.ProviderUserKey.ToString()));
                            if (_user != null)
                            {

                                Session["ClientLogo"] = _user.LogoUrl;
                                Session["BannerText"] = _user.BannerText;
                                Session["Theme"] = "dark";// _user.ThemeName;
                                Session["ColorScheme"] = _user.ThemeName;


                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "✖ Incorrect username or password!");
                        }


                    }

                    User userInfo = repo.UserRepository.GetUserByUserName(model.UserName);
                    Session["UserId"] = userInfo.UserId;

                    HelperClasses.AuditTrailHelperClass auditHelper = new HelperClasses.AuditTrailHelperClass();
                    LoginTrail loginModel = new LoginTrail();
                    loginModel.LoginBy = model.UserName;
                    loginModel.LoginDate = DateTime.Now;
                    loginModel.LoginTime = DateTime.Now.ToString("h:mm tt", CultureInfo.InvariantCulture);
                    try
                    {
                        loginModel.LocalIP = auditHelper.GetIPAddress();
                    }
                    catch
                    {
                    }

                    repo.LoginTrailRepository.InsertOrUpdate(loginModel);
                    repo.LoginTrailRepository.Save();


                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        ClientUser client = repo.ClientRepository.GetClientUserByUserName(model.UserName);
                        if(client.IsFirstLogin == true)
                        {
                            return RedirectToAction("ResetPassword", "Users", new { userName = model.UserName });
                        }
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


        public ActionResult ResetPassword(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("LogOn", "Account");
            }

            // Fetch user details using the username
            ClientUser client = repo.ClientRepository.GetClientUserByUserName(userName);
            if (client == null)
            {
                // Handle case when client is not found
                return RedirectToAction("LogOn", "Account");
            }

            // Store user information in ViewBag to display on the form
            ViewBag.UserName = client.UserName;
            ViewBag.Email = client.Email;

            // Return the ResetPassword view
            return View(new ResetPasswordModel());
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
           if(model.NewPassword != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "The new password and confirmation password do not match.");
                return View(model);
            }
            else
            {
                MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);

                User user = repo.UserRepository.GetUserByUserName(model.UserName);
                
                String NewHashedPassword = Crypto.HashPassword(model.NewPassword);
                user.Password = NewHashedPassword;
                repo.UserRepository.InsertOrUpdate(user);
                repo.UserRepository.Save();

                ClientUser clientUser = repo.ClientRepository.GetClientUserByUserName(model.UserName);
                clientUser.IsFirstLogin = false;
                repo.ClientRepository.InsertOrUpdateClientUser(clientUser);
                repo.ClientRepository.Save();
                return RedirectToAction("LogOn", "Users");
            }
            // Return the ResetPassword view
            return View(new ResetPasswordModel());
        }
        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.RemoveAll();
            //Session["ClientLogo"] = _user.LogoUrl;
            //Session["BannerText"] = _user.BannerText;
            //Session["Theme"] = _user.ThemeName;
            return RedirectToAction("Index", "Home");
        }
    }
}