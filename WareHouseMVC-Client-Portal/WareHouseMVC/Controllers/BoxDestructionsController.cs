using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;

namespace WareHouseMVC.Controllers
{
    public class BoxDestructionsController : BaseController
    {
        UnitOfWork repo = new UnitOfWork();
        long clientId = 0;
        public ViewResult Index()
        {

            return View(repo.BoxDestructionRepository.GetAll());
        }

        public ActionResult Create()
        {
           // ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();

            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }


            ViewBag.ClientName = repo.ClientRepository.Find(clientId).ClientName;
            ViewBag.ClientId = clientId;
            //ViewBag.PossibleDepartments = repo.DepartmentRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(clientId);
            return View();
        }


        [HttpPost]
        public ActionResult Create(BoxDestruction destruction)
        {
            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }

            destruction.ClientID = clientId;
            if (destruction.BoxName != null && destruction.BoxNumner != null)
            {
                Item item = new Item();

                if (!string.IsNullOrEmpty(destruction.BoxYear))
                {
                    item = repo.ItemRepository.GetByCliendIDandDeptIdandBoxNameandBoxNoAndYear(destruction.ClientID, destruction.DepartmentID, destruction.BoxName, destruction.BoxNumner,destruction.BoxYear);
                }
                else
                {
                    item = repo.ItemRepository.GetByCliendIDandDeptIdandBoxNameandBoxNo(destruction.ClientID, destruction.DepartmentID, destruction.BoxName, destruction.BoxNumner);
                }

                if (item != null)
                {

                    BoxDestruction boxDes = new BoxDestruction();
                    boxDes.BoxName = destruction.BoxName;
                    boxDes.BoxNumner = destruction.BoxNumner;
                    boxDes.BoxYear = destruction.BoxYear;
                    boxDes.ClientID = destruction.ClientID;
                    boxDes.ClientName = repo.ClientRepository.Find(destruction.ClientID).ClientName;
                    boxDes.CurrentStatus = 0;
                    boxDes.DepartmentID = destruction.DepartmentID;
                    boxDes.DepartmentName = repo.DepartmentRepository.Find(destruction.DepartmentID).DepartmentName;
                    boxDes.ItemId = item.ItemId;
                    boxDes.RequestBy = System.Web.HttpContext.Current.User.Identity.Name;
                    boxDes.RequestDate = DateTime.Now;
                    boxDes.DestructionDate = DateTime.Now;
                    repo.BoxDestructionRepository.InsertOrUpdate(boxDes);
                    repo.BoxDestructionRepository.Save();

                    ViewBag.Flag = "0";
                }
                else
                {
                    ViewBag.Flag = "2";
                }
            }
            else
            {
                ViewBag.Flag = "1";
            }
            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);
            }


            ViewBag.ClientName = repo.ClientRepository.Find(clientId).ClientName;
            ViewBag.ClientId = clientId;
            //ViewBag.PossibleDepartments = repo.DepartmentRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(clientId);
            return View();
        }


        [HttpGet]
        public ActionResult UpcomingList(DesBoxViewModel model)
        {
            if (System.Web.HttpContext.Current.Session["ClientId"] != null)
            {
                clientId = Convert.ToInt64(System.Web.HttpContext.Current.Session["ClientId"]);

                model.ClientID = clientId;
            }



            ViewBag.ClientName = repo.ClientRepository.Find(clientId).ClientName;
            ViewBag.ClientId = clientId;


            List<Item> items = new List<Item>();

            if (model.ClientID == 0)
            {
                items = repo.ItemRepository.GetByNextOneMonth();
            }
            else
            {
                if(model.DepartmentID==0 || model.DepartmentID==-1)
                {
                    items = repo.ItemRepository.GetByNextOneMonthByClient(clientId);
                }
                else
                {
                    items = repo.ItemRepository.GetByNextOneMonthByClientandDept(clientId, model.DepartmentID);
                }
            }
            model.SearchResults = items;


           // ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(clientId);
            return View(model);
        }


    }
}