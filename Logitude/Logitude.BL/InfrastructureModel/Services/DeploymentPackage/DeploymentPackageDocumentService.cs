
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class DeploymentPackageDocumentService
    {
        private Document document;
        private DocumentRepository documentRepository;
        private DeploymentPackagePM deploymentPackagePM;
        private int tenant;

        public DeploymentPackageDocumentService(int tenant, DeploymentPackagePM deploymentPackagePM)
        {
            this.tenant = tenant;
            documentRepository = new DocumentRepository(tenant);
            this.deploymentPackagePM = deploymentPackagePM;
        }

        public string Create()
        {
            var deploymentPackageZipFileBytes = ZipFileService.Compress(new Dictionary<string, byte[]>());
            document = GetInstanceOfDocument(deploymentPackageZipFileBytes);
            documentRepository.Add(document);
            documentRepository.SubmitChanges();
            StorageDataService.WriteFileOnStorage(new StorageDataArgs()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Tenant = document.Tenant,
                FileData = deploymentPackageZipFileBytes,
                Extension = document.Extension
            });

            return document.Id;
        }
        private Document GetInstanceOfDocument(byte[] deploymentPackageZipFileBytes)
        {
            return new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "zip",
                FileSize = deploymentPackageZipFileBytes.Length,
                Tenant = Convert.ToInt32(tenant),
                Id = IdCounter.GetNumber("Document", tenant),
                HasFile = true,
                Folder = "others",
                FileName = deploymentPackagePM.Name + "_1.0"
            };
        }
        public void Update(DeploymentPackagePM deploymentPackagePM)
        {
            if (deploymentPackagePM.DocumentId == null) return;
            Document document = new DocumentRepository(deploymentPackagePM.Tenant).GetSingleDocument(deploymentPackagePM.Tenant, deploymentPackagePM.DocumentId);
            string versionName = new DeploymentPackagesVersionQuery(deploymentPackagePM.Tenant).GetDeploymentPackageVersionNameById(deploymentPackagePM.VersionId, deploymentPackagePM.Tenant);
            var deploymentPackageZipFileDetailsList = new DeploymentPackageDetailsService(deploymentPackagePM).Build();
            var deploymentPackageZipFileDetailsBytes = ZipFileService.Compress(deploymentPackageZipFileDetailsList);
            document.FileSize = deploymentPackageZipFileDetailsBytes.Length;
            document.FileName = deploymentPackagePM.Name + "_" + versionName;
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

        public bool Delete(string documentId, int tenant)
        {
            if (string.IsNullOrEmpty(documentId)) return false;
            Document document = documentRepository.GetSingleDocument(tenant, documentId);
            if (document == null) return false;
            DeleteFileFromStorage(document, tenant);
            documentRepository.Remove(document);
            documentRepository.SubmitChanges();
            return true;
        }

        private static void DeleteFileFromStorage(Document document, int tenant)
        {
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = tenant,
                FileSize = document.FileSize,

            };
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            storageservice.Delete(fileInfo);
        }
    }

}
