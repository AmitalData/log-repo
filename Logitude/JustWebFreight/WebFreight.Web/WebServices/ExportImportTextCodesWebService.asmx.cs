using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Services;
using System.Xml.Serialization;
//using Microsoft.WindowsAzure.Storage;

using Simplog.Server.Infrastructure.Azure;

using WebFreight.Web.Azure;
using WebFreight.Web.Helpers;
using Microsoft.WindowsAzure.Storage.Queue;

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for ExportImportTextCodesWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
        
    public class ExportImportTextCodesWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public void ExportImportTextCodes(byte[] connectiondetails)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ConnectionDetails));
            MemoryStream memstream = new MemoryStream(connectiondetails);
            ConnectionDetails connectionDetailsObject = (ConnectionDetails)serializer.Deserialize(memstream);

            var storageaccount = StorageAcountDetails.StorageAccount;
            var queueclient = storageaccount.CreateCloudQueueClient();
            CloudQueue queue = null;

            queue = queueclient.GetQueueReference("textcodeexport");
            queue.CreateIfNotExists();

            string messageType = "";
            if (connectionDetailsObject.IsTextCodesExport)
            {
               
                messageType = "export";
            }
            else if (connectionDetailsObject.IsTextCodesImport)
            {
               
                messageType = "import";
            }


            string msg = !string.IsNullOrEmpty(connectionDetailsObject.Server) ? connectionDetailsObject.Server : "";
            msg += ",";
            msg += !string.IsNullOrEmpty(connectionDetailsObject.UserName) ? connectionDetailsObject.UserName : "";
            msg += ",";
            msg += !string.IsNullOrEmpty(connectionDetailsObject.Password) ? connectionDetailsObject.Password : "";
            msg += ",";
            msg += !string.IsNullOrEmpty(connectionDetailsObject.DataBase) ? connectionDetailsObject.DataBase : "";
            msg += ",";
            msg += !string.IsNullOrEmpty(connectionDetailsObject.BlobName) ? connectionDetailsObject.BlobName : "";
            msg += ",";
            msg += messageType;

            var message = new CloudQueueMessage(msg);
            queue.AddMessage(message);
        }


        [WebMethod]
        public byte[] GetAllDatabases(byte[] connectiondetails)
        {
            string result = "";
            XmlSerializer serializer = new XmlSerializer(typeof(ConnectionDetails));
            MemoryStream memstream = new MemoryStream(connectiondetails);
            ConnectionDetails connectionDetailsObject = (ConnectionDetails)serializer.Deserialize(memstream);

            string server = !string.IsNullOrEmpty(connectionDetailsObject.Server) ? connectionDetailsObject.Server : "";
            string username = !string.IsNullOrEmpty(connectionDetailsObject.UserName) ? connectionDetailsObject.UserName : "";
            string password = !string.IsNullOrEmpty(connectionDetailsObject.Password) ? connectionDetailsObject.Password : "";

            String conxString = "Data Source=" + server + ";Persist Security Info=False;User ID=" + username + ";Password=" + password;
            //"Data Source=MYSERVER; Integrated Security=True;";

            try
            {
                using (SqlConnection sqlConx = new SqlConnection(conxString))
                {
                    sqlConx.Open();
                    DataTable tblDatabases = sqlConx.GetSchema("Databases");
                    sqlConx.Close();

                    foreach (DataRow row in tblDatabases.Rows)
                    {
                        result += row["database_name"] + ",";
                    }
                }
            }
            catch
            {
                return null;
                throw new Exception();
            }

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(result);
        }

      
    }
}
