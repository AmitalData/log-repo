using Logitude.BL.InfrastructureModel.EntityPMs;
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
    public class DeploymentPackageObjectFieldLookUpCustomTablesDependienciesService
    {
        public void Validate(DeploymentPackageDependiencyContext deploymentPackageDependiencyContext, List<CustomFields> customFields)
        {
            List<string> customObjectTablesNames = deploymentPackageDependiencyContext.CustomObjectTables?.Select(customObjectTable => customObjectTable.Name)?.ToList();
            if (customObjectTablesNames == null || customObjectTablesNames.Count() == 0) return;

            List<CustomFields> lookUpCustomFields = customFields.Where(customField => IsLookUpCustomFieldRelatedToCustomTable(customField, customObjectTablesNames)).ToList();
            if (lookUpCustomFields == null || lookUpCustomFields.Count() == 0) return;

            lookUpCustomFields.ForEach(lookUpCustomField =>
            {
                if (!IsValidDependencyLookUpCustomField())
                {
                    AddCustomFieldToMissingDependiencies(deploymentPackageDependiencyContext, lookUpCustomField);
                }
            });
        }

        private bool IsLookUpCustomFieldRelatedToCustomTable(CustomFields customField, List<string> customObjectTablesNames)
        {
            if (customField.DataTypeName != "LookUp") return false;
            if (string.IsNullOrEmpty(customField.LookUpTableName)) return false;

            return customObjectTablesNames.Contains(customField.LookUpTableName);
        }

        private void AddCustomFieldToMissingDependiencies(DeploymentPackageDependiencyContext deploymentPackageDependiencyContext, CustomFields lookUpCustomField)
        {
            deploymentPackageDependiencyContext.DeploymentPackageDependiencyResult.MissingDependiencies.Add(new DeploymentPackageMissingDependiency
            {
                Name = lookUpCustomField.DefaultText,
                DataTypeName = "Object Field",
                EntityName = lookUpCustomField.ObjectTableName,
                DependencyOn = lookUpCustomField.LookUpTableName,
            });
        }

        private bool IsValidDependencyLookUpCustomField()
        {
            //Need to check if custom object table included or not to the package
            return false;
        }
    }
}
