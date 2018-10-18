using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using Logitude.SystemLogs;
using Microsoft.WindowsAzure.Storage.Blob;
using Simplog.Server.Infrastructure.Azure;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;

namespace WebFreight.Web.ReportsWebServices
{
    /// <summary>
    /// Summary description for ReportsTemplatesWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ReportsTemplatesWebService : System.Web.Services.WebService
    {
        [WebMethod]
        public byte[] GetReportTemplate(string reportId,int tenant, bool containTenantZero = true)
        {
            byte[] theDatainByte = null;

            try
            {
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = reportId,
                    FolderName = "reports",
                    Extension = "mrt",
                    Tenant = tenant,
                    
                };
                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                theDatainByte = storageservice.Read(fileInfo);


                if (theDatainByte  == null && containTenantZero)
                {
                    fileInfo.Tenant = 0;
                    theDatainByte = storageservice.Read(fileInfo);
                    
                }

                //string containername = StorageAcountDetails.GetCurrentContainer(tenant).Name;
                //CloudBlobContainer blobContainer = StorageAcountDetails.BlobClient.GetContainerReference(containername);
                //string filename = StorageAcountDetails.GetBlobNameByLocation(reportId + ".mrt", "reports");
                //var blobfile = blobContainer.GetBlockBlobReference(filename);

                //if (blobfile.Exists())
                //{
                //    using (MemoryStream memstream = new MemoryStream())
                //    {

                //        blobfile.DownloadToStream(memstream);
                //        theDatainByte = memstream.ToArray();

                //    }
                //}
                //else
                //{
                //    containername = StorageAcountDetails.GetCurrentContainer(0).Name;
                //    blobContainer = StorageAcountDetails.BlobClient.GetContainerReference(containername);
                //    filename = StorageAcountDetails.GetBlobNameByLocation(reportId + ".mrt", "reports");
                //    blobfile = blobContainer.GetBlockBlobReference(filename);

                //    if (blobfile.Exists())
                //    {
                //        using (MemoryStream memstream = new MemoryStream())
                //        {

                //            blobfile.DownloadToStream(memstream);
                //            theDatainByte = memstream.ToArray();

                //        }
                //    }

                //}
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
                ExceptionHandler.HandleException(e, DateTime.Now, 0, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "Uploader : DownloadStaticFile Method",ip);
                return null;
            }

           
        }

        [WebMethod]
        public byte[] GetChartTemplate(string chartName, int tenant)
        {
            byte[] theDatainByte = null;

            try
            {
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = chartName,
                    FolderName =  "charts",
                    Extension = "mrt",
                    Tenant = tenant,
                };
                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                theDatainByte = storageservice.Read(fileInfo);
                if (theDatainByte == null)
                {
                    
                    fileInfo.Tenant = 0;
                    theDatainByte = storageservice.Read(fileInfo);
                    
                }

                //string containername = StorageAcountDetails.GetCurrentContainer(tenant).Name;
                //CloudBlobContainer blobContainer = StorageAcountDetails.BlobClient.GetContainerReference(containername);
                //string filename = StorageAcountDetails.GetBlobNameByLocation(chartName + ".mrt", "charts");
                //var blobfile = blobContainer.GetBlockBlobReference(filename);

                //if (blobfile.Exists())
                //{
                //    using (MemoryStream memstream = new MemoryStream())
                //    {
                //        blobfile.DownloadToStream(memstream);
                //        theDatainByte = memstream.ToArray();
                //    }
                //}
                //else
                //{
                //    containername = StorageAcountDetails.GetCurrentContainer(0).Name;
                //    blobContainer = StorageAcountDetails.BlobClient.GetContainerReference(containername);
                //    filename = StorageAcountDetails.GetBlobNameByLocation(chartName + ".mrt", "charts");
                //    blobfile = blobContainer.GetBlockBlobReference(filename);

                //    if (blobfile.Exists())
                //    {
                //        using (MemoryStream memstream = new MemoryStream())
                //        {
                //            blobfile.DownloadToStream(memstream);
                //            theDatainByte = memstream.ToArray();
                //        }
                //    }
                //}
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
    }
}
