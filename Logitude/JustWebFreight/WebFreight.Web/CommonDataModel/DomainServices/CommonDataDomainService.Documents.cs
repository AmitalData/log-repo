using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Transactions;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.SystemLogs;
using System.Web;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Simplog.Data.ShipmentsModel.Repositories;
using ICSharpCode.SharpZipLib.Zip;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using ICSharpCode.SharpZipLib.Checksums;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        #region DocumentTypes
        public void UpdateDocumentTypeList(DocumentTypeList currentEntity)
        {
        }

        public IQueryable<DocumentType> GetDocumentTypes(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeRepository = new DocumentTypeRepository(tenant);
            return documentTypeRepository.GetDocumentTypes(0);
        }

        public List<DocumentTypePM> GetDocumentTypesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }
            documentTypeRepository = new DocumentTypeRepository(objectContext);
            documentTypeQuery = new DocumentTypeQuery(documentTypeRepository);

            documentTypeTemplateRepository = new DocumentTypeTemplateRepository(objectContext);
            documentTypeTemplateQuery = new DocumentTypeTemplateQuery(documentTypeTemplateRepository);

            documentTypeCopyRepository = new DocumentTypeCopyRepository(objectContext);
            documentTypeCopyQuery = new DocumentTypeCopyQuery(documentTypeCopyRepository);

            List<DocumentTypePM> documentTypes = documentTypeQuery.GetDocumentTypePMsByTenant(tenant).ToList();

            foreach (DocumentTypePM doc in documentTypes)
            {
                doc.DocumentTypeTemplates = documentTypeTemplateQuery.GetDocumentTypeTemplatesByDocumentTypeId(doc.Id, tenant).ToList();
                doc.DocumentTypeCopies = documentTypeCopyQuery.GetDocumentTypeCopiesByDocumentType(doc.Id, null, tenant);
            }
            return documentTypes;
        }

        public List<DocumentTypePM> GetDocumentTypesByObjectTableAndTenant(string objectTableid, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeQuery = new DocumentTypeQuery(tenant);
            List<DocumentTypePM> documentTypes = documentTypeQuery.GetDocumentTypePMsByObjectTableAndTenant(objectTableid, tenant);
            return documentTypes;
        }

        public List<DocumentTypePM> GetDocumentTypesByEnityIdAndTenant(string transportModeId, string shipmentLevelCode, string objecttableId, int tenant, string childrenObjectTableIds = null)
        {

            // Thread.Sleep(500000);

            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeQuery = new DocumentTypeQuery(tenant);
            List<DocumentTypePM> documentTypes = documentTypeQuery.GetDocumentTypePMsByEnityIdAndTenant(transportModeId, shipmentLevelCode, objecttableId, tenant, childrenObjectTableIds);
            return documentTypes;
        }


        public List<DocumentTypeList> GetDocumentTypeListsByEnityIdAndTenant(string transportModeId, string shipmentLevelCode, string objecttableId, int tenant)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeQuery = new DocumentTypeQuery(tenant);
            List<DocumentTypeList> documentTypes = documentTypeQuery.GetDocumentTypeListsByEnityIdAndTenant(transportModeId, shipmentLevelCode, objecttableId, tenant);
            return documentTypes;
        }




        public List<DocumentTypeList> GetDocumentTypeListsByObjectTableIdForAutomations(string objectTableId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            documentTypeQuery = new DocumentTypeQuery(tenant);
            List<DocumentTypeList> documentTypes = documentTypeQuery.GetDocumentTypeListsByObjectTableId(objectTableId, tenant);
            return documentTypes;
        }




        public IQueryable<DocumentTypePM> GetDocumentTypesByTenantAndTransportMode(int tenant, string transportMode)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeQuery = new DocumentTypeQuery(tenant);
            return documentTypeQuery.GetDocumentTypesByTenantAndTransportMode(tenant, transportMode);
        }

        public DocumentTypePM GetDocumentTypeByCode(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeQuery = new DocumentTypeQuery(tenant);
            DocumentTypePM result = documentTypeQuery.GetSinglePMByCodeAndTenant(code, tenant);
            return result;
        }

        public List<DocumentTypePM> GetDocumentTypesSearch(string name, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeQuery = new DocumentTypeQuery(tenant);
            List<DocumentTypePM> q = documentTypeQuery.GetDocumentTypePMsByTenant(tenant).Where(d => d.Name.StartsWith(name) && d.Tenant == tenant).ToList();
            return q;
        }

        public bool DoesDocumentTypeCodeExist(string code, int tenant)
        {
            documentTypeRepository = new DocumentTypeRepository(tenant);
            return (documentTypeRepository.GetDocumentTypes(tenant).Where(d => d.Code == code && d.Tenant == tenant)).Any();
        }

        public DocumentTypePM GetSingleDocumentType(string id, string documentOutId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeQuery = new DocumentTypeQuery(tenant);
            return this.documentTypeQuery.GetSinglePM(id, documentOutId, tenant);
        }

        public DocumentTypeList GetSingleDocumentTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeRepository = new DocumentTypeRepository(tenant);
            DocumentType documentType = documentTypeRepository.GetSingleDocumentTypes(id, tenant);
            DocumentTypeList documentTypeList = null;

            if (documentType != null)
            {
                List<DocumentType> singleEntityList = new List<DocumentType>();
                singleEntityList.Add(documentType);

                documentTypeQuery = new DocumentTypeQuery(documentTypeRepository);
                IQueryable<DocumentType> iQueryable = singleEntityList.AsQueryable();
                IQueryable<DocumentTypeList> iQueryableEntityList = documentTypeQuery.GetIQueryableEntityList(iQueryable);
                documentTypeList = iQueryableEntityList.FirstOrDefault();

                ObjectTablePM pm = ObjectTableQuery.GetSingleObjectTableById(documentTypeList.ObjectTableId, documentTypeList.Tenant);

                if (pm != null)
                {
                    documentTypeList.ObjectTableName = pm.Name;
                }



            }
            return documentTypeList;
        }

        public IQueryable<DocumentTypeList> GetDocumentTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeRepository = new DocumentTypeRepository(tenant);
            documentTypeQuery = new DocumentTypeQuery(documentTypeRepository);

            IQueryable<DocumentType> documentTypes = documentTypeRepository.GetDocumentTypes(tenant);
            IQueryable<DocumentTypeList> query2 = documentTypeQuery.GetIQueryableEntityList(documentTypes);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<DocumentTypeList> GetDocumentTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeRepository = new DocumentTypeRepository(tenant);
            documentTypeQuery = new DocumentTypeQuery(documentTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<DocumentType> documentTypes = documentTypeRepository.GetDocumentTypes(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            documentTypes = filter.GetFilteredQuery<DocumentType>(nonListQueryOperation, documentTypes);
            int skippedDocumentTypes = queryOperations.PageIndex;

            IQueryable<DocumentTypeList> query2 = documentTypeQuery.GetIQueryableEntityList(documentTypes);

            query2 = filter.GetFilteredQuery<DocumentTypeList>(listQueryOperation, query2);
            
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(DocumentTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("DocumentType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();
                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<DocumentTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<DocumentTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<DocumentTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<DocumentTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<DocumentTypeList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Code);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }
            
            query2 = query2.Skip(skippedDocumentTypes);
            query2 = query2.Take(queryOperations.PageSize);


            List<DocumentTypeList> objectsList = query2.ToList();
            foreach (var list in objectsList)
            {
                if (!string.IsNullOrEmpty(list.ObjectTableId))
                {
                ObjectTablePM pm = ObjectTableQuery.GetSingleObjectTableById(list.ObjectTableId, list.Tenant);
                list.ObjectTableName = pm.Name;
                }

            }

            query2 = objectsList.AsQueryable();

            return query2;
        }

        public int GetDocumentTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeRepository = new DocumentTypeRepository(tenant);
            documentTypeQuery = new DocumentTypeQuery(documentTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<DocumentType> documentTypes = documentTypeRepository.GetDocumentTypes(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            documentTypes = filter.GetFilteredQuery<DocumentType>(nonListQueryOperation, documentTypes);

            int skippeddocumentTypes = queryOperations.PageIndex;
            IQueryable<DocumentTypeList> query2 = documentTypeQuery.GetIQueryableEntityList(documentTypes);
            query2 = filter.GetFilteredQuery<DocumentTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertDocumentType(DocumentTypePM entity)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entity.Tenant);
            }
            DocumentTypeService service = new DocumentTypeService(objectContext, entity.Tenant);
            service.Create(entity);

            //documentTypeRepository = new DocumentTypeRepository(objectContext);
            //documentTypeCopyRepository = new DocumentTypeCopyRepository(objectContext);
            //documentTypeCustomFieldRepository = new DocumentTypeCustomFieldRepository(objectContext);

            //DocumentType newEntity = new DocumentType();
            //entity.Id = IdCounter.GetNumber("DocumentType", entity.Tenant).ToString();
            //newEntity.Id = entity.Id;

            //#region documentTypecopeis
            //if (entity.DocumentTypeCopies != null)
            //{
            //    foreach (DocumentTypeCopyPM a in entity.DocumentTypeCopies)
            //    {
            //        a.DocumentTypeId = entity.Id;

            //        DocumentTypeCopy copy = new DocumentTypeCopy()
            //        {
            //            Id = IdCounter.GetNumber("DocumentTypeCopy", entity.Tenant).ToString(),
            //            Code = a.Code,
            //            Name = a.Name,
            //            Tenant = entity.Tenant,
            //            DocumentTypeId = entity.Id,
            //        };
            //        a.Id = copy.Id;
            //        documentTypeCopyRepository.Add(copy);
            //    }

            //    if (entity.DocumentTypeCopies.Count == 0)
            //    {
            //        DocumentTypeCopy copy = new DocumentTypeCopy()
            //        {
            //            Id = IdCounter.GetNumber("DocumentTypeCopy", entity.Tenant).ToString(),
            //            Code = entity.Code,
            //            Name = entity.Name,
            //            Tenant = entity.Tenant,
            //            DocumentTypeId = entity.Id,
            //            IsSelectedByDefault = true,
            //        };
            //        documentTypeCopyRepository.Add(copy);
            //    }
            //}
            //#endregion

            //MapDocuemntTypeDocumentTypePM(entity, newEntity);
            //documentTypeRepository.Add(newEntity);
           
            ////create TraceEvent
            //WebFreightDomainService webfreightService = new WebFreightDomainService();
            //ContactRepository contactsRepositorypository = new ContactRepository(objectContext);
            //ContactQuery contactQuery = new ContactQuery(contactsRepositorypository);
            //ContactPM contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), entity.Tenant, true);
            //if (contact != null) EventTracer.CreateTraceEvent(new TraceEvent(), "CRDT", entity.Tenant, contact.Id, entity.Id, null, "DocumentType", null, null, false);
            
            //TableLastUpdateClass.UpdateTableHistory(entity.Tenant, "DocumentType");

            //#region DocuemntTypeCustomField
            //if (entity.Code == "740")
            //{                            
            //}
            //if (entity.Code == "716")
            //{
            //    DocumentTypeCustomField customfield1 = new DocumentTypeCustomField() { DefaultValue = "3", DocumentTypeId = entity.Id, Name = "Number Of Originals", FieldCode = "NumberOfOriginals", FieldDataTypeCode = "Text", Id = IdCounter.GetNumber("DocumentTypeCustomField", entity.Tenant).ToString(), InActive = false, IsRequired = false, Tenant = entity.Tenant, MultiLine = false };
            //    DocumentTypeCustomField customfield2 = new DocumentTypeCustomField() { DefaultValue = "Copy", DocumentTypeId = entity.Id, Name = "Copy Or Original", FieldCode = "CopyOrOriginal", FieldDataTypeCode = "Text", Id = IdCounter.GetNumber("DocumentTypeCustomField", entity.Tenant).ToString(), InActive = false, IsRequired = false, Tenant = entity.Tenant, MultiLine = false };
            //    DocumentTypeCustomField customfield3 = new DocumentTypeCustomField() { DefaultValue = "False", DocumentTypeId = entity.Id, Name = "Has attachment list", FieldCode = "HasAttachmentList", FieldDataTypeCode = "Boolean", Id = IdCounter.GetNumber("DocumentTypeCustomField", entity.Tenant).ToString(), InActive = false, IsRequired = false, Tenant = entity.Tenant, MultiLine = false };

            //    documentTypeCustomFieldRepository.Add(customfield1);
            //    documentTypeCustomFieldRepository.Add(customfield2);
            //    documentTypeCustomFieldRepository.Add(customfield3);
            //}

            //if (entity.Code == "716SD")
            //{
            //    DocumentTypeCustomField cutomfield1 = new DocumentTypeCustomField() { DefaultValue = "", DocumentTypeId = entity.Id, Name = "Originals/Copies", FieldCode = "OriginalsOrCopiesNo", FieldDataTypeCode = "Text", Id = IdCounter.GetNumber("DocumentTypeCustomField", entity.Tenant).ToString(), InActive = false, IsRequired = false, Tenant = entity.Tenant, MultiLine = false };
            //    DocumentTypeCustomField customfield3 = new DocumentTypeCustomField() { DefaultValue = "False", DocumentTypeId = entity.Id, Name = "Has attachment list", FieldCode = "HasAttachmentList", FieldDataTypeCode = "Boolean", Id = IdCounter.GetNumber("DocumentTypeCustomField", entity.Tenant).ToString(), InActive = false, IsRequired = false, Tenant = entity.Tenant, MultiLine = false };
            //    documentTypeCustomFieldRepository.Add(cutomfield1);
            //    documentTypeCustomFieldRepository.Add(customfield3);
            //}

            //if (entity.Code == "CMR")
            //{
            //    DocumentTypeCustomField cutomfield1 = new DocumentTypeCustomField() { DefaultValue = "", DocumentTypeId = entity.Id, Name = "Cash On Delivery", FieldCode = "CashOnDelivery", FieldDataTypeCode = "Text", Id = IdCounter.GetNumber("DocumentTypeCustomField", entity.Tenant).ToString(), InActive = false, IsRequired = false, Tenant = entity.Tenant, MultiLine = false };
            //    DocumentTypeCustomField cutomfield2 = new DocumentTypeCustomField() { DefaultValue = "", DocumentTypeId = entity.Id, Name = "Instructions", FieldCode = "Instructions", FieldDataTypeCode = "Text", Id = IdCounter.GetNumber("DocumentTypeCustomField", entity.Tenant).ToString(), InActive = false, IsRequired = false, Tenant = entity.Tenant, MultiLine = true };
            //    DocumentTypeCustomField customfield3 = new DocumentTypeCustomField() { DefaultValue = "", DocumentTypeId = entity.Id, Name = "Documents Attached", FieldCode = "DocumentsAttached", FieldDataTypeCode = "Text", Id = IdCounter.GetNumber("DocumentTypeCustomField", entity.Tenant).ToString(), InActive = false, IsRequired = false, Tenant = entity.Tenant, MultiLine = true };

            //    documentTypeCustomFieldRepository.Add(cutomfield1);
            //    documentTypeCustomFieldRepository.Add(cutomfield2);
            //    documentTypeCustomFieldRepository.Add(customfield3);
            //}

            //if (entity.Code == "SCMR")
            //{
            //    DocumentTypeCustomField cutomfield1 = new DocumentTypeCustomField() { DefaultValue = "", DocumentTypeId = entity.Id, Name = "Cash On Delivery", FieldCode = "CashOnDelivery", FieldDataTypeCode = "Text", Id = IdCounter.GetNumber("DocumentTypeCustomField", entity.Tenant).ToString(), InActive = false, IsRequired = false, Tenant = entity.Tenant, MultiLine = false };
            //    DocumentTypeCustomField cutomfield2 = new DocumentTypeCustomField() { DefaultValue = "", DocumentTypeId = entity.Id, Name = "Instructions (13)", FieldCode = "Instructions(13)", FieldDataTypeCode = "Text", Id = IdCounter.GetNumber("DocumentTypeCustomField", entity.Tenant).ToString(), InActive = false, IsRequired = false, Tenant = entity.Tenant, MultiLine = true };
            //    DocumentTypeCustomField customfield3 = new DocumentTypeCustomField() { DefaultValue = "", DocumentTypeId = entity.Id, Name = "Documents Attached", FieldCode = "DocumentsAttached", FieldDataTypeCode = "Text", Id = IdCounter.GetNumber("DocumentTypeCustomField", entity.Tenant).ToString(), InActive = false, IsRequired = false, Tenant = entity.Tenant, MultiLine = true };
            //    documentTypeCustomFieldRepository.Add(cutomfield1);
            //    documentTypeCustomFieldRepository.Add(cutomfield2);
            //    documentTypeCustomFieldRepository.Add(customfield3);
            //}
            //if (entity.Code == "740L")
            //{
            //    DocumentTypeCustomField cutomfield1 = new DocumentTypeCustomField() { DefaultValue = "", DocumentTypeId = entity.Id, Name = "Additional Information", FieldCode = "AdditionalInformation", FieldDataTypeCode = "Text", Id = IdCounter.GetNumber("DocumentTypeCustomField", entity.Tenant).ToString(), InActive = false, IsRequired = false, Tenant = entity.Tenant, MultiLine = true };
            //    documentTypeCustomFieldRepository.Add(cutomfield1);
            //}
            //#endregion
        }

        public void UpdateDocumentType(DocumentTypePM currentEntity)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentEntity.Tenant);
            }
         
            documentTypeRepository = new DocumentTypeRepository(objectContext);
            documentTypeCopyRepository = new DocumentTypeCopyRepository(objectContext);

            DocumentType docType = documentTypeRepository.GetSingleDocumentTypes(currentEntity.Id, currentEntity.Tenant);
            if (currentEntity.Name != docType.Name)
            {
                if (currentEntity.DocumentTypeCopies.Count == 1)
                {
                    DocumentTypeCopyPM copyPM = currentEntity.DocumentTypeCopies.FirstOrDefault() as DocumentTypeCopyPM;
                    DocumentTypeCopy documentTypeCopy = documentTypeCopyRepository.GetSingleDocumentTypeCopy(copyPM.Id);
                    documentTypeCopy.Name = currentEntity.Name;
                    copyPM.Name = currentEntity.Name;

                    documentTypeCopyRepository.Update(documentTypeCopy);
                }
            }

           // MapDocuemntTypeDocumentTypePM(currentEntity, docType);

            List<DocumentTypeCopyPM> documentTypeCopyChangeSet = ChangeSet.GetAssociatedChanges(currentEntity, d => d.DocumentTypeCopies).Cast<DocumentTypeCopyPM>().ToList();

            #region DocumentTypeCopies
            foreach (DocumentTypeCopyPM r in documentTypeCopyChangeSet)
            {
                ChangeOperation op = ChangeSet.GetChangeOperation(r);
                switch (op)
                {
                    case ChangeOperation.Insert:
                        {
                            r.ChangeSetOp = ChangeSetOperation.Insert;
                            //r.Id = IdCounter.GetNumber("DocumentTypeCopy", currentEntity.Tenant).ToString();
                            //DocumentTypeCopy newDocumentTypeCopy = new DocumentTypeCopy();
                            //newDocumentTypeCopy.Id = r.Id;
                            //MapDocumentTypeCopyDocumentTypeCopyPM(r, newDocumentTypeCopy);
                            //documentTypeCopyRepository.Add(newDocumentTypeCopy);
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            r.ChangeSetOp = ChangeSetOperation.Update;
                            //DocumentTypeCopy documentTypeCopy = documentTypeCopyRepository.GetSingleDocumentTypeCopy(r.Id);
                            //MapDocumentTypeCopyDocumentTypeCopyPM(r, documentTypeCopy);
                            //documentTypeCopyRepository.Update(documentTypeCopy);
                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            r.ChangeSetOp = ChangeSetOperation.Delete;
                            //DocumentTypeCopy documentTypeCopy = documentTypeCopyRepository.GetSingleDocumentTypeCopy(r.Id);
                            //documentTypeCopyRepository.Remove(documentTypeCopy);
                            break;
                        }
                    case ChangeOperation.None:
                        {
                            r.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            # endregion

            DocumentTypeService service = new DocumentTypeService(objectContext, currentEntity.Tenant);
            service.Update(currentEntity, documentTypeCopyChangeSet);


            //MapDocuemntTypeDocumentTypePM(currentEntity, docType);
            //documentTypeRepository.Update(docType);

            ////create TraceEvent
            //WebFreightDomainService webfreightService = new WebFreightDomainService();
            //ContactRepository contactsRepositorypository = new ContactRepository(objectContext);
            //ContactQuery contactQuery = new ContactQuery(contactsRepositorypository);
            //ContactPM contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), currentEntity.Tenant, true);
            //if (contact != null) EventTracer.CreateTraceEvent(new TraceEvent(), "UPDT", currentEntity.Tenant, contact.Id, currentEntity.Id, null, "DocumentType", null, null, false);
            
            TableLastUpdateClass.UpdateTableHistory(currentEntity.Tenant, "DocumentType");
        }

        public void DeleteDocumentType(DocumentTypePM entity)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entity.Tenant);
            }
            documentTypeRepository = new DocumentTypeRepository(objectContext);
            DocumentType docType = documentTypeRepository.GetSingleDocumentTypes(entity.Id, entity.Tenant);
            documentTypeRepository.Remove(docType);
        }
        #endregion

        #region DocumentOuts
        public IQueryable<DocumentOut> GetDocumentOuts(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentOutRepository = new DocumentOutRepository(tenant);
            return documentOutRepository.GetDocumentOuts(0);
        }

        public List<DocumentOutPM> GetDocumentOutsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentOutQuery = new DocumentOutQuery(tenant);
            return documentOutQuery.GetDocumentOutPMsByTenant(tenant);
        }

        public List<DocumentOutPM> GetDocumentOutsByEntityId(string entityId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentOutQuery = new DocumentOutQuery(tenant);
            return documentOutQuery.GetDocumentOutPMsByEntityId(entityId, tenant);
        }

        public List<DocumentOutPM> GetDocumentOutsByDocumentType(string docType, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentOutQuery = new DocumentOutQuery(tenant);
            return documentOutQuery.GetDocumentOutPMsByTenant(tenant).Where(d => d.DocumentTypeCode == docType && d.Tenant == tenant).ToList();
        }

        public DocumentOutPM GetSingleDocumentOutPM(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentOutQuery = new DocumentOutQuery(tenant);
            return this.documentOutQuery.GetSinglePM(id, tenant);
        }

        public List<DocumentOutPM> GetDocumentOutsByEntityIdAndObjectTable(string entityId, string childEntityId, string objectTableId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentOutQuery = new DocumentOutQuery(tenant);
            return documentOutQuery.GetDocumentOutPMsByEntityIdAndObjectTable(entityId, childEntityId, objectTableId, tenant);
        }

        public DocumentOutPM GetDocumentOutByDocumentTypeEntityAndChild(string entityId, string childEntityId, string documentTypeId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentOutQuery = new DocumentOutQuery(tenant);
            return documentOutQuery.GetDocumentOutByDocumentTypeEntityAndChild(entityId, childEntityId, documentTypeId, tenant);
        }

        public DocumentOutPM GetDocumentOutReturnNewIfNon(string entityId, string childEntityId, string documentTypeId, string childentityreference, string objectTableId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentOutQuery = new DocumentOutQuery(tenant);
            
            DocumentOutPM documentout = documentOutQuery.GetDocumentOutByDocumentTypeEntityAndChild(entityId, childEntityId, documentTypeId, tenant);
            if (documentout == null)
            {
                documentout = this.CreateDocumentOut(documentTypeId, entityId, childEntityId, childentityreference, objectTableId, tenant);
            }
            return documentout;
        }

        //public void MapDocumentOutPMDocumentOut(DocumentOutPM docPM, DocumentOut doc)
        //{
        //    doc.DocumentTypeId = docPM.DocumentTypeId;
        //    doc.ChildEntityReference = docPM.ChildEntityReference;
        //    doc.ChildEntityId = docPM.ChildEntityId;
        //    doc.Issued = docPM.Issued;
        //    doc.IssuedByUserId = docPM.IssuedByUserId;
        //    doc.IssuedDate = docPM.IssuedDate;
        //    doc.ObjectTableId = docPM.ObjectTableId;
        //    doc.EntityId = docPM.EntityId;
        //    doc.Note = docPM.Note;
        //    doc.Tenant = docPM.Tenant;
        //    doc.EditableFields = docPM.EditableFields;
        //    doc.DocumentTemplateId = docPM.DocumentTemplateId;
        //    doc.EmailTemplateId = docPM.EmailTemplateId;
        //    doc.XamlDocumentId = docPM.XamlDocumentId;
        //    doc.NeedsRebuild = docPM.NeedsRebuild;
        //    doc.IsDuplex = docPM.IsDuplex;
        //}

        //public void MapDocumentOutCopyDocumentOutCopyPM(DocumentOutCopyPM documentOutCopyPM, DocumentOutCopy documentOutCopy)
        //{
        //    documentOutCopy.DocumentId = documentOutCopyPM.DocumentId;
        //    documentOutCopy.DocumentOutId = documentOutCopyPM.DocumentOutId;
        //    documentOutCopy.DocumentTypeCopyId = documentOutCopyPM.DocumentTypeCopyId;
        //    documentOutCopy.Tenant = documentOutCopyPM.Tenant;
        //}

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [Invoke]
        public DocumentOutPM CreateDocumentOut(string documentTypeId, string entityId, string childEntityId, string childReference, string objectTableId, int tenant)
        {
            try
            {
                if (objectContext == null)
                {
                    objectContext = CommonDataContext.GetContext(tenant);
                }

                documentOutRepository = new DocumentOutRepository(objectContext);
                documentOutQuery = new DocumentOutQuery(documentOutRepository);

                DocumentOutPM documentOutPM = documentOutQuery.GetDocumentOutByDocumentTypeEntityAndChild(entityId, childEntityId, documentTypeId, tenant);
                if (documentOutPM == null)
                {
                    documentOutPM = CreateDocumentOutPM(documentTypeId, entityId, childEntityId, childReference, objectTableId, tenant);
                }
                return documentOutPM;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                string Error = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Error += "Entity of type " + eve.Entry.Entity.GetType().Name + " in state " + eve.Entry.State + " has the following validation errors:";
                    foreach (var ve in eve.ValidationErrors)
                    {
                        //Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                        //ve.PropertyName, ve.ErrorMessage);

                        Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                    }
                }


                string authenticateduser = "";

                try
                {
                    authenticateduser = Security.SecurityUtility.GetAuthenticatedUser();
                }

                catch
                {
                    authenticateduser = "UnKnown";
                }
                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }
                ExceptionHandler.HandleException(new Exception(Error), DateTime.Now, 0, "", authenticateduser, "", ip);
                throw new Exception(Error);
            }
        }

        private DocumentOutPM CreateDocumentOutPM(string documentTypeId, string entityId, string childEntityId, string childReference, string objectTableId, int tenant)
        {
            documentTypeRepository = new DocumentTypeRepository(objectContext);
 
            DocumentType documentType = documentTypeRepository.GetSingleDocumentTypes(documentTypeId, tenant);
            string documentTemplateId = null;
            string emailTemplateId = null;

            documentTemplateId = documentType.DocumentTypeDefaultReportTemplateId;
            emailTemplateId = documentType.DocumentTypeDefaultHTMLTemplateId;


            //---------------------------------------- islam
            documentsFilingRepository = new DocumentsFilingRepository(objectContext);
            string loggedUserEmail = AuthenticationUtil.GetAuthenticatedUser();
            UserRepository userRepository = new UserRepository(tenant);
            User loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, loggedUserEmail, tenant, true);
            DocumentsFiling newDocumentFiling = new DocumentsFiling() { DocumentTypeId = documentTypeId, EntityId = entityId, Tenant = tenant, ObjectTableId = objectTableId, ChildEntityId = childEntityId, ChildEntityReference = childReference, DirectionCode = "O" };

            newDocumentFiling.Id = IdCounter.GetNumber("Document", tenant).ToString();

            Random rnd = new Random();
            newDocumentFiling.SecurityId = newDocumentFiling.Id + RandomString(10);

            newDocumentFiling.Code = CodeCounter.GetNumber("DocumentsFiling", tenant).ToString();
            newDocumentFiling.CreatedByUserId = loggedUser.Id;
            newDocumentFiling.OwnerId = loggedUser.Id;
            newDocumentFiling.UpdatedByUserId = loggedUser.Id;
            newDocumentFiling.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            newDocumentFiling.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            newDocumentFiling.SearchFields = newDocumentFiling.Code + "," + newDocumentFiling.DirectionCode;
            documentsFilingRepository.Add(newDocumentFiling);

            //----------------------------------------
            DocumentOut newDocument = new DocumentOut() { EmailTemplateId = emailTemplateId, DocumentTemplateId = documentTemplateId, Tenant = tenant, Issued = false };
            newDocument.Id = newDocumentFiling.Id;
            documentOutRepository.Add(newDocument);

            objectContext.SaveChanges();

            DocumentOutPM docPM = documentOutQuery.GetSinglePM(newDocument.Id, newDocument.Tenant);
            return docPM;
        }

        public void UpdateDocumentOut(DocumentOutPM currentEntity)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentEntity.Tenant);
            }

             
            //documentOutRepository = new DocumentOutRepository(objectContext);
            //documentOutCopyRepository = new DocumentOutCopyRepository(objectContext);

            //DocumentOut doc = documentOutRepository.GetSingleDocumentOut(currentEntity.Id, currentEntity.Tenant);
            //MapDocumentOutPMDocumentOut(currentEntity, doc);

            List<DocumentOutCopyPM> documentOutCopyChangeSet = ChangeSet.GetAssociatedChanges(currentEntity, d => d.DocumentOutCopies).Cast<DocumentOutCopyPM>().ToList();
           
            #region DocumentOutCopies
            foreach (DocumentOutCopyPM r in documentOutCopyChangeSet)
            {
                ChangeOperation op = ChangeSet.GetChangeOperation(r);
                switch (op)
                {
                    case ChangeOperation.Insert:
                        {
                            r.changeOp = ChangeSetOperation.Insert;
                            //r.Id = IdCounter.GetNumber("Document", currentEntity.Tenant).ToString();
                            //DocumentOutCopy newDocumentOutCopy = new DocumentOutCopy();
                            //newDocumentOutCopy.Id = r.Id;
                            //MapDocumentOutCopyDocumentOutCopyPM(r, newDocumentOutCopy);
                            //documentOutCopyRepository.Add(newDocumentOutCopy);
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            r.changeOp = ChangeSetOperation.Update;
                            //DocumentOutCopy documentOutCopy = documentOutCopyRepository.GetSingleDocumentOutCopy(r.Id);
                            //MapDocumentOutCopyDocumentOutCopyPM(r, documentOutCopy);
                            //documentOutCopyRepository.Update(documentOutCopy);
                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            r.changeOp = ChangeSetOperation.Delete;
                            //DocumentOutCopy documentOutCopy = documentOutCopyRepository.GetSingleDocumentOutCopy(r.Id);
                            //documentOutCopyRepository.Remove(documentOutCopy);
                            break;
                        }
                    case ChangeOperation.None:
                        {
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            # endregion

          //  documentOutRepository.Update(doc);

            DocumentOutService service = new DocumentOutService(objectContext, currentEntity.Tenant);
            service.Update(currentEntity, documentOutCopyChangeSet);
            TableLastUpdateClass.UpdateTableHistory(currentEntity.Tenant, "DocumentOut");
        }

        public void DeleteDocumentOut(DocumentOut entity)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entity.Tenant);
            }
            documentOutRepository = new DocumentOutRepository(objectContext);
            DocumentOut doc = documentOutRepository.GetSingleDocumentOut(entity.Id, entity.Tenant);
            documentOutRepository.Remove(doc);
        }

        #endregion

        #region Documents
        public IQueryable<Document> GetDocuments(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentRepository = new DocumentRepository(tenant);
            return documentRepository.GetDocuments(0);
        }

        public IQueryable<Document> GetDocumentsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentRepository = new DocumentRepository(tenant);
            return documentRepository.GetDocuments(tenant);
        }

        public Document GetDocumentById(string documentId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentRepository = new DocumentRepository(tenant);
            return documentRepository.GetSingleDocument(tenant, documentId);
        }

        public void InsertDocument(Document entity)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entity.Tenant);
            }
            DocumentService service = new DocumentService(objectContext, entity.Tenant);
            service.Create(entity);

            //documentRepository = new DocumentRepository(objectContext);
            //entity.Id = IdCounter.GetNumber("Document", entity.Tenant).ToString();
            //documentRepository.Add(entity);

            TableLastUpdateClass.UpdateTableHistory(entity.Tenant, "Document");
        }

        public void UpdateDocument(Document currentEntity)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentEntity.Tenant);
            }
            DocumentService service = new DocumentService(objectContext, currentEntity.Tenant);
            service.Update(currentEntity);

            //documentRepository = new DocumentRepository(objectContext);
            //documentRepository.Update(currentEntity);

            TableLastUpdateClass.UpdateTableHistory(currentEntity.Tenant, "Document");
        }

        public void DeleteDocument(Document entity)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entity.Tenant);
            }
            documentRepository = new DocumentRepository(objectContext);
            documentRepository.Remove(entity);
        }

        [Invoke]
        public Document CreateDocument(Document document, string documentId, int tenant, string externalDocumentId, string receivedByUserId)
        {
            try
            {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }
            documentRepository = new DocumentRepository(objectContext);
            documentsFilingRepository = new DocumentsFilingRepository(objectContext);

            document.Id = documentId;

            document.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            document.Tenant = tenant;
            
            using (TransactionScope scope = TransactionFactory.GetTransaction())//TransactionFactory.GetTransaction())
            {
                documentRepository.Add(document);
                DocumentsFiling extDoc = documentsFilingRepository.GetSingleDocumentsFiling(externalDocumentId, tenant);
                extDoc.DocumentId = documentId;
                //extDoc.Received = true;
                extDoc.CreatedByUserId = receivedByUserId;
                extDoc.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                this.objectContext.SaveChanges();
                scope.Complete();
                return document;
            }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                string Error = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Error += "Entity of type " + eve.Entry.Entity.GetType().Name + " in state " + eve.Entry.State + " has the following validation errors:";
                    foreach (var ve in eve.ValidationErrors)
                    {
                        //Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                        //ve.PropertyName, ve.ErrorMessage);

                        Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                    }
                }


                string authenticateduser = "";

                try
                {
                    authenticateduser = Security.SecurityUtility.GetAuthenticatedUser();
                }

                catch
                {
                    authenticateduser = "UnKnown";
                }
                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }
                ExceptionHandler.HandleException(new Exception(Error), DateTime.Now, 0, "", authenticateduser, "", ip);
                throw new Exception(Error);
            }
        }

        [Invoke]
        public double? GetUsedSpaceForTenant(int tenant)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }
            documentRepository = new DocumentRepository(objectContext);
            double? usedSpace = documentRepository.GetUsedSpaceForTenant(tenant);
            return usedSpace;
        }
        #endregion

        #region DocumentTypeCusotmFields
        public IQueryable<DocumentTypeCustomField> GetDocumentTypeCustomFields(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeCustomFieldRepository = new DocumentTypeCustomFieldRepository(tenant);
            return documentTypeCustomFieldRepository.GetDocumentTypeCustomFields(0);
        }

        public DocumentTypeCustomFieldPM GetSingleDocumentTypeCustomField(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeCustomFieldQuery = new DocumentTypeCustomFieldQuery(tenant);
            return documentTypeCustomFieldQuery.GetSinglePM(id, tenant);
        }

        public IQueryable<DocumentTypeCustomFieldPM> GetDocumentTypeCustomFieldsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeCustomFieldQuery = new DocumentTypeCustomFieldQuery(tenant);
            return documentTypeCustomFieldQuery.GetDocumentTypeCustomFieldPMsByTenant(tenant);
        }

        public IQueryable<DocumentTypeCustomFieldPM> GetDocumentTypeCustomFieldsByDocument(int tenant, string documentTypeId)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeCustomFieldQuery = new DocumentTypeCustomFieldQuery(tenant);
            return documentTypeCustomFieldQuery.GetDocumentTypeCusotmFieldPMsByDocumentTypeId(documentTypeId, tenant);
        }

        public void InsertDocumentTypeCustomField(DocumentTypeCustomFieldPM entityPM)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            documentTypeCustomFieldRepository = new DocumentTypeCustomFieldRepository(objectContext);

            bool exist = (from a in documentTypeCustomFieldRepository.GetDocumentTypeCustomFields(entityPM.Tenant)
                          where a.FieldCode == entityPM.FieldCode && a.Tenant == entityPM.Tenant && a.DocumentTypeId == entityPM.DocumentTypeId
                          select a).Any();
            if (!exist)
            {
                DocumentTypeCustomFieldService service = new DocumentTypeCustomFieldService(objectContext, entityPM.Tenant);
                service.Create(entityPM);
                TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "DocumentTypeCustomField");
            }

            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", entityPM.Tenant);
                msg = msg.Replace("%Entity", "Field");
                throw new Exception(msg);
            }
        }

        public void UpdateDocumentTypeCustomField(DocumentTypeCustomFieldPM entityPM)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            DocumentTypeCustomFieldService service = new DocumentTypeCustomFieldService(objectContext, entityPM.Tenant);
            service.Update(entityPM);
            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "DocumentTypeCustomField");
        }

        public void DeleteDocumentTypeCustomField(DocumentTypeCustomFieldPM entity)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entity.Tenant);
            }
            documentTypeCustomFieldRepository = new DocumentTypeCustomFieldRepository(objectContext);

            DocumentTypeCustomField documentTypeCustomField = documentTypeCustomFieldRepository.GetSingleDocumentTypeCusotmField(entity.Id, entity.Tenant);
            documentTypeCustomFieldRepository.Remove(documentTypeCustomField);
        }
        #endregion

        #region TemplateFormats
        public IQueryable<TemplateFormat> GetTemplateFormats(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            templateFormatRepository = new TemplateFormatRepository(tenant);
            return templateFormatRepository.GetTemplateFormats();
        }

        public IQueryable<TemplateFormatPM> GetTemplateFormatsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            templateFormatQuery = new TemplateFormatQuery(tenant);
            return templateFormatQuery.GetTemplateFormatPMs();
        }

        public IQueryable<TemplateFormat> GetFirstTemplateFormats(string input, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            templateFormatRepository = new TemplateFormatRepository(tenant);
            input = input.ToUpper();
            return templateFormatRepository.GetTemplateFormats().Where(p => p.Code.ToUpper().StartsWith(input) || p.Name.ToUpper().StartsWith(input));
        }

        public TemplateFormatList GetSingleTemplateFormatList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            templateFormatRepository = new TemplateFormatRepository(tenant);
            
            TemplateFormat templateFormat = templateFormatRepository.GetSingleTemplateFormat(code);
            TemplateFormatList templateFormatList = null;

            if (templateFormat != null)
            {
                List<TemplateFormat> singleEntityList = new List<TemplateFormat>();
                singleEntityList.Add(templateFormat);

                templateFormatQuery = new TemplateFormatQuery(templateFormatRepository);
                IQueryable<TemplateFormat> iQueryable = singleEntityList.AsQueryable();
                IQueryable<TemplateFormatList> iQueryableEntityList = templateFormatQuery.GetIQueryableEntityList(iQueryable);
                templateFormatList = iQueryableEntityList.FirstOrDefault();
            }
            return templateFormatList;
        }

        public TemplateFormatPM GetSingleTemplateFormat(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            templateFormatQuery = new TemplateFormatQuery(tenant);
            return templateFormatQuery.GetSigleTemplateFormatPM(code);
        }

        public IQueryable<TemplateFormatList> GetChargeGroupLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            templateFormatRepository = new TemplateFormatRepository(tenant);
            templateFormatQuery = new TemplateFormatQuery(templateFormatRepository);

            IQueryable<TemplateFormat> iQueryable = templateFormatRepository.GetTemplateFormats();
            IQueryable<TemplateFormatList> query2 = templateFormatQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<TemplateFormatList> GetTemplateFormatFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            templateFormatRepository = new TemplateFormatRepository(tenant);
            templateFormatQuery = new TemplateFormatQuery(templateFormatRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<TemplateFormat> templateFormats = templateFormatRepository.GetTemplateFormats();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            templateFormats = filter.GetFilteredQuery<TemplateFormat>(nonListQueryOperation, templateFormats);

            int skippedChargregroups = queryOperations.PageIndex;
            IQueryable<TemplateFormatList> query2 = templateFormatQuery.GetIQueryableEntityList(templateFormats);
            query2 = filter.GetFilteredQuery<TemplateFormatList>(listQueryOperation, query2);
            
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(TemplateFormatList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("TemplateFormat", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<TemplateFormatList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<TemplateFormatList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<TemplateFormatList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<TemplateFormatList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<TemplateFormatList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Code);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }
            
            query2 = query2.Skip(skippedChargregroups);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetTemplateFormatFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            templateFormatRepository = new TemplateFormatRepository(tenant);
            templateFormatQuery = new TemplateFormatQuery(templateFormatRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<TemplateFormat> templateFormats = templateFormatRepository.GetTemplateFormats();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            templateFormats = filter.GetFilteredQuery<TemplateFormat>(nonListQueryOperation, templateFormats);
            int skippedVatTypes = queryOperations.PageIndex;

            IQueryable<TemplateFormatList> query2 = templateFormatQuery.GetIQueryableEntityList(templateFormats);
            query2 = filter.GetFilteredQuery<TemplateFormatList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public void InsertTemplateFormat(TemplateFormat entity)
        {
            templateFormatRepository.Add(entity);
        }

        public void UpdateTemplateFormat(TemplateFormat currentEntity)
        {
            templateFormatRepository.Update(currentEntity);
        }

        public void DeleteTemplateFormat(TemplateFormat entity)
        {
            templateFormatRepository.Remove(entity);
        }
        #endregion

        #region DocumentTypeTemplates
        public IQueryable<DocumentTypeTemplate> GetDocumentTypeTemplates(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeTemplateRepository = new DocumentTypeTemplateRepository(tenant);
            return documentTypeTemplateRepository.GetDocumentTypeTemplatesByTenant(0);
        }

        public DocumentTypeTemplatePM GetSingleDocumentTypeTemplate(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeTemplateQuery = new DocumentTypeTemplateQuery(tenant);
            return documentTypeTemplateQuery.GetSingleDocumentTypeTemplatePM(id);
        }

        public DocumentTypeTemplateList GetSingleDocumentTypeTemplateList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeTemplateRepository = new DocumentTypeTemplateRepository(tenant);
            DocumentTypeTemplate documentTypeTemplate = documentTypeTemplateRepository.GetSingleDocumentTypeTemplate(id);
            DocumentTypeTemplateList documentTypeTemplateList = null;

            if (documentTypeTemplate != null)
            {
                List<DocumentTypeTemplate> singleEntityList = new List<DocumentTypeTemplate>();
                singleEntityList.Add(documentTypeTemplate);

                documentTypeTemplateQuery = new DocumentTypeTemplateQuery(documentTypeTemplateRepository);
                IQueryable<DocumentTypeTemplate> iQueryable = singleEntityList.AsQueryable();
                IQueryable<DocumentTypeTemplateList> iQueryableEntityList = documentTypeTemplateQuery.GetIQueryableEntityList(iQueryable);
                documentTypeTemplateList = iQueryableEntityList.FirstOrDefault();
            }
            return documentTypeTemplateList;
        }

        public List<DocumentTypeTemplatePM> GetDocumentTypeTemplatesForDocumentType(string documentTypeId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeTemplateQuery = new DocumentTypeTemplateQuery(tenant);
            return documentTypeTemplateQuery.GetDocumentTypeTemplatesByDocumentTypeId(documentTypeId, tenant).ToList();
        }

        //public List<DocumentTypeTemplatePM> GetDocumentTypeTemplatesByDocumentTypeIdForAutomations(string documentTypeId, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    DocumentTypeTemplateQuery documentTypeTemplateQuery = new DocumentTypeTemplateQuery(tenant);
        //    return documentTypeTemplateQuery.GetDocumentTypeTemplatesByDocumentTypeIdForAutomation(documentTypeId, tenant);
        //}

        public List<DocumentTypeTemplateList> GetDocumentTypeTemplateListsForDocumentType(string documentTypeId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeTemplateQuery = new DocumentTypeTemplateQuery(tenant);
            return documentTypeTemplateQuery.GetDocumentTypeTemplateListsByDocumentTypeId(documentTypeId, tenant);
        }

        public IQueryable<DocumentTypeTemplateList> GetDocumentTypeTemplateLists(int tenant)
        {
            documentTypeTemplateRepository = new DocumentTypeTemplateRepository(tenant);
            documentTypeTemplateQuery = new DocumentTypeTemplateQuery(documentTypeTemplateRepository);

            IQueryable<DocumentTypeTemplate> iQueryable = documentTypeTemplateRepository.GetDocumentTypeTemplatesByTenant(tenant);
            IQueryable<DocumentTypeTemplateList> query2 = documentTypeTemplateQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<DocumentTypeTemplateList> GetDocumentTypeTemplateFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeTemplateRepository = new DocumentTypeTemplateRepository(tenant);
            documentTypeTemplateQuery = new DocumentTypeTemplateQuery(documentTypeTemplateRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<DocumentTypeTemplate> iQueryable = documentTypeTemplateRepository.GetDocumentTypeTemplatesByTenant(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<DocumentTypeTemplate>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<DocumentTypeTemplateList> query2 = documentTypeTemplateQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<DocumentTypeTemplateList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(DocumentTypeTemplateList).GetProperty(queryOperations.SortByColumnName);
                switch (propInfo.PropertyType.Name.ToLower())
                {
                    case "string":
                        {
                            query2 = sortClass.GetSorterQuery<DocumentTypeTemplateList, string>(queryOperations, query2);
                            break;
                        }
                    case "double":
                        {
                            query2 = sortClass.GetSorterQuery<DocumentTypeTemplateList, double>(queryOperations, query2);
                            break;
                        }
                    case "datetime":
                        {
                            query2 = sortClass.GetSorterQuery<DocumentTypeTemplateList, DateTime>(queryOperations, query2);
                            break;
                        }
                    case "int":
                        {
                            query2 = sortClass.GetSorterQuery<DocumentTypeTemplateList, int>(queryOperations, query2);
                            break;
                        }
                    case "boolean":
                        {
                            query2 = sortClass.GetSorterQuery<DocumentTypeTemplateList, bool>(queryOperations, query2);
                            break;
                        }
                    default:
                        {
                            query2 = query2.OrderByDescending(d => d.Name);
                            break;
                        }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Name);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetDocumentTypeTemplateFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeTemplateRepository = new DocumentTypeTemplateRepository(tenant);
            documentTypeTemplateQuery = new DocumentTypeTemplateQuery(documentTypeTemplateRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<DocumentTypeTemplate> iQueryable = documentTypeTemplateRepository.GetDocumentTypeTemplatesByTenant(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<DocumentTypeTemplate>(nonListQueryOperation, iQueryable);

            IQueryable<DocumentTypeTemplateList> query2 = documentTypeTemplateQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<DocumentTypeTemplateList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public IQueryable<DocumentTypeTemplatePM> GetDocumentTypeTemplatesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeTemplateQuery = new DocumentTypeTemplateQuery(tenant);
            return documentTypeTemplateQuery.GetDocumentTypeTemplatePMsByTenant(tenant);
        }

        //public List<DocumentTypeTemplateList> GetDocumentTypeTemplatesFromLibraryByTenant(int tenant, string documentTypeId, bool isfilter, int mytenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);

        //    documentTypeTemplateQuery = new DocumentTypeTemplateQuery(tenant);

        //    if (isfilter)
        //    {

        //        return documentTypeTemplateQuery.GetDocumentTypeTemplateFromLibraryByTenant(tenant, documentTypeId, mytenant);
        //    }
        //    else
        //    {
        //        return documentTypeTemplateQuery.GetDocumentTypeTemplateFromLibraryByTenantWithOutFilter(tenant, documentTypeId);
        //    }
        //}

        //public List<DocumentTypeTemplatePM> GetDocumentTypeTemplatesPMFromLibraryByTenant(int tenant, string documentTypeId, bool isfilter, int mytenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);

        //    documentTypeTemplateQuery = new DocumentTypeTemplateQuery(tenant);

        //    if (isfilter)
        //    {

        //        return documentTypeTemplateQuery.GetDocumentTypeTemplateFromLibraryPMsByTenant(tenant, documentTypeId, mytenant);
        //    }
        //    else
        //    {
        //        return documentTypeTemplateQuery.GetDocumentTypeTemplateFromLibraryPMsByTenantWithOutFilter(tenant, documentTypeId);
        //    }
        //}
      
       public void UpdateDocumentTypeTemplate(DocumentTypeTemplateList currentEntity)
       {

       }

        public void InsertDocumentTypeTemplate(DocumentTypeTemplatePM entityPM)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            if (entityPM.EditorTool != "S")
            {
                if (entityPM.TemplateBody != null)
                {
                    HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
                    entityPM = htmlEditorHelper.UpdateHeaderAndFooterAndBodyHtmlDocumentTypeTemplatePM(entityPM);
                }
            }


            DocumentTypeTemplateService service = new DocumentTypeTemplateService(objectContext, entityPM.Tenant);
            service.Create(entityPM);
        }

        public void UpdateDocumentTypeTemplate(DocumentTypeTemplatePM entityPM)
        {
            if (entityPM.EditorTool != "S")
            {
                if (entityPM.TemplateBody != null)
                {
                    HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
                    entityPM = htmlEditorHelper.UpdateHeaderAndFooterAndBodyHtmlDocumentTypeTemplatePM(entityPM);
                }
            }
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            DocumentTypeTemplateService service = new DocumentTypeTemplateService(objectContext, entityPM.Tenant);
            service.Update(entityPM);
        }



        public void DeleteDocumentTypeTemplate(DocumentTypeTemplatePM entity)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entity.Tenant);
            }

            documentTypeTemplateRepository = new DocumentTypeTemplateRepository(objectContext);
            DocumentTypeTemplate deletedEntity = documentTypeTemplateRepository.GetSingleDocumentTypeTemplate(entity.Id);
            documentTypeTemplateRepository.Remove(deletedEntity);
        }



        //public List<DocumentTypeTemplateList> GetAllDocumentTypeTemplatesByObjectTableId(string objecttableid, int tenant, bool isfilter, string transportModeId, string shipmentLevelCode)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    documentTypeTemplateQuery = new DocumentTypeTemplateQuery(tenant);
        //    if (isfilter)
        //    {
        //        return documentTypeTemplateQuery.GetDocumentTypeTemplatesByObjectTableId(objecttableid, tenant, transportModeId, shipmentLevelCode);
        //    }
        //    else
        //    {
        //        return documentTypeTemplateQuery.GetDocumentTypeTemplatesByObjectTableIdWithoutFilter(objecttableid, tenant, transportModeId, shipmentLevelCode);
        //    }
        //}



        [Invoke]
        public string CopyDocumentTypeAndDocumentTypTemplate(string docmentTypeTemplateId, int tenant)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }


            string CopyDocumentTypeCode = "";
            DocumentTypeCopyRepository theDocumentTypeCopyRepository = new DocumentTypeCopyRepository(tenant);
            DocumentTypeCustomFieldRepository documentTypeCustomFieldRepository = new DocumentTypeCustomFieldRepository(tenant);

            DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(tenant);
            DocumentTypeTemplateRepository documentTypeTemplateRepository = new DocumentTypeTemplateRepository(tenant);

            DocumentTypeTemplate documentTypeTemplate = documentTypeTemplateRepository.GetSingleDocumentTypeTemplate(docmentTypeTemplateId, 0);


          //  DocumentType documentType = documentTypeRepository.GetSingleDocumentTypes(documentTypeTemplate.DocumentTypeId, 0);
            DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);

            DocumentTypeCustomFieldQuery documentTypeCustomFieldQuery = new DocumentTypeCustomFieldQuery(tenant);

            DocumentTypePM documentType = documentTypeQuery.GetSinglePMByCodeAndTenant(documentTypeTemplate.DocumentType.Code, 0);
          

            if (documentType != null)
            {
                DocumentType itemDocumentType = new DocumentType()
                {
                    Id = IdCounter.GetNumber("DocumentType", tenant).ToString(),
                    Tenant = tenant,
                    Name = documentType.Name,
                    Code = documentType.Code,
                    Notes = documentType.Notes,
                    IsAir = documentType.IsAir,
                    IsOcean = documentType.IsOcean,
                    IsInland = documentType.IsInland,
                    IsDocIn = documentType.IsDocIn,
                    IsDocOut = documentType.IsDocOut,
                    ObjectTableId = documentType.ObjectTableId,
                    Subject = documentType.Subject,
                   // DocumentTypeDefaultReportTemplateId = documentType.DocumentTypeDefaultReportTemplateId,
                   // DocumentTypeDefaultHTMLTemplateId = documentType.DocumentTypeDefaultHTMLTemplateId,
                    TemplateFormatCode = documentType.TemplateFormatCode,
                  //  DocumentTypeDefaultEditorTool = documentType.DocumentTypeDefaultEditorTool,
                    InActive = false,
                    IsMaster = documentType.IsMaster,
                    IsDirect = documentType.IsDirect,
                    IsHouse = documentType.IsHouse,
                    SearchFields = documentType.SearchFields,
                    CustomControl = documentType.CustomControl,
                    CustomerRoleId = documentType.CustomerRoleId,
                    AgentRoleId = documentType.AgentRoleId,
                    IsCustomerView = documentType.IsCustomerView,
                    IsAgentView = documentType.IsAgentView,
                    IsReadOnly = documentType.IsReadOnly,
                   LimitedPrintCopyId = documentType.LimitedPrintCopyId,
                    IsDocumentOneTimePrintLimited = documentType.IsDocumentOneTimePrintLimited,
                    IsCopiedAtSignup = true,
                    IsEnabledForCustomers = true,
                    CountryCode = documentType.CountryCode,
                    DocumentTypeCategoryCode = documentType.DocumentTypeCategoryCode,
                    IsSystemAdditionalPrintingFields = documentType.IsSystemAdditionalPrintingFields,
                    PrintingFieldsScreenCode = documentType.PrintingFieldsScreenCode,

                };


                CopyDocumentTypeCode = itemDocumentType.Code;

                foreach (DocumentTypeCopyPM copy in documentType.DocumentTypeCopies)
                {
                    
                    
                        DocumentTypeCopy newCopy = new DocumentTypeCopy()
                        {
                            Id = IdCounter.GetNumber("DocumentTypeCopy", tenant).ToString(),
                            Code = copy.Code,
                            Name = copy.Name,
                            Tenant = tenant,
                            IndexOrder = copy.IndexOrder,
                            IsSelectedByDefault = copy.IsSelectedByDefault,
                            DocumentTypeId = itemDocumentType.Id,
                          
                        };
                        theDocumentTypeCopyRepository.Add(newCopy);
                    
                }


                IQueryable<DocumentTypeCustomField> zeroCustomFields = documentTypeCustomFieldRepository.GetDocumentTypeCusotmFieldsByDocumentTypeId(itemDocumentType.Id, 0);
             //   List<DocumentTypeCustomField> zeroCustomFields = documentTypeCustomFieldQuery.GetDocumentTypeCusotmFieldListByDocumentTypeId(itemDocumentType.Id, 0);
         
              foreach (DocumentTypeCustomField customField in zeroCustomFields)
              {
                  DocumentTypeCustomField newCustomField = new DocumentTypeCustomField()
                  {
                      DocumentTypeId = itemDocumentType.Id,
                      DefaultValue = customField.DefaultValue,
                      FieldCode = customField.FieldCode,
                      FieldDataTypeCode = customField.FieldDataTypeCode,
                      Id = IdCounter.GetNumber("DocumentTypeCustomField", tenant).ToString(),
                        InActive = false,
                      IndexOrder = customField.IndexOrder,
                      IsRequired = customField.IsRequired,
                      MultiLine = customField.MultiLine,
                      Name = customField.Name,
                      Tenant = tenant,
                  };
                  documentTypeCustomFieldRepository.Add(newCustomField);
              }





                documentTypeRepository.Add(itemDocumentType);


            
      

                DocumentTypeTemplate itemDocumentTypeTemplate = new DocumentTypeTemplate()
                {
                    Description = documentTypeTemplate.Description,
                    DocumentTypeId = itemDocumentType.Id,
                    Id = IdCounter.GetNumber("DocumentTypeTemplate", tenant).ToString(),
                    LastUpdateDate = documentTypeTemplate.LastUpdateDate,
                    LastUpdatedByUserId = documentTypeTemplate.LastUpdatedByUserId,
                    TemplateBody = documentTypeTemplate.TemplateBody,
                    TemplateType = documentTypeTemplate.TemplateType,
                    Tenant = tenant,

                    InActive = false,
                    EditorTool = documentTypeTemplate.EditorTool,
                    HorizontalShift = documentTypeTemplate.HorizontalShift,
                    VerticalShift = documentTypeTemplate.VerticalShift,
                    Subject = documentTypeTemplate.Subject,
                    CountryCode = documentTypeTemplate.CountryCode,
                    InternalRemarks = documentTypeTemplate.InternalRemarks,
                    Language = documentTypeTemplate.Language,
                    OriginalTemplateId = documentTypeTemplate.Id,
                    IsCopiedAtSignup = true,
                    IsEnabledForCustomers = true,
                    IsSystem = documentTypeTemplate.IsSystem,
                    
                };
                documentTypeTemplateRepository.Add(itemDocumentTypeTemplate);



                if (itemDocumentTypeTemplate.TemplateType == "M")
                {
                 itemDocumentType.DocumentTypeDefaultHTMLTemplateId = itemDocumentTypeTemplate.Id;
                 itemDocumentType.TemplateFormatCode = "M";
               
                 itemDocumentType.DocumentTypeDefaultEditorTool = itemDocumentTypeTemplate.EditorTool;
                }
                else
                {
                    itemDocumentType.DocumentTypeDefaultReportTemplateId = itemDocumentTypeTemplate.Id;
                    itemDocumentType.DocumentTypeDefaultEditorTool = itemDocumentTypeTemplate.EditorTool;
                    itemDocumentType.TemplateFormatCode = "P";
                    
                }

               

                


                documentTypeRepository.SubmitChanges();
                theDocumentTypeCopyRepository.SubmitChanges();
                documentTypeCustomFieldRepository.SubmitChanges();
                documentTypeTemplateRepository.SubmitChanges();

                //DocumentTypeTemplateService service = new DocumentTypeTemplateService(objectContext, itemDocumentTypeTemplatePM.Tenant);
                //service.Create(itemDocumentTypeTemplatePM);
               // documentTypeTemplateRepository.Add(itemDocumentTypeTemplate as DocumentTypeTemplate );

             
                
               // documentTypeTemplateRepository.SubmitChanges();
            }
            return CopyDocumentTypeCode;
        }

        #endregion

        #region DocumentsFilings

        public IQueryable<DocumentsFilingList> GetRecentDocumentsFilings(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentsFilingRepository = new DocumentsFilingRepository(tenant);
            documentsFilingQuery = new DocumentsFilingQuery(documentsFilingRepository);

            IQueryable<DocumentsFiling> documentsFilings = documentsFilingRepository.GetDocumentsFilings(tenant);
            IQueryable<DocumentsFilingList> query2 = documentsFilingQuery.GetIQueryableEntityList(documentsFilings);
            return query2;
        }

        public DocumentsFilingList GetSingleDocumentsFilingList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentsFilingRepository = new DocumentsFilingRepository(tenant);
            DocumentsFiling documentsFiling = documentsFilingRepository.GetSingleDocumentsFiling(id, tenant);
            DocumentsFilingList documentsFilingList = null;

            if (documentsFiling != null)
            {
                List<DocumentsFiling> singleEntityList = new List<DocumentsFiling>();
                singleEntityList.Add(documentsFiling);

                documentsFilingQuery = new DocumentsFilingQuery(documentsFilingRepository);
                IQueryable<DocumentsFiling> iQueryable = singleEntityList.AsQueryable();
                IQueryable<DocumentsFilingList> iQueryableEntityList = documentsFilingQuery.GetIQueryableEntityList(iQueryable);
                documentsFilingList = iQueryableEntityList.FirstOrDefault();

               



            }
            return documentsFilingList;
        }

        public IQueryable<DocumentsFilingList> GetDocumentsFilingLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentsFilingRepository = new DocumentsFilingRepository(tenant);
            documentsFilingQuery = new DocumentsFilingQuery(documentsFilingRepository);

            IQueryable<DocumentsFiling> documentsFilings = documentsFilingRepository.GetDocumentsFilings(tenant);
            IQueryable<DocumentsFilingList> query2 = documentsFilingQuery.GetIQueryableEntityList(documentsFilings);
            return query2;
        }
      


        public DocumentsFilingPM GetSingleDocumentsFiling(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentsFilingQuery = new DocumentsFilingQuery(tenant);
            return this.documentsFilingQuery.GetSinglePM(id, tenant);
        }

        public void UpdateDocumentsFilingList(DocumentsFilingList currentEntity)
        {
        }

        [Query(HasSideEffects = true)]
        public IQueryable<DocumentsFilingList> GetDocumentsFilingFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentsFilingRepository = new DocumentsFilingRepository(tenant);
            documentsFilingQuery = new DocumentsFilingQuery(documentsFilingRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<DocumentsFiling> documentsFilings = null;
            IQueryable<DocumentsFilingsView> documentsFilingsViews = null;

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            if (LogitudeSettings.WorkEnvironment != "customs")
            {
                documentsFilings = documentsFilingRepository.GetDocumentsFilings(tenant);
                documentsFilings = filter.GetFilteredQuery<DocumentsFiling>(nonListQueryOperation, documentsFilings);
            }
            else
            {
                documentsFilingsViews = documentsFilingRepository.GetDocumentsFilingsViews(tenant);
                documentsFilingsViews = filter.GetFilteredQuery<DocumentsFilingsView>(nonListQueryOperation, documentsFilingsViews);
            }

            int skippedDocumentsFilings = queryOperations.PageIndex;

            IQueryable<DocumentsFilingList> query2 = null;
            if (LogitudeSettings.WorkEnvironment != "customs")
            {
                query2 = documentsFilingQuery.GetIQueryableEntityList(documentsFilings);
            }
            else
            {
                query2 = documentsFilingQuery.GetIQueryableEntityList(documentsFilingsViews);
            }

            query2 = filter.GetFilteredQuery<DocumentsFilingList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(DocumentsFilingList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("DocumentsFiling", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();
                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<DocumentsFilingList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<DocumentsFilingList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<DocumentsFilingList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<DocumentsFilingList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<DocumentsFilingList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Code);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedDocumentsFilings);
            query2 = query2.Take(queryOperations.PageSize);


            //List<DocumentsFilingList> objectsList = query2.ToList();
            //foreach (var list in objectsList)
            //{
            //    ObjectTablePM pm = ObjectTabelQuery.GetSingleObjectTableById(list.ObjectTableId, list.Tenant);
            //    list.ObjectTableName = pm.Name;

            //}

            //query2 = objectsList.AsQueryable();

            return query2;
        }

        public int GetDocumentsFilingFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentsFilingRepository = new DocumentsFilingRepository(tenant);
            documentsFilingQuery = new DocumentsFilingQuery(documentsFilingRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<DocumentsFiling> documentsFilings = null;
            IQueryable<DocumentsFilingsView> documentsFilingsViews = null;

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            if (LogitudeSettings.WorkEnvironment != "customs")
            {
                documentsFilings = documentsFilingRepository.GetDocumentsFilings(tenant);
                documentsFilings = filter.GetFilteredQuery<DocumentsFiling>(nonListQueryOperation, documentsFilings);
            }
            else
            {
                documentsFilingsViews = documentsFilingRepository.GetDocumentsFilingsViews(tenant);
                documentsFilingsViews = filter.GetFilteredQuery<DocumentsFilingsView>(nonListQueryOperation, documentsFilingsViews);
            }
           
           
            int skippedDocumentsFilings = queryOperations.PageIndex;
            IQueryable<DocumentsFilingList> query2 =null;
            if (LogitudeSettings.WorkEnvironment != "customs")
            {
                query2 = documentsFilingQuery.GetIQueryableEntityList(documentsFilings);
            }
            else
            {
                query2 = documentsFilingQuery.GetIQueryableEntityList(documentsFilingsViews);
            }

            query2 = filter.GetFilteredQuery<DocumentsFilingList>(listQueryOperation, query2);
           
            
            int count = query2.Count();
            return count;
        } 

        public IQueryable<DocumentsFiling> GetDocumentsFilings(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }

            documentsFilingRepository = new DocumentsFilingRepository(objectContext);

            return documentsFilingRepository.GetDocumentsFilings(tenant);
        }

        //public List<DocumentsFilingPM> GetDocumentsFilingsByTenant(string directionCode, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);

        //    documentsFilingQuery = new DocumentsFilingQuery(tenant);
        //    return documentsFilingQuery.GetDocumentsFilingPMs(directionCode, tenant);
        //}

        public DocumentsFilingPM GetDocumentsFilingById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentsFilingQuery = new DocumentsFilingQuery(tenant);
            return this.documentsFilingQuery.GetSinglePM(id, tenant);
        }

        public List<DocumentsFilingPM> GetDocumentsFilingsByEntityId(string entityId, string directionCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentsFilingQuery = new DocumentsFilingQuery(tenant);
            return documentsFilingQuery.GetDocumentsFilingPMsByEntityId(entityId, directionCode, tenant);
        }

        public List<DocumentsFilingPM> GetDocumentsFilingsByEntityIdAndObjectTable(string entityId, string childEntityId, string objectTableId, string directionCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentsFilingQuery = new DocumentsFilingQuery(tenant);
            return documentsFilingQuery.GetDocumentsFilingPMsByEntityIdAndObjectTable(entityId, childEntityId, objectTableId, directionCode, tenant);
        }

        public List<DocumentsFilingPM> GetDocumentsFilingsWithDocuments(string entityId, string childEntityId, string objectTableId, string directionCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentsFilingQuery = new DocumentsFilingQuery(tenant);
            return documentsFilingQuery.GetDocumentsFilingPMsByEntityIdAndObjectTable(entityId, childEntityId, objectTableId, directionCode, tenant).Where(d => d.DocumentId != null && d.HasFile == true).ToList();
        }

        public DocumentsFilingPM GetDocumentsFilingsByTenantAndID(int tenant, string id)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentsFilingQuery = new DocumentsFilingQuery(tenant);
            return documentsFilingQuery.GetSinglePM(id, tenant);
        }

        //public List<DocumentsFilingPM> GetDocumentsFilingsByDocumentType(string docType, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);

        //    documentsFilingQuery = new DocumentsFilingQuery(tenant);
        //    return documentsFilingQuery.GetDocumentsFilingPMs(tenant).Where(d => d.DocumentTypeCode == docType && d.Tenant == tenant).ToList();
        //}

        public DocumentsFilingPM GetSingleDocumentsFilingPM(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentsFilingQuery = new DocumentsFilingQuery(tenant);
            DocumentsFilingPM newExtDoc = documentsFilingQuery.GetSinglePM(id, tenant);
            return newExtDoc;
        }

        public DocumentsFilingPM GetSingleDocumentsFilingByDocumentType(string documentTypeId, string objectTableId, string entityId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentsFilingQuery = new DocumentsFilingQuery(tenant);
            DocumentsFilingPM newExtDoc = documentsFilingQuery.GetDocumentsFilingByDocumentType(documentTypeId, objectTableId, entityId, tenant);
            return newExtDoc;
        }

        public DocumentsFilingPM GetSingleDocumentsFilingByChild(string documentTypeId, string paymentNumber, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentsFilingQuery = new DocumentsFilingQuery(tenant);
            DocumentsFilingPM newExtDoc = documentsFilingQuery.GetDocumentsFilingByChild(documentTypeId, paymentNumber, tenant);
            return newExtDoc;
        }

  

        public DocumentsFilingPM GetSingleDocumentsFilingByChildId(string documentTypeId, string childId, string childObjectTableId, string childEntityReference, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentsFilingQuery = new DocumentsFilingQuery(tenant);
            DocumentsFilingPM newExtDoc = documentsFilingQuery.GetDocumentsFilingByChildId(documentTypeId, childId, childObjectTableId, childEntityReference, tenant);
            return newExtDoc;
        }

        [Invoke]
        public DocumentsFilingPM CreateDocumentsFiling(string documentTypeId, string entityId, string childEntityId, string childReference, string objectTableId, string directionCode, int tenant)
        {
            try
            {
                if (objectContext == null)
                {
                    objectContext = CommonDataContext.GetContext(tenant);
                }

                documentsFilingRepository = new DocumentsFilingRepository(objectContext);
                documentRepository = new DocumentRepository(objectContext);
                documentsFilingQuery = new DocumentsFilingQuery(documentsFilingRepository);

                string loggedUserEmail = AuthenticationUtil.GetAuthenticatedUser();
                UserRepository userRepository = new UserRepository(tenant);
                User loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, loggedUserEmail, tenant, true);
                DocumentsFilingPM newDocument = new DocumentsFilingPM() { DocumentTypeId = documentTypeId, EntityId = entityId, Tenant = tenant, ObjectTableId = objectTableId, ChildEntityId = childEntityId, ChildEntityReference = childReference, DirectionCode = directionCode };
                newDocument.SearchFields = newDocument.Code + "," + newDocument.DirectionCode + "," + loggedUser.Contact.EnglishName + "," + loggedUser.Contact.LocalName;
                //newDocument.Id = IdCounter.GetNumber("Document", tenant).ToString();
                //newDocument.Code = CodeCounter.GetNumber("DocumentsFiling", tenant).ToString();
                //newDocument.CreatedByUserId = loggedUser.Id;
                //newDocument.OwnerId = loggedUser.Id;
                //newDocument.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                //newDocument.UpdatedByUserId = loggedUser.Id;
                //newDocument.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
               // documentsFilingRepository.Add(newDocument);
                //documentsFilingRepository.SubmitChanges();
             
                DocumentsFilingService service = new DocumentsFilingService(objectContext, tenant);
                service.Create(newDocument, null);

                DocumentsFilingPM extDocPM = documentsFilingQuery.GetSinglePM(newDocument.Id, newDocument.Tenant);


                return extDocPM;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                string Error = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Error += "Entity of type " + eve.Entry.Entity.GetType().Name + " in state " + eve.Entry.State + " has the following validation errors:";
                    foreach (var ve in eve.ValidationErrors)
                    {
                        //Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                        //ve.PropertyName, ve.ErrorMessage);

                        Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                    }
                }


                string authenticateduser = "";

                try
                {
                    authenticateduser = Security.SecurityUtility.GetAuthenticatedUser();
                }

                catch
                {
                    authenticateduser = "UnKnown";
                }
                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }
                ExceptionHandler.HandleException(new Exception(Error), DateTime.Now, 0, "", authenticateduser, "", ip);
                throw new Exception(Error);
            }
        }


        public void InsertDocumentsFiling(DocumentsFilingPM newDocument)
        {
            try
            {
                if (objectContext == null)
                {
                    objectContext = CommonDataContext.GetContext(newDocument.Tenant);
                }

                documentsFilingRepository = new DocumentsFilingRepository(objectContext);
                documentRepository = new DocumentRepository(objectContext);
                documentsFilingQuery = new DocumentsFilingQuery(documentsFilingRepository);

                string loggedUserEmail = AuthenticationUtil.GetAuthenticatedUser();
                UserRepository userRepository = new UserRepository(newDocument.Tenant);
                User loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, loggedUserEmail, newDocument.Tenant, true);
               
                newDocument.SearchFields = newDocument.Code + "," + newDocument.DirectionCode + "," + loggedUser.Contact.EnglishName + "," + loggedUser.Contact.LocalName;
                newDocument.Code = CodeCounter.GetNumber("DocumentsFiling", newDocument.Tenant).ToString();
                newDocument.CreatedByUserId = loggedUser.Id;
                newDocument.OwnerId = loggedUser.Id;
                newDocument.CreateDate = TenantServerConfigration.GetCurrentDateTime(newDocument.Tenant);
                newDocument.UpdatedByUserId = loggedUser.Id;
                newDocument.UpdateDate = TenantServerConfigration.GetCurrentDateTime(newDocument.Tenant);

                DocumentsFilingService service = new DocumentsFilingService(objectContext, newDocument.Tenant);
                service.Create(newDocument, null);

            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                string Error = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Error += "Entity of type " + eve.Entry.Entity.GetType().Name + " in state " + eve.Entry.State + " has the following validation errors:";
                    foreach (var ve in eve.ValidationErrors)
                    {
                        //Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                        //ve.PropertyName, ve.ErrorMessage);

                        Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                    }
                }


                string authenticateduser = "";

                try
                {
                    authenticateduser = Security.SecurityUtility.GetAuthenticatedUser();
                }

                catch
                {
                    authenticateduser = "UnKnown";
                }
                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }
                ExceptionHandler.HandleException(new Exception(Error), DateTime.Now, 0, "", authenticateduser, "", ip);
                throw new Exception(Error);
            }
        }


       


        

        

        public void UpdateDocumentsFiling(DocumentsFilingPM currentEntity)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentEntity.Tenant);
            }
            DocumentsFilingService service = new DocumentsFilingService(objectContext, currentEntity.Tenant);
            List<DocumentsFilingMetaDataValuePM> DocumentsFilingChangeSet = ChangeSet.GetAssociatedChanges(currentEntity, d => d.DocumentsFilingMetaDataValues).Cast<DocumentsFilingMetaDataValuePM>().ToList();
            foreach (DocumentsFilingMetaDataValuePM itemPM in DocumentsFilingChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    case ChangeOperation.Update: { itemPM.ChangeSetOp = ChangeSetOperation.Update; break; }

                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }
            service.SetChangeSet(currentEntity.DocumentsFilingMetaDataValues);
            service.Update(currentEntity, null);
 
            //TableLastUpdateClass.UpdateTableHistory(currentEntity.Tenant, "DocumentsFiling");
        }

        [Invoke]
        public void DeleteDocumentsFiling(DocumentsFilingPM entity, int Tenant)
        {
            
           
                if (objectContext == null)
                {
                    objectContext = CommonDataContext.GetContext(Tenant);
                }

                
                documentsFilingRepository = new DocumentsFilingRepository(objectContext);
                documentRepository = new DocumentRepository(objectContext);
                DocumentsFiling extDoc = documentsFilingRepository.GetSingleDocumentsFiling(entity.Id, Tenant);
                documentsFilingRepository.Remove(extDoc);
                documentsFilingMetaDataValueRepository = new DocumentsFilingMetaDataValueRepository(objectContext);
                var DocumentMetaDataValues = documentsFilingMetaDataValueRepository.GetDocumentsFilingMetaDataValuesByTenantDocFilingId(Tenant, entity.Id);
                foreach (var itemPM in DocumentMetaDataValues)
                {
                    documentsFilingMetaDataValueRepository.Remove(itemPM);
                }
                if (!string.IsNullOrEmpty(entity.DocumentId))
                {
                    var document = documentRepository.GetSingleDocument(Tenant, entity.DocumentId);
                    documentRepository.Remove(document);
                }
                documentRepository.SubmitChanges();

                ObjectTableRepository objectTableRepository = new ObjectTableRepository(Tenant);
                ShipmentComputedFieldsRepository shipmentComputedFieldsRepository = new ShipmentComputedFieldsRepository(Tenant);
                DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(Tenant);
                


                var OTName = objectTableRepository.GetSingleObjectTable(entity.ObjectTableId, Tenant, false);
                if (OTName.Name == "Shipment")
                {
                    var ShipmentCompField = shipmentComputedFieldsRepository.GetSingleShipmentComputedFields(entity.EntityId, Tenant);
                ShipmentCompField.MissingDocumentsCount = documentsFilingQuery.GetMissingDocCountForEntity(entity.EntityId, entity.ObjectTableId, Tenant);
                if (ShipmentCompField.MissingDocumentsCount == 0)
                    { 
                    ShipmentCompField.IsMissingDocuments = false;
                    }
                    else
                    {
                    ShipmentCompField.IsMissingDocuments = true;
                    }
                //if (entity.HasFile)
                //{
                //    bool hasmissing = documentsFilingQuery.CheckMissingDocForEntity(entity.EntityId, entity.ObjectTableId, Tenant);
                //    ShipmentCompField.IsMissingDocuments = hasmissing;
                //}
                //else
                //{
                //    bool hasmissing = documentsFilingQuery.CheckMissingDocForEntity(entity.EntityId, entity.ObjectTableId, Tenant);
                //    ShipmentCompField.IsMissingDocuments = hasmissing;
                //}
                ShipmentCompField.MissingDocumentsNames = documentsFilingQuery.GetMissingDocsNamesForEntity(entity.EntityId, entity.ObjectTableId, Tenant);
                ShipmentCompField.MissingDocumentsCount = documentsFilingQuery.GetMissingDocCountForEntity(entity.EntityId, entity.ObjectTableId, Tenant);
                ShipmentCompField.IsRequestedDocuments = documentsFilingQuery.GetIfIsRequestedForEntity(entity.EntityId, Tenant);
                ShipmentCompField.RequestedDocumentsCount = documentsFilingQuery.GetRequestedDocCountForEntity(entity.EntityId, Tenant);
                ShipmentComputedFieldsHelper shipmentComputedFieldsHelper = new ShipmentComputedFieldsHelper();
                shipmentComputedFieldsHelper.UpdateShipmentComputedFields(ShipmentCompField, shipmentComputedFieldsRepository.context);


               // shipmentComputedFieldsRepository.Update(ShipmentCompField);
                  //  shipmentComputedFieldsRepository.SubmitChanges();
                }
        }

        public List<DocumentsFilingPM> GetDocumentsFilingsForRelatedDocuments(string entityId, string childEntityId, string objectTableId, string directionCode, string referenceNumber, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentsFilingQuery = new DocumentsFilingQuery(tenant);
            return documentsFilingQuery.GetDocumentsFilingsForRelatedDocuments(entityId, childEntityId, objectTableId, directionCode, referenceNumber,null, tenant);
        }

        public List<DocumentsFilingPM> GetDocumentsFilingsByEntityIdForRelatedDocuments(string entityId, string childEntityId, string objectTableId, string directionCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentsFilingQuery = new DocumentsFilingQuery(tenant);
            return documentsFilingQuery.GetDocumentsFilingsByIdForRelatedDocuments(entityId, childEntityId, objectTableId, directionCode, tenant, null);
        }


        [Invoke]
        public string DownLoadAllFilesForShipments(string ShipmentId,string ObjectTableId,int Tenant)
        {
            documentsFilingQuery = new DocumentsFilingQuery(Tenant);
            var AllDocs = documentsFilingQuery.GetDocumentsFilingPMsByEntityIdAndObjectTable(ShipmentId, null, ObjectTableId, "I", Tenant);
            var guid = Guid.NewGuid();
            var base64string = Convert.ToBase64String(guid.ToByteArray()).ToLower();
            base64string = base64string.Substring(0, 22);
            base64string = base64string.Replace("/", "_");
            base64string = base64string.Replace("+", "-");
            base64string = base64string.Replace("_", "0");
            BlobFileInfo zipfileInfo = new BlobFileInfo()
            {
                FileName = "ShipmentDocuments",
                //FolderName = "others",
                HasExternalContainer = true,
                ExternalContainerName = "tenant" + Tenant.ToString(),
                Extension = "zip",
                Tenant = Tenant,
            };
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;

           // CloudBlobContainer blobContainer = StorageAcountDetails.GetCurrentContainer(Tenant);
            //var DownLoadBlob = blobContainer.GetBlockBlobReference("ShipmentDocuments.zip");
            Crc32 crc32 = new Crc32();
            bool ShipmentHasFiles = false;
            using (MemoryStream blobStream = new MemoryStream())//DownLoadBlob.OpenWrite())
            {
                /*
                 

MemoryStream outputMemStream = new MemoryStream();

            ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);

 

            zipStream.SetLevel(3);

            byte[] bytes = null;

            foreach (string key in dataBackList.Keys)

            {

                var newEntry = new ZipEntry(key + ".josn");

                newEntry.DateTime = DateTime.Now;

 

                zipStream.PutNextEntry(newEntry);

 

                bytes = dataBackList[key];

 

                MemoryStream inStream = new MemoryStream(bytes);

                long inStreamLength = inStream.Length;

                if (inStreamLength < 200)

                {

                    inStreamLength = 200;

                }

 

                StreamUtils.Copy(inStream, zipStream, new byte[inStreamLength]);

                inStream.Close();

                zipStream.CloseEntry();

 

            }

 

            zipStream.IsStreamOwner = false;

            zipStream.Close();

            outputMemStream.Position = 0;

            return outputMemStream.ToArray();
                 */
                ZipOutputStream stream = new ZipOutputStream(blobStream);
                stream.SetLevel(3);
                //if (blobContainer != null)
                //{

                    foreach (var item in AllDocs)
                    {
                        if (item.HasFile)
                        {
                            ShipmentRepository ShRepos = new ShipmentRepository(item.Tenant);
                            var ShipmentNumber = ShRepos.GetShipmentNumberByShipmentIdTenant(item.EntityId, item.Tenant);
                            ShipmentHasFiles = true;
                            string documentName = item.DocumentId + "." + item.FileExtension;//TenantContext.Current.Id + "_" + item.DocumentId;// +"." + CurrentDocument.Extension;
                            //var blob = blobContainer.GetBlockBlobReference(documentName);
                            ZipEntry entry = new ZipEntry(item.DocumentTypeCode + "_" + ShipmentNumber + "_" + item.Code + "." + item.FileExtension);//Path.GetExtension(blob.Uri.AbsolutePath));//Path.GetFileName(blob.Uri.AbsolutePath));

                            //
                            string fileName = item.DocumentId + "." + item.FileExtension;
                            //string filePath = "tenant" + Tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileName.ToLower(), "docsin");

                            BlobFileInfo fileInfo = new BlobFileInfo()
                            {
                            FileName = item.DocumentId,
                                FolderName = "docsin",
                                Extension = item.FileExtension,
                                Tenant = Tenant,
                                FileSize = item.FileSize,

                            };


                            byte[] datainByte = storageservice.Read(fileInfo);
                            if (datainByte != null)
                            {
                               
                                 
                                entry.Size = datainByte.Length;
                                //entry.Name = item.DocumentTypeCode + "_" + item.EntityNumber + "_" + item.Code;
                                crc32.Reset();
                                crc32.Update(datainByte);
                                entry.Crc = crc32.Value;
                                stream.PutNextEntry(entry);
                                int size = 20480;
                                int remaining = datainByte.Length;
                                for (int i = 0; i < datainByte.Length; )
                                {
                                    if (remaining < size)
                                    {
                                        size = remaining;
                                    }
                                    stream.Write(datainByte, i, size);
                                    remaining = remaining - size;
                                    i = i + size;
                                    stream.Flush();
                                }
                            }


                            //
                            entry.DateTime = DateTime.Now;


                        }
                    }
                    stream.Finish();
                    stream.Close();

                    zipfileInfo.FileSize = blobStream.ToArray().Length;
                    storageservice.Write(blobStream.ToArray(), zipfileInfo);
                //}
               
            }
            if (ShipmentHasFiles)
            {
                return "ShipmentDocuments.zip";
            }
            else
            {
                return null;
            } 
        }

        public DocumentsFilingPM GetDocumentsFilingByDocumentType(string documentTypeId, string objectTableId, string entityId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentsFilingQuery = new DocumentsFilingQuery(tenant);
            return documentsFilingQuery.GetDocumentsFilingByDocumentType(documentTypeId, objectTableId, entityId, tenant);
        }

        
        #endregion


        #region DocumentTypeMetaData

        public DocumentTypeMetaDataPM GetSingleDocumentTypeMetaDataPM(string id, int tenant)
        {
            objectContext = CommonDataContext.GetContext(tenant);
            documentTypeMetaDataQuery = new DocumentTypeMetaDataQuery(tenant);
            DocumentTypeMetaDataPM DocumentTypeMetaData = documentTypeMetaDataQuery.GetSinglePM(id, tenant);
            return DocumentTypeMetaData;
        }

        //public DocumentTypeMetaDataList GetSingleDocumentTypeMetaDataList(string id,int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
           

        //    if (objectContext == null)
        //    {
        //        objectContext = CommonDataContext.GetContext(tenant);
        //    }
        //    objectContext = CommonDataContext.GetContext(tenant);
        //    documentTypeMetaDataRepository = new DocumentTypeMetaDataRepository(tenant);
        //    DocumentTypeMetaDataList DocMetaData = documentTypeMetaDataRepository.GetSingleDocumentTypeMetaData(id, tenant);


        //    return DocMetaData;


        //}

        public IQueryable<DocumentTypeMetaDataList> GetDocumentTypeMetaDataLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeMetaDataRepository = new DocumentTypeMetaDataRepository(tenant);
            documentTypeMetaDataQuery = new DocumentTypeMetaDataQuery(documentTypeMetaDataRepository);
            //objectContext = CommonDataContext.GetContext(tenant);
            IQueryable<DocumentTypeMetaData> docMetaQuory = documentTypeMetaDataRepository.GetDocumentTypeMetaDatas(tenant);
            IQueryable<DocumentTypeMetaDataList> list = documentTypeMetaDataQuery.GetIQueryableEntityList(docMetaQuory);

            return list;
            
       
        }


        public IQueryable<DocumentTypeMetaDataPM> GetDocumentTypeMetaDataByDocumentTypeId(string documentTypeId, int tenant)
        {
            objectContext = CommonDataContext.GetContext(tenant);
            documentTypeMetaDataQuery = new DocumentTypeMetaDataQuery(tenant);
            IQueryable<DocumentTypeMetaDataPM> DocumentTypeMetaData = documentTypeMetaDataQuery.GetDocumentTypeMetaDataPMsByDocumentIdTenant(documentTypeId, tenant); 
            return DocumentTypeMetaData;
        }


        #endregion


        #region DocumentFilingMetaDataValues

        //public DocumentsFilingMetaDataValuePM GetSingleDocumentMetaDataValuePM(string id, int tenant)
        //{
        //    objectContext = CommonDataContext.GetContext(tenant);
        //    documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(tenant);
        //    DocumentsFilingMetaDataValuePM CustomsDocumentMetaDataValue = documentsFilingMetaDataValueQuery.GetSinglePM(id,tenant);
        //    return CustomsDocumentMetaDataValue;
        //}

        //public DocumentsFilingMetaDataValueList GetSingleDocumentMetaDataValueList(string customDocumentId, string metadataTypeCode, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    //SecurityUtility.CheckContactFeature("Customs.CustomsDocumentMetaDataValue", "READ", tenant);

        //    if (objectContext == null)
        //    {
        //        objectContext = CommonDataContext.GetContext(tenant);
        //    }
        //    objectContext = CommonDataContext.GetContext(tenant);
     
        //}

        //public List<DocumentsFilingMetaDataValueList> GetCustomsDocumentMetaDataValueLists(int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    //SecurityUtility.CheckContactFeature("Customs.CustomsDocumentMetaDataValue", "READ", tenant);
        //    customContext = CustomContext.GetContext(tenant);
        //    CustomsDocumentMetaDataValueListQueryService listService = new CustomsDocumentMetaDataValueListQueryService(customContext);
        //    return listService.GetList(tenant);
        //}
        public IQueryable<DocumentsFilingMetaDataValuePM> GetDocumentMetaDataValuesByDocument(string documentsFilingId, int tenant)
        {
            objectContext = CommonDataContext.GetContext(tenant);
            documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(tenant);
            IQueryable<DocumentsFilingMetaDataValuePM> DocumentMetaDataValues = documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByDocumentIdTenant(documentsFilingId, tenant);
            return DocumentMetaDataValues;
        }
        #endregion

    }
}