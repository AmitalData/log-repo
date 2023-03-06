using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Helpers.WorkerRole.Importer;

namespace WebFreight.Web.Helpers.WorkerRole.DeploymentPackages.Validator
{
    public class DeploymentPackageImporterValidatingService
    {
        private DeploymentPackageImporterContext deploymentPackageImporterContext;
        private string exceptionMessege = "";

        public DeploymentPackageImporterValidatingService(DeploymentPackageImporterContext deploymentPackageImporterContext)
        {
            this.deploymentPackageImporterContext = deploymentPackageImporterContext;
        }

        public void ValidateImportedPackage()
        {
            List<IDeploymentPackageImporterValidatingService> validators = BuildValidationServices();
            foreach (IDeploymentPackageImporterValidatingService validator in validators)
            {
                exceptionMessege += validator.Validate(deploymentPackageImporterContext);
            }
            if (string.IsNullOrEmpty(exceptionMessege)) return;
            throw new Exception(exceptionMessege);
        }
        private List<IDeploymentPackageImporterValidatingService> BuildValidationServices()
        {
            List<IDeploymentPackageImporterValidatingService> validators = new List<IDeploymentPackageImporterValidatingService>();
            validators.Add(new DeploymentPackageCustomFieldsValidatingService());
            return validators;
        }

    }
}