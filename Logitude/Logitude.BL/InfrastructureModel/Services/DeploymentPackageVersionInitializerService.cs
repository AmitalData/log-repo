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
        DeploymentPackageDocumentService deploymentPackageDocumentService;
        public DeploymentPackageVersionInitializerService(DeploymentPackagePM deploymentPackagePM, IWebFreightContext iWebFreightContext)
        {
            this.deploymentPackagePM = deploymentPackagePM;
            deploymentPackagesVersionPM = new DeploymentPackagesVersionPM();
            documentrepository = new DocumentRepository(deploymentPackagePM.Tenant);
            deploymentPackagesVersionService = new DeploymentPackagesVersionService(iWebFreightContext, deploymentPackagePM.Tenant);
            deploymentPackagesVersionQuery = new DeploymentPackagesVersionQuery(deploymentPackagePM.Tenant);
            deploymentPackageDocumentService = new DeploymentPackageDocumentService(deploymentPackagePM.Tenant, deploymentPackagePM);
        }
        public DeploymentPackagesVersionPM Create()
        {
            var documentId = deploymentPackagePM.DirectionId == "E" ? deploymentPackageDocumentService.Create() : deploymentPackagePM.DocumentId;
            deploymentPackagesVersionPM.DocumentId = documentId;
            deploymentPackagesVersionPM.DeploymentPackageID = deploymentPackagePM.Id;
            deploymentPackagesVersionPM.Tenant = deploymentPackagePM.Tenant;
            deploymentPackagesVersionService.Create(deploymentPackagesVersionPM, false);
            return deploymentPackagesVersionPM;
        }
        public DeploymentPackagesVersionPM Update()
        {
            deploymentPackageDocumentService.Update(deploymentPackagePM);
            deploymentPackagesVersionPM = deploymentPackagesVersionQuery.GetSinglePM(deploymentPackagePM.VersionId, deploymentPackagePM.Tenant);
            deploymentPackagesVersionPM.IsExported = deploymentPackagePM.IsExported;
            deploymentPackagesVersionService.Update(deploymentPackagesVersionPM);
            return deploymentPackagesVersionPM;
        }

    }
}
