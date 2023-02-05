
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
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
            if (string.IsNullOrEmpty(customField.CustomPickListCode)) return;
            var isExistCustomPickList = extractedDeploymentPackageDetails.CustomPickLists.Where(d => d.Code == customField.CustomPickListCode).Any();
            if (!isExistCustomPickList)
            {
                extractedDeploymentPackageDetails.CustomPickLists.AddRange(deploymentPackageDetails.CustomPickLists.Where(p => p.Code == customField.CustomPickListCode).ToList());
            }
        }

        public DeploymentPackageDetails ExtractDeploymentPackageDetailsByDocumentId(string documentId, int tenant)
        {
            if (string.IsNullOrEmpty(documentId)) return null;
            StorageDataArgs storageDataArgs = GetStorageDataArgs(documentId, tenant);
            if (storageDataArgs == null) return GetInstanceOfDeploymentPackageDetails();
            byte[] deploymentPackageZipFileDetailsBytes = StorageDataService.ReadFileFromStorage(storageDataArgs);
            return Extract(deploymentPackageZipFileDetailsBytes);
        }

        private StorageDataArgs GetStorageDataArgs(string documentId, int tenant)
        {
            Document document = new DocumentRepository(tenant).GetSingleDocument(tenant, documentId);
            if (document == null) return null;
            return new StorageDataArgs()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Tenant = document.Tenant,
                Extension = document.Extension
            };
        }
    }

}
