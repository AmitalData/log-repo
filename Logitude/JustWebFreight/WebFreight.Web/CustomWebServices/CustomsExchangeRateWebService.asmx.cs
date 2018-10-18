using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;
using UnifreightIIG.Common.CurrencyRateServiceReference;

namespace WebFreight.Web.CustomWebServices
{
    /// <summary>
    /// Summary description for CustomsExchangeRateWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CustomsExchangeRateWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public byte[] SearchCurrencyRateRequest(byte[] searchCurrencyRateParams)
        {
            MemoryStream memorystream = new MemoryStream(searchCurrencyRateParams);
            XmlSerializer serializer = new XmlSerializer(typeof(CD_NG_8347_Web01_CurrencyRateSearchRequestParams));
            CD_NG_8347_Web01_CurrencyRateSearchRequestParams searchRateParams = (CD_NG_8347_Web01_CurrencyRateSearchRequestParams)serializer.Deserialize(memorystream);

            CD_NG_8348_Web02_CurrencyRateDetailResponseData responseData;
            if (searchRateParams.TestCase != null && searchRateParams.TestCase.Type == "webservice" && searchRateParams.TestCase.Code != "Real Logic")
            {
                responseData = new CD_NG_8348_Web02_CurrencyRateDetailResponseData();
                switch (searchRateParams.TestCase.Code)
                {

                    case "Send Succeeded":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;

                            responseData.CurrencyRateList = GetTestCurrencyRateResultList(searchRateParams);


                            break;
                        }
                    case "Send Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for search custom currency rate message!";
                            break;
                        }

                }
            }
            else
            {
                var messageService = new CD_NG_8347_Web01_CurrencyRateSearchParamMessagingService();
                responseData = messageService.Send(searchRateParams);                
            }

            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(CD_NG_8348_Web02_CurrencyRateDetailResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }

        public List<CurrencyRateResult> GetTestCurrencyRateResultList(CD_NG_8347_Web01_CurrencyRateSearchRequestParams newSearchCurrencyRateParams)
        {

            List<CurrencyRateResult> rateResults = new List<CurrencyRateResult>() ;
            #region list
            DateTime? rateDate = newSearchCurrencyRateParams.FromDate;

            while (newSearchCurrencyRateParams.ToDate >= rateDate)
            {
              

                 CurrencyRateResult rate =   new CurrencyRateResult()
                                {
                                   
                                    Tenant=newSearchCurrencyRateParams.Tenant, 
                                   CurrencyTypeId = newSearchCurrencyRateParams.CurrencyTypeId,
                                   CustomsCurrencyRate = (decimal)0.25,
                                   StartDate = rateDate,
                                   EndDate = newSearchCurrencyRateParams.ToDate,

                                   
                                    
                                };
                rateResults.Add(rate);
                rateDate = rateDate.Value.AddDays(1);
            }

             ICustomContext customContext = CustomContext.GetContext(newSearchCurrencyRateParams.Tenant);

            CustomsExchangeRateUpdateService rateUpdate = new CustomsExchangeRateUpdateService(customContext, new Dictionary<string, IContext>(), newSearchCurrencyRateParams.Tenant);
            foreach(CurrencyRateResult item in rateResults)
            {
                CustomsExchangeRatePM rate = new CustomsExchangeRatePM()
                {
                    CurrencyTypeCode = item.CurrencyTypeId,
                    RateDate = item.StartDate,
                    Tenant = item.Tenant,
                    ExchangeRate=item.CustomsCurrencyRate,
                };

                rate.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                rateUpdate.Update(rate,true);

            }
          

           #endregion
            //CustomsVendorQueryService vendorQueryService = new CustomsVendorQueryService(newSearchVendorParams.Tenant);
            //foreach (VendorResult vendor in vendorResults)
            //{
            //    bool exists = vendorQueryService.DoesVendorExist(vendor.VendorNumber);
            //    vendor.Exists = exists;
            //}

            //List<VendorResult> filteredResult = vendorResults;


            return rateResults;
        }


    }
}
