using AmitalCloud.Infrastructure.Data.Azure;
using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Domain.DataContracts;
using AmitalCloud.Infrastructure.Model.EntityClasses ;
using AmitalCloud.Infrastructure.Domain.Helpers;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AmitalCloud.Infrastructure.Model.Interfaces;

namespace AmitalCloud.Infrastructure.Data.Services
{
    public class AzureBlobService : IBlobService, IDisposable
    {
        public byte[] Read(BlobFileInfo fileInfo)
        {
            //tenant1/docsout/1-379.pdf
            //string containername = filepath.Split('/')[0];
            //string localPath = filepath.Replace(containername + "/", "");
            byte[] result = null;
            if (fileInfo.FolderName != "SchedularLogs")
            {
                CloudBlob blobfile = GetCloudBlockBlob(fileInfo);

                result = DownloadCloudBlob(blobfile);
                if (fileInfo.FolderName != "logos" && !fileInfo.IsDecrypted)
                {
                    Document document = GetDocument(fileInfo, AmitalCloudContext.GetContext(fileInfo.Tenant));
                    if (document != null && document.IsEncrypted)
                    {
                        AesFunction aesFunction = new AesFunction();
                        result = aesFunction.DecryptData(result, fileInfo.Tenant);
                    }
                }
            }
            else
            {
                CloudBlob blobfile = GetCloudAppendBlob(fileInfo);
                result = DownloadCloudBlob(blobfile);
            }

            return result;
        }

        private byte[] DownloadCloudBlob(CloudBlob blobfile)
        {
            byte[] result = null;
            if (blobfile.Exists())
            {
                using (MemoryStream memstream = new MemoryStream())
                {
                    blobfile.DownloadToStream(memstream);
                    result = memstream.ToArray();
                }
            }
            return result;
        }
        private CloudBlob GetCloudBlockBlob(BlobFileInfo fileInfo)
        {
            string localPath = null;
            CloudBlobContainer blobContainer = null;
            GetFileBlobContainerInfo(fileInfo, out localPath, out blobContainer);
            CloudBlob blobfile = blobContainer.GetBlockBlobReference(localPath);
            if (!blobfile.Exists())
            {
                if (!string.IsNullOrEmpty(AmitalCloudSettings.AzureFolderName) && IsEnableAzureRootFolder(fileInfo.Tenant))
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
            return blobfile;
        }

        private CloudBlob GetCloudAppendBlob(BlobFileInfo fileInfo)
        {
            string localPath = null;
            CloudBlobContainer blobContainer = null;
            GetFileBlobContainerInfo(fileInfo, out localPath, out blobContainer);
            CloudBlob blobfile = blobContainer.GetAppendBlobReference(localPath);

            if (!blobfile.Exists())
            {
                if (!string.IsNullOrEmpty(AmitalCloudSettings.AzureFolderName) && IsEnableAzureRootFolder(fileInfo.Tenant))
                {
                    GetFileBlobContainerWithoutAzureFolder(fileInfo, out localPath, out blobContainer);
                    blobfile = blobContainer.GetAppendBlobReference(localPath);
                    if (!blobfile.Exists())
                    {
                        blobfile = blobContainer.GetAppendBlobReference(localPath.ToLower());
                    }
                }
                else
                    blobfile = blobContainer.GetAppendBlobReference(localPath.ToLower());
            }

            return blobfile;
        }


        private void GetFileBlobContainerWithoutAzureFolder(BlobFileInfo fileInfo, out string localPath, out CloudBlobContainer blobContainer)
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

        private void GetFileBlobContainerInfo(BlobFileInfo fileInfo, out string localPath, out CloudBlobContainer blobContainer)
        {
            if (fileInfo.HasExternalContainer)
            {
                if (string.IsNullOrEmpty(fileInfo.ExternalContainerName))
                {
                    fileInfo.ExternalContainerName = "tenant" + fileInfo.Tenant;
                }
                string extension = !string.IsNullOrEmpty(fileInfo.Extension) ? fileInfo.Extension.ToLower() : "";

                localPath = fileInfo.FileName + "." + extension;
                if (!string.IsNullOrEmpty(AmitalCloudSettings.AzureFolderName) && IsEnableAzureRootFolder(fileInfo.Tenant))
                {
                    localPath = fileInfo.ExternalContainerName + "/" + localPath;
                    blobContainer = StorageAcountDetails.GetCurrentContainer(AmitalCloudSettings.AzureFolderName.ToLower());
                }
                else
                {
                    blobContainer = StorageAcountDetails.GetCurrentContainer(fileInfo.ExternalContainerName);
                }
            }
            else
            {
                string extension = !string.IsNullOrEmpty(fileInfo.Extension) ? fileInfo.Extension.ToLower() : "";

                localPath = StorageAcountDetails.GetBlobNameByLocation(fileInfo.FileName + "." + extension, fileInfo.FolderName);
                if (!string.IsNullOrEmpty(AmitalCloudSettings.AzureFolderName) && IsEnableAzureRootFolder(fileInfo.Tenant))
                {
                    localPath = fileInfo.ContainerName + "/" + localPath;
                    blobContainer = StorageAcountDetails.GetCurrentContainer(AmitalCloudSettings.AzureFolderName.ToLower());
                }
                else
                {
                    if (fileInfo.FolderName == "how-to")
                    {
                        blobContainer = StorageAcountDetails.GetCurrentContainer(fileInfo.FolderName);
                    }

                    else
                    {
                        blobContainer = StorageAcountDetails.GetCurrentContainer(fileInfo.ContainerName);
                    }
                }
            }
        }

        public void MoveFromAnotherStorage(string containerSASURI, string fileNameSource, BlobFileInfo destinationFileInfo)
        {
            string cacheKey = $"BlobContainer_({containerSASURI.Substring(containerSASURI.Length - 30, 29)})";

            CloudBlobContainer blobContainerSource = CacheManager.GetOrInsertNewObject(cacheKey, () =>
                new CloudBlobContainer(new Uri(containerSASURI)));
            CloudBlockBlob blobSource = blobContainerSource.GetBlockBlobReference(fileNameSource);
            GetFileBlobContainerInfo(destinationFileInfo, out string localPath, out CloudBlobContainer blobContainerDest);
            CloudBlockBlob blobDestination = blobContainerDest.GetBlockBlobReference(localPath);

            blobDestination.StartCopy(blobSource);

            ICloudBlob destBlobRef = blobContainerDest.GetBlobReferenceFromServer(blobDestination.Name);
            while (destBlobRef.CopyState.Status == CopyStatus.Pending)
            {
                Task.Delay(50).Wait();
                destBlobRef = blobContainerDest.GetBlobReferenceFromServer(destBlobRef.Name);
            }

            blobSource.Delete();
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
                    IAmitalCloudContext context = AmitalCloudContext.GetContext(fileInfo.Tenant);
                    Document document = GetDocument(fileInfo, context);
                    if ((document != null && document.IsEncrypted) || fileInfo.IsEncrypted)
                    {
                        DocumentsFiling documentsFiling = GetDocumentInfo(fileInfo, context);
                        if (documentsFiling != null)
                        {
                            KeyValuePair<string, string> metadata = new KeyValuePair<string, string>("Code", documentsFiling.Code);
                            blobfile.Metadata.Add(metadata);
                        }
                        AesFunction aesFunction = new AesFunction();
                        data = aesFunction.EncryptData(data, fileInfo.Tenant, fileInfo.AesKey);
                    }
                }
                blobstream.Write(data, 0, (int)data.Length);
            }
        }

