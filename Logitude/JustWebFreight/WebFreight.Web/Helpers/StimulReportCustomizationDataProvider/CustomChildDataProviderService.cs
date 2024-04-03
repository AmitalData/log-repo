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

        private CustomFieldDataProviderService customFieldDataProviderService;
        public CustomChildDataProviderService(List<Field> fields, DocumentDataProviderArgs documentDataProviderArgs , string parentObjectTableName)
        {
            this.documentDataProviderArgs = documentDataProviderArgs;
            this.fields = fields;
            customFieldResolver = new CustomFieldResolver(documentDataProviderArgs.DocumentTypeTemplatePM.Tenant);
            customChildEntities = new CustomChildEntityService(new CustomChildEntityArgs() { ParentEntityId = documentDataProviderArgs.EntityId, ParentObjectTableName = parentObjectTableName, Tenant = documentDataProviderArgs.DocumentTypeTemplatePM.Tenant }).BuildCustomChildEntities();
            customFieldDataProviderService = new CustomFieldDataProviderService(fields , documentDataProviderArgs, parentObjectTableName);
        }

        public void Set(object dataProvider)
        {
            foreach (Field field in fields.Where(d => d.IsCustom && d.IsChild && d.IsList).ToList())
            {
                customFieldDataProviderService.SetFieldValue(dataProvider , field.Name , GetCustomChildObjects(dataProvider ,field));
            }
        }

        private IList GetCustomChildObjects(object dataProvider, Field field)
        {
            CustomChildEntity customChildEntity = customChildEntities.Where(d => d.Name == field.Code).FirstOrDefault();
            if (customChildEntity == null || customChildEntity.Values == null || customChildEntity.Values.Count() == 0) return null;
            var customChildObjects = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(field.AdditinalDetails.Type));
            foreach (CustomChildObjectPM customChildObjectPM in customChildEntity.Values.OrderBy(d=>d.CreateDate).ToList())
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
                customFieldDataProviderService.SetFieldValue(customChildObject, field.Name, customFieldDataProviderService.GetFieldValue(customChildObjectPM, field));
            }
            return customChildObject;
        }
    }

}