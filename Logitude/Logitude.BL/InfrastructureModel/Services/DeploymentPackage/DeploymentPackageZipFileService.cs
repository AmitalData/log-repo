
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class DeploymentPackageZipFileService
    {
        private DeploymentPackageZipFileDetails deploymentPackageZipFileDetails;
        private Document document;
        private DocumentRepository documentRepository;
        private int tenant;
        public DeploymentPackageZipFileService(int tenant)
        {
            deploymentPackageZipFileDetails = new DeploymentPackageZipFileDetails();
            this.tenant = tenant;
            documentRepository = new DocumentRepository(tenant);
        }

        public string Create(DeploymentPackageDetails deploymentPackageDetails)//for name + desc + code (where to store them)
        {
            var deploymentPackageZipFileDetails = GetInstanceOfZipFileDetails();
            var deploymentPackageZipFileDetailsBytes = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(deploymentPackageZipFileDetails));
            document = GetInstanceOfDocument(deploymentPackageZipFileDetailsBytes);
            documentRepository.Add(document);
            documentRepository.SubmitChanges();
            StorageDataService.WriteFileOnStorage(new StorageDataArgs()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Tenant = document.Tenant,
                FileData = deploymentPackageZipFileDetailsBytes,
                Extension = document.Extension
            });

            return document.Id;
        }
        private DeploymentPackageZipFileDetails GetInstanceOfZipFileDetails()
        {
            return new DeploymentPackageZipFileDetails
            {
                DeploymentPackageDetailsList = new Dictionary<string, DeploymentPackageDetails>(),
                FilesNames = new List<string>()
            };
        }
        private Document GetInstanceOfDocument(byte[] deploymentPackageZipFileDetailsBytes)
        {
            return new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "zip",
                FileSize = deploymentPackageZipFileDetailsBytes.Length,
                Tenant = Convert.ToInt32(tenant),
                Id = IdCounter.GetNumber("Document", tenant),
                HasFile = true,
                Folder = "others",
            };
        }


        public void Update(DeploymentPackagePM deploymentPackagePM)
        {
            if (deploymentPackagePM.DocumentId == null) return;
            Document document = new DocumentRepository(deploymentPackagePM.Tenant).GetSingleDocument(deploymentPackagePM.Tenant, deploymentPackagePM.DocumentId);
            var deploymentPackageZipFileDetails = DeploymentPackageZipFileDetailsService.Build(deploymentPackagePM.DeploymentPackageDetails);
            var deploymentPackageZipFileDetailsBytes = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(deploymentPackageZipFileDetails));
            document.FileSize = deploymentPackageZipFileDetailsBytes.Length;
            documentRepository.Update(document);
            documentRepository.SubmitChanges();
            StorageDataService.WriteFileOnStorage(new StorageDataArgs()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Tenant = document.Tenant,
                FileData = deploymentPackageZipFileDetailsBytes,
                Extension = document.Extension
            });
            return;
        }

        private DeploymentPackageDetails GetDeploymentPackageDetails(string objectTableName)
        {
            //check if in DeploymentPackageDetailsList
            return new DeploymentPackageDetails();
        }

        public DeploymentPackageDetails GetDeserializedZipFileDetails(byte[] deploymentPackageZipFileDetailsBytes)
        {
            return JsonSerializer.Deserialize<DeploymentPackageDetails>(deploymentPackageZipFileDetailsBytes);
        }
    }

    public class DeploymentPackageZipFileDetails
    {
        public Dictionary<string, DeploymentPackageDetails> DeploymentPackageDetailsList;
        public List<string> FilesNames;
    }
}
