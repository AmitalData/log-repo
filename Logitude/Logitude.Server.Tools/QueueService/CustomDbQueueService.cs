using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.QueueService
{
   

    public class CustomDbQueueService : DbQueueService
    {
    
        CustomDbQueueModel CustomDbQueueParams;
        public CustomDbQueueService(string queueCode, int tenant, 
            int lockDurationInMin = 2, 
            int maxDeliveryCount=50,
            int timeOutInHour= 24)
            :base(queueCode, tenant)
        {
            CustomDbQueueParams= new CustomDbQueueModel();
            CustomDbQueueParams.QueueCode = queueCode;
            CustomDbQueueParams.Tenant=tenant;
            CustomDbQueueParams.LockDuration = TimeSpan.FromMinutes(lockDurationInMin);//due debug + raise after 5 min !!
            //_QueueDescription.MaxDeliveryCount = 100;
            CustomDbQueueParams.MaxDeliveryCount = maxDeliveryCount;
            CustomDbQueueParams.TimeOutInHour = timeOutInHour;
            
            
        }

        public CustomDbQueueService(CustomDbQueueModel CustomDbQueueParams, CustomDBQueueMessage customDBQueueMessage)
            :base(CustomDbQueueParams.QueueCode,CustomDbQueueParams.Tenant)
        {
            // TODO: Complete member initialization
            this.CustomDbQueueParams = CustomDbQueueParams;
            CurrentCustomQueueResponse = customDBQueueMessage;
            base.CurrentMessageId = CurrentCustomQueueResponse.MessageId;
        }

        
        
        new private QueueResponse Receive() { throw new NotImplementedException(); }
        new private QueueResponse Receive(TimeSpan serverWaitTime) { throw new NotImplementedException(); }
        new private void Complete(string messageId){throw new NotImplementedException(); }
            
        
        new public CustomDBQueueMessage Receive(int? nextRunDelayInSec = null)
        {
            //var r= new DualRepository()
            if (CurrentCustomQueueResponse != null && !String.IsNullOrWhiteSpace( CurrentCustomQueueResponse.MessageId) && CurrentCustomQueueResponse.QueueStatus == QueueStatusEnum.Received)
            {
                if (CustomDbQueueParams.MaxDeliveryCount < CurrentCustomQueueResponse.Retries)
                {
                    this.CompleteAsFailed();
                }
                else
                {
                    if (
                    DateTime.Now
                        // TO DO  get oracle SysDate 
                   .Subtract(CurrentCustomQueueResponse.MessageCreatedServerTime.Value) > TimeSpan.FromHours(CustomDbQueueParams.TimeOutInHour))
                    {
                        this.CompleteAsFailed();
                    }
                }
            }
            CurrentCustomQueueResponse = null;
            nextRunDelayInSec = nextRunDelayInSec ?? (int)(CustomDbQueueParams.LockDuration.TotalSeconds);
            base.CurrentMessageId = null;
            var q = base.Receive(nextRunDelayInSec.Value);
            if (q == null)
            {
                return null;
            }
            CurrentCustomQueueResponse = new CustomDBQueueMessage(q, CustomDbQueueParams);
            CurrentCustomQueueResponse.QueueStatus = QueueStatusEnum.Received;
            LogMessagingUtil.Instance.AppendLine("CustomDbQueueService:Receive:DbQueueName=" + CustomDbQueueParams.QueueCode + ":QMId=" + base.CurrentMessageId );
            return CurrentCustomQueueResponse;
        }


        new private void Send(Dictionary<string, string> messageValues, TimeSpan? delayTime = null, string CustomerId = null, string BatchNumber = null, DateTime? NextRunDate = null)
        { throw new NotImplementedException(); }

        public int? Send(Dictionary<string, string> messageValues, int tenant,  TimeSpan? delayTime, int tenantPriority)
        {
            //int tenantPriority=8;
            
            var queueId= base.SendReturnId(messageValues, tenant,delayTime,null,null,null, tenantPriority);
            LogMessagingUtil.Instance.AppendLine("CustomDbQueueService:CreateNew:DbQueueName=" + CustomDbQueueParams.QueueCode + "QMId=" + queueId);
            return queueId;
        }

        public void SafeComplete()
        {
            this.Complete();
            CurrentCustomQueueResponse.QueueStatus = QueueStatusEnum.Complete;
            LogMessagingUtil.Instance.AppendLine("CustomDbQueueService:SafeComplete:DbQueueName=" + CustomDbQueueParams.QueueCode + "QMId=" + base.CurrentMessageId);

        }
        public void SafeAbandon()
        {
            bool safcomplete = false;
            if (CurrentCustomQueueResponse.Retries > 10)
            {
                this.SafeComplete();
                safcomplete = true;
            }
            else
            {
                if (CurrentCustomQueueResponse.MessageCreatedServerTime.HasValue)
                {
                    if (DateTime.UtcNow.Subtract(CurrentCustomQueueResponse.MessageCreatedServerTime.GetValueOrDefault()) > TimeSpan.FromHours(12))
                    {
                        this.SafeComplete();
                        safcomplete = true;
                    }
                }
            }
            if (!safcomplete)
            {
                if (CurrentCustomQueueResponse.Retries < 8)
                {
                    this.Delay(TimeSpan.FromMinutes(1));
                }
                else
                {
                    this.Delay(TimeSpan.FromMinutes(10));
                }
                
            }
            
            //else if (CurrentCustomQueueResponse.Retries > 10)
            //{
            //    this.Delay(TimeSpan.FromMinutes(60));
            //}
            //else if (CurrentCustomQueueResponse.Retries > 5)
            //{
            //    this.Delay(TimeSpan.FromMinutes(10));
            //}
            //this.SafeComplete();

            CurrentCustomQueueResponse.QueueStatus = QueueStatusEnum.DeadLetter;
            LogMessagingUtil.Instance.AppendLine("CustomDbQueueService:SafeAbandon:DbQueueName=" + CustomDbQueueParams.QueueCode + "QMId=" + base.CurrentMessageId);
            //this.Return();
        }
        public CustomDBQueueMessage CurrentCustomQueueResponse { get; set; }
        
    }
    public class CustomDbQueueModel
    {
     
        public TimeSpan LockDuration { get; set; }

        public int MaxDeliveryCount { get; set; }

        public int TimeOutInHour { get; set; }

        public string QueueCode { get; set; }
    
public  int Tenant { get; set; }}
}
