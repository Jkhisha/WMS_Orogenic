using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using WareHouseMVC.Models;

namespace WareHouseMVC.Controllers
{

    public class AccountController : Controller
    {
        UnitOfWork repo = new UnitOfWork();

        //
        // GET: /Account/LogOn

        //
        // GET: /Account/Register


        public ActionResult UserList()
        {
            if (TempData["FLag"] == "1")
            {
                ViewBag.Check = false;
            }
            else
            {
                ViewBag.Check = true;
            }

            return View(repo.UserRepository.AllIncluding(Role => Role.Roles));


        }

        public ActionResult Delete(string userName)
        {

            User user = new User();
            user = repo.UserRepository.GetUserByUserName(userName);

            if (HttpContext.User.Identity.Name == userName)
            {

                ViewData["isEnable"] = false;

            }
            else
            {

                ViewData["isEnable"] = true;

            }


            return View(user);

        }

        [HttpPost]
        public ActionResult Delete(User model)
        {
            if (Roles.IsUserInRole(model.Username, "Client"))
            {
                Client _client = new Client();
                _client = repo.ClientRepository.GetByUserName(model.Username);

                _client.UserId = new Guid("00000000-0000-0000-0000-000000000000");
                _client.UserName = null;

                repo.ClientRepository.InsertOrUpdate(_client);
                repo.ClientRepository.Save();



            }
            Membership.DeleteUser(model.Username);
            List<User> user = repo.UserRepository.AllIncluding().ToList();

            return View("UserList", user);
        }


        public ActionResult Register()
        {
            RegisterModel register = new RegisterModel();
            using (UnitOfWork unit = new UnitOfWork())
            {
                //Roles.CreateRole("Client");
                //Roles.AddUserToRole(model.UserName, role);

                List<Role> roleList = new List<Role>();


                roleList = unit.RoleRepository.AllRoleList();
                register.RoleList = roleList;
                //ViewBag.PossibleRoles = roleList;
            }





            return View(register);
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            string _viewName = string.Empty;
            bool IsSuccess = false;
            List<User> users = new List<User>();
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                foreach (string role in model.RoleNames)
                {
                    Roles.AddUserToRole(model.UserName, role);
                }

                if (createStatus == MembershipCreateStatus.Success)
                {
                    // FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    // return RedirectToAction("Index", "Home");
                    ViewBag.successMessage = "User Created Successfully.";

                    users = repo.UserRepository.AllIncluding().ToList();

                    _viewName = "UserList";
                    IsSuccess = true;


                }
                else
                {
                    ViewBag.successMessage = ErrorCodeToString(createStatus);
                    _viewName = "Register";

                }



            }

            using (UnitOfWork unit = new UnitOfWork())
            {
                List<Role> roleList = new List<Role>();
                roleList = unit.RoleRepository.AllRoleList();
                model.RoleList = roleList;

                if (IsSuccess)
                    return View(_viewName, users);

                else
                    return View(_viewName, model);
            }
            // If we got this far, something failed, redisplay form

        }


        public ActionResult EditUser(string userName)
        {

            using (UnitOfWork repo = new UnitOfWork())
            {

                UserforEdit user = new UserforEdit();
                user = repo.UserRepository.GetUserByUserNameWithRoles(userName);


                List<Role> roleList = new List<Role>();
                roleList = repo.RoleRepository.AllRoleList();

                ViewBag.RoleList = roleList;

                return View(user);

            }


        }

        [HttpPost]
        public ActionResult EditUser(UserforEdit model)
        {

            using (UnitOfWork repo = new UnitOfWork())
            {

                WareHouseMVC.Models.User user = new Models.User();
                user = repo.UserRepository.GetUserByUserName(model.Username);

                user.Email = model.Email;
                foreach (string item in model.Roles)
                {
                    Role role = repo.RoleRepository.GetByRoleName(item);
                    user.Roles.Add(role);

                }

                repo.UserRepository.InsertOrUpdate(user);
                repo.UserRepository.Save();



                UserforEdit user2 = new UserforEdit();
                user2 = repo.UserRepository.GetUserByUserNameWithRoles(model.Username);


                List<Role> roleList = new List<Role>();
                roleList = repo.RoleRepository.AllRoleList();

                ViewBag.RoleList = roleList;

                TempData["FLag"] = "1";
                return RedirectToAction("UserList");
            }


        }


        //
        // GET: /Account/ChangePassword

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
    }
}
