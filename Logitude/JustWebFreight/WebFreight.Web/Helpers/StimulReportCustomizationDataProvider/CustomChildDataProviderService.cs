using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
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
        public CustomChildDataProviderService(List<Field> fields, DocumentDataProviderArgs documentDataProviderArgs)
        {
            this.documentDataProviderArgs = documentDataProviderArgs;
            this.fields = fields;
            customFieldResolver = new CustomFieldResolver();

        }

        public void Set(object dataProvider)
        {
            var customChildEntityService = new CustomChildEntityService(new CustomChildEntityArgs() { ParentEntityId = documentDataProviderArgs.EntityId, ParentObjectTableName = "Shipment", Tenant = documentDataProviderArgs.Tenant });
            var customChildEntities = customChildEntityService.BuildCustomChildEntities();

            foreach (Field field in fields.Where(d => d.IsCustom && d.IsList).ToList())
            {
                BuildCustomChildEntity(field, customChildEntities.Where(d => d.Name == field.Code).FirstOrDefault());

            }
        }

        private void BuildCustomChildEntity(Field field, CustomChildEntity customChildEntity)
        {
            if (customChildEntity == null || customChildEntity.Values == null || customChildEntity.Values.Count() == 0) return;
            var customChildEntities = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(field.AdditinalDetails.Type));
            var customChildProperties = field.AdditinalDetails.Type.GetProperties().ToDictionary(e => e.Name, e => e);
            var customChildObjectProperties = customChildEntity.Values[0].GetType().GetProperties().ToDictionary(e => e.Name, e => e);

            foreach (CustomChildObjectPM customChildObjectPM in customChildEntity.Values)
            {
                customChildEntities.Add(GetNewInStanceFromCustomChildEntity(
                        new CustomChildDataProviderArgs() { Field = field,
                        CustomChildProperties = customChildProperties,
                        CustomChildObjectProperties = customChildObjectProperties,
                        CustomChildObjectPM = customChildObjectPM }
                    ));
            }
        }

        private object GetNewInStanceFromCustomChildEntity(CustomChildDataProviderArgs customChildDataProviderArgs)
        {
            var customChildEntity = Activator.CreateInstance(customChildDataProviderArgs.Field.AdditinalDetails.Type);

            foreach (var customChildField in customChildDataProviderArgs.Field.AdditinalDetails.Fields)
            {
                MapCustomChildEntityFields(customChildDataProviderArgs, customChildEntity, customChildField);
            }
            return customChildEntity;
        }

        private void MapCustomChildEntityFields(CustomChildDataProviderArgs customChildDataProviderArgs, object customChildEntity, Field customChildField)
        {
            var customChildProperty = customChildDataProviderArgs.CustomChildProperties.Where(d => d.Key == customChildField.Code).Select(d => d.Value).FirstOrDefault();
            var customChildObjectProperty = customChildDataProviderArgs.CustomChildObjectProperties.Where(d => d.Key == customChildField.Code).Select(d => d.Value).FirstOrDefault();
            var customFieldValue = customChildObjectProperty.GetValue(customChildDataProviderArgs.CustomChildObjectPM);
            if (customFieldValue != null && customFieldValue.GetType() == typeof(CustomFieldClass))
            {
                customFieldValue = (customFieldValue as CustomFieldClass)?.Value;
            }
            string resolveCustomFieldValue = customFieldResolver.GetFieldValue2(customFieldValue, new ObjectField() { LookUpTableId = customChildField.LookUpTableId, Tenant = customChildField.Tenant, DataTypeCode = customChildField.DataTypeCode }, documentDataProviderArgs.Tenant);
            customChildProperty.SetValue(customChildEntity, customFieldResolver.SetFieldDataType(customChildField.DataTypeCode, resolveCustomFieldValue));
        }
    }

    public class CustomChildDataProviderArgs
    {
      public Dictionary<string, PropertyInfo> CustomChildProperties { get; set; }
        public Dictionary<string, PropertyInfo> CustomChildObjectProperties { get; set; }
        public CustomChildObjectPM CustomChildObjectPM { get; set; }
        public object CustomChildEntity { get; set; }
        public Field Field { get; set; }

        public Field CustomChildField { get; set; }

    }
}