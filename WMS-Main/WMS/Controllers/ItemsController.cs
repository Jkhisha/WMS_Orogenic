using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;
using System.IO;
using System.Configuration;
using System.Data.OleDb;
using PagedList;

namespace WareHouseMVC.Controllers
{
    public class ItemsController : Controller
    {
        // private WareHouseMVCContext context = new WareHouseMVCContext();
        UnitOfWork repo = new UnitOfWork();
        public IPagedList<AssignBox> pagedList { get; set; }
        public IPagedList<AssignBox> pagedListSesion { get; set; }
        public IPagedList<Item> pagedListItem { get; set; }
        int archievedItemNo = 0;






        //
        // GET: /Items/

        public ViewResult Index()
        {
            return View(repo.ItemRepository.AllIncluding(item => item.TransmittalINs).Include(item => item.TrasmittalOUTs).ToList());
        }

        //
        // GET: /Items/Details/5

        public ViewResult Details(long id)
        {
            Item item = repo.ItemRepository.Find(id);//.Single(x => x.ItemId == id);
            ViewBag.id = item.TransmittalINs[0].TransmittalINId;

            return View(item);
        }

        public ActionResult RemoveBox(long id, long trID)
        {

            List<TransmittalIN> trIN = new List<TransmittalIN>();
            Item item = repo.ItemRepository.Find(id);

            TransmittalIN trINSingle = repo.TransmittalINRepository.Find(Convert.ToInt64(trID));


            trINSingle.Items.Remove(item);
            repo.TransmittalINRepository.InsertOrUpdate(trINSingle);
            repo.TransmittalINRepository.Save();

            AssignBox _assignBox = repo.AssignBoxRepository.GetByTrIDandItemIDwithCurrentStatus(trID, id);

            if (_assignBox != null)
            {
                repo.AssignBoxRepository.Delete(_assignBox.AssignBoxId);
                repo.AssignBoxRepository.Save();
            }

            //Method for Partial View


            TransmittalIN transIN = repo.TransmittalINRepository.GetTrIN(trID);//.TransmittalINs.Where(t => t.TransmittalINId == _trID).Include(tr => tr.Items).FirstOrDefault();

            ViewBag.TransmittalNo = transIN.TransmittalNo;
            ViewBag.TransmittalRefNo = transIN.ClientRequestreference;
            ViewBag.TransmittalType = "Transmittal IN";
            ViewBag.TransmittalId = transIN.TransmittalINId;
            ViewBag.TransmittalDate = transIN.TransmittalDate.ToShortDateString();
            ViewBag.AllItems = transIN.Items;

            ViewBag.ClientName = repo.ClientRepository.Find(transIN.ClientID).ClientName;
            ViewBag.Department = repo.DepartmentRepository.Find(transIN.DepartmentID).DepartmentName;
            ViewBag.TotalArchieveItem = archievedItemNo;


            if (transIN.Type == "File")
            {
                ViewBag.File = "1";

            }

            if (transIN.Type == "Box")
            {
                ViewBag.File = "0";

            }


            return View("Create");
        }

        //
        public ActionResult CreateWitTrINId(int trId)
        {

            TransmittalIN transIN = repo.TransmittalINRepository.GetTrIN(trId);

            ViewBag.TransmittalNo = transIN.TransmittalNo;
            ViewBag.TransmittalRefNo = transIN.ClientRequestreference;
            ViewBag.TransmittalType = "Transmittal IN";
            ViewBag.TransmittalId = transIN.TransmittalINId;
            ViewBag.TransmittalDate = transIN.TransmittalDate.ToShortDateString();
            ViewBag.ClientName = repo.ClientRepository.Find(transIN.ClientID).ClientName;
            ViewBag.Department = repo.DepartmentRepository.Find(transIN.DepartmentID).DepartmentName;
            ViewBag.AllItems = transIN.Items; //context.TransmittalINs.Where(tr=>tr.TransmittalINId==trId).Include(tr => tr.Items).ToList();

            if (transIN.TransmittalINStatusId > 2)
            {
                ViewBag.status = 1;
            }
            else if (transIN.TransmittalINStatusId < 3)
            {
                ViewBag.status = 0;
            }

            if (transIN.TransmittalINStatusId >= 2)
            {
                ViewBag.btnPrint = "1";
            }

            ViewBag.TotalArchieveItem = archievedItemNo;


            if (transIN.Type == "File")
            {
                ViewBag.File = "1";

            }

            if (transIN.Type == "Box")
            {
                ViewBag.File = "0";

            }


            return View("Create");
        }







        // GET: /Items/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Items/Create

