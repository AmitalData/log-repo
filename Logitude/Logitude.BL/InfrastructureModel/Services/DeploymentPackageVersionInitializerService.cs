using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using System;
using System.Text;
using System.Text.Json;
using Logitude.Server.Tools.StorageService;
using System.Collections.Generic;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.InfrastructureModel.EntityQueries;

namespace Logitude.BL.InfrastructureModel.Services
{
    public class DeploymentPackageVersionInitializerService
    {
        private DeploymentPackagePM deploymentPackagePM;
        private DeploymentPackagesVersionPM deploymentPackagesVersionPM;
        private Document document;
        private DocumentRepository documentrepository;
        DeploymentPackagesVersionService deploymentPackagesVersionService;
        DeploymentPackagesVersionQuery deploymentPackagesVersionQuery;
        DeploymentPackageZipFileService deploymentPackageZipFileService;
        public DeploymentPackageVersionInitializerService(DeploymentPackagePM deploymentPackagePM, IWebFreightContext iWebFreightContext)
        {
            this.deploymentPackagePM = deploymentPackagePM;
            deploymentPackagesVersionPM = new DeploymentPackagesVersionPM();
            documentrepository = new DocumentRepository(deploymentPackagePM.Tenant);
            deploymentPackagesVersionService = new DeploymentPackagesVersionService(iWebFreightContext, deploymentPackagePM.Tenant);
            deploymentPackagesVersionQuery = new DeploymentPackagesVersionQuery(deploymentPackagePM.Tenant);
            deploymentPackageZipFileService = new DeploymentPackageZipFileService(deploymentPackagePM.Tenant);
        }
        public DeploymentPackagesVersionPM Create()
        {
            var documentId = deploymentPackageZipFileService.Create(deploymentPackagePM.DeploymentPackageDetails);
            deploymentPackagesVersionPM.DocumentId = documentId;
            deploymentPackagesVersionPM.DeploymentPackageID = deploymentPackagePM.Id;
            deploymentPackagesVersionPM.Tenant = deploymentPackagePM.Tenant;
            deploymentPackagesVersionService.Create(deploymentPackagesVersionPM, false);
            return deploymentPackagesVersionPM;
        }
        public DeploymentPackagesVersionPM Update()
        {
            UpdateDocument(deploymentPackagePM);
            deploymentPackagesVersionPM = deploymentPackagesVersionQuery.GetSinglePM(deploymentPackagePM.VersionId, deploymentPackagePM.Tenant);
            deploymentPackagesVersionPM.IsExported = deploymentPackagePM.IsExported;
            deploymentPackagesVersionService.Update(deploymentPackagesVersionPM);
            return deploymentPackagesVersionPM;
        }

        public void CreateDocument()
        {
            deploymentPackagePM.DeploymentPackageDetails = GetInstanceOfDeploymentPackageDetails();
            var deploymentPackageDetailsBytes = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(deploymentPackagePM.DeploymentPackageDetails));
            document = GetInstanceOfDocument(deploymentPackageDetailsBytes);
            documentrepository.Add(document);
            documentrepository.SubmitChanges();
            StorageDataService.WriteFileOnStorage(new StorageDataArgs()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Tenant = document.Tenant,
                FileData = deploymentPackageDetailsBytes,
                Extension = document.Extension
            });
        }

        private Document GetInstanceOfDocument(byte[] deploymentPackageDetailsBytes)
        {
           return new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "json",
                FileSize = deploymentPackageDetailsBytes.Length,
                Tenant = Convert.ToInt32(deploymentPackagePM.Tenant),
                Id = IdCounter.GetNumber("Document", deploymentPackagePM.Tenant),
                HasFile = true,
                Folder = "others",
            };
        }

        private DeploymentPackageDetails GetInstanceOfDeploymentPackageDetails()
        {
            return new DeploymentPackageDetails
            {
                Name = deploymentPackagePM.Name,
                Code = deploymentPackagePM.Code,
                Description = deploymentPackagePM.Description,
                CustomFields = new List<CustomFields>(),
                CustomPickLists = new List<CustomPickListItem>()
            };
        }

        private void UpdateDocument(DeploymentPackagePM deploymentPackagePM)
        {
            if (deploymentPackagePM.DocumentId == null) return;
            Document document = new DocumentRepository(deploymentPackagePM.Tenant).GetSingleDocument(deploymentPackagePM.Tenant, deploymentPackagePM.DocumentId);
            var deploymentPackageDetailsBytes = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(deploymentPackagePM.DeploymentPackageDetails));
            document.FileSize = deploymentPackageDetailsBytes.Length;
            DocumentRepository documentrepository = new DocumentRepository(deploymentPackagePM.Tenant);
            documentrepository.Update(document);
            documentrepository.SubmitChanges();
            StorageDataService.WriteFileOnStorage(new StorageDataArgs()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Tenant = document.Tenant,
                FileData = deploymentPackageDetailsBytes,
                Extension = document.Extension
            });
        }




    }
}
