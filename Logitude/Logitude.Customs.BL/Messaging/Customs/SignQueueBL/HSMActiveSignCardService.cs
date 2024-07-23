using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using Newtonsoft.Json;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.Customs.SignQueueBL
{
    public class HSMActiveSignCardService
    {
        HSMActiveSignCardResponse GetActiveCertificates_NOCache(
            string signHSMGetActiveCertificates_Url, 
            string azureServiceToken /*xfunctionskey*/, 
            HSMActiveSignCardParams hSMActiveSignCardParams)
        {
            string responseString = "";
            if (string.IsNullOrWhiteSpace(signHSMGetActiveCertificates_Url))
            {
                throw new ArgumentNullException("signHSMGetActiveCertificates_Url");
            }
            if (string.IsNullOrWhiteSpace(azureServiceToken))
            {
                throw new ArgumentNullException("azureServiceToken");
            }
            var stopwatch = Stopwatch.StartNew();
            try
            {
                int timeOutInSec = 10;
                using (var client = new HttpClient(
                    //new WebRequestHandler { ReadWriteTimeout = timeOutInSec * 1000 }
                    ))
                {
                    
                    client.DefaultRequestHeaders.Add(
                        "x-functions-key",
                        azureServiceToken //"9edYig7zg_b2mBV-72DaOKVMlqtJp-xovFY0k5uBNSRtAzFuY2xcGA=="
                        );

                    var serializedhSMActiveSignCardParams = JsonConvert.SerializeObject(hSMActiveSignCardParams);
                    var formContent = new MultipartFormDataContent
{
    //{new StringContent("ParamValue1"),"ParamName1"},
    {new StringContent(serializedhSMActiveSignCardParams),"request"},
    
};
                    LogMessagingUtil.Instance.AppendLine(signHSMGetActiveCertificates_Url);
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    var result = client.PostAsync(signHSMGetActiveCertificates_Url, formContent);// "application/json");
                    result.Wait();

                    responseString = result.Result.Content.ReadAsStringAsync().Result;
                    
                    LogMessagingUtil.Instance.AppendLine($"Took:{stopwatch.Elapsed}");
                    LogMessagingUtil.Instance.AppendLine(result?.Result?.StatusCode.ToString());
                    //LogMessagingUtil.Instance.AppendLine(responseString);
                   NetCommonHelper.Logger.DevLog.Instance.WriteDebug(responseString);
                    switch (result.Result.StatusCode)
                    {
                      
                        case System.Net.HttpStatusCode.OK:
                            HSMActiveSignCardResponse hSMActiveSignCardResponse= JsonConvert.DeserializeObject<HSMActiveSignCardResponse>(responseString);
                            return hSMActiveSignCardResponse;
                            break;
                        case System.Net.HttpStatusCode.Unauthorized:
                            throw new Exception("Unauthorized,Fix AzureServiceToken," + responseString);
                            break;    
                        default:
                            throw new Exception($"{result.Result.StatusCode},response-{responseString}");
                            break;
                    }
                    
                        
                    //queueservice.CompleteAsFailed();
                    
                }
            }
            catch (Exception ex)
            {
                
                ExceptionHandler.HandleException(ex, DateTime.Now, 1, null, "HSMActiveSignCardService GetActiveCertificates Failed", null, null);
                throw;

            }

        }
        const int CACHE_TIMEOUT = 5;
        
       

        public List<Cert> GetActiveCertificates(
            int tenant,
            string signHSMGetActiveCertificates_Url,
            string azureServiceToken /*xfunctionskey*/,
            HSMActiveSignCardParams hSMActiveSignCardParams,
            bool fromCache= true
            )
        {

            string key = $"HSMGetActiveCertificates({tenant})";
            if (!fromCache)
            {
                CacheManager.CacheWrapper.Remove(key);
            }
            
            var res=CacheManager.GetOrInsertNewObject<HSMActiveSignCardResponse>(key,
                () =>
                {

                    var my=this.GetActiveCertificates_NOCache(
                        signHSMGetActiveCertificates_Url,
                        azureServiceToken,
                        hSMActiveSignCardParams

                        );
                    return my;

                },
                fromCache: true, donotCacheNull: true, 
                supressForceInsert: true, 
                absoluteExpiration : CACHE_TIMEOUT /*5  min*/
            );
            if (!res.success)
            {
                CacheManager.CacheWrapper.Remove(key);
                
                throw new Exception(res.errorMessage);    
            }
            return res?.certs ?? new List<Cert>();
            

           

        }



    }
    
    public enum CompanyPersonalEnum
    {
        NOOOONE,
        C, //Company
        P, //Personal
        PC, //Personal or Company
        PD, //Personal Default
        PCD //Personal Company Default
    }
    public class HSMActiveSignCardParams

    {
        public string companyid { get; set; }
        public string token { get; set; }
        public string signprocess { get; set; }
        public string companyBN { get; set; }    
        


    }
    
    public class Cert
    {
        public string companyID { get; set; }
        public string id { get; set; }
        public string userName { get; set; }
        public string companyBN { get; set; }
        public string companyPersonal { get; set; }
    }

    public class HSMActiveSignCardResponse
    {
        public bool success { get; set; }
        public string errorMessage { get; set; }
        public List<Cert> certs { get; set; }
    }



}

/*            
          var client = new RestClient(https://customs.amital.co.il/api/SignHSMGetActiveCertificates);
          client.Timeout = -1;
                      var request = new RestRequest(Method.POST);
                      request.AddHeader("x-functions-key", "9edYig7zg_b2mBV-72DaOKVMlqtJp-xovFY0k5uBNSRtAzFuY2xcGA==");
                      request.AlwaysMultipartFormData = true;
                      request.AddParameter("request", "{
              \"companyid\": 101,
              \"token\": \"c6f85591-6e4e-4203-95ef-628b826577b8\",
              \"signprocess\": \"MehesExport\"
          }");
          IRestResponse response = client.Execute(request);
                  Console.WriteLine(response.Content);


          {
          "companyid": 101,
          "token": "c6f85591-6e4e-4203-95ef-628b826577b8",
          "signprocess": "MehesExport"
          }
          */
