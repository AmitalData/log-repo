using Logitude.BL.InfrastructureModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Services.DeploymentPackage.ImportingValidator
{
    public interface IDeploymentPackageImporterValidatingService
    {
        string Validate(DeploymentPackageDetails deploymentPackageDetails, int tenant);
    }
}
