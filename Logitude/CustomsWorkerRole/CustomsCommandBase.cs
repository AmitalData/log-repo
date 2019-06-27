
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
//using Microsoft.WindowsAzure.ServiceRuntime;
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
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using CustomsWorkerRole.Utils;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Server.Infrastructure;
using System.Transactions;

namespace CustomsWorkerRole
{
    public class CustomsCommandBase : CustomsWorkerEntryPoint
    {
        public CustomsCommandBase()
        {

        }
        public CustomsCommandBase(string queueNameOverride)
        {
            _QueueNameOverride = queueNameOverride;
        }
        bool _OnStartDone = false;
        QueueDescription _QueueDescription;
        QueueClient _QueueClient;
        private QueueClient _DeadletterQueueClient;
        protected CustomDbQueueService _CustomDbQueueService;
        public override void Run()
        {

            while (true)
            {

                if (General.IsUpdating())
                {
                    Thread.Sleep(600);
                    continue;
                }
                try
                {
                    WorkOnce();
                    Thread.Sleep(
                        TimeSpan.FromSeconds(
                        //1
                        1
                        )

                        );
                }
                catch (Exception e)
                {
                    ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR : Run() Method", null);
                    Thread.Sleep(1000);
                }

            }

        }

        public override bool OnStart()
        {
            //try
            //{
            if (_OnStartDone) return true;
            _OnStartDone = true;

            var myClass = this.GetType().Name;
            if (!string.IsNullOrWhiteSpace(_QueueNameOverride))
            {
                myClass = _QueueNameOverride;
            }
            string emailQueueName = ThreadedRoleEntryPoint.GetQueueByEnviroment(myClass); //Amitalqueue

            if (LogitudeSettings.QueueServiceMode != "db")
            {

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

                //RoleEnvironment.Changing += RoleEnvironmentChanging;
            }
            else
            {
                _CustomDbQueueService = new CustomDbQueueService(myClass, 0);
            }


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
            //}
            //catch (Exception ex)
            //{
            //    ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "amital send data worker role start", null, null);
            //}

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            

            return base.OnStart();
        }






        //private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        //{

        //    // If a configuration setting is changing
        //    if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
        //    {

        //        // Set e.Cancel to true to restart this role instance
        //        e.Cancel = true;
        //    }
        //}


