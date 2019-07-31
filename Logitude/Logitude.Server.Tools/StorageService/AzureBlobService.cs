using Logitude.Server.Tools.Helpers;
using Microsoft.WindowsAzure.Storage.Blob;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.StorageService
{
    public class AzureBlobService : IBlobService
    {

         

        public byte[] Read(BlobFileInfo fileInfo)
        {
            BlobServiceReference.Response response = new BlobServiceReference.Response();

            //tenant1/docsout/1-379.pdf
            //string containername = filepath.Split('/')[0];
            //string localPath = filepath.Replace(containername + "/", "");

            byte[] result = null;
            string localPath = null;
            CloudBlobContainer blobContainer = null;
            GetFileBlobContainerInfo(fileInfo, out localPath, out blobContainer);

            var blobfile = blobContainer.GetBlockBlobReference(localPath);
            if (!blobfile.Exists())
            {
                if (!string.IsNullOrEmpty(LogitudeSettings.AzureFolderName) && IsEnableAzureRootFolder(fileInfo.Tenant))
                {
                    GetFileBlobContainerWithoutAzureFolder(fileInfo, out localPath, out blobContainer);
                    blobfile = blobContainer.GetBlockBlobReference(localPath);
                    if (!blobfile.Exists())
                    {
                        blobfile = blobContainer.GetBlockBlobReference(localPath.ToLower());
                    }
                }
                else
                    blobfile = blobContainer.GetBlockBlobReference(localPath.ToLower());
            }

            if (blobfile.Exists())
            {
                using (MemoryStream memstream = new MemoryStream())
                {
                    blobfile.DownloadToStream(memstream);
                    result = memstream.ToArray();
                }
            }
            //else
            // throw new Exception("File not found");


            if (fileInfo.FolderName != "logos" && !fileInfo.IsDecrypted)
            {
                DocumentRepository documentRepository = new DocumentRepository(fileInfo.Tenant);
                Document document = documentRepository.GetSingleDocument(fileInfo.Tenant, fileInfo.FileName);
                if (document != null && document.IsEncrypted)
                {
                    AesFunction aesFunction = new AesFunction();
                    result = aesFunction.DecryptData(result, fileInfo.Tenant);
                }
            }


            return result;
        }

        private  void GetFileBlobContainerWithoutAzureFolder(BlobFileInfo fileInfo, out string localPath, out CloudBlobContainer blobContainer)
        {
            if (fileInfo.HasExternalContainer)
            {
                string extension = !string.IsNullOrEmpty(fileInfo.Extension) ? fileInfo.Extension.ToLower() : "";
                localPath = fileInfo.FileName + "." + extension;
                blobContainer = StorageAcountDetails.GetCurrentContainer(fileInfo.ExternalContainerName);

            }
            else
            {
                string extension = !string.IsNullOrEmpty(fileInfo.Extension) ? fileInfo.Extension.ToLower() : "";
                localPath = StorageAcountDetails.GetBlobNameByLocation(fileInfo.FileName + "." + extension, fileInfo.FolderName);
                blobContainer = StorageAcountDetails.GetCurrentContainer(fileInfo.ContainerName);
            }
        }
        private  void GetFileBlobContainerInfo(BlobFileInfo fileInfo, out string localPath, out CloudBlobContainer blobContainer)
        {
            if (fileInfo.HasExternalContainer)
            {
                if (string.IsNullOrEmpty(fileInfo.ExternalContainerName))
                {
                    fileInfo.ExternalContainerName = "tenant" + fileInfo.Tenant;
                }
                string extension = !string.IsNullOrEmpty(fileInfo.Extension) ? fileInfo.Extension.ToLower() : "";

                localPath = fileInfo.FileName + "." + extension;
                if (!string.IsNullOrEmpty(LogitudeSettings.AzureFolderName) &&  IsEnableAzureRootFolder(fileInfo.Tenant))
                {
                    localPath = fileInfo.ExternalContainerName + "/" + localPath;
                    blobContainer = StorageAcountDetails.GetCurrentContainer(LogitudeSettings.AzureFolderName.ToLower());
                }
                else
                {
                    blobContainer = StorageAcountDetails.GetCurrentContainer(fileInfo.ExternalContainerName);
                }
            }
            else
            {
                string extension =!string.IsNullOrEmpty(fileInfo.Extension) ? fileInfo.Extension.ToLower() : "";

                localPath = StorageAcountDetails.GetBlobNameByLocation(fileInfo.FileName + "." + extension, fileInfo.FolderName);
                if (!string.IsNullOrEmpty(LogitudeSettings.AzureFolderName) && IsEnableAzureRootFolder(fileInfo.Tenant))
                {
                    localPath = fileInfo.ContainerName + "/" + localPath;
                    blobContainer = StorageAcountDetails.GetCurrentContainer(LogitudeSettings.AzureFolderName.ToLower());
                }
                else
                {
                    blobContainer = StorageAcountDetails.GetCurrentContainer(fileInfo.ContainerName);
                }
            }
        }

        public void Write(byte[] data, BlobFileInfo fileInfo)
        {
            string localPath = null;
            CloudBlobContainer blobContainer = null;
            GetFileBlobContainerInfo(fileInfo, out localPath, out blobContainer);


            var blobfile = blobContainer.GetBlockBlobReference(localPath);
            using (Stream blobstream = blobfile.OpenWrite())
            {
                if (fileInfo.FolderName != "logos")
                {
                    DocumentRepository documentRepository = new DocumentRepository(fileInfo.Tenant);
                    Document document = documentRepository.GetSingleDocument(fileInfo.Tenant, fileInfo.FileName);
                    if ((document != null && document.IsEncrypted) || fileInfo.IsEncrypted)
                    {
                        AesFunction aesFunction = new AesFunction();
                        data = aesFunction.EncryptData(data, fileInfo.Tenant, fileInfo.AesKey);
                    }

                }

                blobstream.Write(data, 0, (int)data.Length);

            }

        }


        public void WriteBlock(byte[] buffer, long sentBytes, string[] blockIdsList, int bufferNumber, BlobFileInfo fileInfo)
        {
            string localPath = null;
            CloudBlobContainer blobContainer = null;
            GetFileBlobContainerInfo(fileInfo, out localPath, out blobContainer);

            //temp file ( to be deleted when upload done)
            var tempcloudBlockBlob = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(fileInfo.FileName, ""));

        

            if (sentBytes < fileInfo.FileSize)
            {
                MemoryStream memorystream = new MemoryStream(buffer);
                tempcloudBlockBlob.PutBlock(blockIdsList[bufferNumber], memorystream, null);
            }
            else
            {
                var finalcloudBlockBlob = blobContainer.GetBlockBlobReference(localPath);

                MemoryStream memorystream = new MemoryStream(buffer);
                tempcloudBlockBlob.PutBlock(blockIdsList[bufferNumber], memorystream, null);

                int numberOfBlocks = blockIdsList.Length;
                String[] blockIds = new String[numberOfBlocks];
                for (int i = 0; i < numberOfBlocks; i++)
                {
                    blockIds[i] = blockIdsList[i];
                }

                tempcloudBlockBlob.PutBlockList(blockIds);

                using (MemoryStream memstream = new MemoryStream())
                {
                    tempcloudBlockBlob.DownloadToStream(memstream);
                    var result = memstream.ToArray();

                    using (Stream blobstream = finalcloudBlockBlob.OpenWrite())
                    {

                        if (fileInfo.FolderName != "logos")
                        {
                            DocumentRepository documentRepository = new DocumentRepository(fileInfo.Tenant);
                            Document document = documentRepository.GetSingleDocument(fileInfo.Tenant, fileInfo.FileName);
                            if ((document != null && document.IsEncrypted) || fileInfo.IsEncrypted)
                            {
                                AesFunction aesFunction = new AesFunction();
                                result = aesFunction.EncryptData(result, fileInfo.Tenant);
                            }
                        }

                        blobstream.Write(result, 0, (int)result.Length);

                    }


                }

               // finalcloudBlockBlob.StartCopy(tempcloudBlockBlob);
                tempcloudBlockBlob.DeleteIfExists();
            }
        }


        public void Delete(BlobFileInfo fileInfo)
        {
            //tenant1/docsout/1-379.pdf
            //string localPath = StorageAcountDetails.GetBlobNameByLocation(fileInfo.FileName + "." + fileInfo.Extension.ToLower(), fileInfo.FolderName);
            //CloudBlobContainer blobContainer = StorageAcountDetails.GetCurrentContainer(fileInfo.ContainerName);

            string localPath = null;
            CloudBlobContainer blobContainer = null;
            GetFileBlobContainerInfo(fileInfo, out localPath, out blobContainer);

            var blobfile = blobContainer.GetBlockBlobReference(localPath);
            if (!blobfile.Exists())
            {
                if (!string.IsNullOrEmpty(LogitudeSettings.AzureFolderName) && IsEnableAzureRootFolder(fileInfo.Tenant))
                {
                    GetFileBlobContainerWithoutAzureFolder(fileInfo, out localPath, out blobContainer);
                    blobfile = blobContainer.GetBlockBlobReference(localPath);
                }
            }
            if (blobfile.Exists())
            {
                blobfile.Delete();
            }


        }

        public bool FileExists(BlobFileInfo fileInfo)
        {
            string localPath = null;
            CloudBlobContainer blobContainer = null;
            GetFileBlobContainerInfo(fileInfo, out localPath, out blobContainer);

            var blobfile = blobContainer.GetBlockBlobReference(localPath);
            if (!blobfile.Exists())
            {
                if (!string.IsNullOrEmpty(LogitudeSettings.AzureFolderName) && IsEnableAzureRootFolder(fileInfo.Tenant))
                {
                    GetFileBlobContainerWithoutAzureFolder(fileInfo, out localPath, out blobContainer);
                    blobfile = blobContainer.GetBlockBlobReference(localPath);
                }
            }


            return blobfile.Exists();
        }

        private bool IsEnableAzureRootFolder(int tenant)
        {
            bool result = false;

            result = FeatureToggleHelper.HasFeatureToggle("EZR", tenant);

            return result;
        }



    }
}

