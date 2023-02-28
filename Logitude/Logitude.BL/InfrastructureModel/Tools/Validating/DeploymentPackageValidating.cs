using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.Validating
{
    public class DeploymentPackageValidating
    {
        public DeploymentPackageValidating()
        {

        }

        public static void Validate(DeploymentPackagePM entityPM, DeploymentPackageRepository entityRepository)
        {
            if (!string.IsNullOrEmpty(entityPM.Code) && entityRepository.CheckIfDeploymentPackageCodeExist(entityPM.Code, entityPM.Tenant))
            {
                throw new Exception("Another Deployment Package already exist with this Code ");
            }
        }
    }
}
