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
    public class BaseController : Controller
    {
        UnitOfWork repo = new UnitOfWork();


        public BaseController()
        {
            //if (System.Web.HttpContext.Current.User.IsInRole("Client"))
            //{
            //    Client _client = new Client();
            //    MembershipUser user = Membership.GetUser(System.Web.HttpContext.Current.User.Identity.Name);

            //    long clientId = repo.ClientRepository.GetcUserByUserId(new Guid(user.ProviderUserKey.ToString())).ClientId;

            //    //_client = repo.ClientRepository.GetByUserID(new Guid(user.ProviderUserKey.ToString()));
            //    System.Web.HttpContext.Current.Session["ClientId"] = clientId;

            //    User _user = new User();
            //    _user = repo.UserRepository.GetByUserId(new Guid(user.ProviderUserKey.ToString()));
            //    if (_user != null)
            //    {

            //        Session["ClientLogo"] = _user.LogoUrl;
            //        Session["BannerText"] = _user.BannerText;
            //        Session["Theme"] = "dark";// _user.ThemeName;
            //        Session["ColorScheme"] = _user.ThemeName;


            //}

        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["UserId"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary {
                            { "Controller", "Users" },
                            { "Action", "LogOn" }
                    });
            }
            base.OnActionExecuting(filterContext);
        }
    }        
}