        [HttpPost]
        public ActionResult Create(Item item)
        {
            ModelState.Remove("ItemId");
            ModelState.Remove("BoxNo");
            if (ModelState.IsValid)
            {
                long _trIDGlobal = Convert.ToInt64(item.TransmittalINs[0].TransmittalINId.ToString());

                TransmittalIN trINFile = repo.TransmittalINRepository.Find(Convert.ToInt64(_trIDGlobal));

                if (trINFile.Type == "File")
                {
                    int _trID = Convert.ToInt32(item.TransmittalINs[0].TransmittalINId.ToString());
                    List<TransmittalIN> trIN = new List<TransmittalIN>();


                    trIN = repo.TransmittalINRepository.GetTrINList(_trID);
                    TransmittalIN trINSingle = repo.TransmittalINRepository.Find(Convert.ToInt64(_trID));

                    item.TransmittalINs = trIN;

                    item.ClientID = trINSingle.ClientID;
                    item.DepartmentID = trINSingle.DepartmentID;
                    if (trINSingle.ProjectId.HasValue)
                    {
                        item.ProjectId = trINSingle.ProjectId.Value;
                    }

                    item.Unit = "File";

                    repo.ItemRepository.InsertOrUpdate(item);
                    repo.ItemRepository.Save();

                    //Method for Partial View


                    TransmittalIN transIN = repo.TransmittalINRepository.GetTrIN(Convert.ToInt64(item.TransmittalINs[0].TransmittalINId.ToString()));//.TransmittalINs.Where(t => t.TransmittalINId == _trID).Include(tr => tr.Items).FirstOrDefault();

                    ViewBag.TransmittalNo = transIN.TransmittalNo;
                    ViewBag.TransmittalRefNo = transIN.ClientRequestreference;
                    ViewBag.TransmittalType = "Transmittal IN";
                    ViewBag.TransmittalId = transIN.TransmittalINId;
                    ViewBag.TransmittalDate = transIN.TransmittalDate.ToShortDateString();
                    ViewBag.ClientName = repo.ClientRepository.Find(transIN.ClientID).ClientName;
                    ViewBag.Department = repo.DepartmentRepository.Find(transIN.DepartmentID).DepartmentName;
                    ViewBag.AllItems = transIN.Items;
                    ViewBag.TotalArchieveItem = archievedItemNo;
                    ViewBag.File = "1";


                    //--------------------------//
                    return View("Create");
                }
                if (trINFile.Type == "Box")
                {

                    ////////////////////
                    if (item.ItemId == 0)
                    {
                        if (item.BoxNo.Length < 10)
                        {
                            ViewBag.Flag = 0;

                            int _trID = Convert.ToInt32(item.TransmittalINs[0].TransmittalINId.ToString());
                            List<TransmittalIN> trIN = new List<TransmittalIN>();


                            trIN = repo.TransmittalINRepository.GetTrINList(_trID);
                            TransmittalIN trINSingle = repo.TransmittalINRepository.Find(Convert.ToInt64(_trID));

                            item.TransmittalINs = trIN;

                            item.ClientID = trINSingle.ClientID;
                            item.DepartmentID = trINSingle.DepartmentID;
                            if (trINSingle.ProjectId.HasValue)
                            {
                                item.ProjectId = trINSingle.ProjectId.Value;
                            }
                            TransmittalIN transIN = repo.TransmittalINRepository.GetTrIN(_trID);//.TransmittalINs.Where(t => t.TransmittalINId == _trID).Include(tr => tr.Items).FirstOrDefault();

                            ViewBag.TransmittalNo = transIN.TransmittalNo;
                            ViewBag.TransmittalRefNo = transIN.ClientRequestreference;
                            ViewBag.TransmittalType = "Transmittal IN";
                            ViewBag.TransmittalId = transIN.TransmittalINId;
                            ViewBag.TransmittalDate = transIN.TransmittalDate.ToShortDateString();
                            ViewBag.ClientName = repo.ClientRepository.Find(transIN.ClientID).ClientName;
                            ViewBag.Department = repo.DepartmentRepository.Find(transIN.DepartmentID).DepartmentName;
                            ViewBag.AllItems = transIN.Items;
                            ViewBag.File = "0";

                        }
                        else
                        {
                            //put a checking for re archieve,check previous IN/OUT and update IN/OUT count

                            Item _item = new Item();


                            int trID = Convert.ToInt32(item.TransmittalINs[0].TransmittalINId.ToString());
                            TransmittalIN transmittalIN = repo.TransmittalINRepository.Find(trID);

                            //_item = repo.ItemRepository.GetItemUsingItemNo(item.ItemId);
                            if (item.ItemId != 0)
                            {
                                _item = repo.ItemRepository.GetItemUsingItemNo(item.ItemId);
                            }
                            else
                            {
                                _item = repo.ItemRepository.CheckArchieve(item.BoxNo, item.ItemName, item.Year, transmittalIN.ClientID, transmittalIN.DepartmentID);
                            }

                            if (item.IsArchieve == true && _item != null)
                            {

                                AssignBox _box = repo.AssignBoxRepository.GetByItemIDandCurrentStatus(_item.ItemId);


                                int transmittalINId = Convert.ToInt32(item.TransmittalINs[0].TransmittalINId.ToString());
                                TransmittalIN transMIttalIN = repo.TransmittalINRepository.Find(Convert.ToInt64(transmittalINId));




                                if (_box == null && transMIttalIN.Items.Contains(_item) == true)
                                {
                                    ViewBag.Flag = 4;
                                }

                                else if (_box == null && transMIttalIN.Items.Contains(_item) == false)
                                {

                                    ViewBag.Flag = 1;

                                    int _trID = Convert.ToInt32(item.TransmittalINs[0].TransmittalINId.ToString());
                                    List<TransmittalIN> trIN = new List<TransmittalIN>();


                                    trIN = repo.TransmittalINRepository.GetTrINList(_trID);
                                    TransmittalIN trINSingle = repo.TransmittalINRepository.Find(Convert.ToInt64(_trID));

                                    _item.TransmittalINs = trIN;

                                    _item.ClientID = trINSingle.ClientID;
                                    _item.DepartmentID = trINSingle.DepartmentID;
                                    if (trINSingle.ProjectId.HasValue)
                                    {
                                        _item.ProjectId = trINSingle.ProjectId.Value;
                                    }

                                    _item.Unit = "Box";

                                    //if (item.BarCodeText != null)
                                    //{
                                    //    EmptyBoxBarcode barcode = repo.EmptyBoxRepository.FindEmptyBoxBarcodeByBarcodeText(item.BarCodeText);
                                    //    AssignBox assignedBarcode = repo.AssignBoxRepository.FindAssignedBoxBarcodeByBarcodeText(item.BarCodeText);

                                    //        if (barcode == null)
                                    //        {

                                    //            ModelState.AddModelError("BarCodeText", "Invalid barcode. Check again!");
                                    //            ViewBag.Flag = 69;
                                    //        }
                                    //        else if (assignedBarcode != null)
                                    //        {
                                    //            ModelState.AddModelError("BarCodeText", "Already assigned barcode.");
                                    //            ViewBag.Flag = 69;

                                    //        }
                                    //        else
                                    //        {

                                    //            repo.ItemRepository.InsertOrUpdate(_item);
                                    //            repo.ItemRepository.Save();
                                    //        }
                                    //    //else
                                    //    //{
                                    //    //    ModelState.AddModelError("BarCodeText", "Invalid barcode or the barcode is already assigned");
                                    //    //}
                                    //}
                                    //else
                                    //{
                                    //    repo.ItemRepository.InsertOrUpdate(item);
                                    //    repo.ItemRepository.Save();
                                    //}
                                    repo.ItemRepository.InsertOrUpdate(_item);
                                    repo.ItemRepository.Save();
                                    archievedItemNo = Convert.ToInt32(repo.TransmittalINRepository.Find(_trIDGlobal).TotalArchieveItem);// Convert.ToInt32(item.TransmittalINs[0].TotalArchieveItem.ToString());
                                    int newArchieve = archievedItemNo + 1;
                                    ViewBag.TotalArchieveItem = newArchieve;

                                    TransmittalIN trINUpdate = new TransmittalIN();
                                    trINUpdate = repo.TransmittalINRepository.Find(_trIDGlobal);
                                    trINUpdate.TotalArchieveItem = newArchieve;
                                    repo.TransmittalINRepository.InsertOrUpdate(trINUpdate);
                                    repo.TransmittalINRepository.Save();



                                }
                                else
                                {
                                    ViewBag.Flag = 3;
                                }




                                TransmittalIN transIN = repo.TransmittalINRepository.GetTrIN(Convert.ToInt64(item.TransmittalINs[0].TransmittalINId.ToString()));//.TransmittalINs.Where(t => t.TransmittalINId == _trID).Include(tr => tr.Items).FirstOrDefault();

                                ViewBag.TransmittalNo = transIN.TransmittalNo;
                                ViewBag.TransmittalRefNo = transIN.ClientRequestreference;
                                ViewBag.TransmittalType = "Transmittal IN";
                                ViewBag.TransmittalId = transIN.TransmittalINId;
                                ViewBag.TransmittalDate = transIN.TransmittalDate.ToShortDateString();
                                ViewBag.ClientName = repo.ClientRepository.Find(transIN.ClientID).ClientName;
                                ViewBag.Department = repo.DepartmentRepository.Find(transIN.DepartmentID).DepartmentName;
                                ViewBag.AllItems = transIN.Items;
                                ViewBag.TotalArchieveItem = archievedItemNo;
                                ViewBag.File = "0";
                                item.IsArchieve = false;

                            }
                            else if (item.IsArchieve == false && _item != null)
                            {
                                ViewBag.Flag = 2;
                                TransmittalIN transIN = repo.TransmittalINRepository.GetTrIN(Convert.ToInt64(item.TransmittalINs[0].TransmittalINId.ToString()));//.TransmittalINs.Where(t => t.TransmittalINId == _trID).Include(tr => tr.Items).FirstOrDefault();

                                ViewBag.TransmittalNo = transIN.TransmittalNo;
                                ViewBag.TransmittalRefNo = transIN.ClientRequestreference;
                                ViewBag.TransmittalType = "Transmittal IN";
                                ViewBag.TransmittalId = transIN.TransmittalINId;
                                ViewBag.TransmittalDate = transIN.TransmittalDate.ToShortDateString();
                                ViewBag.ClientName = repo.ClientRepository.Find(transIN.ClientID).ClientName;
                                ViewBag.Department = repo.DepartmentRepository.Find(transIN.DepartmentID).DepartmentName;
                                ViewBag.AllItems = transIN.Items;
                                ViewBag.TotalArchieveItem = archievedItemNo;
                                ViewBag.File = "0";
                                item.IsArchieve = false;
                            }
                            else
                            {

                                ViewBag.Flag = 1;

                                int _trID = Convert.ToInt32(item.TransmittalINs[0].TransmittalINId.ToString());
                                List<TransmittalIN> trIN = new List<TransmittalIN>();


                                trIN = repo.TransmittalINRepository.GetTrINList(_trID);
                                TransmittalIN trINSingle = repo.TransmittalINRepository.Find(Convert.ToInt64(_trID));

                                item.TransmittalINs = trIN;

                                item.ClientID = trINSingle.ClientID;
                                item.DepartmentID = trINSingle.DepartmentID;
                                if (trINSingle.ProjectId.HasValue)
                                {
                                    item.ProjectId = trINSingle.ProjectId.Value;
                                }

                                item.Unit = "Box";
                                //if (item.BarCodeText != null)
                                //{
                                //    EmptyBoxBarcode barcode = repo.EmptyBoxRepository.FindEmptyBoxBarcodeByBarcodeText(item.BarCodeText);
                                //    AssignBox assignedBarcode = repo.AssignBoxRepository.FindAssignedBoxBarcodeByBarcodeText(item.BarCodeText);

                                //    if (barcode == null)
                                //    {

                                //        ModelState.AddModelError("BarCodeText", "Invalid barcode. Check again!");
                                //        ViewBag.Flag = 69;
                                //    }
                                //    else if (assignedBarcode != null)
                                //    {
                                //        ModelState.AddModelError("BarCodeText", "Already assigned barcode.");
                                //        ViewBag.Flag = 69;

                                //    }
                                //    else
                                //    {

                                //        repo.ItemRepository.InsertOrUpdate(item);
                                //        repo.ItemRepository.Save();
                                //    }
                                //    //else
                                //    //{
                                //    //    ModelState.AddModelError("BarCodeText", "Invalid barcode or the barcode is already assigned");
                                //    //}
                                //}
                                //else
                                //{
                                //    repo.ItemRepository.InsertOrUpdate(item);
                                //    repo.ItemRepository.Save();
                                //}
                                repo.ItemRepository.InsertOrUpdate(item);
                                repo.ItemRepository.Save();
                                //Method for Partial View


                                TransmittalIN transIN = repo.TransmittalINRepository.GetTrIN(Convert.ToInt64(item.TransmittalINs[0].TransmittalINId.ToString()));//.TransmittalINs.Where(t => t.TransmittalINId == _trID).Include(tr => tr.Items).FirstOrDefault();

                                //Newly Added (12 sep 2021)
                                ViewBag.OldBoxNo = item.BoxNo;
                                //End

                                ViewBag.TransmittalNo = transIN.TransmittalNo;
                                ViewBag.TransmittalRefNo = transIN.ClientRequestreference;
                                ViewBag.TransmittalType = "Transmittal IN";
                                ViewBag.TransmittalId = transIN.TransmittalINId;
                                ViewBag.TransmittalDate = transIN.TransmittalDate.ToShortDateString();
                                ViewBag.ClientName = repo.ClientRepository.Find(transIN.ClientID).ClientName;
                                ViewBag.Department = repo.DepartmentRepository.Find(transIN.DepartmentID).DepartmentName;
                                ViewBag.AllItems = transIN.Items;
                                ViewBag.TotalArchieveItem = archievedItemNo;
                                ViewBag.File = "0";
                                item.IsArchieve = false;

                                //--------------------------//
                                return View("Create");
                            }



                        }
                    }
                    else
                    {
                        //put a checking for re archieve,check previous IN/OUT and update IN/OUT count

                        Item _item = new Item();


                        int trID = Convert.ToInt32(item.TransmittalINs[0].TransmittalINId.ToString());
                        TransmittalIN transmittalIN = repo.TransmittalINRepository.Find(trID);

                        //_item = repo.ItemRepository.GetItemUsingItemNo(item.ItemId);

                        _item = repo.ItemRepository.GetItemUsingItemNo(item.ItemId);



                        if (item.IsArchieve == true && _item != null)
                        {

                            ViewBag.ArchiveStatus = 1;
                            AssignBox _box = repo.AssignBoxRepository.GetByItemIDandCurrentStatus(_item.ItemId);


                            int transmittalINId = Convert.ToInt32(item.TransmittalINs[0].TransmittalINId.ToString());
                            TransmittalIN transMIttalIN = repo.TransmittalINRepository.Find(Convert.ToInt64(transmittalINId));




                            if (_box == null && transMIttalIN.Items.Contains(_item) == true)
                            {
                                ViewBag.Flag = 4;
                            }

                            else if (_box == null && transMIttalIN.Items.Contains(_item) == false)
                            {

                                ViewBag.Flag = 1;

                                int _trID = Convert.ToInt32(item.TransmittalINs[0].TransmittalINId.ToString());
                                List<TransmittalIN> trIN = new List<TransmittalIN>();


                                trIN = repo.TransmittalINRepository.GetTrINList(_trID);
                                TransmittalIN trINSingle = repo.TransmittalINRepository.Find(Convert.ToInt64(_trID));

                                _item.TransmittalINs = trIN;

                                _item.ClientID = trINSingle.ClientID;
                                _item.DepartmentID = trINSingle.DepartmentID;
                                if (trINSingle.ProjectId.HasValue)
                                {
                                    _item.ProjectId = trINSingle.ProjectId.Value;
                                }

                                _item.Unit = "Box";

                                repo.ItemRepository.InsertOrUpdate(_item);
                                repo.ItemRepository.Save();

                                archievedItemNo = Convert.ToInt32(repo.TransmittalINRepository.Find(_trIDGlobal).TotalArchieveItem);// Convert.ToInt32(item.TransmittalINs[0].TotalArchieveItem.ToString());
                                int newArchieve = archievedItemNo + 1;
                                ViewBag.TotalArchieveItem = newArchieve;

                                TransmittalIN trINUpdate = new TransmittalIN();
                                trINUpdate = repo.TransmittalINRepository.Find(_trIDGlobal);
                                trINUpdate.TotalArchieveItem = newArchieve;
                                repo.TransmittalINRepository.InsertOrUpdate(trINUpdate);
                                repo.TransmittalINRepository.Save();



                            }
                            else
                            {
                                ViewBag.Flag = 3;
                            }




                            TransmittalIN transIN = repo.TransmittalINRepository.GetTrIN(Convert.ToInt64(item.TransmittalINs[0].TransmittalINId.ToString()));//.TransmittalINs.Where(t => t.TransmittalINId == _trID).Include(tr => tr.Items).FirstOrDefault();

                            ViewBag.TransmittalNo = transIN.TransmittalNo;
                            ViewBag.TransmittalRefNo = transIN.ClientRequestreference;
                            ViewBag.TransmittalType = "Transmittal IN";
                            ViewBag.TransmittalId = transIN.TransmittalINId;
                            ViewBag.TransmittalDate = transIN.TransmittalDate.ToShortDateString();
                            ViewBag.ClientName = repo.ClientRepository.Find(transIN.ClientID).ClientName;
                            ViewBag.Department = repo.DepartmentRepository.Find(transIN.DepartmentID).DepartmentName;
                            ViewBag.AllItems = transIN.Items;
                            ViewBag.TotalArchieveItem = archievedItemNo;
                            ViewBag.File = "0";
                            item.IsArchieve = false;

                        }
                        else
                        {

                            ViewBag.Flag = 1;
                            ViewBag.ItemZero = 1;
                            ViewBag.ArchiveStatus = 1;

                            //int _trID = Convert.ToInt32(item.TransmittalINs[0].TransmittalINId.ToString());
                            //List<TransmittalIN> trIN = new List<TransmittalIN>();


                            //trIN = repo.TransmittalINRepository.GetTrINList(_trID);
                            //TransmittalIN trINSingle = repo.TransmittalINRepository.Find(Convert.ToInt64(_trID));

                            //item.TransmittalINs = trIN;

                            //item.ClientID = trINSingle.ClientID;
                            //item.DepartmentID = trINSingle.DepartmentID;
                            //if (trINSingle.ProjectId.HasValue)
                            //{
                            //    item.ProjectId = trINSingle.ProjectId.Value;
                            //}

                            //item.Unit = "Box";

                            //repo.ItemRepository.InsertOrUpdate(item);
                            //repo.ItemRepository.Save();

                            //Method for Partial View


                            TransmittalIN transIN = repo.TransmittalINRepository.GetTrIN(Convert.ToInt64(item.TransmittalINs[0].TransmittalINId.ToString()));//.TransmittalINs.Where(t => t.TransmittalINId == _trID).Include(tr => tr.Items).FirstOrDefault();

                            ViewBag.TransmittalNo = transIN.TransmittalNo;
                            ViewBag.TransmittalRefNo = transIN.ClientRequestreference;
                            ViewBag.TransmittalType = "Transmittal IN";
                            ViewBag.TransmittalId = transIN.TransmittalINId;
                            ViewBag.TransmittalDate = transIN.TransmittalDate.ToShortDateString();
                            ViewBag.ClientName = repo.ClientRepository.Find(transIN.ClientID).ClientName;
                            ViewBag.Department = repo.DepartmentRepository.Find(transIN.DepartmentID).DepartmentName;
                            ViewBag.AllItems = transIN.Items;
                            ViewBag.TotalArchieveItem = archievedItemNo;
                            ViewBag.File = "0";
                            item.IsArchieve = false;

                            //--------------------------//
                            //return View("Create");
                        }



                    }
                }
                ////////////////
            }

            TransmittalIN transINnew = repo.TransmittalINRepository.GetTrIN(Convert.ToInt64(item.TransmittalINs[0].TransmittalINId.ToString()));//.TransmittalINs.Where(t => t.TransmittalINId == _trID).Include(tr => tr.Items).FirstOrDefault();

            ViewBag.TransmittalNo = transINnew.TransmittalNo;
            ViewBag.TransmittalRefNo = transINnew.ClientRequestreference;
            ViewBag.TransmittalType = "Transmittal IN";
            ViewBag.TransmittalId = transINnew.TransmittalINId;
            ViewBag.TransmittalDate = transINnew.TransmittalDate.ToShortDateString();
            ViewBag.ClientName = repo.ClientRepository.Find(transINnew.ClientID).ClientName;
            ViewBag.Department = repo.DepartmentRepository.Find(transINnew.DepartmentID).DepartmentName;
            ViewBag.AllItems = transINnew.Items;
            ViewBag.TotalArchieveItem = archievedItemNo;
            item.IsArchieve = false;
            if (transINnew.Type == "File")
            {
                ViewBag.File = "1";
            }
            if (transINnew.Type == "Box")
            {
                ViewBag.File = "0";
            }

            return View(item);
        }

