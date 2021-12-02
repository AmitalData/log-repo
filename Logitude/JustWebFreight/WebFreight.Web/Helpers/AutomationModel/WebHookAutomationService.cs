using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System.Collections.Generic;

namespace WebFreight.Web.Helpers.AutomationModel
{
    public class WebHookAutomationService
    {
        private int tenant = 0;
        private WebHookAutomationDetails webHookDetails;
        private string documentId = string.Empty;
        private string entityId = string.Empty;
        private string objectTableId = string.Empty;
        private string documentFileName = string.Empty;
        private Contact loggedContact = null;
        private string companyName = string.Empty;
        private string computingPartnerName = string.Empty;
        public WebHookAutomationService(WebHookAutomationServiceArgs webHookAutomationServiceArgs)
        {
            tenant = webHookAutomationServiceArgs.Tenant;
            documentId = webHookAutomationServiceArgs.DocumentId;
            entityId = webHookAutomationServiceArgs.EntityId;
            objectTableId = webHookAutomationServiceArgs.ObjectTableId;
            webHookDetails = webHookAutomationServiceArgs.WebHookDetails;
            documentFileName = webHookAutomationServiceArgs.DocumentFileName;
            loggedContact = GetLoggedContact();
            companyName = GetCompanyName();
            computingPartnerName = GetComputingPartnerName(webHookAutomationServiceArgs.ComputingPartnerId);
        }

        private Contact GetLoggedContact()
        {
            string loggedUserEmail = AuthenticationUtil.GetLoggedUserEmail(tenant);
            ContactRepository contactRepository = new ContactRepository(tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(loggedUserEmail, tenant);
            return loggedContact;
        }

        private string GetCompanyName()
        {
            TenantQuery tenantQuery = new TenantQuery(tenant);
            string result = tenantQuery.GetCompanyNameById(tenant);
            return result;
        }

        private string GetComputingPartnerName(string computingPartnerId)
        {
            ComputingPartnerQuery computingPartnerQuery = new ComputingPartnerQuery(tenant);
            string result = computingPartnerQuery.GetComputingPartnerNameById(computingPartnerId);
            return result;
        }

        public void Run()
        {
            CommunicationLog communicationLog = CreateCommunicationLog();
            AddWebHookCommunicationLogQueue(communicationLog);
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
            string to = GetToDomainString();
            return new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", tenant),
                To = to,
                From = companyName,
                Subject = string.IsNullOrEmpty(documentFileName) ? string.IsNullOrEmpty(computingPartnerName) ? "Shipment Interface" : ("Shipment Interface for " + computingPartnerName) : documentFileName,
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
                LogSettings = this.GetWebHookCommunicationLogSettingAsJosnString(),
                QueueName = "WebHookCommunicationLogQueue",
            };
        }

        private string GetToDomainString()
        {
            string to = webHookDetails.URL;
            to = to.Replace("https://", "");
            to = to.Replace("http://", "");
            if (to.IndexOf('/') > -1)
            {
                to = to.Substring(0, to.IndexOf('/'));
            }
            return to;
        }

        private string GetWebHookCommunicationLogSettingAsJosnString()
        {
            WebHookCommunicationLogSettings webHookCommunicationLogSettings = new WebHookCommunicationLogSettings();
            webHookCommunicationLogSettings.URL = GetWebHookURL();
            webHookCommunicationLogSettings.Filename = string.IsNullOrEmpty(documentFileName) ? GetDocumentFileName() : documentFileName;
            string result = JsonConvert.SerializeObject(webHookCommunicationLogSettings);

            return result;
        }

        private string GetWebHookURL()
        {
            if (webHookDetails.AuthenticationType == "BASICAUTHENTICATION")
                return GetWebHookBasicAuthenticationURL();
            return webHookDetails.URL;
        }

        private string GetWebHookBasicAuthenticationURL()
        {
            return "https://" + webHookDetails.BasicAuthUserName + ":" + webHookDetails.BasicAuthPassword + "@" + webHookDetails.URL.Replace("https://", "");
        }

        private string GetDocumentFileName()
        {
            DocumentRepository documentRepository = new DocumentRepository(tenant);
            string fileName = documentRepository.GetFileNameByDocumentId(documentId, tenant);
            return fileName;
        }

        private void AddWebHookCommunicationLogQueue(CommunicationLog communicationLog)
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue(communicationLog.QueueName, 0);
            queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", communicationLog.Id }, { "Tenant", tenant.ToString() } }, tenant);
        }
    }

    public class WebHookAutomationServiceArgs
    {
        public WebHookAutomationDetails WebHookDetails { get; set; }
        public string DocumentId { get; set; }
        public int Tenant { get; set; }
        public string EntityId { get; set; }
        public string ObjectTableId { get; set; }
        public string ComputingPartnerId { get; set; }
        public string DocumentFileName { get; set; }
    }

    public class WebHookCommunicationLogSettings
    {
        public string URL { get; set; }
        public string Filename { get; set; }
    }
}