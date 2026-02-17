using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Net.Http; 
using System.Web.Http;

namespace WebFreight.Web.App_Code
{
    public class BrandingController : ApiController
    {
        public string GetTenantLogoUri(int tenant)
        {
            try
            {

                string result = "";
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = "logo" + tenant,
                    FolderName = "logos",
                    Extension = "jpg",
                    Tenant = tenant,
                };

                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                byte[] datainByte = storageservice.Read(fileInfo);

                
                if (datainByte != null)
                {
                    int height = LogitudeSettings.WorkEnvironment != "cloud" && tenant == 1245 ? 170: 114;
                 
                    datainByte = ResizeImage(datainByte, 290, height, "jpg");
                    string base64String = System.Convert.ToBase64String(datainByte, 0, datainByte.Length);
                    result = "data:image/jpg;base64," + base64String;
                    return result;
                }

                else return null;


            }

            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "BrandingController : GetTenantLogoUri Method", null);
                return null;
            }


        }


        public BrandingResult GetIsBrandingTenant(int tenant)
        {
            try
            {
                BrandingResult brandingArgs = new BrandingResult();

                TenantManagementQuery tenantManagementQuery = new TenantManagementQuery(tenant);
                TenantManagementPM tenantManagementPM = tenantManagementQuery.GetSinglePM(tenant);

                if (tenantManagementPM != null)
                {
                    brandingArgs.EnableBranding = tenantManagementPM.EnableBranding;
                    brandingArgs.ContactEmail = tenantManagementPM.ContactEmail;
                }

                return brandingArgs;
            }

            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "BrandingController : GetIsBrandingTenant Method", null);
                return null;
            }
        }


        #region ResizeTenantImage
        private byte[] ResizeImage(byte[] image, int width, int height, string extension)
        {
            using (var stream = new System.IO.MemoryStream(image))
            {
                var img = Image.FromStream(stream);

                float nPercentW = 1;
                float nPercentH = 1;

                if (img.Height > height) nPercentH = ((float)height / (float)img.Height);

                if (img.Width > width) nPercentW = ((float)width / (float)img.Width);

                if (nPercentH != 1 && nPercentW != 1)
                {
                    var thumbnail = FixedSize(img, width, height);

                    System.Drawing.Imaging.EncoderParameters param = new System.Drawing.Imaging.EncoderParameters(1);
                    ImageCodecInfo myImageCodecInfo;

                    var Quality = 90L;
                    param.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Quality);


                    using (var thumbStream = new System.IO.MemoryStream())
                    {
                        if (extension == "jpg")
                        {
                            myImageCodecInfo = GetEncoderInfo("image/jpeg");
                            thumbnail.Save(thumbStream, myImageCodecInfo, param); //thumbnail.Save(thumbStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                        else if (extension == "png")
                        {
                            myImageCodecInfo = GetEncoderInfo("image/png");
                            thumbnail.Save(thumbStream, myImageCodecInfo, param);


                            // thumbnail.Save(thumbStream, System.Drawing.Imaging.ImageFormat.Png);
                        }

                        return thumbStream.GetBuffer();
                    }
                }
                else
                {
                    return image;
                }

            
            }

        }
        private  Image FixedSize(Image imgPhoto, int Width, int Height)
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



                if (sourceHeight > Height) nPercentH = ((float)Height / (float)sourceHeight);

                if (sourceWidth > Width) nPercentW = ((float)Width / (float)sourceWidth);
               
                if (nPercentH != 1 && nPercentW != 1)
                {
                    if (nPercentH < nPercentW)
                    {
                        nPercent = nPercentW;
                        destX = System.Convert.ToInt16((Width -
                                      (sourceWidth * nPercent)) / 2);
                    }
                    else
                    {
                        nPercent = nPercentH;
                        destY = System.Convert.ToInt16((Height -
                                      (sourceHeight * nPercent)) / 2);
                    }
                }
                else
                {

                    return imgPhoto;
                    //nPercent = nPercentW;
                    //destX = System.Convert.ToInt16((Width -
                    //              (sourceWidth * nPercent)) / 2);

                    //nPercent = nPercentH;
                    //destY = System.Convert.ToInt16((Height -
                    //              (sourceHeight * nPercent)) / 2);
                }



                if (destX < 0) destX = 0;
                if (destY < 0) destY = 0;
                int destWidth = (int)(sourceWidth * nPercentW);
                int destHeight = (int)(sourceHeight * nPercentH);

                Bitmap bmPhoto = new Bitmap(Width, Height,
                                  PixelFormat.Format24bppRgb);
                bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                                 imgPhoto.VerticalResolution);

                Graphics grPhoto = Graphics.FromImage(bmPhoto);
                grPhoto.Clear(Color.White);
                grPhoto.InterpolationMode =
                        InterpolationMode.HighQualityBicubic;

                grPhoto.DrawImage(imgPhoto,
                    new Rectangle(destX, -((destWidth - Height)/2), destWidth, destWidth),
                    new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                    GraphicsUnit.Pixel);

                grPhoto.Dispose();
                return bmPhoto;
            }
            catch (Exception ex)
            {
                return imgPhoto;
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

        public byte[] CreateImageThumbnail(byte[] image, int width = 50, int height = 50)
        {
            using (var stream = new System.IO.MemoryStream(image))
            {
                var img = Image.FromStream(stream);
                var thumbnail = img.GetThumbnailImage(width, height, () => false, IntPtr.Zero);

                using (var thumbStream = new System.IO.MemoryStream())
                {
                    thumbnail.Save(thumbStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    return thumbStream.GetBuffer();
                }
            }
        }
        #endregion
    }



}
public class BrandingResult
{
    public bool EnableBranding { get; set; }
    public string ContactEmail { get; set; }

}