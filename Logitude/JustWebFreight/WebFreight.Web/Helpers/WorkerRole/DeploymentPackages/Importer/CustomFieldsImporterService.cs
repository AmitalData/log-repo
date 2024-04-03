using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Extensibility.Implementation;

namespace WebFreight.Web.Helpers.WorkerRole.Importer
{
    public class CustomFieldsImporterService : IDeploymentPackageImporterService
    {
        DeploymentPackageImporterContext importerContext;
        private IWebFreightContext webFreightContext;
        private ObjectTableQuery objectTableQuery;
        private ObjectFieldRepository objectFieldRepository;
        private ObjectFieldService objectFieldService;
        private ObjectFieldQuery objectFieldQuery;
        private int tenant;
        private List<string> fullObjectTables;
        private List<ObjectTablePM> objectTables;
        private List<ObjectFieldPM> objectFields;

        public CustomFieldsImporterService()
        {
            objectFields = new List<ObjectFieldPM>();
        }
        private void Initialize()
        {
            webFreightContext = WebFreightContext.GetContext(importerContext.Tenant);
            objectTableQuery = new ObjectTableQuery(tenant);
            objectFieldRepository = new ObjectFieldRepository(webFreightContext);
            objectFieldService = new ObjectFieldService(webFreightContext, importerContext.Tenant);
            objectFieldQuery = new ObjectFieldQuery(tenant);
            fullObjectTables = new List<string>();
            objectTables = new List<ObjectTablePM>();
        }
        public void Deploy(DeploymentPackageImporterContext context)
        {
            if (context.DeploymentPackageDetails == null) return;
            if (context.DeploymentPackageDetails.CustomFields == null || context.DeploymentPackageDetails.CustomFields.Count == 0) return;

            importerContext = context;
            tenant = context.Tenant;

            Initialize();
            BuildObjectFieldsList();
            CreateObjectFieldsAndSubmitChanges();
        }
        private void BuildObjectFieldsList()
        {
            foreach (CustomFields customfield in importerContext.DeploymentPackageDetails.CustomFields)
            {
                BuildObjectField(customfield);
            }
        }

        private void BuildObjectField(CustomFields customfield)
        {
            ObjectFieldPM objectFieldPM = GetInstanceOfObjectFieldPM(customfield);
            objectFields.Add(objectFieldPM);
            customfield.Code = objectFieldPM.FieldCode;
        }

        private void CreateObjectFieldsAndSubmitChanges()
        {
            foreach (ObjectFieldPM objectField in objectFields)
            {
                CreateObjectField(objectField);
            }
        }
        private void CreateObjectField(ObjectFieldPM objectField)
        {
            try
            {
                objectField.DefaultAdditionalTreeFilters = GetDefaultAdditionalTreeFilters(objectField.DefaultAdditionalFilters, objectField.ObjectTable_LookUpTableName);
                objectFieldService.Create(objectField);
            }
            catch (Exception exception)
            {
                HandleCreatingCustomObjectFieldException(objectField, exception);
            }
        }

        private void HandleCreatingCustomObjectFieldException(ObjectFieldPM objectField, Exception exception)
        {
            if (exception.Message != "An Object Field with the same code already exists") return;
            HandleDuplicatedObjectFieldsCodes(objectField);
        }

        private void HandleDuplicatedObjectFieldsCodes(ObjectFieldPM objectField)
        {
            objectField.Code = objectField.Code + "Copy" + objectFieldRepository.GetCustomObjectFieldCountByCodeAndCopies(objectField.Code, objectField.ObjectTableId, objectField.Tenant);
            objectFieldService.Create(objectField);
        }

