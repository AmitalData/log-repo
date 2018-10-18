using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Services;

namespace Logitude.StorageService
{
    /// <summary>
    /// Summary description for BlobService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
     
    public class BlobService : System.Web.Services.WebService
    {

        [WebMethod]
        public Response Read(string filePath)
        {
             
            Response response = new Response();
            try
            {
                string fileMapPath = System.Configuration.ConfigurationManager.AppSettings.Get("StorageServiceMapPath");
                string finalPath = fileMapPath + filePath;
                char c = Convert.ToChar(@"\");
                finalPath = finalPath.Replace('/', c);

                using (FileStream fs = File.OpenRead(finalPath))
                {
                    byte[] result = new byte[fs.Length];
                    fs.Read(result, 0, (int)fs.Length);
                    fs.Close();

                    response.Result = result;
                }


            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return response;
            }
           

            return response;
        }

        [WebMethod]
        public Response Write(byte[] data,string filePath)
        {
            Response response = new Response();
            try
            {
               
                string fileMapPath = System.Configuration.ConfigurationManager.AppSettings.Get("StorageServiceMapPath");
                string finalPath = fileMapPath + filePath;
                char c = Convert.ToChar(@"\");
                finalPath = finalPath.Replace('/', c);

                string filename = finalPath.Split(c)[finalPath.Split(c).Length - 1];
                string directory = finalPath.Replace(filename, "");

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                var doNotUpdateStream = true;///itzik 
                if (doNotUpdateStream)
                {
                    if (File.Exists(finalPath))
                    {
                        throw new Exception("The final Path already exist !!! impossible to update the stream ");  
                    }
                }                      ///
                using (FileStream fs = File.OpenWrite(finalPath))
                {
                    fs.Write(data, 0, (int)data.Length);

                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return response;
            }

            return response;
        }

        public static long ReceivedBytes
        {
            get;
            set;
        }

        [WebMethod]
        public Response WriteBlock(byte[] buffer, long fileSize, long sentBytes, string filePath, string fileName)
        {
            Response response = new Response();
            try
            {

                string fileMapPath = System.Configuration.ConfigurationManager.AppSettings.Get("StorageServiceMapPath");
                string finalPath = fileMapPath + filePath;
                char c = Convert.ToChar(@"\");
                finalPath = finalPath.Replace('/', c);

                string filename = finalPath.Split(c)[finalPath.Split(c).Length - 1];
                string directory = finalPath.Replace(filename, "");

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                //FileIOPermissionAccess.Append

                //FileName = filename;
                //if (sentBytes < fileSize)
                //{

                if (File.Exists(finalPath))
                {
                    using (var fileStream = new FileStream(finalPath, FileMode.Append, FileAccess.Write, FileShare.None))
                    using (var bw = new BinaryWriter(fileStream))
                    {

                        bw.Write(buffer);

                        bw.Close();
                    }
                }
                else
                {
                    using (var fileStream = new FileStream(finalPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    using (var bw = new BinaryWriter(fileStream))
                    {

                        bw.Write(buffer);

                        bw.Close();
                    }
                }
                //ReceivedBytes += buffer.Length;
                //if (sentBytes == fileSize)
                //{
                //    ReceivedBytes = 0;
                //}
                //}
                //else
                //{
                //    using (FileStream fs = File.OpenWrite(finalPath))
                //    {

                //        fs.Write(buffer, Convert.ToInt32(sentBytes), (int)buffer.Length);

                //        fs.Close();
                //    }
                //}


            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return response;
            }

            return response;
        }

        [WebMethod]
        public Response Delete(string filePath)
        {

            Response response = new Response();
            try
            {
                string fileMapPath = System.Configuration.ConfigurationManager.AppSettings.Get("StorageServiceMapPath");
                string finalPath = fileMapPath + filePath;
                char c = Convert.ToChar(@"\");
                finalPath = finalPath.Replace('/', c);

                File.Delete(finalPath);
                
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return response;
            }


            return response;
        }
       
    }
}
