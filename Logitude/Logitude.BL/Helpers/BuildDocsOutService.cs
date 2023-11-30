using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.Helpers
{
    public class BuildDocsOutService
    {

        public void BuildDocsOut(BuildDocsOutArgs buildDocsOutArgs)
        {
            DocumentOutPM documentOutPM = GetDocumentOutPM(buildDocsOutArgs);
            if (documentOutPM == null) documentOutPM = CreateDcoumentOutPM(buildDocsOutArgs);

            DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(buildDocsOutArgs.Tenant);
            var documentTypePM = documentTypeQuery.GetSinglePM(buildDocsOutArgs.DocumentTypeId, documentOutPM.Id, buildDocsOutArgs.Tenant);
            if (documentTypePM != null)
            {
                ExportDocumentArgs exportDocumentArgs = GetExportDocumentArgs(buildDocsOutArgs, documentOutPM, documentTypePM);
                SendQueueService(exportDocumentArgs, buildDocsOutArgs);
            }
        }

        private ExportDocumentArgs GetExportDocumentArgs(BuildDocsOutArgs buildDocsOutArgs, DocumentOutPM documentOutPM, DocumentTypePM documentTypePM)
        {
            var documentTypeCopyIdsList = documentTypePM.DocumentTypeCopies != null ? documentTypePM.DocumentTypeCopies.Select(d => d.Id).ToList() : new List<string>();
            return new ExportDocumentArgs()
            {
                DocumentTypeId = documentTypePM.Id,
                EntityId = buildDocsOutArgs.EntityId,
                ObjectTableId = buildDocsOutArgs.ObjectTableId,
                ChildEntityId = buildDocsOutArgs.ChildEntityId,
                ChildObjectTableId = buildDocsOutArgs.ChildObjectTableId,
                CurrentDocumentOutId = documentOutPM.Id,
                LoggedContactId = buildDocsOutArgs.LoggedUserId,
                Tenant = buildDocsOutArgs.Tenant,
                DocumentTypeName = documentTypePM.Name,
                DocumentTypeTemplateId = documentOutPM.DocumentTemplateId,
                CurrentDocumentTypeCode = documentTypePM.Code,
                ObjectTableName = documentTypePM.ObjectTableName,
                DocumentTemplateEditorTool = documentOutPM.DocumentTemplateEditorTool,
                DocumentTypeCopyIdsList = documentTypeCopyIdsList,
            };
        }

        private DocumentOutPM GetDocumentOutPM(BuildDocsOutArgs args)
        {

            DocumentOutQuery documentOutQuery = new DocumentOutQuery(args.Tenant);
            DocumentOutPM documentOutPM = documentOutQuery.GetDocumentOutByDocumentTypeEntityAndChild(args.EntityId, args.ChildEntityId, args.DocumentTypeId, args.Tenant);
            return documentOutPM;
        }

        private DocumentOutPM CreateDcoumentOutPM(BuildDocsOutArgs args)
        {
            DocumentHelper documentHelper = new DocumentHelper();
            return documentHelper.CreateDocumentOut(args.DocumentTypeId, args.EntityId, args.ChildEntityId, args.ChildEntityReference, args.ObjectTableId, args.Tenant, args.LoggedUserId, args.DocumentTypeTemplateId);

        }

        private void SendQueueService(ExportDocumentArgs args, BuildDocsOutArgs buildDocsOutArgs)
        {
            string callBackDetailsXml = !string.IsNullOrEmpty(buildDocsOutArgs.CallBackDetailsXml )? buildDocsOutArgs.CallBackDetailsXml: "";
            DocumentsExecutionLog documentsExecutionLog = GetNewInStanceFromDocumentsExecutionLog(args);
            IQueueService queueservice = new DbQueueService();
            string documentsExecutionQueueCode = FeatureToggleHelper.HasFeatureToggle("DE2", documentsExecutionLog.Tenant) ? "DocumentsExecutionV2Queue" : "DocumentsExecutionQueue";
            queueservice.InitializeQueue(documentsExecutionQueueCode, documentsExecutionLog.Tenant);
            Dictionary<string, string> queueMessage = new Dictionary<string, string>() {
                { "DocumentsExecutionLogId", documentsExecutionLog.Id },
                { "Tenant", documentsExecutionLog.Tenant.ToString() }
            };
            if (!string.IsNullOrEmpty(callBackDetailsXml))
            {
                queueMessage.Add("CallBackDetailsXml", callBackDetailsXml);
            }
            queueservice.Send(queueMessage, documentsExecutionLog.Tenant, null, null, null, null);
        }


        public DocumentsExecutionLog GetNewInStanceFromDocumentsExecutionLog(ExportDocumentArgs exportDocumentArgs)
        {
            DocumentsExecutionLogRepository documentsExecutionLogRepository = new DocumentsExecutionLogRepository(exportDocumentArgs.Tenant);
            DocumentsExecutionLog documentsExecutionLog = new DocumentsExecutionLog()
            {
                Id = IdCounter.GetNumber("DocumentsExecutionLog", exportDocumentArgs.Tenant).ToString(),
                Tenant = exportDocumentArgs.Tenant,
                CreateDate = DateTime.Now,
                CreatedByUserId = exportDocumentArgs.LoggedContactId,
                RequestXML = LogitudeXmlSerializer.SerializeObjectToXmlString(exportDocumentArgs),
                StatusCode = "W",
                DocumentTypeId = exportDocumentArgs.DocumentTypeId,
                DocumentTypeTemplateId = exportDocumentArgs.DocumentTypeTemplateId,
                Subject = exportDocumentArgs.DocumentTypeName,
            };
            documentsExecutionLogRepository.Add(documentsExecutionLog);
            documentsExecutionLogRepository.SubmitChanges();

            return documentsExecutionLog;
        }
    }

   
    public class BuildDocsOutArgs
    {
        public string DocumentTypeTemplateId { get; set; }

        public string DocumentTypeId { get; set; }
        public string EntityId { get; set; }
        public string ObjectTableId { get; set; }
        public string ChildEntityId { get; set; }
        public string ChildEntityReference { get; set; }
        public int Tenant { get; set; }
        public string LoggedUserId { get; set; }
        public string ChildObjectTableId { get; set; }
        public string CallBackDetailsXml { get; set; }



    }

}