using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebFreight.Web.Helpers.StimulReportCustomizationDataProvider
{
    public class CustomFieldDataProviderService
    {
        private List<Field> fields;
        private DocumentDataProviderArgs documentDataProviderArgs;
        private CustomFieldResolver customFieldResolver;
        CustomFieldClass customFieldClass;
        private string objectTableName = string.Empty;
        public CustomFieldDataProviderService(List<Field> fields, DocumentDataProviderArgs documentDataProviderArgs, string objectTableName)
        {
            this.documentDataProviderArgs = documentDataProviderArgs;
            this.objectTableName = objectTableName;
            this.fields = fields;
            customFieldResolver = new CustomFieldResolver(documentDataProviderArgs.DocumentTypeTemplatePM.Tenant);
            customFieldClass = new CustomFieldClass();

        }

        public void Set(object dataProvider)
        {
            if (fields.Where(d => d.IsCustom && !d.IsChild && !d.IsList).ToList().Count() == 0) return ;
            var entityPM = this.documentDataProviderArgs.EntityPM;
            if(entityPM == null) entityPM = InjectionUtil.Instance.GetEntityByObjectTableNameAndEntityId(objectTableName, documentDataProviderArgs.EntityId, documentDataProviderArgs.DocumentTypeTemplatePM.Tenant);
            if (entityPM == null) return;
            foreach (Field field in fields.Where(d => d.IsCustom && !d.IsChild && !d.IsList).ToList())
            {
                SetFieldValue(dataProvider, field.Name, GetFieldValue(entityPM, field));
            }
        }

        public void SetFieldValue(object entity, string fieldName, object fieldValue)
        {
            PropertyInfo propertyInfo = entity.GetType().GetProperty(fieldName);
            if (propertyInfo == null) return;
            propertyInfo.SetValue(entity, fieldValue);
        }

        public object GetFieldValue(object entity, Field field)
        {
            PropertyInfo propertyInfo = entity.GetType().GetProperty(field.Code);
            if (propertyInfo == null) return null;
            var fieldValue = propertyInfo.GetValue(entity);

            if (field.IsCustom && fieldValue != null && fieldValue.GetType() == typeof(CustomFieldClass))
            {
                fieldValue = (fieldValue as CustomFieldClass)?.Value;
            }

            if (field.IsCustom || field.DataTypeCode == "LookUp" || field.DataTypeCode == "PickList")
            {
                fieldValue = customFieldResolver.GetFieldValue2(fieldValue, new ObjectField() { LookUpTableId = field.LookUpTableId, Tenant = field.Tenant, DataTypeCode = field.DataTypeCode }, documentDataProviderArgs.DocumentTypeTemplatePM.Tenant);
            }

            if (field.IsCustom && fieldValue != null)
            {
                fieldValue = customFieldClass.GetFieldValue(field.DataTypeCode, fieldValue.ToString());
            }

            return fieldValue;
        }

    }

}