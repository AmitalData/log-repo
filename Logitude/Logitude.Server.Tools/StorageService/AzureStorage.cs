using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.StorageService
{
    public class AzureStorage
    {
        private readonly BlobContainerClient containerClient;

        public AzureStorage(string connectionString, string containerName)
        {
            containerClient = GetContainer(connectionString, containerName);
        }

        public static BlobContainerClient GetContainer(string connectionString, string containerName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            if (!containerClient.Exists())
                containerClient = blobServiceClient.CreateBlobContainer(containerName);

            return containerClient;
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
            catch(System.Exception ex)
            {
                return null;
            }

            
        }


        public string DownloadJsonFileFromZip1(string fileName)
        {
            Stream stream = this.DownloadToStreamAsync(fileName);

            using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Read))
            {
                // Assuming there's only one JSON file in the ZIP archive, you can retrieve it as follows:
                ZipArchiveEntry jsonEntry = archive.Entries.FirstOrDefault(e => Path.GetExtension(e.Name) == ".json");
                if (jsonEntry != null)
                {
                    using (Stream jsonStream = jsonEntry.Open())
                    using (StreamReader reader = new StreamReader(jsonStream))
                    {
                        string jsonContent = reader.ReadToEnd();

                        // Process the JSON content as needed
                        return jsonContent;
                    }
                }
                else
                {
                    return null;
                }
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


        //public async Task<IEnumerable<string>> GetAllBlobNamesAsync()
        //{
        //    var blobNames = new List<string>();

        //    await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
        //        blobNames.Add(blobItem.Name);

        //    return blobNames;
        //}
    }
}
