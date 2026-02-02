using System;
using System.Collections.Generic;

namespace Logitude.Server.Tools.QueueService
{
    public interface IQueueService
    {
        void InitializeQueue(string queueCode, int tenant,string queueDefinitionGroup = null);
        void Send(Dictionary<string, string> messageValues, int tenant, TimeSpan? delayTime = null, string CustomerId = null, string BatchNumber = null, DateTime? NextRunDate = null);
        QueueResponse Receive(TimeSpan? serverWaitTime = null);

        QueueResponse ReceiveDetailsByTenant(string objectTable, TimeSpan? serverWaitTime = null);
        void Complete();
        void Delay(TimeSpan delayTime);
        void Return();
        void CompleteAsFailed();
       
    }

    public partial class QueueResponse
    {
        public string MessageId { get; set; }
        public int RetryNumber { get; set; }
        public int Tenant { get; set; }
        public IDictionary<string, string> MessageValues { get; set; }
    }

    public partial class QueueResponse
    {
        public DateTime? MessageCreatedServerTime { get; set; }
    }

    public partial class CustomDBQueueMessage
    {
        //private QueueResponse q;
        private CustomDbQueueModel CustomDbQueueParams;

        public CustomDBQueueMessage(QueueResponse baseQueueResponse)
        {
            if (baseQueueResponse == null)
            {
                return;
            }

            MyQueueResponse= baseQueueResponse;

            this.MessageId = baseQueueResponse.MessageId;
            this.Retries = baseQueueResponse.RetryNumber;
            this.Properties = baseQueueResponse.MessageValues;
            this.MessageCreatedServerTime = baseQueueResponse.MessageCreatedServerTime;
        }

        public CustomDBQueueMessage(QueueResponse q, CustomDbQueueModel CustomDbQueueParams)
            :this(q)
        {
            
            this.CustomDbQueueParams = CustomDbQueueParams;
        }
         public QueueResponse MyQueueResponse { get; private set; }
 
         public DateTime? MessageCreatedServerTime { get; set; }
        public QueueStatusEnum QueueStatus { get; set; }

        public IDictionary<string, string> Properties { get; set; }

        public int Retries { get; set; }

        public string MessageId { get; set; }

        public bool SafeAbandon()
        {

            var myCustomDbQueueService = new CustomDbQueueService(CustomDbQueueParams,this);
            return myCustomDbQueueService.SafeAbandon();
        }
        public void SafeComplete()
        {
            var myCustomDbQueueService = new CustomDbQueueService(CustomDbQueueParams, this);
            myCustomDbQueueService.SafeComplete();
        }
    }
    public enum QueueStatusEnum
    {
        none=0,
        Received,
        DeadLetter,
        Complete
    }
}
