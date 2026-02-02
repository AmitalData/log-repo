using Microsoft.ServiceBus.Messaging;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.Server.Tools.QueueService
{
    public class AzureQueueService : IQueueService
    {
        private int Tenant { get; set; }
        private string QueueCode { get; set; }
        private QueueClient QueueClient { get; set; }
        private BrokeredMessage CurrentMessage { get; set; }
        public void InitializeQueue(string queueCode, int tenant, string queueDefinitionGroup = null)
        {
            this.Tenant = tenant;
            this.QueueCode = queueCode;
            this.QueueClient = Communications.GetQueueClient(this.QueueCode);
        }

        public void Send(Dictionary<string, string> messageValues, int tenant, TimeSpan? delayTime = null, string CustomerId = null, string BatchNumber = null, DateTime? NextRunDate = null)
        {
            using (TransactionScope scope = TransactionFactory.GetNewSerializableTransaction())
            {
                BrokeredMessage message = new BrokeredMessage();
                foreach (string key in messageValues.Keys)
                    message.Properties[key] = messageValues[key];
                if (delayTime != null)
                {
                    message.ScheduledEnqueueTimeUtc = DateTime.UtcNow.Add(delayTime.Value);
                }

                this.QueueClient.Send(message);

                scope.Complete();
            }
        }

        public QueueResponse Receive(TimeSpan? serverWaitTime = null)
        {
            if (serverWaitTime == null) { serverWaitTime = TimeSpan.FromSeconds(5); }
            using (TransactionScope scope = TransactionFactory.GetNewSerializableTransaction())
            {
                QueueResponse response = new QueueResponse();
                if (this.CurrentMessage == null)
                {
                    BrokeredMessage message = this.QueueClient.Receive(serverWaitTime.Value);
                    if (message != null)
                    {
                        response.MessageId = message.MessageId;
                        response.MessageValues = message.Properties.ToDictionary(pair => pair.Key, pair => (pair.Value != null ? pair.Value.ToString() : null));
                        response.RetryNumber = message.DeliveryCount;
                        this.CurrentMessage = message;
                    }
                }

                scope.Complete();

                return response;
            }
        }

        public QueueResponse ReceiveDetailsByTenant(string objectTable, TimeSpan? serverWaitTime = null)
        {
            throw new NotImplementedException();
        }

        public void Complete()
        {
            using (TransactionScope scope = TransactionFactory.GetNewSerializableTransaction())
            {
                if (this.CurrentMessage != null)
                {
                    this.CurrentMessage.Complete();
                    this.CurrentMessage = null;
                }

                scope.Complete();
            }
        }

        public void Delay(TimeSpan delayTime)
        {
            using (TransactionScope scope = TransactionFactory.GetNewSerializableTransaction())
            {
                if (this.CurrentMessage != null)
                {
                    this.CurrentMessage.ScheduledEnqueueTimeUtc = DateTime.UtcNow.Add(delayTime);

                    this.CurrentMessage.Abandon();
                    this.CurrentMessage = null;
                }
                scope.Complete();
            }
        }

        public void Return()
        {
            using (TransactionScope scope = TransactionFactory.GetNewSerializableTransaction())
            {
                if (this.CurrentMessage != null)
                {
                    this.CurrentMessage.Abandon();
                    this.CurrentMessage = null;
                }

                scope.Complete();
            }
        }
 

        public void CompleteAsFailed()
        {
            //throw new NotImplementedException();
        }
 
    }
}
