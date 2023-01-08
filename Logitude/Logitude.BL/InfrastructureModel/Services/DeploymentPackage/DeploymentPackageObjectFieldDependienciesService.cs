using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Services.DeploymentPackage
{
    class DeploymentPackageObjectFieldDependienciesService : IDeploymentPackageDependiency
    {

        public void Validate(DeploymentPackageDependiencyContext deploymentPackageDependiencyContext)
        {

            List<CustomFields> customFields = deploymentPackageDependiencyContext?.DeploymentPackagePM?.DeploymentPackageDetails?.CustomFields;
            if (customFields == null || customFields.Count() == 0) return;

            List<string> customObjectTablesNames = deploymentPackageDependiencyContext.CustomObjectTables?.Select(customObjectTable => customObjectTable.Name)?.ToList();
            if (customObjectTablesNames == null || customObjectTablesNames.Count() == 0) return;

            List<CustomFields> lookUpCustomFields = customFields.Where(customField => IsLookUpCustomFieldRelatedToCustomTable(customField, customObjectTablesNames)).ToList();
            if (lookUpCustomFields == null || lookUpCustomFields.Count() == 0) return;

            lookUpCustomFields.ForEach(lookUpCustomField =>
            {
                if (!IsValidDependencyLookUpCustomField()) {
                    AddCustomFieldToMissingDependiencies(deploymentPackageDependiencyContext, lookUpCustomField);
                }
            });

            if(deploymentPackageDependiencyContext.DeploymentPackageDependiencyResult.MissingDependiencies.Count() > 0)
            {
                deploymentPackageDependiencyContext.DeploymentPackageDependiencyResult.IsValid = false;
            }
        }

        private static bool IsLookUpCustomFieldRelatedToCustomTable(CustomFields customField, List<string> customObjectTablesNames)
        {
            if (customField.DataTypeName != "LookUp") return false;
            if (string.IsNullOrEmpty(customField.LookUpTableName)) return false;

            return customObjectTablesNames.Contains(customField.LookUpTableName);
        }

        private static void AddCustomFieldToMissingDependiencies(DeploymentPackageDependiencyContext deploymentPackageDependiencyContext, CustomFields lookUpCustomField)
        {
            deploymentPackageDependiencyContext.DeploymentPackageDependiencyResult.MissingDependiencies.Add(new DeploymentPackageMissingDependiency
            {
                Name = lookUpCustomField.Name,
                DataTypeName = "Object Field",
                EntityName = lookUpCustomField.ObjectTableName,
                DependencyOn = lookUpCustomField.LookUpTableName,
            });
        }

        private static bool IsValidDependencyLookUpCustomField()
        {
            //Need to check if custom object table included or not to the package
            return false;
        }
    }
}
