using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Stimulsoft.Report;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.WebPages
{
    public partial class DownLoadImage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

                string ImageId = Request["imageId"] ?? "";
                 string mTenant = Request["tenant"] ?? "";
                if (!string.IsNullOrEmpty(mTenant) && !string.IsNullOrEmpty(ImageId))
                {
                     int tenant = int.Parse(mTenant);
                     ImageLibraryRepository imageLibraryRepository = new ImageLibraryRepository(tenant);
                     string documentId = imageLibraryRepository.GetDocumentIdByImageLibraryId(ImageId);
                     if (!string.IsNullOrEmpty(documentId))
                     {
                         DocumentRepository documentRepository = new DocumentRepository(tenant);
                         Document document = documentRepository.GetSingleDocument(tenant, documentId);
                         if (document != null)
                         {
                           
                             //IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                             //BlobFileInfo fileInfo = new BlobFileInfo()
                             //{
                             //    FileName = document.FileName,
                             //    FolderName = "ImageLibrary",
                             //    Extension = document.Extension,
                             //    Tenant = tenant,
                             //};

                             Uploader up = new Uploader();
                             byte[] _DatainByte = up.DownloadFile(document.Id, document.Extension, "", document.Tenant);

                             if (_DatainByte != null)
                             {
                                 var browser = HttpContext.Current.Request.Browser;
                                 string documentName = document.FileName + "." + document.Extension;
                                 string ShowType = "inline";
                                 switch (document.Extension)
                                 {
                                     case "jpg":
                                         HttpContext.Current.Response.ContentType = "image/jpeg";
                                         break;

                                     case "png":
                                         HttpContext.Current.Response.ContentType = "image/png";
                                         break;
                                     default:
                                         HttpContext.Current.Response.ContentType = "application/octet-stream";
                                         break;
                                 }


                                 if (browser != null && browser.Browser.Equals("ie", StringComparison.OrdinalIgnoreCase))
                                 {
                                     HttpContext.Current.Response.AppendHeader("Content-Disposition", ShowType + "; filename*=UTF-8''" + HttpUtility.UrlPathEncode(documentName) + "\"");
                                 }
                                 else
                                 {
                                     HttpContext.Current.Response.AppendHeader("Content-Disposition", ShowType + "; filename=\"" + HttpUtility.UrlPathEncode(documentName) + "\"");
                                 }


                                 if (_DatainByte != null)
                                 {
                                     HttpContext.Current.Response.BinaryWrite(_DatainByte);

                                     if (HttpContext.Current.Response.IsClientConnected)
                                     {
                                         HttpContext.Current.Response.Flush();
                                         //HttpContext.Current.Response.Close();
                                         HttpContext.Current.Response.End();
                                         HttpContext.Current.ApplicationInstance.CompleteRequest();

                                     }
                                 }
                             }

                         }



                     }
      
                }
        }




        
    }
}