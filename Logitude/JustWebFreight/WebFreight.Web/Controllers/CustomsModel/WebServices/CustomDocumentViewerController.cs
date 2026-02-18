using Logitude.BL.Security;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Xml.Serialization;
using WebFreight.Web.Helpers;
using WebFreight.Web.WebServices;
using System.Text;
using ICSharpCode.SharpZipLib.Tar;
using iTextSharp.text.pdf.qrcode;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    public class CustomDocumentViewerController : ApiController
    {

        [HttpGet]
        public HttpResponseMessage GetDocumentPageAsPdf(string documentId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                // todo: currently not implemented
                // SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                var customsDocumentQueryService = new CustomsDocumentQueryService(customContext);
                var attachment = customsDocumentQueryService.GetAttachment(documentId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, attachment);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDocumentPage(string documentId, int currPage, bool isConnectedToUni, int? angle=0)
        {
            string TiffPageLines;
            string ErrorMessage;
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                var pm = CustomsSettingQueryService.GetSettingByTenant(tenant);
                

                var pageObj = new CustomDocumentPageObject();
                if (isConnectedToUni || !String.IsNullOrWhiteSpace(pm.OnPremiseFillingService))
                {
                    currPage = currPage + 1;

                    Uploader up = new Uploader();
                    byte[] imageBytes = null;
                    imageBytes = up.GetPageTiffAsB64FromTarByTenantComIdPage(documentId, tenant, currPage, out TiffPageLines, out ErrorMessage);
					if (imageBytes != null)
                    {
                        Bitmap bmp = GetBitmap(imageBytes);
                        RotateBitmap(bmp,angle);

                        byte[] newBytes = GetImageBytes(bmp);
                        pageObj.Page = Resize(new MemoryStream(newBytes));
                        pageObj.TiffPageLines = TiffPageLines;
                        pageObj.ErrorMessage = ErrorMessage;

                        var TiffPages = new List<string>(TiffPageLines.Split(new char[] { '\n' }));
                        pageObj.Count = TiffPages.Count - 1;
                    }
					pageObj.ErrorMessage = ErrorMessage;

				}
				else
                {
                    currPage = currPage + 1;
                    Uploader up = new Uploader();
                    byte[] tarBytes = null;
                    var externalDocumentRepository = new DocumentsFilingRepository(tenant);
                    DocumentRepository documentRepository = new DocumentRepository(tenant);
                    var pdfDocumentFillingId = externalDocumentRepository.GetSingleDocumentsFilingIdByDocumentId(documentId, tenant);
                    if (pdfDocumentFillingId != null)
                    {
                        var TarDocumentId = documentRepository.GetDocumentIdByFileName(pdfDocumentFillingId, "tar",tenant);
                        if (TarDocumentId != null)
                        {
                            string documentExtension = up.GetFileExtension(TarDocumentId, tenant);
                            var poco = externalDocumentRepository.GetSingleDocumentsFilingIdByDocumentId(TarDocumentId, tenant);
                            {
                                tarBytes = up.DownloadFile(TarDocumentId, documentExtension, "", tenant);
                            }
                            var byteArr = this.GetTiffPageFromTar(tarBytes, currPage, out string TiffList);

                            if (tarBytes != null && byteArr != null)
                            {
                                //get multi pages tiff count
                                Bitmap bmp = GetBitmap(byteArr);
                                RotateBitmap(bmp, angle);
                                byte[] newBytes = GetImageBytes(bmp);
                                pageObj.Count = bmp.GetFrameCount(FrameDimension.Page);
                                pageObj.Page = Resize(new MemoryStream(newBytes));
                                pageObj.TiffPageLines = TiffList;
                                var TiffPages = new List<string>(TiffList.Split(new char[] { '\n' }));
                                pageObj.Count = TiffPages.Count - 1;
                            }
                        }
                        else
                        {
                            ErrorMessage = "document is not found by filename";
                            pageObj.ErrorMessage = ErrorMessage;
                        }
                    }
                    else
                    {
                        ErrorMessage = "document is not found";
                        pageObj.ErrorMessage = ErrorMessage;
                    }

                }

                return Request.CreateResponse(HttpStatusCode.OK, pageObj);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public byte[] GetTiffPageFromTar(byte[] tarBytes, int pageRequest, out string TiffList)
        {
            StringBuilder sbTiffList = new StringBuilder();
            byte[] byteArr = null;

            try
            {
                using (MemoryStream tarMemoryStream = new MemoryStream(tarBytes))
                using (TarInputStream tarIn = new TarInputStream(tarMemoryStream))
                {
                    TarEntry tarEntry;
                    int i = 1;

                
                    while ((tarEntry = tarIn.GetNextEntry()) != null)
                    {
                   
                        if (tarEntry.IsDirectory)
                        {
                            continue;
                        }
                        string name = tarEntry.Name.Replace('/', Path.DirectorySeparatorChar);

                        if (Path.IsPathRooted(name))
                        {
                            name = name.Substring(Path.GetPathRoot(name).Length);
                        }
                        var fi = new FileInfo(name);
                        if (fi.Extension.Equals(".tiff", StringComparison.OrdinalIgnoreCase) || fi.Extension.Equals(".tif", StringComparison.OrdinalIgnoreCase))
                        {
                            sbTiffList.AppendLine(fi.Name);
                            if (pageRequest == i)
                            {
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    tarIn.CopyEntryContents(ms); 
                                    byteArr = ms.ToArray();
                                }
                            }

                            i++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while processing TAR file: {ex.Message}");
                byteArr = null;  // Ensure byteArr is null if an error occurs
            }

            TiffList = sbTiffList.ToString();
            return byteArr;
        }

        private static Bitmap RotateBitmap(Bitmap bmp,int? angle)
        {
            switch (angle)
            {
                case 90:
                    bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case 180:
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case 270:
                    bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                default:
                    bmp.RotateFlip(RotateFlipType.RotateNoneFlipNone);
                    break;
            }
            return bmp;
        }

        public byte[] GetImageBytes(Bitmap img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
        public Bitmap GetBitmap(byte [] imageBytes)
        {
            MemoryStream st = new MemoryStream(imageBytes);
            Bitmap bmp = (Bitmap)Image.FromStream(st);
            return bmp;
        }

        // Service Methods:
        private byte[] GetPageTiffAsB64FromTarByTenantComIdPage(string documentId, int tenant, int currPage, out string TiffPageLines, out string ErrorMessage)
        {
            Uploader up = new Uploader();
            byte[] _DatainByte = null;
            _DatainByte = up.GetPageTiffAsB64FromTarByTenantComIdPage(documentId, tenant, currPage, out TiffPageLines, out ErrorMessage);
            if (_DatainByte != null)
            {
                return BinaryImageToSerializeListBytes(_DatainByte, currPage);
            }

            return null;
        }
        public byte[] Resize(Stream stream)
        {
            double scaleFactor = 1.0;
            double width = -1;
            double height = -1;
            bool isLandScape = false;
            MemoryStream streamPNG = new MemoryStream();
            using (var srcImage = Image.FromStream(stream))
            {

                if (srcImage.Width > srcImage.Height)
                {
                    isLandScape = true;
                }
                var maxSize = Math.Max(srcImage.Width, srcImage.Height);
                width = srcImage.Width;
                height = srcImage.Height;
                if (isLandScape)
                {
                    if (srcImage.Width > 3508)
                    {
                        scaleFactor = (double)3508 / srcImage.Width;
                    }
                }
                else
                {
                    if (srcImage.Height > 3508)
                    {
                        scaleFactor = (double)3508 / srcImage.Height;
                    }

                }

                if (scaleFactor != 1 && (scaleFactor < .6 || scaleFactor > 1.4) )
                {
                    var newWidth = (int)(srcImage.Width * scaleFactor);
                    var newHeight = (int)(srcImage.Height * scaleFactor);

                    Image destImage = ResizeImage(srcImage, newWidth, newHeight);

                    destImage.Save(streamPNG, ImageFormat.Png);
                    return streamPNG.ToArray();
                }
                else
                {

                    srcImage.Save(streamPNG, ImageFormat.Png);
                    return streamPNG.ToArray();
                }

            }


        }
        public Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var resizeImage = new Bitmap(width, height);

            resizeImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(resizeImage))
            {
                //graphics.CompositingMode = CompositingMode.SourceCopy;
                //graphics.CompositingQuality = CompositingQuality.HighQuality;
                //graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //graphics.SmoothingMode = SmoothingMode.HighQuality;
                //graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                    //wrapMode.Dispose();
                }
                //graphics.Dispose();
            }
            image.Dispose();
            image = null;
            //return destImage;  // Memory Leak...
            var destImage = resizeImage.Clone();
            resizeImage.Dispose();
            return ((Bitmap)destImage);
        }
        private byte[] BinaryImageToSerializeListBytes(byte[] _DatainByte, int index)
        {
            MemoryStream st = new MemoryStream(_DatainByte);
            List<byte[]> images = new List<byte[]>();

            System.Drawing.Bitmap bmp = (Bitmap)Image.FromStream(st);
            int count = bmp.GetFrameCount(FrameDimension.Page);
            for (int idx = 0; idx < count; idx++)
            {

                bmp.SelectActiveFrame(FrameDimension.Page, idx);
                MemoryStream byteStream = new MemoryStream();

                bmp.Save(byteStream, ImageFormat.Tiff);
                Bitmap imageBit = new Bitmap(byteStream);
                MemoryStream imageSt = new MemoryStream();
                imageBit.Save(imageSt, ImageFormat.Png);
                images.Add(imageSt.ToArray());

            }


            XmlSerializer serializer = new XmlSerializer(typeof(List<byte[]>));
            MemoryStream memstream = new MemoryStream();

            byte[] image = images[index]; // returns current page image

            return image;
        }
   

    }


    class CustomDocumentPageObject
    {
        public byte[] Page;
        public string TiffPageLines;
        public string ErrorMessage;
        public int Count;
    }
}