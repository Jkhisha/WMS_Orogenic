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
    public class ReceivedBiesController : Controller
    {
        UnitOfWork repo = new UnitOfWork();

        //
        // GET: /ReceivedBies/

        public ViewResult Index()
        {
            return View(repo.ReceivedByRepository.AllIncluding(receivedby => receivedby.TrasmittalOUTs).ToList());
        }

        //
        // GET: /ReceivedBies/Details/5

        public ViewResult Details(long id)
        {
            ReceivedBy receivedby = repo.ReceivedByRepository.Find(id);//.ReceivedBies.Single(x => x.ReceivedById == id);
            return View(receivedby);
        }

        public ActionResult Remove(long id, long trID)
        {
            ORBLOperator _rvdby  = repo.ORBLOperatorRepository.Find(id);
            TransmittalIN trINSingle = repo.TransmittalINRepository.Find(Convert.ToInt64(trID));


            trINSingle.ReceivedBy.Remove(_rvdby);
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
            ViewBag.AllReceivedBy = transIN.ReceivedBy;
            ViewBag.PossibleOperator = repo.ORBLOperatorRepository.AllIncluding();


            if (transIN.ReceivedBy.Count > 0)
            {

                ViewBag.Flag = 1;


            }

            else if (transIN.ReceivedBy.Count <= 0)
            {

                ViewBag.Flag = 0;
            }

            //return View("Create");
            return RedirectToAction("ReceivedBy", new { trID = trID });
        }

        public ActionResult RemoveOUT(long id, long trID)
        {

            ReceivedBy _rvdby = repo.ReceivedByRepository.Find(id);
            TransmittalOUT trOUTSingle = repo.TransmittalOUTRepository.Find(Convert.ToInt64(trID));


            trOUTSingle.ReceivedBy.Remove(_rvdby);
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

            ViewBag.AllReceivedBy = transOUT.ReceivedBy;



            //HandOverBy hand = new HandOverBy();
            //hand = repo.HandOverByRepository.GetByTrId(trId);



            if (transOUT.ReceivedBy.Count > 0)
            {

                ViewBag.Flag = 1;


            }

            else if (transOUT.ReceivedBy.Count <= 0)
            {

                ViewBag.Flag = 0;
            }

            return View("AddReceivedBy");
        }

        public ActionResult ReceivedBy(int trId)
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
           // ViewBag.AllReceivedBy = transIN.ReceivedBy;
            ViewBag.AllReceivedBy = transIN.ReceivedBy;
            ViewBag.PossibleOperator=repo.ORBLOperatorRepository.AllIncluding();

          //  ViewBag.AllReceiveBy = repo.ORBLOperatorRepository.AllIncluding();



            //HandOverBy hand = new HandOverBy();
            //hand = repo.HandOverByRepository.GetByTrId(trId);



            if (transIN.ReceivedBy.Count > 0)
            {

                ViewBag.Flag = 1;


            }

            else if (transIN.ReceivedBy.Count <= 0)
            {

                ViewBag.Flag = 0;
            }

            return View("Create");
        }

        public ActionResult ReceivedByForTrOUT(long trId)
        {

            TransmittalOUT transOUT = repo.TransmittalOUTRepository.Find(trId);
            ViewBag.TransmittalNo = transOUT.TransmittalNo;
            ViewBag.TransmittalRefNo = transOUT.ClientRequestreference;
            ViewBag.TransmittalType = "Transmittal OUT";
            ViewBag.TransmittalId = transOUT.TransmittalOUTId;
            ViewBag.TransmittalDate = transOUT.TransmittalDate.ToShortDateString();
            ViewBag.ClientName = repo.ClientRepository.Find(transOUT.ClientID).ClientName;
            ViewBag.Department = repo.DepartmentRepository.Find(transOUT.DepartmentID).DepartmentName;

            ViewBag.AllReceivedBy = transOUT.ReceivedBy;



            //HandOverBy hand = new HandOverBy();
            //hand = repo.HandOverByRepository.GetByTrId(trId);



            if (transOUT.ReceivedBy.Count > 0)
            {

                ViewBag.Flag = 1;


            }

            else if (transOUT.ReceivedBy.Count <= 0)
            {

                ViewBag.Flag = 0;
            }

            return View("AddReceivedBy");
        }

        [HttpPost]
        public ActionResult ReceivedByForTrOUT(ReceivedBy receivedby, string trId)
        {
            if (ModelState.IsValid)
            {
                long _trID = Convert.ToInt64(trId);
                List<TransmittalOUT> trOUTList = new List<TransmittalOUT>();


                trOUTList = repo.TransmittalOUTRepository.GetTrOUTList(_trID);

                receivedby.TrasmittalOUTs = trOUTList;


                repo.ReceivedByRepository.InsertOrUpdate(receivedby);
                repo.ReceivedByRepository.Save();

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

                ViewBag.AllReceivedBy = transOUT.ReceivedBy;


                if (transOUT.ReceivedBy.Count > 0)
                {

                    ViewBag.Flag = 1;


                }

                else if (transOUT.ReceivedBy.Count <= 0)
                {

                    ViewBag.Flag = 0;
                }
                //--------------------------//

                return RedirectToAction("ReceivedByForTrOUT", new { trId = _trID });
            }

            return View(receivedby);
        }

        //
        // GET: /ReceivedBies/Create

        public ActionResult Create()
        {
            ViewBag.PossibleOperator = repo.ORBLOperatorRepository.AllIncluding();

            return View();
        } 

        //
        // POST: /ReceivedBies/Create

        [HttpPost]
        public ActionResult Create(ORBLOperator receivedby, string trId)
        {
            if (ModelState.IsValid)
            {
                long _trID = Convert.ToInt64(trId);
                List<TransmittalIN> trIN = new List<TransmittalIN>();

                TransmittalIN trin = repo.TransmittalINRepository.Find(_trID);

                ORBLOperator _operator = repo.ORBLOperatorRepository.Find(receivedby.ORBLOperatorId);

                //trout.ORBLOperatorId = handoverby.ORBLOperatorId;
                //trout.OrblOperatorAddress = handoverby.Address;
                List<ORBLOperator> operatorList = new List<ORBLOperator>();
                operatorList.Add(_operator);

                trin.ReceivedBy = operatorList;
                repo.TransmittalINRepository.InsertOrUpdate(trin);
                repo.TransmittalINRepository.Save();


                trIN = repo.TransmittalINRepository.GetTrINList(_trID);

              //  receivedby.TransmittalINs = trIN;


             //   repo.ReceivedByRepository.InsertOrUpdate(receivedby);//.ReceivedBies.Add(receivedby);
               // repo.ReceivedByRepository.Save();//.SaveChanges();

                TransmittalIN transIN = repo.TransmittalINRepository.GetTrIN(_trID);//.TransmittalINs.Where(t => t.TransmittalINId == _trID).Include(tr => tr.Items).FirstOrDefault();

                ViewBag.TransmittalNo = transIN.TransmittalNo;
                ViewBag.TransmittalRefNo = transIN.ClientRequestreference;
                ViewBag.TransmittalType = "Transmittal IN";
                ViewBag.TransmittalId = transIN.TransmittalINId;
                ViewBag.TransmittalDate = transIN.TransmittalDate.ToShortDateString();
                ViewBag.ClientName = repo.ClientRepository.Find(transIN.ClientID).ClientName;
                ViewBag.Department = repo.DepartmentRepository.Find(transIN.DepartmentID).DepartmentName;
                ViewBag.AllReceivedBy = transIN.ReceivedBy;
            ViewBag.PossibleOperator=repo.ORBLOperatorRepository.AllIncluding();

                //ViewBag.AllItems = context.Items.Include(tr => tr.TransmittalINs).Include(tr => tr.TrasmittalOUTs).ToList(); 
              //  ViewBag.AllReceivedBy = transIN.ReceivedBy;


                if (transIN.ReceivedBy.Count > 0)
                {

                    ViewBag.Flag = 1;


                }

                else if (transIN.ReceivedBy.Count <= 0)
                {

                    ViewBag.Flag = 0;
                }
                //--------------------------//

                return View("Create");
            }

            return View(receivedby);
        }
        
        //
        // GET: /ReceivedBies/Edit/5
 
        public ActionResult Edit(long id)
        {
            ReceivedBy receivedby = repo.ReceivedByRepository.Find(id);//context.ReceivedBies.Single(x => x.ReceivedById == id);
            return View(receivedby);
        }

        //
        // POST: /ReceivedBies/Edit/5

        [HttpPost]
        public ActionResult Edit(ReceivedBy receivedby)
        {
            if (ModelState.IsValid)
            {
                repo.ReceivedByRepository.InsertOrUpdate(receivedby);//.Entry(receivedby).State = EntityState.Modified;
                repo.ReceivedByRepository.Save();//.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(receivedby);
        }

        //
        // GET: /ReceivedBies/Delete/5
 
        public ActionResult Delete(long id)
        {
            ReceivedBy receivedby = repo.ReceivedByRepository.Find(id);//context.ReceivedBies.Single(x => x.ReceivedById == id);
            return View(receivedby);
        }

        //
        // POST: /ReceivedBies/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            ReceivedBy receivedby = repo.ReceivedByRepository.Find(id);//context.ReceivedBies.Single(x => x.ReceivedById == id);
            repo.ReceivedByRepository.Delete(id);//.ReceivedBies.Remove(receivedby);
            repo.ReceivedByRepository.Save();//.SaveChanges();
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