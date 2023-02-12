using System;
using System.IO;
using System.Net;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Simplog.Server.Infrastructure;

namespace Logitude.Workflow.Data.WorkflowStorage
{
    public class WorkflowAzureStorage
    {
        private readonly CloudBlobContainer CloudBlobContainer;

        public WorkflowAzureStorage(string containerName)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            CloudBlobContainer = GetCloudBlobContainer(containerName);
        }
        
        public byte[] GetBlobBytes(string blobName)
        {
            try
            {
                if (!string.IsNullOrEmpty(blobName))
                {
                    CloudBlockBlob cloudBlockBlob = GetCloudBlockBlob(blobName);
                    return GetBlobBytes(cloudBlockBlob);
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private CloudBlobContainer GetCloudBlobContainer(string containerName)
        {
            try
            {
                if (!string.IsNullOrEmpty(containerName))
                {
                    string storageConnectionString = GetStorageConnectionString();
                    CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(storageConnectionString);
                    CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                    CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName.ToLower());
                    return cloudBlobContainer != null && cloudBlobContainer.Exists() ? cloudBlobContainer : null;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private CloudBlockBlob GetCloudBlockBlob(string blobName)
        {
            try
            {
                if (CloudBlobContainer != null && CloudBlobContainer.Exists() && !string.IsNullOrEmpty(blobName))
                {
                    CloudBlockBlob cloudBlockBlob = CloudBlobContainer.GetBlockBlobReference(blobName);
                    return cloudBlockBlob != null && cloudBlockBlob.Exists() ? cloudBlockBlob : null;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private byte[] GetBlobBytes(CloudBlockBlob cloudBlockBlob)
        {
            try
            {
                if (cloudBlockBlob != null && cloudBlockBlob.Exists())
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        cloudBlockBlob.DownloadToStream(memoryStream);
                        return memoryStream.ToArray();
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string GetStorageConnectionString()
        {
            string accountProtocol = "https";
            string accountName = LogitudeSettings.WorkflowStorageAccountName;
            string accountKey = LogitudeSettings.WorkflowStorageAccountKey;
            return string.Format("DefaultEndpointsProtocol={0};AccountName={1};AccountKey={2}", accountProtocol, accountName, accountKey);
        }
    }
}