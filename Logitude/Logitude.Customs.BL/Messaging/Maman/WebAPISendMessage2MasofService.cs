using Logitude.BL.Helpers;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
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
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;

namespace Logitude.Customs.BL.Messaging/*.Maman*/
{
    public   class WebAPISendMessage2MasofService
    {
        //private string _communicationSubject = "שידור מסר  פעולות מיוחדות לממן";


        public void BuildCommunicationLog(byte[] bytearray, int tenant, string declarationId, string InterfaceName, string PartnerCode)///using  by SendWEBAPIMessage2MamanWRWR
        {
            ObjectTableRepository repo = new ObjectTableRepository(tenant);
            var objectTableId = repo.GetObjectTableIdByName("Customs.Declaration"/*"Customs.CourierMaster"*/);

            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
            DocumentRepository documentRepository = new DocumentRepository(commonContext);
            var amitalContext = AmitalContext.GetContext(tenant);
            var myGDFDATAQueryService = new GDFDATAQueryService(amitalContext);

            switch (PartnerCode)
            {
                case CustomsPartnerFtpDetails.PartnerCode_Mamam:
                    {
                        var def = myGDFDATAQueryService.GetSingle("ISRAEL", "CGO_CUST_MAMAN", "NON", "NON", false, true);

                        bool sendMamanWEBAPIIsOn = def.DEFDATA.Contains("ILMMN");
                        if (!sendMamanWEBAPIIsOn)
                        {
                            throw new Exception("WebAPISendMessage2MamanService()->!sendMamanWEBAPIIsOn");
                        }

                    }
                    break;
                default:
                    break;
            }

            var customsPartnerFtpDetails = new CustomsPartnerFtpDetails();
            var defDefaultJSON = customsPartnerFtpDetails.GetAllInterfaceName().First(r => r.Key == InterfaceName).Value;
            var defDefault = ProxyUtil.JsonConvertDeserializeTyped<InterfaceDetails>(defDefaultJSON);

            var myCustomsPartnerFtpQueryService = new CustomsPartnerFtpQueryService(tenant);
            var pmCustomsPartnerFtp = myCustomsPartnerFtpQueryService.GetBy(tenant, InterfaceName /*CustomsPartnerFtpDetails.InterfaceName_ECSPCL*/,
                PartnerCode/*CustomsPartnerFtpDetails.PartnerCode_Mamam*/,
                CustomsPartnerFtpDetails.TypeCode_Out);


            if (string.IsNullOrWhiteSpace(pmCustomsPartnerFtp.CommunicationDetails))
            {
                throw new Exception($"  מסר {defDefault.Name} -לא נמצא הגדרת תקשורת ");
            }
            var dtoWebApiDefinition = ProxyUtil.JsonConvertDeserializeTyped<WebApiDefinitionDTO>(pmCustomsPartnerFtp.CommunicationDetails);
            if (string.IsNullOrWhiteSpace(dtoWebApiDefinition.WEBAPIAuthenticationURL))
            {
                throw new Exception($" היינו שדה חובה מסר {defDefault.Name} -כתובת אימות השירות  ");
            }
            if (string.IsNullOrWhiteSpace(dtoWebApiDefinition.WEBAPIURL))
            {
                throw new Exception($" הינו שדה חובה מסר {defDefault.Name} לממן -כתובת  השירות  ");
            }
            if (string.IsNullOrWhiteSpace(dtoWebApiDefinition.User))
            {
                throw new Exception($" הינו שדה חובה {defDefault.Name}-שם משתמש  ");
            }

            if (string.IsNullOrWhiteSpace(dtoWebApiDefinition.Password))
            {
                throw new Exception($" הינו שדה חובה {defDefault.Name} -סיסמא");
            }
            using (var scop = TransactionFactory.GetTransaction())
            {
                ContactRepository contactRepository = new ContactRepository(tenant);
                Contact loggedContact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.ResolveLoggingUserId(tenant), tenant);
                string loggedContactId = "";
                if (loggedContact != null)
                {
                    loggedContactId = loggedContact.Id;
                }

                //.PostIt("", "F_unitedf", "Unit2019", data);
                var settings = new CourierWEBAPICommSettings()
                {
                    ///WEBAPICredentialType = defDefault.WEBAPICredentialType,
                    MessageCode = InterfaceName,
                    URIMethod = dtoWebApiDefinition.WEBAPIURL,/// @"https://maman.wsfreeze.co.il/WebAPIExt/api/baldar/CreateECTHRMessgae ",
                    URIToken = dtoWebApiDefinition.WEBAPIAuthenticationURL, ///@"https://maman.wsfreeze.co.il/WebAPIExt/Token", //HTTP/1.1;

                    username = dtoWebApiDefinition.User, //"F_unitedf",
                    password = dtoWebApiDefinition.Password,// "Unit2019",

                    Tenant = tenant,
                    DeclarationId = declarationId,
                    LoggedContactId = loggedContactId
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
                    Folder = /*CustomsPartnerFtpDetails.PartnerCode_Mamam*/PartnerCode.ToLower(),
                };

                documentRepository.Add(document);
                documentRepository.SubmitChanges();


                CommunicationLog commLog = new CommunicationLog()
                {
                    Id = IdCounter.GetNumber("CommunicationLog", tenant),
                    LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    LastStatusDateUTC = DateTime.UtcNow,
                    To = /*CustomsPartnerFtpDetails.PartnerCode_Mamam*/PartnerCode,
                    InOut = "O",
                    EntityId = declarationId,
                    ObjectTableId = objectTableId,
                    Subject = defDefault.Name,
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
                scop.Complete();
            }


        }



        private void SendCommunicationLogMessageToQueue(string queueName, string communicationLogId, int tenant)
        {
            try
            {
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue(queueName, 0);
                queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", communicationLogId }, { "Tenant", tenant.ToString() } },tenant);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Send FTP CommunicationLog Queue", null, null);
            }
        }


    }

    public interface IWebAPIMessage2MamanAnalyzer
    {
        void AnalyzeResponse(CourierWEBAPICommSettings settings, string webAPIResultString);
    }


   
    public class CourierWEBAPICommSettings
    {
        
        
        public string URIToken { get; set; }////@"https://maman.wsfreeze.co.il/WebAPIExt/Token"; //HTTP/1.1;
        public string URIMethod { get; set; }///"https://maman.wsfreeze.co.il/WebAPIExt/api/baldar/CreateECTHRMessgae";

        //.PostIt("", "ftp-uti", "Pariz2019+", data);
        //.PostIt("", "F_unitedf", "Unit2019", data);

        public string username { get; set; }
        public string password { get; set; }


        public int Tenant { get; set; }
        public string DeclarationId { get; set; }
        public string MessageCode { get;  set; }
        public string LoggedContactId { get;  set; }
        public string RqstCommLogID { get; set; }
    }
}

