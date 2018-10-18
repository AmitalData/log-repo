using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Microsoft.WindowsAzure.Storage.Blob;
using Simplog.Server.Infrastructure.Azure;

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for DownloadTransferFileWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DownloadTransferFileWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public byte[] Download(string filename, int tenant)
        {
            byte[] result = null;

            Uploader up = new Uploader();

            CloudBlobContainer ob = StorageAcountDetails.GetCurrentContainer(tenant);

            result = up.DownloadStaticFile(filename, ob.Name);

            return result;
        }
    }
}
