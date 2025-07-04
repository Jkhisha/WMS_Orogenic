using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouseMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Configuration;
using WareHouseMVC.HelperClasses;
using System.Drawing;
using System.IO;
using PagedList;
using WareHouseMVC.Models.API;
using Newtonsoft.Json.Linq;

namespace WareHouseMVC.Controllers
{
    public class EmptyBoxesController : Controller
    {
        //private WareHouseMVCContext context = new WareHouseMVCContext();
        UnitOfWork repo = new UnitOfWork();

        //
        // GET: /EmptyBoxes/

        public ViewResult Index()
        {
            var emptyBoxes = repo.EmptyBoxRepository.AllIncluding(emptybox => emptybox.Client).OrderByDescending(emptybox => emptybox.RecuisitionDate).ToList();
            return View(emptyBoxes);
        }

        //
        // GET: /EmptyBoxes/Details/5

        public ViewResult Details(long id)
        {
            EmptyBox emptybox = repo.EmptyBoxRepository.Find(id);
            return View(emptybox);
        }

        //
        // GET: /EmptyBoxes/Create

        public ActionResult Create()
        {
            EmptyBox emptyBox = new EmptyBox();
            emptyBox.EmptyBoxNo = "This No will be generated";
            emptyBox.RecuisitionDate = DateTime.Now;
            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.AllIncluding();
            ViewBag.PossibleOperator = repo.ORBLOperatorRepository.AllIncluding();
            ViewBag.EmptyBoxNo = "This No will be generated";
            return View(emptyBox);
        }




        //
        // POST: /EmptyBoxes/Create

        [HttpPost]
        public ActionResult Create(EmptyBox emptybox)
        {
            emptybox.EmptyBoxNo = "demo";


            if (ModelState.IsValid)
            {
                repo.EmptyBoxRepository.InsertOrUpdate(emptybox);
                repo.EmptyBoxRepository.Save();
                long id = emptybox.EmptyBoxId;

                emptybox.EmptyBoxNo = "ORBL-EMPBX-" + DateTime.Now.Year.ToString() + "-" + id.ToString();
                repo.EmptyBoxRepository.InsertOrUpdate(emptybox);
                repo.EmptyBoxRepository.Save();

                if (emptybox.NoofBoxes > 0)
                {
                    BarCodeRepository barcodeRef = new BarCodeRepository();
                    for (int i = 0; i < emptybox.NoofBoxes; i++)
                    {


                        BarCode BarCodeObj = barcodeRef.GetBarcodeTxtEmptyBoxes(emptybox.EmptyBoxId);

                        EmptyBoxBarcode BarcodeBox = new EmptyBoxBarcode()
                        {
                            BarCodeText = BarCodeObj.BoxName,
                            BarCodeId = BarCodeObj.BarCodeId,
                            EmptyBoxId = emptybox.EmptyBoxId
                        };
                        repo.EmptyBoxRepository.InsertOrUpdateEmptyBoxBarcode(BarcodeBox);
                        repo.EmptyBoxRepository.Save();

                    }

                }

                ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
                ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(emptybox.ClientID);
                ViewBag.PossibleOperator = repo.ORBLOperatorRepository.AllIncluding();
                ViewBag.Flag = 1;
                return View(emptybox);
            }

            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.AllIncluding();
            ViewBag.PossibleOperator = repo.ORBLOperatorRepository.AllIncluding();
            ViewBag.Flag = 0;
            return View(emptybox);
        }

        //
        // GET: /EmptyBoxes/Edit/5

        public ActionResult Edit(long id)
        {
            EmptyBox emptybox = repo.EmptyBoxRepository.Find(id);
            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.AllIncluding();
            ViewBag.PossibleOperator = repo.ORBLOperatorRepository.AllIncluding();
            return View(emptybox);
        }

        //
        // POST: /EmptyBoxes/Edit/5

        [HttpPost]
        public ActionResult Edit(EmptyBox emptybox)
        {
            if (ModelState.IsValid)
            {
                repo.EmptyBoxRepository.InsertOrUpdate(emptybox);
                repo.EmptyBoxRepository.Save();
                return RedirectToAction("Index");
            }
            ViewBag.PossibleClients = repo.ClientRepository.AllIncluding();
            ViewBag.PossibleDepartments = repo.DepartmentRepository.FindByClientID(emptybox.ClientID);
            ViewBag.PossibleOperator = repo.ORBLOperatorRepository.AllIncluding();
            return View(emptybox);
        }

        //
        // GET: /EmptyBoxes/Delete/5

        public ActionResult Delete(long id)
        {
            EmptyBox emptybox = repo.EmptyBoxRepository.Find(id);
            return View(emptybox);
        }

        //
        // POST: /EmptyBoxes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            EmptyBox emptybox = repo.EmptyBoxRepository.Find(id);
            repo.EmptyBoxRepository.DeleteEmptyBoxBarcode(id);
            repo.EmptyBoxRepository.Save();
            repo.EmptyBoxRepository.Delete(id);
            repo.EmptyBoxRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            //if (disposing) {
            //    context.Dispose();
            //}
            base.Dispose(disposing);
        }

        public ActionResult ClientBarcode(string barcodes)
        {
            ViewBag.Barcodes = barcodes?.Split(',').ToList();
            return View();
        }
        private WareHouseMVCContext db = new WareHouseMVCContext();

        [HttpPost]
        public JsonResult GenerateClientBarcode(string originalBarcode)
        {
            try
            {
                string jsonPath = Server.MapPath("~/clientbarcode.json");
                string jsonContent = System.IO.File.ReadAllText(jsonPath);
                JObject jsonObject = JObject.Parse(jsonContent);
                string code = "";
                var hsbc1Entry = jsonObject["clientBarCode"]
                    .Children()
                    .FirstOrDefault(x => x["encoding"].ToString() == "HSBC1");
                if (hsbc1Entry != null)
                {
                    string lastBarcode = hsbc1Entry["lastbarcode"]?.ToString();
                    if (!string.IsNullOrEmpty(lastBarcode))
                    {
                        string prefix = lastBarcode.Substring(0, 3); 
                        int number = int.Parse(lastBarcode.Substring(3)); 
                        number++; // Increment number
                        string newBarcode = $"{prefix}{number:D4}";
                        System.Diagnostics.Debug.WriteLine($"Last barcode for HSBC1: {newBarcode}");
                        var clientBarcodeMap = db.ClientBarCodeMaps.FirstOrDefault(b => b.OrogenicBarCodeText == originalBarcode);
                        if (clientBarcodeMap != null)
                        {
                            if (string.IsNullOrEmpty(clientBarcodeMap.ClientBarCodeText))
                            {
                                clientBarcodeMap.ClientBarCodeText = newBarcode;
                            }
                            else
                            {
                                newBarcode = clientBarcodeMap.ClientBarCodeText; 
                            }
                        }
                        else
                        {
                            clientBarcodeMap = new ClientBarCodeMap
                            {
                                OrogenicBarCodeText = originalBarcode,
                                ClientBarCodeText = newBarcode,
                                Encoding = "HSBC1" 
                            };
                            db.ClientBarCodeMaps.Add(clientBarcodeMap);
                        }
                        db.SaveChanges();
                        hsbc1Entry["lastbarcode"] = newBarcode;
                        System.IO.File.WriteAllText(jsonPath, jsonObject.ToString());
                        code = newBarcode;
                    }
                    System.Diagnostics.Debug.WriteLine($"Last barcode for HSBC1: {code}");
                }
                return Json(new { success = true, barcode = code}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}