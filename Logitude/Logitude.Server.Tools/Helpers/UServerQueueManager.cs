using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Microsoft.WindowsAzure.Storage.Blob;
using Simplog.Server.Infrastructure.Azure;
using System.IO;
using Simplog.Server.Infrastructure;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Server.Tools.Helpers
{

    public class UServerQueueManager
    {

        public enum QueueType
        {
            AsyncUrouterQueue,
            SyncUrouterQueue
        }

        static readonly string _ConnectionString;
        static readonly NamespaceManager _NamespaceManager;

        //private static string QueueName = "SampleQueue";
        //private const string SyncUrouterQueue = "SyncUrouterQueue";
        //private const string AsySyncUrouterQueue = "AsySyncUrouterQueue";


        const Int16 maxTrials = 4;

        private const string ResponseCommunicationLogId = "ResponseCommunicationLogId";
        private const string RequestCommunicationLogId = "RequestCommunicationLogId";
        private const string Tenant = "Tenant";

        static string UServerResponseArriveQ(int tenant)
        {

            return WebFreightEntryPoint.GetQueueByEnviroment(SufixTenant("UServerResponseArriveQ",tenant) );

        }

        static UServerQueueManager()
        {
            string cur = "";
            _ConnectionString = //CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
                @"Endpoint=sb://unifreighthybrid.servicebus.windows.net/;SharedSecretIssuer=owner;SharedSecretValue=yGhtL0cEjtKB61CsvEIO7yYRqyGgjSekVBj5WkvompI=";
            _NamespaceManager = NamespaceManager.CreateFromConnectionString(_ConnectionString);
            return;
            foreach (var tenant in ResolveTenantsWithQueue() )
            {
                cur = SufixTenant(QueueType.SyncUrouterQueue.ToString(), tenant);
                enshureQ(cur);
                cur = SufixTenant(QueueType.AsyncUrouterQueue.ToString(), tenant);
                enshureQ(cur);

                enshureTopicQ(tenant);
                enshureSubTopic(tenant);
            }
            
            

        }

        public UServerQueueManager(int tenant)
        {
            string cur = "";
            cur = SufixTenant(QueueType.SyncUrouterQueue.ToString(), tenant);
            enshureQ(cur);
            cur = SufixTenant(QueueType.AsyncUrouterQueue.ToString(), tenant);
            enshureQ(cur);


            enshureTopicQ(tenant );
            enshureSubTopic(tenant);
        }

        private static string SufixTenant(string qName, int tenant)
        {
            return qName + ";T" + tenant.ToString();
        }

        private static IEnumerable<int> ResolveTenantsWithQueue()
        {
            return Enumerable.Range(1, 10);
        }

        private static void enshureSubTopic(int tenant)
        {
            if (!_NamespaceManager.SubscriptionExists(UServerResponseArriveQ(tenant), GetSubscriptionName()))
            {
                _NamespaceManager.CreateSubscription(UServerResponseArriveQ(tenant), GetSubscriptionName());
            }
        }

        private static void enshureTopicQ(int tenant)
        {
            if (!_NamespaceManager.TopicExists(UServerResponseArriveQ(tenant)))
            {
                _NamespaceManager.CreateTopic(GetTopicDec(tenant));
            }
        }

        private static TopicDescription GetTopicDec(int tenant)
        {

           NetCommonHelper.Logger.DevLog.Instance.WriteDebug(string.Format("\nCreating Topic '{0}'...", UServerResponseArriveQ(tenant)));
            // Configure Topic Settings
            TopicDescription td = new TopicDescription(UServerResponseArriveQ(tenant));
            td.MaxSizeInMegabytes = 5120;
            td.DefaultMessageTimeToLive = new TimeSpan(0, 3, 0);
            return td;
        }

        private static void enshureQ(string queueName)
        {
            queueName = WebFreightEntryPoint.GetQueueByEnviroment(queueName); //GetEnvironmentQueueName(curQ);
            if (!_NamespaceManager.QueueExists(queueName))
            {
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug(string.Format("\nCreating Queue '{0}'...", queueName));
                _NamespaceManager.CreateQueue(queueName);
            }
        }
        public UServerQueueManager()
        {

            //// Set the maximum number of concurrent connections 
            //ServicePointManager.DefaultConnectionLimit = 12;

            // Create the queue if it does not exist already


            // Initialize the connection to Service Bus Queue



        }
        public void UnifreightSetResponse(string serializeReference, string xmlResponse)
        {
            int tenant = 1;//todo get from serializeReference
            var topicClient = TopicClient.CreateFromConnectionString(_ConnectionString, UServerResponseArriveQ(tenant));
            try
            {

                
                string requestCommunicationLogId;

                var myReference=UnifreightListsUtil.Deserialize(serializeReference);
                var sTenant =UnifreightListsUtil.GetValue(ref  myReference, Tenant );
                requestCommunicationLogId = UnifreightListsUtil.GetValue(ref  myReference, RequestCommunicationLogId);
                if (!int.TryParse(sTenant, out tenant))
                {
                    throw new Exception("sTenant is bad " + sTenant);  
                }
                var reqComm = Communications.GetCommunicationLog(tenant, requestCommunicationLogId);
                var myByteData = Encoding.UTF8.GetBytes(xmlResponse);


                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {

                    var newCommunication = Communications.AddCommunicationLog(new CommunicationsParams()
                    {
                        Status = "D",
                        InOut = "I",
                        From = "Unifreight",
                        CorrelationID = requestCommunicationLogId,
                        LoggingEntityId = reqComm.EntityId,
                        FolderName = "Amital",
                        LoggingUserId = reqComm.CreatedByUserId,
                        LoggingObjectTableId = reqComm.ObjectTableId,
                        LoggingEntityReference = reqComm.EntityReference,
                        ByteData = myByteData

                    });
                    var mess = new BrokeredMessage(requestCommunicationLogId);
                    //mess.MessageId = communicationLogId;
                    mess.Properties[Tenant] = tenant;
                    mess.Properties[RequestCommunicationLogId] = requestCommunicationLogId;
                    mess.Properties[ResponseCommunicationLogId] = newCommunication;
                    


                    topicClient.Send(mess);
                    scope.Complete();
                }
            }
            finally
            {
                if (topicClient != null)
                {
                    topicClient.Close();
                }
            }
        }
        public void UnifreightGetRequest(out string xmlRequest, out string serializeReference)
        {
            var sw = Stopwatch.StartNew();
            serializeReference=xmlRequest = "";
            QueueClient queueClient = QueueClient.CreateFromConnectionString(_ConnectionString, QueueType.SyncUrouterQueue.ToString());
            try
            {
                BrokeredMessage message = null;

                try
                {
                    //receive messages from Queue
                   NetCommonHelper.Logger.DevLog.Instance.WriteDebug("<<<<Receiving message from SyncUrouterQueue...");
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        message = queueClient.Receive(TimeSpan.FromMilliseconds(10));

                        if (message == null)
                        {
                            queueClient.Close();
                            queueClient = null;
                           NetCommonHelper.Logger.DevLog.Instance.WriteDebug("<<<<Receiving message from AsyncUrouterQueue...");
                            queueClient = QueueClient.CreateFromConnectionString(_ConnectionString, QueueType.AsyncUrouterQueue.ToString());
                            message = queueClient.Receive(TimeSpan.FromMilliseconds(10));
                        }
                        if (message != null)
                        {
                           NetCommonHelper.Logger.DevLog.Instance.WriteDebug(string.Format("Message received: Id = {0}, Body = {1}", message.MessageId, message.GetBody<string>()));


                            //tomer bl
                            // Further custom message processing could go here...
                            //xmlRequest = message.GetBody<string>();


                            var tenant = (int)message.Properties[Tenant];
                            var requestCommunicationLogId = message.Properties[RequestCommunicationLogId] as string;

                            var comm = Communications.GetCommunicationLog(tenant, requestCommunicationLogId);

                            xmlRequest = Communications.GetData(comm); ;


                            var myRef = new Dictionary<string, string>();
                            myRef[Tenant] = tenant.ToString();
                            myRef[RequestCommunicationLogId] = requestCommunicationLogId.ToString();
                            serializeReference = UnifreightListsUtil.Serialize(myRef);

                            message.Complete();

                        }
                    }

                }
                catch (MessagingException e)
                {
                    if (!e.IsTransient)
                    {
                       NetCommonHelper.Logger.DevLog.Instance.WriteDebug(e.Message);
                        throw;
                    }
                    else
                    {
                       NetCommonHelper.Logger.DevLog.Instance.WriteDebug(e.Message);
                       NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Will retry sending the message in 2 seconds");
                        throw;
                    }
                }

            }
            finally
            {
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug(">>>>>>queueClient.Close()");
                queueClient.Close();
            }
            

        }

        


        public void SendSync(string communicationLogId, int tenant, out string responseCommunicationLogId)
        {
            QueueClient queueClient = null;
            SubscriptionClient topicSubscriptionClient = null;
            responseCommunicationLogId = "";
            BrokeredMessage tMessage = null;
            try
            {
                queueClient = QueueClient.CreateFromConnectionString(_ConnectionString, QueueType.SyncUrouterQueue.ToString());
                var mess = new BrokeredMessage(communicationLogId);
                mess.MessageId = communicationLogId;
                mess.Properties[Tenant] = tenant;
                mess.Properties[RequestCommunicationLogId] = communicationLogId;
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug(string.Format("Sending Message SyncUrouterQueue sent: Id = {0}, Body = {1}", mess.MessageId, mess.GetBody<string>()));


                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    queueClient.Send(mess);
                    scope.Complete();
                }

                var sw = Stopwatch.StartNew();

               NetCommonHelper.Logger.DevLog.Instance.WriteDebug("<<<<Receiving message from UServerResponseArriveQ...");

                topicSubscriptionClient = SubscriptionClient.CreateFromConnectionString(_ConnectionString, UServerResponseArriveQ(tenant), GetSubscriptionName());
                bool done = false;
                while (sw.Elapsed < TimeSpan.FromMinutes(3))
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        if (tMessage != null)
                        {
                            tMessage = topicSubscriptionClient.Receive(TimeSpan.FromSeconds(5));
                            scope.Complete();
                        }
                    }

                    if (tMessage != null)
                    {
                       NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Topic Recived:" + tMessage.MessageId);

                        int currTenant = (int)tMessage.Properties[Tenant];
                        if (tMessage.Properties[RequestCommunicationLogId] == communicationLogId && currTenant == tenant)
                        {
                            
                            
                           NetCommonHelper.Logger.DevLog.Instance.WriteDebug("My !!");
                            responseCommunicationLogId = tMessage.Properties[ResponseCommunicationLogId] as string;

                            //var responseComm = Communications.GetCommunicationLog(tenant, responseCommunicationLogId);
                            //var xmlResponse = Communications.GetData(responseComm); ;

                            break;

                        }
                       NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Other !!");
                        tMessage = null;
                    }
                }
                throw new Exception("Not recived :Timeout");
            }
            catch (MessagingException e)
            {
                if (!e.IsTransient)
                {
                   NetCommonHelper.Logger.DevLog.Instance.WriteDebug(e.Message);
                    throw;
                }
                else
                {
                    //HandleTransientErrors(e);
                    //If transient error/exception, let's back-off for 2 seconds and retry
                   NetCommonHelper.Logger.DevLog.Instance.WriteDebug(e.Message);
                   NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Will retry sending the message in 2 seconds");
                    //Thread.Sleep(2000);
                    throw;
                }
            }

            finally
            {
                if (queueClient != null)
                {
                    queueClient.Close();
                }
                if (topicSubscriptionClient == null)
                {
                    topicSubscriptionClient.Close();
                }
            }

        }

        private static string GetSubscriptionName()
        {
            return "DummyTopicSubscriptionName";
        }






    }
}
