using System.Web.Services;
using Microsoft.WindowsAzure.Storage;

using Simplog.Server.Infrastructure.Azure;

using WebFreight.Web.Azure;
using Microsoft.WindowsAzure.Storage.Queue;

namespace WebFreight.Web.InfrastructureModel
{
    /// <summary>
    /// Summary description for UpdateTenantZeroService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class UpdateTenantZeroService : System.Web.Services.WebService
    {

        [WebMethod]
        public void SendMessageToQueue(string msg)
        {
            var storageaccount = StorageAcountDetails.StorageAccount;
            var queueclient = storageaccount.CreateCloudQueueClient();

            var queue = queueclient.GetQueueReference("updatetenantzeroqueue");
            queue.CreateIfNotExists();

            var message = new CloudQueueMessage(msg);
          
            queue.AddMessage(message);

        }

       

    }
}
