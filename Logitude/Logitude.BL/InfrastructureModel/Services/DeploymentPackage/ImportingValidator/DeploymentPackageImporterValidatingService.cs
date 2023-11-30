using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logitude.BL.InfrastructureModel.Services.DeploymentPackage.ImportingValidator
{
    public class DeploymentPackageImporterValidatingService
    {
        private DeploymentPackageDetails deploymentPackageDetails;
        private int tenant;
        private string exceptionMessege = "";

        public DeploymentPackageImporterValidatingService(DeploymentPackageDetails deploymentPackageDetails, int tenant)
        {
            this.deploymentPackageDetails = deploymentPackageDetails;
            this.tenant = tenant;
        }

        public void ValidateImportedPackage()
        {
            List<IDeploymentPackageImporterValidatingService> validators = BuildValidationServices();
            foreach (IDeploymentPackageImporterValidatingService validator in validators)
            {
                exceptionMessege += validator.Validate(deploymentPackageDetails, tenant);
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