using System;
using System.Web.Services;
using Microsoft.WindowsAzure.Storage;

using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Azure;

using WebFreight.Web.Azure;
//using WebFreight.Web.Azure.AzureCommunicationLog;
using WebFreight.Web.Helpers;
using WebFreight.Web.Azure.AzureCommunicationLog;
using Microsoft.WindowsAzure.Storage.Queue;

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for CommunicationLogsAzureService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CommunicationLogsAzureService : System.Web.Services.WebService
    {

        [WebMethod]
        public string InsertCommunicationLog(int tenant, string createdByUserId, string createdByUserName, string from, string to, string subject, string bodyDocumentId, bool isBodyHtml,string BCC = null)
        {


            //MemoryStream memorystream = new MemoryStream(bytearray);

            //XmlSerializer serializer = new XmlSerializer(typeof(CommunicationLogsAzure));

            //CommunicationLogsAzure communicationLogsAzure = (CommunicationLogsAzure)serializer.Deserialize(memorystream);

            CommunicationLogsAzure communicationLog = new CommunicationLogsAzure()
            {
                PartitionKey = DateTime.Now.Date.Day + "," + DateTime.Now.Date.Month + "," + DateTime.Now.Date.Year, //"1",
                RowKey = Guid.NewGuid().ToString(),
                Id = Guid.NewGuid().ToString(),
                Tenant = tenant,
                CreatedByUserId = createdByUserId,
                CreatedByUserName = createdByUserName,
                From = from,
                To = to,
                Subject = subject,
                StatusCode = "W",
                GMTCreateDateTime = DateTime.Now,
                SendDateTime = DateTime.Now,
                LocalCreateDateTime = TenantServerConfigration.GetCurrentDateTime(tenant),
                BodyDocumentId = bodyDocumentId,
                IsBodyHtml = isBodyHtml,
            };

            if (!string.IsNullOrEmpty(BCC))
            {
                communicationLog.BCC = BCC;
            } 

            CommunicationLogsAzureDomainService commLogService = new CommunicationLogsAzureDomainService();
            commLogService.AddCommunicationLogsAzure(communicationLog);
            //commLogService.SaveChanges();

            var storageaccount = StorageAcountDetails.StorageAccount;
            var queueclient = storageaccount.CreateCloudQueueClient();

            var queue = queueclient.GetQueueReference("communicationlogqueue");
            queue.CreateIfNotExists();

            var message = new CloudQueueMessage(communicationLog.Id + "," + communicationLog.Tenant);
            queue.AddMessage(message);

            return "";
        }
    }
}