        //
        // GET: /Items/Edit/5

        public ActionResult Edit(long id)
        {
            Item item = repo.ItemRepository.Find(id);// context.Items.Single(x => x.ItemId == id);
            ViewBag.id = item.TransmittalINs[0].TransmittalINId;
            return View(item);
        }

        //
        // POST: /Items/Edit/5

        [HttpPost]
        public ActionResult Edit(Item item)
        {
            if (ModelState.IsValid)
            {


                repo.ItemRepository.InsertOrUpdate(item);
                repo.ItemRepository.Save();

                long id = item.TransmittalINs[0].TransmittalINId;

                return RedirectToAction("CreateWitTrINId", "Items", new { id = id });

            }
            return View(item);
        }

        //
        // GET: /Items/Delete/5

        public ActionResult Delete(long id)
        {
            Item item = repo.ItemRepository.Find(id);//.Items.Single(x => x.ItemId == id);
            return View(item);
        }

        //
        // POST: /Items/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            Item item = repo.ItemRepository.Find(id);



            repo.ItemRepository.Delete(id);
            repo.ItemRepository.Save();

            long trid = item.TransmittalINs[0].TransmittalINId;
            return RedirectToAction("CreateWitTrINId", "Items", new { id = trid });
        }


        public ActionResult AssignBoxes(int trID, int flag)
        {

            if (flag == 0)
            {
                ViewBag.hidden = 0;
            }
            else if (flag == 1)
            {
                ViewBag.hidden = 1;
                ViewBag.print = 1;
            }


            ///////////////

            TransmittalIN transIN = repo.TransmittalINRepository.GetTrIN(trID);
            if (transIN.TransmittalINStatusId == (int)EnumHelper.Status.WareHouse_Assigned)
            {
                ViewBag.hidden = 0;
                // ViewBag.hidden = 1;
                ViewBag.print = 1;
            }

            if (transIN.Type == "File")
            {

                ViewBag.TransmittalNo = transIN.TransmittalNo;
                ViewBag.TransmittalRefNo = transIN.ClientRequestreference;
                ViewBag.TransmittalType = "Transmittal IN";
                ViewBag.TransmittalId = transIN.TransmittalINId;
                ViewBag.TransmittalDate = transIN.TransmittalDate.ToShortDateString();
                ViewBag.AllItems = transIN.Items;
                ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
                ViewBag.File = "1";


                Warehouse wareHouse = repo.WarehouseRepository.GetFirst();


                List<AssignBox> boxList = repo.AssignBoxRepository.GetByTrInId(trID);

                foreach (var item in boxList)
                {

                    if (item.TransmittalINStatusId == 1)
                    {
                        repo.AssignBoxRepository.Delete(item.AssignBoxId);
                        repo.AssignBoxRepository.Save();
                    }
                    else
                    {
                        ViewBag.Assigned = 1;
                        ViewBag.WID = item.WarehouseID;
                    }


                }

                foreach (var item in transIN.Items)
                {
                    AssignBox _boxChk = repo.AssignBoxRepository.GetByItemIdandTrINId(item.ItemId, trID);

                    if (_boxChk != null)
                    {
                        if (_boxChk.TransmittalINStatusId == 1)
                        {


                            AssignBox _box = new AssignBox();
                            _box.AssignDate = DateTime.Now;
                            _box.CurrentStatus = 1;
                            _box.TransmittalINStatusId = 1;
                            _box.TransmittalINId = transIN.TransmittalINId;
                            _box.ItemId = item.ItemId;
                            _box.WarehouseID = wareHouse.WarehouseID;
                            _box.WarehouseName = wareHouse.WarehouseName;
                            Item boxItem = repo.ItemRepository.Find(item.ItemId);

                            _box.BoxNo = boxItem.BoxNo;
                            _box.BoxName = boxItem.ItemName;
                            _box.Year = boxItem.Year;


                            _box.BoxNameFile = boxItem.FileBoxName;
                            _box.FileNumber = boxItem.FileNumber;
                            _box.ReferrenceNo = boxItem.ReferrenceNo;
                            _box.RingNo = boxItem.RingNo;
                            _box.AccountNo = boxItem.AccountNo;

                            _box.DestructionPeriod = boxItem.DestructionPeriod;

                            repo.AssignBoxRepository.InsertOrUpdate(_box);
                            repo.AssignBoxRepository.Save();
                        }
                    }

                    else
                    {
                        AssignBox _box = new AssignBox();
                        _box.AssignDate = DateTime.Now;
                        _box.CurrentStatus = 1;
                        _box.TransmittalINStatusId = 1;
                        _box.TransmittalINId = transIN.TransmittalINId;
                        _box.ItemId = item.ItemId;
                        _box.WarehouseID = wareHouse.WarehouseID;
                        _box.WarehouseName = wareHouse.WarehouseName;
                        Item boxItem = repo.ItemRepository.Find(item.ItemId);

                        _box.BoxNo = boxItem.BoxNo;
                        _box.BoxName = boxItem.ItemName;
                        _box.Year = boxItem.Year;

                        _box.BoxNameFile = boxItem.FileBoxName;
                        _box.FileNumber = boxItem.FileNumber;
                        _box.ReferrenceNo = boxItem.ReferrenceNo;
                        _box.RingNo = boxItem.RingNo;
                        _box.AccountNo = boxItem.AccountNo;


                        _box.DestructionPeriod = boxItem.DestructionPeriod;

                        repo.AssignBoxRepository.InsertOrUpdate(_box);
                        repo.AssignBoxRepository.Save();
                    }
                }


            }
            if (transIN.Type == "Box")
            {

                //  TransmittalIN transIN = repo.TransmittalINRepository.GetTrIN(trID);



                ViewBag.TransmittalNo = transIN.TransmittalNo;
                ViewBag.TransmittalRefNo = transIN.ClientRequestreference;
                ViewBag.TransmittalType = "Transmittal IN";
                ViewBag.TransmittalId = transIN.TransmittalINId;
                ViewBag.TransmittalDate = transIN.TransmittalDate.ToShortDateString();
                ViewBag.AllItems = transIN.Items;
                ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
                ViewBag.File = "0";


                Warehouse wareHouse = repo.WarehouseRepository.GetFirst();


                List<AssignBox> boxList = repo.AssignBoxRepository.GetByTrInId(trID);

                foreach (var item in boxList)
                {

                    if (item.TransmittalINStatusId == 1)
                    {
                        repo.AssignBoxRepository.Delete(item.AssignBoxId);
                        repo.AssignBoxRepository.Save();
                    }
                    else
                    {
                        ViewBag.Assigned = 1;
                        ViewBag.WID = item.WarehouseID;
                    }


                }


                foreach (var item in transIN.Items)
                {
                    AssignBox _boxChk = repo.AssignBoxRepository.GetByItemIdandTrINId(item.ItemId, trID);

                    if (_boxChk != null)
                    {
                        if (_boxChk.TransmittalINStatusId == 1)
                        {


                            AssignBox _box = new AssignBox();
                            _box.AssignDate = DateTime.Now;
                            _box.CurrentStatus = 1;
                            _box.TransmittalINStatusId = 1;
                            _box.TransmittalINId = transIN.TransmittalINId;
                            _box.ItemId = item.ItemId;
                            _box.WarehouseID = wareHouse.WarehouseID;
                            _box.WarehouseName = wareHouse.WarehouseName;
                            Item boxItem = repo.ItemRepository.Find(item.ItemId);
                            _box.BoxNo = boxItem.BoxNo;
                            _box.BoxName = boxItem.ItemName;
                            _box.Year = boxItem.Year;
                            _box.DestructionPeriod = boxItem.DestructionPeriod;
                            _box.Category = boxItem.Category;
                            if (boxItem.BarCodeText != null)
                            {
                                EmptyBoxBarcode barcode = repo.EmptyBoxRepository.FindEmptyBoxBarcodeByBarcodeText(boxItem.BarCodeText);
                                _box.BarCodeText = boxItem.BarCodeText;
                                _box.BarCodeId = barcode.BarCodeId;


                            }

                            repo.AssignBoxRepository.InsertOrUpdate(_box);
                            repo.AssignBoxRepository.Save();
                        }
                    }

                    else
                    {
                        //My Change Code ( Sabbir )
                        List<AssignBox> itemListCheck = repo.AssignBoxRepository.GetByItemId(item.ItemId);

                        AssignBox _box = new AssignBox();
                        _box.AssignDate = DateTime.Now;
                        _box.CurrentStatus = 1;

                        if (itemListCheck.Count > 0)
                        {
                            _box.TransmittalINStatusId = 2;
                            _box.WarehouseID = itemListCheck[0].WarehouseID;
                            _box.WarehouseName = itemListCheck[0].WarehouseName;
                        }
                        else
                        {
                            _box.TransmittalINStatusId = 1;
                            _box.WarehouseID = wareHouse.WarehouseID;
                            _box.WarehouseName = wareHouse.WarehouseName;
                        }

                        //_box.TransmittalINStatusId = 1;
                        _box.TransmittalINId = transIN.TransmittalINId;
                        _box.ItemId = item.ItemId;
                        //_box.WarehouseID = wareHouse.WarehouseID;
                        //_box.WarehouseName = wareHouse.WarehouseName;
                        Item boxItem = repo.ItemRepository.Find(item.ItemId);

                        _box.BoxNo = boxItem.BoxNo;
                        _box.BoxName = boxItem.ItemName;
                        _box.Year = boxItem.Year;
                        _box.Category = boxItem.Category;
                        if (boxItem.BarCodeText != null)
                        {
                            EmptyBoxBarcode barcode = repo.EmptyBoxRepository.FindEmptyBoxBarcodeByBarcodeText(boxItem.BarCodeText);
                            if (barcode != null)
                            {
                                _box.BarCodeText = boxItem.BarCodeText;
                                _box.BarCodeId = barcode.BarCodeId;
                            }
                        }
                        _box.DestructionPeriod = boxItem.DestructionPeriod;

                        repo.AssignBoxRepository.InsertOrUpdate(_box);
                        repo.AssignBoxRepository.Save();

                    }
                }

                ///////////
            }
            List<AssignBox> boxListNew = repo.AssignBoxRepository.GetByTrInId(trID);

            return View(boxListNew);
        }

