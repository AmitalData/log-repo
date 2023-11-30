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

        public static void ValidateCode(string code, int tenant, DeploymentPackageRepository entityRepository)
        {
            if (!string.IsNullOrEmpty(code) && entityRepository.CheckIfDeploymentPackageCodeExist(code, tenant))
            {
                throw new Exception("Another Deployment Package already exist with this Code ");
            }
        }
    }
}
