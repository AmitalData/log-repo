using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFreight.Web.Helpers.WorkerRole.Importer;

namespace WebFreight.Web.Helpers.WorkerRole.DeploymentPackages.Validator
{
    public interface IDeploymentPackageImporterValidatingService
    {
        string Validate(DeploymentPackageImporterContext context);
    }
}