        DateTime _LastGC = DateTime.MinValue;
        public override void WorkOnce()
        {
            try
            {
                if (DateTime.Now.Subtract(_LastGC) > TimeSpan.FromMinutes(10))//cache 20 min
                {
                    _LastGC = DateTime.Now;
                    CacheManager.ClearCacheItems();
                    CustomsWorkerRole.Utils.GenUtil.CollectGC();

                    ///_AllCustomsSetting.Clear();
                    GenUtil.CollectGC();
                }

                OnStart();
                if (LogitudeSettings.QueueServiceMode != "db")
                {
                    WorkUntilQEmpty();
                }
                else
                {
                    WorkUntilQEmpty_Db();
                }

            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole" + this.GetType().Name, " : Run() Method", null);
                Thread.Sleep(TimeSpan.FromSeconds(1));
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
                while (true)
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

        protected virtual bool ProcessMessage(BrokeredMessage message, OverrideControllerModel controller = null)
        {

            try
            {

                var analyzeClass = message.GetProperty<string>(QueueExt.QueuePropertyNames.InterfaceTypeCode, "");//, "Logitude.CustomsMessaging.MessagingServices.DF_MSG10000_ImportDeclarationMessagingService");
                if (String.IsNullOrWhiteSpace(analyzeClass))
                {
                    message.SafeComplete();
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method :analyzeClass ==null", null);
                    //message.DeadLetter();
                    return false;//
                }

                if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(analyzeClass))
                {
                    message.SafeComplete();
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage():!ContainerAccessor.Container.IsRegistered :analyzeClass=" + analyzeClass, null);
                    //message.DeadLetter();
                    return false;
                }
                var tenant = message.GetProperty<int>(QueueExt.QueuePropertyNames.Tenant, -1);
                if (tenant == -1)
                {
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method :tenant==-1", null);
                    return false;
                }
                var correlationId = message.CorrelationId;
                ///Due Failed if i procces it !!! ---LogMessagingUtil.Instance.AppendLine("receivedMessage.DeliveryCount =" + message.DeliveryCount.ToString());

                var s = this.GetType().Name;
                CustomsCommandEnum myCustomsCommandEnum;
                if (Enum.TryParse<CustomsCommandEnum>(s, out myCustomsCommandEnum))
                {
                    //myCustomsCommandEnum
                }
                else
                {
                    throw new Exception("Enum.TryParse<CustomsCommandEnum>(s, out myCustomsCommandEnum)");
                }

                MessagingServiceFactoryHelper.ResolveAndExecute(analyzeClass, tenant, correlationId, myCustomsCommandEnum, controller);

                message.SafeComplete();
                return true;

            }
            catch (CustomsRequestsSheetDomainModelServiceException customsRequestsSheetServiceException)
            {

                //ExceptionHandler.HandleException(customsRequestsSheetServiceException, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method/CustomsRequestsSheetServiceException ", null);
                if (customsRequestsSheetServiceException.What2Do == CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue)
                {
                    message.SafeComplete();
                    return true;
                }
                else
                {
                    message.SetProperty<DateTime>(QueueExt.QueuePropertyNames.LastExecAt, DateTime.UtcNow);
                    message.SafeAbandon();
                    return false;
                }

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method", null);
                message.SafeComplete();
                return false;
                //throw;
            }
        }






        // islam db queue service
        void WorkUntilQEmpty_Db()
        {

            
            
            CustomDBQueueMessage response;
            List<long> deferredSequenceNumbers = new List<long>();
            bool proccesDone = false;
            for (int filtterPriority = 2; filtterPriority < 3; filtterPriority++)
            {
                while (true)
                {
                    //throw new Exception("BrokeredMessage receivedMessage = _QueueClient.Receive(TimeSpan.FromSeconds(5));");
                    using (TransactionScope Queue_scope = TransactionFactory.GetTransaction())
                    {
                        try
                        {




                            response = _CustomDbQueueService.Receive();


                            // receivedMessage = _QueueClient.Receive(TimeSpan.FromSeconds(5)); //islam
                        }
                        catch (Exception)
                        {

                            throw;
                        }


                        if (response == null || (response != null && response.MessageId == null))
                        {
                            Thread.Sleep(TimeSpan.FromSeconds(1));
                            break;
                        }



                        proccesDone = true;
                        bool successProcessMessage = ProcessMessage_Db(response);
                        if (successProcessMessage)
                        {
                            _CustomDbQueueService.SafeComplete();
                            Queue_scope.Complete();
                        }
                        else if (!successProcessMessage)/// IF FAILED USE NEW TRANS !!!!
                        {
                            Queue_scope.Dispose();//remove lock !!
                            using (var Abandon_Queue_scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                            {

                                _CustomDbQueueService.SafeAbandon();
                                Abandon_Queue_scope.Complete();
                            }
                        }
                        LogDoneItemInMemory();
                    }
                }
            }


        }

        protected virtual bool ProcessMessage_Db(CustomDBQueueMessage msgResponse)
        {

            try
            {
                int tenant = -1;
                string analyzeClass = msgResponse.Properties["InterfaceTypeCode"].ToString();
                

                
                if (String.IsNullOrWhiteSpace(analyzeClass))
                {
                    //_CustomDbQueueService.SafeAbandon();
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method :analyzeClass ==null", null);
                    //message.DeadLetter();
                    return false;//
                }

                if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(analyzeClass))
                {
                    //_CustomDbQueueService.SafeAbandon();
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage():!ContainerAccessor.Container.IsRegistered :analyzeClass=" + analyzeClass, null);
                    //message.DeadLetter();
                    return false;
                }
                //var tenant = message.GetProperty<int>(QueueExt.QueuePropertyNames.Tenant, -1);
                int.TryParse(msgResponse.Properties["Tenant"].ToString(), out tenant);
                if (tenant == -1)
                {
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method :tenant==-1", null);
                    return false;
                }

                string correlationId = msgResponse.Properties["CorrelationId"].ToString();
                // var correlationId = message.CorrelationId;
                //LogMessagingUtil.Instance.AppendLine("receivedMessage.DeliveryCount =" + message.DeliveryCount.ToString());

                var s = this.GetType().Name;
                CustomsCommandEnum myCustomsCommandEnum;
                if (Enum.TryParse<CustomsCommandEnum>(s, out myCustomsCommandEnum))
                {
                    //myCustomsCommandEnum
                }
                else
                {
                    throw new Exception("Enum.TryParse<CustomsCommandEnum>(s, out myCustomsCommandEnum)");
                }

                MessagingServiceFactoryHelper.ResolveAndExecute(analyzeClass, tenant, correlationId, myCustomsCommandEnum);

                

                //_CustomDbQueueService.SafeComplete();
                return true;

            }
            catch (CustomsRequestsSheetDomainModelServiceException customsRequestsSheetServiceException)
            {

                //ExceptionHandler.HandleException(customsRequestsSheetServiceException, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method/CustomsRequestsSheetServiceException ", null);
                
                if (customsRequestsSheetServiceException.What2Do == CustomsRequestsSheetDomainModelServiceException.What2DoEnum.StopQueue)
                {
                    //message.SafeComplete();
                    //_CustomDbQueueService.SafeComplete();
                    return true;
                }
                else
                {
                    //_CustomDbQueueService.SafeAbandon();
                    return false;  
                }
                

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method", null);
                //message.SafeComplete();
                //_CustomDbQueueService.SafeComplete();

                //_CustomDbQueueService.SafeAbandon();// make try (in 5101 CRS was analyze *1000000)
                return false;
                //throw;
            }
        }

        public string _QueueNameOverride { get; set; }
    }
}
