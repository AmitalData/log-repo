using System;
using System.Globalization;
using System.IO;
using System.Web;
//using Microsoft.WindowsAzure.Storage;

using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;

using WebFreight.Web.Azure;
using Microsoft.WindowsAzure.Storage.Blob;

namespace WebFreight.Web
{
    /// <summary>
    /// Summary description for XapFileDownloadHandler
    /// </summary>
    public class XapFileDownloadHandler : IHttpHandler
    {
        CloudBlobContainer blobContainer;
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                 
                blobContainer = StorageAcountDetails.GetCurrentContainer("xapflesblob");
                string xapname = HttpContext.Current.Request.QueryString["xapname"];
                string xapdate = HttpContext.Current.Request.QueryString["xapdate"];

                if (!String.IsNullOrEmpty(xapname))
                {
                    GetXapFileData(xapname, xapdate);
                }

                //context.Response.ContentType = "text/plain";
                //context.Response.Write("Hello World");
            }
            catch
            {

            }
        }

        

        public void GetXapFileData(string xapfilename, string xapdate)
        {
            xapfilename = xapfilename.ToLower();


            if (
                !LogitudeSettings.IsCostomsDeploy
                //Simplog.Server.Infrastructure.Helpers.SettingUtil.ForceDownloadXapFromIIS() 
                &&
                
                LogitudeSettings.DeploymentStage != "Dev" && LogitudeSettings.UsingAzure)
            {

                var blobfile = blobContainer.GetBlockBlobReference(xapfilename);

                if (blobfile.Exists())
                {

                    using (MemoryStream memstream = new MemoryStream())
                    {

                        blobfile.DownloadToStream(memstream);
                        byte[] datainByte = memstream.ToArray();

                        WriteXapBinary(datainByte, "application/x-silverlight-app", xapdate, xapfilename);


                    }


                }

             

            }
            else
            {
                byte[] datainByte = GetFileFromServer(xapfilename);
                WriteXapBinary(datainByte, "application/x-silverlight-app", xapdate, xapfilename);
            }



        }

        #region WriteXapBinary

        
        void WriteXapBinary(byte[] datainByte, string contenttype, string xapdate, string xapname)
        {

            try
            {

                
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ClearHeaders();
             
                CultureInfo en = new CultureInfo("en-US");
                DateTime date;
                if (!DateTime.TryParse(xapdate, out date))
                {
                    date = DateTime.Parse(xapdate, en.DateTimeFormat);
                    
                }
                date = date.ToUniversalTime();

                string d = date.ToString(@"ddd, dd MMM yyyy HH':'mm':'ss 'GMT'", en.DateTimeFormat);
              /////////////////////////////////////////////////////////////////////////////// 
                HttpContext.Current.Response.Headers.Add("Last-Modified", d);
                HttpContext.Current.Response.Headers.Add("Accept-Ranges", "bytes");

                HttpContext.Current.Response.ContentType = contenttype;
               
                //HttpContext.Current.Response.Buffer = true;
                //HttpContext.Current.Response.BufferOutput = true;
              
                HttpContext.Current.Response.BinaryWrite(datainByte);
                  
                
                    





                //        HTTP/1.1 304 Not Modified
                //Last-Modified: Tue, 27 Mar 2012 08:33:03 GMT
                //Accept-Ranges: bytes
                //ETag: "2d4dfa39f4bcd1:0"
                //Server: Microsoft-IIS/7.5
                //X-Powered-By: ASP.NET
                //Date: Tue, 27 Mar 2012 08:37:44 GMT


                ///////////////////////////////////////////////////////////////infra


//              HTTP/1.1 200 OK
//Cache-Control: public
//Content-Type: text/html
//Last-Modified: 27/04/33 11:00:15 ص
//Accept-Ranges: bytes
//ETag: 27/04/33 11:00:15 ص
//Server: Microsoft-IIS/7.5
//X-AspNet-Version: 4.0.30319
//X-Powered-By: ASP.NET
//Date: Tue, 27 Mar 2012 08:45:25 GMT
//Content-Length: 4628041





            }
            catch (Exception ex)
            {
                string err = ex.Message;
                HttpContext.Current.Response.Write(" ERROR_READING_FILE ");
            }

           
        }

        #endregion


        #region UploadXapFile


        private byte[] UploadXapFile(string filename)
        {
            filename = filename.ToLower();
            byte[] buffer = GetFileFromServer(filename);


            

            //temp file ( to be deleted when upload done)
            CloudBlockBlob tempcloudBlob = blobContainer.GetBlockBlobReference(filename);


            MemoryStream memorystream = new MemoryStream(buffer);

            tempcloudBlob.UploadFromStream(memorystream);

            return buffer;
        }


        #endregion

        #region GetFileFromServer



        public byte[] GetFileFromServer(string filename)
        {


            string path = HttpContext.Current.Server.MapPath(".");
            path += "\\ClientBin\\";
            path += filename;


            FileStream fs = File.OpenRead(path);

            byte[] resultFile = new byte[fs.Length];
            fs.Read(resultFile, 0, resultFile.Length);
            fs.Close();

            return resultFile;

        }

        #endregion





         


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }



        public byte[] GetXapFileByteData(string xapfilename, string xapdate)
        {
            byte[] datainByte = null;
            xapfilename = xapfilename.ToLower();

            if (LogitudeSettings.DeploymentStage != "Dev")
            {

                var blobfile = blobContainer.GetBlockBlobReference(xapfilename);

                if (blobfile.Exists())
                {

                    using (MemoryStream memstream = new MemoryStream())
                    {

                        blobfile.DownloadToStream(memstream);
                        datainByte = memstream.ToArray();

                     


                    }


                }

                //else
                //{
                //   byte[] DatainByte= UploadXapFile(xapfilename);

                //    WriteXapBinary(DatainByte, "application/x-silverlight-app");
                //}

            }
            else
            {
               datainByte = GetFileFromServer(xapfilename);
                
            }

            return datainByte;

        }
    }
}