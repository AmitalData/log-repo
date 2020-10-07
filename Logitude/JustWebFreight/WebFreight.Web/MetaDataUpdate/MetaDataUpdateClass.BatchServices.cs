using Logitude.BL.GlobalModel.EntityPMs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using WebFreight.Web.MetaDataUpdate.AddClasses;

namespace WebFreight.Web.MetaDataUpdate
{
    public partial class MetaDataUpdateClass
    {
        public void FillBatchServicesDefinitions()
        {
            BatchServicesDefinitionRepository batchServicesDefinitionRepository = new BatchServicesDefinitionRepository();

            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "EmailOut-EmailQueue", ClassName = "EmailsWorkerRole", InActive = false, NumberOfThreads = 1, Parameter1 = "E", Parameter2 = "null" }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "CheckandUpdate", ClassName = "CheckandUpdateWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "CommunicationLog", ClassName = "CommunicationLogWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "ExportImportTextCodes", ClassName = "ExportImportTextCodesWR", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "DataBackup", ClassName = "DataBackupWR", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "Leads", ClassName = "LeadsWrokerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "MessageAnalyze", ClassName = "MessageAnalyzeWR", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "TaxWR", ClassName = "TaxWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "AutoSignUp", ClassName = "AutoSignUpWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "AirlineStatistics", ClassName = "AirlineStatisticsWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "GLSHKMessageAnalyze", ClassName = "GLSHKMessageAnalyzeWR", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "EmailOut-champmessageoutqueue", ClassName = "EmailsWorkerRole", InActive = false, NumberOfThreads = 1, Parameter1 = "T", Parameter2 = "CHAMP" }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "EmailOut-glshkmessageoutqueue", ClassName = "EmailsWorkerRole", InActive = false, NumberOfThreads = 1, Parameter1 = "T", Parameter2 = "GLSHK" }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "LogitudeMessagesTransmissionLog", ClassName = "MessagesTransmissionLogWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "CustomerTenantAccessRequest", ClassName = "CustomerTenantAccessRequestWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "CustomerTenantAccess", ClassName = "CustomerTenantAccessWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "ImporterShipments", ClassName = "ImporterShipmentsWorkerRole", InActive = false, NumberOfThreads = 1, Parameter1 = "0" }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "ImporterShipmentDocuments", ClassName = "ImporterShipmentDocumentsWorkerRole", InActive = false, NumberOfThreads = 1, Parameter1 = "0" }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "ForwarderShipmentDocuments", ClassName = "ForwarderShipmentDocumentsWorkerRole", InActive = false, NumberOfThreads = 1, Parameter1 = "0" }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "ImporterShipmentsBatchWR", ClassName = "ImporterShipmentsBatchWorkerRole", InActive = false, NumberOfThreads = 1, Parameter1 = "0" }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "ImporterShipmentsQueueBuilderWR", ClassName = "ImporterShipmentsQueueBuilderWorkerRole", InActive = false, NumberOfThreads = 1, Parameter1 = "0" }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "ImporterShipmentDocumentsBatch", ClassName = "ImporterShipmentDocumentsBatchWorkerRole", InActive = false, NumberOfThreads = 1, Parameter1 = "0" }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "ImporterDocumentsQueueBuilderWR", ClassName = "ImporterShipmentsDocumentsQueueBuilderWorkerRole", InActive = false, NumberOfThreads = 1, Parameter1 = "0" }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "TenantStatistics", ClassName = "TenantStatisticsWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "TenantStatisticsMessage", ClassName = "TenantStatisticsMessageWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "Social", ClassName = "SocialWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "SignUp", ClassName = "SignUpWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "GeneralWR", ClassName = "GeneralWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "AutoSignUpMail", ClassName = "AutoSignUpMailWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "CustomerActualData", ClassName = "CustomerActualDataWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "QuoteAutomaticallyClosing", ClassName = "QuoteAutomaticallyClosing", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "ContactUnseenEntity", ClassName = "ContactUnseenEntityWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "InboundEmail", ClassName = "InboundEmailWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "TicketWR", ClassName = "TicketWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "MobileNotification", ClassName = "MobileNotificationWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "EntityChange", ClassName = "EntityChangeWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "WarmSystem", ClassName = "WarmWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "GLSHKMessageIn", ClassName = "GLSHKMessageInWR", InActive = false, NumberOfThreads = 1, Parameter1 = "amazon" }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "ChampMessageIn", ClassName = "ChampMessageInWR", InActive = false, NumberOfThreads = 1, Parameter1 = "amazon" }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "ChampMessageToAnalyzeQueue", ClassName = "AddChampMessageToAnalyzeQueueWR", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "GLSHKMessageInAmital", ClassName = "GLSHKMessageInWR", InActive = false, NumberOfThreads = 1,Parameter1="amital" }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "ChampMessageInAmital", ClassName = "ChampMessageInWR", InActive = false, NumberOfThreads = 1, Parameter1 = "amital" }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "MailgunPageAnalyzer", ClassName = "MailgunPageAnalyzerWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "QuickbooksOnline", ClassName = "QuckbooksOnlineWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);

            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "DelayAutomationWR", ClassName = "DelayAutomationWorkerRole", InActive = false, NumberOfThreads = 1, Parameter1 = "0" }, batchServicesDefinitionRepository);

            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "AgentsSharedLogistics", ClassName = "AgentsSharedLogisticsWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "EntityExternalUpdateWR", ClassName = "EntityExternalUpdateWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "SendToCustoms", ClassName = "SendToCustomsWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);


            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "SendToCustoms", ClassName = "SendToCustomsWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);



            batchServicesDefinitionRepository.SubmitChanges();
        }


        public void FillCustomsBatchServicesDefinitions()
        {
            BatchServicesDefinitionRepository batchServicesDefinitionRepository = new BatchServicesDefinitionRepository();

            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM()
            { Code = "SendDataToExternal", ClassName = "SendDataToExternalServicesWR", InActive = false, NumberOfThreads = 1, Parameter1 = "E", Parameter2 = "null" }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM()
            { Code = "CustomsMessagingSheet", ClassName = "CustomsMessagingSheetWR", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM()
            { Code = "DownloadDca", ClassName = "DownloadDcaMessageSheetWR", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM()
            { Code = "GetCustomRequest", ClassName = "CustomsCommandGetCustomRequestWR", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM()
            { Code = "CustomsCommandSendDCAWR", ClassName = "CustomsCommandSendDCAWR", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "Leads", ClassName = "LeadsWrokerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "MessageAnalyze", ClassName = "MessageAnalyzeWR", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "TaxWR", ClassName = "TaxWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "AutoSignUp", ClassName = "AutoSignUpWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "AirlineStatistics", ClassName = "AirlineStatisticsWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "GLSHKMessageAnalyze", ClassName = "GLSHKMessageAnalyzeWR", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "EmailOut-champmessageoutqueue", ClassName = "EmailsWorkerRole", InActive = false, NumberOfThreads = 1, Parameter1 = "T", Parameter2 = "CHAMP" }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "EmailOut-glshkmessageoutqueue", ClassName = "EmailsWorkerRole", InActive = false, NumberOfThreads = 1, Parameter1 = "T", Parameter2 = "GLSHK" }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "LogitudeMessagesTransmissionLog", ClassName = "MessagesTransmissionLogWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "CustomerTenantAccessRequest", ClassName = "CustomerTenantAccessRequestWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "CustomerTenantAccess", ClassName = "CustomerTenantAccessWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "ImporterShipments", ClassName = "ImporterShipmentsWorkerRole", InActive = false, NumberOfThreads = 1, Parameter1 = "0" }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "ImporterShipmentDocuments", ClassName = "ImporterShipmentDocumentsWorkerRole", InActive = false, NumberOfThreads = 1, Parameter1 = "0" }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "ForwarderShipmentDocuments", ClassName = "ForwarderShipmentDocumentsWorkerRole", InActive = false, NumberOfThreads = 1, Parameter1 = "0" }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "ImporterShipmentsBatchWR", ClassName = "ImporterShipmentsBatchWorkerRole", InActive = false, NumberOfThreads = 1, Parameter1 = "0" }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "ImporterShipmentsQueueBuilderWR", ClassName = "ImporterShipmentsQueueBuilderWorkerRole", InActive = false, NumberOfThreads = 1, Parameter1 = "0" }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "ImporterShipmentDocumentsBatch", ClassName = "ImporterShipmentDocumentsBatchWorkerRole", InActive = false, NumberOfThreads = 1, Parameter1 = "0" }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "ImporterDocumentsQueueBuilderWR", ClassName = "ImporterShipmentsDocumentsQueueBuilderWorkerRole", InActive = false, NumberOfThreads = 1, Parameter1 = "0" }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "TenantStatistics", ClassName = "TenantStatisticsWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "TenantStatisticsMessage", ClassName = "TenantStatisticsMessageWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "Social", ClassName = "SocialWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "SignUp", ClassName = "SignUpWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "GeneralWR", ClassName = "GeneralWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "AutoSignUpMail", ClassName = "AutoSignUpMailWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "CustomerActualData", ClassName = "CustomerActualDataWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "QuoteAutomaticallyClosing", ClassName = "QuoteAutomaticallyClosing", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "ContactUnseenEntity", ClassName = "ContactUnseenEntityWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "InboundEmail", ClassName = "InboundEmailWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "TicketWR", ClassName = "TicketWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "MobileNotification", ClassName = "MobileNotificationWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "EntityChange", ClassName = "EntityChangeWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "WarmSystem", ClassName = "WarmWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "GLSHKMessageIn", ClassName = "GLSHKMessageInWR", InActive = false, NumberOfThreads = 1, Parameter1 = "amazon" }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "ChampMessageIn", ClassName = "ChampMessageInWR", InActive = false, NumberOfThreads = 1, Parameter1 = "amazon" }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "ChampMessageToAnalyzeQueue", ClassName = "AddChampMessageToAnalyzeQueueWR", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "GLSHKMessageInAmital", ClassName = "GLSHKMessageInWR", InActive = false, NumberOfThreads = 1, Parameter1 = "amital" }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "ChampMessageInAmital", ClassName = "ChampMessageInWR", InActive = false, NumberOfThreads = 1, Parameter1 = "amital" }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "MailgunPageAnalyzer", ClassName = "MailgunPageAnalyzerWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "QuickbooksOnline", ClassName = "QuckbooksOnlineWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);

            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "DelayAutomationWR", ClassName = "DelayAutomationWorkerRole", InActive = false, NumberOfThreads = 1, Parameter1 = "0" }, batchServicesDefinitionRepository);

            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "AgentsSharedLogistics", ClassName = "AgentsSharedLogisticsWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "EntityExternalUpdateWR", ClassName = "EntityExternalUpdateWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "SendToCustoms", ClassName = "SendToCustomsWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);


           
            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "AccountingJournalApproveWR", ClassName = "AccountingJournalApproveWR", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);

            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "AccountingConversionJournalApproveWR", ClassName = "AccountingConversionJournalApproveWR", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);//ExternalSystem


            AddBatchServicesDefinitions.AddBatchServicesDefinition(new BatchServicesDefinitionPM() { Code = "RevaluationWorkerRole", ClassName = "RevaluationWorkerRole", InActive = false, NumberOfThreads = 1 }, batchServicesDefinitionRepository);


            batchServicesDefinitionRepository.SubmitChanges();
        }
    }
}