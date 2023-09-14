using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.BL.Security;
using Logitude.BL.ShipmentsModel.Tools.Validating;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;



namespace Logitude.Accounting.BL.Messaging
{
    public class HSMSignFileService:IHSMSignFileService
    {
        public HSMSignFileService()
        {
          
        }

        private byte[] SignFile(

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
                bytecontent.Headers.ContentDisposition.FileName = $"\"{hSMSignFileParams.filename}\""; ; //fileName;
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
                        Debug.WriteLine(responseString);
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
                                throw new HSMException("Unauthorized,Fix AzureServiceToken," + responseString, "Unauthorized");
                                break;
                            default:
                                throw new HSMException($"{task.Result.StatusCode},response-{responseString}","Fails");
                               
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
           string invocieId,
           byte[] signBytes,
           string fileName,
           string customsAgentId,
           string personalId,
           FullAccountingSettingPM accountingSettings
           )
        {
            IFullAccountingSettingQueryServiceExt query = ContainerAccessor.Container.Resolve(typeof(IFullAccountingSettingQueryServiceExt), "FullAccountingSettingQueryServiceExt", new ParameterOverride("", 1)) as IFullAccountingSettingQueryServiceExt;
           
            FullAccountingSettingPM accountingSettingsTenant0 = query.GetFullAccountingSettingByTenant(0);
            if (string.IsNullOrWhiteSpace(accountingSettings.HSMaddress ))
            {
                throw new ArgumentNullException("HSMaddress");
            }
            if (string.IsNullOrWhiteSpace(accountingSettings.HSMtoken ))
            {
                throw new ArgumentNullException("HSMtoken");
            }


            var res = this.SignFile(
                 accountingSettingsTenant0.HSMaddress,//  @"https://customs.amital.co.il/api/SignHSM",
                 accountingSettingsTenant0.HSMtoken,// @"9edYig7zg_b2mBV-72DaOKVMlqtJp-xovFY0k5uBNSRtAzFuY2xcGA==",
                 new HSMSignFileService.HSMSignFileParams
                 {
                     companyid = accountingSettings.HSM.ToString(),// "101",
                     token = accountingSettings.HSMtoken,//  "c6f85591-6e4e-4203-95ef-628b826577b8",
                     signprocess = "Accounting",// ""Accounting",
                     id = personalId,
                     companypersonal = "C", // P OR C  
                     filename = $"{fileName}.PDF",
                     reference = fileName,
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
