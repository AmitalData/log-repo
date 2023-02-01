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
    public class DeploymentPackageObjectFieldDependienciesService : IDeploymentPackageDependiency
    {
        public void Validate(DeploymentPackageDependiencyContext deploymentPackageDependiencyContext)
        {
            List<CustomFields> customFields = deploymentPackageDependiencyContext?.DeploymentPackagePM?.DeploymentPackageDetails?.CustomFields;
            if (customFields == null || customFields.Count() == 0) return;

            new DeploymentPackageObjectFieldLookUpCustomTablesDependienciesService().Validate(deploymentPackageDependiencyContext, customFields);
            new DeploymentPackageObjectFieldAdditionalFiltersDependienciesService().Validate(deploymentPackageDependiencyContext, customFields);

            if (deploymentPackageDependiencyContext.DeploymentPackageDependiencyResult.MissingDependiencies.Count() > 0)
            {
                deploymentPackageDependiencyContext.DeploymentPackageDependiencyResult.IsValid = false;
            }
        }
    }
}
