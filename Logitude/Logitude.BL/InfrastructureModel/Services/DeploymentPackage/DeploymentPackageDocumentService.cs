
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
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
        private int tenant;

        public DeploymentPackageDocumentService(int tenant)
        {
            //deploymentPackageDetailsList = new List<DeploymentPackageDetails>();
            this.tenant = tenant;
            documentRepository = new DocumentRepository(tenant);
        }

        public string Create(DeploymentPackageDetails deploymentPackageDetails)
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
            };
        }
        public void Update(DeploymentPackagePM deploymentPackagePM)
        {
            if (deploymentPackagePM.DocumentId == null) return;
            Document document = new DocumentRepository(deploymentPackagePM.Tenant).GetSingleDocument(deploymentPackagePM.Tenant, deploymentPackagePM.DocumentId);
            var deploymentPackageZipFileDetailsList = new DeploymentPackageDetailsService(deploymentPackagePM).Build();
            var deploymentPackageZipFileDetailsBytes = ZipFileService.Compress(deploymentPackageZipFileDetailsList);
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

    }

}
