using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.FTP;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TextManager.Interop;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.DataContracts;

namespace WebFreight.Web.Helpers.AutomationModel
{
    public class FTPAutomationService
    {

        private int tenant = 0;
        private string documentId = string.Empty;
        private FTPAutomationDetails fTPDetails;
        private string entityId = string.Empty;
        private string objectTableId = string.Empty;
        private Contact loggedContact = null;


        public FTPAutomationService(FTPAutomationServiceArgs ftpAutomationServiceArgs)
        {
            tenant = ftpAutomationServiceArgs.Tenant;
            documentId = ftpAutomationServiceArgs.DocumentId;
            fTPDetails = ftpAutomationServiceArgs.FTPDetails;
            entityId = ftpAutomationServiceArgs.EntityId;
            objectTableId = ftpAutomationServiceArgs.ObjectTableId;
            loggedContact = this.GetLoggedContact();
        }


        public void Run()
        {
            var communicationLog = CreateCommunicationLog();
            AddFTPCommunicationLogQueue(communicationLog);
        }


        private Contact GetLoggedContact()
        {
            string loggedUserEmail = AuthenticationUtil.GetLoggedUserEmail(tenant);
            ContactRepository contactRepository = new ContactRepository(tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(loggedUserEmail, tenant);
            return loggedContact;
        }

        private CommunicationLog CreateCommunicationLog()
        {
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(tenant);
            CommunicationLog communicationLog = GetNewCommunicationLog();
            communicationLogRepository.Add(communicationLog);
            communicationLogRepository.SubmitChanges();
            return communicationLog;
        }

        private CommunicationLog GetNewCommunicationLog()
        {
            return new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                LastStatusDateUTC = System.DateTime.UtcNow,
                InOut = "O",
                From = loggedContact.Email,
                EntityId = entityId,
                ObjectTableId = objectTableId,
                Subject = "Shipment XML via Automation",
                Tenant = tenant,
                CommunicationLogTypeCode = "T",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                CommunicationStatusTypeCode = "W",
                CreatedByUserId = loggedContact.Id,
                DocumentId = documentId,
                CreateDateUTC = System.DateTime.UtcNow,
                LogSettings = this.GetCommunicationLogSettingAsJosnString(),
                QueueName = "FTPCommunicationLogQueue",
            };
        }

        private string GetCommunicationLogSettingAsJosnString()
        {
            string myResult = string.Empty;

            CommunicationLogSettings settings = new CommunicationLogSettings()
            {
                Host = fTPDetails.Host,
                Folder = fTPDetails.Folder,
                Username = fTPDetails.UserName,
                Password = fTPDetails.Password,
                Filename = documentId,
            };

            myResult = JsonConvert.SerializeObject(settings);

            return myResult;

        }

        private void AddFTPCommunicationLogQueue(CommunicationLog communicationLog)
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue(communicationLog.QueueName, 0);
            queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", communicationLog.Id }, { "Tenant", tenant.ToString() } }, tenant);
        }

    }

    public class FTPAutomationServiceArgs
    {
        public FTPAutomationDetails FTPDetails { get; set; }
        public string DocumentId { get; set; }
        public int Tenant { get; set; }
        public string EntityId { get; set; }
        public string ObjectTableId { get; set; }

    }

    public class CommunicationLogSettings
    {
        public string Host { get; set; }
        public string Folder { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Filename { get; set; }
    }
}