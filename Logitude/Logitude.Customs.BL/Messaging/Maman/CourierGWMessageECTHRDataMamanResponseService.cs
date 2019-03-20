
    //https://maman.wsfreeze.co.il/WebAPIExt/Help/Api/POST-api-baldar-CreateECTHRMessgae
    //https://docs.google.com/document/d/1cjjeORaFsWMS32LhIxmEqNza3q7s7ZAr_PQVQOlwupw/edit#
    


using Logitude.BL.Helpers;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue;
using Logitude.Customs.Data;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.Utils;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.Maman
{
    public class CourierGWMessageECTHRDataMamanResponseService 
        //: IWebAPIMessage2MamanAnalyzer
    //: WebAPIMessage2MamanBase///using  by SendWEBAPIMessage2MamanWRWR
    {

#if false
        private string communicationSubject = "שידור מסר שטר מטען בלדר  לממן";
        public void BuildCommunicationLog(byte[] bytearray, int tenant, string declarationId)///using  by SendWEBAPIMessage2MamanWRWR
        {
            ObjectTableRepository repo = new ObjectTableRepository(tenant);
            var objectTableId = repo.GetObjectTableIdByName("Customs.Declaration"/*"Customs.CourierMaster"*/);

            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
            DocumentRepository documentRepository = new DocumentRepository(commonContext);

            string target = "Maman";
            string xmlSubject = communicationSubject;
            string host = "";
            string folder = "";
            string username = "";
            string password = "";




            var myCustomsPartnerFtpQueryService = new CustomsPartnerFtpQueryService(tenant);
            var pmCustomsPartnerFtp = myCustomsPartnerFtpQueryService.GetBy(tenant, CustomsPartnerFtpDetails.InterfaceName_ECTHR, CustomsPartnerFtpDetails.PartnerCode_Mamam, CustomsPartnerFtpDetails.TypeCode_Out);


            if (string.IsNullOrWhiteSpace(pmCustomsPartnerFtp.CommunicationDetails))
            {
                throw new Exception("  שטר מטען בלדר  לממן -לא נמצא הגדרת תקשורת ");
            }
            var dtoWebApiDefinition = ProxyUtil.JsonConvertDeserializeTyped<WebApiDefinitionDTO>(pmCustomsPartnerFtp.CommunicationDetails);
            if (string.IsNullOrWhiteSpace(dtoWebApiDefinition.WEBAPIAuthenticationURL))
            {
                throw new Exception(" היינו שדה חובה שטר מטען בלדר  לממן -כתובת אימות השירות  ");
            }
            if (string.IsNullOrWhiteSpace(dtoWebApiDefinition.WEBAPIURL))
            {
                throw new Exception(" הינו שדה חובה שטר מטען בלדר  לממן -כתובת  השירות  ");
            }
            if (string.IsNullOrWhiteSpace(dtoWebApiDefinition.User))
            {
                throw new Exception(" הינו שדה חובה שטר מטען בלדר  לממן -שם משתמש  ");
            }

            if (string.IsNullOrWhiteSpace(dtoWebApiDefinition.Password))
            {
                throw new Exception(" הינו שדה חובה שטר מטען בלדר  לממן -סיסמא");
            }


            //.PostIt("", "F_unitedf", "Unit2019", data);
            var settings = new Courier2MamanCommSettings()
            {
                MessageCode = CustomsPartnerFtpDetails.InterfaceName_ECTHR,
                URIBaldarCreateECTHRMessgae = dtoWebApiDefinition.WEBAPIURL,/// @"https://maman.wsfreeze.co.il/WebAPIExt/api/baldar/CreateECTHRMessgae ",
                URIToken = dtoWebApiDefinition.WEBAPIAuthenticationURL, ///@"https://maman.wsfreeze.co.il/WebAPIExt/Token", //HTTP/1.1;

                username = dtoWebApiDefinition.User, //"F_unitedf",
                password = dtoWebApiDefinition.Password,// "Unit2019",

                Tenant = tenant,
                DeclarationId = declarationId
            };
            var settingsData = Logitude.Server.Tools.Utils.ProxyUtil.JsonConvertSerialize(settings);

            Document document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "TXT",
                FileSize = bytearray.Length,
                Tenant = Convert.ToInt32(tenant),
                Id = IdCounter.GetNumber("Document", tenant),
                HasFile = true,
                Folder = target.ToLower(),
            };

            documentRepository.Add(document);
            documentRepository.SubmitChanges();
            ContactRepository contactRepository = new ContactRepository(tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.ResolveLoggingUserId(tenant), tenant);
            string loggedContactId = "";
            if (loggedContact != null)
            {
                loggedContactId = loggedContact.Id;
            }

            CommunicationLog commLog = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                LastStatusDateUTC = DateTime.UtcNow,
                To = target,
                InOut = "O",
                EntityId = declarationId,
                ObjectTableId = objectTableId,
                Subject = xmlSubject,
                Tenant = tenant,
                CommunicationLogTypeCode = "T",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                CommunicationStatusTypeCode = "W",
                DocumentId = document.Id,
                CreateDateUTC = DateTime.UtcNow,
                CreatedByUserId = loggedContactId,
                LogSettings = settingsData,
                QueueName = SBQueueNames.SendWEBAPIMessage2MamanQ.ToString() ///using  by SendWEBAPIMessage2MamanWR
            };

            communicationLogRepository.Add(commLog);
            communicationLogRepository.SubmitChanges();

            Logitude.Server.Tools.BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = tenant,
                FileSize = bytearray.Length,
            };

            Logitude.Server.Tools.StorageService.IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            storageservice.Write(bytearray.ToArray(), fileInfo);

            SendCommunicationLogMessageToQueue(commLog.QueueName, commLog.Id, tenant);



        }

        

        private void SendCommunicationLogMessageToQueue(string queueName, string communicationLogId, int tenant)
        {
            try
            {
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue(queueName, 0);
                queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", communicationLogId }, { "Tenant", tenant.ToString() } });

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Send FTP CommunicationLog Queue", null, null);
            }
        }