        private DocumentsFiling GetDocumentInfo(BlobFileInfo fileInfo, IAmitalCloudContext context)
        {
            return new Repository<DocumentsFiling>(context).GetMulti(a => a.Document.FileName == fileInfo.FileName && a.Tenant == fileInfo.Tenant).FirstOrDefault();
            //.GetSingleDocumentsFilingByDocumentId(fileInfo.FileName, fileInfo.Tenant);
        }

        private Document GetDocument(BlobFileInfo fileInfo, IAmitalCloudContext context)
        {
            return new Repository<Document>(context).GetMulti(a => a.FileName == fileInfo.FileName && a.Tenant == fileInfo.Tenant).FirstOrDefault();
            //.GetSingleDocument(fileInfo.Tenant, fileInfo.FileName);
        }

        public void WriteBlock(byte[] buffer, long sentBytes, string[] blockIdsList, int bufferNumber, BlobFileInfo fileInfo)
        {
            string localPath = null;
            CloudBlobContainer blobContainer = null;
            GetFileBlobContainerInfo(fileInfo, out localPath, out blobContainer);
            var tempcloudBlockBlob = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(fileInfo.FileName, ""));
            if (sentBytes < fileInfo.FileSize)
            {
                using (MemoryStream memorystream = new MemoryStream(buffer))
                {
                    tempcloudBlockBlob.PutBlock(blockIdsList[bufferNumber], memorystream, null);
                }
            }
            else
            {
                var finalcloudBlockBlob = blobContainer.GetBlockBlobReference(localPath);
                using (MemoryStream memorystream = new MemoryStream(buffer))
                {
                    tempcloudBlockBlob.PutBlock(blockIdsList[bufferNumber], memorystream, null);
                }
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
                        bool skippfileTest = false;
                        if (fileInfo.FolderName != "logos")
                        {
                            IAmitalCloudContext context = AmitalCloudContext.GetContext(fileInfo.Tenant);
                            Document document = GetDocument(fileInfo, context);
                            if ((document != null && document.IsEncrypted) || fileInfo.IsEncrypted)
                            {
                                DocumentsFiling documentsFiling = GetDocumentInfo(fileInfo, context);

                                if (documentsFiling != null)
                                {
                                    KeyValuePair<string, string> metadata = new KeyValuePair<string, string>("Code", documentsFiling.Code);

                                    finalcloudBlockBlob.Metadata.Add(metadata);
                                }

                                AesFunction aesFunction = new AesFunction();
                                result = aesFunction.EncryptData(result, fileInfo.Tenant);

                                skippfileTest = (document != null && document.FileName == "test update failed");
                            }
                        }

                        if (!skippfileTest)
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
                if (!string.IsNullOrEmpty(AmitalCloudSettings.AzureFolderName) && IsEnableAzureRootFolder(fileInfo.Tenant))
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
                if (!string.IsNullOrEmpty(AmitalCloudSettings.AzureFolderName) && IsEnableAzureRootFolder(fileInfo.Tenant))
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

        public void AppendText(string text, BlobFileInfo fileInfo)
        {
            string localPath = null;
            CloudBlobContainer blobContainer = null;
            GetFileBlobContainerInfo(fileInfo, out localPath, out blobContainer);

            var blobfile = blobContainer.GetAppendBlobReference(localPath);
            if (!blobfile.Exists())
            {
                blobfile.CreateOrReplace();
            }

            blobfile.AppendText(text);



            //



        }
        public void Dispose()
        {
        }
    }
}

