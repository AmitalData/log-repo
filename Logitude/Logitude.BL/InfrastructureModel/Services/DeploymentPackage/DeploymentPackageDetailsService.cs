
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
    public class DeploymentPackageDetailsService
    {
        private Dictionary<string, DeploymentPackageDetails> deploymentPackageDetailsDectionary;
        private DeploymentPackagePM deploymentPackagePM;
        private DeploymentPackageDetails deploymentPackageMainDetails;
        public DeploymentPackageDetailsService(DeploymentPackagePM deploymentPackagePM)
        {
            deploymentPackageDetailsDectionary = new Dictionary<string, DeploymentPackageDetails>();
            deploymentPackageMainDetails = deploymentPackagePM.DeploymentPackageDetails;
            this.deploymentPackagePM = deploymentPackagePM;
        }

        public Dictionary<string, byte[]> Build()
        {
            BuildCustomFields();
            return BuildZipFile();
        }

        private Dictionary<string, byte[]> BuildZipFile()
        {
            Dictionary<string, byte[]> deploymentPackageZipFiles = new Dictionary<string, byte[]>();

            foreach (var deploymentPackageDetails in deploymentPackageDetailsDectionary)
            {
                var bytes = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(deploymentPackageDetails.Value));
                deploymentPackageZipFiles.Add(deploymentPackageDetails.Key, bytes);
            }
            AddPackageToZipFile(deploymentPackageZipFiles);
            return deploymentPackageZipFiles;
        }

        private void BuildCustomFields()
        {
            if (deploymentPackageMainDetails.CustomFields == null || deploymentPackageMainDetails.CustomFields.Count() == 0) return;
            deploymentPackageMainDetails.CustomFields.ForEach(customField =>
            {
                AddCustomField(customField);
            });    
        }

        private void AddCustomField(CustomFields customField)
        {
            DeploymentPackageDetails deploymentPackageDetails = GetDeploymentPackageDetailsByObjectName(customField.ObjectTableName);
            deploymentPackageDetails.CustomFields.Add(customField);
            if (string.IsNullOrEmpty(customField.CustomPickListCode)) return;
            var isExistCustomPickList = deploymentPackageDetails.CustomPickLists.Where(d => d.Code == customField.CustomPickListCode).Any();
            if (!isExistCustomPickList)
            {
                deploymentPackageDetails.CustomPickLists.AddRange(deploymentPackageMainDetails.CustomPickLists.Where(p => p.Code == customField.CustomPickListCode).ToList());
            }
        }

        private DeploymentPackageDetails GetDeploymentPackageDetailsByObjectName(string objectTableName)
        {
            if (string.IsNullOrEmpty(objectTableName)) return null;
            DeploymentPackageDetails deploymentPackageDetails = deploymentPackageDetailsDectionary.Where(d => d.Key == objectTableName).Select(d => d.Value).FirstOrDefault();
            if (deploymentPackageDetails != null) return deploymentPackageDetails;
            deploymentPackageDetails = GetInstanceOfDeploymentPackageDetails();
            deploymentPackageDetailsDectionary.Add(objectTableName, deploymentPackageDetails);
            return deploymentPackageDetails;
        }

        private DeploymentPackageDetails GetInstanceOfDeploymentPackageDetails()
        {
            return new DeploymentPackageDetails
            {
                CustomFields = new List<CustomFields>(),
                CustomPickLists = new List<CustomPickListItem>(),
            };
        }


        private void AddPackageToZipFile(Dictionary<string, byte[]> deploymentPackageZipFiles)
        {
            string versionName = new DeploymentPackagesVersionQuery(deploymentPackagePM.Tenant).GetDeploymentPackageVersionNameById(deploymentPackagePM.VersionId, deploymentPackagePM.Tenant);
            var packageDescription = new PackageDescription() { 
                Name = deploymentPackagePM.Name,
                Version = versionName,
                Files = new List<string>()
            };
            foreach (KeyValuePair<string, byte[]> entry in deploymentPackageZipFiles)
            {
                packageDescription.Files.Add(entry.Key + ".json");
            }
            var packageBytes = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(packageDescription));
            deploymentPackageZipFiles.Add("Packages", packageBytes);
        }

    }

    public class PackageDescription
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public List<string> Files { get; set; }

    }
}