        public ActionResult AddBoxesForTrOUT(long trId, int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            TransmittalOUT transOUT = repo.TransmittalOUTRepository.Find(trId);

            List<AssignBox> assignBoxList = new List<AssignBox>();
            pagedList = new PagedList<AssignBox>(assignBoxList, pageNumber, pageSize);

            if (transOUT.Type == "File")
            {
                ViewBag.TransmittalNo = transOUT.TransmittalNo;
                ViewBag.TransmittalRefNo = transOUT.ClientRequestreference;
                ViewBag.TransmittalType = "Transmittal OUT";
                ViewBag.TransmittalId = transOUT.TransmittalOUTId;
                ViewBag.TransmittalDate = transOUT.TransmittalDate.ToShortDateString();
                ViewBag.ClientName = repo.ClientRepository.Find(transOUT.ClientID).ClientName;
                ViewBag.Department = repo.DepartmentRepository.Find(transOUT.DepartmentID).DepartmentName;
                ViewBag.AllItems = transOUT.Items; //context.TransmittalINs.Where(tr=>tr.TransmittalINId==trId).Include(tr => tr.Items).ToList();

                if (transOUT.TransmittalOUTStatusId > 1)
                {
                    ViewBag.status = 1;
                }
                else if (transOUT.TransmittalOUTStatusId < 2)
                {
                    ViewBag.status = 0;
                }
                ViewBag.File = "1";


                bool IsXL = false;

                try
                {
                    IsXL = Convert.ToBoolean(Session["IsXLUploadFile"]);
                }
                catch
                {
                    IsXL = false;
                }

                //  Session["AssignBoxListForXL"] = finalList;
                //Session["IsXLUpload"] = true;
                if (IsXL == true)
                {

                    pagedList = (PagedList<AssignBox>)Session["AssignBoxListForXLFile"];
                    //  return View();
                }

                else
                {

                    List<Item> itemList = new List<Item>();
                    pagedListItem = new PagedList<Item>(itemList, pageNumber, pageSize);
                    if (transOUT.ProjectId.HasValue)
                    {
                        if (transOUT.ProjectId.Value != -1)
                        {
                            // pagedListItem = repo.ItemRepository.GetByCliendIDandDeptIdandProjectIdFilePagedList(transOUT.ClientID, transOUT.DepartmentID, transOUT.ProjectId.Value,pageNumber,pageSize);
                            pagedList = repo.AssignBoxRepository.GetByCliendIDandDeptIdandProjectIdFilePagedList(transOUT.ClientID, transOUT.DepartmentID, transOUT.ProjectId.Value, pageNumber, pageSize);
                        }
                        else
                        {
                            // pagedListItem = repo.ItemRepository.GetByCliendIDandDeptIdFilePagedList(transOUT.ClientID, transOUT.DepartmentID, pageNumber, pageSize);
                            pagedList = repo.AssignBoxRepository.GetByCliendIDandDeptIdFilePagedList(transOUT.ClientID, transOUT.DepartmentID, pageNumber, pageSize);
                        }
                    }
                    else
                    {
                        //  pagedListItem = repo.ItemRepository.GetByCliendIDandDeptIdFilePagedList(transOUT.ClientID, transOUT.DepartmentID, pageNumber, pageSize);
                        pagedList = repo.AssignBoxRepository.GetByCliendIDandDeptIdFilePagedList(transOUT.ClientID, transOUT.DepartmentID, pageNumber, pageSize);
                    }

                    //foreach (Item item in pagedListItem)
                    //{
                    //    AssignBox _box = new AssignBox();
                    //    _box = repo.AssignBoxRepository.GetByItemIdandStatus(item.ItemId, Convert.ToInt64(EnumHelper.Status.WareHouse_Assigned));

                    //    if (_box != null)
                    //    {

                    //        assignBoxList.Add(_box);
                    //    }

                    //}

                    //pagedList = new PagedList<AssignBox>(assignBoxList, pageNumber, pageSize);

                    //assignBoxList = assignBoxList.OrderBy(b => b.BoxName).ToList();
                }
            }

            if (transOUT.Type == "Box")
            {


                /////////////////////
                ViewBag.TransmittalNo = transOUT.TransmittalNo;
                ViewBag.TransmittalRefNo = transOUT.ClientRequestreference;
                ViewBag.TransmittalType = "Transmittal OUT";
                ViewBag.TransmittalId = transOUT.TransmittalOUTId;
                ViewBag.TransmittalDate = transOUT.TransmittalDate.ToShortDateString();
                ViewBag.ClientName = repo.ClientRepository.Find(transOUT.ClientID).ClientName;
                ViewBag.Department = repo.DepartmentRepository.Find(transOUT.DepartmentID).DepartmentName;
                ViewBag.AllItems = transOUT.Items; //context.TransmittalINs.Where(tr=>tr.TransmittalINId==trId).Include(tr => tr.Items).ToList();

                ViewBag.File = "0";


                if (transOUT.TransmittalOUTStatusId > 1)
                {
                    ViewBag.status = 1;
                }
                else if (transOUT.TransmittalOUTStatusId < 2)
                {
                    ViewBag.status = 0;
                }



                bool IsXL = false;

                try
                {
                    IsXL = Convert.ToBoolean(Session["IsXLUpload"]);
                }
                catch
                {
                    IsXL = false;
                }

                //  Session["AssignBoxListForXL"] = finalList;
                //Session["IsXLUpload"] = true;
                if (IsXL == true)
                {
                    //return View((List<AssignBox>)Session["AssignBoxListForXL"]);
                    pagedList = (PagedList<AssignBox>)Session["AssignBoxListForXL"];
                }
                else
                {


                    List<Item> itemList = new List<Item>();
                    pagedListItem = new PagedList<Item>(itemList, pageNumber, pageSize);
                    if (transOUT.ProjectId.HasValue)
                    {
                        if (transOUT.ProjectId.Value != -1)
                        {
                            //pagedListItem = repo.ItemRepository.GetByCliendIDandDeptIdandProjectIdPagedList(transOUT.ClientID, transOUT.DepartmentID, transOUT.ProjectId.Value,pageNumber,pageSize);
                            pagedList = repo.AssignBoxRepository.GetByCliendIDandDeptIdandProjectIdPagedList(transOUT.ClientID, transOUT.DepartmentID, transOUT.ProjectId.Value, pageNumber, pageSize);
                        }
                        else
                        {
                            //pagedListItem = repo.ItemRepository.GetByCliendIDandDeptIdPagedList(transOUT.ClientID, transOUT.DepartmentID, pageNumber, pageSize);
                            pagedList = repo.AssignBoxRepository.GetByCliendIDandDeptIdPagedList(transOUT.ClientID, transOUT.DepartmentID, pageNumber, pageSize);
                        }
                    }
                    else
                    {
                        //pagedListItem = repo.ItemRepository.GetByCliendIDandDeptIdPagedList(transOUT.ClientID, transOUT.DepartmentID, pageNumber, pageSize);
                        pagedList = repo.AssignBoxRepository.GetByCliendIDandDeptIdPagedList(transOUT.ClientID, transOUT.DepartmentID, pageNumber, pageSize);
                    }




                }


            }
            return View(pagedList);
        }

