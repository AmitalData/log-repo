using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code
{
    public class PrivateLableController : ApiController
    {
        public string GetPrivateLabelLogoUri(string url)
        {
            try
            {
                string result = "";
                byte[] datainByte = null;
                if (!url.Contains("system.logitudeworld.com") && !url.Contains("system.logbox.co.il") && !url.Contains("cloud.amital.co.il"))
                {
                    TenantManagmentPrivateLabelsQuery query = new TenantManagmentPrivateLabelsQuery(0);
                    var privatelabel = query.GetSingleActivePMByUrl(url);
                    if (privatelabel != null)
                    {
                        datainByte = privatelabel.MainLogo;
                        if (datainByte != null)
                        {

                            datainByte = ResizeImage(datainByte, 290, 114, "jpg");
                            string base64String = System.Convert.ToBase64String(datainByte, 0, datainByte.Length);
                            result = "data:image/jpg;base64," + base64String;
                            return result;
                        }
                        else return null;
                    }
                    else return null;

                }

                else return null;


            }

            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "PrivateLableController : GetTenantLogoUri Method", null);
                return null;
            }


        }

        public string GetPrivateLabelSmallLogoUri(byte[] datainByte,int width,int height)
        {
            try
            {
                string result = "";

                if (datainByte != null)
                {

                    datainByte = ResizeImage(datainByte, width, height, "jpg");
                    string base64String = System.Convert.ToBase64String(datainByte, 0, datainByte.Length);
                    result = "data:image/jpg;base64," + base64String;
                    return result;
                }
                else return null;




            }

            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "PrivateLableController : GetTenantLogoUri Method", null);
                return null;
            }


        }


        public PrivateLableResult GetIsPrivateLableUrl(string url)
        {
            try
            {
                //var URL = SecurityUtility.getLoggedDomain();
                PrivateLableResult PrivateLableArgs = MapPrivateLableArgs(url);
                return PrivateLableArgs;
            }

            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "PrivateLableController : GetIsPrivateLableUrl Method", null);
                return null;
            }
        }

        public PrivateLableResult GetIsPrivateLableByLoggedDomain()
        {
            try
            {
                string url = SecurityUtility.getLoggedDomain();
                PrivateLableResult PrivateLableArgs = MapPrivateLableArgs(url);
                return PrivateLableArgs;
            }

            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "PrivateLableController : GetIsPrivateLableUrl Method", null);
                return null;
            }
        }

        private PrivateLableResult MapPrivateLableArgs(string url)
        {
            PrivateLableResult PrivateLableArgs = new PrivateLableResult();
            if (!url.Contains("system.logitudeworld.com") && !url.Contains("system.logbox.co.il") && !url.Contains("cloud.amital.co.il"))
            {
                TenantManagmentPrivateLabelsQuery query = new TenantManagmentPrivateLabelsQuery(0);
                var privatelabel = query.GetSingleActivePMByUrl(url);
                if (privatelabel != null)
                {
                    PrivateLableArgs.Id = privatelabel.Id;
                    PrivateLableArgs.PrivateLabelName = privatelabel.PrivateLabelName;
                    PrivateLableArgs.PrivateLabelShortName = privatelabel.PrivateLabelShortName;
                    PrivateLableArgs.PrivateLabelUrl = privatelabel.PrivateLabelUrl;
                    PrivateLableArgs.PrivateLabelDomain = privatelabel.PrivateLabelDomain;
                    PrivateLableArgs.MainLogo = privatelabel.MainLogo;
                    PrivateLableArgs.ContactUsEmail = privatelabel.ContactUsEmail;
                    PrivateLableArgs.EnablePrivateLable = true;
                    PrivateLableArgs.SmallLogoURL = GetPrivateLabelSmallLogoUri(privatelabel.SmallLogo, 24, 24);
                    PrivateLableArgs.LogoURL = GetPrivateLabelSmallLogoUri(privatelabel.MainLogo, 290, 114);
                }
                else
                {
                    PrivateLableArgs.EnablePrivateLable = false;
                }
            }

            return PrivateLableArgs;
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

                        //thumbnail.Save(@"C:\test.jpg");
                        // g.Dispose();
                        return thumbStream.GetBuffer();
                    }
                }
                else
                {
                    return image;
                }


            }

        }
        private Image FixedSize(Image imgPhoto, int Width, int Height)
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
                    new Rectangle(destX, -((destWidth - Height) / 2), destWidth, destWidth),
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
        #endregion
    }



}
public class PrivateLableResult
{
    public string Id { get; set; }
    public string PrivateLabelName { get; set; }
    public string PrivateLabelShortName { get; set; }
    public string PrivateLabelUrl { get; set; }
    public string PrivateLabelDomain { get; set; }
    public byte[] MainLogo { get; set; }
    public string ContactUsEmail { get; set; }
    public bool ReceiveAllStatuses { get; set; }
    public string HybridPartnerId { get; set; }
    public bool InActive { get; set; }
    public bool EnablePrivateLable { get; set; }
    public string SmallLogoURL { get; set; }
    public string LogoURL { get; set; }

}