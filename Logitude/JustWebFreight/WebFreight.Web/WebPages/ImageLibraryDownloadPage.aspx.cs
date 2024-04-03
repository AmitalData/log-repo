using System;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.SystemLogs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using WebFreight.Web.WebServices;
using System.Web;

namespace WebFreight.Web.WebPages
{
    public partial class ImageLibraryDownloadPage : System.Web.UI.Page
    {
        private static readonly string authenticationError = "Sorry you’re not authenticated to view this document.";
        private string securityId;
        private int tenant;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ValidateRequest();
                SetParameters();
                DownloadImage();
            }
            catch (ExceptionInErrorLog ExceptionInErrorLog)
            {
                Response.Clear();
                Response.Output.Write(ExceptionInErrorLog.ToString());
            }
            catch (Exception errorInfo)
            {
                string errorMessage = errorInfo.Message;
                if (!errorMessage.Contains("Sorry you’re not authenticated to view this document") && !errorMessage.Contains("Sorry, your download link has expired.") && !errorMessage.Contains("Document file is empty."))
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "DownloadPage : PageLoad Method", null);
                }
            }
        }

        private void ValidateRequest()
        {
            if (string.IsNullOrEmpty(Request["securityId"]) || string.IsNullOrEmpty(Request["tenant"]))
            {
                ThrowAuthenticationError();
            }
        }

        private void ThrowAuthenticationError()
        {
            Response.Output.Write(authenticationError);
            throw new ApplicationException(authenticationError);
        }

        private void SetParameters()
        {
            securityId = Request["securityId"] ?? "";
            tenant = int.Parse(Request["tenant"]);
        }

        private void DownloadImage()
        {
            ImageLibrary imageLibrary = GetImageDetail();
            Uploader uploader = new Uploader();
            byte[] imageBytes = GetImageByteByTenant(imageLibrary, uploader, tenant);
            if (imageBytes == null) imageBytes = GetImageByteByTenant(imageLibrary, uploader, 0);
            if (imageBytes == null) ThrowAuthenticationError();
            BuildHttpResponse(imageBytes, imageLibrary);
        }

        private byte[] GetImageByteByTenant(ImageLibrary imageLibrary, Uploader uploader, int tenant)
        {
            return uploader.DownloadFile(imageLibrary.ImageDetail.Id, imageLibrary.ImageDetail.Extension, "images", tenant, tenant == 0);
        }

        private void BuildHttpResponse(byte[] imageBytes, ImageLibrary imageLibrary)
        {
            var contentHeader = GetHttpResponseContentHeader(imageLibrary);

            HttpContext.Current.Response.AppendHeader("Content-Disposition", contentHeader);
            HttpContext.Current.Response.ContentType = "image/" + imageLibrary.ImageDetail.Extension;
            HttpContext.Current.Response.BinaryWrite(imageBytes);
            if (HttpContext.Current.Response.IsClientConnected)
            {
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.Close();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        private string GetHttpResponseContentHeader(ImageLibrary imageLibrary)
        {
            string fileName = imageLibrary.Name + "." + imageLibrary.ImageDetail.Extension;
            var browser = HttpContext.Current.Request.Browser;
            if (browser != null && browser.Browser.Equals("ie", StringComparison.OrdinalIgnoreCase))
            {
                return "attachment; filename*=UTF-8''" + HttpUtility.UrlPathEncode(fileName) + "\"";
            }
            return "attachment; filename=\"" + HttpUtility.UrlPathEncode(fileName) + "\"";
        }

        private ImageLibrary GetImageDetail()
        {
            ImageLibrary imageLibrary = new ImageLibraryRepository(tenant).GetSingleBySecurityIdAndTenant(securityId, tenant);
            if (imageLibrary == null) imageLibrary = new ImageLibraryRepository(0).GetSingleBySecurityIdAndTenant(securityId, 0);
            if (imageLibrary == null || imageLibrary.ImageDetail == null) ThrowAuthenticationError();
            return imageLibrary;
        }


    }
}