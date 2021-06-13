using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Helpers
{
   public class DocumentPopulateAutomaticDateUpdateService
    {

        public void Update(DocumentPopulateAutomaticDateArgs documentPopulateAutomaticDateArgs)
        {
            DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(documentPopulateAutomaticDateArgs.Tenant);
            DocumentType documentType = documentTypeRepository.GetSingleDocumentTypeByCode(documentPopulateAutomaticDateArgs.DocumentTypeCode, documentPopulateAutomaticDateArgs.Tenant);
            if (documentType != null && !string.IsNullOrEmpty(documentPopulateAutomaticDateArgs.ObjectTableName))
            {
                string populateAutomaticDateFieldCode = GetPopulateAutomaticDateFieldCode(documentPopulateAutomaticDateArgs.ProcessType, documentType);
                if (!string.IsNullOrEmpty(populateAutomaticDateFieldCode))
                {
                    ObjectFieldRepository objectFieldRepository = new ObjectFieldRepository(documentPopulateAutomaticDateArgs.Tenant);
                    var populateAutomaticDateObjectField = objectFieldRepository.GetSingleObjectFieldByFieldCode(populateAutomaticDateFieldCode, documentPopulateAutomaticDateArgs.Tenant);
                    if (populateAutomaticDateObjectField != null)
                    {
                        var entity = InjectionUtil.Instance.GetEntityByObjectTableNameAndEntityId(documentPopulateAutomaticDateArgs.ObjectTableName, documentPopulateAutomaticDateArgs.EntityId, documentPopulateAutomaticDateArgs.Tenant);
                        if (entity != null)
                        {
                            object documentPopulateAutomaticValue = GetDocumentPopulateAutomaticValue(documentPopulateAutomaticDateArgs, populateAutomaticDateObjectField);
                            SetPropertyValueToEntity(populateAutomaticDateObjectField, entity, documentPopulateAutomaticValue);
                            InjectionUtil.Instance.UpdateEntity(entity, documentPopulateAutomaticDateArgs.ObjectTableName, documentPopulateAutomaticDateArgs.Tenant);
                        }
                    }
                }
            }
        }

        private  string GetPopulateAutomaticDateFieldCode(string processType, DocumentType documentType)
        {
            string fieldCode = string.Empty;
            if (processType == "Send" && !string.IsNullOrEmpty(documentType.OnSendPopulateDateFieldName)) fieldCode = documentType.OnSendPopulateDateFieldName;
            else if (processType == "Print" && !string.IsNullOrEmpty(documentType.OnPrintPopulateDateFieldName)) fieldCode = documentType.OnPrintPopulateDateFieldName;
            else if (processType == "Upload" && !string.IsNullOrEmpty(documentType.OnUploadPopulateDateFieldName)) fieldCode = documentType.OnUploadPopulateDateFieldName;
            return fieldCode;
        }


        private object GetDocumentPopulateAutomaticValue(DocumentPopulateAutomaticDateArgs documentPopulateAutomaticDateArgs, ObjectField objectField)
        {
            DateTime currentDateTime = TenantServerConfigration.GetCurrentDateTime(documentPopulateAutomaticDateArgs.Tenant);
            if (objectField.IsCustom)
            {
                return new CustomFieldClass(
                    objectField.FieldName,
                    documentPopulateAutomaticDateArgs.ObjectTableName,
                    new CustomFieldClass().SetFieldDataType(objectField.DataTypeCode, currentDateTime)
                    );
            }
            return currentDateTime;
        }




        private void SetPropertyValueToEntity(ObjectField objectField, object entity, object fieldValue)
        {
            PropertyInfo propInfo = entity.GetType().GetProperty(objectField.FieldName);
            if (propInfo != null) propInfo.SetValue(entity, fieldValue, null);
        }


    }

    public class DocumentPopulateAutomaticDateArgs
    {
        public string EntityId { get; set; }
        public string ObjectTableName { get; set; }
        public string DocumentTypeCode { get; set; }
        public int Tenant { get; set; }
        public string ProcessType { get; set; }



    }
}
