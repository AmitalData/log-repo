using Dropbox.Api;
using Dropbox.Api.Files;
using Dropbox.Api.Users;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Helpers
{
    public class DropBoxActionsHelper
    {
        string accessToken = "";
        DropboxClient client = null;
        public DropBoxActionsHelper(int tenant)
        {
            TenantAdditionalDataRepository Repo = new TenantAdditionalDataRepository(tenant);
            var currentTenant = Repo.GetSingleTenantAdditionalData(tenant);
            accessToken = currentTenant.DropBoxAccessToken;

            var httpClient = new HttpClient(new WebRequestHandler { ReadWriteTimeout = 10 * 1000 })
            {
                // Specify request level timeout which decides maximum time that can be spent on
                // download/upload files.
                Timeout = TimeSpan.FromMinutes(20)
            };

            try
            {
                var config = new DropboxClientConfig("LogitudeDropBoxApp")
                {
                    HttpClient = httpClient
                };

                client = new DropboxClient(accessToken, config);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "DropBoxActionsHelper", "", null);
            }
        }

        /// <summary>
        /// Creates the specified folder.
        /// </summary>
        /// <remarks>This demonstrates calling an rpc style api in the Files namespace.</remarks>
        /// <param name="path">The path of the folder to create.</param>
        /// <param name="client">The Dropbox client.</param>
        /// <returns>The result from the ListFolderAsync call.</returns>
        public async Task<FolderMetadata> CreateFolder(string path)
        {

            var folderArg = new CreateFolderArg(path);
            var folder = await client.Files.CreateFolderAsync(folderArg);

            return folder;
        }

        /// <summary>
        /// Downloads a file.
        /// </summary>
        /// <remarks>This demonstrates calling a download style api in the Files namespace.</remarks>
        /// <param name="client">The Dropbox client.</param>
        /// <param name="folder">The folder path in which the file should be found.</param>
        /// <param name="file">The file to download within <paramref name="folder"/>.</param>
        /// <returns></returns>
        public async Task<byte[]> Download(string folder, FileMetadata file)
        {
            using (var response = await client.Files.DownloadAsync(folder + "/" + file.Name))
            {
                var Data = await response.GetContentAsByteArrayAsync();
                return Data;
            }
        }

        /// <summary>
        /// Uploads given content to a file in Dropbox.
        /// </summary>
        /// <param name="client">The Dropbox client.</param>
        /// <param name="folder">The folder to upload the file.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="fileContent">The file content.</param>
        /// <returns></returns>
        public async Task Upload(string folder, string fileName, byte[] fileContent)
        {
            using (var stream = new MemoryStream(fileContent))
            {
                var response = await client.Files.UploadAsync(folder + "/" + fileName, WriteMode.Overwrite.Instance, body: stream);

                Console.WriteLine("Uploaded Id {0} Rev {1}", response.Id, response.Rev);
            }
        }

        public async Task<FullAccount> GetCurrentAccount()
        {
            var full = await client.Users.GetCurrentAccountAsync();
            return full;
        }
        

        /// <summary>
        /// Uploads a big file in chunk. The is very helpful for uploading large file in slow network condition
        /// and also enable capability to track upload progerss.
        /// </summary>
        /// <param name="client">The Dropbox client.</param>
        /// <param name="folder">The folder to upload the file.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <returns></returns>
        public async Task ChunkUpload(string folder, string fileName, byte[] Content)
        {
            Console.WriteLine("Chunk upload file...");
            // Chunk size is 128KB.
            const int chunkSize = 128 * 1024;

            // Create a random file of 1MB in size.
            var fileContent = Content;//new byte[1024 * 1024];
            new Random().NextBytes(fileContent);

            using (var stream = new MemoryStream(fileContent))
            {
                int numChunks = (int)Math.Ceiling((double)stream.Length / chunkSize);
              
                    byte[] buffer = new byte[chunkSize];
                    string sessionId = null;

                    for (var idx = 0; idx < numChunks; idx++)
                    {
                        Console.WriteLine("Start uploading chunk {0}", idx);
                        var byteRead = stream.Read(buffer, 0, chunkSize);

                        using (MemoryStream memStream = new MemoryStream(buffer, 0, byteRead))
                        {
                            if (idx == 0)
                            {
                                var result = await client.Files.UploadSessionStartAsync(body: memStream);
                                sessionId = result.SessionId;
                                UploadSessionCursor cursor = new UploadSessionCursor(sessionId, (ulong)(chunkSize * idx));

                                if (idx == numChunks - 1)
                                {
                                    await client.Files.UploadSessionFinishAsync(cursor, new CommitInfo(folder + "/" + fileName),null, memStream);
                                }
                            }

                            else
                            {
                                UploadSessionCursor cursor = new UploadSessionCursor(sessionId, (ulong)(chunkSize * idx));

                                if (idx == numChunks - 1)
                                {
                                    await client.Files.UploadSessionFinishAsync(cursor, new CommitInfo(folder + "/" + fileName),null, memStream);
                                }

                                else
                                {
                                    await client.Files.UploadSessionAppendV2Async(cursor, body: memStream);
                                }
                            }
                        }
                    }
                 
            }
        }

        /// <summary>
        /// Lists the items within a folder.
        /// </summary>
        /// <remarks>This demonstrates calling an rpc style api in the Files namespace.</remarks>
        /// <param name="path">The path to list.</param>
        /// <param name="client">The Dropbox client.</param>
        /// <returns>The result from the ListFolderAsync call.</returns>
        public async Task<ListFolderResult> ListFolder(string path)
        {
            try
            {
                var list = await client.Files.ListFolderAsync(path);
                return list;
            }
            catch (Exception)
            { 
                return null;
            }
           
        }

        public async Task<Metadata> DeleteFile(string path)
        {

            var DeleteArg = new DeleteArg(path);
            var folder = await client.Files.DeleteAsync(DeleteArg);

            return folder;
        }


        public CommunicationLog CreateDropBoxCommunicationLog(int tenant,string ObjectTableId,byte[] ByteData,string FileName,string FolderName, string subject = "Contpaq Interface", string EntityId= null, string to = "DropBox",string entityRefrence = null)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
            DocumentRepository documentrepository = new DocumentRepository(commonContext);
            TenantRepository tenantrepository = new TenantRepository(commonContext);
            var curtenant = tenantrepository.GetSingleTenant(tenant);
            var ext = FileName.Split('.').Length > 1 ? FileName.Split('.')[1] : "xml";
            var settings = new CommunicationLogSettings() { FileName = FileName,FolderName = FolderName };
            var settingsData = JsonConvert.SerializeObject(settings);
            //settingsData = "<?xml version=\"1.0\" encoding=\"UTF - 16\" ?>" + settingsData;
            Document document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = ext,
                FileSize = ByteData.Length,
                Tenant = Convert.ToInt32(tenant),
                Id = IdCounter.GetNumber("Document", tenant),
                HasFile = true,
                Folder = "DropBox",
            };
            documentrepository.Add(document);
            documentrepository.SubmitChanges();

            var communicationLogId = IdCounter.GetNumber("CommunicationLog", tenant);
            ObjectTableRepository objecttableRep = new ObjectTableRepository(tenant);
            //ObjectTable objectTable = null;

            //objectTable = objecttableRep.GetObjectTableByName(ObjectTableName, 0, true);
            CommunicationLog commLog = new CommunicationLog()
            {
                Id = communicationLogId,
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                To = to,
                InOut = "O",
                ObjectTableId = ObjectTableId,
                Subject = subject,
                Tenant = tenant,
                CommunicationLogTypeCode = "T",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                CommunicationStatusTypeCode = "W",
                DocumentId = document.Id,
                LastStatusDateUTC = DateTime.UtcNow,
                CreateDateUTC = DateTime.UtcNow,
                From = curtenant.Company,
                QueueName = "DropBoxCommunicationLogQueue",
                LogSettings = settingsData,
                EntityId = EntityId,
                EntityReference = entityRefrence,
            };
            communicationLogRepository.Add(commLog);
            communicationLogRepository.SubmitChanges();
            communicationLogId = commLog.Id;
            string filename = document.Id + "." + document.Extension;
            string filePath = "tenant" + commLog.Tenant + "/" + StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder);
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = tenant,
                FileSize = ByteData.Length,

            };

            storageservice.Write(ByteData, fileInfo);

            if (!string.IsNullOrEmpty(commLog.QueueName))
            {
                try
                {
                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, null, commLog.CommunicationStatusTypeCode, "Before adding message to queue Forwarder Shipment " + DateTime.Now.ToString(), null);
                    SendCommunicationLogMessageToQueue(commLog.QueueName, commLog.Id, tenant);
                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, null, commLog.CommunicationStatusTypeCode, "after adding message to queue  Forwarder Shipment " + DateTime.Now.ToString(), null);

                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;

                    if (!string.IsNullOrEmpty(ex.StackTrace))
                    {
                        errorMessage += Environment.NewLine + ex.StackTrace;
                    }

                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, null, commLog.CommunicationStatusTypeCode, "Exception occured while adding message to queue Forwarder Shipment " + DateTime.Now.ToString(), errorMessage);
                   

                }
            }
            return commLog;
        }

        private void SendCommunicationLogMessageToQueue(string queueName, string communicationLogId, int tenant)
        {
            try
            {
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue(queueName, 0);
                queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", communicationLogId }, { "Tenant", tenant.ToString() } }, tenant);
               
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "SendCommunicationLogMessageToQueue Forwarder Shipment", null, null);
            }
        }
    }

    public class CommunicationLogSettings
    {
        public string FileName { get; set; }
        public string FolderName { get; set; }
    }
}
