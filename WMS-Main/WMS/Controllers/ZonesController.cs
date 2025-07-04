using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;

namespace WareHouseMVC.Controllers
{
    public class ZonesController : Controller
    {
        UnitOfWork repo = new UnitOfWork();

        //
        // GET: zones/

        public ViewResult Index()
        {
            ViewBag.NoofZones = repo.ZoneRepository.All.Count();
            return View(repo.ZoneRepository.AllIncluding(Zone => Zone.Floor, Zone => Zone.Floor.Warehouse));
        }

        //
        // GET: zones/Details/5

        public ViewResult Details(long id)
        {
            //Zone zone = context.Zones.Single(x => x.ZoneID == id);
            return View(repo.ZoneRepository.Find(id));
        }

        //
        // GET: zones/Create

        public ActionResult Create()
        {
            ViewBag.NoofZones = repo.ZoneRepository.All.Count();
            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
            return View();
        }



        //
        // POST: zones/Create

        [HttpPost]
        public ActionResult Create(Zone zone)
        {
            if (ModelState.IsValid)
            {
                repo.ZoneRepository.InsertOrUpdate(zone);
                repo.ZoneRepository.Save();
                ViewBag.NoofZones = repo.ZoneRepository.All.Count();
                return RedirectToAction("Index");
            }
            ViewBag.NoofZones = repo.ZoneRepository.All.Count();
            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
            return View(zone);
        }

        //
        // GET: zones/Edit/5

        public ActionResult Edit(long id)
        {

            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
            return View(repo.ZoneRepository.Find(id));
        }

        //
        // POST: zones/Edit/5

        [HttpPost]
        public ActionResult Edit(Zone zone)
        {
            if (ModelState.IsValid)
            {
                repo.ZoneRepository.InsertOrUpdate(zone);
                repo.ZoneRepository.Save();
                return RedirectToAction("Index");


            }
            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();
            return View(zone);
        }

        //
        // GET: zones/Delete/5

        public ActionResult Delete(long id)
        {
            return View(repo.ZoneRepository.Find(id));
        }

        //
        // POST: zones/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            repo.ZoneRepository.Delete(id);
            repo.ZoneRepository.Save();
            return RedirectToAction("Index");
        }


        public ActionResult GetFloors(long WarehouseID)
        {
            var floors = repo.FloorRepository.FindByWarehouseID(WarehouseID);
            return View(floors);
        }


        public ActionResult GetOperatorAddress(long ORBLOperatorId)
        {
            var Orbloperator = repo.ORBLOperatorRepository.Find(ORBLOperatorId);
            return View(Orbloperator);
        }

        public ActionResult GetZones(long FloorID)
        {
            var zones = repo.ZoneRepository.FindByFloorID(FloorID);
            return View(zones);
        }


        public ActionResult GetTrains(long ZoneID)
        {
            var trains = repo.TrainRepository.FindByZoneID(ZoneID);
            return View(trains);
        }

        public ActionResult GetRacks(long TrainID)
        {
            var racks = repo.RackRepository.FindByTrainID(TrainID);
            return View(racks);
        }


        public ActionResult GetProjects(long DepartmentID)
        {
            var projects = repo.ProjectRepository.FindByDepartmentID(DepartmentID);
            return View(projects);
        }


        public ActionResult GetLevels(long RackID)
        {
            var levels = repo.LevelRepository.FindByRackID(RackID);
            return View(levels);
        }

        public ActionResult GetHeights(long LevelID)
        {
            var heights = repo.HeightRepository.FindByLevelID(LevelID);
            return View(heights);
        }

        public ActionResult GetColumns(long HeightID)
        {
            var columns = repo.ColumnRepository.FindByHeightID(HeightID);
            return View(columns);
        }

        public ActionResult GetRows(long ColumnID)
        {
            var rows = repo.RowRepository.FindByColumnID(ColumnID);
            return View(rows);
        }

        public ActionResult GetPallets(long ZoneID)
        {
            var pallets = repo.PalletRepository.GetByZoneID(ZoneID);
            return View(pallets);
        }

        public ActionResult GetDepts(long ClientID)
        {
            var depts = repo.DepartmentRepository.FindByClientID(ClientID);

            return View(depts);
        }

        public ActionResult GetDepts1(long ClientID)
        {
            var depts = repo.DepartmentRepository.FindByClientID(ClientID);

            return View(depts);
        }
        public ActionResult GetDepts2(long ClientID)
        {
            var depts = repo.DepartmentRepository.FindByClientID(ClientID);

            return View(depts);
        }
        public ActionResult GetDepts3(long ClientID)
        {
            var depts = repo.DepartmentRepository.FindByClientID(ClientID);

            return View(depts);
        }

        public ActionResult GetDeptsOld(long ClientID)
        {
            var depts = repo.DepartmentRepository.FindByClientID(ClientID);

            return View(depts);
        }

        public ActionResult GetDeptsNew(long ClientID)
        {
            var depts = repo.DepartmentRepository.FindByClientID(ClientID);

            return View(depts);
        }

        public ActionResult GetPersons(long DepartmentID)
        {
            var persons = repo.ContactPersonRepository.FindByDepartmentID(DepartmentID);
            return View(persons);
        }


        public ActionResult CreateWithFl(int flid)
        {
            ViewBag.fLID = flid;
            Floor fl = repo.FloorRepository.Find(flid);
            ViewBag.wHID = fl.WarehouseID;

            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            ViewBag.PossibleFloors = repo.FloorRepository.AllIncluding();


            return View("Create");
        }


        public ActionResult GetItemsByDeptId(long OldDeptId)
        {
            List<Item> itemList = new List<Item>();

            itemList = repo.ItemRepository.GetByDeptId(OldDeptId);

            ChangeDepartmentViewModel model=new ChangeDepartmentViewModel();
            model.itemList=itemList;
            return PartialView(model);
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