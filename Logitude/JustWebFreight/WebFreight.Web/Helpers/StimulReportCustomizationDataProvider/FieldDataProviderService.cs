using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebFreight.Web.Helpers.StimulReportCustomizationDataProvider
{
    public class FieldDataProviderService
    {
        private List<Field> fields;
        private DocumentDataProviderArgs documentDataProviderArgs;
        public ObjectTable objectTable;
        private ObjectTableRepository objectTableRepository;
        private int tenant;
        public FieldDataProviderService(DocumentDataProviderArgs documentDataProviderArgs)
        {
            this.documentDataProviderArgs = documentDataProviderArgs;
            tenant = this.documentDataProviderArgs.DocumentTypeTemplatePM.Tenant;
            objectTableRepository = new ObjectTableRepository(tenant);

            objectTable = GetObjectTable();
            fields = new List<Field>();
        }

        public List<Field> GetFields()
        {
            GetStanderFields();
            GetCustomEntityFields();
            GetCustomFields();

            return fields;
        }


        public void GetStanderFields()
        {
            Type type = this.documentDataProviderArgs.Type;
            PropertyInfo[] props = type.GetProperties();
            foreach (PropertyInfo propertyInfo in props)
            {
                fields.Add(new Field() { Name = propertyInfo.Name, Type = propertyInfo.PropertyType });
            }
        }

        public void GetCustomEntityFields()
        {
            if (objectTable == null || !objectTable.AvailableInCustomization || !objectTable.SupportSubEntity) return;
            List<ObjectTable> objectTables = objectTableRepository.GetObjects().Where(d => d.Tenant == tenant && d.IsCustom && d.ParentObjectTableId == objectTable.Id).ToList();
            foreach (ObjectTable objectTable in objectTables)
            {
                AddCustomEntityField(objectTable);
            }

        }


        private void GetCustomFields()
        {
            if (objectTable == null || !objectTable.AvailableInCustomization || !objectTable.AllowCustomFields || objectTable.MaxNumberOfCustomFields <= 0) return;
            List<ObjectFieldList> objectFields = new ObjectFieldQuery(objectTable.Tenant).GetObjectFields().Where(d => d.ObjectTableId == objectTable.Id && d.Tenant == tenant && d.IsCustom).ToList();
            if (objectFields.Count() <= 0) return;
            foreach (ObjectFieldList objectField in objectFields)
            {
                fields.Add(new Field() { Name = AddCustomPrefixDisplyName(objectField.FullNameTextCodeDefaultText), Code = objectField.FieldName, Type = GetFieldDataType(objectField.DataTypeCode), DataTypeCode = objectField.DataTypeCode, IsCustom = objectField.IsCustom , LookUpTableId = objectField.LookUpTableId });
            }
        }

        private void AddCustomEntityField(ObjectTable objectTable)
        {
            string objectTableName = TranslateTextsClass.Translate(objectTable.FullNameTextCodeCode, objectTable.Tenant)?.Replace(" ", "");
            Field customEntityField = new Field() {IsChild = true, Name = AddCustomPrefixDisplyName(objectTableName), Code = objectTable.Name  ,IsList = true,IsCustom =true, AdditinalDetails = new AdditionalDetails() { Fields = new List<Field>() } };
            List<ObjectFieldList> objectFields = new ObjectFieldQuery(objectTable.Tenant).GetObjectFields().Where(d => d.ObjectTableId == objectTable.Id && d.Tenant == objectTable.Tenant).ToList();
            if (objectFields.Count() == 0) return;
            foreach (ObjectFieldList objectField in objectFields)
            {
                customEntityField.AdditinalDetails.Fields.Add(new Field() { Name = AddCustomPrefixDisplyName(objectField.FullNameTextCodeDefaultText), Code = objectField.FieldName, Type =GetFieldDataType(objectField.DataTypeCode) , DataTypeCode = objectField.DataTypeCode , IsCustom = objectField.IsCustom , LookUpTableId = objectField.LookUpTableId , Tenant = objectField.Tenant });
            }
            customEntityField.AdditinalDetails.Type = GenericClassCreator.Create(customEntityField.Name, customEntityField.AdditinalDetails.Fields);
            customEntityField.Type = typeof(List<>).MakeGenericType(customEntityField.AdditinalDetails.Type);
            fields.Add(customEntityField);
        }

        private Type GetFieldDataType(string dataTypeCode)
        {
            switch (dataTypeCode.Trim())
            {
                case "Text":
                case "Emails":
                case "nText":
                    {
                        return typeof(String);
                    }

                case "DateTime":
                case "Date":
                    {
                        return typeof(DateTime);
                    }

                case "UnsDecimal":
                case "Decimal":
                    {
                        return typeof(Decimal);
                    }

                case "Integer":
                case "UnsInteger":
                    {
                        return typeof(Int32);
                    }

                case "Double":
                case "SigDouble":
                    {
                        return typeof(Double);
                    }

                case "Boolean":
                    {
                        return typeof(Boolean);
                    }

                default:
                    {
                        return typeof(String);
                    }
            }
        }

        private string AddCustomPrefixDisplyName(string name)
        {
            return !string.IsNullOrEmpty(name) ? "c_" + name : ""; 
        }

        public ObjectTable GetObjectTable()
        {
            var objectTableId =!string.IsNullOrEmpty(documentDataProviderArgs.DocumentTypeTemplatePM.ObjectTableId )? documentDataProviderArgs.DocumentTypeTemplatePM.ObjectTableId : new DocumentTypeRepository(tenant).GetObjectTableIdByDocumentCode(documentDataProviderArgs.DocumentTypeTemplatePM.DocumentTypeCode, tenant);
            if (string.IsNullOrEmpty(objectTableId)) return null;
           return objectTableRepository.GetSingleObjectTable(objectTableId, tenant, true);
        }

    }
}