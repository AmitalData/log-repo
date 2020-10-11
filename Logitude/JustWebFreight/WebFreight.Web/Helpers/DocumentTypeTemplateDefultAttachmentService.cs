using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
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
        public List<AttachmentsList> GetDefultAttachmentList(DocumentTypeTemplateDefultAttachmentArgs defultAttachmentArgs)
        {
            this.defultAttachmentArgs = defultAttachmentArgs;
            attachmentsLists = new List<AttachmentsList>();
            DocumentTypeTemplateQuery documentTypeTemplateQuery = new DocumentTypeTemplateQuery(defultAttachmentArgs.Tenant);
            var defultAttachmentList = documentTypeTemplateQuery.GetDefultAttachmentLists(defultAttachmentArgs.DocumentTypeTemplateId, defultAttachmentArgs.Tenant);
            if (defultAttachmentList != null && defultAttachmentList.Count() > 0)
            {
                commonDataContext = CommonDataContext.GetContext(defultAttachmentArgs.Tenant);

                BuildDocOutAttachmentList(defultAttachmentList);
                BuildDocInAttachmentList(defultAttachmentList);

            }
            return attachmentsLists;

        }

        private void BuildDocOutAttachmentList(List<DocumentDefultAttachment> defultAttachmentList)
        {

            List<string> documentTypeIds = defultAttachmentList.Where(d=>d.Type == "DocOut").Select(d => d.DocumentTypeId).ToList();
            if (documentTypeIds.Count() > 0)
            {
                List<string> documentsFilingIds = (from a in commonDataContext.DocumentsFilings
                                                   where a.Tenant == defultAttachmentArgs.Tenant && a.ObjectTableId == defultAttachmentArgs.ObjectTableId && documentTypeIds.Contains(a.DocumentTypeId) && a.DirectionCode == "O" && a.EntityId == defultAttachmentArgs.EntityId
                                                   select a.Id).ToList();
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

                List<AttachmentsList> attachments = (from a in commonDataContext.DocumentsFilings.Include("Document").Include("DocumentType")
                                                   where a.Tenant == defultAttachmentArgs.Tenant && a.ObjectTableId == defultAttachmentArgs.ObjectTableId && documentTypeIds.Contains(a.DocumentTypeId) && a.DirectionCode == "I" && a.EntityId == defultAttachmentArgs.EntityId
                                                     select a).GroupBy(d => d.DocumentTypeId).Select(d => d.FirstOrDefault()).OrderByDescending(d => d.CreateDate)
                                                   .Select(d => new AttachmentsList()
                                                   {
                                                       Id = d.DocumentId,
                                                       DocumentFilingId = d.Id,
                                                       FileExtension = d.Document !=null ? d.Document.Extension:null , 
                                                       FileSize = d.Document != null ? d.Document.FileSize:null, 
                                                       Tenant = d.Document != null ? d.Document.Tenant:d.Tenant,
                                                       DocumentTypeCopyNameWithDocumentTypeName = d.DocumentType != null  ?  d.DocumentType.Name
                                                   }).ToList();



                if (attachments.Count > 0)
                {
                    attachmentsLists =  attachmentsLists.Concat(attachments).ToList();
                }
            }
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


    }

}