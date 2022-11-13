using Logitude.BL.Helpers;
using Logitude.Server.Tools;
using Logitude.Server.Tools.EntityChanges.AutomationResult;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.AutomationModel;
using WebFreight.Web.Helpers.CallBack;

namespace CommunicationWorkerRole.Services
{
    class AutomationDocumentOutBuildService
    {
        public AutomationDocumentOutBuildService()
        {

        }

        public bool HaveDocumentOutNeedBuild(AutomationSendEmailArgs automationSendEmailArgs)
        {
            if (string.IsNullOrEmpty(automationSendEmailArgs.ReportTemplateId)) return false;
            string documentOutId = GetDocumentOutId(automationSendEmailArgs);
            return string.IsNullOrEmpty(documentOutId) && (automationSendEmailArgs.ObjectTableName == "Shipment");
        }

        public string GetDocumentOutId(AutomationSendEmailArgs automationSendEmailArgs)
        {
            ReportTemplateDocOutArgs reportTemplateDocOutArgs = new ReportTemplateDocOutArgs()
            {
                Tenant = automationSendEmailArgs.Tenant,
                ReportTemplateId = automationSendEmailArgs.ReportTemplateId,
                DocumentCopyId = automationSendEmailArgs.DocumentCopyId,
                DocumentTypeId = automationSendEmailArgs.Automation.DocumentTypeId,
                EntityId = automationSendEmailArgs.EntityId,
                ObjectTableId = automationSendEmailArgs.ObjectTableId,
            };

            return new ReportTemplateDocOutService(reportTemplateDocOutArgs).GetDocOutDocumentId();
        }
        
        public void BuildDocumentWithCallBack(AutomationSendEmailArgs automationSendEmailArgs)
        {
            string callBackDetailsXml = GetCallBackDetailsXml(automationSendEmailArgs);
            BuildDocsOutArgs buildDocsOutArgs = GetBuildDocsOutArgs(automationSendEmailArgs, callBackDetailsXml);
            new BuildDocsOutService().BuildDocsOut(buildDocsOutArgs);
        }

        private BuildDocsOutArgs GetBuildDocsOutArgs(AutomationSendEmailArgs automationSendEmailArgs, string callBackDetailsXml)
        {
            return new BuildDocsOutArgs()
            {
                EntityId = automationSendEmailArgs.EntityId,
                Tenant = automationSendEmailArgs.Tenant,               
                LoggedUserId = automationSendEmailArgs.CreateByUserId, //CHECK
                ObjectTableId = automationSendEmailArgs.ObjectTableId,
                DocumentTypeId = automationSendEmailArgs.Automation.DocumentTypeId,
                CallBackDetailsXml = callBackDetailsXml,
                DocumentTypeTemplateId = automationSendEmailArgs.ReportTemplateId,
                ChildEntityId = null,
                ChildEntityReference = null,
                ChildObjectTableId = null,
            };
        }

        private string GetCallBackDetailsXml(AutomationSendEmailArgs automationSendEmailArgs)
        {
            AutomationQueueArgs automationQueueArgs = GetAutomationQueueArgs(automationSendEmailArgs);
            CallBackDetails callBackDetails = new CallBackDetails
            {
                HandlerServiceName = "AutomationDocumentOutHandlerService",
                HandlerArgs = LogitudeXmlSerializer.SerializeObjectToXmlElementString(automationQueueArgs)
            };
            return LogitudeXmlSerializer.SerializeObjectToXmlElementString(callBackDetails); 
        }

        private AutomationQueueArgs GetAutomationQueueArgs(AutomationSendEmailArgs automationSendEmailArgs)
        {
            return new AutomationQueueArgs()
            {
                EntityChangeId = automationSendEmailArgs.EntityChangeId,
                AutomationId = automationSendEmailArgs.Automation.Id,
                AutomationType = automationSendEmailArgs.Automation.Type,
                EntityId = automationSendEmailArgs.EntityId,
                Tenant = automationSendEmailArgs.Tenant,
                AutomationDelayTime = null,
                EntityReference = automationSendEmailArgs.EntityReference
            };
        }
    }
}
