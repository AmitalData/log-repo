
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
//using CustomsWorkerRole.Queue;
using UnifreightIIG.Common.Utils;
using Logitude.Server.Tools;
//using Logitude.CustomsMessaging.MessagingServices;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.BL.Messaging.Customs;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
//using CustomsWorkerRole.Utils;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.BL.Utils;

namespace CommunicationWorkerRole.Accounting
{
    public class ARPaymentChequeRedemptionWorkerRole : WorkerEntryPoint
    {
        bool _OnStartDone = false;
        QueueDescription _QueueDescription;
        QueueClient _QueueClient;
        private QueueClient _DeadletterQueueClient;

        public override void Run()
        {

            while (IsRunning)
            {

                if (General.IsUpdating())
                {
                    Thread.Sleep(60000);
                    continue;
                }
                try
                {
                    WorkOnce();
                    Thread.Sleep(
                        TimeSpan.FromSeconds(
                        //1
                        .5
                        )

                        );
                }
                catch (Exception e)
                {
                    ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "ARPaymentChequeRedemptionWorkerRole : Run() Method", null);
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








                var myClass = this.GetType().Name;
                string emailQueueName = ThreadedRoleEntryPoint.GetQueueByEnviroment(myClass); //Amitalqueue

                if (!StorageAcountDetails.NameSpaceManager.QueueExists(emailQueueName))
                {
                    _QueueDescription = new QueueDescription(emailQueueName);
                    _QueueDescription.MaxSizeInMegabytes = 5120;
                    _QueueDescription.LockDuration = TimeSpan.FromMinutes(5);//due debug + raise after 5 min !!
                    //_QueueDescription.MaxDeliveryCount = 100;
                    _QueueDescription.MaxDeliveryCount = 20;

                    StorageAcountDetails.NameSpaceManager.CreateQueue(_QueueDescription);
                }
                string deadLetterQueuePath = QueueClient.FormatDeadLetterPath(Logitude.Server.Tools.Helpers.SBQueueNames.CustomsMessagingSheetBQ.ToString());

                _DeadletterQueueClient = StorageAcountDetails.CreateServiceBusQueueClient(deadLetterQueuePath);


                _QueueClient = StorageAcountDetails.CreateServiceBusQueueClient(emailQueueName);

                if (this.DebugMode)
                {
                    //TestCache();
                    //TestCL_MSG101_GetCustomerByEntityCustomerIdentificationMassagingService();
                    //Test3053();
                    //TestTableDCA();
                    ///TestCustomsRequestsSheetService10000();
                    //TestCustomsRequestsSheetService2715();
                    //SendDebugTest2715();
                    //SendDebugTest1000();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "amital send data worker role start", null, null);
            }

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
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


        DateTime _LastGC = DateTime.MinValue;
        public override void WorkOnce()
        {
            try
            {
                if (DateTime.Now.Subtract(_LastGC) > TimeSpan.FromMinutes(10))//cache 20 min
                {
                    _LastGC = DateTime.Now;
                    //CacheManager.ClearCacheItems();
                    //CustomsWorkerRole.Utils.GenUtil.CollectGC();

                    ///_AllCustomsSetting.Clear();
                    //GenUtil.CollectGC();
                }

                OnStart();
                WorkUntilQEmpty();

            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole" + this.GetType().Name, " : Run() Method", null);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                _OnStartDone = false;
            }

        }

        void WorkUntilQEmpty()
        {
            BrokeredMessage receivedMessage = null;
            List<long> deferredSequenceNumbers = new List<long>();
            bool proccesDone = false;
            for (int filtterPriority = 2; filtterPriority < 3; filtterPriority++)
            {
                while (IsRunning)
                {
                    //throw new Exception("BrokeredMessage receivedMessage = _QueueClient.Receive(TimeSpan.FromSeconds(5));");
                    try
                    {
                        receivedMessage = _QueueClient.Receive(TimeSpan.FromSeconds(5));
                    }
                    catch (Exception)
                    {

                        throw;
                    }


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
                    //if (receivedMessage.DeliveryCount > 6) // default max DeliveryCount ==10

                    //if (receivedMessage.DeliveryCount > DefaultMessageController.MaxToRetry) 
                    //{
                    //    receivedMessage.SafeComplete();
                    //    return;
                    //    receivedMessage.DeadLetter();
                    //    receivedMessage.Dispose();
                    //    return;
                    //}


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

            //// Process the low-priority messages: 
            //foreach (long sequenceNumber in deferredSequenceNumbers)
            //{
            //    ProcessMessage(_QueueClient.Receive(sequenceNumber));
            //}

            //if (proccesDone) return;
            return;
            while (IsRunning)
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



                var tenant = message.GetProperty<int>(QueueExt.QueuePropertyNames.Tenant, -1);
                if (tenant == -1)
                {
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "ARPaymentChequeRedemptionWorkerRole: ProcessMessage() Method :tenant==-1", null);
                    return;
                }
                var correlationId = message.CorrelationId;
                LogMessagingUtil.Instance.AppendLine("receivedMessage.DeliveryCount =" + message.DeliveryCount.ToString());



                PostDatedChequesRedemptionBatch postDatedChequesRedemptionBatch = new PostDatedChequesRedemptionBatch();
                postDatedChequesRedemptionBatch.RunOnePayableARPaymentCheque(correlationId, tenant);
                //MessagingServiceFactoryHelper.ResolveAndExecute(analyzeClass, tenant, correlationId, myCustomsCommandEnum);

                message.SafeComplete();

            }
            catch (CustomsRequestsSheetDomainModelServiceException customsRequestsSheetServiceException)
            {

                //ExceptionHandler.HandleException(customsRequestsSheetServiceException, DateTime.Now, 0, "", "WorkerRole", "ARPaymentChequeRedemptionWorkerRole: ProcessMessage() Method/CustomsRequestsSheetServiceException ", null);
                if (customsRequestsSheetServiceException.What2Do == CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue)
                {
                    message.SafeComplete();
                }
                else
                {
                    message.SetProperty<DateTime>(QueueExt.QueuePropertyNames.LastExecAt, DateTime.UtcNow);
                    message.SafeAbandon();
                }

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "ARPaymentChequeRedemptionWorkerRole: ProcessMessage() Method", null);
                message.SafeComplete();
                //throw;
            }
        }



    }
}
