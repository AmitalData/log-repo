using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Services.DeploymentPackage.Dependiencies
{
    public interface IDeploymentPackageDependiency
    {
        void Validate(DeploymentPackageDependiencyContext deploymentPackageDependiencyContext);
    }

    public class DeploymentPackageDependiencyContext
    {
        public DeploymentPackageDependiency DeploymentPackageDependiencyResult { get; set; }
        public int Tenant { get; set; }
        public DeploymentPackagePM DeploymentPackagePM { get; set; }
        public List<ObjectTable> CustomObjectTables { get; set; }
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
