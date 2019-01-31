using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




using Logitude.Server.Tools.Models;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
//using Microsoft.WindowsAzure.ServiceRuntime;

using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnifreightIIG.UServer;

using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Server.Infrastructure;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.Helpers;
using Logitude.Server.Tools;

using Logitude.Customs.BL.Messaging.Maman;
using Microsoft.Practices.Unity;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.FTP;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel.Repositories;
using Unifreight.Data.AmitalModel;
using Logitude.Server.Tools.Utils;


namespace Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue
{
    public class AnalyzeQueueUtil
    {

        public AnalyzeQueue SaveMessageToAnalyzeQueue(string fileName, byte[] messageData, int tenant, InterfaceDetails defInterfaceDetail)
        {
            AnalyzeQueue analyzeQueue = null;
            //var defInterfaceDetail = (new CustomsPartnerFtpDetails()).GetAllInterfaceDetails().First(r => r.Code == customsPartnerFtpPM.InterfaceName);
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {

                //ProccessReceivedMessage();

                fileName = fileName.Split('/')[fileName.Split('/').Length - 1].ToLower();
                var analyzeQueueReposiory = new AnalyzeQueueRepository();

                analyzeQueue = new AnalyzeQueue()
                {
                    Subject = defInterfaceDetail.Subject,
                    //fileName.StartsWith("bl") ? "BL Response" : (fileName.StartsWith("voyage") ? "Voyage Response" : "Artemus Response"),
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(0),
                    From = defInterfaceDetail.Partner + "," + defInterfaceDetail.Code,
                    Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                    MessageBody = System.Text.Encoding.UTF8.GetBytes(" "),//ismust !!!!!
                    Status = "W",
                    Retries = 0,
                    ConnectedToEntity = false,
                    ConnectedToTenant = true,
                    Tenant = 0,// IHAB said It Must be ZERO !!!
                    FileSize = System.Text.Encoding.UTF8.GetBytes(" ").Length,
                    FileName = fileName,

                    CommunicationLogId = BuildCommunicationLog(messageData, tenant, defInterfaceDetail)
                };

                analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
                analyzeQueueReposiory.Add(analyzeQueue);
                analyzeQueueReposiory.SubmitChanges();

                scope.Complete();
            }
            return analyzeQueue;
        }



        public string BuildCommunicationLog(byte[] bytearray, int tenant, InterfaceDetails defInterfaceDetail)///using  by SendWEBAPIMessage2MamanWRWR
        {


            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
            DocumentRepository documentRepository = new DocumentRepository(commonContext);








            ContactRepository contactRepository = new ContactRepository(tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.ResolveLoggingUserId(tenant), tenant);
            string loggedContactId = "";
            if (loggedContact != null)
            {
                loggedContactId = loggedContact.Id;
            }

            //.PostIt("", "F_unitedf", "Unit2019", data);
            var settings = new Courier2MamanCommSettings()
            {
                MessageCode = defInterfaceDetail.Code,
                Tenant = tenant,
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
                Folder = CustomsPartnerFtpDetails.PartnerCode_Mamam.ToLower(),
            };

            documentRepository.Add(document);
            documentRepository.SubmitChanges();

            var def = (new CustomsPartnerFtpDetails()).GetAllInterfaceDetails().First(r => r.Code == defInterfaceDetail.Code);
            CommunicationLog commLog = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                LastStatusDateUTC = DateTime.UtcNow,
                //To = ,
                From = CustomsPartnerFtpDetails.PartnerCode_Mamam + "," + CustomsPartnerFtpDetails.InterfaceName_ECSTB,
                InOut = "O",
                //EntityId = declarationId,
                //ObjectTableId = objectTableId,
                Subject = defInterfaceDetail.Subject,
                Tenant = tenant,
                CommunicationLogTypeCode = "T",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                CommunicationStatusTypeCode = "W",
                DocumentId = document.Id,
                CreateDateUTC = DateTime.UtcNow,
                CreatedByUserId = loggedContactId,
                LogSettings = settingsData,
                //QueueName = def.QueueName //SBQueueNames.SendWEBAPIMessage2MamanQ.ToString() ///using  by SendWEBAPIMessage2MamanWR
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
            bool useQueueMessage = false;
            if (useQueueMessage)
            {
                SendCommunicationLogMessageToQueue(commLog.QueueName, commLog.Id, tenant);
            }


            return commLog.Id;

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
    }
}
