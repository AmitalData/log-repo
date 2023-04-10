using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logitude.BL.InfrastructureModel.Services.DeploymentPackage.ImportingValidator
{
    public class DeploymentPackageCustomFieldsValidatingService : IDeploymentPackageImporterValidatingService
    {
        private string exceptionMessage = "";
        private DeploymentPackageDetails deploymentPackageDetails;
        private int tenant;
        private ObjectTableQuery objectTableQuery;
        private ObjectFieldRepository objectFieldRepository;
        public DeploymentPackageCustomFieldsValidatingService()
        {
            objectTableQuery = new ObjectTableQuery(tenant);
            objectFieldRepository = new ObjectFieldRepository(tenant);
        }

        public string Validate(DeploymentPackageDetails deploymentPackageDetails, int tenant)
        {
            this.deploymentPackageDetails = deploymentPackageDetails;
            this.tenant = tenant;
            int customFieldsCountToAdd = 0;
            deploymentPackageDetails.CustomFields.GroupBy(customField => customField.ObjectTableName).ToList().ForEach(objectTableGroup => {
                customFieldsCountToAdd = deploymentPackageDetails.CustomFields.Where(o => o.ObjectTableName == objectTableGroup.Key).Count();
                ValidateImportedCustomFieldsToObjectTable(customFieldsCountToAdd, objectTableGroup.Key);
            });
            return exceptionMessage;
        }


        private void ValidateImportedCustomFieldsToObjectTable(int customFieldsCountToAdd, string objectTableName)
        {
            if (string.IsNullOrEmpty(objectTableName)) return;
            int allowedCount = GetAllowedCustomFieldsCount(objectTableName);
            if (customFieldsCountToAdd <= allowedCount) return;
            exceptionMessage += objectTableName + " object exceeds the limit of adding custom fields-";
        }

        private int GetAllowedCustomFieldsCount(string objectTableName)
        {
            ObjectTablePM objectTablePM = objectTableQuery.GetObjectTableByName(objectTableName, tenant);
            int customFieldsCount = objectFieldRepository.GetObjectFieldsByTenant(tenant).Where(objectfield => objectfield.ObjectTableId == objectTablePM.Id && objectfield.IsCustom == true).Count();
            return objectTablePM.MaxNumberOfCustomFields - customFieldsCount;
        }
    }
}