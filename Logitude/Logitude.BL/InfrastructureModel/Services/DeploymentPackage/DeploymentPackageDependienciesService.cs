using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
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
            //
            return deploymentPackageDependiency;
        }
    }

    public class DeploymentPackageDependiency
    {
        public bool IsValid { get; set; }
        public List<DeploymentPackageMissingDependiency> MissingDependiencies { get; set; }
    }

    public class DeploymentPackageMissingDependiency
    {
        public string Name { get; set; }
        public string DataTypeName { get; set; }
        public string EntityName { get; set; }
        public string DependencyOn { get; set; }
    }
}
