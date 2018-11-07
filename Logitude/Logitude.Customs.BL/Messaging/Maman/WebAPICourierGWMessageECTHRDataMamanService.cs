
    //https://maman.wsfreeze.co.il/WebAPIExt/Help/Api/POST-api-baldar-CreateECTHRMessgae
    //https://docs.google.com/document/d/1cjjeORaFsWMS32LhIxmEqNza3q7s7ZAr_PQVQOlwupw/edit#
    


using Logitude.BL.Helpers;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
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
    public class WebAPICourierGWMessageECTHRDataMamanService///using  by SendWebAPI2MamanGWMessageECTHRDataWR
    {


        private string communicationSubject = "שידור מסר שטר מטען בלדר  לממן";
        public void BuildCommunicationLog(byte[] bytearray, int tenant, string declarationId)///using  by SendWebAPI2MamanGWMessageECTHRDataWR
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










            var settings = new CourierHawbMamanCommunicationLogSettings()
            {
                host = "https://maman.wsfreeze.co.il/api/baldar/CreateECTHRMessgae",
                
                username = "",
                password = "",

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
                LogSettings = settingsData,
                QueueName = SBQueueNames.SendGWMessageECTHRData2MamanQ.ToString() ///using  by SendWebAPI2MamanGWMessageECTHRDataWR
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

        public void AnalyzeResponse(CourierHawbMamanCommunicationLogSettings settings, GWMessageECTHRData  responeGWMessageECTHRData)
        {
            var context = CustomContext.GetContext(settings.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myCourierMasterQueryService = new CourierMasterQueryService(context);
            var declarationPM = myDeclarationQueryService.GetSingle(settings.DeclarationId, false, false);
            declarationPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            declarationPM.MamanStatusCode = responeGWMessageECTHRData.ResponseStatusCode.ToString();
            declarationPM.MamanErrorXml = responeGWMessageECTHRData.ResponseStatusMsg;

            using (var scope = TransactionFactory.GetNewTransaction())
            {
                var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>() ,settings.Tenant);
                myDeclarationUpdateService.Update(declarationPM, true);
                scope.Complete();
            }
        }

    }
    public class CourierHawbMamanCommunicationLogSettings
    {
        public string host { get; set; }
        
        public string username { get; set; }
        public string password { get; set; }

        public int Tenant { get; set; }
        
        public string DeclarationId { get; internal set; }
    }

}
