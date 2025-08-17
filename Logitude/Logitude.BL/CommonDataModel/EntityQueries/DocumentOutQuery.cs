using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class DocumentOutQuery
    {
        DocumentOutRepository repository;



        public DocumentOutQuery(int tenant)
        {
            repository = new DocumentOutRepository(tenant);
        }

        public DocumentOutQuery(DocumentOutRepository addressRepository)
        {
            repository = addressRepository;
        }

        public DocumentOutPM GetSinglePM(string id, int tenant)
        {
            DocumentOutCopyRepository documentOutCopyRep = new DocumentOutCopyRepository(repository.context);
            DocumentOutCopyQuery documentOutCopyQuery = new DocumentOutCopyQuery(documentOutCopyRep);

            DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(repository.context);
            DocumentsFiling documentFiling = documentsFilingRepository.GetSingleDocumentsFiling(id, tenant);

            DocumentOut doc = (from a in repository.context.DocumentOuts
                               where a.Id == id && a.Tenant == tenant
                               select a).FirstOrDefault();

            User user = (from a in repository.context.Users.Include("Contact")
                         where a.Id == doc.IssuedByUserId
                         select a).FirstOrDefault();

            DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(tenant);
            DocumentType docType = documentTypeRepository.GetSingleDocumentTypes(documentFiling.DocumentTypeId, doc.Tenant);
            string editorTool = null;
            if (!string.IsNullOrEmpty(doc.DocumentTemplateId))
            {
                DocumentTypeTemplate documentTypeTemplate = repository.context.DocumentTypeTemplates.Where(t => t.Id == doc.DocumentTemplateId).FirstOrDefault();
                if (documentTypeTemplate != null)
                {
                    editorTool = documentTypeTemplate.EditorTool;

                }

            }
            //string editorTool =!string.IsNullOrEmpty(doc.DocumentTemplateId)? repository.context.DocumentTypeTemplates.Where(t => t.Id == doc.DocumentTemplateId).First().EditorTool : null;


            DocumentOutPM docPm = new DocumentOutPM()
            {
                DocumentTypeId = documentFiling.DocumentTypeId,
                ChildEntityId = documentFiling.ChildEntityId,
                ChildEntityReference = documentFiling.ChildEntityReference,
                Id = doc.Id,
                Issued = doc.Issued,
                IssuedByUserId = doc.IssuedByUserId,
                IssuedDate = doc.IssuedDate,
                ObjectTableId = documentFiling.ObjectTableId,
                EntityId = documentFiling.EntityId,
                Note = documentFiling.Notes,
                Tenant = doc.Tenant,
                IssuedByUserName = user != null ? user.Contact.Name : "",
                DocumentTypeSubject = docType.Subject,
                TemplateType = docType.TemplateFormatCode,
                EditableFields = doc.EditableFields,
                DocumentTemplateId = doc.DocumentTemplateId,
                EmailTemplateId = doc.EmailTemplateId,
                DocumentTemplateEditorTool = editorTool,
                XamlDocumentId = doc.XamlDocumentId,
                NeedsRebuild = doc.NeedsRebuild,
                SecurityId = documentFiling.SecurityId,
            };

            docPm.DocumentOutCopies = documentOutCopyQuery.GetDocumentOutCopiesForDocumentOut(docPm.Id, docPm.Tenant);
            SetDocumentFollowUp(docPm);

            if (user != null)
            {
                docPm.IssuedByUserName = user.Contact.EnglishName;
            }

            docPm.DocumentTypeName = docType.Name;
            docPm.DocumentTypeCode = docType.Code;
            docPm.DocumentTypeSubject = docType.Subject;
            docPm.DocumentTypeObjectTableId = docType.ObjectTableId;
            return docPm;
        }

        private static void SetDocumentFollowUp(DocumentOutPM docPm, string followUpId = null)
        {
            if(followUpId == null) followUpId = new FollowUpRepository(docPm.Tenant).GetFollowUpIdByInternalDocumentId(docPm.Tenant, docPm.Id);
            if (string.IsNullOrEmpty(followUpId)) return;

            docPm.FollowUpCount = 1;
            docPm.HasFollowUp = true;
            docPm.FollowUpId = followUpId;
        }

        public List<DocumentOutPM> GetDocumentOutPMsByTenant(int tenant)
        {
            DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(tenant);
            DocumentOutCopyQuery documentOutCopyQuery = new DocumentOutCopyQuery(tenant);
            DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(repository.context);
            List<DocumentOut> internalDocs = (from a in repository.context.DocumentOuts.Include("DocumentsFiling")
                                              where a.Tenant == tenant
                                              select a).ToList();

            var followUpIds = new FollowUpRepository(tenant).GetFollowUpIdByInternalDocumentIds(tenant, internalDocs.Select(x => x.Id).ToArray());
            List<DocumentOutPM> internalDocumentPMs = new List<DocumentOutPM>();
            foreach (DocumentOut doc in internalDocs)
            {
                DocumentsFiling documentFiling = doc.DocumentsFiling;

                User user = (from a in repository.context.Users.Include("Contact")
                             where a.Id == documentFiling.UpdatedByUserId
                             select a).FirstOrDefault();

                DocumentType docType = documentTypeRepository.GetSingleDocumentTypes(documentFiling.DocumentTypeId, doc.Tenant);


                string editorTool = null;
                if (!string.IsNullOrEmpty(doc.DocumentTemplateId))
                {
                    DocumentTypeTemplate documentTypeTemplate = repository.context.DocumentTypeTemplates.Where(t => t.Id == doc.DocumentTemplateId).FirstOrDefault();
                    if (documentTypeTemplate != null)
                    {
                        editorTool = documentTypeTemplate.EditorTool;

                    }

                }
                
                DocumentOutPM docPm = new DocumentOutPM()
                {
                    DocumentTypeId = documentFiling.DocumentTypeId,
                    ChildEntityId = documentFiling.ChildEntityId,
                    ChildEntityReference = documentFiling.ChildEntityReference,
                    Id = doc.Id,
                    Issued = doc.Issued,
                    IssuedByUserId = doc.IssuedByUserId,
                    IssuedDate = doc.IssuedDate,
                    ObjectTableId = documentFiling.ObjectTableId,
                    EntityId = documentFiling.EntityId,
                    Note = documentFiling.Notes,
                    Tenant = doc.Tenant,
                    IssuedByUserName = user != null ? user.Contact.Name : "",
                    DocumentTypeSubject = docType.Subject,
                    TemplateType = docType.TemplateFormatCode,
                    EditableFields = doc.EditableFields,
                    DocumentTemplateId = doc.DocumentTemplateId,
                    EmailTemplateId = doc.EmailTemplateId,
                    DocumentTemplateEditorTool = editorTool,
                    XamlDocumentId = doc.XamlDocumentId,
                    NeedsRebuild = doc.NeedsRebuild,
                    SecurityId = documentFiling.SecurityId,
                };

                docPm.DocumentOutCopies = documentOutCopyQuery.GetDocumentOutCopiesForDocumentOut(docPm.Id, docPm.Tenant);
                SetDocumentFollowUp(docPm, followUpIds.ContainsKey(docPm.Id) ? followUpIds[docPm.Id] : string.Empty);
                if (user != null)
                {
                    docPm.IssuedByUserName = user.Contact.EnglishName;
                }

                docPm.DocumentTypeName = docType.Name;
                docPm.DocumentTypeCode = docType.Code;
                docPm.DocumentTypeSubject = docType.Subject;
                docPm.DocumentTypeObjectTableId = docType.ObjectTableId;

                internalDocumentPMs.Add(docPm);
            }
            return internalDocumentPMs;
        }

        public List<DocumentOutPM> GetDocumentOutPMsByEntityId(string id, int tenant)
        {
            DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(tenant);
            DocumentOutCopyQuery documentOutCopyQuery = new DocumentOutCopyQuery(tenant);
            DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(repository.context);
            List<DocumentOut> internalDocs = (from a in repository.context.DocumentOuts.Include("DocumentsFiling")
                                              where a.Tenant == tenant && a.DocumentsFiling.EntityId == id
                                              select a).ToList();

            var followUpIds = new FollowUpRepository(tenant).GetFollowUpIdByInternalDocumentIds(tenant, internalDocs.Select(x => x.Id).ToArray());
            List<DocumentOutPM> internalDocumentPMs = new List<DocumentOutPM>();
            foreach (DocumentOut doc in internalDocs)
            {
                DocumentsFiling documentFiling = doc.DocumentsFiling;

                User user = (from a in repository.context.Users.Include("Contact")
                             where a.Id == documentFiling.UpdatedByUserId
                             select a).FirstOrDefault();


                DocumentType docType = documentTypeRepository.GetSingleDocumentTypes(documentFiling.DocumentTypeId, doc.Tenant);

              

                string editorTool = null;
                if (!string.IsNullOrEmpty(doc.DocumentTemplateId))
                {
                    DocumentTypeTemplate documentTypeTemplate = repository.context.DocumentTypeTemplates.Where(t => t.Id == doc.DocumentTemplateId).FirstOrDefault();
                    if (documentTypeTemplate != null)
                    {
                        editorTool = documentTypeTemplate.EditorTool;

                    }

                }
                DocumentOutPM docPm = new DocumentOutPM()
                {
                    DocumentTypeId = documentFiling.DocumentTypeId,
                    ChildEntityId = documentFiling.ChildEntityId,
                    ChildEntityReference = documentFiling.ChildEntityReference,
                    Id = doc.Id,
                    Issued = doc.Issued,
                    IssuedByUserId = doc.IssuedByUserId,
                    IssuedDate = doc.IssuedDate,
                    ObjectTableId = documentFiling.ObjectTableId,
                    EntityId = documentFiling.EntityId,
                    Note = documentFiling.Notes,
                    Tenant = doc.Tenant,
                    IssuedByUserName = user != null ? user.Contact.Name : "",
                    DocumentTypeSubject = docType.Subject,
                    TemplateType = docType.TemplateFormatCode,
                    EditableFields = doc.EditableFields,
                    DocumentTemplateId = doc.DocumentTemplateId,
                    EmailTemplateId = doc.EmailTemplateId,
                    DocumentTemplateEditorTool = editorTool,
                    XamlDocumentId = doc.XamlDocumentId,
                    NeedsRebuild = doc.NeedsRebuild,
                    IsCustomerView=docType.IsCustomerView,
                    IsAgentView=docType.IsAgentView,
                    SecurityId = documentFiling.SecurityId,
                    
                };

                docPm.DocumentOutCopies = documentOutCopyQuery.GetDocumentOutCopiesForDocumentOut(docPm.Id, docPm.Tenant);
                SetDocumentFollowUp(docPm, followUpIds.ContainsKey(docPm.Id) ? followUpIds[docPm.Id] : string.Empty);
                if (user != null)
                {
                    docPm.IssuedByUserName = user.Contact.EnglishName;
                }

                docPm.DocumentTypeName = docType.Name;
                docPm.DocumentTypeCode = docType.Code;
                docPm.DocumentTypeSubject = docType.Subject;
                docPm.DocumentTypeObjectTableId = docType.ObjectTableId;

                internalDocumentPMs.Add(docPm);
            }
            return internalDocumentPMs;
        }

        public DocumentOutPM GetDocumentOutByDocumentTypeEntityAndChild(string entityId, string childEntityId, string documentTypeId, int tenant)
        {
            DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(tenant);
            DocumentType docType = documentTypeRepository.GetSingleDocumentTypes(documentTypeId, tenant);
            DocumentOutCopyRepository documentOutCopyRep = new DocumentOutCopyRepository(repository.context);
            DocumentOutCopyQuery documentOutCopyQuery = new DocumentOutCopyQuery(documentOutCopyRep);
            DocumentOutPM docout = null;

            if (!string.IsNullOrEmpty(childEntityId))
            {
                docout = (from a in repository.context.DocumentOuts
                          join docFile in repository.context.DocumentsFilings on a.Id equals docFile.Id
                          where docFile.EntityId == entityId && docFile.ChildEntityId == childEntityId && docFile.DocumentTypeId == documentTypeId && a.Tenant == tenant
                          select new DocumentOutPM()
                          {
                              DocumentTypeId = docFile.DocumentTypeId,
                              Id = a.Id,
                              Issued = a.Issued,
                              IssuedByUserId = a.IssuedByUserId,
                              IssuedDate = a.IssuedDate,
                              ObjectTableId = docFile.ObjectTableId,
                              ChildEntityId = docFile.ChildEntityId,
                              ChildEntityReference = docFile.ChildEntityReference,
                              EntityId = docFile.EntityId,
                              Note = docFile.Notes,
                              Tenant = a.Tenant,
                              DocumentTypeSubject = docType.Subject,
                              EditableFields = a.EditableFields,
                              DocumentTemplateId = a.DocumentTemplateId,
                              EmailTemplateId = a.EmailTemplateId,
                              TemplateType = docType.TemplateFormatCode,
                              XamlDocumentId = a.XamlDocumentId,
                              NeedsRebuild = a.NeedsRebuild,
                              SecurityId =docFile.SecurityId,

                          }
                           ).FirstOrDefault();
            }
            else
            {
                docout = (from a in repository.context.DocumentOuts
                          join docFile in repository.context.DocumentsFilings on a.Id equals docFile.Id
                          where docFile.EntityId == entityId && docFile.DocumentTypeId == documentTypeId && a.Tenant == tenant
                          select new DocumentOutPM()
                          {
                              DocumentTypeId = docFile.DocumentTypeId,
                              Id = a.Id,
                              Issued = a.Issued,
                              IssuedByUserId = a.IssuedByUserId,
                              IssuedDate = a.IssuedDate,
                              ObjectTableId = docFile.ObjectTableId,
                              ChildEntityId = docFile.ChildEntityId,
                              ChildEntityReference = docFile.ChildEntityReference,
                              EntityId = docFile.EntityId,
                              Note = docFile.Notes,
                              Tenant = a.Tenant,
                              DocumentTypeSubject = docType.Subject,
                              EditableFields = a.EditableFields,
                              DocumentTemplateId = a.DocumentTemplateId,
                              EmailTemplateId = a.EmailTemplateId,
                              TemplateType = docType.TemplateFormatCode,
                              XamlDocumentId = a.XamlDocumentId,
                              SecurityId = docFile.SecurityId,
                              // DocumentTemplateEditorTool = a.DocumentTemplateId != null ? repository.context.DocumentTypeTemplates.Where(t => t.Id == a.DocumentTemplateId).FirstOrDefault().EditorTool : null,
                              NeedsRebuild = a.NeedsRebuild,
                          }).FirstOrDefault();
            }

            if (docout != null)
            {
                User user = (from a in repository.context.Users.Include("Contact")
                             where a.Tenant == docout.Tenant && a.Id == docout.IssuedByUserId
                             select a).FirstOrDefault();
                docout.IssuedByUserName = user != null ? user.Contact.EnglishName : null;
                docout.DocumentOutCopies = documentOutCopyQuery.GetDocumentOutCopiesForDocumentOut(docout.Id, docout.Tenant);
                docout.DocumentTypeName = docType.Name;
                docout.DocumentTypeCode = docType.Code;
                docout.DocumentTypeSubject = docType.Subject;
                docout.DocumentTypeObjectTableId = docType.ObjectTableId;
                DocumentTypeTemplate template = null;

                if (string.IsNullOrEmpty(docout.DocumentTemplateId)) docout.DocumentTemplateId = docType.DocumentTypeDefaultReportTemplateId;
                if (string.IsNullOrEmpty(docout.EmailTemplateId)) docout.EmailTemplateId = docType.DocumentTypeDefaultHTMLTemplateId;
                if (!string.IsNullOrEmpty(docout.DocumentTemplateId))
                {
                    template = repository.context.DocumentTypeTemplates.Where(t => t.Id == docout.DocumentTemplateId && t.DocumentTypeId == docType.Id).FirstOrDefault();
                }
           


                if (!string.IsNullOrEmpty(docout.DocumentTemplateId))
                {
                    template = repository.context.DocumentTypeTemplates.Where(t => t.Id == docout.DocumentTemplateId && t.DocumentTypeId == docType.Id).FirstOrDefault();
                }

                if(template!=null) docout.DocumentTemplateEditorTool = template.EditorTool;
             

            }

            return docout;
        }

        public List<DocumentOutPM> GetDocumentOutPMsByEntityIdAndObjectTable(string entityId, string childEntityId, string objectTableId, int tenant)
        {
            DocumentOutCopyRepository documentOutCopyRep = new DocumentOutCopyRepository(repository.context);
            DocumentOutCopyQuery documentOutCopyQuery = new DocumentOutCopyQuery(documentOutCopyRep);
            List<DocumentOut> documentOuts;


            if (string.IsNullOrEmpty(childEntityId))
            {
                documentOuts = (from a in repository.context.DocumentOuts.Include("DocumentsFiling")
                                where a.DocumentsFiling.EntityId == entityId && a.DocumentsFiling.ObjectTableId == objectTableId
                                select a).ToList();
            }
            else
            {
                documentOuts = (from a in repository.context.DocumentOuts.Include("DocumentsFiling")
                                where a.DocumentsFiling.EntityId == entityId && a.DocumentsFiling.ChildEntityId == childEntityId && a.DocumentsFiling.ObjectTableId == objectTableId
                                select a).ToList();
            }

            var followUpIds = new FollowUpRepository(tenant).GetFollowUpIdByInternalDocumentIds(tenant, documentOuts.Select(x => x.Id).ToArray());
            List<DocumentOutPM> documentOutPMs = new List<DocumentOutPM>();
            DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(tenant);
       
            foreach (DocumentOut doc in documentOuts)
            {
                //User user = (from a in repository.context.Users.Include("Contact")
                //             where a.Tenant == doc.Tenant && a.Id == doc.DocumentsFiling.UpdatedByUserId
                //             select a).FirstOrDefault();
                Contact issuedByContact = (from a in repository.context.Contacts
                                           where a.Tenant == doc.Tenant && a.Id == doc.IssuedByUserId
                                           select a).FirstOrDefault();

                DocumentType docType = documentTypeRepository.GetSingleDocumentTypes(doc.DocumentsFiling.DocumentTypeId, doc.Tenant);
        
                string editorTool = null;
                if (!string.IsNullOrEmpty(doc.DocumentTemplateId))
                {
                    DocumentTypeTemplate documentTypeTemplate = repository.context.DocumentTypeTemplates.Where(t => t.Id == doc.DocumentTemplateId).FirstOrDefault();
                    if (documentTypeTemplate != null)
                    {
                        editorTool = documentTypeTemplate.EditorTool;

                    }

                }

                DocumentOutPM docPm = new DocumentOutPM()
                {
                    DocumentTypeId = doc.DocumentsFiling.DocumentTypeId,

                    Id = doc.Id,
                    Issued = doc.Issued,
                    IssuedByUserId = doc.IssuedByUserId,
                    IssuedDate = doc.IssuedDate,
                    ObjectTableId = doc.DocumentsFiling.ObjectTableId,
                    ChildEntityId = doc.DocumentsFiling.ChildEntityId,
                    ChildEntityReference = doc.DocumentsFiling.ChildEntityReference,
                    EntityId = doc.DocumentsFiling.EntityId,
                    Note = doc.DocumentsFiling.Notes,
                    Tenant = doc.Tenant,
                    DocumentTypeSubject = docType.Subject,
                    IssuedByUserName = issuedByContact != null ? issuedByContact.EnglishName : null,
                    EditableFields = doc.EditableFields,
                    DocumentTemplateId = doc.DocumentTemplateId,
                    EmailTemplateId = doc.EmailTemplateId,
                    TemplateType = docType.TemplateFormatCode,
                    DocumentTemplateEditorTool = editorTool,
                    XamlDocumentId = doc.XamlDocumentId,
                    NeedsRebuild = doc.NeedsRebuild,
                    SecurityId = doc.DocumentsFiling.SecurityId,
                    
                };

                SetDocumentFollowUp(docPm, followUpIds.ContainsKey(docPm.Id) ? followUpIds[docPm.Id] : string.Empty);
                //if (user != null)
                //{
                //    docPm.IssuedByUserName = user.Contact.EnglishName;

                //}
                docPm.DocumentOutCopies = documentOutCopyQuery.GetDocumentOutCopiesForDocumentOut(docPm.Id, docPm.Tenant);
                docPm.DocumentTypeName = docType.Name;
                docPm.DocumentTypeCode = docType.Code;
                docPm.DocumentTypeSubject = docType.Subject;
                docPm.DocumentTypeObjectTableId = docType.ObjectTableId;
                documentOutPMs.Add(docPm);
            }

            return documentOutPMs;
        }


        public List<DocumentOutPM> GetDocumentOutPMsByEntityIdAndObjectTableAndChildEntityId(string entityId, string childEntityId, string objectTableId, int tenant)
        {

            DocumentOutCopyRepository documentOutCopyRep = new DocumentOutCopyRepository(repository.context);
            DocumentOutCopyQuery documentOutCopyQuery = new DocumentOutCopyQuery(documentOutCopyRep);
            List<DocumentOut> documentOuts;
            List<DocumentOutPM> documentOutPMs = new List<DocumentOutPM>();
            if (string.IsNullOrEmpty(childEntityId))
            {
                documentOuts = (from a in repository.context.DocumentOuts.Include("DocumentsFiling")
                                where a.DocumentsFiling.EntityId == entityId && a.DocumentsFiling.ObjectTableId == objectTableId
                                select a).ToList();
            }
            else
            {
                documentOuts = (from a in repository.context.DocumentOuts.Include("DocumentsFiling")
                                where a.DocumentsFiling.EntityId == entityId && a.DocumentsFiling.ChildEntityId == childEntityId && a.DocumentsFiling.ObjectTableId == objectTableId
                                select a).ToList();
            }

            DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(tenant);

            List<string> contactIds = new List<string>();
            List<string> documentTypeIds = new List<string>();
            foreach (DocumentOut doc in documentOuts)
            {
                if (!string.IsNullOrEmpty(doc.IssuedByUserId))
                {
                    if (!contactIds.Contains(doc.IssuedByUserId)) contactIds.Add(doc.IssuedByUserId);
                }

                if (doc.DocumentsFiling!=null && !string.IsNullOrEmpty(doc.DocumentsFiling.DocumentTypeId))
                {
                    if (!documentTypeIds.Contains(doc.DocumentsFiling.DocumentTypeId)) documentTypeIds.Add(doc.DocumentsFiling.DocumentTypeId);
                }
            }

            List<Contact> contactLists = null;
            if (contactIds.Count > 0)
            {
                ContactRepository contactRepository = new ContactRepository(tenant);
                contactLists = contactRepository.GetContactListsByListids(contactIds, tenant).ToList();
            }

            List<DocumentType> documentTypeLists = null;
            if (documentTypeIds.Count > 0)
            {
                DocumentTypeRepository dcumentTypeRepository = new DocumentTypeRepository(tenant);
                documentTypeLists = documentTypeRepository.GetDocumentTypeListsByListids(documentTypeIds, tenant).ToList();
            }


            foreach (DocumentOut doc in documentOuts)
            {
                Contact issuedByContact = null;
                if (!string.IsNullOrEmpty(doc.IssuedByUserId) && contactLists != null) issuedByContact = contactLists.Where(d => d.Id == doc.IssuedByUserId).FirstOrDefault();
     
                DocumentType docType = null;
                if (doc.DocumentsFiling !=null && !string.IsNullOrEmpty(doc.DocumentsFiling.DocumentTypeId) && documentTypeLists!=null) docType = documentTypeLists.Where(d => d.Id == doc.DocumentsFiling.DocumentTypeId).FirstOrDefault();
             
    
                string editorTool = null;
                if (string.IsNullOrEmpty(doc.DocumentTemplateId))
                {
                    if (docType != null)
                    {
                        if (docType.TemplateFormatCode == "P")
                        {
                            doc.DocumentTemplateId = docType.DocumentTypeDefaultReportTemplateId;
                        }
    
                    }
                }





                if (!string.IsNullOrEmpty(doc.DocumentTemplateId))
                {
                     editorTool =  (from record in repository.context.DocumentTypeTemplates where record.Id == doc.DocumentTemplateId  select record.EditorTool).FirstOrDefault();
                }

                DocumentOutPM docPm = new DocumentOutPM()
                {
                    DocumentTypeId = doc.DocumentsFiling.DocumentTypeId,

                    Id = doc.Id,
                    Issued = doc.Issued,
                    IssuedByUserId = doc.IssuedByUserId,
                    IssuedDate = doc.IssuedDate,
                    ObjectTableId = doc.DocumentsFiling.ObjectTableId,
                    ChildEntityId = doc.DocumentsFiling.ChildEntityId,
                    ChildEntityReference = doc.DocumentsFiling.ChildEntityReference,
                    EntityId = doc.DocumentsFiling.EntityId,
                    Note = doc.DocumentsFiling.Notes,
                    Tenant = doc.Tenant,
                    DocumentTypeSubject = docType!=null? docType.Subject:"",
                    IssuedByUserName = issuedByContact != null ? issuedByContact.EnglishName : null,
                    EditableFields = doc.EditableFields,
                    DocumentTemplateId = doc.DocumentTemplateId,
                    EmailTemplateId = doc.EmailTemplateId,
                    TemplateType = docType!=null? docType.TemplateFormatCode:"",
                    DocumentTemplateEditorTool = editorTool,
                    XamlDocumentId = doc.XamlDocumentId,
                    NeedsRebuild = doc.NeedsRebuild,
                    SecurityId = doc.DocumentsFiling.SecurityId,

                };

               
                docPm.DocumentOutCopies = documentOutCopyQuery.GetDocumentOutCopiesForDocumentOut(docPm.Id, docPm.Tenant);

                if (docType != null)
                {
                    docPm.DocumentTypeName = docType.Name;
                    docPm.DocumentTypeCode = docType.Code;
                    docPm.DocumentTypeSubject = docType.Subject;
                    docPm.DocumentTypeObjectTableId = docType.ObjectTableId;
                }
                documentOutPMs.Add(docPm);
            }

            return documentOutPMs;
        }


        public List<DocumentOutPM> GetDocumentOutPMsByDocOutIds(List<string>docOutIds, int tenant)
        {
       
            List<DocumentOutPM> documentOutLists = (from a in repository.context.DocumentOuts
                                              where a.Tenant == tenant && docOutIds.Contains(a.Id)
                                                    select new DocumentOutPM()
                                                    {
                                                        Id = a.Id,
                                                        IssuedDate = a.IssuedDate,
                                                    }).ToList();




            return documentOutLists;
        }

        public byte[] GetEditableFieldsByDocumentId(string id, int tenant)
        {
            return (from a in repository.context.DocumentOuts
                    where a.Tenant == tenant && a.Id == id
                    select a.EditableFields).FirstOrDefault();


        }



        public string  GetCalculatedFileNameForDocumentOutCopy(string documentOutId, string documentTypeCopyId, int tenant)
        {
            string fileName = "";
            DocumentOutCopyQuery dcumentOutCopyQuery = new DocumentOutCopyQuery(tenant);
            DocumentOutCopyList documentOutCopyList = dcumentOutCopyQuery.GeDocumentOutCopyBydocumentTypeCopyAndDocumentOutId(documentTypeCopyId, documentOutId, tenant);
            if (documentOutCopyList != null)
            {
                DocumentRepository documentRepository = new DocumentRepository(tenant);
                fileName = documentRepository.GetCalculatedFileNameById(documentOutCopyList.Id , tenant);

            }

            return fileName;
        }


    }
}
