using System.Collections.Generic;
using System.ServiceModel.DomainServices.Server;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class DocumentTypeService: DomainService
    {
        bool isNewEntity;
        private int tenant;
        public DocumentType Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private DocumentTypePM entityPM;
        private ICommonDataContext objectContext;
        private DocumentTypeRepository entityRepository;
        DocumentTypeCopyRepository documentTypeCopyRepository;
        DocumentTypeCustomFieldRepository documentTypeCustomFieldRepository;

        public DocumentTypeService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new DocumentTypeRepository(objectContext);
        }

        public void Create(DocumentTypePM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("DocumentType", tenant).ToString();
            this.Poco = new DocumentType();
            this.Poco.Id = this.entityPM.Id;
            
            documentTypeCopyRepository = new DocumentTypeCopyRepository(objectContext);
            documentTypeCustomFieldRepository = new DocumentTypeCustomFieldRepository(objectContext);
            
            theEntityPm.Id = IdCounter.GetNumber("DocumentType", theEntityPm.Tenant).ToString();
            this.Poco.Id = theEntityPm.Id;

            #region documentTypecopeis
            if (theEntityPm.DocumentTypeCopies != null)
            {
                foreach (DocumentTypeCopyPM a in theEntityPm.DocumentTypeCopies)
                {
                    a.DocumentTypeId = theEntityPm.Id;

                    DocumentTypeCopy copy = new DocumentTypeCopy()
                    {
                        Id = IdCounter.GetNumber("DocumentTypeCopy", theEntityPm.Tenant).ToString(),
                        Code = a.Code,
                        Name = a.Name,
                        Tenant = theEntityPm.Tenant,
                        DocumentTypeId = theEntityPm.Id,
                    };
                    a.Id = copy.Id;
                    documentTypeCopyRepository.Add(copy);
                }

                if (theEntityPm.DocumentTypeCopies.Count == 0)
                {
                    DocumentTypeCopy copy = new DocumentTypeCopy()
                    {
                        Id = IdCounter.GetNumber("DocumentTypeCopy", theEntityPm.Tenant).ToString(),
                        Code = theEntityPm.Code,
                        Name = theEntityPm.Name,
                        Tenant = theEntityPm.Tenant,
                        DocumentTypeId = theEntityPm.Id,
                        IsSelectedByDefault = true,
                    };
                    documentTypeCopyRepository.Add(copy);
                }
            }
            #endregion
            
            DocumentTypeValidating.Validate(theEntityPm);
            DocumentTypeTracing.Trace(theEntityPm, Poco, isNewEntity);

            DocumentTypeMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

            TableLastUpdateClass.UpdateTableHistory(theEntityPm.Tenant, "DocumentType");

            #region DocuemntTypeCustomField
            if (theEntityPm.Code == "740")
            {
            }
            if (theEntityPm.Code == "716")
            {
                DocumentTypeCustomField customfield1 = new DocumentTypeCustomField() { DefaultValue = "3", DocumentTypeId = theEntityPm.Id, Name = "Number Of Originals", FieldCode = "NumberOfOriginals", FieldDataTypeCode = "Text", Id = IdCounter.GetNumber("DocumentTypeCustomField", theEntityPm.Tenant).ToString(), InActive = false, IsRequired = false, Tenant = theEntityPm.Tenant, MultiLine = false };
                DocumentTypeCustomField customfield2 = new DocumentTypeCustomField() { DefaultValue = "Copy", DocumentTypeId = theEntityPm.Id, Name = "Copy Or Original", FieldCode = "CopyOrOriginal", FieldDataTypeCode = "Text", Id = IdCounter.GetNumber("DocumentTypeCustomField", theEntityPm.Tenant).ToString(), InActive = false, IsRequired = false, Tenant = theEntityPm.Tenant, MultiLine = false };
                DocumentTypeCustomField customfield3 = new DocumentTypeCustomField() { DefaultValue = "False", DocumentTypeId = theEntityPm.Id, Name = "Has attachment list", FieldCode = "HasAttachmentList", FieldDataTypeCode = "Boolean", Id = IdCounter.GetNumber("DocumentTypeCustomField", theEntityPm.Tenant).ToString(), InActive = false, IsRequired = false, Tenant = theEntityPm.Tenant, MultiLine = false };

                documentTypeCustomFieldRepository.Add(customfield1);
                documentTypeCustomFieldRepository.Add(customfield2);
                documentTypeCustomFieldRepository.Add(customfield3);
            }

            if (theEntityPm.Code == "716SD")
            {
                DocumentTypeCustomField cutomfield1 = new DocumentTypeCustomField() { DefaultValue = "", DocumentTypeId = theEntityPm.Id, Name = "Originals/Copies", FieldCode = "OriginalsOrCopiesNo", FieldDataTypeCode = "Text", Id = IdCounter.GetNumber("DocumentTypeCustomField", theEntityPm.Tenant).ToString(), InActive = false, IsRequired = false, Tenant = theEntityPm.Tenant, MultiLine = false };
                DocumentTypeCustomField customfield3 = new DocumentTypeCustomField() { DefaultValue = "False", DocumentTypeId = theEntityPm.Id, Name = "Has attachment list", FieldCode = "HasAttachmentList", FieldDataTypeCode = "Boolean", Id = IdCounter.GetNumber("DocumentTypeCustomField", theEntityPm.Tenant).ToString(), InActive = false, IsRequired = false, Tenant = theEntityPm.Tenant, MultiLine = false };
                documentTypeCustomFieldRepository.Add(cutomfield1);
                documentTypeCustomFieldRepository.Add(customfield3);
            }

            if (theEntityPm.Code == "CMR")
            {
                DocumentTypeCustomField cutomfield1 = new DocumentTypeCustomField() { DefaultValue = "", DocumentTypeId = theEntityPm.Id, Name = "Cash On Delivery", FieldCode = "CashOnDelivery", FieldDataTypeCode = "Text", Id = IdCounter.GetNumber("DocumentTypeCustomField", theEntityPm.Tenant).ToString(), InActive = false, IsRequired = false, Tenant = theEntityPm.Tenant, MultiLine = false };
                DocumentTypeCustomField cutomfield2 = new DocumentTypeCustomField() { DefaultValue = "", DocumentTypeId = theEntityPm.Id, Name = "Instructions", FieldCode = "Instructions", FieldDataTypeCode = "Text", Id = IdCounter.GetNumber("DocumentTypeCustomField", theEntityPm.Tenant).ToString(), InActive = false, IsRequired = false, Tenant = theEntityPm.Tenant, MultiLine = true };
                DocumentTypeCustomField customfield3 = new DocumentTypeCustomField() { DefaultValue = "", DocumentTypeId = theEntityPm.Id, Name = "Documents Attached", FieldCode = "DocumentsAttached", FieldDataTypeCode = "Text", Id = IdCounter.GetNumber("DocumentTypeCustomField", theEntityPm.Tenant).ToString(), InActive = false, IsRequired = false, Tenant = theEntityPm.Tenant, MultiLine = true };

                documentTypeCustomFieldRepository.Add(cutomfield1);
                documentTypeCustomFieldRepository.Add(cutomfield2);
                documentTypeCustomFieldRepository.Add(customfield3);
            }

            if (theEntityPm.Code == "SCMR")
            {
                DocumentTypeCustomField cutomfield1 = new DocumentTypeCustomField() { DefaultValue = "", DocumentTypeId = theEntityPm.Id, Name = "Cash On Delivery", FieldCode = "CashOnDelivery", FieldDataTypeCode = "Text", Id = IdCounter.GetNumber("DocumentTypeCustomField", theEntityPm.Tenant).ToString(), InActive = false, IsRequired = false, Tenant = theEntityPm.Tenant, MultiLine = false };
                DocumentTypeCustomField cutomfield2 = new DocumentTypeCustomField() { DefaultValue = "", DocumentTypeId = theEntityPm.Id, Name = "Instructions (13)", FieldCode = "Instructions(13)", FieldDataTypeCode = "Text", Id = IdCounter.GetNumber("DocumentTypeCustomField", theEntityPm.Tenant).ToString(), InActive = false, IsRequired = false, Tenant = theEntityPm.Tenant, MultiLine = true };
                DocumentTypeCustomField customfield3 = new DocumentTypeCustomField() { DefaultValue = "", DocumentTypeId = theEntityPm.Id, Name = "Documents Attached", FieldCode = "DocumentsAttached", FieldDataTypeCode = "Text", Id = IdCounter.GetNumber("DocumentTypeCustomField", theEntityPm.Tenant).ToString(), InActive = false, IsRequired = false, Tenant = theEntityPm.Tenant, MultiLine = true };
                documentTypeCustomFieldRepository.Add(cutomfield1);
                documentTypeCustomFieldRepository.Add(cutomfield2);
                documentTypeCustomFieldRepository.Add(customfield3);
            }
            if (theEntityPm.Code == "740L")
            {
                DocumentTypeCustomField cutomfield1 = new DocumentTypeCustomField() { DefaultValue = "", DocumentTypeId = theEntityPm.Id, Name = "Additional Information", FieldCode = "AdditionalInformation", FieldDataTypeCode = "Text", Id = IdCounter.GetNumber("DocumentTypeCustomField", theEntityPm.Tenant).ToString(), InActive = false, IsRequired = false, Tenant = theEntityPm.Tenant, MultiLine = true };
                documentTypeCustomFieldRepository.Add(cutomfield1);
            }
            #endregion
        }

        public void Update(DocumentTypePM theEntityPm , List<DocumentTypeCopyPM> documentTypeCopyList = null)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleDocumentTypes(theEntityPm.Id, theEntityPm.Tenant);

            DocumentTypeCopyService service = new DocumentTypeCopyService(objectContext, theEntityPm.Tenant);
            #region DocumentTypeCopies

            if (documentTypeCopyList == null)
            {
                if (theEntityPm.DocumentTypeCopies != null && theEntityPm.DocumentTypeCopies.Count > 0)
                {
                    documentTypeCopyList = theEntityPm.DocumentTypeCopies;
                }
            }


            if (documentTypeCopyList != null)
            {
                foreach (DocumentTypeCopyPM r in documentTypeCopyList)
                {

                    switch (r.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                service.Create(r);
                                break;
                            }
                        case ChangeSetOperation.Update:
                            {
                                service.Update(r);
                                break;
                            }
                        case ChangeSetOperation.Delete:
                            {
                                DocumentTypeCopy documentTypeCopy = documentTypeCopyRepository.GetSingleDocumentTypeCopy(r.Id);
                                documentTypeCopyRepository.Remove(documentTypeCopy);
                                break;
                            }
                        case ChangeSetOperation.None:
                            {
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
            }
            # endregion

            DocumentTypeValidating.Validate(theEntityPm);
            DocumentTypeTracing.Trace(theEntityPm, Poco, isNewEntity);
            DocumentTypeMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
            RemoveEntityFromCache(theEntityPm);
            TableLastUpdateClass.UpdateTableHistory(theEntityPm.Tenant, "DocumentType");
		}

        public void Update(DocumentTypePM theEntityPm, bool mapComposition = false)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleDocumentTypes(theEntityPm.Id, theEntityPm.Tenant);

            DocumentTypeCopyService service = new DocumentTypeCopyService(objectContext, theEntityPm.Tenant);
            #region DocumentTypeCopies

            if (theEntityPm.DocumentTypeCopies != null)
            {
                foreach (DocumentTypeCopyPM r in theEntityPm.DocumentTypeCopies)
                {

                    switch (r.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                service.Create(r);
                                break;
                            }
                        case ChangeSetOperation.Update:
                            {
                                service.Update(r);
                                break;
                            }
                        case ChangeSetOperation.Delete:
                            {
                                DocumentTypeCopy documentTypeCopy = documentTypeCopyRepository.GetSingleDocumentTypeCopy(r.Id);
                                documentTypeCopyRepository.Remove(documentTypeCopy);
                                break;
                            }
                        case ChangeSetOperation.None:
                            {
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
            }
            # endregion

            DocumentTypeValidating.Validate(theEntityPm);
            DocumentTypeTracing.Trace(theEntityPm, Poco, isNewEntity);
            DocumentTypeMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
            RemoveEntityFromCache(theEntityPm);
            TableLastUpdateClass.UpdateTableHistory(theEntityPm.Tenant, "DocumentType");
		}


        private void RemoveEntityFromCache(DocumentTypePM entityPm)
        {
            var cacheKey = "DocumentTypeByCode,code," + entityPm.Code + ",tenant," + entityPm.Tenant.ToString();
            if (CacheManager.CacheWrapper.Get(cacheKey) != null)
            {
                CacheManager.CacheWrapper.Invalidate(cacheKey);
            }
        }
    }
}
