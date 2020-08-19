using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Helpers
{
   public class DocumentDateUpdateService
    {


        public void Update(DocumentDateUpdateArgs documentDateUpdateArgs)
        {
            DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(documentDateUpdateArgs.Tenant);
            DocumentType documentType = documentTypeRepository.GetSingleDocumentTypeByCode(documentDateUpdateArgs.DocumentTypeCode, documentDateUpdateArgs.Tenant);
            if (documentType != null && !string.IsNullOrEmpty(documentDateUpdateArgs.ObjectTableName))
            {
                string fieldCode = string.Empty;
                if (documentDateUpdateArgs.ProcessType == "Send" && !string.IsNullOrEmpty(documentType.OnSendPopulateDateFieldName)) fieldCode = documentType.OnSendPopulateDateFieldName;
                else if (documentDateUpdateArgs.ProcessType == "Print" && !string.IsNullOrEmpty(documentType.OnPrintPopulateDateFieldName)) fieldCode = documentType.OnPrintPopulateDateFieldName;
                else if (documentDateUpdateArgs.ProcessType == "Upload" && !string.IsNullOrEmpty(documentType.OnUploadPopulateDateFieldName)) fieldCode = documentType.OnUploadPopulateDateFieldName;
                if (!string.IsNullOrEmpty(fieldCode))
                {
                    ObjectFieldRepository objectFieldRepository = new ObjectFieldRepository(documentDateUpdateArgs.Tenant);
                    var objectField = objectFieldRepository.GetSingleObjectFieldByFieldCode(fieldCode, documentDateUpdateArgs.Tenant);
                    if (objectField != null)
                    {
                        var entity = InjectionUtil.Instance.GetEntityByObjectTableNameAndEntityId(documentDateUpdateArgs.ObjectTableName, documentDateUpdateArgs.EntityId, documentDateUpdateArgs.Tenant);
                        if (entity != null)
                        {
                            SetPropertyValueToEntity(objectField, entity, DateTime.Now);
                            InjectionUtil.Instance.UpdateEntity(entity, documentDateUpdateArgs.ObjectTableName, documentDateUpdateArgs.Tenant);
                        }
                    }

                }

            }

        }

        private static void SetPropertyValueToEntity(ObjectField objectField, object entity, object fieldValue)
        {
            PropertyInfo propInfo = entity.GetType().GetProperty(objectField.FieldName);
            if (propInfo != null) propInfo.SetValue(entity, fieldValue, null);
        }





    }

    public class DocumentDateUpdateArgs
    {

        public string EntityId { get; set; }
        public string ObjectTableName { get; set; }
        public string DocumentTypeCode { get; set; }
        public int Tenant { get; set; }
        public string ProcessType { get; set; }



    }
}