        //newly modify by kalam 15/06/2021 Start 
        [HttpGet]
        public ActionResult AddBoxesForTrOUT(AssignBox model, string trId, int? page, string searchString, string currentFilter)
        {
            long _trId = Convert.ToInt64(trId);
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            TransmittalOUT transOUT = repo.TransmittalOUTRepository.Find(_trId);
            List<AssignBox> assignBoxList = new List<AssignBox>();
            pagedList = new PagedList<AssignBox>(assignBoxList, pageNumber, pageSize);


            if (transOUT.Type == "File")
            {

                if (model.ItemIds != null)
                {

                    #region Getting The ItemsList

                    List<Item> _itemList = repo.TransmittalOUTRepository.GetAllItemsByTrID(_trId);

                    foreach (var item in model.ItemIds)
                    {
                        Item _item = repo.ItemRepository.Find(item);

                        if (transOUT.Items.Contains(_item))
                        {
                            ViewBag.Flag = 0;
                        }
                        else
                        {
                            _itemList.Add(_item);
                        }

                    }

                    transOUT.Items = _itemList;


                    repo.TransmittalOUTRepository.InsertOrUpdate(transOUT);
                    repo.TransmittalOUTRepository.Save();

                    #endregion
                }
                else
                {
                    ViewBag.Flag = 0;
                }

                TransmittalOUT _transOUT = repo.TransmittalOUTRepository.Find(_trId);

                ViewBag.TransmittalNo = _transOUT.TransmittalNo;
                ViewBag.TransmittalRefNo = _transOUT.ClientRequestreference;
                ViewBag.TransmittalType = "Transmittal OUT";
                ViewBag.TransmittalId = _transOUT.TransmittalOUTId;
                ViewBag.TransmittalDate = _transOUT.TransmittalDate.ToShortDateString();
                ViewBag.ClientName = repo.ClientRepository.Find(transOUT.ClientID).ClientName;
                ViewBag.Department = repo.DepartmentRepository.Find(transOUT.DepartmentID).DepartmentName;


                ViewBag.listofDep = repo.AssignBoxRepository.GetByCliendIDandDeptIdFilePagedList(transOUT.ClientID, transOUT.DepartmentID, pageNumber, pageSize);
                //newly write by k
                ViewBag.DepartmentSearchByClientId = repo.DepartmentRepository.GetAllDepartmentByClientId(transOUT.ClientID);
                //new write end
                ViewBag.AllItems = _transOUT.Items;
                ViewBag.File = "1";

                if (transOUT.TransmittalOUTStatusId > 1)
                {
                    ViewBag.status = 1;
                }
                else if (transOUT.TransmittalOUTStatusId < 2)
                {
                    ViewBag.status = 0;
                }


                bool IsXL = false;

                try
                {
                    IsXL = Convert.ToBoolean(Session["IsXLUploadFile"]);
                }
                catch
                {
                    IsXL = false;
                }


                if (IsXL == true)
                {

                    assignBoxList = (List<AssignBox>)Session["AssignBoxListForXLFile"];
                    pagedList = new PagedList<AssignBox>(assignBoxList, pageNumber, pageSize);
                }


                else
                {

                    List<Item> itemList = new List<Item>();
                    pagedListItem = new PagedList<Item>(itemList, pageNumber, pageSize);
                    if (transOUT.ProjectId.HasValue)
                    {
                        if (transOUT.ProjectId.Value != -1)
                        {

                            pagedList = repo.AssignBoxRepository.GetByCliendIDandDeptIdandProjectIdFilePagedList(transOUT.ClientID, transOUT.DepartmentID, transOUT.ProjectId.Value, pageNumber, pageSize);
                        }
                        else
                        {

                            pagedList = repo.AssignBoxRepository.GetByCliendIDandDeptIdFilePagedList(transOUT.ClientID, transOUT.DepartmentID, pageNumber, pageSize);
                        }
                    }
                    else
                    {

                        pagedList = repo.AssignBoxRepository.GetByCliendIDandDeptIdFilePagedList(transOUT.ClientID, transOUT.DepartmentID, pageNumber, pageSize);
                    }


                }

            }
            if (transOUT.Type == "Box")
            {


                ////////////////////

                if (model.ItemIds != null)
                {

                    #region Getting The ItemsList

                    List<Item> _itemList = repo.TransmittalOUTRepository.GetAllItemsByTrID(_trId);

                    foreach (var item in model.ItemIds)
                    {
                        Item _item = repo.ItemRepository.Find(item);

                        if (transOUT.Items.Contains(_item))
                        {
                            ViewBag.Flag = 0;

                        }
                        else
                        {
                            _itemList.Add(_item);
                        }

                    }

                    transOUT.Items = _itemList;


                    repo.TransmittalOUTRepository.InsertOrUpdate(transOUT);
                    repo.TransmittalOUTRepository.Save();

                    #endregion
                }
                else
                {
                    ViewBag.Flag = 0;
                }



                TransmittalOUT _transOUT = repo.TransmittalOUTRepository.Find(_trId);

                ViewBag.TransmittalNo = _transOUT.TransmittalNo;
                ViewBag.TransmittalRefNo = _transOUT.ClientRequestreference;
                ViewBag.TransmittalType = "Transmittal OUT";
                ViewBag.TransmittalId = _transOUT.TransmittalOUTId;
                ViewBag.TransmittalDate = _transOUT.TransmittalDate.ToShortDateString();
                ViewBag.ClientName = repo.ClientRepository.Find(transOUT.ClientID).ClientName;
                ViewBag.Department = repo.DepartmentRepository.Find(transOUT.DepartmentID).DepartmentName;

                //new write by k
                ViewBag.DepartmentSearchByClientId = repo.DepartmentRepository.GetAllDepartmentByClientId(transOUT.ClientID);
                //new write end



                ViewBag.AllItems = _transOUT.Items;
                ViewBag.File = "0";

                if (transOUT.TransmittalOUTStatusId > 1)
                {
                    ViewBag.status = 1;
                }
                else if (transOUT.TransmittalOUTStatusId < 2)
                {
                    ViewBag.status = 0;
                }


                bool IsXL = false;

                try
                {
                    IsXL = Convert.ToBoolean(Session["IsXLUpload"]);
                }
                catch
                {
                    IsXL = false;
                }

                //New Write

                if (searchString != null)
                {
                    pageNumber = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                if (IsXL == true)
                {


                    assignBoxList = (List<AssignBox>)Session["AssignBoxListForXL"];
                    pagedList = new PagedList<AssignBox>(assignBoxList, pageNumber, pageSize);
                }
                else
                {


                    List<Item> itemList = new List<Item>();
                    pagedListItem = new PagedList<Item>(itemList, pageNumber, pageSize);
                    if (transOUT.ProjectId.HasValue)
                    {
                        if (transOUT.ProjectId.Value != -1)
                        {

                            pagedList = repo.AssignBoxRepository.GetByCliendIDandDeptIdandProjectIdPagedList2(transOUT.ClientID, transOUT.DepartmentID, transOUT.ProjectId.Value, searchString, pageNumber, pageSize);

                        }
                        else
                        {

                            pagedList = repo.AssignBoxRepository.GetByCliendIDandDeptIdPagedList2(transOUT.ClientID, transOUT.DepartmentID, searchString, pageNumber, pageSize);
                        }
                    }
                    else
                    {

                        pagedList = repo.AssignBoxRepository.GetByCliendIDandDeptIdPagedList2(transOUT.ClientID, transOUT.DepartmentID, searchString, pageNumber, pageSize);
                    }

                }

            }

            return View(pagedList);
        }

        // End 15/06/2021

        //[HttpGet]
        //public ActionResult AddBoxesForTrOUT(AssignBox model, string trId, int? page)
        //{
        //    long _trId = Convert.ToInt64(trId);
        //    int pageSize = 10;
        //    int pageNumber = (page ?? 1);

        //    TransmittalOUT transOUT = repo.TransmittalOUTRepository.Find(_trId);
        //    List<AssignBox> assignBoxList = new List<AssignBox>();
        //    pagedList = new PagedList<AssignBox>(assignBoxList, pageNumber, pageSize);


        //    if (transOUT.Type == "File")
        //    {
        //        if (model.ItemIds != null)
        //        {

        //            #region Getting The ItemsList

        //            List<Item> _itemList = repo.TransmittalOUTRepository.GetAllItemsByTrID(_trId);

        //            foreach (var item in model.ItemIds)
        //            {
        //                Item _item = repo.ItemRepository.Find(item);

        //                if (transOUT.Items.Contains(_item))
        //                {
        //                    ViewBag.Flag = false;
        //                }
        //                else
        //                {
        //                    _itemList.Add(_item);
        //                }

        //            }

        //            transOUT.Items = _itemList;


        //            repo.TransmittalOUTRepository.InsertOrUpdate(transOUT);
        //            repo.TransmittalOUTRepository.Save();

        //            #endregion
        //        }
        //        else
        //        {
        //            ViewBag.Flag = false;
        //        }



        //        TransmittalOUT _transOUT = repo.TransmittalOUTRepository.Find(_trId);

        //        ViewBag.TransmittalNo = _transOUT.TransmittalNo;
        //        ViewBag.TransmittalRefNo = _transOUT.ClientRequestreference;
        //        ViewBag.TransmittalType = "Transmittal OUT";
        //        ViewBag.TransmittalId = _transOUT.TransmittalOUTId;
        //        ViewBag.TransmittalDate = _transOUT.TransmittalDate.ToShortDateString();
        //        ViewBag.ClientName = repo.ClientRepository.Find(transOUT.ClientID).ClientName;
        //        ViewBag.Department = repo.DepartmentRepository.Find(transOUT.DepartmentID).DepartmentName;
        //        ViewBag.AllItems = _transOUT.Items;
        //        ViewBag.File = "1";

        //        if (transOUT.TransmittalOUTStatusId > 1)
        //        {
        //            ViewBag.status = 1;
        //        }
        //        else if (transOUT.TransmittalOUTStatusId < 2)
        //        {
        //            ViewBag.status = 0;
        //        }


        //        bool IsXL = false;

        //        try
        //        {
        //            IsXL = Convert.ToBoolean(Session["IsXLUploadFile"]);
        //        }
        //        catch
        //        {
        //            IsXL = false;
        //        }

        //        //  Session["AssignBoxListForXL"] = finalList;
        //        //Session["IsXLUpload"] = true;
        //        if (IsXL == true)
        //        {

        //            assignBoxList = (List<AssignBox>)Session["AssignBoxListForXLFile"];
        //            pagedList = new PagedList<AssignBox>(assignBoxList, pageNumber, pageSize);
        //        }


        //        else
        //        {

        //            List<Item> itemList = new List<Item>();
        //            pagedListItem = new PagedList<Item>(itemList, pageNumber, pageSize);
        //            if (transOUT.ProjectId.HasValue)
        //            {
        //                if (transOUT.ProjectId.Value != -1)
        //                {
        //                    // pagedListItem = repo.ItemRepository.GetByCliendIDandDeptIdandProjectIdFilePagedList(transOUT.ClientID, transOUT.DepartmentID, transOUT.ProjectId.Value,pageNumber,pageSize);
        //                    pagedList = repo.AssignBoxRepository.GetByCliendIDandDeptIdandProjectIdFilePagedList(transOUT.ClientID, transOUT.DepartmentID, transOUT.ProjectId.Value, pageNumber, pageSize);
        //                }
        //                else
        //                {
        //                    // pagedListItem = repo.ItemRepository.GetByCliendIDandDeptIdFilePagedList(transOUT.ClientID, transOUT.DepartmentID, pageNumber, pageSize);
        //                    pagedList = repo.AssignBoxRepository.GetByCliendIDandDeptIdFilePagedList(transOUT.ClientID, transOUT.DepartmentID, pageNumber, pageSize);
        //                }
        //            }
        //            else
        //            {
        //                //  pagedListItem = repo.ItemRepository.GetByCliendIDandDeptIdFilePagedList(transOUT.ClientID, transOUT.DepartmentID, pageNumber, pageSize);
        //                pagedList = repo.AssignBoxRepository.GetByCliendIDandDeptIdFilePagedList(transOUT.ClientID, transOUT.DepartmentID, pageNumber, pageSize);
        //            }

        //            //foreach (Item item in pagedListItem)
        //            //{
        //            //    AssignBox _box = new AssignBox();
        //            //    _box = repo.AssignBoxRepository.GetByItemIdandStatus(item.ItemId, Convert.ToInt64(EnumHelper.Status.WareHouse_Assigned));

        //            //    if (_box != null)
        //            //    {

        //            //        assignBoxList.Add(_box);
        //            //    }

        //            //}

        //            //pagedList = new PagedList<AssignBox>(assignBoxList, pageNumber, pageSize);

        //            //assignBoxList = assignBoxList.OrderBy(b => b.BoxName).ToList();
        //        }

        //    }
        //    if (transOUT.Type == "Box")
        //    {



        //        ////////////////////

        //        if (model.ItemIds != null)
        //        {

        //            #region Getting The ItemsList

        //            List<Item> _itemList = repo.TransmittalOUTRepository.GetAllItemsByTrID(_trId);

        //            foreach (var item in model.ItemIds)
        //            {
        //                Item _item = repo.ItemRepository.Find(item);

        //                if (transOUT.Items.Contains(_item))
        //                {
        //                    ViewBag.Flag = false;
        //                }
        //                else
        //                {
        //                    _itemList.Add(_item);
        //                }

        //            }

        //            transOUT.Items = _itemList;


        //            repo.TransmittalOUTRepository.InsertOrUpdate(transOUT);
        //            repo.TransmittalOUTRepository.Save();

        //            #endregion
        //        }
        //        else
        //        {
        //            ViewBag.Flag = false;
        //        }





        //        TransmittalOUT _transOUT = repo.TransmittalOUTRepository.Find(_trId);

        //        ViewBag.TransmittalNo = _transOUT.TransmittalNo;
        //        ViewBag.TransmittalRefNo = _transOUT.ClientRequestreference;
        //        ViewBag.TransmittalType = "Transmittal OUT";
        //        ViewBag.TransmittalId = _transOUT.TransmittalOUTId;
        //        ViewBag.TransmittalDate = _transOUT.TransmittalDate.ToShortDateString();
        //        ViewBag.ClientName = repo.ClientRepository.Find(transOUT.ClientID).ClientName;
        //        ViewBag.Department = repo.DepartmentRepository.Find(transOUT.DepartmentID).DepartmentName;
        //        ViewBag.AllItems = _transOUT.Items;
        //        ViewBag.File = "0";

        //        if (transOUT.TransmittalOUTStatusId > 1)
        //        {
        //            ViewBag.status = 1;
        //        }
        //        else if (transOUT.TransmittalOUTStatusId < 2)
        //        {
        //            ViewBag.status = 0;
        //        }


        //        bool IsXL = false;

        //        try
        //        {
        //            IsXL = Convert.ToBoolean(Session["IsXLUpload"]);
        //        }
        //        catch
        //        {
        //            IsXL = false;
        //        }

        //        //  Session["AssignBoxListForXL"] = finalList;
        //        //Session["IsXLUpload"] = true;
        //        if (IsXL == true)
        //        {
        //            //return View((List<AssignBox>)Session["AssignBoxListForXL"]);
        //            //pagedList = (PagedList<AssignBox>)Session["AssignBoxListForXL"];

        //            assignBoxList = (List<AssignBox>)Session["AssignBoxListForXL"];
        //            pagedList = new PagedList<AssignBox>(assignBoxList, pageNumber, pageSize);
        //        }
        //        else
        //        {


        //            List<Item> itemList = new List<Item>();
        //            pagedListItem = new PagedList<Item>(itemList, pageNumber, pageSize);
        //            if (transOUT.ProjectId.HasValue)
        //            {
        //                if (transOUT.ProjectId.Value != -1)
        //                {
        //                    //pagedListItem = repo.ItemRepository.GetByCliendIDandDeptIdandProjectIdPagedList(transOUT.ClientID, transOUT.DepartmentID, transOUT.ProjectId.Value,pageNumber,pageSize);
        //                    pagedList = repo.AssignBoxRepository.GetByCliendIDandDeptIdandProjectIdPagedList(transOUT.ClientID, transOUT.DepartmentID, transOUT.ProjectId.Value, pageNumber, pageSize);

        //                }
        //                else
        //                {
        //                    //pagedListItem = repo.ItemRepository.GetByCliendIDandDeptIdPagedList(transOUT.ClientID, transOUT.DepartmentID, pageNumber, pageSize);
        //                    pagedList = repo.AssignBoxRepository.GetByCliendIDandDeptIdPagedList(transOUT.ClientID, transOUT.DepartmentID, pageNumber, pageSize);
        //                }
        //            }
        //            else
        //            {
        //                //pagedListItem = repo.ItemRepository.GetByCliendIDandDeptIdPagedList(transOUT.ClientID, transOUT.DepartmentID, pageNumber, pageSize);
        //                pagedList = repo.AssignBoxRepository.GetByCliendIDandDeptIdPagedList(transOUT.ClientID, transOUT.DepartmentID, pageNumber, pageSize);
        //            }

        //            //foreach (Item item in pagedListItem)
        //            //{
        //            //    AssignBox _box = new AssignBox();
        //            //    _box = repo.AssignBoxRepository.GetByItemIdandStatus(item.ItemId, Convert.ToInt64(EnumHelper.Status.WareHouse_Assigned));

        //            //    if (_box != null)
        //            //    {
        //            //        assignBoxList.Add(_box);
        //            //    }

        //            //}

        //            ////assignBoxList = assignBoxList.OrderBy(b => b.BoxName).ToList();
        //            //pagedList = new PagedList<AssignBox>(assignBoxList, pageNumber, pageSize);
        //            ///////////////////
        //        }
        //    }

        //    return View(pagedList);
        //}

        public ActionResult XLUploadwithFileForBox(HttpPostedFileBase filename, string trId2)
        {
            if (filename != null && filename.ContentLength > 0)
            {
                // extract only the fielname
                string _name = Path.GetFileName(filename.FileName);
                _name = DateTime.Now.Ticks.ToString() + _name;
                //// store the file inside ~/App_Data/uploads folder
                //var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                var path = Path.Combine(Server.MapPath("~/Content/Images/") + ConfigurationManager.AppSettings["UploadedDocument"].ToString() + "XLUpload/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }


                filename.SaveAs(Path.Combine(path, _name));
                string Extension = Path.GetExtension(filename.FileName);

                path = path + _name;

                bool flag = Import_To_Grid(path, Extension, trId2);

                try
                {
                    System.IO.File.Delete(path);
                }
                catch (Exception e)
                {
                }






            }
            return RedirectToAction("AddBoxesForTrOUT", new { trId = Convert.ToInt64(trId2) });
        }

        public ActionResult XLUploadwithFileForFile(HttpPostedFileBase filename, string trId3)
        {
            if (filename != null && filename.ContentLength > 0)
            {
                // extract only the fielname
                string _name = Path.GetFileName(filename.FileName);
                _name = DateTime.Now.Ticks.ToString() + _name;
                //// store the file inside ~/App_Data/uploads folder
                //var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                var path = Path.Combine(Server.MapPath("~/Content/Images/") + ConfigurationManager.AppSettings["UploadedDocument"].ToString() + "XLUpload/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }


                filename.SaveAs(Path.Combine(path, _name));
                string Extension = Path.GetExtension(filename.FileName);

                path = path + _name;

                bool flag = Import_To_GridFile(path, Extension, trId3);

                try
                {
                    System.IO.File.Delete(path);
                }
                catch (Exception e)
                {
                }






            }


            return RedirectToAction("AddBoxesForTrOUT", new { trId = Convert.ToInt64(trId3) });
        }

        private bool Import_To_GridFile(string FilePath, string Extension, string trId)
        {
            bool flag = false;

            string conStr = "";

            switch (Extension)
            {

                case ".xls": //Excel 97-03

                    conStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FilePath + ";Extended Properties=Excel 8.0;Persist Security Info=False";

                    break;

                case ".xlsx": //Excel 07

                    conStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath + ";Extended Properties=Excel 12.0;Persist Security Info=False";

                    break;

            }





            conStr = String.Format(conStr, FilePath);

            OleDbConnection connExcel = new OleDbConnection(conStr);


            OleDbCommand cmdExcel = new OleDbCommand();

            OleDbDataAdapter oda = new OleDbDataAdapter();

            DataTable dt = new DataTable();

            cmdExcel.Connection = connExcel;



            //Get the name of First Sheet

            connExcel.Open();

            DataTable dtExcelSchema;

            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();





            //Read Data from First Sheet

            //connExcel.Open();

            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";

            oda.SelectCommand = cmdExcel;

            oda.Fill(dt);
            if (dt.Rows.Count <= 0)
            {
                SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";

                oda.SelectCommand = cmdExcel;

                oda.Fill(dt);
            }
            //connExcel.Close();
            connExcel.Close();



            try
            {
                SaveDataFile(dt, trId);
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                // ShowSuccessResult("invalid file formate");
            }

            return flag;

        }


        private void SaveDataFile(DataTable dt, string trId)
        {
            List<AssignBox> finalListFile = new List<AssignBox>();
            foreach (DataRow dr in dt.Rows)
            {
                string boxNo = dr["BoxNo"].ToString();
                string bookNo = dr["BoxName"].ToString();
                string fileNumber = dr["FileNumber"].ToString();
                string refNumber = dr["ReferrenceNumber"].ToString();
                string ringNumber = dr["RingNumber"].ToString();
                string accountNumber = dr["AccountNumber"].ToString();
                string year = dr["Year"].ToString();
                string clientName = dr["ClientName"].ToString();
                string departmentName = dr["Department"].ToString();

                Client objclient = new Client();
                objclient = repo.ClientRepository.GetByClientName(clientName);

                if (objclient != null)
                {

                    Department objdepartment = new Department();
                    // objdepartment = repo.DepartmentRepository.GetByDeptName(departmentName);
                    objdepartment = repo.DepartmentRepository.GetByClientIDandDeptName(objclient.ClientID, departmentName);
                    AssignBox objAssignBox = new AssignBox();


                    if (objdepartment.Client.ClientName.Trim() == clientName)
                    {
                        AssignBox assignBox = new AssignBox();
                        if (year == "" || year == null || year == string.Empty)
                        {
                            assignBox = repo.AssignBoxRepository.GetByClientIdandDeptIDwithoutYearAndStatusFile(objdepartment.DepartmentID, objdepartment.ClientID, boxNo.Trim(), bookNo.Trim(), fileNumber.Trim(), refNumber.Trim(), ringNumber.Trim(), accountNumber.Trim(), Convert.ToInt64(EnumHelper.Status.WareHouse_Assigned));
                        }
                        else
                        {
                            assignBox = repo.AssignBoxRepository.GetByClientIdandDeptIDAndStatusFile(objdepartment.DepartmentID, objdepartment.ClientID, boxNo.Trim(), bookNo.Trim(), fileNumber.Trim(), refNumber.Trim(), ringNumber.Trim(), accountNumber.Trim(), year.Trim(), Convert.ToInt64(EnumHelper.Status.WareHouse_Assigned));
                        }
                        if (assignBox != null)
                            finalListFile.Add(assignBox);

                    }
                }
            }

            Session["AssignBoxListForXLFile"] = finalListFile;
            Session["IsXLUploadFile"] = true;


        }

        private bool Import_To_Grid(string FilePath, string Extension, string trId)
        {
            bool flag = false;

            string conStr = "";

            switch (Extension)
            {

                case ".xls": //Excel 97-03

                    conStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FilePath + ";Extended Properties=Excel 8.0;Persist Security Info=False";

                    break;

                case ".xlsx": //Excel 07

                    conStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath + ";Extended Properties=Excel 12.0;Persist Security Info=False";

                    break;

            }





            conStr = String.Format(conStr, FilePath);

            OleDbConnection connExcel = new OleDbConnection(conStr);


            OleDbCommand cmdExcel = new OleDbCommand();

            OleDbDataAdapter oda = new OleDbDataAdapter();

            DataTable dt = new DataTable();

            cmdExcel.Connection = connExcel;



            //Get the name of First Sheet

            connExcel.Open();

            DataTable dtExcelSchema;

            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();





            //Read Data from First Sheet

            //connExcel.Open();

            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";

            oda.SelectCommand = cmdExcel;

            oda.Fill(dt);
            if (dt.Rows.Count <= 0)
            {
                SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";

                oda.SelectCommand = cmdExcel;

                oda.Fill(dt);
            }
            //connExcel.Close();
            connExcel.Close();



            try
            {
                SaveData(dt, trId);
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                // ShowSuccessResult("invalid file formate");
            }

            return flag;

        }

        private void SaveData(DataTable dt, string trId)
        {
            List<AssignBox> finalList = new List<AssignBox>();
            foreach (DataRow dr in dt.Rows)
            {
                string boxName = dr["Box Name"].ToString();
                string boxNo = dr["Box No"].ToString();
                string year = dr["Year"].ToString();
                string clientName = dr["Client Name"].ToString();
                string departmentName = dr["Department Name"].ToString();
                string ItemNo = dr["ItemNo"].ToString();

                Client objclient = new Client();
                objclient = repo.ClientRepository.GetByClientName(clientName);

                if (objclient != null)
                {
                    Department objdepartment = new Department();
                    //objdepartment = repo.DepartmentRepository.GetByDeptName(departmentName);
                    objdepartment = repo.DepartmentRepository.GetByClientIDandDeptName(objclient.ClientID, departmentName);
                    AssignBox objAssignBox = new AssignBox();


                    if (objdepartment.Client.ClientName.Trim() == clientName)
                    {
                        AssignBox assignBox = new AssignBox();
                        if (year == "" || year == null || year == string.Empty)
                        {
                            //  assignBox = repo.AssignBoxRepository.GetByClientIdandDeptIDwithoutYearAndStatus(objdepartment.DepartmentID, objdepartment.ClientID, boxName.Trim(), boxNo.Trim(), Convert.ToInt64(EnumHelper.Status.WareHouse_Assigned),Convert.ToInt64(ItemNo));

                            assignBox = repo.AssignBoxRepository.GetByItemIdAndStatus(Convert.ToInt64(ItemNo), Convert.ToInt64(EnumHelper.Status.WareHouse_Assigned));
                        }
                        else
                        {
                            // assignBox = repo.AssignBoxRepository.GetByClientIdandDeptIDAndStatus(objdepartment.DepartmentID, objdepartment.ClientID, boxName.Trim(), boxNo.Trim(), year.Trim(), Convert.ToInt64(EnumHelper.Status.WareHouse_Assigned), Convert.ToInt64(ItemNo));
                            assignBox = repo.AssignBoxRepository.GetByItemIdAndStatus(Convert.ToInt64(ItemNo), Convert.ToInt64(EnumHelper.Status.WareHouse_Assigned));
                        }
                        if (assignBox != null)
                            finalList.Add(assignBox);

                    }
                }
            }

            Session["AssignBoxListForXL"] = finalList;
            Session["IsXLUpload"] = true;


        }




        public ActionResult RemoveBOXforTrOUT(long id, long trID)
        {
            long _trId = Convert.ToInt64(trID);

            int pageSize = 10;
            int pageNumber = 1;

            TransmittalOUT transOUT = repo.TransmittalOUTRepository.Find(_trId);
            List<AssignBox> assignBoxList = new List<AssignBox>();
            pagedList = new PagedList<AssignBox>(assignBoxList, pageNumber, pageSize);
            ////////
            if (transOUT.Type == "File")
            {
                Item _item = repo.ItemRepository.Find(id);

                transOUT.Items.Remove(_item);
                repo.TransmittalOUTRepository.InsertOrUpdate(transOUT);
                repo.TransmittalOUTRepository.Save();

                TransmittalOUT _transOUT = repo.TransmittalOUTRepository.Find(_trId);

                ViewBag.TransmittalNo = _transOUT.TransmittalNo;
                ViewBag.TransmittalRefNo = _transOUT.ClientRequestreference;
                ViewBag.TransmittalType = "Transmittal OUT";
                ViewBag.TransmittalId = _transOUT.TransmittalOUTId;
                ViewBag.TransmittalDate = _transOUT.TransmittalDate.ToShortDateString();
                ViewBag.ClientName = repo.ClientRepository.Find(transOUT.ClientID).ClientName;
                ViewBag.Department = repo.DepartmentRepository.Find(transOUT.DepartmentID).DepartmentName;
                ViewBag.AllItems = _transOUT.Items;
                ViewBag.File = "1";
                if (transOUT.TransmittalOUTStatusId > 1)
                {
                    ViewBag.status = 1;
                }
                else if (transOUT.TransmittalOUTStatusId < 2)
                {
                    ViewBag.status = 0;
                }



                bool IsXL = false;

                try
                {
                    IsXL = Convert.ToBoolean(Session["IsXLUploadFile"]);
                }
                catch
                {
                    IsXL = false;
                }

                //  Session["AssignBoxListForXL"] = finalList;
                //Session["IsXLUpload"] = true;
                if (IsXL == true)
                {

                    assignBoxList = (List<AssignBox>)Session["AssignBoxListForXLFile"];
                    pagedList = new PagedList<AssignBox>(assignBoxList, pageNumber, pageSize);
                }

                else
                {

                    List<Item> itemList = new List<Item>();
                    pagedListItem = new PagedList<Item>(itemList, pageNumber, pageSize);
                    if (transOUT.ProjectId.HasValue)
                    {
                        if (transOUT.ProjectId.Value != -1)
                        {
                            // pagedListItem = repo.ItemRepository.GetByCliendIDandDeptIdandProjectIdFilePagedList(transOUT.ClientID, transOUT.DepartmentID, transOUT.ProjectId.Value,pageNumber,pageSize);
                            pagedList = repo.AssignBoxRepository.GetByCliendIDandDeptIdandProjectIdFilePagedList(transOUT.ClientID, transOUT.DepartmentID, transOUT.ProjectId.Value, pageNumber, pageSize);
                        }
                        else
                        {
                            // pagedListItem = repo.ItemRepository.GetByCliendIDandDeptIdFilePagedList(transOUT.ClientID, transOUT.DepartmentID, pageNumber, pageSize);
                            pagedList = repo.AssignBoxRepository.GetByCliendIDandDeptIdFilePagedList(transOUT.ClientID, transOUT.DepartmentID, pageNumber, pageSize);
                        }
                    }
                    else
                    {
                        //  pagedListItem = repo.ItemRepository.GetByCliendIDandDeptIdFilePagedList(transOUT.ClientID, transOUT.DepartmentID, pageNumber, pageSize);
                        pagedList = repo.AssignBoxRepository.GetByCliendIDandDeptIdFilePagedList(transOUT.ClientID, transOUT.DepartmentID, pageNumber, pageSize);
                    }

                }



            }
            if (transOUT.Type == "Box")
            {


                Item _item = repo.ItemRepository.Find(id);

                transOUT.Items.Remove(_item);
                repo.TransmittalOUTRepository.InsertOrUpdate(transOUT);
                repo.TransmittalOUTRepository.Save();

                TransmittalOUT _transOUT = repo.TransmittalOUTRepository.Find(_trId);

                ViewBag.TransmittalNo = _transOUT.TransmittalNo;
                ViewBag.TransmittalRefNo = _transOUT.ClientRequestreference;
                ViewBag.TransmittalType = "Transmittal OUT";
                ViewBag.TransmittalId = _transOUT.TransmittalOUTId;
                ViewBag.TransmittalDate = _transOUT.TransmittalDate.ToShortDateString();
                ViewBag.ClientName = repo.ClientRepository.Find(transOUT.ClientID).ClientName;
                ViewBag.Department = repo.DepartmentRepository.Find(transOUT.DepartmentID).DepartmentName;
                ViewBag.AllItems = _transOUT.Items;
                ViewBag.File = "0";


                if (transOUT.TransmittalOUTStatusId > 1)
                {
                    ViewBag.status = 1;
                }
                else if (transOUT.TransmittalOUTStatusId < 2)
                {
                    ViewBag.status = 0;
                }


                bool IsXL = false;

                try
                {
                    IsXL = Convert.ToBoolean(Session["IsXLUpload"]);
                }
                catch
                {
                    IsXL = false;
                }

                //  Session["AssignBoxListForXL"] = finalList;
                //Session["IsXLUpload"] = true;
                if (IsXL == true)
                {
                    // return View("AddBoxesForTrOUT", (List<AssignBox>)Session["AssignBoxListForXL"]);

                    assignBoxList = (List<AssignBox>)Session["AssignBoxListForXL"];
                    pagedList = new PagedList<AssignBox>(assignBoxList, pageNumber, pageSize);
                }
                else
                {


                    List<Item> itemList = new List<Item>();
                    pagedListItem = new PagedList<Item>(itemList, pageNumber, pageSize);
                    if (transOUT.ProjectId.HasValue)
                    {
                        if (transOUT.ProjectId.Value != -1)
                        {
                            //pagedListItem = repo.ItemRepository.GetByCliendIDandDeptIdandProjectIdPagedList(transOUT.ClientID, transOUT.DepartmentID, transOUT.ProjectId.Value,pageNumber,pageSize);
                            pagedList = repo.AssignBoxRepository.GetByCliendIDandDeptIdandProjectIdPagedList(transOUT.ClientID, transOUT.DepartmentID, transOUT.ProjectId.Value, pageNumber, pageSize);
                        }
                        else
                        {
                            //pagedListItem = repo.ItemRepository.GetByCliendIDandDeptIdPagedList(transOUT.ClientID, transOUT.DepartmentID, pageNumber, pageSize);
                            pagedList = repo.AssignBoxRepository.GetByCliendIDandDeptIdPagedList(transOUT.ClientID, transOUT.DepartmentID, pageNumber, pageSize);
                        }
                    }
                    else
                    {
                        //pagedListItem = repo.ItemRepository.GetByCliendIDandDeptIdPagedList(transOUT.ClientID, transOUT.DepartmentID, pageNumber, pageSize);
                        pagedList = repo.AssignBoxRepository.GetByCliendIDandDeptIdPagedList(transOUT.ClientID, transOUT.DepartmentID, pageNumber, pageSize);
                    }


                }
            }
            return View("AddBoxesForTrOUT", pagedList);
        }

        #region OLD Method (Assign All Items At the Same Time)

        [HttpPost]
        public ActionResult AssignBoxes(AssignBox model)
        {
            model.AssignDate = DateTime.Now;

            long trID = Convert.ToInt64(Request.QueryString["trID"].ToString());
            model.TransmittalINId = trID;

            if (model.ItemIds != null && model.WarehouseID != -1)
            {


                if (ModelState.IsValid)
                {


                    #region Getting The ItemsList


                    foreach (var item in model.ItemIds)
                    {
                        long itemId = Convert.ToInt64(item);
                        AssignBox _boxAssign = repo.AssignBoxRepository.GetByItemIdandTrINId(item, Convert.ToInt32(model.TransmittalINId));

                        _boxAssign.TransmittalINStatusId = 2;
                        _boxAssign.WarehouseID = model.WarehouseID;


                        Warehouse wareHouse = repo.WarehouseRepository.Find(model.WarehouseID);
                        _boxAssign.WarehouseName = wareHouse.WarehouseName;

                        repo.AssignBoxRepository.InsertOrUpdate(_boxAssign);
                        repo.AssignBoxRepository.Save();
                        ViewBag.Assigned = 1;

                        #endregion

                        TransmittalIN transIN = repo.TransmittalINRepository.GetTrIN(model.TransmittalINId);

                        ViewBag.TransmittalNo = transIN.TransmittalNo;
                        ViewBag.TransmittalRefNo = transIN.ClientRequestreference;
                        ViewBag.TransmittalType = "Transmittal IN";
                        ViewBag.TransmittalId = transIN.TransmittalINId;
                        ViewBag.TransmittalDate = transIN.TransmittalDate.ToShortDateString();
                        ViewBag.AllItems = transIN.Items;
                        ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
                        if (transIN.Type == "File")
                        {
                            ViewBag.File = "1";
                        }
                        if (transIN.Type == "Box")
                        {
                            ViewBag.File = "0";
                        }
                    }
                    ViewBag.Check = true;

                    #region Check If All Assigned
                    List<AssignBox> boxListForChk = repo.AssignBoxRepository.GetByTrInId(Convert.ToInt32(trID));
                    int _totalAssigned = 0;

                    foreach (var item in boxListForChk)
                    {
                        if (item.TransmittalINStatusId > 1)
                        {
                            _totalAssigned++;
                        }

                    }

                    int _diffr = Convert.ToInt32(boxListForChk.Count()) - _totalAssigned;
                    ViewBag.DoneState = _diffr;
                    #endregion
                }
            }

            else
            {
                ViewBag.Check = false;

                TransmittalIN transIN = repo.TransmittalINRepository.GetTrIN(trID);

                ViewBag.TransmittalNo = transIN.TransmittalNo;
                ViewBag.TransmittalRefNo = transIN.ClientRequestreference;
                ViewBag.TransmittalType = "Transmittal IN";
                ViewBag.TransmittalId = transIN.TransmittalINId;
                ViewBag.TransmittalDate = transIN.TransmittalDate.ToShortDateString();
                ViewBag.AllItems = transIN.Items;
                ViewBag.WID = model.WarehouseID;
                if (transIN.Type == "File")
                {
                    ViewBag.File = "1";
                }
                if (transIN.Type == "Box")
                {
                    ViewBag.File = "0";
                }
                ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
                //List<AssignBox> boxList = repo.AssignBoxRepository.GetByTrInId(Convert.ToInt32(trID));

                //return View(boxList);
            }


            // ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
            List<AssignBox> boxListNew = repo.AssignBoxRepository.GetByTrInId(Convert.ToInt32(trID));

            return View(boxListNew);
        }

        #endregion

        #region NewMethod

        //[HttpPost]
        //public ActionResult AssignBoxes(AssignBox model)
        //{
        //    model.AssignDate = DateTime.Now;

        //        long trID = Convert.ToInt64(Request.QueryString["trID"].ToString());
        //        model.TransmittalINId = trID;

        //        if (model.ItemIds != null && model.WarehouseID!=-1)
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                foreach (var item in model.ItemIds)
        //                {

        //                    long itemId = Convert.ToInt64(item);
        //                    AssignBox _boxAssign = repo.AssignBoxRepository.GetByItemIdandTrINId(item, Convert.ToInt32(model.TransmittalINId));

        //                    _boxAssign.IsAssigned = true;
        //                    _boxAssign.WarehouseID = model.WarehouseID;

        //                    Warehouse wareHouse = repo.WarehouseRepository.Find(model.WarehouseID);
        //                    _boxAssign.WarehouseName = wareHouse.WarehouseName;

        //                    repo.AssignBoxRepository.InsertOrUpdate(_boxAssign);
        //                    repo.AssignBoxRepository.Save();


        //                }

        //            }
        //            List<AssignBox> AssignedBoxList = repo.AssignBoxRepository.GetByTrIDForChallan(trID);

        //            TransmittalIN transIN = repo.TransmittalINRepository.GetTrIN(model.TransmittalINId);

        //            ViewBag.TransmittalNo = transIN.TransmittalNo;
        //            ViewBag.TransmittalRefNo = transIN.ClientRequestreference;
        //            ViewBag.TransmittalType = "Transmittal IN";
        //            ViewBag.TransmittalId = transIN.TransmittalINId;
        //            ViewBag.TransmittalDate = transIN.TransmittalDate.ToShortDateString();
        //            ViewBag.AllItems = transIN.Items;
        //            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
        //            ViewBag.Check = true;
        //            ViewBag.WID = model.WarehouseID;




        //            return View("Challan", AssignedBoxList);
        //        }

        //        else
        //        {
        //            ViewBag.Check = false;

        //            TransmittalIN transIN = repo.TransmittalINRepository.GetTrIN(trID);

        //            ViewBag.TransmittalNo = transIN.TransmittalNo;
        //            ViewBag.TransmittalRefNo = transIN.ClientRequestreference;
        //            ViewBag.TransmittalType = "Transmittal IN";
        //            ViewBag.TransmittalId = transIN.TransmittalINId;
        //            ViewBag.TransmittalDate = transIN.TransmittalDate.ToShortDateString();
        //            ViewBag.AllItems = transIN.Items;
        //            ViewBag.WID = model.WarehouseID;

        //            if (model.WarehouseID == -1)
        //            {
        //                if (string.IsNullOrEmpty(Request.QueryString["whID"].ToString()))
        //                {
        //                    ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
        //                }
        //                else
        //                {
        //                    long whID = Convert.ToInt64(Request.QueryString["whID"].ToString());
        //                    if (whID != 0)
        //                    {
        //                        ViewBag.PossibleWarehouses = repo.WarehouseRepository.GetListByID(whID);
        //                    }
        //                    else
        //                    {
        //                        ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();
        //                    }

        //                }

        //            }



        //            List<AssignBox> boxListNew = repo.AssignBoxRepository.GetNotAssignedBoxList(Convert.ToInt32(trID));

        //            return View(boxListNew);
        //        }



        //}

        #endregion


        public ActionResult AddMoreBox(int trID, long wID)
        {
            bool flag = false;
            TransmittalIN transIN = repo.TransmittalINRepository.GetTrIN(trID);

            List<AssignBox> boxList = repo.AssignBoxRepository.GetByTrInId(trID);

            if (boxList != null)
            {
                foreach (var item in boxList)
                {
                    if (item.TransmittalINStatusId == 1)
                    {
                        flag = true;
                    }

                }
            }


            if (flag == true)
            {

                ViewBag.TransmittalNo = transIN.TransmittalNo;
                ViewBag.TransmittalRefNo = transIN.ClientRequestreference;
                ViewBag.TransmittalType = "Transmittal IN";
                ViewBag.TransmittalId = transIN.TransmittalINId;
                ViewBag.TransmittalDate = transIN.TransmittalDate.ToShortDateString();
                ViewBag.AllItems = transIN.Items;
                ViewBag.PossibleWarehouses = repo.WarehouseRepository.GetListByID(wID);
                List<AssignBox> boxListNew = repo.AssignBoxRepository.GetNotAssignedBoxList(trID);

                //return View("AssignBoxes", boxListNew);
                return RedirectToAction("AssignBoxes", new { trID = trID, flag = 1, whID = wID });

            }

            else
            {
                ViewBag.TransmittalNo = transIN.TransmittalNo;
                ViewBag.TransmittalRefNo = transIN.ClientRequestreference;
                ViewBag.TransmittalType = "Transmittal IN";
                ViewBag.TransmittalId = transIN.TransmittalINId;
                ViewBag.TransmittalDate = transIN.TransmittalDate.ToShortDateString();
                ViewBag.AllItems = transIN.Items;
                ViewBag.PossibleWarehouses = repo.WarehouseRepository.GetListByID(wID);
                ViewBag.ChkBx = 1;
                List<AssignBox> boxListNew = repo.AssignBoxRepository.GetByTrIDForChallan(trID);

                //return View("AssignBoxes", boxListNew);
                return View("Challan", boxListNew);
            }
        }



        public ActionResult DeleteBox(long id)
        {
            AssignBox item = repo.AssignBoxRepository.Find(id);

            item.TransmittalINStatusId = 1;
            repo.AssignBoxRepository.InsertOrUpdate(item);
            repo.AssignBoxRepository.Save();

            List<AssignBox> AssignedBoxList = repo.AssignBoxRepository.GetByTrIDForChallan(item.TransmittalINId);
            TransmittalIN transIN = repo.TransmittalINRepository.GetTrIN(item.TransmittalINId);

            ViewBag.TransmittalNo = transIN.TransmittalNo;
            ViewBag.TransmittalRefNo = transIN.ClientRequestreference;
            ViewBag.TransmittalType = "Transmittal IN";
            ViewBag.TransmittalId = transIN.TransmittalINId;
            ViewBag.TransmittalDate = transIN.TransmittalDate.ToShortDateString();
            ViewBag.AllItems = transIN.Items;
            ViewBag.WID = item.WarehouseID;
            ViewBag.PossibleWarehouses = repo.WarehouseRepository.AllIncluding();

            return View("Challan", AssignedBoxList);


        }


        public ActionResult DoneTransmittal(int trId)
        {
            TransmittalIN trIN = repo.TransmittalINRepository.Find(trId);
            trIN.TransmittalINStatusId = 2;
            repo.TransmittalINRepository.InsertOrUpdate(trIN);
            repo.TransmittalINRepository.Save();

            #region Mail Method

            string MailSubject = "Approval for Your Box Assignment Request!";
            string MailBody = "Dear User,</br>";
            MailBody += "Your request to assign new boxes has been approved. </br>";
            MailBody += "Transmittal No: " + trIN.TransmittalNo.ToString() + " </br>";
            MailBody += "</br> Thank you.</br>" + System.Environment.NewLine;

            HandOverBy handOverBy = trIN.HandOverBy.FirstOrDefault();
            User user = repo.UserRepository.GetUserByUserName(handOverBy.Name);
            string userEmail = user == null ? "" : user.Email;

            WareHouseMVC.HelperClasses.MailHelper mailHelper = new HelperClasses.MailHelper();
            mailHelper.SendMail(userEmail, MailSubject, MailBody);

            #endregion

            return RedirectToAction("AssignBoxes", new { trID = trId, flag = 1 });
        }


        [HttpPost]
        public ActionResult AssignBoxEdit(AssignBox model)
        {
            return View();
        }


        #region BoxNo & BoxName Edit Option

        [HttpGet]
        public ActionResult BoxSearch(BoxSearchViewModel model)
        {

            int pageIndex = model.Page ?? 1;
            int pageSize = 10;
            int pageNumber = pageIndex;
            //}

            long _clientID = model.ClientID;
            long _deptID;
            if (model.DepartmentID.HasValue && model.DepartmentID.Value != -1)
            {
                _deptID = model.DepartmentID.Value;
            }
            else
            {
                _deptID = 0;
            }
            DateTime _startDate = DateTime.Now;
            DateTime _endDate = DateTime.Now;

            if (model.StartDate != DateTime.MinValue)
            {
                _startDate = model.StartDate;
            }
            if (model.EndDate != DateTime.MinValue)
            {
                _endDate = model.EndDate;
            }


            string _boxName = model.BoxName;
            string _boxNo = model.BoxNo;
            if (_boxName == "/")
            {
                _boxName = string.Empty;
            }
            if (_boxNo == "/")
            {
                _boxNo = string.Empty;
            }


            List<AssignBox> finalList = new List<AssignBox>();
            IPagedList<AssignBox> listofAllBoxes = listofAllBoxes = repo.AssignBoxRepository.GetByClientIdAndDepartmentIdPList(_clientID, _deptID, model.StartDate, model.EndDate, _boxName, _boxNo, pageNumber, pageSize);




            foreach (AssignBox item in listofAllBoxes)
            {
                if (_startDate.ToShortDateString() != DateTime.Now.ToShortDateString() && _endDate.ToShortDateString() != DateTime.Now.ToShortDateString())
                {

                    if (item.AssignDate.Date >= _startDate.Date && item.AssignDate.Date <= _endDate.Date)
                    {

                        List<BoxLocation> _boxLocationList = repo.BoxLocationRepository.GetListByAssignBoxId(item.AssignBoxId);
                        item.BoxLocation = _boxLocationList;


                    }


                }

                else
                {
                    List<BoxLocation> _boxLocationList = repo.BoxLocationRepository.GetListByAssignBoxId(item.AssignBoxId);
                    item.BoxLocation = _boxLocationList;


                }


            }



            ViewBag.ClientList = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(model.ClientID).OrderBy(o => o.DepartmentName);
            ViewBag.flag = 1;
            model.SearchResults = listofAllBoxes;

            ViewBag.BoxNo = model.BoxNo;
            ViewBag.BoxName = model.BoxName;



            ViewBag.ModelClientID = model.ClientID;
            ViewBag.ModelDepartmentID = model.DepartmentID;
            ViewBag.ModelStartDate = model.StartDate;
            ViewBag.ModelEndDate = model.EndDate;
            ViewBag.ModelBoxName = _boxName;
            ViewBag.ModelBoxNo = _boxNo;


            return View(model);


        }


        public ActionResult UpdateBoxInfo(long ItemId)
        {
            Item item = new Item();
            item = repo.ItemRepository.Find(ItemId);

            ViewBag.Client = item.Client.ClientName;
            ViewBag.Dept = item.Department.DepartmentName;
            ViewBag.Project = "N/A";
            ViewBag.ItemId = item.ItemId;

            if (item.ProjectId != null || item.ProjectId > -1)
            {
                ViewBag.Project = repo.ProjectRepository.Find(item.ProjectId.Value);
            }




            return View(item);
        }


        [HttpPost]
        public ActionResult UpdateBoxInfo(Item model)
        {


            Item _item = new Item();

            _item = repo.ItemRepository.Find(model.ItemId);

            Item _itemTemp = new Item();
            _itemTemp = repo.ItemRepository.GetByClientIdDeptIdBoxNoBoxNameYear(_item.ClientID, _item.DepartmentID, model.BoxNo, model.ItemName, model.Year);


            if (_itemTemp != null)
            {
                //throw error msg

                ViewBag.ErrorMsg = false;
            }
            else
            {
                #region Audit Trail
                AuditTrailCP auditCp = new AuditTrailCP();
                auditCp.ChangingDate = DateTime.Now;
                auditCp.NewBoxName = model.ItemName;
                auditCp.NewBoxNumber = model.BoxNo;
                auditCp.NewBoxYear = model.Year;
                auditCp.NewComments = model.Comments;
                auditCp.PreBoxYear = _item.Year;
                auditCp.PreBoxNumber = _item.BoxNo;
                auditCp.PreBoxName = _item.ItemName;
                auditCp.PreComments = _item.Comments;

                auditCp.UserName = User.Identity.Name;
                repo.AuditTrailCPRepository.InsertOrUpdate(auditCp);
                repo.AuditTrailCPRepository.Save();
                #endregion



                #region Item Table Update
                _item.Year = model.Year;
                _item.ItemName = model.ItemName;
                _item.BoxNo = model.BoxNo;

                repo.ItemRepository.InsertOrUpdate(_item);
                repo.ItemRepository.Save();
                #endregion




                #region AssignBox Table Update



                List<AssignBox> assignBoxList = new List<AssignBox>();
                assignBoxList = repo.AssignBoxRepository.GetByItemId(_item.ItemId);

                if (assignBoxList.Count > 0)
                {
                    foreach (AssignBox _assignBox in assignBoxList)
                    {
                        AssignBox tempAssignBox = new AssignBox();

                        tempAssignBox = repo.AssignBoxRepository.Find(_assignBox.AssignBoxId);

                        tempAssignBox.BoxName = model.ItemName;
                        tempAssignBox.BoxNo = model.BoxNo;
                        tempAssignBox.Year = model.Year;

                        repo.AssignBoxRepository.InsertOrUpdate(tempAssignBox);
                        repo.AssignBoxRepository.Save();

                    }
                }

                #endregion

                #region AssignBoxTrOUT Table Update


                List<AssignBoxTrOUT> assignBoxTrOutList = new List<AssignBoxTrOUT>();

                assignBoxTrOutList = repo.AssignBoxTrOUTRepository.GetListByItemId(_item.ItemId);

                if (assignBoxTrOutList.Count > 0)
                {
                    foreach (AssignBoxTrOUT _assignBoxTrOut in assignBoxTrOutList)
                    {
                        AssignBoxTrOUT _tempAssignBoxTrOut = new AssignBoxTrOUT();

                        _tempAssignBoxTrOut = repo.AssignBoxTrOUTRepository.Find(_assignBoxTrOut.AssignBoxTrOUTId);

                        _tempAssignBoxTrOut.BoxNo = model.BoxNo;
                        repo.AssignBoxTrOUTRepository.InsertOrUpdate(_tempAssignBoxTrOut);
                        repo.AssignBoxTrOUTRepository.Save();


                        #region DelPnding Table Update

                        DelPendingBoxModel delPending = new DelPendingBoxModel();
                        delPending = repo.DelPendingBoxModelRepository.GetByTrOutNo(_tempAssignBoxTrOut.TransmittalOUT.TransmittalNo);

                        if (delPending != null)
                        {
                            delPending.BoxName = model.ItemName;
                            delPending.BoxNo = model.BoxNo;
                            delPending.Year = model.Year;
                            delPending.ClientID = model.ClientID;
                            delPending.DepatID = model.DepartmentID;

                            repo.DelPendingBoxModelRepository.InsertOrUpdate(delPending);
                            repo.DelPendingBoxModelRepository.Save();
                        }
                        #endregion




                    }
                }

                #endregion




                ViewBag.ErrorMsg = true;
            }

            ViewBag.Client = _item.Client.ClientName;
            ViewBag.Dept = _item.Department.DepartmentName;
            ViewBag.Project = "N/A";
            ViewBag.ItemId = _item.ItemId;
            return View(_item);
        }



        #endregion

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }


    }
}