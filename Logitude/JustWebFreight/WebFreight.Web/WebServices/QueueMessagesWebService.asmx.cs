using System.Web.Services;
using Microsoft.WindowsAzure.Storage;

using Simplog.Server.Infrastructure.Azure;

using WebFreight.Web.Azure;
using Microsoft.WindowsAzure.Storage.Queue;
using WebFreight.Web.Security;
using Logitude.BL.CommonDataModel.EntityQueries;
using Microsoft.ServiceBus.Messaging;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.EntityPMs;
using System;
using Logitude.Server.Tools.QueueService;
using System.Collections.Generic;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for QueueMessagesWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class QueueMessagesWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public void BackUpForClientDataTables(int tenant)
        {
            QueueMessagesWebHelper queueMessagesWebHelper = new QueueMessagesWebHelper();
            queueMessagesWebHelper.BackUpForClientData(tenant);
        }

       

        [WebMethod]
        public void UpdateTenantManagementStatistics(int tenant)
        {

            BrokeredMessage message = new BrokeredMessage();
            message.Properties["Tenant"] = tenant;
            string taxqueueName = WebFreightEntryPoint.GetQueueByEnviroment("TenantStatisticsdataqueue");
            QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(taxqueueName);

            client.Send(message);

        }

        [WebMethod]
        public void UpdatePostFeeds(string postId, string email, int tenant)
        {

            BrokeredMessage message = new BrokeredMessage();
            message.Properties["PostId"] = postId;
            message.Properties["Email"] = email;
            message.Properties["Tenant"] = tenant;

            string taxqueueName = WebFreightEntryPoint.GetQueueByEnviroment("socialqueue");


            QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(taxqueueName);
            client.Send(message);


        }

        //[WebMethod]
        //public void UpdateReadyForActivationCustomers(string customerId,int tenant)
        //{
        //    try
        //    {


        //        QueueClient client = CreateCustomerQueue();
        //        BrokeredMessage message = new BrokeredMessage();
        //        message.Properties["CustomerId"] = customerId;
        //        message.Properties["Tenant"] = tenant;

        //        client.Send(message);
        //    }
        //    catch (System.Exception ex)
        //    {

        //    }

        //}

        [WebMethod]
        public void ReturnFaildShipmentToQueue(string Message, string customerId, string BatchNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(BatchNumber))
                {
                    IQueueService queueservice = new DbQueueService();
                    queueservice.InitializeQueue("ImportersShipmentQueue", 0);
                    var MSG = DictionaryJsonConverter.FromJsonToDictionary(Message);
                    //MSG["CorrelationId"] = Guid.NewGuid().ToString();
                    queueservice.Send(MSG, null, customerId);
                }
                else
                {
                    IQueueService queueservice = new DbQueueService();
                    queueservice.InitializeQueue("ImportersShipmentsBatchQueue", 0);
                    var MSG = DictionaryJsonConverter.FromJsonToDictionary(Message);
                    //MSG["CorrelationId"] = Guid.NewGuid().ToString();
                    queueservice.Send(MSG, null, customerId, BatchNumber);
                }


            }
            catch (System.Exception ex)
            {

            }

        }

        [WebMethod]
        public void ReturnFaildDocumentsToQueue(string Message, string customerId, string BatchNumber)
        {
            try
            {

                if (string.IsNullOrEmpty(BatchNumber))
                {
                    IQueueService queueservice = new DbQueueService();
                    queueservice.InitializeQueue("ImportersShipmentDocumentsQueue", 0);
                    var MSG = DictionaryJsonConverter.FromJsonToDictionary(Message);
                    //MSG["CorrelationId"] = Guid.NewGuid().ToString();
                    queueservice.Send(MSG);
                }
                else
                {
                    IQueueService queueservice = new DbQueueService();
                    queueservice.InitializeQueue("ImportersShipmentDocumentsBatchQueue", 0);
                    var MSG = DictionaryJsonConverter.FromJsonToDictionary(Message);
                    //MSG["CorrelationId"] = Guid.NewGuid().ToString();
                    queueservice.Send(MSG, null, customerId, BatchNumber);
                }

            }
            catch (System.Exception ex)
            {

            }

        }
        [WebMethod]
        public void ReturnFaildSharedDocumentsToQueue(string Message)
        {
            try
            { 
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("ForwardersShipmentDocumentsQueue", 0);
                var MSG = DictionaryJsonConverter.FromJsonToDictionary(Message);
                queueservice.Send(MSG); 
            }
            catch (System.Exception ex)
            {

            }

        }





    }
}
