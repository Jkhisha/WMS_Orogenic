using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using WareHouseMVC.Models;
using WareHouseMVC.Models.API;
using System.Web.Security;

namespace WareHouseMVC.Controllers
{
    public class WMSController : ApiController
    {
        // GET api/<controller>
        UnitOfWork repo = new UnitOfWork();
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }


        #region BarCodes

        [System.Web.Http.HttpPost]

        public HttpResponseMessage GetBarCodeList(HttpRequestMessage request)
        {


            TrBarCodeMobileGet getModel = new TrBarCodeMobileGet();
            getModel = JsonConvert.DeserializeObject<TrBarCodeMobileGet>(request.Content.ReadAsStringAsync().Result);
            TrBarCodeMobileResponse respModel = new TrBarCodeMobileResponse();
            GetBarcodeVM vm = new GetBarcodeVM();
            List<TrBarCodeModel> barcodes = new List<TrBarCodeModel>();
            if (getModel.TrId != null && getModel.TrType != null)
            {

                if (getModel.TrType == 1)
                {
                    TransmittalIN trIn = new TransmittalIN();
                    trIn = repo.TransmittalINRepository.GetByTrNo(getModel.TrId);

                    if (trIn != null)
                    {
                        List<AssignBox> assignBoxes = new List<AssignBox>();
                        assignBoxes = repo.AssignBoxRepository.GetListByTrId(trIn.TransmittalINId);
                        respModel.Client = repo.ClientRepository.Find(trIn.ClientID).ClientName;
                        respModel.Department = repo.DepartmentRepository.Find(trIn.DepartmentID).DepartmentName;
                        respModel.TrDate = trIn.TransmittalDate.ToShortDateString();
                        respModel.TrId = trIn.TransmittalNo;
                        int count = 0;
                        foreach (AssignBox item in assignBoxes)
                        {
                            count++;
                            TrBarCodeModel model = new TrBarCodeModel();
                            model.BarCode = (item.ItemId * 5000).ToString();
                            model.IsDetected = false;
                            barcodes.Add(model);

                        }

                        respModel.BarCodes = barcodes;
                        respModel.Message = "Total : " + count.ToString() + " Barcodes are found for this Transmittal IN";
                        vm.Model = respModel;
                        vm.IsValid = true;
                        vm.Message = "Transmittal Found.";

                    }
                    else
                    {
                        vm.IsValid = false;
                        vm.Message = "No Transmittal IN found for thid ID.";
                        vm.Model = new TrBarCodeMobileResponse();
                    }
                }
                else
                {
                    TransmittalOUT trOut = new TransmittalOUT();
                    trOut = repo.TransmittalOUTRepository.GetByTrNo(getModel.TrId);

                    if (trOut != null)
                    {

                        respModel.Client = repo.ClientRepository.Find(trOut.ClientID).ClientName;
                        respModel.Department = repo.DepartmentRepository.Find(trOut.DepartmentID).DepartmentName;
                        respModel.TrDate = trOut.TransmittalDate.ToShortDateString();
                        respModel.TrId = trOut.TransmittalNo;
                        int count = 0;
                        foreach (Item item in trOut.Items)
                        {
                            count++;
                            TrBarCodeModel model = new TrBarCodeModel();
                            model.BarCode = (item.ItemId * 5000).ToString();
                            model.IsDetected = false;
                            barcodes.Add(model);

                        }
                        respModel.Message = "Total : " + count.ToString() + " Barcodes are found for this Transmittal OUT";
                        respModel.BarCodes = barcodes;
                        vm.Model = respModel;
                        vm.IsValid = true;
                        vm.Message = "Transmittal Found.";
                    }
                    else
                    {
                        vm.IsValid = false;
                        vm.Message = "No Transmittal OUT found for thid ID.";
                        vm.Model = new TrBarCodeMobileResponse();
                    }
                }


            }
            else
            {
                vm.IsValid = false;
                vm.Message = "Transmittal ID can not be empty";
                vm.Model = new TrBarCodeMobileResponse();
            }


            #region Return Response

            string json = JsonConvert.SerializeObject(vm);
            var resp = new HttpResponseMessage()
            {
                Content = new StringContent(json)
            };
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;

            #endregion

        }


        [System.Web.Http.HttpPost]
        public HttpResponseMessage SendBarCodeList(HttpRequestMessage request)
        {
            TrBarCodeMobileResponse getModel = new TrBarCodeMobileResponse();
            getModel = JsonConvert.DeserializeObject<TrBarCodeMobileResponse>(request.Content.ReadAsStringAsync().Result);
            TrBarCodeMobileSuccess respModel = new TrBarCodeMobileSuccess();

            bool flag = true;
            int IsDetectedCount = 0;
            int notDetectedCount = 0;

            foreach (TrBarCodeModel item in getModel.BarCodes)
            {
                if (item.IsDetected == false)
                {
                    flag = false;
                    notDetectedCount++;
                }
                else
                {
                    IsDetectedCount++;
                }

            }


            if (flag == true)
            {
                respModel.Message = "Verification SuccessFull";
                respModel.TrId = getModel.TrId;
                respModel.IsValid = true;

            }
            else
            {
                
            respModel.Message = "Verification Unsuccessfull.Matched Barcode : "+IsDetectedCount.ToString()+ "  ,Not matched Barcode : " +notDetectedCount.ToString();
            respModel.TrId = getModel.TrId;
            respModel.IsValid = false;
            }



            #region Return Response

            string json = JsonConvert.SerializeObject(respModel);
            var resp = new HttpResponseMessage()
            {
                Content = new StringContent(json)
            };
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;

            #endregion


        }


        [System.Web.Http.HttpPost]
        public HttpResponseMessage AppLogin(HttpRequestMessage request)
        {
            LoginVM login = new LoginVM();
            login = JsonConvert.DeserializeObject<LoginVM>(request.Content.ReadAsStringAsync().Result);
            LoginResponse respModel = new LoginResponse();

            if (Membership.ValidateUser(login.user, login.password))
            {
                respModel.IsValid = true;
                MembershipUser currentUser = Membership.GetUser(login.user);
                respModel.UserId = currentUser.ProviderUserKey.ToString();
                respModel.UserName = currentUser.UserName;
                respModel.Message = "User Successfully Validated.";
            }
            else
            {
                respModel.IsValid = false;

                respModel.UserId = string.Empty;
                respModel.UserName = string.Empty;
                respModel.Message = "Username or Password Incorrect.";
            }


            #region Return Response

            string json = JsonConvert.SerializeObject(respModel);
            var resp = new HttpResponseMessage()
            {
                Content = new StringContent(json)
            };
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;

            #endregion


        }



        #endregion


    }
}