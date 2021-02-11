using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebFreight.Web.Helpers
{
    public  class SendDocumentDataExernalService
    {
        private int tenant;
        private string entityId;
        private Automation automation;
        private AutomationDocumentResult automationDocumentResult;

        public SendDocumentDataExernalService(EntityChange entityChange, Automation automation, AutomationDocumentResult automationDocumentResult)
        {
            this.tenant = entityChange.Tenant;
            this.entityId = entityChange.EntityId;
            this.automation = automation;
            this.automationDocumentResult = automationDocumentResult;
        }

        public string GetDocumentFileName()
        {
            string documentFileName = GetDocumentTypeName();
            HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
            object entityPM = htmlEditorHelper.GetEntity(automationDocumentResult.ObjectTableName, entityId, tenant);
            if(entityPM != null)
                documentFileName += GetExtraFileName(entityPM);
            return documentFileName;
        }

        private string GetDocumentTypeName()
        {
            string documentTypeName = string.Empty;
            DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
            DocumentTypePM documentTypePM = documentTypeQuery.GetSinglePM(automation.DocumentTypeId, tenant);
            if (documentTypePM != null)
                documentTypeName = documentTypePM.Name;
            return documentTypeName;
        }

        private string GetExtraFileName(object entityPM)
        {
            string extraFileName = string.Empty;
            string invoiceNumber = GetPropertyValueFromObject("InvoiceNumber", entityPM);
            if (!string.IsNullOrEmpty(invoiceNumber))
                extraFileName = "_" + invoiceNumber.ToLower();
            return extraFileName;
        }

        private string GetPropertyValueFromObject(string propertyName, object entity)
        {
            string propertyValue = string.Empty;
            if (entity != null)
            {
                PropertyInfo propertyInfo = entity.GetType().GetProperty(propertyName);
                if (propertyInfo != null)
                {
                    propertyValue = propertyInfo.GetValue(entity).ToString();
                }
            }
            return propertyValue;
        }
    }
}
