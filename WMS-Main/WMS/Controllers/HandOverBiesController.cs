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
    public class HandOverBiesController : Controller
    {
        UnitOfWork repo = new UnitOfWork();

        //
        // GET: /HandOverBies/

        public ViewResult Index()
        {
            return View(repo.HandOverByRepository.AllIncluding(handoverby => handoverby.TransmittalINs).Include(handoverby => handoverby.TrasmittalOUTs).ToList());
        }

        //
        // GET: /HandOverBies/Details/5

        public ViewResult Details(long id)
        {
            HandOverBy handoverby = repo.HandOverByRepository.Find(id);// context.HandOverBies.Single(x => x.HandOverById == id); ;
            return View(handoverby);
        }

        //
        // GET: /HandOverBies/Create

        public ActionResult Remove(long id, long trID)
        {
            HandOverBy _hob = repo.HandOverByRepository.Find(id);

            TransmittalIN trINSingle = repo.TransmittalINRepository.Find(Convert.ToInt64(trID));


            trINSingle.HandOverBy.Remove(_hob);
            repo.TransmittalINRepository.InsertOrUpdate(trINSingle);
            repo.TransmittalINRepository.Save();


            TransmittalIN transIN = repo.TransmittalINRepository.GetTrIN(trID);
            ViewBag.TransmittalNo = transIN.TransmittalNo;
            ViewBag.TransmittalRefNo = transIN.ClientRequestreference;
            ViewBag.TransmittalType = "Transmittal IN";
            ViewBag.TransmittalId = transIN.TransmittalINId;
            ViewBag.TransmittalDate = transIN.TransmittalDate.ToShortDateString();
            ViewBag.ClientName = repo.ClientRepository.Find(transIN.ClientID).ClientName;
            ViewBag.Department = repo.DepartmentRepository.Find(transIN.DepartmentID).DepartmentName;

            ViewBag.AllHandOverBy = transIN.HandOverBy;

            if (transIN.HandOverBy.Count > 0)
            {

                ViewBag.Flag = 1;


            }

            else if (transIN.HandOverBy.Count <= 0)
            {

                ViewBag.Flag = 0;
            }

            return View("Create");
        }

        public ActionResult RemoveOUT(long id, long trID)
        {
            //HandOverBy _hob = repo.HandOverByRepository.Find(id);
            ORBLOperator _hob = repo.ORBLOperatorRepository.Find(id);

            TransmittalOUT trOUTSingle = repo.TransmittalOUTRepository.Find(Convert.ToInt64(trID));


            trOUTSingle.HandOverBy.Remove(_hob);
            repo.TransmittalOUTRepository.InsertOrUpdate(trOUTSingle);
            repo.TransmittalINRepository.Save();

            TransmittalOUT transOUT = repo.TransmittalOUTRepository.Find(trID);
            ViewBag.TransmittalNo = transOUT.TransmittalNo;
            ViewBag.TransmittalRefNo = transOUT.ClientRequestreference;
            ViewBag.TransmittalType = "Transmittal OUT";
            ViewBag.TransmittalId = transOUT.TransmittalOUTId;
            ViewBag.TransmittalDate = transOUT.TransmittalDate.ToShortDateString();
            ViewBag.ClientName = repo.ClientRepository.Find(transOUT.ClientID).ClientName;
            ViewBag.Department = repo.DepartmentRepository.Find(transOUT.DepartmentID).DepartmentName;
            ViewBag.AllHandOverBy = transOUT.HandOverBy;
            ViewBag.PossibleOperator = repo.ORBLOperatorRepository.AllIncluding();



            //HandOverBy hand = new HandOverBy();
            //hand = repo.HandOverByRepository.GetByTrId(trId);



            if (transOUT.HandOverBy.Count > 0)
            {

                ViewBag.Flag = 1;


            }

            else if (transOUT.HandOverBy.Count <= 0)
            {

                ViewBag.Flag = 0;
            }

            //return View("AddHandOverBy");
            return RedirectToAction("HandOverByForTrOUT", new { trId = trID });
        }

        public ActionResult HandOverBy(int trId)
        {
            string _viewName = string.Empty;

            TransmittalIN transIN = repo.TransmittalINRepository.GetTrIN(trId);
            ViewBag.TransmittalNo = transIN.TransmittalNo;
            ViewBag.TransmittalRefNo = transIN.ClientRequestreference;
            ViewBag.TransmittalType = "Transmittal IN";
            ViewBag.TransmittalId = transIN.TransmittalINId;
            ViewBag.TransmittalDate = transIN.TransmittalDate.ToShortDateString();
            ViewBag.ClientName = repo.ClientRepository.Find(transIN.ClientID).ClientName;
            ViewBag.Department = repo.DepartmentRepository.Find(transIN.DepartmentID).DepartmentName;
            ViewBag.AllHandOverBy = transIN.HandOverBy;



            //HandOverBy hand = new HandOverBy();
            //hand = repo.HandOverByRepository.GetByTrId(trId);



            if (transIN.HandOverBy.Count > 0)
            {

                ViewBag.Flag = 1;


            }

            else if (transIN.HandOverBy.Count <= 0)
            {

                ViewBag.Flag = 0;
            }

            return View("Create");
        }


        public ActionResult HandOverByForTrOUT(long trId)
        {
            TransmittalOUT transOUT = repo.TransmittalOUTRepository.Find(trId);
            ViewBag.TransmittalNo = transOUT.TransmittalNo;
            ViewBag.TransmittalRefNo = transOUT.ClientRequestreference;
            ViewBag.TransmittalType = "Transmittal OUT";
            ViewBag.TransmittalId = transOUT.TransmittalOUTId;
            ViewBag.TransmittalDate = transOUT.TransmittalDate.ToShortDateString();
            ViewBag.ClientName = repo.ClientRepository.Find(transOUT.ClientID).ClientName;
            ViewBag.Department = repo.DepartmentRepository.Find(transOUT.DepartmentID).DepartmentName;
            ViewBag.AllHandOverBy = transOUT.HandOverBy;
            ViewBag.PossibleOperator = repo.ORBLOperatorRepository.AllIncluding();



            //HandOverBy hand = new HandOverBy();
            //hand = repo.HandOverByRepository.GetByTrId(trId);



            if (transOUT.HandOverBy.Count > 0)
            {

                ViewBag.Flag = 1;


            }

            else if (transOUT.HandOverBy.Count <= 0)
            {

                ViewBag.Flag = 0;
            }

            return View("AddHandOverBy");

        }

        [HttpPost]
        public ActionResult HandOverByForTrOUT(ORBLOperator handoverby, string trId)
        {
            if (ModelState.IsValid)
            {
                long _trID = Convert.ToInt64(trId);
                List<TransmittalOUT> trOUTList = new List<TransmittalOUT>();
                TransmittalOUT trout = repo.TransmittalOUTRepository.Find(_trID);

                ORBLOperator _operator = repo.ORBLOperatorRepository.Find(handoverby.ORBLOperatorId);

                //trout.ORBLOperatorId = handoverby.ORBLOperatorId;
                //trout.OrblOperatorAddress = handoverby.Address;
                List<ORBLOperator> operatorList = new List<ORBLOperator>();
                operatorList.Add(_operator);
                
                trout.HandOverBy = operatorList;
                repo.TransmittalOUTRepository.InsertOrUpdate(trout);
                repo.TransmittalOUTRepository.Save();


                trOUTList = repo.TransmittalOUTRepository.GetTrOUTList(_trID);


              //  handoverby.TrasmittalOUTs = trOUTList;


               //repo.orbl.InsertOrUpdate(handoverby);
                //repo.HandOverByRepository.Save();

                //Method for Partial View


                TransmittalOUT transOUT = repo.TransmittalOUTRepository.Find(_trID);//.TransmittalINs.Where(t => t.TransmittalINId == _trID).Include(tr => tr.Items).FirstOrDefault();

                ViewBag.TransmittalNo = transOUT.TransmittalNo;
                ViewBag.TransmittalRefNo = transOUT.ClientRequestreference;
                ViewBag.TransmittalType = "Transmittal OUT";
                ViewBag.TransmittalId = transOUT.TransmittalOUTId;
                ViewBag.TransmittalDate = transOUT.TransmittalDate.ToShortDateString();
                ViewBag.ClientName = repo.ClientRepository.Find(transOUT.ClientID).ClientName;
                ViewBag.Department = repo.DepartmentRepository.Find(transOUT.DepartmentID).DepartmentName;

                //ViewBag.AllItems = context.Items.Include(tr => tr.TransmittalINs).Include(tr => tr.TrasmittalOUTs).ToList(); 
                ViewBag.AllHandOverBy = transOUT.HandOverBy;
                ViewBag.PossibleOperator = repo.ORBLOperatorRepository.AllIncluding();


                if (transOUT.HandOverBy.Count > 0)
                {

                    ViewBag.Flag = 1;


                }

                else if (transOUT.HandOverBy.Count <= 0)
                {

                    ViewBag.Flag = 0;
                }
                //--------------------------//

                return RedirectToAction("HandOverByForTrOUT", new { trId = _trID });
            }

            return View(handoverby);
        }


        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /HandOverBies/Create

        [HttpPost]
        public ActionResult Create(HandOverBy handoverby)
        {
            if (ModelState.IsValid)
            {
                int _trID = Convert.ToInt32(handoverby.TransmittalINs[0].TransmittalINId.ToString());
                List<TransmittalIN> trIN = new List<TransmittalIN>();


                trIN = repo.TransmittalINRepository.GetTrINList(_trID);

                handoverby.TransmittalINs = trIN;


                repo.HandOverByRepository.InsertOrUpdate(handoverby);
                repo.HandOverByRepository.Save();

                //Method for Partial View


                TransmittalIN transIN = repo.TransmittalINRepository.GetTrIN(_trID);//.TransmittalINs.Where(t => t.TransmittalINId == _trID).Include(tr => tr.Items).FirstOrDefault();

                ViewBag.TransmittalNo = transIN.TransmittalNo;
                ViewBag.TransmittalRefNo = transIN.ClientRequestreference;
                ViewBag.TransmittalType = "Transmittal IN";
                ViewBag.TransmittalId = transIN.TransmittalINId;
                ViewBag.TransmittalDate = transIN.TransmittalDate.ToShortDateString();
                ViewBag.ClientName = repo.ClientRepository.Find(transIN.ClientID).ClientName;
                ViewBag.Department = repo.DepartmentRepository.Find(transIN.DepartmentID).DepartmentName;

                //ViewBag.AllItems = context.Items.Include(tr => tr.TransmittalINs).Include(tr => tr.TrasmittalOUTs).ToList(); 
                ViewBag.AllHandOverBy = transIN.HandOverBy;


                if (transIN.HandOverBy.Count > 0)
                {

                    ViewBag.Flag = 1;


                }

                else if (transIN.HandOverBy.Count <= 0)
                {

                    ViewBag.Flag = 0;
                }
                //--------------------------//

                return View("Create");
            }

            return View(handoverby);
        }

        //
        // GET: /HandOverBies/Edit/5

        public ActionResult Edit(long id)
        {
            HandOverBy handoverby = repo.HandOverByRepository.Find(id);//.HandOverBies.Single(x => x.HandOverById == id);
            return View(handoverby);
        }

        //
        // POST: /HandOverBies/Edit/5

        [HttpPost]
        public ActionResult Edit(HandOverBy handoverby)
        {
            if (ModelState.IsValid)
            {

                repo.HandOverByRepository.InsertOrUpdate(handoverby);
                repo.HandOverByRepository.Save();
                return RedirectToAction("Index");
            }
            return View(handoverby);
        }

        //
        // GET: /HandOverBies/Delete/5

        public ActionResult Delete(long id)
        {
            HandOverBy handoverby = repo.HandOverByRepository.Find(id);
            return View(handoverby);
        }

        //
        // POST: /HandOverBies/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            HandOverBy handoverby = repo.HandOverByRepository.Find(id);
            repo.HandOverByRepository.Delete(id);
            repo.HandOverByRepository.Save();
            return RedirectToAction("Index");
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