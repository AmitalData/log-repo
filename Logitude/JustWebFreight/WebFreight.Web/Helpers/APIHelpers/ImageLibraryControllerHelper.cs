using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Web;
using WebFreight.Web.WebServices;
using System.Web.Http;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class ImageLibraryControllerHelper
    {
        public string UploadImage(ImageParameter filter)
        {
            Uploader uploaderService = new Uploader();


            filter.FileData = Convert.FromBase64String(filter.Base64String);
            filter.Base64String = "";


            if (filter.FileName != "verysmalllogo" && filter.FileName != "sharedLogtsitcslogo" && !filter.KeepOriginalSize)
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
                result = uploaderService.UploadFile(filter.FileName, filter.buffer, filter.FileSize, filter.SentSize, blockIdlist, filter.BufferNumber, null, filter.Tenant, "images", null, false, null);
            }
            #region Company Logos
            else if (filter.UploadMode == "CompanyLogos" || string.IsNullOrEmpty(filter.UploadMode))
            {
                result = uploaderService.UploadFile(filter.FileName, filter.buffer, filter.FileSize, filter.SentSize, blockIdlist, filter.BufferNumber, null, filter.Tenant, "logos", null, false, null);
            }
            return result;
            #endregion

        }

        public ImageParameter UploadAttachementOrChunk(ImageParameter filter)
        {
            #region  Attachment Uploader
            Uploader uploaderService = new Uploader();
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
            string documentId = GetDocumentId(filter.Result);
            filter.Result = uploaderService.UploadFile(filter.EncodedFileName + "." + filter.Extension, filter.buffer, filter.FileSize, filter.SentSize, filter.BlockIdsList.ToArray(), filter.BufferNumber, filter.EntityId, filter.Tenant, filter.FileLocation, filter.FileName, filter.ForceCreateDocument, documentId);
            filter.buffer = null;
            return filter;

            #endregion
        }

        private string GetDocumentId(string result)
        {
            if (string.IsNullOrEmpty(result)) return null;
            return result.Split('.')[0];
        }

        public byte[] ResizeImage(byte[] image, int width, int height, string extension)
        {
            using (var stream = new System.IO.MemoryStream(image))
            {
                var img = Image.FromStream(stream);
                Image thumbnail = null;
                //if (uploadName == "ImageComponent")
                //{
                //    thumbnail = FixedSize2(img, width, height);

                //}
                thumbnail = FixedSize(img, width, height, extension);



                img.Dispose();
                img = null;
                System.Drawing.Imaging.EncoderParameters param = new System.Drawing.Imaging.EncoderParameters(1);
                ImageCodecInfo myImageCodecInfo;

                var Quality = 90L;
                param.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Quality);

                // var thumbnail = img.GetThumbnailImage(tw, th, () => false, IntPtr.Zero);

                using (var thumbStream = new System.IO.MemoryStream())
                {
                    if (extension == "jpg" || extension == "jpeg")
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

        public Image FixedSize(Image imgPhoto, int width, int height, string extension)
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

        public ImageCodecInfo GetEncoderInfo(String mimeType)
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
    }
}