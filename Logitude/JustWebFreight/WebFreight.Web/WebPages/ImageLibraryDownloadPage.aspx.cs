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
        private ImageDetail imageDetail;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PageValidation();
                GetParameters();
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

        private void PageValidation()
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

        private void GetParameters()
        {
            securityId = Request["securityId"] ?? "";
            tenant = int.Parse(Request["tenant"]);
        }

        private void DownloadImage()
        {
            GetImageDetail();
            Uploader uploader = new Uploader();
            byte[] imageBytes = uploader.DownloadFile(imageDetail.Id, imageDetail.Extension, "images", tenant, tenant == 0);
            if (imageBytes == null) ThrowAuthenticationError();
            ShowImage(imageBytes);
        }

        private void ShowImage(byte[] imageBytes)
        {
            string fileName = imageDetail.Id + "." + imageDetail.Extension;
            HttpContext.Current.Response.ContentType = "image/" + imageDetail.Extension;
            var browser = HttpContext.Current.Request.Browser;
            if (browser != null && browser.Browser.Equals("ie", StringComparison.OrdinalIgnoreCase))
            {
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename*=UTF-8''" + HttpUtility.UrlPathEncode(fileName) + "\"");
            }
            else
            {
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + HttpUtility.UrlPathEncode(fileName) + "\"");
            }


            HttpContext.Current.Response.BinaryWrite(imageBytes);
            if (HttpContext.Current.Response.IsClientConnected)
            {
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.Close();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        private void GetImageDetail()
        {
            ImageLibrary imageLibrary = new ImageLibraryRepository(tenant).GetSingleBySecurityIdAndTenant(securityId, tenant);
            if (imageLibrary == null || imageLibrary.ImageDetail == null) ThrowAuthenticationError();
            imageDetail = imageLibrary.ImageDetail;
        }


    }
}