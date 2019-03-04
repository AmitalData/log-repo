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

namespace WebFreight.Web.App_Code
{

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

        public HttpResponseMessage GetDownloadFile(string filename, string documentExtension, string fileLocation, string type, int tenant)
        {
            string result = "";

            try
            {
                string token = System.Web.HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

           
                if (tenant != authToken.Tenant)
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

        private byte[] ResizeImage(byte[] image, int width, int height, string extension)
        {
            using (var stream = new System.IO.MemoryStream(image))
            {
                var img = Image.FromStream(stream);
                Image thumbnail = null;
                //if (uploadName == "ImageComponent")
                //{
                //    thumbnail = FixedSize2(img, width, height);

                //}
                 thumbnail = FixedSize(img, width, height , extension);

           

                img.Dispose();
                img = null;
                System.Drawing.Imaging.EncoderParameters param = new System.Drawing.Imaging.EncoderParameters(1);
                ImageCodecInfo myImageCodecInfo;

                var Quality = 90L;
                param.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Quality);

                // var thumbnail = img.GetThumbnailImage(tw, th, () => false, IntPtr.Zero);

                using (var thumbStream = new System.IO.MemoryStream())
                {
                    if (extension == "jpg" || extension=="jpeg")
                    {
                        myImageCodecInfo = GetEncoderInfo("image/jpeg");
                        thumbnail.Save(thumbStream, myImageCodecInfo, param); //thumbnail.Save(thumbStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    else if (extension == "png")
                    {
                        myImageCodecInfo = GetEncoderInfo("image/png");
                        thumbnail.Save(thumbStream, myImageCodecInfo, param);
          
                    }
                    myImageCodecInfo = null;
                    thumbnail.Dispose();
                    thumbnail = null;
                    return thumbStream.GetBuffer();
                    thumbStream.Dispose();
           
                }
            }

        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        [ActionName("PostUploadFile")]
        public HttpResponseMessage PostUploadFile(ImageParameter filter)
        {

            try
            {

                string token = System.Web.HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
             

                if (filter.Tenant != authToken.Tenant)
                {
                    throw new Exception("Sorry you’re not authenticated to upload file");
                }

                Uploader uploaderService = new Uploader();
               
                if (filter.UploadMode == "AttachmentUploader" || filter.UploadMode == "Chunk")
                {
                    #region  Attachment Uploader

                    filter.BufferNumber++;
                    if (string.IsNullOrEmpty(filter.EncodedFileName)) filter.EncodedFileName = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 10).Replace('/', 'A').ToLower();
                    if (!string.IsNullOrEmpty(filter.Base64String))
                    {
                        filter.buffer = Convert.FromBase64String(filter.Base64String);
                        filter.Buffersize = filter.buffer.Length;
                    }
                    var blockId = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

                    if (filter.BlockIdsList != null && filter.BlockIdsList.Count() > 0) filter.BlockIdsList.Add(blockId);
                    else
                    {
                        filter.BlockIdsList = new List<string>();
                        filter.BlockIdsList.Add(Convert.ToBase64String(Guid.NewGuid().ToByteArray()));
                    }
                    filter.SentSize += filter.buffer.Length;
                    if (filter.IsFirstTry)
                    {
                        filter.IsFirstTry = false;
                        filter.BlocksNumber = Math.Ceiling(Convert.ToDouble(filter.FileSize) / filter.Buffersize);
                    }
                    filter.Result = uploaderService.UploadFile(filter.EncodedFileName + "." + filter.Extension, filter.buffer, filter.FileSize, filter.SentSize, filter.BlockIdsList.ToArray(), filter.BufferNumber, filter.EntityId, filter.Tenant, "", filter.FileName);
                    filter.buffer = null;
                    return Request.CreateResponse(HttpStatusCode.OK, filter);

                    #endregion

                }

                else
                {
                    filter.FileData = Convert.FromBase64String(filter.Base64String);
                    filter.Base64String = "";


                    if (filter.FileName != "verysmalllogo" && filter.FileName != "sharedLogtsitcslogo")
                    {
                        filter.FileData = ResizeImage(filter.FileData, filter.Width, filter.Height, filter.Extension);

                    }
                    filter.buffer = filter.FileData;
                    filter.SentSize = filter.FileData.Length;
                    filter.FileSize = filter.FileData.Length;
                    string result = "";
                    string[] blockIdlist = { Convert.ToBase64String(Guid.NewGuid().ToByteArray()) };

                    #region Image Component
                    if (filter.UploadMode == "ImageComponent" || filter.UploadMode == "Image")
                    {
                        result = uploaderService.UploadImage(filter.FileName, filter.buffer, filter.FileSize, filter.SentSize, blockIdlist, filter.BufferNumber, filter.Tenant, filter.Extension, filter.EntityId, filter.ContactId, filter.Key);
                    }
                    #endregion

                    else if (filter.UploadMode == "HybridPartner")
                    {
                        ImageDetail imagedetail = null;
                        string imagedetailid = null;
                        ImageDetailRepository imageDetailRep = new ImageDetailRepository(filter.Tenant);
                        if (!string.IsNullOrEmpty(imagedetailid))
                        {
                            imagedetail = imageDetailRep.GetSingleImageDetail(imagedetailid, filter.Tenant);
                        }
                        if (imagedetail == null)
                        {
                            imagedetail = new ImageDetail() { Id = IdCounter.GetNumber("ImageDetail", filter.Tenant), Tenant = filter.Tenant, Extension = filter.Extension, Size = filter.FileSize };
                            imageDetailRep.Add(imagedetail);
                            imageDetailRep.SubmitChanges();
                        }
                        imagedetailid = imagedetail.Id;
                        result = uploaderService.UploadFile(filter.FileName, filter.buffer, filter.FileSize, filter.SentSize, blockIdlist, filter.BufferNumber, null, filter.Tenant, "images", null);
                    }
                    #region Company Logos
                    else if (filter.UploadMode == "CompanyLogos" || string.IsNullOrEmpty(filter.UploadMode))
                    {
                        result = uploaderService.UploadFile(filter.FileName, filter.buffer, filter.FileSize, filter.SentSize, blockIdlist, filter.BufferNumber, null, filter.Tenant, "logos", null);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                    #endregion
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
            ImageParameter.FileData = Convert.FromBase64String(ImageParameter.Base64String);
            ImageParameter.Base64String = "";
            ImageParameter.FileData = ResizeImage(ImageParameter.FileData, ImageParameter.Width, ImageParameter.Height, ImageParameter.Extension);
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
                filter.FileData = ResizeImage(filter.FileData, filter.Width, filter.Height, filter.Extension);
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
                    result.Name = uploaderService.UploadPdfFile(filter.FileName, filter.buffer, filter.FileSize, filter.SentSize, blockIdlist, filter.BufferNumber, filter.EntityId, filter.Tenant, "", (filter.FileName +"." + filter.Extension),ref isDigitallySigned, ref signersList);
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
            string token = System.Web.HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

            Uploader uploaderService = new Uploader();
            uploaderService.CancelUpload(documentId, "", tenant);

            return Request.CreateResponse(HttpStatusCode.OK, "");

        }


        static Image FixedSize(Image imgPhoto, int width, int height, string extension)
        {

            try
            {
                int sourceWidth = imgPhoto.Width;
                int sourceHeight = imgPhoto.Height;
                int sourceX = 0;
                int sourceY = 0;
                int destX = 0;
                int destY = 0;

                float nPercent = 0;
                float nPercentW = 1;
                float nPercentH = 1;



                if (sourceHeight > height)
                {
                    nPercentH = ((float)height / (float)sourceHeight);
                }
                if (sourceWidth > width)
                {
                    nPercentW = ((float)width / (float)sourceWidth);
                }


                // nPercentW = ((float)Width / (float)sourceWidth);
                // nPercentH = ((float)Height / (float)sourceHeight);
                if (nPercentH != 1 && nPercentW != 1)
                {
                    if (nPercentH < nPercentW)
                    {
                        nPercent = nPercentH;
                        destX = System.Convert.ToInt16((width -
                                      (sourceWidth * nPercent)) / 2);
                    }
                    else
                    {
                        nPercent = nPercentW;
                        destY = System.Convert.ToInt16((height -
                                      (sourceHeight * nPercent)) / 2);
                    }
                }
                else
                {

                    nPercent = nPercentH;
                    destX = System.Convert.ToInt16((width -
                                  (sourceWidth * nPercent)) / 2);

                    nPercent = nPercentW;
                    destY = System.Convert.ToInt16((height -
                                  (sourceHeight * nPercent)) / 2);
                }



                if (destX < 0) destX = 0;
                if (destY < 0) destY = 0;
                int destWidth = (int)(sourceWidth * nPercent);
                int destHeight = (int)(sourceHeight * nPercent);

                Bitmap bmPhoto = new Bitmap(width, height,
                                  PixelFormat.Format24bppRgb);
                bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                                 imgPhoto.VerticalResolution);

                Graphics grPhoto = Graphics.FromImage(bmPhoto);

                grPhoto.Clear(Color.White);

                grPhoto.InterpolationMode =
                        InterpolationMode.HighQualityBicubic;

                grPhoto.DrawImage(imgPhoto,
                    new Rectangle(destX, destY, destWidth, destHeight),
                    new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                    GraphicsUnit.Pixel);

       

                grPhoto.Dispose();

                if (!string.IsNullOrEmpty(extension) && extension.ToLower() == "png")
                {
                    bmPhoto.MakeTransparent();
                }

                return bmPhoto;
            }
            catch (Exception ex)
            {
                return imgPhoto;
            }
        }

        public HttpResponseMessage GetRemoveFile(string documentId, int tenant)
        {
            try
            {
                string token = System.Web.HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                if (tenant  != authToken.Tenant)
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