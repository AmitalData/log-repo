using System.Collections.Generic;
using System.Linq;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public IQueryable<FormCustomField> GetFormCustomFields(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            formCustomFieldRepository = new FormCustomFieldRepository(tenant);
            return formCustomFieldRepository.GetFormCustomFields(0);
        }

        public FormCustomFieldPM GetSingleFormCustomField(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            formCustomFieldQuery = new FormCustomFieldQuery(tenant);
            return formCustomFieldQuery.GetSinglePM(id, tenant);
        }

        public IQueryable<FormCustomFieldPM> GetFormCustomFieldsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            formCustomFieldQuery = new FormCustomFieldQuery(tenant);
            return formCustomFieldQuery.GetFormCustomFieldPMsByTenant(tenant);
        }

        public List<FormCustomFieldPM> GetFormCustomFieldsByDocument(int tenant, string documentTypeId, string entityId, string entityTypeId)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }
            formCustomFieldRepository = new FormCustomFieldRepository(objectContext);
            documentTypeRepository = new DocumentTypeRepository(objectContext);
            documentTypeCustomFieldRepository = new DocumentTypeCustomFieldRepository(objectContext);
            documentTypeCustomFieldQuery = new DocumentTypeCustomFieldQuery(documentTypeCustomFieldRepository);

            List<FormCustomFieldPM> fromCustomFieldsList = new List<FormCustomFieldPM>();
            List<DocumentTypeCustomFieldPM> customFieldsList = documentTypeCustomFieldQuery.GetDocumentTypeCusotmFieldPMsByDocumentTypeId(documentTypeId, tenant).ToList();

            foreach (DocumentTypeCustomFieldPM customField in customFieldsList)
            {
                formCustomFieldQuery = new FormCustomFieldQuery(formCustomFieldRepository);
                DocumentType docType = documentTypeRepository.GetSingleDocumentTypes(customField.DocumentTypeId, tenant);
                FormCustomFieldPM formCustomField = formCustomFieldQuery.GetSingleFormCustomFieldsPMByDocument(tenant, documentTypeId, customField.FieldCode, entityId, entityTypeId);

                if (formCustomField != null)
                {
                    fromCustomFieldsList.Add(formCustomField);
                }
                else
                {
                    formCustomField = new FormCustomFieldPM()
                    {
                        Tenant = tenant,
                        DocumentTypeId = documentTypeId,
                        EntityId = entityId,
                        ObjectTableId = docType.ObjectTableId,
                        FieldCode = customField.FieldCode,
                        Value = customField.DefaultValue,
                        Id = customField.FieldCode,
                    };
                    fromCustomFieldsList.Add(formCustomField);
                }
            }
            return fromCustomFieldsList;
        }


        public void InsertFormCustomField(FormCustomFieldPM entityPM)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            FormCustomFieldService service = new FormCustomFieldService(objectContext, entityPM.Tenant);
            service.Create(entityPM);

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "FormCustomField");
        }

        public void UpdateFormCustomField(FormCustomFieldPM entityPM)
        {
            SecurityUtility.CheckContactFeature("DocumentType", "UPDATE", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            if (entityPM.Id != entityPM.FieldCode)
            {
                FormCustomFieldService service = new FormCustomFieldService(objectContext, entityPM.Tenant);
                service.Update(entityPM);
                TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "FormCustomField");
            }

            else
            {
                InsertFormCustomField(entityPM);
            }
        }

        public void DeleteFormCustomField(FormCustomFieldPM entity)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entity.Tenant);
            }

            formCustomFieldRepository = new FormCustomFieldRepository(objectContext);
            FormCustomField formCustom = formCustomFieldRepository.GetSingleFormCustomField(entity.Id, entity.Tenant);
            formCustomFieldRepository.Remove(formCustom);
        }
    }
}