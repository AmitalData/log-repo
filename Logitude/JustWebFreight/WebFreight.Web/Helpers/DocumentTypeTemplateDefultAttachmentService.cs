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
                IQueryable<DocumentsFiling> documentsFilings = (from a in commonDataContext.DocumentsFilings
                                                         where a.Tenant == defultAttachmentArgs.Tenant && a.ObjectTableId == defultAttachmentArgs.ObjectTableId && documentTypeIds.Contains(a.DocumentTypeId) && a.DirectionCode == "O" && a.EntityId == defultAttachmentArgs.EntityId
                                                         select a);

                if (!string.IsNullOrEmpty(defultAttachmentArgs.ChildEntityId))
                {
                    documentsFilings = documentsFilings.Where(d => d.ChildEntityId == defultAttachmentArgs.ChildEntityId);
                }


                List<string> documentsFilingIds = documentsFilings.Select(d => d.Id).ToList();

                if (documentsFilingIds.Count > 0)
                {
                    List<string> documentTypeCopyIds = defultAttachmentList.Where(d => d.Type == "DocOut").Select(d => d.DocumentTypeCopyId).ToList();
                    DocumentOutCopyQuery documentOutCopyQuery = new DocumentOutCopyQuery(defultAttachmentArgs.Tenant);
                    var documentOutCopys = documentOutCopyQuery.GeDocumentOutCopiesPMListsBydocumentTypeCopyIdsAndDocumentOutIds(documentTypeCopyIds, documentsFilingIds, defultAttachmentArgs.Tenant);
                    foreach (DocumentOutCopyPM copy in documentOutCopys)
                    {
                        attachmentsLists.Add(new AttachmentsList() { Id = copy.DocumentId, DocumentFilingId = copy.DocumentOutId, DocumentTypeCopyNameWithDocumentTypeName = copy.DocumentTypeCopyNameWithDocumentTypeName, FileSize = copy.FileSize, FileExtension = copy.FileExtension , Tenant = copy.Tenant }) ;
                    }
                }
            }

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
                    attachmentsLists = attachmentsLists.Concat(attachments).ToList();
                }
            }
        }

        private IQueryable<DocumentsFiling> GetDocumentsFiligns(List<string> documentTypeIds)
        {
            IQueryable<DocumentsFiling> documentsFilings = (from a in commonDataContext.DocumentsFilings.Include("Document").Include("DocumentType")
                                                            where a.Tenant == defultAttachmentArgs.Tenant && (a.ObjectTableId == defultAttachmentArgs.ObjectTableId || a.ObjectTableId == shipmentObjectId) && documentTypeIds.Contains(a.DocumentTypeId) && a.DirectionCode == "I" && a.EntityId == defultAttachmentArgs.EntityId && a.IsDeleted == false && (a.Document != null && a.Document.HasFile)
                                                            select a);
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


    }

    public class DocumentTypeTemplateDefultAttachmentArgs
    {
        public string DocumentTypeTemplateId { get; set; }
        public string EntityId { get; set; }
        public string ObjectTableId { get; set; }
        public int Tenant { get; set; }
        public string ChildEntityId { get; set; }

        
    }

}