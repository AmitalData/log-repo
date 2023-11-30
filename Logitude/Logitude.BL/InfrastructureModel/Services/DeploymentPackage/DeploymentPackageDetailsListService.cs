
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Services.DeploymentPackage.ImportingValidator;
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
        private DeploymentPackageDetailsListArgs deploymentPackageDetailsListArgs;
        public DeploymentPackageDetailsListService(int tenant)
        {
            this.tenant = tenant;
            deploymentPackageDetailsList = new List<DeploymentPackageDetailsList>();
        }

        public DeploymentPackageDetailsListArgs GetDeploymentPackageDetailsListByDocumentId(string documentId)
        {
            deploymentPackageDetailsListArgs = GetInstanceOfDeploymentPackageDetailsListArgs();
            if (string.IsNullOrEmpty(documentId)) return deploymentPackageDetailsListArgs;

            try
            {
                MapDeploymentPackageDetailsListArgs(documentId);
                return deploymentPackageDetailsListArgs;
            }
            catch (Exception ex)
            {
                deploymentPackageDetailsListArgs.IsValidZipFile = false;
                return deploymentPackageDetailsListArgs;
            }
            
        }

        private void MapDeploymentPackageDetailsListArgs(string documentId)
        {
            deploymentPackageDetails = new DeploymentPackageExtractDetailsService().ExtractDeploymentPackageDetailsByDocumentId(documentId, tenant);
            this.BuildCustomFields();
            deploymentPackageDetailsListArgs.DeploymentPackageDetailsList = deploymentPackageDetailsList;
            deploymentPackageDetailsListArgs.IsValidZipFile = true;
            ValidateImportedDeploymentPackage();
        }

        private void ValidateImportedDeploymentPackage()
        {
            try
            {
                new DeploymentPackageImporterValidatingService(deploymentPackageDetails, tenant).ValidateImportedPackage();
            }
            catch(Exception ex)
            {
                deploymentPackageDetailsListArgs.ValidationMessage = ex.Message;
            }
        }

        private DeploymentPackageDetailsListArgs GetInstanceOfDeploymentPackageDetailsListArgs()
        {
            return new DeploymentPackageDetailsListArgs()
            {
                DeploymentPackageDetailsList = new List<DeploymentPackageDetailsList>(),
                ValidationMessage = "",
                IsValidZipFile = true
            };
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
    public class DeploymentPackageDetailsListArgs
    {
        public List<DeploymentPackageDetailsList> DeploymentPackageDetailsList;
        public string ValidationMessage;
        public bool IsValidZipFile;
    }
}
