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
        private OnUpdateDocumentArgs onUpdateDocumentArgs = null;
        private EntityChangeAutomation entityChangesAutomation = null;
        private List<EntityChangeAutomation> entityChangesAutomationsLists = null;
        private DocumentRepository documentRepository = null;
        private AutomationOnUpdateDocument automationOnUpdateDocumentResult = null;
        private int tenant;
        private string entityId;
        private string extraDetails = string.Empty;
        private bool IsValidDocumentSelectionAutomation = false;
        private List<string> documentsIds = null;
        public OnUpdateDocumentService(OnUpdateDocumentArgs onUpdateDocumentArgs, EntityChangeAutomation entityChangesAutomation, List<EntityChangeAutomation> entityChangesAutomationsLists)
        {
            this.onUpdateDocumentArgs = onUpdateDocumentArgs;
            this.entityChangesAutomation = entityChangesAutomation;
            this.entityChangesAutomationsLists = entityChangesAutomationsLists;
            tenant = onUpdateDocumentArgs.Tenant;
            entityId = onUpdateDocumentArgs.EntityId;
            extraDetails = onUpdateDocumentArgs.ExtraDetails;
            documentRepository = new DocumentRepository(tenant);
            entityChangesAutomation.type = onUpdateDocumentArgs.ValidateResult.IsAutomationValid ? "OnUpdateDocumentSsucceed" : "OnUpdateDocumentFailed";
            automationOnUpdateDocumentResult = onUpdateDocumentArgs.AutomatedBackup.AutomationOnUpdateDocument;
            documentsIds = new List<string>();
        }

        public void Execute()
        {
            if(!onUpdateDocumentArgs.ValidateResult.IsAutomationValid) HandleInvalidAutomation();
            else HandleValidAutomation();

            FinishExecuting();
        }

        private void HandleValidAutomation()
        {
            if (automationOnUpdateDocumentResult.SendVia == "FTP") ApplyFTPAutomationResult();
            if (IsValidDocumentSelectionAutomation) MarkEntityChangeExecutedRecord(onUpdateDocumentArgs.EntityChange, entityChangesAutomation, entityChangesAutomationsLists);
            else HandleInvalidAutomation();
        }

        private void ApplyFTPAutomationResult()
        {
            OnUpdateDocumentDetails onUpdateDocumentDetails = LogitudeXmlSerializer.DeserializeObject<OnUpdateDocumentDetails>(extraDetails);
            
            documentsIds = GetAutomationSelectedDocuments(onUpdateDocumentDetails);
            documentsIds.ForEach(documentId => {
                SendDocumentViaFTP(new OnUpdateDocumentFTPArgs { AutomationOnUpdateDocument = automationOnUpdateDocumentResult, DocumentId = documentId, ObjectTableId = onUpdateDocumentArgs.EntityChange.ObjectTableId });
            });

            IsValidDocumentSelectionAutomation = documentsIds.Count() > 0;
        }

        private List<string> GetAutomationSelectedDocuments(OnUpdateDocumentDetails onUpdateDocumentDetails)
        {
            if (!string.IsNullOrEmpty(onUpdateDocumentDetails.DocumentId) && CheckIfDocumentTypeDefinedInDocumentAutomation(onUpdateDocumentDetails.DocumentTypeId))
            {
                documentsIds.Add(onUpdateDocumentDetails.DocumentId);
            }
            else if (onUpdateDocumentDetails.DocumentTypeCopiesDetails.Count() > 0)
            {
                FillDocumentTypeCopiesDocumentId(onUpdateDocumentDetails);
            }

            return documentsIds;
        }

        private bool CheckIfDocumentTypeDefinedInDocumentAutomation(string documentTypeId)
        {
            return automationOnUpdateDocumentResult.DocumentTypeLists.Where(documentType => documentType.DocumentTypeId == documentTypeId).FirstOrDefault() != null;
        }

        private void FillDocumentTypeCopiesDocumentId(OnUpdateDocumentDetails onUpdateDocumentDetails)
        {
            onUpdateDocumentDetails.DocumentTypeCopiesDetails.ForEach(documentTypeCopy =>
            {
                AddDocumentTypeCopyDocumentId(documentTypeCopy);
            });
        }

        private void AddDocumentTypeCopyDocumentId(DocumentTypeCopiesDetails documentTypeCopiesDetails)
        {
            if (automationOnUpdateDocumentResult.DocumentTypeLists.Where(d => d.DocumentTypeCopyId == documentTypeCopiesDetails.DocumentTypeCopyId).FirstOrDefault() != null)
            {
                documentsIds.Add(documentTypeCopiesDetails.DocumentId);
            }
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

        private void SendDocumentViaFTP(OnUpdateDocumentFTPArgs onUpdateDocumentFTPArgs)
        {
            string documentFileName = documentRepository.GetCalculatedFileNameById(onUpdateDocumentFTPArgs.DocumentId, tenant);
            FTPAutomationServiceArgs fTPAutomationServiceArgs = new FTPAutomationServiceArgs()
            {
                FTPDetails = onUpdateDocumentFTPArgs.AutomationOnUpdateDocument.FTPDetails,
                DocumentId = onUpdateDocumentFTPArgs.DocumentId,
                Tenant = tenant,
                EntityId = entityId,
                ObjectTableId = onUpdateDocumentFTPArgs.ObjectTableId,
                DocumentFileName = documentFileName,
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
    }
}