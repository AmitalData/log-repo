
using Logitude.BL.Helpers;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.Maman
{
    public class FTPOutMaman2470ReleaseGoodService///using  by FTPCommunicationWorkerRole
    {

        public void BuildCommunicationLog(byte[] bytearray, int tenant, string declarationId, string FileName, bool sendIsMust)
        {
            var myCustomsPartnerFtpQueryService = new CustomsPartnerFtpQueryService(tenant);
            var pmCustomsPartnerFtp = myCustomsPartnerFtpQueryService.GetBy(tenant, CustomsPartnerFtpDetails.InterfaceName_Ftp2Maman2470, CustomsPartnerFtpDetails.PartnerCode_Mamam, CustomsPartnerFtpDetails.TypeCode_Out);
            var ftpOutService = new FtpOutService();
            ftpOutService.BuildCommunicationLog(tenant, new FtpOutParams()
            {
                bytearray = bytearray,
                tablename = "Customs.Declaration",
                entityId = declarationId,
                MyFileName = new FtpOutParams.FileName(FileName),
                sendIsMust = sendIsMust,
                MyCustomsPartnerFtpPM = pmCustomsPartnerFtp,
            });
        }

        //private string communicationSubject = "שידור פנימיים מסוכנים לממן";
        public void BuildCommunicationLogOld(byte[] bytearray, int tenant, string declarationId, string FileName,bool sendIsMust)
        {

            ObjectTableRepository repo = new ObjectTableRepository(tenant);
            var objectTableId = repo.GetObjectTableIdByName("Customs.Declaration");

            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
            DocumentRepository documentRepository = new DocumentRepository(commonContext);

            string target = "Maman";

            string host = "";
            string folder = "";
            string username = "";
            string password = "";

            var myCustomsPartnerFtpQueryService = new CustomsPartnerFtpQueryService(tenant);
            var pmCustomsPartnerFtp = myCustomsPartnerFtpQueryService.GetBy(tenant, CustomsPartnerFtpDetails.InterfaceName_Ftp2Maman2470, CustomsPartnerFtpDetails.PartnerCode_Mamam, CustomsPartnerFtpDetails.TypeCode_Out);
            string xmlSubject = (new CustomsPartnerFtpDetails()).GetAllInterfaceDetails().First(r => r.Code == CustomsPartnerFtpDetails.InterfaceName_Ftp2Maman2470).Subject; ;


            if (pmCustomsPartnerFtp == null)
            {
                LogMessagingUtil.Instance.AppendLine("FTPOutMaman2470ReleaseGoodService(): NO FTP DEFINITION !!!");
                if (sendIsMust)
                {
                    throw new Exception("no ftp definition for  FTPOutMaman2470ReleaseGoodService");
                }
                else
                {
                    return;
                }
            }
            //string artemusOutSettingsId = interfaceSetting.ArtemusOutSettingsId;
            string FtpDetailsId = pmCustomsPartnerFtp.FtpDetailsId;
            FTPDetailRepository fTPDetailRepository = new FTPDetailRepository(tenant);
            FTPDetail fTPDetail = (from d in commonContext.FTPDetails
                                   where d.Tenant == tenant && d.Id == FtpDetailsId
                                   select d).FirstOrDefault();

            if (fTPDetail != null)
            {
                host = fTPDetail.Host;
                folder = fTPDetail.Folder;
                username = fTPDetail.UserName;
                password = fTPDetail.Password;
            }


            var settings = new CommunicationLogSettings() { host = host, folder = folder, username = username, password = password, filename = Path.GetFileNameWithoutExtension(FileName) };
            var settingsData = Logitude.Server.Tools.Utils.ProxyUtil.JsonConvertSerialize(settings);

            Document document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = Path.GetExtension(FileName),
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
                LogSettings = settingsData,
                QueueName = "FTPCommunicationLogQueue" ,///using  by FTPCommunicationWorkerRole
                CreatedByUserId= loggedContactId
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
                queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", communicationLogId }, { "Tenant", tenant.ToString() } }, tenant);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Send FTP CommunicationLog Queue", null, null);
            }
        }
    }


}
