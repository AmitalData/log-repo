using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.EntityChanges;
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
    public class OnUpdateDocumentService
    {
        OnUpdateDocumentArgs onUpdateDocumentArgs = null;
        EntityChangeAutomation entityChangesAutomation = null;
        List<EntityChangeAutomation> entityChangesAutomationsLists = null;
        DocumentRepository documentRepository = null;
        DocumentTypeCopyRepository documentTypeCopyRepository = null;
        AutomationOnUpdateDocument automationOnUpdateDocument = null;
        int tenant;
        string entityId;
        string extraDetails = string.Empty;
        bool IsValidDocumentSelectionAutomation = false;
        public OnUpdateDocumentService(OnUpdateDocumentArgs onUpdateDocumentArgs, EntityChangeAutomation entityChangesAutomation, List<EntityChangeAutomation> entityChangesAutomationsLists)
        {
            this.onUpdateDocumentArgs = onUpdateDocumentArgs;
            this.entityChangesAutomation = entityChangesAutomation;
            this.entityChangesAutomationsLists = entityChangesAutomationsLists;
            tenant = onUpdateDocumentArgs.Tenant;
            entityId = onUpdateDocumentArgs.EntityId;
            extraDetails = onUpdateDocumentArgs.ExtraDetails;
            documentRepository = new DocumentRepository(tenant);
            documentTypeCopyRepository = new DocumentTypeCopyRepository(tenant);
            entityChangesAutomation.type = onUpdateDocumentArgs.ValidateResult.IsAutomationValid ? "OnUpdateDocumentSsucceed" : "OnUpdateDocumentFailed";
            automationOnUpdateDocument = onUpdateDocumentArgs.AutomatedBackup.AutomationOnUpdateDocument;
        }

        public void Execute()
        {
            if(!onUpdateDocumentArgs.ValidateResult.IsAutomationValid) HandleInvalidAutomation();
            else HandleValidAutomation();

            FinishExecuting();
        }

        private void HandleValidAutomation()
        {
            if (automationOnUpdateDocument.SendVia == "FTP") ApplyFTPSelection();
            if (IsValidDocumentSelectionAutomation) MarkEntityChangeExecutedRecord(onUpdateDocumentArgs.EntityChange, entityChangesAutomation, entityChangesAutomationsLists);
            else HandleInvalidAutomation();
        }

        private void ApplyFTPSelection()
        {
            OnUpdateDocumentResult onUpdateDocumentResult = LogitudeXmlSerializer.DeserializeObject<OnUpdateDocumentResult>(extraDetails);
            if (!string.IsNullOrEmpty(onUpdateDocumentResult.DocumentId))
            {
                SendUploadedSentDocumentToFTP(onUpdateDocumentResult);
            }
            else if (onUpdateDocumentResult.DocumentTypeCopyIds.Count() > 0)
            {
                StartSendingPrintedDocumentToFTP(onUpdateDocumentResult);
            }
        }

        private void StartSendingPrintedDocumentToFTP(OnUpdateDocumentResult onUpdateDocumentResult)
        {
            automationOnUpdateDocument.DocumentTypeLists.ForEach(documentType => {
                SendPrintedDocumentToFTP(onUpdateDocumentResult, documentType);
            });
        }

        private void SendPrintedDocumentToFTP(OnUpdateDocumentResult onUpdateDocumentResult, OnUpdateDocumentTypeAttachment documentType)
        {
            string documentTypeCopyId = onUpdateDocumentResult.DocumentTypeCopyIds.Where(documentCopyId => documentCopyId == documentType.DocumentTypeCopyId).FirstOrDefault();

            if (string.IsNullOrEmpty(documentTypeCopyId)) return;

            DocumentTypeCopy documentTypeCopy = documentTypeCopyRepository.GetSingleDocumentTypeCopyByTenant(documentTypeCopyId, tenant);
            string documnetFileName = "";
            if (documentTypeCopy != null) documnetFileName = documentTypeCopy.Name;
            ApplyAutomationOnUpdateDocumentFTP(new OnUpdateDocumentFTPArgs { AutomationOnUpdateDocument = automationOnUpdateDocument, DocumentId = documentTypeCopyId, ObjectTableId = onUpdateDocumentArgs.EntityChange.ObjectTableId, DocumentFileName = onUpdateDocumentResult.Type + " " + documnetFileName });
            IsValidDocumentSelectionAutomation = true;
        }

        private void SendUploadedSentDocumentToFTP(OnUpdateDocumentResult onUpdateDocumentResult)
        {
            OnUpdateDocumentTypeAttachment onUpdateDocumentTypeAttachment = automationOnUpdateDocument.DocumentTypeLists.Where(documentType => documentType.DocumentTypeId == onUpdateDocumentResult.DocumentTypeId).FirstOrDefault();
            if (onUpdateDocumentTypeAttachment == null) return;

            string documnetFileName = documentRepository.GetCalculatedFileNameById(onUpdateDocumentResult.DocumentId, tenant);
            ApplyAutomationOnUpdateDocumentFTP(new OnUpdateDocumentFTPArgs { AutomationOnUpdateDocument = automationOnUpdateDocument, DocumentId = onUpdateDocumentResult.DocumentId, ObjectTableId = onUpdateDocumentArgs.EntityChange.ObjectTableId, DocumentFileName = onUpdateDocumentResult.Type + " " + documnetFileName });
            IsValidDocumentSelectionAutomation = true;
        }

        private void HandleInvalidAutomation()
        {
            entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            entityChangesAutomationsLists.Add(entityChangesAutomation);
        }

        private void FinishExecuting()
        {
            entityChangesAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - onUpdateDocumentArgs.DateBefore.Ticks) / TimeSpan.TicksPerMillisecond);
            onUpdateDocumentArgs.EntityChange.OnUpdateDocumentAutomationFailedXml = entityChangesAutomationsLists.Where(d => !d.IsConditionTrue).ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(entityChangesAutomationsLists.Where(d => !d.IsConditionTrue).ToList()) : "";
            onUpdateDocumentArgs.EntityChange.OnUpdateDocumentAutomationSsucceedXml = entityChangesAutomationsLists.Where(d => d.IsConditionTrue).ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(entityChangesAutomationsLists.Where(d => d.IsConditionTrue).ToList()) : "";
        }

        private void ApplyAutomationOnUpdateDocumentFTP(OnUpdateDocumentFTPArgs onUpdateDocumentFTPArgs)
        {
            FTPAutomationServiceArgs fTPAutomationServiceArgs = new FTPAutomationServiceArgs()
            {
                FTPDetails = onUpdateDocumentFTPArgs.AutomationOnUpdateDocument.FTPDetails,
                DocumentId = onUpdateDocumentFTPArgs.DocumentId,
                Tenant = tenant,
                EntityId = entityId,
                ObjectTableId = onUpdateDocumentFTPArgs.ObjectTableId,
                DocumentFileName = onUpdateDocumentFTPArgs.DocumentFileName,
            };
            FTPAutomationService ftpAutomationService = new FTPAutomationService(fTPAutomationServiceArgs);
            ftpAutomationService.Run();
        }

        private static void MarkEntityChangeExecutedRecord(EntityChange entityChange, EntityChangeAutomation entityChangesAutomation, List<EntityChangeAutomation> entityChangesAutomationsLists)
        {
            entityChange.HasExecutedRecord = true;
            entityChangesAutomation.IsConditionTrue = true;
            entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(entityChange.Tenant);
            entityChangesAutomationsLists.Add(entityChangesAutomation);
        }
    }

    public class OnUpdateDocumentArgs
    {
        public EntityChange EntityChange { get; set; }
        public DateTime DateBefore { get; set; }
        public ValidateAutomationResultClass ValidateResult { get; set; }
        public AutomatedBackup AutomatedBackup { get; set; }
        public int Tenant { get; set; }
        public string EntityId { get; set; }
        public string ExtraDetails { get; set; }
    }
    public class OnUpdateDocumentFTPArgs
    {
        public AutomationOnUpdateDocument AutomationOnUpdateDocument { get; set; }
        public string DocumentId { get; set; }
        public string ObjectTableId { get; set; }
        public string DocumentFileName { get; set; }
    }
}