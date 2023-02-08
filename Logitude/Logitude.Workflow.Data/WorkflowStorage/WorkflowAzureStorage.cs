using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Net;

namespace Logitude.Workflow.Data.WorkflowStorage
{
    public class WorkflowAzureStorage
    {
        private readonly string ContainerName;

        public WorkflowAzureStorage(string containerName)
        {
            ContainerName = string.IsNullOrEmpty(containerName) ? null : containerName.ToLower();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
        
        public byte[] GetBlobBytes(string blobName)
        {
            try
            {
                if (!string.IsNullOrEmpty(blobName))
                {
                    CloudBlobContainer cloudBlobContainer = GetCloudBlobContainer();
                    CloudBlockBlob cloudBlockBlob = GetCloudBlockBlob(cloudBlobContainer, blobName);
                    byte[] bytes = GetBlobBytes(cloudBlockBlob);
                    return bytes;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private CloudBlobContainer GetCloudBlobContainer()
        {
            if (!string.IsNullOrEmpty(ContainerName))
            {
                string storageConnectionString = GetStorageConnectionString();
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(storageConnectionString);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(ContainerName);
                return cloudBlobContainer != null && cloudBlobContainer.Exists() ? cloudBlobContainer : null;
            }
            return null;
        }

        private CloudBlockBlob GetCloudBlockBlob(CloudBlobContainer cloudBlobContainer, string blobName)
        {
            if (cloudBlobContainer != null && cloudBlobContainer.Exists() && !string.IsNullOrEmpty(blobName))
            {
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(blobName);
                return cloudBlockBlob != null && cloudBlockBlob.Exists() ? cloudBlockBlob : null;
            }
            return null;
        }

        private byte[] GetBlobBytes(CloudBlockBlob cloudBlockBlob)
        {
            if (cloudBlockBlob != null && cloudBlockBlob.Exists())
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    cloudBlockBlob.DownloadToStream(memoryStream);
                    byte[] bytes = memoryStream.ToArray();
                    return bytes;
                }
            }
            return null;
        }

        private string GetStorageConnectionString()
        {
            return string.Format("DefaultEndpointsProtocol={0};AccountName={1};AccountKey={2}",
                WorkflowStorageAccount.DefaultEndpointsProtocol, WorkflowStorageAccount.AccountName, WorkflowStorageAccount.AccountKey);
        }
    }
}