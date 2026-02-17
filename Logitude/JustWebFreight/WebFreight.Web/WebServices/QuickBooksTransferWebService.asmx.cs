using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;
using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.Security;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using WebFreight.Web.RequestParams;

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for QuickBooksTransferWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class QuickBooksTransferWebService : System.Web.Services.WebService
    {

       
        [WebMethod]
        public string TestConnection(byte[] requestParamsData)
        {
            string status = "succeeded";
            string accessToken = "qyprdTrX4ctgHwHaoejWy8u6VVvGpxkzIK5lbuMoAU03sTZM";
            string accessTokenSecret = "9MKgM4X9zCkiHKu7LBFAnDoK6UuoQgon7BPmKBY5";
            string consumerKey = "qyprd4Yz3bsRFpUsDPXbpF68hGzeRW";
            string consumerSecret = "BURunV1MvWvy170dq0awYwAjtb3ASKwP6wQezMfP";

            OAuthRequestValidator oauthValidator = new OAuthRequestValidator(
            accessToken, accessTokenSecret, consumerKey, consumerSecret);

            string appToken = "d16d4d95b6364b4975b8e00b64d7d8ef339b";
            string companyID = "312501836";
            ServiceContext context = new ServiceContext(appToken, companyID, IntuitServicesType.QBO, oauthValidator);

            DataService service = new DataService(context);
            Term term = new Term();
            //Mandatory Fields
            term.Name = "net";
           
            try
            {
                List<Term> customers = service.FindAll<Term>(term, 1, 10).ToList();
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }
            return status;
        }

        [WebMethod]
        public string StartTransferReadyInvoices(byte[] requestParamsData)
        {
            MemoryStream memorystream = new MemoryStream(requestParamsData);
            XmlSerializer serializer = new XmlSerializer(typeof(StartTransferReadyInvoicesRequestParams));
            StartTransferReadyInvoicesRequestParams  requestParams = (StartTransferReadyInvoicesRequestParams)serializer.Deserialize(memorystream);

            string status = "succeed";
            try
            {
                BrokeredMessage message = new BrokeredMessage();
                message.Properties["Tenant"] = requestParams.Tenant;
                message.Properties["type"] = "starttransfer";
                string queueName = WebFreightEntryPoint.GetQueueByEnviroment("quickbooksqueue");
                QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(queueName);
                client.Send(message);
            }
            catch (Exception ex)
            {
                status = ex.Message;
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "quickbooks request answer web service", null,null);
            }
            return status;
        }

        [WebMethod]
        public string GetQuickbooksTables(byte[] requestParamsData)
        {
            MemoryStream memorystream = new MemoryStream(requestParamsData);
            XmlSerializer serializer = new XmlSerializer(typeof(GetQuickbooksTablesRequestParams));
            GetQuickbooksTablesRequestParams requestParams = (GetQuickbooksTablesRequestParams)serializer.Deserialize(memorystream);


            string status = "succeed";
            try
            {
                BrokeredMessage message = new BrokeredMessage();
                message.Properties["Tenant"] = requestParams.Tenant;
                message.Properties["type"] = "gettables";
                string queueName = WebFreightEntryPoint.GetQueueByEnviroment("quickbooksqueue");
                QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(queueName);
                client.Send(message);
            }
            catch (Exception ex)
            {
                status = ex.Message;
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "quickbooks request answer web service", null,null);
            }
            return status;
        }


    }
}
