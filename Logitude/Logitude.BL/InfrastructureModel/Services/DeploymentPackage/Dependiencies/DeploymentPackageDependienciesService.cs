using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Services.DeploymentPackage;
using Logitude.BL.InfrastructureModel.Services.DeploymentPackage.Dependiencies.ObjectFields;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Services.DeploymentPackage.Dependiencies
{
    public class DeploymentPackageDependienciesService
    {
        private DeploymentPackagePM deploymentPackagePM;
        private int tenant;
        private DeploymentPackageDependiency deploymentPackageDependiency;
        public DeploymentPackageDependienciesService(DeploymentPackagePM deploymentPackagePM)
        {
            this.deploymentPackagePM = deploymentPackagePM;
            tenant = deploymentPackagePM.Tenant;
            deploymentPackageDependiency = new DeploymentPackageDependiency
            {
                IsValid = true,
                MissingDependiencies = new List<DeploymentPackageMissingDependiency>()
            };
        }

        public DeploymentPackageDependiency Validate()
        {
            DeploymentPackageDependiencyContext deploymentPackageDependiencyContext = BuildDeploymentPackageDependiencyContext(deploymentPackageDependiency);
            List<IDeploymentPackageDependiency> deploymentPackageDependiencies = BuildDeploymentPackageDependiencies(deploymentPackageDependiencyContext);
            foreach (IDeploymentPackageDependiency deploymentPackage in deploymentPackageDependiencies)
            {
                deploymentPackage.Validate(deploymentPackageDependiencyContext);
            }
            return deploymentPackageDependiencyContext.DeploymentPackageDependiencyResult;
        }

        private DeploymentPackageDependiencyContext BuildDeploymentPackageDependiencyContext(DeploymentPackageDependiency deploymentPackageDependiency)
        {
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(tenant);
            List<ObjectTable> customObjectTables = objectTableRepository.GetObjectsByTenantOrTenantZero(tenant).Where(objectTable => objectTable.IsCustom).ToList();

            return new DeploymentPackageDependiencyContext
            {
                Tenant = tenant,
                DeploymentPackagePM = deploymentPackagePM,
                DeploymentPackageDependiencyResult = deploymentPackageDependiency,
                CustomObjectTables = customObjectTables
            };
        }
        private List<IDeploymentPackageDependiency> BuildDeploymentPackageDependiencies(DeploymentPackageDependiencyContext deploymentPackageDependiencyContext)
        {
            List<IDeploymentPackageDependiency> deploymentPackageDependiencies = new List<IDeploymentPackageDependiency>
            {
                new DeploymentPackageObjectFieldDependienciesService()
            };
            return deploymentPackageDependiencies;
        }
    }
}
