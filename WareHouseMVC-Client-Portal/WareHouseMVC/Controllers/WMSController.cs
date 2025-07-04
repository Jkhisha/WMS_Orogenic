using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using WareHouseMVC.Models;

namespace WareHouseMVC.Controllers
{
    public class WMSController : ApiController
    {
        // GET api/<controller>
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


            if(getModel.TrId!=null && getModel.TrType!=null)
            {
                List<TrBarCodeModel> barcodes = new List<TrBarCodeModel>();
                TrBarCodeModel model = new TrBarCodeModel();
                model.BarCode = "123456";
                model.IsDetected = false;
                barcodes.Add(model);

                respModel.BarCodes =  barcodes;
                respModel.Client = "HSBC";
                respModel.Department = "Account";
                respModel.TrDate = DateTime.Now.ToShortDateString();
                respModel.TrId = getModel.TrId;
                respModel.TrIssuedBy = "Badhon";
                respModel.WareHouse = "S R Tower";
                respModel.Message = "No Barcodes For You";

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
        public HttpResponseMessage SendBarCodeList(HttpRequestMessage request)
        {
            TrBarCodeMobileResponse getModel = new TrBarCodeMobileResponse();
            getModel = JsonConvert.DeserializeObject<TrBarCodeMobileResponse>(request.Content.ReadAsStringAsync().Result);
            TrBarCodeMobileSuccess respModel = new TrBarCodeMobileSuccess();

            respModel.Message = "Verification SuccessFull";
            respModel.TrId = getModel.TrId;

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