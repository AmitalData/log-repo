using System;
using System.IO;
using System.Linq;
using System.Web.Services;
using Microsoft.WindowsAzure.Storage;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.Azure;
using WebFreight.Web.CommonDataModel;
using WebFreight.Web.Helpers;
using WebFreight.Web.Testing;
using Microsoft.WindowsAzure.Storage.Blob;
using Logitude.SystemLogs;
using Logitude.Server.Tools.Counters;
using System.Web;
using Logitude.Server.Tools.StorageService;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Text;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.HybridMapping;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.ExternalServices;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using System.Xml.Linq;
using Logitude.BL.Helpers;
using Simplog.Data.ShipmentsModel;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.Server.Tools.QueueService;
using WebFreight.Web.Helpers.Documents;
using System.Globalization;
using System.Configuration;

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for Uploader
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Uploader : System.Web.Services.WebService
    {
        public Uploader()
        {

        }
        CloudBlobContainer blobContainer;
        CloudBlockBlob tempcloudBlockBlob;
        CloudBlockBlob finalcloudBlockBlob;
        private long receivedBytes = 0;
        bool firstTry = true;
        string documentIdAndExtension;
        string fileNameAndExtension;
        private TenantQuery tenantQuery;

        public long ReceivedBytes
        {
            get { return receivedBytes; }
            set { receivedBytes = value; }
        }

        [WebMethod]

        public string UploadFile(string generatedfilename, byte[] buffer, long fileSize, long sentBytes, string[] blockIdsList, int bufferNumber, string externalDocumentId, int tenant, string fileLocation, string filename, bool forceCreateDocument, string documentId)
        {

            try
            {
                string signerslist = "";
                bool isSigned = false;
                documentIdAndExtension = UploadFileData(generatedfilename, buffer, fileSize, sentBytes, blockIdsList, bufferNumber, externalDocumentId, tenant, fileLocation, filename, ref isSigned, ref signerslist, forceCreateDocument, documentId);
            }
            catch (Exception e)
            {
                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }

                if (!LogitudeSettings.IsCostomsDeploy)
                {
                    ExceptionHandler.HandleException(e, DateTime.Now, tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "Uploader : UploadFile Method", ip);
                    throw (e);
                }
                else
                {
                    var myLogedEx = e.HandleException(User != null ? User.Identity.Name : "", "Uploader : DownloadFile Method" + ip);
                    if (myLogedEx != null)
                    {
                        throw new Exception(myLogedEx.ToString());
                    }
                }
            }

            return documentIdAndExtension;

        }




        public string TruncateLongString(string str, int maxLength)
        {
            if (!string.IsNullOrEmpty(str))

                return str.Substring(0, Math.Min(str.Length, maxLength));

            else
                return str;
        }

        private string BuidDocument(int tenant, string externalDocumentId, long fileSize, string fileName)
        {
            try
            {
                DocumentsFilingRepository externalDocumentRepository = new DocumentsFilingRepository(tenant);
                DocumentRepository docRepository = new DocumentRepository(tenant);
                DocumentsFiling externalDocument = externalDocumentRepository.GetSingleDocumentsFiling(externalDocumentId, tenant);
                Document document = null;


                if (externalDocument != null)
                {
                    if (externalDocument.DocumentId != null) document = docRepository.GetSingleDocument(tenant, externalDocument.DocumentId);
                    string[] fileParams = fileName.Split('.');
                    string fileextension = fileParams[fileParams.Length - 1];
                    string finalFileName = externalDocumentId + "." + fileextension;

                    string realFileName = null;
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        realFileName = fileName.Substring(0, fileName.LastIndexOf('.'));
                    }

                    if (document == null)
                    {
                        document = new Document()
                        {
                            CreateDate = DateTime.Now,
                            Extension = fileextension,
                            FileSize = Convert.ToInt32(fileSize),
                            Tenant = Convert.ToInt32(externalDocument.Tenant),
                            Id = IdCounter.GetNumber("Document", tenant).ToString(),//externalDocumentId,
                            HasFile = true,
                            Folder = "docsin",
                            FileName = TruncateLongString(realFileName, 120),
                            CalculatedFileName = new DocumentTypeCalculateFileNameService(externalDocument, TruncateLongString(realFileName, 120)).Calculate(),

                        };
                        docRepository.Add(document);
                    }
                    else
                    {
                        document.CreateDate = DateTime.Now;
                        document.Extension = fileextension;
                        document.FileSize = Convert.ToInt32(fileSize);
                        document.Tenant = Convert.ToInt32(externalDocument.Tenant);
                        document.HasFile = true;
                        document.Folder = "docsin";
                        document.IsEncrypted = true;
                        document.FileName = TruncateLongString(realFileName, 120);
                        document.CalculatedFileName = new DocumentTypeCalculateFileNameService(externalDocument, document.FileName).Calculate();
                        docRepository.Update(document);
                    }

                    docRepository.SubmitChanges();
                    fileNameAndExtension = document.Id + "." + document.Extension;
                    externalDocument.DocumentId = document.Id;
                    externalDocumentRepository.Update(externalDocument);
                    externalDocumentRepository.SubmitChanges();
                }
                return fileNameAndExtension;

            }

            catch (Exception e)
            {
                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }
                ExceptionHandler.HandleException(e, DateTime.Now, tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "Uploader : UploadFile Method", ip);
            }
            return documentIdAndExtension;
        }


        [WebMethod]
        public string UploadPdfFile(string generatedfilename, byte[] buffer, long fileSize, long sentBytes, string[] blockIdsList, int bufferNumber, string externalDocumentId, int tenant, string fileLocation, string filename, ref bool isDigitallySigned, ref string signersList)
        {
            try
            {
                documentIdAndExtension = UploadFileData(generatedfilename, buffer, fileSize, sentBytes, blockIdsList, bufferNumber, externalDocumentId, tenant, fileLocation, filename, ref isDigitallySigned, ref signersList, false, null);

                if (!string.IsNullOrEmpty(documentIdAndExtension))
                {
                    tenantQuery = new TenantQuery(tenant);
                    TenantPM tenantPM = tenantQuery.GetSinglePM(tenant);

                    if (tenantPM.IsDocumentsArchive == true)
                    {
                        ObjectTableRepository objectTableRepository = new ObjectTableRepository(tenant);
                        ShipmentComputedFieldsRepository shipmentComputedFieldsRepository = new ShipmentComputedFieldsRepository(tenant);
                        DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
                        DocumentsFilingPM extDocPM = documentsFilingQuery.GetSinglePM(externalDocumentId, tenant);
                        var OTName = objectTableRepository.GetSingleObjectTable(extDocPM.ObjectTableId, tenant, false);
                        if (OTName.Name == "Shipment")
                        {
                            var ShipmentCompField = shipmentComputedFieldsRepository.GetSingleShipmentComputedFields(extDocPM.EntityId, tenant);

                            if (ShipmentCompField != null)
                            {
                                ShipmentCompField.LastDocumentDateTime = DateTime.Now;
                                ShipmentCompField.MissingDocumentsCount = documentsFilingQuery.GetMissingDocCountForEntity(extDocPM.EntityId, extDocPM.ObjectTableId, tenant);
                                ShipmentCompField.MissingDocumentsNames = documentsFilingQuery.GetMissingDocsNamesForEntity(extDocPM.EntityId, extDocPM.ObjectTableId, tenant);
                                if (ShipmentCompField.MissingDocumentsCount == 0)
                                {
                                    ShipmentCompField.IsMissingDocuments = false;
                                }
                                else
                                {
                                    ShipmentCompField.IsMissingDocuments = true;
                                }
                                ShipmentCompField.IsRequestedDocuments = documentsFilingQuery.GetIfIsRequestedForEntity(extDocPM.EntityId, tenant);
                                ShipmentCompField.RequestedDocumentsCount = documentsFilingQuery.GetRequestedDocCountForEntity(extDocPM.EntityId, tenant);

                                //if (extDocPM.HasFile)
                                //{
                                //    //DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
                                //    //DocumentsFilingPM extDocPM = documentsFilingQuery.GetSinglePM(externalDocumentId, tenant);
                                //    bool hasmissing = documentsFilingQuery.CheckMissingDocForEntity(extDocPM.EntityId, extDocPM.ObjectTableId, tenant);
                                //    ShipmentCompField.IsMissingDocuments = hasmissing;
                                //}
                                //else
                                //{
                                //    if (extDocPM.DocumentTypeCode == "740" || extDocPM.DocumentTypeCode == "706" || extDocPM.DocumentTypeCode == "380")
                                //    {
                                //        ShipmentCompField.IsMissingDocuments = true;
                                //    } 
                                //}
                                //ShipmentCompField.MissingDocumentsCount = documentsFilingQuery.GetMissingDocCountForEntity(extDocPM.EntityId, extDocPM.ObjectTableId, tenant);

                                ShipmentComputedFieldsHelper shipmentComputedFieldsHelper = new ShipmentComputedFieldsHelper();
                                shipmentComputedFieldsHelper.UpdateShipmentComputedFields(ShipmentCompField, shipmentComputedFieldsRepository.context);

                                //shipmentComputedFieldsRepository.Update(ShipmentCompField);
                                // shipmentComputedFieldsRepository.SubmitChanges();
                            }
                        }

                    }

                }

            }


            catch (Exception e)
            {
                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }
                ExceptionHandler.HandleException(e, DateTime.Now, tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "Uploader : UploadFile Method", ip);
            }

            return documentIdAndExtension;

        }

        [WebMethod]
        public string UploadImage(string filename, byte[] buffer, long fileSize, long sentBytes, string[] blockIdsList, int bufferNumber, int tenant, string extension, string cardId, string contactId, string imageDetalId)
        {
            string filelocation = GetFileLocation("images");
            string fileName = filename.ToLower();
            string filePath = "tenant" + tenant.ToString() + "/";
            string imagedetailid = null;
            try
            {


                ImageDetailRepository imageDetailRep = new ImageDetailRepository(tenant);
                if (!string.IsNullOrEmpty(cardId))
                {
                    CardRepository cardRep = new CardRepository(tenant);
                    Card card = cardRep.GetSingleCard(cardId, tenant);
                    if (string.IsNullOrEmpty(card.ImageDetailId))
                    {

                        ImageDetail imagedetail = new ImageDetail() { Id = IdCounter.GetNumber("ImageDetail", tenant), Tenant = tenant, Extension = extension, Size = fileSize };
                        imageDetailRep.Add(imagedetail);
                        imageDetailRep.SubmitChanges();
                        imagedetailid = imagedetail.Id;
                        card.ImageDetailId = imagedetail.Id;
                        cardRep.Update(card);
                        cardRep.SubmitChanges();
                    }
                    else
                    {
                        UpdateImageDetails(new ImageDetailPM() { Id = card.ImageDetailId, Tenant = card.Tenant, Extension = extension, Size = fileSize }, imageDetailRep);
                    }

                    imagedetailid = card.ImageDetailId;

                }

                else if (!string.IsNullOrEmpty(contactId))
                {
                    ContactRepository contactRepository = new ContactRepository(tenant);
                    Contact contact = contactRepository.GetSingleContact(contactId, tenant);
                    if (string.IsNullOrEmpty(contact.ImageDetailId))
                    {

                        ImageDetail imagedetail = new ImageDetail() { Id = IdCounter.GetNumber("ImageDetail", tenant), Tenant = tenant, Extension = extension, Size = fileSize };
                        imageDetailRep.Add(imagedetail);
                        imageDetailRep.SubmitChanges();
                        imagedetailid = imagedetail.Id;

                        contact.ImageDetailId = imagedetail.Id;
                        contactRepository.Update(contact);
                        contactRepository.SubmitChanges();
                    }
                    else
                    {
                        UpdateImageDetails(new ImageDetailPM() { Id = contact.ImageDetailId, Tenant = contact.Tenant, Extension = extension, Size = fileSize }, imageDetailRep);
                    }

                    imagedetailid = contact.ImageDetailId;

                }
                else
                {
                    ImageDetail imagedetail = null;
                    if (!string.IsNullOrEmpty(imagedetailid))
                    {
                        imagedetail = imageDetailRep.GetSingleImageDetail(imagedetailid, tenant);
                    }
                    if (imagedetail == null)
                    {
                        imagedetail = new ImageDetail() { Id = IdCounter.GetNumber("ImageDetail", tenant), Tenant = tenant, Extension = extension, Size = fileSize };
                        imageDetailRep.Add(imagedetail);
                        imageDetailRep.SubmitChanges();
                    }
                    imagedetailid = imagedetail.Id;

                }

                fileName = imagedetailid;

                //blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);



                //tempcloudBlockBlob = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(fileName, ""));

                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;


                ReceivedBytes += buffer.Length;
                fileNameAndExtension = fileName + "." + extension;
                //if (sentBytes < fileSize)
                //{
                //MemoryStream memorystream = new MemoryStream(buffer);
                //tempcloudBlockBlob.PutBlock(blockIdsList[bufferNumber], memorystream, null);
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = fileName,
                    FolderName = filelocation,
                    Extension = extension,
                    Tenant = tenant,
                    FileSize = fileSize,

                };
                //filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileNameAndExtension, filelocation);
                storageservice.WriteBlock(buffer, sentBytes, blockIdsList, bufferNumber, fileInfo);

                // }

                //else
                //{

                if (sentBytes == fileSize)
                    fileNameAndExtension = fileName + "." + extension;

                //MemoryStream memorystream = new MemoryStream(buffer);
                //tempcloudBlockBlob.PutBlock(blockIdsList[bufferNumber], memorystream, null);

                //int numberOfBlocks = blockIdsList.Length;
                //String[] blockIds = new String[numberOfBlocks];
                //for (int i = 0; i < numberOfBlocks; i++)
                //{
                //    blockIds[i] = blockIdsList[i];
                //}

                //tempcloudBlockBlob.PutBlockList(blockIds);

                //finalcloudBlockBlob = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(fileNameAndExtension, "images"));
                ////finalcloudBlockBlob.CopyFromBlob(tempcloudBlockBlob); // copy temp file to final file
                //finalcloudBlockBlob.StartCopyFromBlob(tempcloudBlockBlob);
                //tempcloudBlockBlob.DeleteIfExists();

                documentIdAndExtension = fileNameAndExtension;

                //    blockIdsList = null;




                //}
            }
            //}
            catch (Exception e)
            {
                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }
                ExceptionHandler.HandleException(e, DateTime.Now, tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "Uploader : UploadImage Method", ip);
            }
            return imagedetailid;
        }




        private void UpdateImageDetails(ImageDetailPM imageDetailPM, ImageDetailRepository imageDetailRepository)
        {
            var imagedetail = imageDetailRepository.GetSingleImageDetail(imageDetailPM.Id, imageDetailPM.Tenant);
            if (imagedetail == null) return;
            imagedetail.Extension = imageDetailPM.Extension;
            imagedetail.Size = imageDetailPM.Size;
            imageDetailRepository.Update(imagedetail);
            imageDetailRepository.SubmitChanges();
        }





        byte[] datainByte;

        [WebMethod]
        public void RemoveFile(string documentId, int tenant)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);

            Document document = (from doc in commonContext.Documents
                                 where doc.Id == documentId
                                 select doc).FirstOrDefault();
            if (document != null)
            {
                try
                {
                    ObjectTableRepository objectTableRepository = new ObjectTableRepository(tenant);
                    ShipmentComputedFieldsRepository shipmentComputedFieldsRepository = new ShipmentComputedFieldsRepository(tenant);
                    DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
                    DocumentsFilingPM extDocPM = documentsFilingQuery.GetDocumentsFilingByDocumentId(documentId, tenant);


                    // 
                    if (extDocPM != null)
                    {
                        DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(tenant);
                        DocumentsFiling extDocPoco = documentsFilingRepository.GetSingleDocumentsFilingByDocumentId(documentId, tenant);
                        extDocPoco.IsDeleted = true;
                        documentsFilingRepository.Update(extDocPoco);
                        documentsFilingRepository.SubmitChanges();
                    }

                    tenantQuery = new TenantQuery(extDocPM.Tenant);
                    TenantPM tenantPM = tenantQuery.GetSinglePM(extDocPM.Tenant);
                    if (tenantPM.IsDocumentsArchive)
                    {

                        var OTName = objectTableRepository.GetSingleObjectTable(extDocPM.ObjectTableId, tenant, false);
                        if (OTName.Name == "Shipment")
                        {
                            var ShipmentCompField = shipmentComputedFieldsRepository.GetSingleShipmentComputedFields(extDocPM.EntityId, tenant);
                            ShipmentCompField.IsMissingDocuments = true;
                            shipmentComputedFieldsRepository.Update(ShipmentCompField);
                            shipmentComputedFieldsRepository.SubmitChanges();
                        }
                    }

                    Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                    {
                        FileName = document.Id,
                        FolderName = document.Folder,
                        Extension = document.Extension,
                        Tenant = document.Tenant,
                        FileSize = document.FileSize,

                    };
                    Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                    storageservice.Delete(fileInfo);


                    UpdateDocument(commonContext, document);
                    OpenKPIDocumentUploderQueue(extDocPM, commonContext);
                    //else // In Azure
                    //{
                    //string filename = document.Id + "." + document.Extension;
                    //blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);
                    //var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder));

                    //blobfile.DeleteIfExists();


                    // }

                }
                catch (Exception e)
                {
                    UpdateDocument(commonContext, document);
                    string ip = "";
                    if (HttpContext.Current != null && HttpContext.Current.Request != null)
                    {
                        string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                        if (string.IsNullOrEmpty(currentIP))
                        {
                            currentIP = HttpContext.Current.Request.UserHostAddress;
                        }
                        ip = currentIP;
                    }
                    ExceptionHandler.HandleException(e, DateTime.Now, tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "Uploader : RemoveFile Method", ip);
                }
            }

        }

        private void UpdateDocument(ICommonDataContext commonContext, Document document)
        {
            if (document != null)
            {
                document.FileName = null;
                document.FileSize = null;
                document.HasFile = false;
                document.Extension = null;
                commonContext.SaveChanges();
            }
        }

        private void OpenKPIDocumentUploderQueue(DocumentsFilingPM documentsFiling, ICommonDataContext commonContext)
        {
            //if (documentsFiling.DocumentTypeCode == "POD")
            //{
            //    IQueueService queueservice = new DbQueueService();
            //    queueservice.InitializeQueue("PODDocumnetUploaderQueue", documentsFiling.Tenant);
            //    queueservice.Send(new Dictionary<string, string>() { { "EntityId", documentsFiling.Id }, { "Tenant", documentsFiling.Tenant.ToString() },
            //                                                         { "IsPODDocumentUploaded", false.ToString() }, { "IsPODDocumentDeleted", true.ToString() }, { "PODRecived", documentsFiling.ReceivedDate.ToString() } },
            //                                                          documentsFiling.Tenant, null, null, null, null);
            //}

            if (IsStartingUploadShipmentDocs(documentsFiling.DocumentTypeCode))
            {
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("ShipmentDocsInUploaderQueue", documentsFiling.Tenant);
                queueservice.Send(new Dictionary<string, string>() {
                    { "EntityId", documentsFiling.Id },
                    { "Tenant", documentsFiling.Tenant.ToString() },
                    { "DocumentCode",  documentsFiling.DocumentTypeCode },
                    { "IsDocumentUploaded", false.ToString() },
                    { "IsDocumentDeleted", true.ToString() },
                    { "RecivedDate", documentsFiling.ReceivedDate.ToString() } },
                    documentsFiling.Tenant, null, null, null, null);
            }
        }

        private bool IsStartingUploadShipmentDocs(string documentTypeCode)
        {
            if (documentTypeCode == "POD")
            {
                return true;
            }

            else if (documentTypeCode == "380")
            {
                return true;
            }

            else if (documentTypeCode == "721")
            {
                return true;
            }

            else if (documentTypeCode == "706")
            {
                return true;
            }

            else if (documentTypeCode == "704")
            {
                return true;
            }

            else if (documentTypeCode == "ARNT")
            {
                return true;
            }

            return false;
        }

        public byte[] GetPageTiffAsB64FromTarByTenantComIdPage(
           string documentId, int tenant, int currPage, out string TiffPageLines,
            out string ErrorMessage
            )
        {
            ErrorMessage = ErrorMessage = null;
            TiffPageLines = null;
            try
            {
                if (String.IsNullOrWhiteSpace(LogitudeSettings.GetLogitudeCustomsSettingsMInject(tenant).OnPremiseFillingService))
                {
                    return null;
                }

                string documentExtension = GetFileExtension(documentId, tenant);
                documentExtension = documentExtension ?? "";
                documentExtension = documentExtension.ToLower();
                bool myExt = (documentExtension == "tiff" || documentExtension == "tif" || documentExtension == "pdf");
                if (!myExt) return null;
                var externalDocumentRepository = new DocumentsFilingRepository(tenant);
                var poco = externalDocumentRepository.GetSingleDocumentsFilingByDocumentId(documentId, tenant);
                if (poco == null)
                {
                    return null;
                }
                var UDocumentsFilingId = poco.Id;
                var unifreightFillingService = new UnifreightFillingService();
                int iPage = currPage;
                var tiffByte = unifreightFillingService.DownloadTiff(tenant, UDocumentsFilingId, iPage, out TiffPageLines, out ErrorMessage);
                return tiffByte;
            }
            catch (Exception eee)
            {
                ErrorMessage = eee.Message;
                return null;
            }
        }


        // this method called from server : can't be called from client
        public string GetFileExtension(string documentId, int tenant, bool withOutTenant = false)
        {
            string extension = "";
            ICommonDataContext context = CommonDataContext.GetContext(tenant);
            Document document = context.Documents.Where(doc => doc.Id == documentId && doc.Tenant == tenant).FirstOrDefault();
            if (document != null)
            {
                extension = document.Extension;
            }
            else
            {
                if (withOutTenant)
                {
                    document = context.Documents.Where(doc => doc.Id == documentId).FirstOrDefault();
                    if (document != null)
                    {
                        extension = document.Extension;
                    }
                }
            }

            return extension;


        }


        public Document GetDocumentById(string documentId, int tenant)
        {
            ICommonDataContext context = CommonDataContext.GetContext(tenant);
            Document document = context.Documents.Where(doc => doc.Id == documentId).FirstOrDefault();

            if (document != null)
            {
                if (string.IsNullOrEmpty(document.CalculatedFileName))
                {
                    document.CalculatedFileName = document.FileName;
                }

            }
            return document;

        }


        public List<DocumentsFilingPM> GetDocumentByEntityAndTenant(string EntityId, int tenant)
        {
            ICommonDataContext myContext = CommonDataContext.GetContext(tenant);
            DocumentsFilingRepository myDocumentsFilingRepository = new DocumentsFilingRepository(myContext);
            DocumentsFilingQuery myDocumentsFilingQuery = new DocumentsFilingQuery(myDocumentsFilingRepository);
            List<DocumentsFilingPM> myDocumentFilings = myDocumentsFilingQuery.GetDocumentsFilingPMsByEntityId(EntityId, tenant);
            return myDocumentFilings;

        }

        public List<DocumentsFilingPM> GetMasterDocumentsAndItsConnectedHousesDocuments(string entityId, int tenant, string partnerType)
        {
            ICommonDataContext myContext = CommonDataContext.GetContext(tenant);
            DocumentsFilingRepository myDocumentsFilingRepository = new DocumentsFilingRepository(myContext);
            DocumentsFilingQuery myDocumentsFilingQuery = new DocumentsFilingQuery(myDocumentsFilingRepository);

            List<DocumentsFilingPM> myDocumentFilings = myDocumentsFilingQuery.GetDocumentsFilingPMsByEntityId(entityId, tenant);
            if (partnerType == "AG") myDocumentFilings = GetAgentDocuments(myDocumentFilings, "C", tenant);

            List<DocumentsFilingPM> housesDocumentFilings = GetConnectedHousesDocumentsFilingPMs(entityId, tenant, myDocumentsFilingQuery, partnerType);
            myDocumentFilings = myDocumentFilings.Concat(housesDocumentFilings).ToList();

            return myDocumentFilings;
        }

        private List<DocumentsFilingPM> GetConnectedHousesDocumentsFilingPMs(string entityId, int tenant, DocumentsFilingQuery myDocumentsFilingQuery, string partnerType)
        {
            List<string> housesShipmentsIds = GetAllConnectedHousesShipmentByShipmentMasterId(entityId, tenant);
            List<DocumentsFilingPM> myDocumentFilings = new List<DocumentsFilingPM>();
            for (int i = 0; i < housesShipmentsIds.Count(); i++)
            {
                List<DocumentsFilingPM> houseDocumentFilings = myDocumentsFilingQuery.GetDocumentsFilingPMsByEntityId(housesShipmentsIds[i], tenant);
                myDocumentFilings = myDocumentFilings.Concat(houseDocumentFilings).ToList();
            }
            return partnerType == "AG" ? GetAgentDocuments(myDocumentFilings, "H", tenant) : myDocumentFilings;
        }

        public List<DocumentsFilingPM> GetAgentDocuments(List<DocumentsFilingPM> documentsFilings, string shipmentLevelCode, int tenant)
        {
            if (IsCloudEnvironment() || !FeatureToggleHelper.HasFeatureToggle("DFP", tenant))
                return documentsFilings.Where(d => d.IsAgentView).ToList();

            if (shipmentLevelCode == "C")
                return documentsFilings.Where(d => d.IsAgentSharedInMaster).ToList();

            if (shipmentLevelCode == "D")
                return documentsFilings.Where(d => d.IsAgentSharedInDirect).ToList();

            if (shipmentLevelCode == "H")
                return documentsFilings.Where(d => d.IsAgentSharedInHouse).ToList();

            return documentsFilings.Where(d => d.IsAgentView).ToList();
        }

        private bool IsCloudEnvironment()
        {
            return Simplog.Server.Infrastructure.LogitudeSettings.WorkEnvironment == "cloud";
        }

        private static List<string> GetAllConnectedHousesShipmentByShipmentMasterId(string entityId, int tenant)
        {
            IShipmentsContext shipmentContext = ShipmentsContext.GetContext(tenant);
            ShipmentConsoleShipmentQuery shipmentConsoleShipmentQuery = new ShipmentConsoleShipmentQuery(shipmentContext);
            List<string> housesShipmentsIds = shipmentConsoleShipmentQuery.GetMasterConnectedHouseShipments(entityId, tenant)
                                                                          .Select(shipment => shipment.Id).ToList();
            return housesShipmentsIds;
        }

        [WebMethod]
        public byte[] DownloadFile(string documentId, string documentExtension, string fileLocation, int tenant, bool withOutTenant = false)
        {
            if (fileLocation == "logos" || fileLocation == "images")
            {
                try
                {


                    string fileName = documentId + "." + documentExtension;
                    string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileName.ToLower(), fileLocation);
                    IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                    BlobFileInfo fileInfo = new BlobFileInfo()
                    {
                        FileName = documentId,
                        FolderName = fileLocation,
                        Extension = documentExtension,
                        Tenant = tenant,


                    };
                    datainByte = storageservice.Read(fileInfo);



                    return datainByte;

                }
                catch (Exception e)
                {
                    string ip = "";
                    if (HttpContext.Current != null && HttpContext.Current.Request != null)
                    {
                        string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                        if (string.IsNullOrEmpty(currentIP))
                        {
                            currentIP = HttpContext.Current.Request.UserHostAddress;
                        }
                        ip = currentIP;
                    }
                    ExceptionHandler.HandleException(e, DateTime.Now, tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "Uploader : DownloadFile Method", ip);
                    return null;
                }
            }
            else
            {
                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);

                Document document = (from doc in commonContext.Documents
                                     where doc.Id == documentId && doc.Tenant == tenant
                                     select doc).FirstOrDefault();

                if (document == null && withOutTenant)
                {
                    document = (from doc in commonContext.Documents
                                where doc.Id == documentId
                                select doc).FirstOrDefault();
                }



                if (document != null)
                {
                    //string filelocation = GetFileLocation(FileLocation);
                    try
                    {
                        string fileName = document.Id + "." + document.Extension;

                        BlobFileInfo fileInfo = new BlobFileInfo()
                        {
                            FileName = document.Id,
                            FolderName = document.Folder,
                            Extension = document.Extension,
                            Tenant = document.Tenant,
                        };

                        //string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileName.ToLower(), document.Folder);
                        IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                        datainByte = storageservice.Read(fileInfo);


                        return datainByte;


                    }
                    catch (Exception e)
                    {
                        string ip = "";
                        if (HttpContext.Current != null && HttpContext.Current.Request != null)
                        {
                            string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                            if (string.IsNullOrEmpty(currentIP))
                            {
                                currentIP = HttpContext.Current.Request.UserHostAddress;
                            }
                            ip = currentIP;
                        }

                        if (!LogitudeSettings.IsCostomsDeploy)
                        {
                            ExceptionHandler.HandleException(e, DateTime.Now, tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "Uploader : DownloadFile Method", ip);

                        }
                        else
                        {
                            var myLogedEx = e.HandleException(User != null ? User.Identity.Name : "", "Uploader : DownloadFile Method" + ip);
                            if (myLogedEx != null)
                            {
                                throw myLogedEx;
                            }
                        }
                        return null;


                    }
                }
            }
            return null;
        }


        [WebMethod]
        public void CancelUpload(string documentId, string documentExtension, int tenant)
        {


            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(tenant);
            DocumentsFiling documentsFiling = documentsFilingRepository.GetSingleDocumentsFiling(documentId, tenant);

            if (documentsFiling != null)
            {
                DocumentRepository documentRepository = new DocumentRepository(tenant);
                Document document = documentRepository.GetSingleDocument(tenant, documentsFiling.DocumentId);

                if (document != null)
                {
                    Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                    {
                        FileName = document.Id,
                        FolderName = document.Folder,
                        Extension = document.Extension,
                        Tenant = document.Tenant,
                        FileSize = document.FileSize,
                    };

                    document.HasFile = false;
                    document.FileSize = null;
                    document.Extension = null;
                    document.FileName = null;
                    documentRepository.Update(document);
                    documentRepository.SubmitChanges();


                    try
                    {
                        Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                        storageservice.Delete(fileInfo);
                    }
                    catch (Exception e)
                    {
                        string ip = "";
                        if (HttpContext.Current != null && HttpContext.Current.Request != null)
                        {
                            string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                            if (string.IsNullOrEmpty(currentIP))
                            {
                                currentIP = HttpContext.Current.Request.UserHostAddress;
                            }
                            ip = currentIP;
                        }
                        ExceptionHandler.HandleException(e, DateTime.Now, document.Tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "Uploader : CancelUpload Method", ip);
                    }


                }

                documentsFiling.ReceivedDate = null;
                documentsFiling.ReceivedByUserId = null;

                documentsFilingRepository.Update(documentsFiling);
                documentsFilingRepository.SubmitChanges();
            }

            //ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);

            //Document document = (from doc in commonContext.Documents
            //                     where doc.Id == documentId
            //                     select doc).FirstOrDefault();
            //if (document != null)
            //{
            //    try
            //    {
            //        Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
            //        {
            //            FileName = document.Id,
            //            FolderName = document.Folder,
            //            Extension = document.Extension,
            //            Tenant = document.Tenant,
            //            FileSize = document.FileSize,
            //        };
            //        Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            //        storageservice.Delete(fileInfo);
            //    }
            //    catch (Exception e)
            //    {
            //        string ip = "";
            //        if (HttpContext.Current != null && HttpContext.Current.Request != null)
            //        {
            //            ip = HttpContext.Current.Request.UserHostAddress;
            //        }
            //        ExceptionHandler.HandleException(e, DateTime.Now, document.Tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "Uploader : CancelUpload Method", ip);
            //    }
            //}

        }

        private string GetFileLocation(string fileLocation)
        {

            if (String.IsNullOrEmpty(fileLocation))
            {
                fileLocation = "UserUploads";

            }
            switch (fileLocation)
            {
                case "UserUploads":
                    fileLocation = "UserUploads";
                    break;

                case "logos":
                    fileLocation = "logos";
                    break;
                case "images":
                    fileLocation = "images";
                    break;
            }

            return fileLocation;
        }
        //[WebMethod]
        //public byte[] DownloadFile(string filename)
        //{
        //    blobContainer = StorageAcountDetails.BlobClient.GetContainerReference("useruploads");
        //    var blobfile = blobContainer.GetBlockBlobReference(filename);

        //    using (MemoryStream memstream = new MemoryStream())
        //    {

        //        blobfile.DownloadToStream(memstream);
        //        DatainByte = memstream.ToArray();

        //    }

        //    return DatainByte;


        //}


        public byte[] DownloadStaticFile(string filename, string containername, int tenant = 0)
        {
            byte[] theDatainByte = null;

            try
            {
                string[] fileparams = filename.Split('.');
                string name = fileparams[0];
                string folder = "others";

                if (fileparams[0].Contains("/"))
                {
                    string[] myparams = fileparams[0].Split('/');
                    folder = myparams[0];
                    name = myparams[1];

                }

                // string filePath = containername + "/" + filename;//"tenant" + requestParams.Tenant + "/" + StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder);
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = fileparams[0],
                    Extension = fileparams[1],
                    ExternalContainerName = containername,
                    HasExternalContainer = true,
                    Tenant = tenant
                };

                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                theDatainByte = storageservice.Read(fileInfo);

             



                return theDatainByte;
                

            }
            catch (Exception e)
            {
                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }
                ExceptionHandler.HandleException(e, DateTime.Now, 0, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "Uploader : DownloadStaticFile Method", ip);
                return null;
            }

        }
        [WebMethod]
        public bool UploadStaticFile(byte[] fileData, string fileName, int tenant)
        {
            try
            {
                string[] fileparams = fileName.Split('.');
                string name = fileparams[0];
                string folder = "others";
                if (fileparams[0].Contains("/"))
                {
                    string[] myparams = fileparams[0].Split('/');
                    folder = myparams[0];
                    name = myparams[1];

                }
                Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                {
                    FileName = name,
                    FolderName = folder,
                    Extension = fileparams.Length > 1 ? fileparams[1] : null,
                    Tenant = tenant,
                    FileSize = fileData.Length,
                };
                Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                storageservice.Write(fileData, fileInfo);



                //string containername = StorageAcountDetails.GetCurrentContainer(tenant).Name;
                //blobContainer = StorageAcountDetails.BlobClient.GetContainerReference(containername);
                //var blobfile = blobContainer.GetBlockBlobReference(fileName);

                //using (MemoryStream memstream = new MemoryStream(fileData))
                //{

                //    blobfile.UploadFromStream(memstream);


                //}

                return true;
            }
            catch (Exception e)
            {
                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }
                ExceptionHandler.HandleException(e, DateTime.Now, 0, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "Uploader : DownloadStaticFile Method", ip);
                return false;
            }
        }




        private string UploadFileData(string generatedfilename, byte[] buffer, long fileSize, long sentBytes,
            string[] blockIdsList, int bufferNumber, string externalDocumentId, int tenant,
            string fileLocation, string filename, ref bool isDigitallySigned, ref string signersList, bool forceCreateDocument, string documentId)
        {
            if (string.IsNullOrEmpty(fileLocation))
            {
                fileLocation = "docsin";
            }
            string filelocation = GetFileLocation(fileLocation);

            string fileName = !string.IsNullOrEmpty(filename) ? filename.ToLower() : generatedfilename;
            string filePath = "tenant" + tenant.ToString() + "/";

            string storageServiceMode = System.Configuration.ConfigurationManager.AppSettings.Get("StorageServiceMode");

            if (filelocation == "logos")
            {
                fileNameAndExtension = fileName + tenant + ".jpg";

                if (fileName == "verysmalllogo" || fileName == "sharedLogtsitcslogo")
                {
                    fileNameAndExtension = fileName + tenant + ".png";
                }

                filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileNameAndExtension.ToLower(), "logos");
            }
            else if (filelocation == "others")
            {
                fileNameAndExtension = generatedfilename;
                filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileNameAndExtension.ToLower(), "others");
            }
            else
            {
                fileNameAndExtension = BuidDocument(tenant, externalDocumentId, fileSize, fileName);
                fileName = fileNameAndExtension;
            }

            //filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileNameAndExtension.ToLower(), filelocation);
            string[] fileparams = fileNameAndExtension.Split('.');
            string finalFileName = fileNameAndExtension.Substring(0, fileNameAndExtension.LastIndexOf('.'));
            if (forceCreateDocument)
            {
                finalFileName = DocumentsCreator.Create(new DocumentsCreatorArgs
                {
                    Tenant = tenant,
                    FileName = fileName.Substring(0, fileName.LastIndexOf('.')),
                    FileExtension = fileparams[fileparams.Length - 1],
                    FileData = buffer,
                    FileFolder = filelocation,
                    DocumentId = documentId
                });

                fileNameAndExtension = finalFileName + "." + fileparams[fileparams.Length - 1];
                documentIdAndExtension = fileNameAndExtension;
            }
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = finalFileName,
                FolderName = filelocation,
                Extension = fileparams[fileparams.Length - 1],
                Tenant = tenant,
                FileSize = fileSize,

            };
            //string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileNameAndExtension.ToLower(), fileLocation);
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            storageservice.WriteBlock(buffer, sentBytes, blockIdsList, bufferNumber, fileInfo);

            if (sentBytes == fileSize)
            {
                documentIdAndExtension = fileNameAndExtension;
                blockIdsList = null;

                if (filelocation != "logos" && filelocation != "others")
                {
                    string loggedUserEmail = AuthenticationUtil.GetAuthenticatedUser();
                    UserRepository userRepository = new UserRepository(tenant);
                    ObjectTableQuery tablesQuery = new ObjectTableQuery(tenant);
                    ObjectTablePM table = tablesQuery.GetObjectTableByName("DocumentsFiling", 0);

                    User loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, loggedUserEmail, tenant, true);
                    DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
                    DocumentsFilingPM extDocPM = documentsFilingQuery.GetSinglePM(externalDocumentId, tenant);
                    AddDocumentBackupLog(extDocPM);

                    byte[] filedata = DownloadFile(extDocPM.DocumentId, fileName.ToLower(), filelocation, tenant);

                    if (filedata != null && filedata.Length > 0)
                    {
                        if (extDocPM.FileExtension.ToLower() == "pdf")
                        {
                            List<Dictionary<string, string>> signatures = new List<Dictionary<string, string>>();
                            string message = "";
                            bool success = this.GetSignatureMetadata(extDocPM.FileName, filedata, out signatures, out message);
                            if (success)
                            {
                                string Signerslist = "";
                                if (signatures.Count > 0)
                                {
                                    for (int i = 0; i < signatures.Count; i++)
                                    {
                                        try
                                        {
                                            Signerslist += signatures[i]["CN"] + ',';
                                            Signerslist += "Vat: " + signatures[i]["O"] + ',';
                                        }
                                        catch (Exception exc)
                                        {

                                        }

                                    }

                                    signersList = Signerslist.TrimEnd(',');
                                    isDigitallySigned = true;
                                }



                            }
                        }

                       
                    }
                    else
                    {
                        DocumentRepository docRepository = new DocumentRepository(tenant);
                        var document = docRepository.GetSingleDocument(tenant, extDocPM.DocumentId);
                        document.HasFile = false;
                        document.FileName = null;
                        document.FileSize = 0;
                        document.Extension = null;
                        document.CalculatedFileName = null;

                        docRepository.Update(document);
                        docRepository.SubmitChanges();

                        throw new Exception("File was not uploaded successfully. Please retry again.");
                    }
                }
            }

            return documentIdAndExtension;



        }


        private void AddDocumentBackupLog(DocumentsFilingPM entityPM)
        {
            int tenant = entityPM.Tenant;
            ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);
            DocumentFilingBackupSettingQuery documentFilingBackupSettingQuery = new DocumentFilingBackupSettingQuery(tenant);
            DocumentFilingBackupSettingPM settingsPM = documentFilingBackupSettingQuery.GetSinglePM(tenant, tenant);
            if (settingsPM != null && settingsPM.IsActive)
            {
                ObjectTableRepository objectTableRepository = new ObjectTableRepository(tenant);
                DocumentsFilingRepository externalDocumentRepository = new DocumentsFilingRepository(objectContext);
                var OTName = objectTableRepository.GetObjectTableByName("DocumentsFiling", tenant, true);

                TenantRepository tenantRepository = new TenantRepository(objectContext);
                Tenant currentTenant = tenantRepository.GetSingleTenant(tenant);

                //string manifestXML = LogitudeXmlSerializer.SerializeObjectToXmlString(manifestSL);
                byte[] logXML = LogitudeXmlSerializer.SerializeObject(entityPM);
                ObjectTableQuery tablesQuery = new ObjectTableQuery(tenant);

                CommunicationsParams logParams = new CommunicationsParams()
                {
                    Tenant = tenant,
                    From = currentTenant.Company,
                    To = "FTP",
                    CommunicationLogTypeCode = "DCBK",
                    QueueName = "DocumentFillingBackupQueue",
                    Priority = 1,
                    InOut = "O",
                    Status = "W",
                    LoggingUserId = entityPM.CreatedByUserId,
                    LoggingObjectTableId = OTName.Id,
                    LoggingEntityId = entityPM.Id,
                    Subject = "Document Backup",
                    FolderName = "DocumentFillingBackupQueue",
                    ByteData = logXML,
                    LoggingEntityReference = entityPM.Code,
                };

                //logParams.QueueParameters = new Dictionary<string, string>() { { "DocumentFilingId", entityPM.Id }, { "Tenant", tenant.ToString() } };
                Communications.AddCommunicationLog(logParams);

                //DocumentsFiling documentFilingPOCO = externalDocumentRepository.GetSingleDocumentsFiling(entityPM.Id);
                //entityPM.BackedupExternally = documentFilingPOCO.BackedupExternally = true;
                //externalDocumentRepository.Update(documentFilingPOCO);
                //externalDocumentRepository.SubmitChanges();
            }
        }
        private bool GetSignatureMetadata(string filename, byte[] filedata, out List<Dictionary<string, string>> signatures, out string message)
        {
            signatures = new List<Dictionary<string, string>>();
            message = "";
            try
            {
                PdfReader.debugmode = true;
                using (PdfReader reader = new PdfReader(filedata))
                {
                    AcroFields af = reader.AcroFields;
                    var names = af.GetSignatureNames();
                    for (int i = 0; i < names.Count; ++i)
                    {
                        Dictionary<string, string> metadata = new Dictionary<string, string>();
                        String name = (string)names[i];
                        PdfPKCS7 pk = af.VerifySignature(name);
                        //metadata.Add("Name", name);
                        //metadata.Add("SignName", pk.SignName);
                        metadata.Add("SignDate", pk.SignDate.ToString("dd.MM.yyyy HH:mi.ss"));
                        var subjectFields = CertificateInfo.GetSubjectFields(pk.SigningCertificate);
                        List<string> mdlist = new List<string>() { "C", "CN", "SN", "T", "OU", "O", "GIVENNAME", "SURNAME", };
                        if (subjectFields != null)
                        {
                            foreach (var md in mdlist)
                            {
                                string value = subjectFields.GetField(md);
                                if (!string.IsNullOrEmpty(value))
                                    metadata.Add(md, value);
                            }
                        }
                        signatures.Add(metadata);
                    }
                }
                return (true);
            }
            catch (Exception ex)
            {
                message = "Failed to get metadata from pdf file '" + filename + "'" + Environment.NewLine + ex.ToString();
                return (false);
            }
        }


        // Added By Maheera
        public Document GetFileExtensionBySecurityId(string securityId, int tenant)
        {

            if (!string.IsNullOrEmpty(securityId) && !securityId.Contains("+"))
            {
                securityId = System.Net.WebUtility.UrlEncode(securityId);
            }

            ICommonDataContext context = CommonDataContext.GetContext(tenant);
            DocumentsFilingRepository documentRepository = new DocumentsFilingRepository(tenant);
            DocumentsFiling documentFiling = documentRepository.GetSingleDocumentFilingBySecurityId(securityId, tenant);

            Document document = context.Documents.Where(doc => doc.Id == documentFiling.DocumentId).FirstOrDefault();
            if (document != null)
            {
                return document;
            }
            return null;
        }


        public Document GetFileExtensionBySecurityIdAndCopyId(string securityId, string copyid, int tenant)
        {


            if (!string.IsNullOrEmpty(securityId) && !securityId.Contains("+"))
            {
                securityId = System.Net.WebUtility.UrlEncode(securityId);
            }

            ICommonDataContext context = CommonDataContext.GetContext(tenant);
            DocumentsFilingRepository documentRepository = new DocumentsFilingRepository(tenant);
            DocumentsFiling documentFiling = documentRepository.GetSingleDocumentFilingBySecurityId(securityId, tenant);
            Document document = null;
            if (documentFiling != null)
            {


                if (!string.IsNullOrEmpty(copyid) && copyid != "null")
                {
                    var docoutcopy = context.DocumentOutCopies.Where(d => d.DocumentOutId == documentFiling.Id && d.Id == copyid && d.Tenant == tenant).FirstOrDefault();
                    if (docoutcopy != null)
                    {
                        string documentId = !string.IsNullOrEmpty(docoutcopy.DocumentId) ? docoutcopy.DocumentId : docoutcopy.Id;
                        document = context.Documents.Where(doc => doc.Id == documentId && doc.Tenant == tenant).FirstOrDefault();
                    }
                }
                else document = context.Documents.Where(doc => doc.Id == documentFiling.DocumentId && doc.Tenant == tenant).FirstOrDefault();

                if (document != null)
                {
                    document.CalculatedFileName = !string.IsNullOrEmpty(document.CalculatedFileName) ? document.CalculatedFileName : document.FileName;

                    //document.DocumentType = documentFiling.DocumentType != null ? documentFiling.DocumentType.Name : "";

                    if (documentFiling.DirectionCode == "I" && !FeatureToggleHelper.HasFeatureToggle("SFC", documentFiling.Tenant))
                    {
                        document.CalculatedFileName = document.FileName;
                    }

                    return document;
                }
                else return null;
            }
            return null;
        }


        public static HttpResponseMessage GetFileStream(string id)//THIS CODE USED  FROM  AmitalChromWinForm!!
        {
            DateTime stopLogAt = DateTime.MinValue;//DateTime stopLogAt = new DateTime(2020, 09, 01);
            string UntilDateyyyyMMdd = ConfigurationManager.AppSettings["20240818T155633.LogUntilDateyyyyMMdd"];
            if (!string.IsNullOrWhiteSpace(UntilDateyyyyMMdd))
            {
                stopLogAt = DateTime.ParseExact(UntilDateyyyyMMdd,
                                                    "yyyyMMdd",
                                                    CultureInfo.InvariantCulture,
                                                    DateTimeStyles.None);
            }
            //Uploader.GetFileStream(id);
            string result = "";
            Uploader uploader = new Uploader();
            var filestrings = id.Split('_');
            string documentId = filestrings[1];
            var tenant = Convert.ToInt32(filestrings[0]);

            bool overrideSecDueIsConnectedToUniFreight = false;
            if (LogitudeSettings.IsCostomsDeploy)
            {
                LogitudeSettings.HandleLogMe(id + "LogitudeSettings.IsCostomsDeploy " + overrideSecDueIsConnectedToUniFreight, false, "GetLast2755ResponseDataAsFileStream", stopLogAt);

                var setting = CustomsSettingQueryService.GetSettingByTenant(tenant) ?? new CustomsSettingPM();
                overrideSecDueIsConnectedToUniFreight = setting.IsConnectedToUniFreight;
                if (!overrideSecDueIsConnectedToUniFreight)//semi a like Connected  == not cloud !!
                {
                    LogitudeSettings.HandleLogMe(id + "!overrideSecDueIsConnectedToUniFreight " + overrideSecDueIsConnectedToUniFreight, false, "GetLast2755ResponseDataAsFileStream", stopLogAt);

                    if (//!String.IsNullOrWhiteSpace( setting.UnfConnectionString)  && 
                        !String.IsNullOrWhiteSpace(setting.OnPremiseFillingService))
                    {
                        LogitudeSettings.HandleLogMe(id + "!String.IsNullOrWhiteSpace(setting.OnPremiseFillingService)) " + overrideSecDueIsConnectedToUniFreight, false, "GetLast2755ResponseDataAsFileStream", stopLogAt);

                        overrideSecDueIsConnectedToUniFreight = true;
                    }

                }
            }
            LogitudeSettings.HandleLogMe(id + "overrideSecDueIsConnectedToUniFreight " + overrideSecDueIsConnectedToUniFreight, false, "GetLast2755ResponseDataAsFileStream", stopLogAt);

            if (!overrideSecDueIsConnectedToUniFreight)
            {
                throw new Exception("using File Stream only @ onpremise");
            }
            string documentExtension = uploader.GetFileExtension(documentId, tenant);

            byte[] data = null;// uploader.DownloadFile(documentId, documentExtension, "", tenant);
            string fileName = null;// documentId + "." + documentExtension;

            try
            {
                LogitudeSettings.HandleLogMe(id + "before DownloadFile", false, "GetLast2755ResponseDataAsFileStream", stopLogAt);

                data = uploader.DownloadFile(documentId, documentExtension, "", tenant);
                fileName = documentId + "." + documentExtension;
                LogitudeSettings.HandleLogMe(id + "after DownloadFile" + fileName, false, "GetLast2755ResponseDataAsFileStream", stopLogAt);

            }
            catch (ExceptionInErrorLog ee)
            {

                XElement myXml =
new XElement("FileStreamError",
new XElement("Error", ee.ToString()

   )
);
                data = System.Text.UTF8Encoding.UTF8.GetBytes(myXml.ToString());
                fileName = documentId + ".xml";
            }
            catch (Exception ex)
            {
                LogitudeSettings.HandleLogMe(id + "catch (Exception)" + ex.Message.ToString(), false, "GetLast2755ResponseDataAsFileStream", stopLogAt);

                throw;
            }
            LogitudeSettings.HandleLogMe(id + "suscsess ", false, "GetLast2755ResponseDataAsFileStream", stopLogAt);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            using (MemoryStream dataMemoryStream = new MemoryStream(data))
            {
                response.Content = new StreamContent(dataMemoryStream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = fileName;
                return response;
            }


        }
    }
}