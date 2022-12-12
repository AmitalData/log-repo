using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebFreight.Web.Helpers.StimulReportCustomizationDataProvider
{
    public class CustomChildDataProviderService
    {
        private List<Field> fields;
        private DocumentDataProviderArgs documentDataProviderArgs;
        private CustomFieldResolver customFieldResolver;
        private List<CustomChildEntity> customChildEntities;
        public CustomChildDataProviderService(List<Field> fields, DocumentDataProviderArgs documentDataProviderArgs , string parentObjectTableName)
        {
            this.documentDataProviderArgs = documentDataProviderArgs;
            this.fields = fields;
            customFieldResolver = new CustomFieldResolver();
            customChildEntities = new CustomChildEntityService(new CustomChildEntityArgs() { ParentEntityId = documentDataProviderArgs.EntityId, ParentObjectTableName = parentObjectTableName, Tenant = documentDataProviderArgs.DocumentTypeTemplatePM.Tenant }).BuildCustomChildEntities();
        }

        public void Set(object dataProvider)
        {
            foreach (Field field in fields.Where(d => d.IsCustom && d.IsChild && d.IsList).ToList())
            {
                SetFieldValue(dataProvider , field.Name , GetCustomChildObjects(dataProvider ,field));
            }
        }

        private IList GetCustomChildObjects(object dataProvider, Field field)
        {
            CustomChildEntity customChildEntity = customChildEntities.Where(d => d.Name == field.Code).FirstOrDefault();
            if (customChildEntity == null || customChildEntity.Values == null || customChildEntity.Values.Count() == 0) return null;
            var customChildObjects = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(field.AdditinalDetails.Type));
            foreach (CustomChildObjectPM customChildObjectPM in customChildEntity.Values)
            {
                customChildObjects.Add(GetNewInStanceFromCustomChildObject(field.AdditinalDetails, customChildObjectPM));
            }
            return customChildObjects;

        }

        private object GetNewInStanceFromCustomChildObject(AdditionalDetails additionalDetails, CustomChildObjectPM customChildObjectPM)
        {
            var customChildObject = Activator.CreateInstance(additionalDetails.Type);
            foreach (var field in additionalDetails.Fields)
            {
                SetFieldValue(customChildObject, field.Name, GetFieldValue(customChildObjectPM, field));
            }
            return customChildObject;
        }


        private void SetFieldValue(object entity , string fieldName , object fieldValue)
        {
            PropertyInfo propertyInfo = entity.GetType().GetProperty(fieldName);
            if (propertyInfo == null) return;
            propertyInfo.SetValue(entity, fieldValue);
        }

        public object GetFieldValue(object entity,  Field field)
        {
            PropertyInfo propertyInfo = entity.GetType().GetProperty(field.Code);
            if (propertyInfo == null) return null;
            var fieldValue = propertyInfo.GetValue(entity);

            if (field.IsCustom && fieldValue != null && fieldValue.GetType() == typeof(CustomFieldClass))
            {
                fieldValue = (fieldValue as CustomFieldClass)?.Value;
            }

            if (field.IsCustom || field.DataTypeCode == "LookUp" && field.DataTypeCode == "PickList")
            {
                fieldValue = customFieldResolver.GetFieldValue2(fieldValue, new ObjectField() { LookUpTableId = field.LookUpTableId, Tenant = field.Tenant, DataTypeCode = field.DataTypeCode }, documentDataProviderArgs.DocumentTypeTemplatePM.Tenant);
            }

            if (field.IsCustom && fieldValue != null)
            {
                fieldValue = customFieldResolver.SetFieldDataType(field.DataTypeCode, fieldValue.ToString());
            }

            return fieldValue;
        }
    
    }

}