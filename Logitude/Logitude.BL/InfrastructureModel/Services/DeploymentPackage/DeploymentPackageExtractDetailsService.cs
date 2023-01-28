
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class DeploymentPackageExtractDetailsService
    {

        private DeploymentPackageDetails extractedDeploymentPackageDetails;

        public DeploymentPackageExtractDetailsService()
        {

        }

        public DeploymentPackageDetails Extract(byte[] deploymentPackageZipFileDetailsBytes)
        {
            extractedDeploymentPackageDetails = GetInstanceOfDeploymentPackageDetails();
            List<DeploymentPackageDetails> extractedDeploymentPackageDetailsList = new List<DeploymentPackageDetails>();
            Dictionary<string, byte[]> zipFileExtractedResult = ZipFileService.Extract(deploymentPackageZipFileDetailsBytes);
            foreach(var item in zipFileExtractedResult)
            {
                AddDeploymentPackageDetails(extractedDeploymentPackageDetailsList, item);
            }
            BuildExtractedDeploymentPackageDetails(extractedDeploymentPackageDetailsList);
            return extractedDeploymentPackageDetails;
        }

        private static void AddDeploymentPackageDetails(List<DeploymentPackageDetails> extractedDeploymentPackageDetailsList, KeyValuePair<string, byte[]> item)
        {
            if (item.Key == "Packages.json") return;
            extractedDeploymentPackageDetailsList.Add(JsonSerializer.Deserialize<DeploymentPackageDetails>(item.Value));
        }

        private DeploymentPackageDetails GetInstanceOfDeploymentPackageDetails()
        {
            return new DeploymentPackageDetails
            {
                CustomFields = new List<CustomFields>(),
                CustomPickLists = new List<CustomPickListItem>(),
            };
        }

        private void BuildExtractedDeploymentPackageDetails(List<DeploymentPackageDetails> extractedDeploymentPackageDetailsList)
        {
            if (extractedDeploymentPackageDetailsList == null || extractedDeploymentPackageDetailsList.Count == 0) return;
            extractedDeploymentPackageDetailsList.ForEach(item =>
            {
                MapExtractedDeploymentPackageDetails(item);
            });
        }

        private void MapExtractedDeploymentPackageDetails(DeploymentPackageDetails deploymentPackageDetails)
        {
            MapCustomFields(deploymentPackageDetails);
        }

        private void MapCustomFields(DeploymentPackageDetails deploymentPackageDetails)
        {
            if (deploymentPackageDetails.CustomFields == null || deploymentPackageDetails.CustomFields.Count() == 0) return;
            deploymentPackageDetails.CustomFields.ForEach(customField =>
            {
                MapCustomField(deploymentPackageDetails, customField);
            });
        }

        private void MapCustomField(DeploymentPackageDetails deploymentPackageDetails, CustomFields customField)
        {
            extractedDeploymentPackageDetails.CustomFields.Add(customField);
            var isExistCustomPickList = extractedDeploymentPackageDetails.CustomPickLists.Where(d => d.Code == customField.CustomPickListCode).Any();
            if (!isExistCustomPickList)
            {
                extractedDeploymentPackageDetails.CustomPickLists.AddRange(deploymentPackageDetails.CustomPickLists.Where(p => p.Code == customField.CustomPickListCode).ToList());
            }
        }
    }

}
