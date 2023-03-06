using Logitude.BL.InfrastructureModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFreight.Web.Helpers.WorkerRole.DeploymentPackages.Validator;

namespace WebFreight.Web.Helpers.WorkerRole.Importer
{
    public class DeploymentPackageImporter
    {
        private DeploymentPackageDetails deploymentPackageDetails;
        private int tenant;
        public DeploymentPackageImporter(DeploymentPackageDetails deploymentPackageDetails, int tenant)
        {
            this.deploymentPackageDetails = deploymentPackageDetails;
            this.tenant = tenant;
        }


        public void Run()
        {
            DeploymentPackageImporterContext deploymentPackageImporterContext = BuildContext();
            new DeploymentPackageImporterValidatingService(deploymentPackageImporterContext).ValidateImportedPackage();
            List<IDeploymentPackageImporterService> expressions = BuildImportersServices();
            foreach (IDeploymentPackageImporterService expression in expressions)
            {
                expression.Deploy(deploymentPackageImporterContext);
            }

        }

        private List<IDeploymentPackageImporterService> BuildImportersServices()
        {
            List<IDeploymentPackageImporterService> expressions = new List<IDeploymentPackageImporterService>();
            expressions.Add(new CustomPickListsImporterService());
            expressions.Add(new CustomFieldsImporterService());
            return expressions;
        }

        private DeploymentPackageImporterContext BuildContext()
        {
            return new DeploymentPackageImporterContext()
            {
                DeploymentPackageDetails = deploymentPackageDetails,
                Tenant = tenant
            };

        }
    }
}
