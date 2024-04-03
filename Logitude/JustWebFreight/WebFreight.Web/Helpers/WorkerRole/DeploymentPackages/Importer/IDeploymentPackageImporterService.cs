using Logitude.BL.InfrastructureModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFreight.Web.Helpers.WorkerRole.Importer
{
    public interface IDeploymentPackageImporterService
    {
        void Deploy(DeploymentPackageImporterContext context);
    }

    public class DeploymentPackageImporterContext
    {
        public DeploymentPackageDetails DeploymentPackageDetails;
        public int Tenant;
    }
}
