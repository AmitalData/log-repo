using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.BL.Messaging.Customs.SignQueueBL;
using Logitude.Server.Tools.Helpers;
using Newtonsoft.Json;
using static Logitude.Customs.BL.Messaging.Customs.SignQueueBL.HSMSignFileService;
using System.Diagnostics;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
using Logitude.SystemLogs;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class OcrDocumentQueryService
    {
             
        public OcrDocument GetOcrDocumentByDocumentFilingId(string documentFilingId, int tenant)
        {

            OcrDocumentRepository ocrDocumentRepository = new OcrDocumentRepository(tenant);

            return ocrDocumentRepository.GetSingleByDocId(documentFilingId, tenant);
        }



        public string RemoveFromTypingQueue(
           int tenant,
           string OcrId
           )
        {
            try
            {
                var customsEnvironmentSettingQueryService = new CustomsEnvironmentSettingQueryService(tenant);
                var environmentSettingPM = customsEnvironmentSettingQueryService.GetEnvironmentSettingPM();
                var settingService = new CustomsSettingQueryService(tenant);
                var tenantSetting = settingService.GetSettingByTenantN(tenant);


                var res = this.RemoveFromTyping(
                     environmentSettingPM.UpdateDocOcrServiceUrl,
                     environmentSettingPM.OcrToken,
                     tenantSetting.OcrToken,
                     new RemoveFromTypingOcr
                     {
                        tenant = tenant,
                        UpdateTypingQueue = "UnsetTyping",
                        RequestFileId = OcrId,


                     }
                    );
                return res;
            }
            catch( Exception ex ) 
            {
                return ex.Message.ToString() ?? null;
            }
        
        }


        public string RemoveFromTyping(

          string ocr_Url,
          string azureServiceToken /*xfunctionskey*/,
          string token,
          RemoveFromTypingOcr removeFromTypingOcr

          )
        {

            if (string.IsNullOrWhiteSpace(ocr_Url))
            {
                throw new ArgumentNullException("ocr_Url");
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


                var serializedParams = JsonConvert.SerializeObject(removeFromTypingOcr);
                content.Add(new StringContent(serializedParams), "request");
                int timeoutInSec = 10;
                using (var client = new HttpClient(
                    ))
                {
                    client.DefaultRequestHeaders.Add(
                           "x-functions-key",
                           azureServiceToken 
                           ) ;

                    client.DefaultRequestHeaders.Add(
                           "token",
                           token
                           );
                    string output = "";
                    LogMessagingUtil.Instance.AppendLine(ocr_Url);
                    using (var task = client.PostAsync(ocr_Url, content))
                    {
                        task.Wait();
                        LogMessagingUtil.Instance.AppendLine($"Took:{stopwatch.Elapsed}");
                      
                        responseString = task.Result.Content.ReadAsStringAsync().Result;
                       NetCommonHelper.Logger.DevLog.Instance.WriteDebug(responseString);

                        return responseString;



                    }
                }

            }

            catch (Exception ex)
            {

                return null;

            }
        }



      

    }

    public class RemoveFromTypingOcr
    {
        public int tenant { get; set; }
        public string UpdateTypingQueue { get; set; }
        public string RequestFileId { get; set; }
        
    }
}