        private QueryFilterItem GetDefaultAdditionalTreeFilters(string defaultAdditionalFilters, string lookUpTableName)
        {
            if (string.IsNullOrEmpty(defaultAdditionalFilters)) return null;
            QueryFilterItem defaultAdditionalTreeFilters = objectFieldQuery.DeserializeQueryFilterItem(defaultAdditionalFilters);
            HandleObjectFieldValue(defaultAdditionalTreeFilters, lookUpTableName);
            return defaultAdditionalTreeFilters;
        }
        private void HandleObjectFieldValue(QueryFilterItem queryFilterItem, string lookUpTableName)
        {
            queryFilterItem.FieldValue = queryFilterItem.Operator == "EqualField" ? MapNewCustomFieldCode(queryFilterItem.FieldValue, lookUpTableName) : queryFilterItem.FieldValue;
            queryFilterItem.FieldName = queryFilterItem.IsCustom ? MapNewCustomFieldCode(queryFilterItem.FieldName, lookUpTableName)?.ToString() : queryFilterItem.FieldName;

            if (queryFilterItem.QueryFilterItems == null) return;

            queryFilterItem.QueryFilterItems.ForEach(queryFilter => {
                HandleObjectFieldValue(queryFilter, lookUpTableName);
            });
        }
       public object MapNewCustomFieldCode(object value, string lookUpTableName)
        {
            if (value == null) return null;
            string combinedValue = value.ToString().Contains('.') ? value.ToString() : (lookUpTableName + '.' + value.ToString());
            var customField = importerContext.DeploymentPackageDetails.CustomFields.Where(c => GetFieldCode(c.FieldCode) == combinedValue).FirstOrDefault();
            if (customField == null) return value;
            string result = GetFieldCode(customField.Code);
            if (result.Contains('.') && result.Split('.')[0] == lookUpTableName) return result.Split('.')[1];
            return result;
        }

        private string GetFieldCode(string fieldCode)
        {
            if (string.IsNullOrEmpty(fieldCode)) return null;
            var parts = fieldCode.Split('.');
            if (parts.Length == 3) return parts[0] +'.'+ parts[2];
            return fieldCode;
        }
        private ObjectFieldPM GetInstanceOfObjectFieldPM(CustomFields customfield)
        {
            ObjectTablePM objectTablePM = GetObjectTablebyName(customfield.ObjectTableName);
            objectTablePM.CustomFieldsCount += 1;
            return new ObjectFieldPM()
            {
                Tenant = importerContext.Tenant,
                ObjectTableId = objectTablePM.Id,
                ObjectTableName = customfield.ObjectTableName,

                IsCustom = true,
                DisplayInEntityVariables = true,
                DisplayInList = true,
                CanFilter = true,
                IndexOrder = false,

                DataTypeCode = GetDataTypeCodeByName(customfield.DataTypeName),
                FullNameTextCodeCode = customfield.DefaultText,
                ListTextCodeCode = customfield.DefaultText,
                Code = customfield.Code,
                FieldName = customfield.Name,
                FieldCode = objectTablePM.Name + "." + tenant.ToString() + ".Field" + objectTablePM.CustomFieldsCount,
                HelpTextCodeCode = customfield.HelpText,

                LookUpTableId = GetObjectTablebyName(customfield.LookUpTableName)?.Id,

                MinLength = customfield.MinLength,
                MaxLength = customfield.MaxLength,
                NumberOfDigits = customfield.NumberOfDigits,
                DigitsAfterPoint = customfield.DigitsAfterPoint,

                DisplayOnly = customfield.DisplayOnly,
                MultiLine = customfield.MultiLine,
                IsRequiered = customfield.IsRequiered,
                CustomPickListCode = customfield.CustomPickListCode,
                DefaultAdditionalFilters = customfield.DefaultAdditionalFilters,
                ObjectTable_LookUpTableName = customfield.LookUpTableName,
            };
        }

        private ObjectTablePM GetObjectTablebyName(string objectTableName)
        {
            if (string.IsNullOrEmpty(objectTableName)) return null;
            ObjectTablePM objectTablePM = GetObjectTableByNameFromLocalCache(objectTableName);
            if (objectTablePM != null) return objectTablePM;
            objectTablePM = objectTableQuery.GetObjectTableByName(objectTableName, tenant);
            objectTablePM.CustomFieldsCount = objectFieldRepository.GetObjectFieldsByTenant(tenant).Where(objectfield => objectfield.ObjectTableId == objectTablePM.Id && objectfield.IsCustom == true).Count();
            objectTables.Add(objectTablePM);
            return objectTablePM;
        }

        private ObjectTablePM GetObjectTableByNameFromLocalCache(string objectTableName)
        {
            return objectTables.Where(o => o.Name == objectTableName).FirstOrDefault();
        }

        private string GetDataTypeCodeByName(string dataTypeName)
        {
            return new DataTypeRepository(webFreightContext).GetDataTypes().Where(d => d.Name == dataTypeName).FirstOrDefault()?.Code;
        }
    }
}
