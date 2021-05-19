
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

namespace Logitude.Customs.BL.Messaging.ILSWS
{
    public class FTPOutMawbSWSService
    {


        //private string communicationSubject = "שידור פנימיים מסוכנים לממן";
        public void BuildCommunicationLog(byte[] bytearray, int tenant, string declarationId, string fileName)
        {
          
            ObjectTableRepository repo = new ObjectTableRepository(tenant);
            var objectTableId = repo.GetObjectTableIdByName("Customs.Declaration");

            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
            DocumentRepository documentRepository = new DocumentRepository(commonContext);

            string target = "ILSWS";

            string host = "";
            string folder = "";
            string username = "";
            string password = "";
            bool useSFTP = false;

            var myCustomsPartnerFtpQueryService = new CustomsPartnerFtpQueryService(tenant);
            var pmCustomsPartnerFtp = myCustomsPartnerFtpQueryService.GetBy(tenant, ftpDetailsName, CustomsPartnerFtpDetails.PartnerCode_ILSWS, CustomsPartnerFtpDetails.TypeCode_Out);
            string xmlSubject = (new CustomsPartnerFtpDetails()).GetAllInterfaceDetails().First(r => r.Code == CustomsPartnerFtpDetails.InterfaceName_ECSWSTHR_REQUEST).Subject; ;
            

            if (pmCustomsPartnerFtp == null)
            {
                LogMessagingUtil.Instance.AppendLine("FTPOutMawbSWSService(): NO FTP DEFINITION !!!");
                return; 
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
                useSFTP = fTPDetail.UseSFTP;
            }
            if (fileName == null)
            {
                fileName = Path.GetFileNameWithoutExtension(pmCustomsPartnerFtp.FileName);
            }

            var settings = new CommunicationLogSettings() { host = host, folder = folder, username = username, password = password, filename = fileName, UseSFTP= useSFTP };
            var settingsData = Logitude.Server.Tools.Utils.ProxyUtil.JsonConvertSerialize(settings);

            Document document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = pmCustomsPartnerFtp.FileExt,
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

        public class CommunicationLogSettings
        {
            public string host { get; set; }
            public string folder { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            public string filename { get; set; }

            public bool UseSFTP { get; set; }
        }

    }


}
