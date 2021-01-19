using Logitude.BL.CommonDataModel.EntityQueries;
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
        private string additionalFolderDetails = string.Empty;
        private string documentFileName = string.Empty;
        private Contact loggedContact = null;
        private string companyName = string.Empty;
        private string computingPartnerName = string.Empty;
        public FTPAutomationService(FTPAutomationServiceArgs ftpAutomationServiceArgs)
        {
            tenant = ftpAutomationServiceArgs.Tenant;
            documentId = ftpAutomationServiceArgs.DocumentId;
            fTPDetails = ftpAutomationServiceArgs.FTPDetails;
            entityId = ftpAutomationServiceArgs.EntityId;
            objectTableId = ftpAutomationServiceArgs.ObjectTableId;
            additionalFolderDetails = ftpAutomationServiceArgs.AdditionalFolderDetails;
            documentFileName = ftpAutomationServiceArgs.DocumentFileName;
            loggedContact = GetLoggedContact();
            companyName = GetCompanyName();
            computingPartnerName = GetComputingPartnerName(ftpAutomationServiceArgs.ComputingPartnerId);
        }


        public void Run()
        {
            var communicationLog = CreateCommunicationLog();
            AddFTPCommunicationLogQueue(communicationLog);
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
                To = fTPDetails.Host,
                From = companyName,
                Subject = string.IsNullOrEmpty(documentFileName) ? string.IsNullOrEmpty(computingPartnerName)? "Shipment Interface": ("Shipment Interface for "+ computingPartnerName):documentFileName,
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                LastStatusDateUTC = System.DateTime.UtcNow,
                InOut = "O",
                EntityId = entityId,
                ObjectTableId = objectTableId,
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
            
            CommunicationLogSettings communicationLogSettings = new CommunicationLogSettings()
            {
                Host = fTPDetails.Host,
                Folder = fTPDetails.Folder + additionalFolderDetails,
                Username = fTPDetails.UserName,
                Password = fTPDetails.Password,
            };

            communicationLogSettings.Filename = string.IsNullOrEmpty(documentFileName) ? GetDocumentFileName():documentFileName;
            myResult = JsonConvert.SerializeObject(communicationLogSettings);

            return myResult;

        }

 

        private void AddFTPCommunicationLogQueue(CommunicationLog communicationLog)
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue(communicationLog.QueueName, 0);
            queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", communicationLog.Id }, { "Tenant", tenant.ToString() } }, tenant);
        }

        private Contact GetLoggedContact()
        {
            string loggedUserEmail = AuthenticationUtil.GetLoggedUserEmail(tenant);
            ContactRepository contactRepository = new ContactRepository(tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(loggedUserEmail, tenant);
            return loggedContact;
        }

        private string GetComputingPartnerName(string computingPartnerId)
        {
            string result = string.Empty;
            ComputingPartnerQuery computingPartnerQuery = new ComputingPartnerQuery(tenant);
            result = computingPartnerQuery.GetComputingPartnerNameById(computingPartnerId);
            return result;
        }

        private string GetCompanyName()
        {
            string result = string.Empty;
            TenantQuery tenantQuery = new TenantQuery(tenant);
            result = tenantQuery.GetCompanyNameById(tenant);
            return result;
        }
     
        private string GetDocumentFileName()
        {
            DocumentRepository documentRepository = new DocumentRepository(tenant);
            string fileName = documentRepository.GetFileNameByDocumentId(documentId, tenant);
            return fileName;
        }

    }

    public class FTPAutomationServiceArgs
    {
        public FTPAutomationDetails FTPDetails { get; set; }
        public string DocumentId { get; set; }
        public int Tenant { get; set; }
        public string EntityId { get; set; }
        public string ObjectTableId { get; set; }
        public string ComputingPartnerId { get; set; }
        public string AdditionalFolderDetails { get; set; }
        public string DocumentFileName { get; set; }
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