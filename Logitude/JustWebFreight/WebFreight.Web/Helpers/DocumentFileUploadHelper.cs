using Logitude.BL.CommonDataModel.APIDataContract;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.LogitudeCacheManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Security;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.Helpers
{
    public class DocumentFileUploadHelper
    {

        string fileNameAndExtension;
        //private string fileName;
        string documentIdAndExtension;
        private string _CallFrom = "";

        public DocumentFileUploadHelper()
            : this("")
        {

        }
        public DocumentFileUploadHelper(string callFrom)
        {
            // TODO: Complete member initialization
            this._CallFrom = callFrom;
        }

        public static string GetTempStorageSasWrite(int tenant) =>
            AzureStorage.GetFromCache(LogitudeSettings.TempStorageConnection, GetTempStorageContainerName(tenant))
            .CreateSaSWrite().ToString();

        private static string GetTempStorageContainerName(int tenant) => "hybrid-upload-tenant" + tenant.ToString();

        public static Response AddDocumentAndSendToInternalStorage(int tenant, string blobname, string DocumentId)
        {
            Response response = new Response();

            try
            {                
                if (string.IsNullOrEmpty(blobname))
                    throw new ArgumentException("Invalid file name!");

                string[] mfileParams = blobname.Split('.');
                string finalFileName = blobname.Substring(0, blobname.LastIndexOf('.'));
                string fileextension = mfileParams[mfileParams.Length - 1];

                if (string.IsNullOrEmpty(fileextension))
                    throw new ArgumentException("Invalid file extension!");

                if (string.IsNullOrEmpty(finalFileName))
                    throw new ArgumentException("Invalid file name!");

                string fileName = new DocumentFileUploadHelper().BuidDocument(tenant, finalFileName, fileextension, 0, DocumentId, true);
                string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileName.ToLower(), "docsin");
                string[] fileParams = fileName.Split('.');
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = fileParams[0],
                    FolderName = "docsin",
                    Extension = fileParams[1],
                    Tenant = tenant,
                    FileSize = 0,
                };
                
                string containerSASURI = AzureStorage.GetFromCache(LogitudeSettings.TempStorageConnection, GetTempStorageContainerName(tenant)).CreateSaSReadDelete().ToString();
                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                storageservice.MoveFromAnotherStorage(containerSASURI, blobname, fileInfo);

                response.Result = fileName.Split('.')[0].ToString();
            }
            catch (Exception e)
            {
                response.ErrorMessage = "Error while copying file to internal storage!, Error: " + e.Message;
            }

            response.HasError = string.IsNullOrEmpty(response.ErrorMessage);

            return response;
        }

        public Response UploadDocumentFileData(byte[] buffer, long fileSize, long sentBytes, string[] blockIdsList, int bufferNumber, int tenant, string FileNameWithExtention, string DocumentId, DocumentsFilingPM documentsFilingPM = null)
        {
            Response response = new Response();

            try
            {
                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                IWebFreightContext webFreightContext = WebFreightContext.GetContext(tenant);

                if (string.IsNullOrEmpty(FileNameWithExtention))
                {
                    response.HasError = true;
                    response.ErrorMessage = "Invalid file name!";

                    return response;
                }

                string[] mfileParams = FileNameWithExtention.Split('.');
                string finalFileName = FileNameWithExtention.Substring(0, FileNameWithExtention.LastIndexOf('.'));
                string fileextension = mfileParams[mfileParams.Length - 1];

                if (string.IsNullOrEmpty(fileextension))
                {
                    response.HasError = true;
                    response.ErrorMessage = "Invalid file extension!";
                }

                if (string.IsNullOrEmpty(finalFileName))
                {
                    response.HasError = true;
                    response.ErrorMessage = "Invalid file name!";
                }

                if (response.HasError)
                {
                    return response;
                }

                if (sentBytes < fileSize)
                {
                    var fileName = BuidDocument(tenant, finalFileName, fileextension, fileSize, DocumentId, false);
                    string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileName.ToLower(), "docsin");
                    string[] fileParams = fileName.Split('.');
                    BlobFileInfo fileInfo = new BlobFileInfo()
                    {
                        FileName = fileParams[0],
                        FolderName = "docsin",
                        Extension = fileParams[fileParams.Length - 1],
                        Tenant = tenant,
                        FileSize = fileSize,

                    };


                    if (this._CallFrom.Equals("DocumentInWcfService", StringComparison.OrdinalIgnoreCase))
                    {
                        //string documentId = ""; DocumentsFiling poco = null;
                        //if (fileInfo.IsUnifreightFillingMode(out documentId, out poco))
                        if (fileInfo.IsUnifreightFillingMode())
                        {
                            throw new Exception("Unifreight Filling Mode is on! Send Meta-data only ");
                        }

                    }
                    IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                    storageservice.WriteBlock(buffer, sentBytes, blockIdsList, bufferNumber, fileInfo);

                    response.Result = fileName.Split('.')[0].ToString();// "In Progress";

                }
                else if (sentBytes == fileSize)
                {
                    var fileName = BuidDocument(tenant, finalFileName, fileextension, fileSize, DocumentId, true);
                    string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileName.ToLower(), "docsin");
                    string[] fileParams = fileName.Split('.');
                    BlobFileInfo fileInfo = new BlobFileInfo()
                    {
                        FileName = fileParams[0],
                        FolderName = "docsin",
                        Extension = fileParams[1],
                        Tenant = tenant,
                        FileSize = fileSize,

                    };

                    IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                    storageservice.WriteBlock(buffer, sentBytes, blockIdsList, bufferNumber, fileInfo);

                    //if (res.HasError)
                    //{
                    //    response.HasError = res.HasError;
                    //    response.ErrorMessage = res.ErrorMessage;
                    //}
                    //else
                    //{
                    response.Result = fileName.Split('.')[0].ToString(); //"Done";
                                                                         //}
                    
                }

                return response;

            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                string Error = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);

                        Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                    }
                }

                response.HasError = true;
                response.ErrorMessage = Error;

                return response;
            }

            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }
                return response;
            }
        }

        public Response UploadSignDocumentFileData(byte[] buffer, long fileSize, long sentBytes, string[] blockIdsList, int bufferNumber, int tenant, string FileNameWithExtention, string DocumentFilingId, ref bool isDigitallySigned, ref string signersList)
        {
            Response response = new Response();
            try
            {
                Uploader up = new Uploader();
                var DocId = up.UploadPdfFile(FileNameWithExtention, buffer, fileSize, sentBytes, blockIdsList, bufferNumber, DocumentFilingId, tenant, "", FileNameWithExtention, ref isDigitallySigned, ref signersList);
                response.Result = DocId;
                return response;
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }
                return response;
            }

        }

        public string BuidDocument(int tenant, string fileName, string fileExtention, long fileSize, string DocId, bool hasfile)
        {
            // try
            //{
            DocumentsFilingRepository externalDocumentRepository = new DocumentsFilingRepository(tenant);
            DocumentRepository docRepository = new DocumentRepository(tenant);

            //DocumentsFiling externalDocument = externalDocumentRepository.GetSingleDocumentsFiling(externalDocumentId, tenant);
            Document document = null;
            if (!string.IsNullOrEmpty(DocId))
            {
                document = docRepository.GetSingleDocument(tenant, DocId);
            }

            //string[] fileParams = FileNameWithExtention.Split('.');
            //string finalFileName = FileNameWithExtention.Substring(0, FileNameWithExtention.LastIndexOf('.'));
            // string fileextension = fileParams[fileParams.Length - 1];

            //string realFileName = null;
            //if (!string.IsNullOrEmpty(fileName))
            //{
            //    realFileName = fileName.Split('.')[0];
            //}

            if (document == null)
            {
                document = new Document()
                {
                    CreateDate = DateTime.Now,
                    Extension = fileExtention,
                    FileSize = Convert.ToInt32(fileSize),
                    Tenant = tenant,
                    Id = IdCounter.GetNumber("Document", tenant).ToString(),//externalDocumentId,
                    HasFile = hasfile,
                    Folder = "docsin",
                    FileName = fileName,
                };
                docRepository.Add(document);
            }
            else
            {
                document.CreateDate = DateTime.Now;
                document.Extension = fileExtention;
                document.FileSize = Convert.ToInt32(fileSize);
                document.Tenant = tenant;
                document.HasFile = hasfile;
                document.Folder = "docsin";
                document.FileName = fileName;
                document.IsEncrypted = true;
                docRepository.Update(document);
            }

            docRepository.SubmitChanges();
            fileNameAndExtension = document.Id + "." + document.Extension;
            //externalDocument.DocumentId = document.Id;
            //externalDocumentRepository.Update(externalDocument);
            //externalDocumentRepository.SubmitChanges();

            return fileNameAndExtension;

            //}

            //catch (Exception e)
            // {
            //    string ip = "";
            //   if (HttpContext.Current != null && HttpContext.Current.Request != null)
            //  {
            //        ip = HttpContext.Current.Request.UserHostAddress;
            // }
            // }
            //return documentIdAndExtension;
        }


        public BlobInfo UploadBlobInfoToStorage(BlobInfo blobInfo, int tenant)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            IWebFreightContext webFreightContext = WebFreightContext.GetContext(tenant);

            if (string.IsNullOrEmpty(blobInfo.Extension))
            {
                throw new ApplicationException("Invalid blob extension");
            }


            //BlobChunkDetails blobChunkDetails = new BlobChunkDetails();
            //blobChunkDetails.SentBytes = blobInfo.BlobChunk.Length;
            //blobChunkDetails.BlockIdsList = new List<string>();
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            //if (!string.IsNullOrEmpty(blobInfo.BlobId))
            //{
            //    string cachedBlobDetails = LogitudeCacheManager.ServerCache.GetFromCache("BlobChunkDetails:" + blobInfo.BlobId + "." + blobInfo.Extension);
            //    if (!string.IsNullOrEmpty(cachedBlobDetails))
            //    {
            //        blobChunkDetails = JsonConvert.DeserializeObject<BlobChunkDetails>(cachedBlobDetails);
            //        blobChunkDetails.BlobNumber += 1;
            //        blobChunkDetails.SentBytes += blobInfo.BlobChunk.Length;

            //    }
            //}
            //blobChunkDetails.BlockIdsList.Add(Convert.ToBase64String(Guid.NewGuid().ToByteArray()));
            if (blobInfo.TotalSentChunksSize < blobInfo.BlobSize)
            {
                var fileName = BuidDocument(tenant, blobInfo.BlobId, blobInfo.Extension, blobInfo.BlobSize, blobInfo.BlobId, false);
                string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileName.ToLower(), "docsin");
                string[] fileParams = fileName.Split('.');
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = fileParams[0],
                    FolderName = "docsin",
                    Extension = fileParams[fileParams.Length - 1],
                    Tenant = tenant,
                    FileSize = blobInfo.BlobSize,

                };

                storageservice.WriteBlock(blobInfo.BlobChunk, blobInfo.TotalSentChunksSize, blobInfo.BlobChunkIdsList.ToArray(), blobInfo.BlobChunkNumber, fileInfo);
                blobInfo.BlobId = fileName.Split('.')[0].ToString();// "In Progress";

                //LogitudeCacheManager.ServerCache.AddToCache("BlobChunkDetails:" + blobInfo.BlobId + "." + blobInfo.Extension, JsonConvert.SerializeObject(blobChunkDetails), new TimeSpan(0, 15, 0));

            }
            else if (blobInfo.TotalSentChunksSize == blobInfo.BlobSize)
            {
                var fileName = BuidDocument(tenant, blobInfo.BlobId, blobInfo.Extension, blobInfo.BlobSize, blobInfo.BlobId, true);
                string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileName.ToLower(), "docsin");
                string[] fileParams = fileName.Split('.');
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = fileParams[0],
                    FolderName = "docsin",
                    Extension = fileParams[1],
                    Tenant = tenant,
                    FileSize = blobInfo.BlobSize,

                };

                storageservice.WriteBlock(blobInfo.BlobChunk, blobInfo.TotalSentChunksSize, blobInfo.BlobChunkIdsList.ToArray(), blobInfo.BlobChunkNumber, fileInfo);
                blobInfo.BlobId = fileName.Split('.')[0].ToString(); //"Done";

                //LogitudeCacheManager.ServerCache.RemoveFromCache("BlobChunkDetails:" + blobInfo.BlobId + "." + blobInfo.Extension);

            }

            return blobInfo;
        }

        
    }

    //public class BlobChunkDetails
    //{
    //    public int BlobNumber { get; set; }
    //    public List<string> BlockIdsList { get; set; }
    //    public int SentBytes { get; set; }
    //}

}
