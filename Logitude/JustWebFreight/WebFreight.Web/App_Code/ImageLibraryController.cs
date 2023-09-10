using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
//using System.Web.Http.Cors;
using WebFreight.Web.Helpers;
using WebFreight.Web.WebServices;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Security;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.Helpers.APIHelpers;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace WebFreight.Web.App_Code
{
#if DEBUG
    /// <summary>
    /// debug classes in App_Code !!!! WEB.<compilation 
    /// </summary>

#endif
    //[EnableCors(origins: "http://localhost:9996", headers: "*", methods: "*")]
    public class ImageLibraryController : ApiController
    {
        public HttpResponseMessage GetLinkImages(int tenant)
        {
            List<LinkImage> LinkImages = new List<LinkImage>();
            //ImageLibraryRepository imageLibraryRepository = new ImageLibraryRepository(tenant);
            //List<ImageLibrary> ImageLibraryLists = imageLibraryRepository.GetImageLibraryListsByTenant(tenant);
            //foreach (ImageLibrary item in ImageLibraryLists)
            //{
            //    LinkImages.Add(new LinkImage()
            //    {

            //        thumb = url + "WebPages/DownLoadImage.aspx?imageId=" + item.Id + "&tenant=" + item.Tenant.ToString(),
            //        url = url + "WebPages/DownLoadImage.aspx?imageId=" + item.Id + "&tenant=" + item.Tenant.ToString(),
            //        tag = "flower"
            //    });



            //}

            LinkImages.Add(new LinkImage()
            {
                thumb = "https://www.froala.com/assets/editor/media_files/photo9.jpg",
                url = "https://www.froala.com/assets/editor/media_files/photo9.jpg",
            });
            return Request.CreateResponse(HttpStatusCode.OK, LinkImages);


        }

        public HttpResponseMessage GetDownloadFile(string filename, string documentExtension, string fileLocation, string type, int tenant,int tokenTenant)
        {
            string result = "";

			try
            {
                string token = System.Web.HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tokenTenant);


                if (tokenTenant != authToken.Tenant)
                {
                    throw new Exception("Sorry you’re not authenticated");
                }
                if (!string.IsNullOrEmpty(type) && type.Contains('^'))
                {
                    var imageType = type.Split('^')[1];
                    type = type.Split('^')[0];

                    if (imageType == "ImageDetail")
                    {
                        ImageDetailRepository imageDetailRepository = new ImageDetailRepository(tenant);
                        string extension = imageDetailRepository.GetImageExtensionbyId(tenant, filename);
                        if (!string.IsNullOrEmpty(extension)) documentExtension = extension;
                    }
                }

                Uploader uploaderService = new Uploader();
                byte[] filedata = uploaderService.DownloadFile(filename, documentExtension, fileLocation, tenant);

                if (filedata != null)
                {
                    if (filename.Contains("logo") || type == "Base64")
                    {
                        result = "data:image/" + documentExtension + ";base64," + Convert.ToBase64String(filedata);
                    }
                    else result = UTF8Encoding.UTF8.GetString(filedata, 0, filedata.Length);
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }

        [ActionName("PostUploadFile")]
        public HttpResponseMessage PostUploadFile(ImageParameter filter)
        {
            try
            {
                string token = System.Web.HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("ImageParameter", filter.TokenTenant, authToken.Tenant);

                ImageLibraryControllerHelper imageLibraryControllerHelper = new ImageLibraryControllerHelper();
                if (filter.UploadMode == "AttachmentUploader" || filter.UploadMode == "Chunk")
                {
                    ImageParameter _filter = imageLibraryControllerHelper.UploadAttachementOrChunk(filter);
                    return Request.CreateResponse(HttpStatusCode.OK, _filter);
                }
                else
                {
                    string result = imageLibraryControllerHelper.UploadImage(filter);
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [ActionName("PostImageAfterResize")]
        public HttpResponseMessage PostImageAfterResize(ImageParameter ImageParameter)
        {
            string token = System.Web.HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnEntityTenant("ImageParameter", ImageParameter.Tenant, authToken.Tenant);

            ImageLibraryControllerHelper imageLibraryControllerHelper = new ImageLibraryControllerHelper();
            ImageParameter.FileData = Convert.FromBase64String(ImageParameter.Base64String);
            ImageParameter.Base64String = "";
            if (!ImageParameter.KeepOriginalSize) ImageParameter.FileData = imageLibraryControllerHelper.ResizeImage(ImageParameter.FileData, ImageParameter.Width, ImageParameter.Height, ImageParameter.Extension);
            ImageParameter.Base64String = Convert.ToBase64String(ImageParameter.FileData);
            return Request.CreateResponse(HttpStatusCode.OK, ImageParameter);
        }

        [ActionName("PostUploadPdfFile")]
        public HttpResponseMessage PostUploadPdfFile(ImageParameter filter)
        {
            try
            {
                string token = System.Web.HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("ImageParameter", filter.Tenant, authToken.Tenant);

                ImageLibraryControllerHelper imageLibraryControllerHelper = new ImageLibraryControllerHelper();

                if (filter.Tenant != authToken.Tenant)
                {
                    throw new Exception("Sorry you’re not authenticated to upload file");
                }

                Uploader uploaderService = new Uploader();
                UploadPDFFinalResult result = new UploadPDFFinalResult();
                bool isDigitallySigned = false;
                string signersList = "";
                if (string.IsNullOrEmpty(filter.UploadMode) || filter.UploadMode != "Block")
                {

                    filter.FileData = Convert.FromBase64String(filter.Base64String);
                    filter.Base64String = "";
                    filter.FileData = imageLibraryControllerHelper.ResizeImage(filter.FileData, filter.Width, filter.Height, filter.Extension);
                    filter.buffer = filter.FileData;
                    filter.SentSize = filter.FileData.Length;
                    filter.FileSize = filter.FileData.Length;


                    string[] blockIdlist = { Convert.ToBase64String(Guid.NewGuid().ToByteArray()) };

                    //if (filter.UploadMode == "Image")
                    //{
                    //    result.Name = uploaderService.UploadImage(filter.FileName, filter.buffer, filter.FileSize, filter.SentSize, blockIdlist, filter.BufferNumber, filter.Tenant, filter.Extension, filter.EntityId, filter.ContactId, filter.Key);
                    //}
                    //else
                    //{
                    result.Name = uploaderService.UploadPdfFile(filter.FileName, filter.buffer, filter.FileSize, filter.SentSize, blockIdlist, filter.BufferNumber, filter.EntityId, filter.Tenant, "", (filter.FileName + "." + filter.Extension), ref isDigitallySigned, ref signersList);
                    //}
                    result.isDigitallySigned = isDigitallySigned;
                    result.signersList = signersList;

                    return Request.CreateResponse(HttpStatusCode.OK, result);

                }

                else
                {
                    filter.BufferNumber++;
                    if (string.IsNullOrEmpty(filter.EncodedFileName))
                    {
                        filter.EncodedFileName = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 10).Replace('/', 'A').ToLower();
                    }
                    if (!string.IsNullOrEmpty(filter.Base64String))
                    {
                        filter.buffer = Convert.FromBase64String(filter.Base64String);
                        filter.Buffersize = filter.buffer.Length;
                    }



                    //int byteDifference2 = filter.FileData.Length - Convert.ToInt32(filter.SentSize);
                    //filter.FileSize = filter.FileData.Length;
                    //if (byteDifference2 > filter.Buffersize)
                    //{
                    //    filter.buffer = new byte[filter.Buffersize];
                    //    Buffer.BlockCopy(filter.FileData, filter.Position, filter.buffer, 0, filter.Buffersize);
                    //}
                    //else
                    //{
                    //    filter.buffer = new byte[byteDifference2];
                    //    Buffer.BlockCopy(filter.FileData, filter.Position, filter.buffer, 0, byteDifference2);
                    //}


                    var blockId2 = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

                    if (filter.BlockIdsList != null && filter.BlockIdsList.Count() > 0)
                    {
                        filter.BlockIdsList.Add(blockId2);
                    }
                    else
                    {

                        filter.BlockIdsList = new List<string>();
                        filter.BlockIdsList.Add(Convert.ToBase64String(Guid.NewGuid().ToByteArray()));

                    }

                    filter.SentSize += filter.buffer.Length;
                    filter.Position = Convert.ToInt32(filter.SentSize);

                    var value = (Convert.ToDouble(!filter.IsFirstTry ? filter.SentSize : 0) / Convert.ToDouble(filter.FileSize)) * 100;
                    if (filter.IsFirstTry)
                    {
                        filter.IsFirstTry = false;
                        filter.BlocksNumber = Math.Ceiling(Convert.ToDouble(filter.FileSize) / filter.Buffersize);
                    }
                    filter.Result = uploaderService.UploadPdfFile(filter.FileName, filter.buffer, filter.FileSize, filter.SentSize, filter.BlockIdsList.ToArray(), filter.BufferNumber, filter.EntityId, filter.Tenant, "", (filter.FileName + "." + filter.Extension), ref isDigitallySigned, ref signersList);
                    //uploaderService.UploadFile(filter.EncodedFileName + "." + filter.Extension, filter.buffer, filter.FileSize, filter.SentSize, filter.BlockIdsList.ToArray(), filter.BufferNumber, filter.EntityId, filter.Tenant, "", filter.FileName);
                    filter.buffer = null;
                    filter.Position = 0;
                    if (string.IsNullOrEmpty(filter.Result))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, filter);
                    }
                    else
                    {
                        result.Name = filter.Result;
                        result.isDigitallySigned = isDigitallySigned;
                        result.signersList = signersList;
                        result.FileSize = filter.FileSize;
                        result.SentSize = filter.SentSize;
                        result.BlocksNumber = filter.BlocksNumber;
                        result.BufferNumber = filter.BufferNumber;
                        return Request.CreateResponse(HttpStatusCode.OK, result);
                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetCancelUpload(string documentId, int tenant)
        {
            try
            {
                string token = System.Web.HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);

                Uploader uploaderService = new Uploader();
                uploaderService.CancelUpload(documentId, "", tenant);

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "");

            }


        }

        public HttpResponseMessage GetRemoveFile(string documentId, int tenant)
        {
            try
            {
                string token = System.Web.HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                if (tenant != authToken.Tenant)
                {
                    throw new Exception("Sorry you’re not authenticated to remove file");
                }

                Uploader uploaderService = new Uploader();
                uploaderService.RemoveFile(documentId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            }

        }

        public HttpResponseMessage GetImageUrl(string name)
        {

            return Request.CreateResponse(HttpStatusCode.OK, "https://www.froala.com/assets/editor/media_files/photo9.jpg");

        }

        [ActionName("PostUploadFileFromCTool")]
        public HttpResponseMessage PostUploadFileFromCTool(ImageParameter filter)
        {

            try
            {

                string token = System.Web.HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("ImageParameter", filter.Tenant, authToken.Tenant);

                ShipmentQuery shipmentQuery = new ShipmentQuery(authToken.Tenant);
                DocumentsFilingPM entityPM = new DocumentsFilingPM();
                string entityId = shipmentQuery.GetEntitiyIdByShipmentNumber(filter.ShipmentNumber, authToken.Tenant);
                string shipmentObjectTableId = ObjectTableRepository.GetObjectTableByName("Shipment");


                if (filter.SentSize == 0)
                {
                   
                    entityPM.DocumentTypeId = filter.DocumentTypeId;
                    entityPM.EntityId = entityId;
                    entityPM.Tenant = authToken.Tenant;
                    entityPM.ObjectTableId = shipmentObjectTableId;
                    entityPM.DirectionCode = "I";
                    ICommonDataContext MyContext = CommonDataContext.GetContext(entityPM.Tenant);
                    DocumentsFilingService service = new DocumentsFilingService(MyContext, entityPM.Tenant);
                    service.Create(entityPM);
                }
                else
                {
                    DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(authToken.Tenant);
                    entityPM = documentsFilingQuery.GetDocumentsFilingPMByEntityIdAndObjectTableIdAndDocumentTypeId(entityId, shipmentObjectTableId,filter.DocumentTypeId,authToken.Tenant);
                }
                filter.EntityId = entityPM.Id;
                ImageLibraryControllerHelper imageLibraryControllerHelper = new ImageLibraryControllerHelper();
                if (filter.UploadMode == "AttachmentUploader" || filter.UploadMode == "Chunk")
                {
                    ImageParameter _filter = imageLibraryControllerHelper.UploadAttachementOrChunk(filter);
                    return Request.CreateResponse(HttpStatusCode.OK, _filter);
                }
                else
                {
                    string result = imageLibraryControllerHelper.UploadImage(filter);
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        //[ActionName("PostUploadPdfFile")]
        //public HttpResponseMessage PostUploadImage(ImageParameter filter)
        //{
        //    Uploader uploaderService = new Uploader(); 
        //    var temp = uploaderService.UploadImage(filter.FileName, filter.buffer, filter.FileSize, filter.SentSize, filter.BlockIdsList.ToArray(),filter.BufferNumber,filter.Tenant, "jpg", null, null, filter.EntityId);

        //}

    }

    public class UploadPDFFinalResult
    {
        public string Name { get; set; }
        public bool isDigitallySigned { get; set; }
        public string signersList { get; set; }
        public int SentSize { get; set; }
        public int FileSize { get; set; }
        public double BlocksNumber { get; set; }
        public int BufferNumber { get; set; }
    }
}