using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure.Storage.Blob;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace WebFreight.Web
{
    /// <summary>
    /// Summary description for FileDownloadHandler
    /// </summary>
    public class FileDownloadHandler : IHttpHandler
    {
        //CloudBlobContainer blobContainer;
        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");


            try
            {

                //blobContainer = StorageAcountDetails.GetCurrentContainer("xapflesblob");
                string fileid = HttpContext.Current.Request.QueryString["fileid"];
                string extention = HttpContext.Current.Request.QueryString["extention"];
                string location = HttpContext.Current.Request.QueryString["location"];
                int tenant = int.Parse(HttpContext.Current.Request.QueryString["tenant"]);
             
                byte[] datainByte= GetFile(fileid,extention,location,tenant);
                if (datainByte != null)
                {
                    WriteFileBinary(datainByte, "application/x-silverlight-app");
                }
                //context.Response.ContentType = "text/plain";
                //context.Response.Write("Hello World");
            }
            catch
            {

            }
        }

        public byte[] GetFile(string fileid, string extension, string location, int tenant)
        {

            try
            {

                byte[] datainByte;
                string filename = fileid + "." + extension;


                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = fileid,
                    FolderName = location,
                    Extension = extension,
                    Tenant = tenant,
                     

                };
                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                datainByte = storageservice.Read(fileInfo);

                return datainByte;
                //blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);


                //var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(filename, location));

                //if (blobfile.Exists())
                //{
                //    using (MemoryStream memstream = new MemoryStream())
                //    {

                //        blobfile.DownloadToStream(memstream);
                //        datainByte = memstream.ToArray();

                //    }

                //    return datainByte;
                //}

                //else
                //    return null;
                //// }
            }
            catch (Exception e)
            {
                // ExceptionHandler.HandleException(e, DateTime.Now, tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "Uploader : DownloadFile Method");
                return null;
            }
        }

        void WriteFileBinary(byte[] datainByte, string contenttype)
        {

            try
            {


                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ClearHeaders();

                CultureInfo en = new CultureInfo("en-US");
                DateTime date = DateTime.Now;
               
                 

                string d = date.ToString(@"ddd, dd MMM yyyy", en.DateTimeFormat);
                /////////////////////////////////////////////////////////////////////////////// 
                HttpContext.Current.Response.Headers.Add("Last-Modified", d);
                HttpContext.Current.Response.Headers.Add("Accept-Ranges", "bytes");

                HttpContext.Current.Response.ContentType = contenttype;

                //HttpContext.Current.Response.Buffer = true;
                //HttpContext.Current.Response.BufferOutput = true;

                HttpContext.Current.Response.BinaryWrite(datainByte);



                 

 


            }
            catch (Exception ex)
            {
                string err = ex.Message;
                HttpContext.Current.Response.Write(" ERROR_READING_FILE ");
            }


        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}