#endif
        //public void AnalyzeResponse(Courier2MamanCommSettings settings, GWMessageECTHRData  responeGWMessageECTHRData)
        public void AnalyzeQResponse(CourierWEBAPICommSettings settings, string webAPIResultString)
        
        {
            
            var responeGWMessageECTHRData = ProxyUtil.JsonConvertDeserializeTyped<GWMessageECTHRData>(webAPIResultString);
            if (responeGWMessageECTHRData == null)
            {
                throw new Exception("(responeGWMessageECTHRData == null)");
            }
            LogMessagingUtil.Instance.AppendLine($"AnalyzeResponse(ResponseStatusCode={responeGWMessageECTHRData.ResponseStatusCode},{responeGWMessageECTHRData.ResponseStatusMsg})");
            var context = CustomContext.GetContext(settings.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myCourierMasterQueryService = new CourierMasterQueryService(context);
            var declarationPM = myDeclarationQueryService.GetSingle(settings.DeclarationId, true, false);
            declarationPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;



            switch (responeGWMessageECTHRData.ResponseStatusCode)
            {
                case 1://45997
                    {
                        declarationPM.MamanStatusCode = "1";
                    }
                    break;
/*                case 0:
                    {
                        declarationPM.MamanStatusCode = "1";
                    }
                    break;
                case 1:
                    {
                        declarationPM.MamanStatusCode = "2";
                    }
                    break;
                    */
                default:
                    declarationPM.MamanStatusCode = "2";//45997
                    //declarationPM.MamanStatusCode = responeGWMessageECTHRData.ResponseStatusCode.ToString();//???        //45997
                    break;
            }


            declarationPM.MamanErrorXml = responeGWMessageECTHRData.ResponseStatusCode.ToString() + "," + responeGWMessageECTHRData.ResponseStatusMsg??"";

            using (var scope = TransactionFactory.GetNewTransaction())
            {
                var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>() ,settings.Tenant);
                myDeclarationUpdateService.Update(declarationPM, true);
                scope.Complete();
            }
        }


        public void AnalyzeResponse(CourierWEBAPICommSettings settings, string webAPIResultString)
        {
            throw new Exception("use  SetInAnalyzeQResponseService by @intrface.ResponseCode");
            var customsPartnerFtpDetails = new CustomsPartnerFtpDetails();
            var def = customsPartnerFtpDetails.GetAllInterfaceDetails().First(r => r.Code == CustomsPartnerFtpDetails.InterfaceName_ECMMNTHR_RESPONE);
            var commSetting = Logitude.Server.Tools.Utils.ProxyUtil.JsonConvertSerialize(settings);
            var analyzeQueueUtil = new AnalyzeQueueUtil();
            var new_analyze = analyzeQueueUtil
               .SaveMessageToAnalyzeQueue("", Encoding.UTF8.GetBytes(webAPIResultString), settings.Tenant,
               commSetting, def,
               new AnalyzeResultModel()
               {
                   EntityID = settings.DeclarationId,
                   ObjectTableID = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),

               });

            LogMessagingUtil.Instance.AppendLine($"new_analyze  CommunicationLogId = {new_analyze.CommunicationLogId}");

        }

    }
  

}
