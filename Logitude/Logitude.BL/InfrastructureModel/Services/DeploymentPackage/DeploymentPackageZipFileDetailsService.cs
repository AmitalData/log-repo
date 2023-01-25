
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools.Counters;
using System;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class DeploymentPackageZipFileDetailsService
    {
        public DeploymentPackageZipFileDetailsService(int tenant)
        {

        }

        public static DeploymentPackageZipFileDetails Build(DeploymentPackageDetails deploymentPackageDetails)
        {
            return new DeploymentPackageZipFileDetails();
        }
    }

}
