using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Services.DeploymentPackage.Dependiencies.ObjectFields
{
    public class DeploymentPackageObjectFieldAdditionalFiltersDependienciesService
    {
        private List<CustomFields> customFields;
        private DeploymentPackageDependiencyContext deploymentPackageDependiencyContext;
        private List<ObjectFieldPM> customObjectFieldPMs;
        public void Validate(DeploymentPackageDependiencyContext deploymentPackageDependiencyContext, List<CustomFields> customFields)
        {
            this.deploymentPackageDependiencyContext = deploymentPackageDependiencyContext;
            this.customFields = customFields;
            List<CustomFields> additionalFiltersCustomFields = customFields.Where(customField => IsHaveDefaultAdditionalFilters(customField)).ToList();

            if (additionalFiltersCustomFields.Count() == 0) return;

            customObjectFieldPMs = GetCustomObjectFieldPMs(deploymentPackageDependiencyContext.Tenant);
            if (customObjectFieldPMs.Count() == 0) return;

            additionalFiltersCustomFields.ForEach(lookUpCustomField =>
            {
                TryValidateLookUpCustomField(lookUpCustomField);
            });
        }

        private bool IsHaveDefaultAdditionalFilters(CustomFields customField)
        {
            if (customField.DataTypeName != "LookUp") return false;
            if (string.IsNullOrEmpty(customField.LookUpTableName)) return false;
            if (string.IsNullOrEmpty(customField.DefaultAdditionalFilters)) return false;

            return true;
        }

        private List<ObjectFieldPM> GetCustomObjectFieldPMs(int tenant)
        {
            ObjectFieldQuery objectFieldQuery = new ObjectFieldQuery(tenant);
            List<ObjectFieldPM> customObjectFieldPMs = objectFieldQuery.GetObjectFieldPMs().Where(objectField => objectField.Tenant == tenant && objectField.IsCustom).ToList();
            return customObjectFieldPMs;
        }

        private void TryValidateLookUpCustomField(CustomFields lookUpCustomField)
        {
            QueryFilterItem defaultAdditionalFilters = JsonSerializer.Deserialize<QueryFilterItem>(lookUpCustomField.DefaultAdditionalFilters);
            string dependencyMessage = "";
            dependencyMessage = ValidateTreeObjectField(defaultAdditionalFilters, lookUpCustomField, dependencyMessage);
            if (!string.IsNullOrEmpty(dependencyMessage) && dependencyMessage.StartsWith(", "))
            {
                AddCustomFieldToMissingDependiencies(deploymentPackageDependiencyContext, lookUpCustomField, dependencyMessage);
            }
        }

        private string ValidateTreeObjectField(QueryFilterItem queryFilterItem, CustomFields lookUpCustomField, string dependencyMessage)
        {
            dependencyMessage += ValidateObjectField(queryFilterItem, lookUpCustomField);
            if (queryFilterItem.QueryFilterItems == null) return dependencyMessage;

            queryFilterItem.QueryFilterItems.ForEach(queryFilter =>
            {
                dependencyMessage = ValidateTreeObjectField(queryFilter, lookUpCustomField, dependencyMessage);
            });

            return dependencyMessage;
        }

        private string ValidateObjectField(QueryFilterItem queryFilterItem, CustomFields lookUpCustomField)
        {
            string validationMessage = "";
            if (queryFilterItem.IsCustom)
            {
                validationMessage = ValidateObjectFieldValue(queryFilterItem.FieldName, lookUpCustomField);
            }

            if (queryFilterItem.Operator != null && queryFilterItem.Operator.Contains("Field"))
            {
                validationMessage += ValidateObjectFieldValue(queryFilterItem.FieldValue, lookUpCustomField);
            }

            return validationMessage;
        }

        private string ValidateObjectFieldValue(object queryFilterItemFieldValue, CustomFields lookUpCustomField)
        {
            if (queryFilterItemFieldValue == null) return "";
            string fieldValue = queryFilterItemFieldValue.ToString();

            if (string.IsNullOrEmpty(fieldValue)) return "";
            if (!fieldValue.Contains("Field")) return "";

            ObjectFieldPM selectedFieldName = GetSelectedCustomObjectField(lookUpCustomField.LookUpTableName, deploymentPackageDependiencyContext.Tenant, fieldValue);
            if (fieldValue.StartsWith("Field") && selectedFieldName != null)
            {
                return ", " + selectedFieldName.FullNameTextCodeDefaultText;
            }

            if (!fieldValue.StartsWith(lookUpCustomField.ObjectTableName + ".Field")) return "";

            string fieldName = fieldValue.Replace(lookUpCustomField.ObjectTableName + ".", "");
            ObjectFieldPM selectedFieldValue = GetSelectedCustomObjectField(lookUpCustomField.ObjectTableName, deploymentPackageDependiencyContext.Tenant, fieldName);
            if (selectedFieldValue != null)
            {
                return ", " + selectedFieldValue.FullNameTextCodeDefaultText;
            }
            return "";
        }

        private ObjectFieldPM GetSelectedCustomObjectField(string objectTableName, int tenant, string fieldName)
        {
            string fieldCode = objectTableName + "." + tenant + "." + fieldName;
            ObjectFieldPM customObjectFieldPM = customObjectFieldPMs.FirstOrDefault(customObjectField => customObjectField.FieldCode == fieldCode);

            if (customObjectFieldPM == null) return null;
            bool isFieldExist = customFields.Any(c => c.FieldCode == customObjectFieldPM.FieldCode);
            if(isFieldExist) return null;

            return customObjectFieldPM;
        }

        private void AddCustomFieldToMissingDependiencies(DeploymentPackageDependiencyContext deploymentPackageDependiencyContext, CustomFields lookUpCustomField, string dependencyOn)
        {
            string fieldDependencyOn = dependencyOn.Remove(0, 2);
            deploymentPackageDependiencyContext.DeploymentPackageDependiencyResult.MissingDependiencies.Add(new DeploymentPackageMissingDependiency
            {
                Name = lookUpCustomField.DefaultText,
                DataTypeName = "Object Field",
                EntityName = lookUpCustomField.ObjectTableName,
                DependencyOn = fieldDependencyOn,
            });
        }
    }
}
