using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace Logitude.Server.Tools.StorageService
{
    public class AzureStorage
    {
        public readonly BlobContainerClient containerClient;
        private static Dictionary<string, AzureStorage> azureStorageCache = new Dictionary<string, AzureStorage>();

        public AzureStorage(string accountName, string key, string containerName)
        {
            containerClient = GetContainer(accountName, key, containerName);
        }

        public AzureStorage(string connectionString, string containerName)
        {
            containerClient = GetContainer(connectionString, containerName);
        }

        public static BlobContainerClient GetContainer(string connectionString, string containerName)
        {
            Azure.Storage.Blobs.BlobServiceClient blobServiceClient = new Azure.Storage.Blobs.BlobServiceClient(connectionString);
            BlobContainerClient containerClient = GetContainer(blobServiceClient, containerName);

            return containerClient;
        }

        public static BlobContainerClient GetContainer(string accountName, string key, string containerName)
        {
            Uri serviceUri = new Uri($"https://{accountName}.blob.core.windows.net/");
            StorageSharedKeyCredential credential = new StorageSharedKeyCredential(accountName, key);
            Azure.Storage.Blobs.BlobServiceClient blobServiceClient = new Azure.Storage.Blobs.BlobServiceClient(serviceUri, credential);
            BlobContainerClient containerClient = GetContainer(blobServiceClient, containerName);

            return containerClient;
        }

        private static BlobContainerClient GetContainer(Azure.Storage.Blobs.BlobServiceClient blobServiceClient, string containerName)
        {
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            TryCreateContainerIfNotExists(containerName, blobServiceClient, containerClient);

            return containerClient;
        }

        private static bool TryCreateContainerIfNotExists(string containerName, Azure.Storage.Blobs.BlobServiceClient blobServiceClient, BlobContainerClient containerClient)
        {
            try
            {
                if (!containerClient.Exists())
                    containerClient = blobServiceClient.CreateBlobContainer(containerName);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static AzureStorage GetFromCache(string accountName, string key, string containerName)
        {
            string cacheKey = accountName + ";" + key + ";" + containerName;
            if (!azureStorageCache.ContainsKey(cacheKey))
                azureStorageCache[cacheKey] = new AzureStorage(accountName, key, containerName);

            return azureStorageCache[cacheKey];
        }

        public static AzureStorage GetFromCache(string connectionString, string containerName)
        {
            string cacheKey = connectionString + ";" + containerName;
            if (!azureStorageCache.ContainsKey(cacheKey))
                azureStorageCache[cacheKey] = new AzureStorage(connectionString, containerName);

            return azureStorageCache[cacheKey];
        }

        public async Task UploadAsync(Stream stream, string fileName, bool overrite = true)
        {
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(stream, overrite);
        }

        public List<string> GetBlobNames() => containerClient.GetBlobs().OfType<CloudBlockBlob>().Select(b => b.Name).ToList();

        public async Task UploadAsync(string filePath, bool overrite = true)
        {
            BlobClient blobClient = containerClient.GetBlobClient(Path.GetFileNameWithoutExtension(filePath));
            await blobClient.UploadAsync(filePath, overrite);
        }

        public Pageable<BlobItem> GetBlobs() => containerClient.GetBlobs();

        public async Task DownloadToFileAsync(string fileName, string downloadPath)
        {
            BlobClient blob = containerClient.GetBlobClient(fileName);
            if (!blob.Exists())
                throw new Exception($"blob {fileName} not exist in container {containerClient.Name}");

            await blob.DownloadToAsync(downloadPath);
        }

        //public async Task<Response> DeleteAsync(string fileName)
        //{
        //    return await containerClient.DeleteBlobAsync(fileName);
        //}

        public Stream DownloadToStreamAsync(string fileName)
        {
            BlobClient blob = containerClient.GetBlobClient(fileName);
            try
            {
                Stream stream = blob.OpenRead();
                return stream;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<ZipArchiveEntry> DownloadAndExtractZipFile(string fileName)
        {
            Stream stream = this.DownloadToStreamAsync(fileName);
            ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Read);

            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                yield return entry;
            }
        }

        public Uri CreateSaSReadDelete() => CreateServiceSASContainer(BlobContainerSasPermissions.Read | BlobContainerSasPermissions.Delete);

        public Uri CreateSaSWrite() => CreateServiceSASContainer(BlobContainerSasPermissions.Write | BlobContainerSasPermissions.Create);

        public Uri CreateServiceSASContainer(BlobContainerSasPermissions permissions)
        {
            BlobSasBuilder sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = containerClient.Name,
                Resource = "c"
            };

            sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddDays(1);
            sasBuilder.SetPermissions(permissions);

            Uri sasURI = containerClient.GenerateSasUri(sasBuilder);

            return sasURI;
        }
    }
}
