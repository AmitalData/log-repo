#if false


using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CustomsWorkerRole.Queue;
using UnifreightIIG.Common.Utils;
using Logitude.Server.Tools;
using Logitude.CustomsMessaging.MessagingServices;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.BL.Messaging.Customs;
using System.Diagnostics;

namespace CustomsWorkerRole
{
    public class CustomsMessagingOutWR : WorkerEntryPoint
    {
        bool _OnStartDone = false;
        QueueDescription _QueueDescription;
        QueueClient _QueueClient;
        private QueueClient _DeadletterQueueClient;

        public override void Run()
        {

            while (true)
            {

                if (General.IsUpdating())
                {
                    Thread.Sleep(60000);
                    continue;
                }
                try
                {
                    WorkOnce();
                }
                catch (Exception e)
                {
                    ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingOutWR : Run() Method");
                    Thread.Sleep(10000);
                }

            }

        }

        public override bool OnStart()
        {
            try
            {
                if (_OnStartDone) return true;
                _OnStartDone = true;

                MessagingServiceFactoryHelper.InitContainer();
                if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>("2715"))
                {
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingOutWR: ProcessMessage():!ContainerAccessor.Container.IsRegistered :analyzeClass=2715" );
                    //message.DeadLetter();
                    ///return;
                }








                string emailQueueName = ThreadedRoleEntryPoint.GetQueueByEnviroment(Logitude.Server.Tools.Helpers.SBQueueNames.CustomsMessagingOutBQ.ToString()); //Amitalqueue

                if (!StorageAcountDetails.NameSpaceManager.QueueExists(emailQueueName))
                {
                    _QueueDescription = new QueueDescription(emailQueueName);
                    _QueueDescription.MaxSizeInMegabytes = 5120;
                    _QueueDescription.LockDuration = TimeSpan.FromMinutes(5);//due debug + raise after 5 min !!

                    StorageAcountDetails.NameSpaceManager.CreateQueue(_QueueDescription);
                }
                string deadLetterQueuePath = QueueClient.FormatDeadLetterPath(Logitude.Server.Tools.Helpers.SBQueueNames.CustomsMessagingOutBQ.ToString());

                _DeadletterQueueClient = StorageAcountDetails.CreateServiceBusQueueClient(deadLetterQueuePath);

                _QueueClient = StorageAcountDetails.CreateServiceBusQueueClient(emailQueueName);
                if (this.DebugMode)
                {
                    
                    //SendDebugTest2715();
                    //SendDebugTest1000();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "amital send data worker role start", null);
            }

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

   
        void SendDebugTest2715()
        {
            var Tenant = 1;

            var req = new Logitude.CustomsMessaging.Common.RequestParams.GenericRequestParams()
            {
                AppicationId = "1-8523",
                Tenant = Tenant,
                LoggingObjectTableId = "1-4985",
                LoggingEnabled = true,
                LoggingEntityId = "1-121",
                LoggingUserId = ""
            };

            req.LoggingEnabled = true;
            req.LoggingEntityId = "1-8523";
            req.LoggingEntityReference = "14010213755911";
            req.LoggingObjectTableId = "1-4985";
            req.LoggingUserId = "1-5341";
            req.RequestName = "CustomsDocument Request";
            req.ResponseName = "CustomsDocument Response";
            req.Tenant = 1;
            req.AppicationId = "1-8523";






            var messService = new Logitude.CustomsMessaging.MessagingServices.D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityMessagingService();
            var correlationId = messService.CreateSBQMessage(req);

        }

        void SendDebugTest1000()
        {
            var Tenant = 1;

            var req = new Logitude.CustomsMessaging.Common.RequestParams.GenericRequestParams()
            {
                AppicationId = "1-50",
                Tenant = Tenant,
                LoggingObjectTableId = "1-4852",
                LoggingEnabled = true,
                LoggingEntityId = "1-121",
                LoggingUserId = ""
            };

            req.LoggingEnabled = true;
            req.LoggingEntityId = "1-50";
            req.LoggingEntityReference = "14010213755911";
            req.LoggingObjectTableId = "1-4852";
            req.LoggingUserId = "1-5341";
            req.RequestName = "Declaration Request";
            req.ResponseName = "Declaration Response";
            req.Tenant = 1;
            req.AppicationId = "1-50";

            var messService = new Logitude.CustomsMessaging.MessagingServices.DF_MSG10000_ImportDeclarationMessagingService();
            var correlationId = messService.CreateSBQMessage(req);

        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {

            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {

                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }



        public override void WorkOnce()
        {
            OnStart();
            WorkUntilQEmpty();
        }

        void WorkUntilQEmpty()
        {
            List<long> deferredSequenceNumbers = new List<long>();
            bool proccesDone = false;
            for (int filtterPriority = 2; filtterPriority < 3; filtterPriority++)
            {
                while (true)
                {
                    BrokeredMessage receivedMessage = _QueueClient.Receive(TimeSpan.FromSeconds(1));

                    if (receivedMessage == null)
                    {
                        break;
                    }
                    if (this.DebugMode)
                    {
                        if (!receivedMessage.GetProperty<bool>(QueueExt.QueuePropertyNames.DebugMode, false))
                        {
                            ///receivedMessage.SafeAbandon();
                            continue;
                        }
                    }
                    if (receivedMessage.DeliveryCount > 6) // default max DeliveryCount ==10
                    {
                        receivedMessage.DeadLetter();
                        receivedMessage.Dispose();
                    }


                    var lastExecAt = receivedMessage.GetProperty<DateTime>(QueueExt.QueuePropertyNames.LastExecAt, DateTime.MinValue);


                    if (receivedMessage.GetProperty<int>(QueueExt.QueuePropertyNames.Priority, 1) > filtterPriority
                        )
                    {
                        receivedMessage.SafeAbandon();
                        continue;
                    }
                    if (DateTime.UtcNow.Subtract(lastExecAt) < TimeSpan.FromMinutes(2))
                    {
                        // lock message for 5 min 
                        continue;
                    }

                    proccesDone = true;
                    ProcessMessage(receivedMessage);
                }
            }

            // Process the low-priority messages: 
            foreach (long sequenceNumber in deferredSequenceNumbers)
            {
                ProcessMessage(_QueueClient.Receive(sequenceNumber));
            }


            if (proccesDone) return;
            return;
            while (true)
            {
                BrokeredMessage msg = _DeadletterQueueClient.Receive();

                if (msg == null)
                {
                    break;
                }

                //Console.WriteLine("Deadlettered message.");
                //Console.WriteLine("MessageId:                  {0}", msg.MessageId);
                //Console.WriteLine("DeliveryCount:              {0}", msg.DeliveryCount);
                //Console.WriteLine("EnqueuedTimeUtc:            {0}", msg.EnqueuedTimeUtc);
                //Console.WriteLine("Size:                       {0} bytes", msg.Size);
                //Console.WriteLine("DeadLetterReason:           {0}",
                //    msg.Properties["DeadLetterReason"]);
                //Console.WriteLine("DeadLetterErrorDescription: {0}",
                //    msg.Properties["DeadLetterErrorDescription"]);
                //Console.WriteLine();
                msg.Complete();
            }


        }

        private void ProcessMessage(BrokeredMessage message)
        {

            try
            {

                var analyzeClass = message.GetProperty<string>(QueueExt.QueuePropertyNames.InterfaceTypeCode, "");//, "Logitude.CustomsMessaging.MessagingServices.DF_MSG10000_ImportDeclarationMessagingService");
                if (String.IsNullOrWhiteSpace(analyzeClass))
                {
                    message.SafeComplete();
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingOutWR: ProcessMessage() Method :analyzeClass ==null");
                    //message.DeadLetter();
                    return;//
                }

                //var reqXml = message.GetBody<string>();

                //if (String.IsNullOrWhiteSpace(reqXml))
                //{
                //    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingOutWR: ProcessMessage() Method:reqXml=null");
                //    //message.DeadLetter();
                //    return;
                //}
                if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(analyzeClass))
                {
                    message.SafeComplete();
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingOutWR: ProcessMessage():!ContainerAccessor.Container.IsRegistered :analyzeClass=" + analyzeClass);
                    //message.DeadLetter();
                    return;
                }
                //var id = message.CorrelationId ?? message.SequenceNumber.ToString() ; //.GetHashCode().ToString();
                //var messagingBatchService = MessagingBatchService.TryGet(id);
                //ProcessState processState = ProcessState.Start;
                var currProcessState = message.GetProperty<int>(QueueExt.QueuePropertyNames.ProcessState, (int)CustomsStepEnum.StartRequestParams);

                var newProcessState = CustomsStepEnum.StartRequestParams;
                //Enum.TryParse<ProcessState>(
                newProcessState = (CustomsStepEnum)currProcessState;
                var anaO = ContainerAccessor.Container.Resolve<IMessagingServiceInterfaceType>(analyzeClass);

                //anaO.SendBatchStateMachine(reqXml, messagingBatchService);

                anaO.SendBatchStateMachine(1, message.CorrelationId, ref newProcessState);
                message.SafeComplete();
                return;
                if ((int)newProcessState == (int)CustomsStepEnum.AnalyzeResponseData)
                {
                    message.SafeComplete();
                }
                else
                {
                    if ((int)newProcessState != (int)currProcessState)
                    {
                        message.SetProperty<int>(QueueExt.QueuePropertyNames.ProcessState, (int)newProcessState);
                        message.Abandon(message.Properties);
                    }
                    else
                    {
                        message.Abandon();
                    }

                }

            }
            catch (Exception ex)
            {

                if (message.Properties.Keys.Contains("CommunicationLogId"))
                {

                    message.SafeComplete();
                    if (false)
                    {
                        string communicationLogId = message.Properties["CommunicationLogId"].ToString();
                        if (communicationLogId != null)
                        {
                            message.SafeAbandon();
                        }
                        else
                        {
                            message.SafeComplete();
                        }    
                    }  
                    
                }
                else
                {
                    message.SafeComplete();
                }
                //throw;
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingOutWR: ProcessMessage() Method");
            }
        }

    }
}

#endif