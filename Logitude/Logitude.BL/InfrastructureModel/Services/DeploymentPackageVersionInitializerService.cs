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

namespace Logitude.BL.InfrastructureModel.Services
{
    public class DeploymentPackageVersionInitializerService
    {
        private DeploymentPackagePM deploymentPackagePM;
        private DeploymentPackagesVersionPM deploymentPackagesVersionPM;
        private Document document;
        private DocumentRepository documentrepository;
        DeploymentPackagesVersionService deploymentPackagesVersionService;
        public DeploymentPackageVersionInitializerService(DeploymentPackagePM deploymentPackagePM, IWebFreightContext iWebFreightContext)
        {
            this.deploymentPackagePM = deploymentPackagePM;
            deploymentPackagesVersionPM = new DeploymentPackagesVersionPM();
            documentrepository = new DocumentRepository(deploymentPackagePM.Tenant);
            deploymentPackagesVersionService = new DeploymentPackagesVersionService(iWebFreightContext, deploymentPackagePM.Tenant);
            CreateDocument();
        }
        public DeploymentPackagesVersionPM Create()
        {
            deploymentPackagesVersionPM.DocumentId = document.Id;
            deploymentPackagesVersionPM.DeploymentPackageID = deploymentPackagePM.Id;
            deploymentPackagesVersionPM.Tenant = deploymentPackagePM.Tenant;
            deploymentPackagesVersionService.Create(deploymentPackagesVersionPM, false);
            return deploymentPackagesVersionPM;
        }
        public void CreateDocument()
        {
            deploymentPackagePM.DeploymentPackageDetails = GetInstanceOfDeploymentPackageDetails();
            var deploymentPackageDetailsBytes = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(deploymentPackagePM.DeploymentPackageDetails));

            document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "json",
                FileSize = deploymentPackageDetailsBytes.Length,
                Tenant = Convert.ToInt32(deploymentPackagePM.Tenant),
                Id = IdCounter.GetNumber("Document", deploymentPackagePM.Tenant),
                HasFile = true,
                Folder = "others",
            };
            documentrepository.Add(document);
            documentrepository.SubmitChanges();
            StorageDataService.WriteFileOnStorage(new StorageDataArgs() { 
                FileName = document.Id,
                FolderName = document.Folder,
                Tenant = document.Tenant,
                FileData = deploymentPackageDetailsBytes,
                Extension = document.Extension
            });
        }

        private DeploymentPackageDetails GetInstanceOfDeploymentPackageDetails()
        {
            return new DeploymentPackageDetails
            {
                Name = deploymentPackagePM.Name,
                Code = deploymentPackagePM.Code,
                Description = deploymentPackagePM.Description,
                CustomFields = new List<ObjectFieldPM>()
            };
        }

   
        
    }
}
