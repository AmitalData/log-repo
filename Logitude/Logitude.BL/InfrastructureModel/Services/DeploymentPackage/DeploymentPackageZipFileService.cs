
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class DeploymentPackageZipFileService
    {
        private Document document;
        private DocumentRepository documentRepository;
        private int tenant;
        private Dictionary<string, DeploymentPackageDetails> deploymentPackageDetailsList;
        public DeploymentPackageZipFileService(int tenant)
        {
            deploymentPackageDetailsList = new Dictionary<string, DeploymentPackageDetails>();
            this.tenant = tenant;
            documentRepository = new DocumentRepository(tenant);
        }

        public string Create(DeploymentPackageDetails deploymentPackageDetails)//for name + desc + code (where to store them)
        {
            var deploymentPackageZipFileBytes = CompressionData(new Dictionary<string, byte[]>());
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

        private byte[] CompressionData(Dictionary<string, byte[]> dataList)
        {
            MemoryStream outputMemStream = new MemoryStream();
            ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);

            zipStream.SetLevel(3);
            byte[] bytes = null;
            foreach (string key in dataList.Keys)
            {
                var newEntry = new ZipEntry(key + ".json");
                newEntry.DateTime = DateTime.Now;

                zipStream.PutNextEntry(newEntry);

                bytes = dataList[key];

                MemoryStream inStream = new MemoryStream(bytes);
                long inStreamLength = inStream.Length;
                if (inStreamLength < 200)
                {
                    inStreamLength = 200;
                }

                StreamUtils.Copy(inStream, zipStream, new byte[inStreamLength]);
                inStream.Close();
                zipStream.CloseEntry();

            }

            zipStream.IsStreamOwner = false;
            zipStream.Close();
            outputMemStream.Position = 0;

            return outputMemStream.ToArray();

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
            var deploymentPackageZipFileDetailsList = new DeploymentPackageZipFileDetailsService().Build(deploymentPackagePM.DeploymentPackageDetails);
            var deploymentPackageZipFileDetailsBytes = CompressionData(deploymentPackageZipFileDetailsList);
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

}
