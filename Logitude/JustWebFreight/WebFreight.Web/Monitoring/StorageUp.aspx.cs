using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.WindowsAzure.Storage;
using System.Text;
using WebFreight.Web.Azure;
using WebFreight.Web.Testing;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using Logitude.SystemLogs;

namespace WebFreight.Web.Monitoring
{
    public partial class StorageUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/xml";
            //Response.Write("<?xml version='1.0' encoding='UTF-8'?>");
            Response.Write("<pingdom_http_custom_check>");

            if (IsStorageUp())
            {
                Response.Write("<status>OK</status>");
            }
            else
            {
                Response.Write("<status>Fail</status>");
            }
            int ResponseTime_Millisecond = HttpContext.Current.Timestamp.Millisecond;
            String ResponseTime = "<response_time>" + ResponseTime_Millisecond + "</response_time>";
            Response.Write(ResponseTime);
            Response.Write("</pingdom_http_custom_check>");
            Response.End();
        }

        private bool IsStorageUp()
        {
            CloudBlobContainer blobContainer;
            CloudBlockBlob testblob;
            StringBuilder stringbuilder;

            try
            {
                blobContainer = StorageAcountDetails.GetCurrentContainer(0);
                blobContainer.CreateIfNotExists();
                string blobName = "testing/" + Guid.NewGuid().ToString() + "_" + "testblob.txt";
                testblob = blobContainer.GetBlockBlobReference(blobName);
                stringbuilder = new StringBuilder();

                //write data to errorlogs
                using (Stream blbstr = testblob.OpenWrite())
                {
                    string newlog = "Test message";
                    Encoding encoding = new UTF8Encoding();
                    stringbuilder.AppendLine(newlog);
                    byte[] errordata = encoding.GetBytes(stringbuilder.ToString());
                    blbstr.Write(errordata, 0, errordata.Length);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "StorageUp", "Bug in IsStorageUp Method : Write to a Blob",null);
                return false;
            }

            if (testblob.Exists())
            {
                try
                {
                    //Read data from errorlogs
                    using (Stream blbstr_read = testblob.OpenRead())
                    {
                        if (blbstr_read != null)
                        {
                            byte[] Previous_errordata = new byte[blbstr_read.Length];
                            blbstr_read.Read(Previous_errordata, 0, Previous_errordata.Length);

                            Encoding encoding = new UTF8Encoding();
                            stringbuilder.Append(encoding.GetString(Previous_errordata));
                        }
                    }
                }
                catch (Exception e)
                {

                    ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "StorageUp", "Bug in IsStorageUp Method : Read from a Blob",null);
                    return false;
                }



                try
                {
                    testblob.DeleteIfExists();
                }
                catch (Exception e)
                {
                    ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "StorageUp", "Bug in IsStorageUp Method : delete a Blob",null);
                    return false;
                }
            }

            return true;
        }
    }
}