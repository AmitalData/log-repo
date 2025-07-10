using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.Customs.SignQueueBL
{
    public class HSMSignFileService
    {

    
        public byte[] SignFile(

            string signHSM_Url,
            string azureServiceToken /*xfunctionskey*/,
            HSMSignFileParams hSMSignFileParams,
            byte[] BytesToSign

            )
        {

            if (string.IsNullOrWhiteSpace(signHSM_Url))
            {
                throw new ArgumentNullException("signHSM_Url");
            }
            if (string.IsNullOrWhiteSpace(azureServiceToken))
            {
                throw new ArgumentNullException("azureServiceToken");
            }
            var stopwatch = Stopwatch.StartNew();
            string responseString;
            
            try
            {
                var content = new MultipartFormDataContent();

                var dExtension = Path.GetExtension(hSMSignFileParams.filename).ToLower();



                ByteArrayContent bytecontent = new ByteArrayContent(BytesToSign /*data*/);
                bytecontent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                bytecontent.Headers.ContentDisposition.Name = "\"file\"";
                bytecontent.Headers.ContentDisposition.FileName = $"\"{hSMSignFileParams.filename}\"";  ; //fileName;
                bytecontent.Headers.ContentType = 
                    new MediaTypeHeaderValue($"application/{dExtension.Substring(1)}");
                content.Add(bytecontent);

                var serializedhSMSignFileParams = JsonConvert.SerializeObject(hSMSignFileParams);
                content.Add(new StringContent(serializedhSMSignFileParams), "request");
                int timeoutInSec = 10;
                using (var client = new HttpClient(
                    ///new WebRequestHandler { ReadWriteTimeout = timeoutInSec * 1000 }
                    ))
                {
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token_type, access_token);
                    client.DefaultRequestHeaders.Add(
                           "x-functions-key",
                           azureServiceToken //"9edYig7zg_b2mBV-72DaOKVMlqtJp-xovFY0k5uBNSRtAzFuY2xcGA=="
                           );
                    string output = "";
                    LogMessagingUtil.Instance.AppendLine(signHSM_Url);
                    using (var task = client.PostAsync(signHSM_Url, content))
                    {
                        task.Wait();
                        LogMessagingUtil.Instance.AppendLine($"Took:{stopwatch.Elapsed}");
                        //task.Result.EnsureSuccessStatusCode();
                        //if (task.Result.IsSuccessStatusCode)//Result.StatusCode == System.Net.HttpStatusCode.OK)
                        responseString = task.Result.Content.ReadAsStringAsync().Result;
                       NetCommonHelper.Logger.DevLog.Instance.WriteDebug(responseString);
                        switch (task.Result.StatusCode)
                        {

                            case System.Net.HttpStatusCode.OK:
                                {

                                    var byteRes = task.Result.Content.ReadAsByteArrayAsync().Result;
                                    return byteRes;



                                    var listTxt = new string[] { ".xml", ".htm", ".txt" }.ToList();
                                    if (!listTxt.Contains(dExtension))
                                    {
                                         byteRes = task.Result.Content.ReadAsByteArrayAsync().Result;
                                        //var responseString11 = Encoding.UTF8.GetString(byteRes, 0, byteRes.Length);
                                        return byteRes;
                                        
                                        //Stream decompressed = new GZipStream(responded, CompressionMode.Decompress);
                                        //StreamReader objReader = new StreamReader(decompressed, Encoding.UTF8);
                                        //string sLine;
                                        //sLine = objReader.ReadToEnd();


                                    }


                                }
                                
                                break;
                            case System.Net.HttpStatusCode.Unauthorized:
                                throw new Exception("Unauthorized,Fix AzureServiceToken," + responseString);
                                break;
                            default:
                                throw new Exception($"{task.Result.StatusCode},response-{responseString}");
                                break;
                        }




                    }
                }
                
            }

            catch (Exception ex)
            {

                ExceptionHandler.HandleException(ex, DateTime.Now, 1, null, "HSMActiveSignCardService GetActiveCertificates Failed", null, null);
                throw;

            }
        }

        public byte[] SignCustomsRequest(
            int tenant, 
            String customsRequestsSheetId, 
            string signByPersonalId, 
            string companypersonal, // P OR C 
            string customsAgentId,
            byte[] signBytes)
        {
            var customsEnvironmentSettingQueryService = new CustomsEnvironmentSettingQueryService(tenant);
            var environmentSettingPM = customsEnvironmentSettingQueryService.GetEnvironmentSettingPM();
            var settingService = new CustomsSettingQueryService(tenant);
            var tenantSetting = settingService.GetSettingByTenantN(tenant);


            var res = this.SignFile(
                 environmentSettingPM.HSMSignServiceUrl,//  @"https://customs.amital.co.il/api/SignHSM",
                 environmentSettingPM.HSMToken,// @"9edYig7zg_b2mBV-72DaOKVMlqtJp-xovFY0k5uBNSRtAzFuY2xcGA==",
                 new HSMSignFileService.HSMSignFileParams
                 {
                     companyid = tenantSetting.HSMCompanyId,// "101",
                     token = tenantSetting.HSMToken,//  "c6f85591-6e4e-4203-95ef-628b826577b8",
                     signprocess = environmentSettingPM.HSMSignProcess,// "MehesExport",
                     id = signByPersonalId ,//"308623615",
                     companypersonal = companypersonal, // P OR C  
                     filename = $"{customsRequestsSheetId}.xml",
                     reference = customsRequestsSheetId,
                     companyBN = customsAgentId, //"550221105"

                 },
                 signBytes //UTF8Encoding.UTF8.GetBytes(xml)
                );
            return res;
        }

        public class HSMSignFileParams
        {
            public string companyid { get; set; }
            public string token { get; set; }
            public string signprocess { get; set; }
            public string companypersonal { get; set; } ///    "P" or "C"
            public string id { get; set; }

            public string filename { get; set; }

            public string reference { get; set; }
            public string companyBN { get; set; }
            

        }
        
    public class HSMSignFileResponse
        {
            public bool success { get; set; }
            public string errorMessage { get; set; }
            public object signeddata { get; set; }
        }



    }
}
