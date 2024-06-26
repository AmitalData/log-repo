using Logitude.BL.Helpers;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging
{
    
    public class FtpOutService///using  by FTPCommunicationWorkerRole
    {



        public void BuildCommunicationLog(int tenant,FtpOutParams ftpOutParams)
        {

            if (ftpOutParams.bytearray==null)
            {
                throw new Exception("ftpOutParams.bytearray==null"); 
            }

            var pmCustomsPartnerFtp = ftpOutParams.MyCustomsPartnerFtpPM;
            if (pmCustomsPartnerFtp == null || (pmCustomsPartnerFtp!=null && String.IsNullOrWhiteSpace( pmCustomsPartnerFtp.Id)))
            {
                LogMessagingUtil.Instance.AppendLine("{{xmlSubject}}: NO FTP DEFINITION !!!");
                if (ftpOutParams.sendIsMust)
                {
                    throw new Exception("no ftp definition for  {xmlSubject}"); /*FTPOutMaman2470ReleaseGoodService*/
                }
                else
                {
                    return;
                }
            }
            if (string.IsNullOrWhiteSpace(Path.GetExtension(ftpOutParams.MyFileName.FileWithExtension)))
            {
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug("!!Warning!!!File name without Extension !!!Warning!!");
                AmitalDebuggerUtil.Break(AmitalDebuggerLevel.Warning);
            }

            string loggedContactId = (new LoggedContactService()).GetLoggedContactId(tenant);

            string xmlSubject = (new CustomsPartnerFtpDetails()).GetAllInterfaceDetails().First(r => r.Code == pmCustomsPartnerFtp.InterfaceName).Subject;
            string target = pmCustomsPartnerFtp.PartnerCode /*"Maman"*/;

            ObjectTableRepository repo = new ObjectTableRepository(tenant);
            string objectTableId = repo.GetObjectTableIdByName(ftpOutParams.tablename /*"Customs.Declaration"*/);

            //string artemusOutSettingsId = interfaceSetting.ArtemusOutSettingsId;
            string FtpDetailsId = pmCustomsPartnerFtp.FtpDetailsId;

            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            FTPDetail fTPDetail = (from d in commonContext.FTPDetails
                                   where d.Tenant == tenant && d.Id == FtpDetailsId
                                   select d).FirstOrDefault();


            var settings = new CommunicationLogSettings()
            {
                host = fTPDetail.Host,
                folder = fTPDetail.Folder,
                username = fTPDetail.UserName,
                password = fTPDetail.Password,
                filename = Path.GetFileNameWithoutExtension(ftpOutParams.MyFileName.FileWithExtension),
                UseSFTP = fTPDetail.UseSFTP
            };
            var settingsData = Logitude.Server.Tools.Utils.ProxyUtil.JsonConvertSerialize(settings);

            CommunicationLog commLog = CreateCommunicationLog(tenant, ftpOutParams, loggedContactId, xmlSubject, target, objectTableId, commonContext, settingsData);
            var customsEnvironmentSettingQueryService = new CustomsEnvironmentSettingQueryService(1);
            var customsEnvironmentSettingPM = customsEnvironmentSettingQueryService.GetEnvironmentSettingPM() ?? new CustomsEnvironmentSettingPM();
            bool UseRabbitMQ = customsEnvironmentSettingPM.UseRabbitMQ;//currInterfaceTenantDefinition.UseRabbitMQ;

            CustomDbQueueService.SendCommunicationLogMessageToQueue(commLog.QueueName, commLog.Id, tenant, UseRabbitMQ);


        }

        private CommunicationLog CreateCommunicationLog(int tenant, FtpOutParams ftpOutParams, string loggedContactId, string xmlSubject, string target, string objectTableId, ICommonDataContext commonContext, string settingsData)
        {
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
            DocumentRepository documentRepository = new DocumentRepository(commonContext);
            Document document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = Path.GetExtension(ftpOutParams.MyFileName.FileWithExtension),
                FileSize = ftpOutParams.bytearray.Length,
                Tenant = Convert.ToInt32(tenant),
                Id = IdCounter.GetNumber("Document", tenant),
                HasFile = true,
                Folder = target.ToLower(),
            };
            documentRepository.Add(document);
            documentRepository.SubmitChanges();

            bool singleton = false;
            if (target == "MAMAN")
            {
                singleton = true;
            } ///!!!!!

            CommunicationLog commLog = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                LastStatusDateUTC = DateTime.UtcNow,
                To = target,
                InOut = "O",
                EntityId = ftpOutParams.entityId /*declarationId*/,
                ObjectTableId = objectTableId,
                Subject = xmlSubject,
                Tenant = tenant,
                CommunicationLogTypeCode = "T",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                CommunicationStatusTypeCode = "W",
                DocumentId = document.Id,
                CreateDateUTC = DateTime.UtcNow,
                LogSettings = settingsData,
                //
                QueueName = (singleton ? "Singleton" : "") + "FTPCommunicationLogQueue",///using  by FTPCommunicationWorkerRole
                CreatedByUserId = loggedContactId
            };

            communicationLogRepository.Add(commLog);
            communicationLogRepository.SubmitChanges();

            Logitude.Server.Tools.BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = tenant,
                FileSize = ftpOutParams.bytearray.Length,
            };

            Logitude.Server.Tools.StorageService.IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            storageservice.Write(ftpOutParams.bytearray.ToArray(), fileInfo);
            return commLog;
        }

        




        //private void SendCommunicationLogMessageToQueue(string queueName, string communicationLogId, int tenant)
        //{
        //    try
        //    {
        //        IQueueService queueservice = new DbQueueService();
        //        queueservice.InitializeQueue(queueName, 0);
        //        queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", communicationLogId }, { "Tenant", tenant.ToString() } }, tenant);

        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Send FTP CommunicationLog Queue", null, null);
        //    }
        //}
    }

    public class FtpOutParams
    {
        public byte[] bytearray { get; set; }
        public string entityId { get; set; }
        public string tablename { get; set; }
        public bool sendIsMust { get; set; }

        public CustomsPartnerFtpPM MyCustomsPartnerFtpPM { get; set; }

        public FileName MyFileName { get; set; }

        public class FileName
        {
            public readonly string FileWithExtension;
            public FileName(string FileWithExtension)
            {
                this.FileWithExtension = FileWithExtension;

            }
            public FileName(bool GenrateUniqueFileName, string ExtensionWithoutDot)
            {
                this.FileWithExtension = $"{GetDefaultFileName()}.{ExtensionWithoutDot}";
            }
            private static string GetDefaultFileName()
            {
                string FileName;
                DateTime @now = DateTime.Now;
                string MM = "00" + @now.Month.ToString();
                MM = MM.Substring(MM.Length - 2);
                string dd = "00" + @now.Day.ToString();
                dd = dd.Substring(dd.Length - 2);
                var hhmn = @now.ToString("HH:mm").Replace(":", String.Empty);
                FileName = $"A{MM}{dd}{hhmn}";//.HWB";
                return FileName;
            }
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

    public class LoggedContactService
    {
        public string GetLoggedContactId(int tenant)
        {
            ContactRepository contactRepository = new ContactRepository(tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.ResolveLoggingUserId(tenant), tenant);
            string loggedContactId = "";
            if (loggedContact != null)
            {
                loggedContactId = loggedContact.Id;
            }

            return loggedContactId;
        }
    }
    
}
