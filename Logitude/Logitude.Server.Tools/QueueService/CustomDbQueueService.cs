using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using Simplog.Data.InfrastructureModel.Repositories;
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
            int maxDeliveryCount = 50,
            int timeOutInHour = 24)
            : base(queueCode, tenant)
        {
            CustomDbQueueParams = new CustomDbQueueModel();
            CustomDbQueueParams.QueueCode = queueCode;
            CustomDbQueueParams.Tenant = tenant;
            CustomDbQueueParams.LockDuration = TimeSpan.FromMinutes(lockDurationInMin);//due debug + raise after 5 min !!
            //_QueueDescription.MaxDeliveryCount = 100;
            CustomDbQueueParams.MaxDeliveryCount = maxDeliveryCount;
            CustomDbQueueParams.TimeOutInHour = timeOutInHour;


        }

        public CustomDbQueueService(CustomDbQueueModel CustomDbQueueParams, CustomDBQueueMessage customDBQueueMessage)
            : base(CustomDbQueueParams.QueueCode, CustomDbQueueParams.Tenant)
        {
            // TODO: Complete member initialization
            this.CustomDbQueueParams = CustomDbQueueParams;
            CurrentCustomQueueResponse = customDBQueueMessage;
            base.CurrentMessageId = CurrentCustomQueueResponse.MessageId;
        }



        new private QueueResponse Receive() { throw new NotImplementedException(); }
        new private QueueResponse Receive(TimeSpan serverWaitTime) { throw new NotImplementedException(); }
        new private void Complete(string messageId) { throw new NotImplementedException(); }


        new public CustomDBQueueMessage Receive(int? nextRunDelayInSec = null)
        {
            //var r= new DualRepository()
            if (CurrentCustomQueueResponse != null && !String.IsNullOrWhiteSpace(CurrentCustomQueueResponse.MessageId) && CurrentCustomQueueResponse.QueueStatus == QueueStatusEnum.Received)
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
            LogMessagingUtil.Instance.AppendLine("CustomDbQueueService:Receive:DbQueueName=" + CustomDbQueueParams.QueueCode + ":QMId=" + base.CurrentMessageId);
            return CurrentCustomQueueResponse;
        }


        new private void Send(Dictionary<string, string> messageValues, TimeSpan? delayTime = null, string CustomerId = null, string BatchNumber = null, DateTime? NextRunDate = null)
        { throw new NotImplementedException(); }

        public int? Send(Dictionary<string, string> messageValues, int tenant, TimeSpan? delayTime, /*int tenantPriority, */
            QueueSendModel queueSendModel = null)
        {
            //int tenantPriority=8;

            var queueId = base.SendReturnId(messageValues, tenant, delayTime, null, null, null, /*tenantPriority,*/ queueSendModel);
            LogMessagingUtil.Instance.AppendLine("CustomDbQueueService:CreateNew:DbQueueName=" + CustomDbQueueParams.QueueCode + "QMId=" + queueId);
            return queueId;
        }

        public void SafeComplete()
        {
            this.Complete();
            CurrentCustomQueueResponse.QueueStatus = QueueStatusEnum.Complete;
            LogMessagingUtil.Instance.AppendLine("CustomDbQueueService:SafeComplete:DbQueueName=" + CustomDbQueueParams.QueueCode + "QMId=" + base.CurrentMessageId);

        }
        public bool SafeAbandon()
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
            return safcomplete;
        }
        public CustomDBQueueMessage CurrentCustomQueueResponse { get; set; }

        public CustomDBQueueMessage GetRabbitMQPseudoByMessageId(long messageId)
        {
            var messagesRepository = new QueueMessageRepository(Tenant);

            var q = messagesRepository.GetSingleQueueMessage(messageId);
            if (q?.Id == null)
            {
                return null;
            }
            Dictionary<string, string> messageValues = DictionaryJsonConverter.FromJsonToDictionary(q.MessageBody);
            ;
            QueueResponse myQueueResponse = new QueueResponse()
            {
                MessageId = messageId.ToString(),
                RetryNumber = q.RetryNumber,
                MessageValues = messageValues

            };
            CurrentCustomQueueResponse = new CustomDBQueueMessage(myQueueResponse, CustomDbQueueParams);
            CurrentCustomQueueResponse.QueueStatus = QueueStatusEnum.Received;
            LogMessagingUtil.Instance.AppendLine("CustomDbQueueService:Receive:DbQueueName=" + CustomDbQueueParams.QueueCode + ":QMId=" + base.CurrentMessageId);
            return CurrentCustomQueueResponse;
        }


        public static void SendCommunicationLogMessageToQueue(string queueName, string communicationLogId, int tenant, bool UseRabbitMQ)
        {
            try
            {

                SendCommunicationLogMessageToQueue(queueName, new Dictionary<string, string>() { { "CommunicationLogId", communicationLogId }, { "Tenant", tenant.ToString() } }, tenant, UseRabbitMQ);
                //IQueueService queueservice = new DbQueueService();
                //queueservice.InitializeQueue(queueName, 0);
                //queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", communicationLogId }, { "Tenant", tenant.ToString() } }, tenant);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, $"SendCommunicationLogMessageToQueue{queueName}", null, null);
            }
        }

        public static bool IsFeatureOnRABBITMQ_Communication()
        {
            int Tenant = 1;
            //INSERT INTO "TOGGLES" (CODE, NAME, SEARCHFIELDS) VALUES ('MQC', 'RABBITMQ Communication', 'MQC,RABBITMQ Communication')
            //INSERT INTO "FEATURETOGGLES"(ID, TENANT, CREATEDATE, CREATEDBYUSERID, UPDATEDATE, UPDATEDBYUSERID, SEARCHFIELDS, TENANTNUMBER, INACTIVE, TOGGLECODE) VALUES('-2', '3', TO_TIMESTAMP('2022-03-01 14:19:28.729000000', 'YYYY-MM-DD HH24:MI:SS.FF'), '1-9', TO_TIMESTAMP('2022-03-01 14:19:46.456000000', 'YYYY-MM-DD HH24:MI:SS.FF'), '1-9', 'MQC', '3', '0', 'MQC')

            ///Bug 75132: העלאת מסמך ללא קישור - מסמך נשלח למכס מס' פעמים
            return Server.Tools.Helpers.FeatureToggleHelper.HasFeatureToggle("MQC", Tenant);
        }
    



        public static void SendCommunicationLogMessageToQueue(string queueName, Dictionary<string, string> messageValues, int tenant,bool UseRabbitMQ)
        {
            try
            {

                UseRabbitMQ = IsFeatureOnRABBITMQ_Communication() && UseRabbitMQ;



                var customDbQueueService = new CustomDbQueueService(queueName,tenant);
                customDbQueueService.Send(messageValues, tenant, null, new QueueSendModel() { UseRabbitMQ = UseRabbitMQ });
                //var queueservice = new DbQueueService();
                //queueservice.InitializeQueue(queueName, 0);
                //queueservice.Send(messageValues, tenant);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, $"SendCommunicationLogMessageToQueue{queueName}", null, null);
            }
        }
    }
    public class CustomDbQueueModel
    {
     
        public TimeSpan LockDuration { get; set; }

        public int MaxDeliveryCount { get; set; }

        public int TimeOutInHour { get; set; }

        public string QueueCode { get; set; }
    
public  int Tenant { get; set; }}
}
