
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class DeploymentPackageDetailsListService
    {
        private DeploymentPackageDetails deploymentPackageDetails;
        private List<DeploymentPackageDetailsList> deploymentPackageDetailsList;
        private int tenant;
        public DeploymentPackageDetailsListService(int tenant)
        {
            this.tenant = tenant;
            deploymentPackageDetailsList = new List<DeploymentPackageDetailsList>();
        }

        public List<DeploymentPackageDetailsList> GetDeploymentPackageDetailsListByDocumentId(string documentId)
        {
            if (string.IsNullOrEmpty(documentId)) return new List<DeploymentPackageDetailsList>();
            deploymentPackageDetails = new DeploymentPackageExtractDetailsService().ExtractDeploymentPackageDetailsByDocumentId(documentId, tenant);

            this.BuildCustomFields();

            return deploymentPackageDetailsList;
        }

        private void BuildCustomFields()
        {
            foreach(CustomFields customField in deploymentPackageDetails.CustomFields)
            {
                var deploymentPackageDetailsListItem = this.GetDeploymentPackageDetailsListItem(customField.DefaultText, "Custom Field", customField.ObjectTableName);
                this.deploymentPackageDetailsList.Add(deploymentPackageDetailsListItem);
            }

        }


        private DeploymentPackageDetailsList GetDeploymentPackageDetailsListItem(string componentName, string type, string entity)
        {
            var deploymentPackageDetailsListItem = new DeploymentPackageDetailsList();
            deploymentPackageDetailsListItem.ComponentName = componentName;
            deploymentPackageDetailsListItem.Type = type;
            deploymentPackageDetailsListItem.Entity = entity;
            return deploymentPackageDetailsListItem;
        }

    }

    public class DeploymentPackageDetailsList
    {
        public string ComponentName { get; set; }
        public string Type { get; set; }
        public string Entity { get; set; }

    }
}
