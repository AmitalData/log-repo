using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class DocumentTypeTemplateDefultAttachmentService
    {

        private ICommonDataContext commonDataContext;
        private List<AttachmentsList> attachmentsLists;
        private DocumentTypeTemplateDefultAttachmentArgs defultAttachmentArgs;
        string shipmentObjectId = null;
        public List<DocumentDefultAttachment> EmptyDefaultDocuments = new List<DocumentDefultAttachment>();
        public List<AttachmentsList> GetDefultAttachmentList(DocumentTypeTemplateDefultAttachmentArgs defultAttachmentArgs)
        {
            this.defultAttachmentArgs = defultAttachmentArgs;
            shipmentObjectId = GetMasterShipmentObjectTableId(defultAttachmentArgs);
            attachmentsLists = new List<AttachmentsList>();
            BuildDefaultAttachemnts(defultAttachmentArgs);
            BuildDefaultExternalAttachment(defultAttachmentArgs);
            return attachmentsLists;

        }

        private void BuildDefaultExternalAttachment(DocumentTypeTemplateDefultAttachmentArgs defultAttachmentArgs)
        {
            List<string> attachedExternalDocumentsIds = GetAttachedExternalDocumentsIds();
            if (attachedExternalDocumentsIds == null || attachedExternalDocumentsIds.Count() == 0)
            {
                return;
            }
            List<Document> documents = GetDocumentsByIds(attachedExternalDocumentsIds);
            AddDocumentsToAttachmentLists(documents);
        }

     

        private List<string> GetAttachedExternalDocumentsIds()
        {
            DocumentTypeTemplateQuery documentTypeTemplateQuery = new DocumentTypeTemplateQuery(defultAttachmentArgs.Tenant);
           return documentTypeTemplateQuery.GetDefaultExternalAttachmentIds(defultAttachmentArgs.DocumentTypeTemplateId, defultAttachmentArgs.Tenant);

        }

        private List<Document> GetDocumentsByIds(List<string> documentsIds)
        {
            DocumentRepository documentRepository = new DocumentRepository(defultAttachmentArgs.Tenant);
            return documentRepository.GetDocumentsByIds(documentsIds);
        }


        private void AddDocumentsToAttachmentLists(List<Document> documents)
        {
            foreach (Document document in documents)
            {
                attachmentsLists.Add(GetNewInstanceFromAttachemntList(document));
            }
     
        }

        private AttachmentsList GetNewInstanceFromAttachemntList(Document item)
        {
          return  new AttachmentsList() { 
                Id = item.Id,
                DocumentTypeCopyNameWithDocumentTypeName = item.FileName, 
                FileSize = item.FileSize,
                FileExtension = item.Extension,
                Tenant = item.Tenant 
            };
        }

        private void BuildDefaultAttachemnts(DocumentTypeTemplateDefultAttachmentArgs defultAttachmentArgs)
        {
            DocumentTypeTemplateQuery documentTypeTemplateQuery = new DocumentTypeTemplateQuery(defultAttachmentArgs.Tenant);
            var defultAttachmentList = documentTypeTemplateQuery.GetDefultAttachmentLists(defultAttachmentArgs.DocumentTypeTemplateId, defultAttachmentArgs.Tenant);
            if (defultAttachmentList != null && defultAttachmentList.Count() > 0)
            {
                commonDataContext = CommonDataContext.GetContext(defultAttachmentArgs.Tenant);

                BuildDocOutAttachmentList(defultAttachmentList);
                BuildDocInAttachmentList(defultAttachmentList);

            }

            if (defultAttachmentList != null)
            {
                this.EmptyDefaultDocuments = defultAttachmentList.Where(d => !d.IsExist).ToList();
            }
        }

        private string GetMasterShipmentObjectTableId(DocumentTypeTemplateDefultAttachmentArgs defultAttachmentArgs)
        {
            string result = string.Empty;
            ObjectTableRepository objectTableRep = new ObjectTableRepository(defultAttachmentArgs.Tenant);
            if (objectTableRep.IsObjectTableMaster(defultAttachmentArgs.ObjectTableId))
            {
                var shipmentObject = ObjectTableQuery.GetObjectTableByCode("Shipment", defultAttachmentArgs.Tenant);
                result = shipmentObject.Id;
            }
            return result;
        }

        private void BuildDocOutAttachmentList(List<DocumentDefultAttachment> defultAttachmentList)
        {

            List<string> documentTypeIds = defultAttachmentList.Where(d=>d.Type == "DocOut").Select(d => d.DocumentTypeId).ToList();
            if (documentTypeIds.Count() > 0)
            {
                List<string> documentsFilingIds = GetDocumentFilinfIds(documentTypeIds);

                if (documentsFilingIds.Count > 0)
                {
                    List<string> documentTypeCopyIds = defultAttachmentList.Where(d => d.Type == "DocOut").Select(d => d.DocumentTypeCopyId).ToList();
                    DocumentOutCopyQuery documentOutCopyQuery = new DocumentOutCopyQuery(defultAttachmentArgs.Tenant);
                    var documentOutCopys = documentOutCopyQuery.GeDocumentOutCopiesPMListsBydocumentTypeCopyIdsAndDocumentOutIds(documentTypeCopyIds, documentsFilingIds, defultAttachmentArgs.Tenant);
                    foreach (DocumentOutCopyPM copy in documentOutCopys)
                    {
                        MarkDocsOutAttachmentAsExist(defultAttachmentList, copy);

                        attachmentsLists.Add(new AttachmentsList() { Id = copy.DocumentId, DocumentFilingId = copy.DocumentOutId, DocumentTypeCopyNameWithDocumentTypeName = copy.DocumentTypeCopyNameWithDocumentTypeName, FileSize = copy.FileSize, FileExtension = copy.FileExtension, Tenant = copy.Tenant });
                    }
                }
            }

        }

        private static void MarkDocsOutAttachmentAsExist(List<DocumentDefultAttachment> defultAttachmentList, DocumentOutCopyPM copy)
        {
            var documentDefultAttachment = defultAttachmentList.Where(d => d.DocumentTypeId == copy.DocumentTypeId && d.DocumentTypeCopyId == copy.DocumentTypeCopyId).FirstOrDefault();
            if(documentDefultAttachment != null)
            {
                documentDefultAttachment.IsExist = true;
            }
        }

        private List<string> GetDocumentFilinfIds(List<string> documentTypeIds)
        {

            IQueryable<DocumentsFiling> documentsFilings = (from a in commonDataContext.DocumentsFilings
                                                            where a.Tenant == defultAttachmentArgs.Tenant &&
                                                            documentTypeIds.Contains(a.DocumentTypeId) && a.DirectionCode == "O"
                                                            select a);

            if (!defultAttachmentArgs.IsAutomation || !IsNotChildObjectTable())
            {
                documentsFilings = documentsFilings.Where(a => a.EntityId == defultAttachmentArgs.EntityId && a.ObjectTableId == defultAttachmentArgs.ObjectTableId);
            }
            else
            {
                documentsFilings = documentsFilings.Where(a => a.ChildEntityId == defultAttachmentArgs.EntityId && (a.ObjectTable.Name == "Master" || a.ObjectTable.Name == "Shipment"));
            }

            if (!string.IsNullOrEmpty(defultAttachmentArgs.ChildEntityId))
            {
                documentsFilings = documentsFilings.Where(d => d.ChildEntityId == defultAttachmentArgs.ChildEntityId);
            }
            return documentsFilings.Select(d => d.Id).ToList();
        }

        private bool IsNotChildObjectTable()
        {
            return defultAttachmentArgs.ObjectTableName == "ARInvoice" || defultAttachmentArgs.ObjectTableName == "ARPayment" || defultAttachmentArgs.ObjectTableName == "APInvoice" || defultAttachmentArgs.ObjectTableName == "APPayment";
        }

        private void BuildDocInAttachmentList (List<DocumentDefultAttachment> defultAttachmentList)
        {
            List<string> documentTypeIds = defultAttachmentList.Where(d => d.Type == "DocIn").Select(d => d.DocumentTypeId).ToList();
            if (documentTypeIds.Count() > 0)
            {
                IQueryable<DocumentsFiling> documentsFilings = GetDocumentsFiligns(documentTypeIds);

                List<AttachmentsList> attachments = new List<DocumentsFiling>(documentsFilings).Select(d => GetAttachment(d)).ToList();

                if (attachments.Count > 0)
                {
                    MarkDocsInAttachmentAsExist(defultAttachmentList, attachments);
                    attachmentsLists = attachmentsLists.Concat(attachments).ToList();
                }
            }
        }

        private static void MarkDocsInAttachmentAsExist(List<DocumentDefultAttachment> defultAttachments, List<AttachmentsList> attachments)
        {
            foreach (DocumentDefultAttachment defultAttachment in defultAttachments.Where(d => d.Type == "DocIn"))
            {
                defultAttachment.IsExist = attachments.Where(d => d.DocumentTypeId == defultAttachment.DocumentTypeId).Any();
            }
        }

        private IQueryable<DocumentsFiling> GetDocumentsFiligns(List<string> documentTypeIds)
        {
            IQueryable<DocumentsFiling> documentsFilings = (from a in commonDataContext.DocumentsFilings.Include("Document").Include("DocumentType")
                                                            where a.Tenant == defultAttachmentArgs.Tenant 
                                                             && documentTypeIds.Contains(a.DocumentTypeId) && a.DirectionCode == "I" 
                                                            && a.IsDeleted == false && (a.Document != null && a.Document.HasFile)
                                                            select a);

            if (!defultAttachmentArgs.IsAutomation || !IsNotChildObjectTable())
            {
                documentsFilings = documentsFilings.Where(a => a.EntityId == defultAttachmentArgs.EntityId && a.ObjectTableId == defultAttachmentArgs.ObjectTableId);
            }
            else
            {
                documentsFilings = documentsFilings.Where(a => a.ChildEntityId == defultAttachmentArgs.EntityId && (a.ObjectTable.Name == "Master" || a.ObjectTable.Name == "Shipment"));
            }

            if (!string.IsNullOrEmpty(defultAttachmentArgs.ChildEntityId))
            {
                documentsFilings = documentsFilings.Where(d => d.ChildEntityId == defultAttachmentArgs.ChildEntityId);
            }

            return documentsFilings;
        }

 
        private static AttachmentsList GetAttachment(DocumentsFiling d)
        {
            return new AttachmentsList()
            {

                Id = d.DocumentId,
                DocumentFilingId = d.Id,
                FileExtension = d.Document != null ? d.Document.Extension : null,
                FileSize = d.Document != null ? d.Document.FileSize : null,
                Tenant = d.Document != null ? d.Document.Tenant : d.Tenant,
                DocumentTypeCopyNameWithDocumentTypeName = d.DocumentType != null ? d.DocumentType.Name : "",
                DocumentTypeId = d.DocumentType != null ? d.DocumentType.Id: "",
            };
        }
    }

    public class AttachmentsList {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public double? FileSize { get; set; }
        public string DocumentFilingId { get; set; }
        public string FileExtension { get; set; }
        public string DocumentTypeCopyNameWithDocumentTypeName { get; set; }
        public string DocumentTypeId { get; set; }
    }

    public class DocumentTypeTemplateDefultAttachmentArgs
    {
        public string DocumentTypeTemplateId { get; set; }
        public string EntityId { get; set; }
        public string ObjectTableId { get; set; }
        public int Tenant { get; set; }
        public string ChildEntityId { get; set; }
        public bool IsAutomation { get; set; }
        public string ObjectTableName { get; set; }
    }

}