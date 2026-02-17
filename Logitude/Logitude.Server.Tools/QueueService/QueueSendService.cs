using Logitude.Server.Tools.Helpers;
using Microsoft.ServiceBus.Messaging;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.QueueService
{
    public class QueueSendService
    {
        private string _CorrelationId;
        
        string _SBQueueName;
        private QueueSendModel _QueueSendModel;
       

        public QueueSendService(string SBQueueName, string correlationId, QueueSendModel queueSendModel)
        {
            // TODO: Complete member initialization
            this._SBQueueName = SBQueueName ;
            this._CorrelationId = correlationId;
            _QueueSendModel = queueSendModel;

        }


        public void Send()
        {
            if (LogitudeSettings.QueueServiceMode != "db")
            {
            var mQueue = new BrokeredMessage();
            mQueue.CorrelationId = _CorrelationId;
            
            mQueue.SetProperty<string>(QueueExt.QueuePropertyNames.InterfaceTypeCode, "Logitude.CustomsMessaging.MessagingServices.DF_MSG10000_ImportDeclarationMessagingService");
            mQueue.SetProperty<string>(QueueExt.QueuePropertyNames.InterfaceTypeCode, _QueueSendModel.InterfaceTypeCode);

            //mQueue.SetProperty<bool>(QueueExt.QueuePropertyNames.DebugMode,
            //    _QueueSendModel.DebugMode //requestParams.DebugMode
            //    );
            //mQueue.SetProperty<int>(QueueExt.QueuePropertyNames.ProcessState, (int)_QueueSendModel.ProcessState);
            mQueue.SetProperty<int>(QueueExt.QueuePropertyNames.Tenant, _QueueSendModel.Tenant);
            mQueue.SetProperty<string>(QueueExt.QueuePropertyNames.DcaAnalyzeAggregateKey, _QueueSendModel.DcaAnalyzeAggregateKey);

            string customsMessagingOutBQ = WebFreightEntryPoint.GetQueueByEnviroment(_SBQueueName.ToString()); //Amitalqueue
            var queueClient = StorageAcountDetails.CreateServiceBusQueueClient(customsMessagingOutBQ);
            var address = (queueClient.MessagingFactory).Address.ToString();
            var mess = "address:" + address + "Queue:" + customsMessagingOutBQ + "/CorrelationId=" + mQueue.CorrelationId;

            if (_QueueSendModel.Delay != null)
            {
                //  Message=Local transactions are not supported with other resource managers/DTC.
                //  Source=Microsoft.ServiceBus
                    
                var time = TenantServerConfigration.GetCurrentDateTime(_QueueSendModel.Tenant).Add(_QueueSendModel.Delay.Value);
                    
                var crashDTCWhenServiceBus = false;
                if (!crashDTCWhenServiceBus)
                {
                    //var time = TenantServerConfigration.GetCurrentDateTime(_QueueSendModel.Tenant).Add(_QueueSendModel.Delay.Value);
                    if (LogitudeSettings.DatabaseManagementSystem == "oracle")
                    {
                        mQueue.ScheduledEnqueueTimeUtc = time.ToUniversalTime();//.UtcNow;
                    }
                    else
                    {
                        LogMessagingUtil.Instance.AppendLine("***** Due bad define (Not oracle) Suppress send mQueue.ScheduledEnqueueTimeUtc = " + time.ToUniversalTime().ToString());
                    }
                }
                else
                {
                    LogMessagingUtil.Instance.AppendLine("***** Due crashDTCWhenServiceBus Suppress send mQueue.ScheduledEnqueueTimeUtc = " + time.ToUniversalTime().ToString());
                }
            }

            try
            {
                LogMessagingUtil.Instance.AppendLine(mess);
                var test = false;
                if (test)
                {
                    throw new Exception("test"); 
                }
                
                queueClient.Send(mQueue);
            }
            catch (Exception e)
            {

                Logitude.SystemLogs.ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "", "QueueSendService.Send()" + mess, null);
                throw;
            }
            }
            else
            {
                var queueService = new CustomDbQueueService//();
                //queueService.InitializeQueue
                    (this._SBQueueName, 0);
                Dictionary<string, string> messageProperties = new Dictionary<string, string>();
                //messageProperites.Add(QueueExt.QueuePropertyNames.InterfaceTypeCode.ToString(), "Logitude.CustomsMessaging.MessagingServices.DF_MSG10000_ImportDeclarationMessagingService");
                messageProperties.Add("InterfaceTypeCode", _QueueSendModel.InterfaceTypeCode);
                //messageProperties.Add("DebugMode", _QueueSendModel.DebugMode.ToString()); //requestParams.DebugMode
                //messageProperties.Add("ProcessState", _QueueSendModel.ProcessState.ToString());
                messageProperties.Add("Tenant", _QueueSendModel.Tenant.ToString());
                messageProperties.Add("CorrelationId", _CorrelationId);

                if (!String.IsNullOrWhiteSpace(_QueueSendModel.DcaAnalyzeAggregateKey))
                {
                    messageProperties.Add("DcaAnalyzeAggregateKey", _QueueSendModel.DcaAnalyzeAggregateKey);
                }
                try
                {
                    
                    var test = false;
                    if (test)
                    {
                        throw new Exception("test");
                    }
                    int? queueId = null;
                    //queueClient.Send(mQueue);
                    if (_QueueSendModel.Delay != null)
                    {
                        
                        
                        queueService.Send(messageProperties, _QueueSendModel.Delay);
                    }
                    else
                    {
                        queueId = queueService.Send(messageProperties);
                    }
                    
                    LogMessagingUtil.Instance.AppendLine("CustomDbQueueService:CreateNew:SBQueueName=" + _SBQueueName + "QMId=" + queueId);
                }
                catch (Exception e)
                {

                    Logitude.SystemLogs.ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "", "QueueSendService.Send()" + messageProperties.ToString(), null);
                    throw;
                }
            
            }
            
        }

    }
    public class QueueSendModel
    {
        //public int ProcessState { get; set; }

        public int Tenant { get; set; }

        public string InterfaceTypeCode { get; set; }

        //public bool DebugMode { get; set; }

        public TimeSpan? Delay { get; set; }

        string _DcaAnalyzeAggregateKey;

        public string DcaAnalyzeAggregateKey
        {
            get { return _DcaAnalyzeAggregateKey; }
            set { _DcaAnalyzeAggregateKey = value; }
        }

        
        
    }
}
