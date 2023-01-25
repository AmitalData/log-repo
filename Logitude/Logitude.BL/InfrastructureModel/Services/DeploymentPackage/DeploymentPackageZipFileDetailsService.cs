
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class DeploymentPackageZipFileDetailsService
    {
        private Dictionary<string, byte[]> deploymentPackageZipFileDetails;
        private Dictionary<string, DeploymentPackageDetails> deploymentPackageDetailsList;
        public DeploymentPackageZipFileDetailsService()
        {
            deploymentPackageZipFileDetails = new Dictionary<string, byte[]>();
            deploymentPackageDetailsList = new Dictionary<string, DeploymentPackageDetails>();
        }

        public Dictionary<string, byte[]> Build(DeploymentPackageDetails deploymentPackageDetails)
        {

            SetCustomFields(deploymentPackageDetails.CustomFields);
            SetCustomPickLists(deploymentPackageDetails.CustomPickLists);

            //AddPackageDescption(dataList , deploymentPackageDetails);
            return deploymentPackageZipFileDetails;
        }

        private void SetCustomFields(List<CustomFields> customFields)
        {
            customFields.ForEach(customField =>
            {
                var deploymentPackageDetails = GetDeploymentPackageDetailsByObjectName(customField.ObjectTableName);
                deploymentPackageDetails.CustomFields.Add(customField);
            });
        }
        private void SetCustomPickLists(List<CustomPickListItem> customPickLists)
        {

        }
        private DeploymentPackageDetails GetDeploymentPackageDetailsByObjectName(string objectTableName)
        {
            if (string.IsNullOrEmpty(objectTableName)) return null;
            DeploymentPackageDetails deploymentPackageDetails = null;
            if (!deploymentPackageDetailsList.TryGetValue(objectTableName,out deploymentPackageDetails)) return null;//need to add it first then return deploymentPackageDetails
            return deploymentPackageDetails;
        }
        //public void AddPackageDescption(Dictionary<string, byte[]> dataList , DeploymentPackageDetails deploymentPackageDetails)
        //{
        //    var ackageDescption = new PackageDescption() {Name = deploymentPackageDetails.Name };
        //    ackageDescption.Entites = new List<string>() { "Shipment"};

        //}
    }

    //public class PackageDescption
    //{
    //    public string Name { get; set; }
    //    public string Version { get; set; }
    //    public List<string> Entites { set; set; }

    //}

}
