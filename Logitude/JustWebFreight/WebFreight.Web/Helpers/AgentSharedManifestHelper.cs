using Logitude.BL.CommonDataModel.DataContracts;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class AgentSharedManifestHelper
    {
        public List<ManifestSL> GetAgentShareManifestSLByEntityId(string entityId, int tenant)
        {
            List<ManifestSL> manifestSLLists = new List<ManifestSL>();
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(tenant);

            List<CommunicationLog> sharedAgentcommLogLists = communicationLogRepository.GetShareManifestCommunicationLogByEntityIdAndQueueNameAndSubject(entityId, "AgentsSharedLogisticsQueue", "Shared Manifest", "Update Shared Agent");

            foreach (CommunicationLog sharedAgentcommLog in sharedAgentcommLogLists)
            {
                if (sharedAgentcommLog != null)
                {
                    BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                    {
                        FileName = sharedAgentcommLog.Document.Id,
                        FolderName = sharedAgentcommLog.Document.Folder,
                        Extension = sharedAgentcommLog.Document.Extension,
                        Tenant = sharedAgentcommLog.Document.Tenant,
                        FileSize = sharedAgentcommLog.Document.FileSize,
                    };
                    Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                    byte[] date = storageservice.Read(fileInfo);
                    if (date != null)
                    {
                        var manifestSL = LogitudeXmlSerializer.DeserializeObject<ManifestSL>(date);
                        manifestSLLists.Add(manifestSL);
                    }
                }
            }


            return manifestSLLists;
        }

        public void SendEmail(string agentSharedKey, string agentRef ,int tenant)
        {
            string invitedEmail = string.Empty;
            string invitedContactName = string.Empty;
            Contact loggedContact = null;

            AgentSharedLogisticsKeyRepository agentSharedLogisticsKeyRepository = new AgentSharedLogisticsKeyRepository();
            AgentSharedLogisticsKey agentSharedLogisticsKey = agentSharedLogisticsKeyRepository.GetSingleAgentSharedLogisticsKey(agentSharedKey);

            if (agentSharedLogisticsKey == null) return;
            if (string.IsNullOrEmpty(agentSharedLogisticsKey.CreatedByUserEmail) || string.IsNullOrEmpty(agentSharedLogisticsKey.ApprovedByUserEmail)) return;
            int desTenant = agentSharedLogisticsKey.Agent1Tenant == tenant ? agentSharedLogisticsKey.Agent2Tenant : agentSharedLogisticsKey.Agent1Tenant;


            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            ContactRepository contactRepository = new ContactRepository(commonContext);
            loggedContact = contactRepository.GetSingleContactByEmail(agentSharedLogisticsKey.CreatedByUserEmail, tenant);
            if (loggedContact == null) return;

            invitedEmail = agentSharedLogisticsKey.ApprovedByUserEmail;
            Contact invitedContact = contactRepository.GetSingleContactByEmail(invitedEmail, desTenant);
            if (invitedContact == null) return;
            invitedContactName = invitedContact.EnglishName;

            string logo = "logo" + tenant;
            string subject = "Mistake in Sharing "+ agentRef + " Manifest";
            string fromEmail = loggedContact.Email;
            StringBuilder HtmlTemplate = new StringBuilder();
            HtmlTemplate.Append("<p style='text-align:left'>");
            HtmlTemplate.Append("Dear " + invitedContactName);
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("Please notice that the received manifest with reference " + agentRef + " was shared by mistake. Please cancel the shipment you opened from it.");
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("Thank you");

            //HtmlTemplate.Append("<br />");
            //HtmlTemplate.Append("<br />");
            //HtmlTemplate.Append("Best Regards,");
            //HtmlTemplate.Append("<br />");
            //HtmlTemplate.Append(loggedContact.EnglishName);
            //HtmlTemplate.Append("<br /><br />");
            //HtmlTemplate.Append("<img width='290' height='101' src='cid:" + logo + "' />");

            EmailCommunicationParams emailParams = new EmailCommunicationParams()
            {
                From = fromEmail,
                To = invitedEmail,
                Subject = subject,
                EmailBody = HtmlTemplate.ToString(),
                LoggingUserId = loggedContact.Id,
                Tenant = tenant,
            };
            Communications.AddEmailCommunicationLogQueue(emailParams, tenant);
        }

    }
}