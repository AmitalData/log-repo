
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
using Logitude.Server.Tools.Utils;
using System.Configuration;
using Logitude.Customs.BL.Messaging.Customs.PerformanceLogger;
using Logitude.CustomsMessaging.RabbitMQ;
using Logitude.Customs.BL.CloseTables;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Newtonsoft.Json;
using System.Globalization;

namespace CustomsWorkerRole
{
    public partial class CustomsCommandBase : CustomsWorkerEntryPoint
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
        string className;
        public override bool OnStart()
        {
            //try
            //{
            if (_OnStartDone) return true;
            _OnStartDone = true;
            DoneItemsInRange = new Dictionary<DateTime, int>();
            var myClass = this.GetType().Name;
            if (!string.IsNullOrWhiteSpace(_QueueNameOverride))
            {
                myClass = _QueueNameOverride;
            }
            className = myClass;
            var customsEnvironmentSettingQueryService = new CustomsEnvironmentSettingQueryService(1);
            var customsEnvironmentSettingPM = customsEnvironmentSettingQueryService.GetEnvironmentSettingPM() ?? new CustomsEnvironmentSettingPM();
            if (customsEnvironmentSettingPM.UseRabbitMQ)
            {
                base.WorkerQueueType = WorkerQueueType.RabbitMQ;
            }
            else
            {
                base.WorkerQueueType = WorkerQueueType.DB;
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
        private string _RabbitQueueCode;

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
                //if (LogitudeSettings.QueueServiceMode != "db")
                //{
                //    WorkUntilQEmpty();
                //}
                //else
                //{


                switch (base.WorkerQueueType)
                {

                    case WorkerQueueType.RabbitMQ:
                        WorkUntilPrcossesStop_RabbitMQ();
                        break;
                    case WorkerQueueType.DB:
                    default:
                        {
                            if (Logitude.Server.Tools.Helpers.FeatureToggleHelper.HasFeatureToggle("DQN", 0))
                            {
                                WorkUntilQEmpty_Db_new();
                            }
                            else
                            {
                                WorkUntilQEmpty_Db();
                            }
                        }
                        break;
                }
                
                //}

            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole" + this.GetType().Name, " : Run() Method", null);
                Thread.Sleep(TimeSpan.FromSeconds(1));
                _OnStartDone = false;
            }

        }







        private static void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("Connection broke!");
        }
            
        void WorkUntilPrcossesStop_RabbitMQ()
        {
            this._RabbitQueueCode = RabbitQueueCodeService.GetRabbitQueueCode(this.GetType().Name, base.QueueGroupCodeRabbit);
            if (!string.IsNullOrWhiteSpace(base.OverrideRMQ))
            {
                this._RabbitQueueCode = base.OverrideRMQ;
            }
            EventHandler<BasicDeliverEventArgs> consumerEventArgs = null;
            try
            {
                var factory = RabbitmqHelper.GetConnectionFactory(tryFromAppSettings: false);
                //var factory = new ConnectionFactory() { HostName = "unimq", UserName = "v5101", Password = "Aa123"  };
                factory.RequestedHeartbeat = TimeSpan.FromMinutes(10);
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    try
                    {

                        connection.ConnectionShutdown += Connection_ConnectionShutdown;

                        

                        channel.BasicQos(0, 5, true);

                        
                        

                        
                        


                        RabbitmqHelper.DeclareQueue(channel, this._RabbitQueueCode, true);
                        


                        consumerEventArgs = (model, ea) =>
                        {
                            if (channel == null)
                                return;
                            if (!channel.IsOpen)
                                return;

                            string messageId = "";
                            CustomDBQueueMessage customDBQueueMessage = null;
                            try
                            {
                                var body = ea.Body.ToArray();
                                var message = Encoding.UTF8.GetString(body);
                                messageId = ea.BasicProperties.MessageId;
                                string log = "";


                                LogMessagingUtilWR.Instance.Clear();
                                LogMessagingUtil.Instance.Clear();


                                string que_id = Encoding.UTF8.GetString(ea.BasicProperties.Headers["que_id"] as byte[]);
                                long longQId = -999;
                                if (string.IsNullOrWhiteSpace(que_id) || !long.TryParse(que_id, out longQId))
                                {
                                    channel.BasicAck(ea.DeliveryTag, false);
                                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "CustomsCommandBase:RabbitMQ", $"BasicProperties.Headers[que_id] is null  ", null);
                                    Thread.Sleep(1000);

                                    return;
                                }


                                customDBQueueMessage = _CustomDbQueueService.GetRabbitMQPseudoByMessageId(longQId) ;
                                if (customDBQueueMessage == null)
                                {
                                    channel.BasicAck(ea.DeliveryTag, false);
                                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRoleRabbitMQ", $"GetRabbitMQPseudoByMessageId not found({longQId})", null);
                                    Thread.Sleep(100);
                                    return;

                                }
                                
                                LogMessagingUtilWR.Instance.AppendLine("ProcessMessage_RabbitMQ");
                                bool successProcessMessage = false;
                                using (TransactionScope queue_TransactionScope = TransactionFactory.GetTransaction())
                                {
                                    try
                                    {
                                        successProcessMessage = ProcessMessage_Db(customDBQueueMessage);
                                        LogMessagingUtilWR.Instance.AppendLine($"successProcessMessage:{successProcessMessage}");
                                        if (successProcessMessage)
                                        {
                                            customDBQueueMessage.SafeComplete();  //RemoveQueueMessage(messageId);
                                            queue_TransactionScope.Complete();

                                        }

                                    }
                                    catch (Exception e1)
                                    {

                                        queue_TransactionScope.Dispose();
                                        successProcessMessage = false;
                                        NetCommonHelper.Logger.DevLog.Instance.WriteFatal(e1);
                                        ExceptionHandler.HandleException(e1, DateTime.Now, 0, "", "WorkerRoleRabbitMQ", $"WorkUntilPrcossesStop_RabbitMQ{messageId}", null);
                                        Thread.Sleep(1000);

                                    }
                                    finally
                                    {

                                        bool isTimeToEndDueMaxTries = false;
                                        try
                                        {

                                            if (!successProcessMessage)
                                            {
                                                using (TransactionScope Abandon_Queue_scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                                                {

                                                    isTimeToEndDueMaxTries = customDBQueueMessage.SafeAbandon();
                                                    Abandon_Queue_scope.Complete();
                                                }
                                            }
                                            // all the time remove the queue 
                                            // on success - remove the queue 
                                            // on fail - 
                                            //   if isTimeToEndDueMaxTries = > remove the queue 
                                            //   if not isTimeToEndDueMaxTries - we suuceesed to update the same queue to NextRunDateTime =  +1Min, RetryNumber = +1 ,haverabbitmq =0 ==> create new Queue = > remove the queue 

                                            channel.BasicAck(ea.DeliveryTag, false);


                                        }
                                        catch (Exception eee)
                                        {

                                            NetCommonHelper.Logger.DevLog.Instance.WriteFatal(eee);
                                            ExceptionHandler.HandleException(eee, DateTime.Now, 0, "", "WorkerRoleRabbitMQ", $"WorkUntilPrcossesStop_RabbitMQ{messageId}", null);
                                            Thread.Sleep(1000);
                                        }



                                        PerformanceM.LastInstance.QueueSuccessComplete = true;
                                    }
                                }

                                



                                LogDoneItemInMemory();


                            }

                            catch (Exception ex)
                            {

                                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex);
                                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRoleRabbitMQ", $"WorkUntilPrcossesStop_RabbitMQ{messageId}", null);
                                Thread.Sleep(1000);


                            }

                        };

                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += consumerEventArgs;

                        channel.BasicConsume(queue: this._RabbitQueueCode,
                                            autoAck: false,
                                            consumer: consumer);
                        Debug.WriteLine("Start BasicConsume " + this._RabbitQueueCode);
                        while (!WorkerRoleServiceLocator.PleaseShutDown)
                        {

                            Thread.Sleep(100);
                            bool getOut = false;
                            if (getOut)
                            {
                                break;
                            }
                        }


                    }

                    catch (Exception e)
                    {

                    }

                    finally
                    {
                        channel.Close();
                        connection.Close();
                    }

                }
            }
            catch (Exception e)
            {

                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "CustomsAnalyzeQueueWR : Run() Method", null);
                Thread.Sleep(5000);
            }
        }



        // islam db queue service
        void WorkUntilQEmpty_Db()
        {



            CustomDBQueueMessage response = null;
            List<long> deferredSequenceNumbers = new List<long>();
            bool proccesDone = false;
            for (int filtterPriority = 2; filtterPriority < 3; filtterPriority++)
            {
                while (!WorkerRoleServiceLocator.PleaseShutDown)
                {
                    LogMessagingUtilWR.Instance.Clear();
                    LogMessagingUtil.Instance.Clear();

                    DateTime QueueStartDate = DateTime.Now;
                    //throw new Exception("BrokeredMessage receivedMessage = _QueueClient.Receive(TimeSpan.FromSeconds(5));");
                    try
                    {
                        LogMessagingUtilWR.Instance.AppendLine("TransactionFactory.GetTransaction");
                        //int transactionTimeOutInMin = Math.Max(10, CustomsWorkerRole.Utils.GenUtil.GetQueueTimeOutInMin());
                        //using (TransactionScope Queue_scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(transactionTimeOutInMin)))
                        using (TransactionScope Queue_scope = TransactionFactory.GetTransaction())
                        {
                            using (TransactionScope scopeRecive = TransactionFactory.GetNewReadCommittedTransaction())
                            {


                                try
                                {

                                    LogMessagingUtilWR.Instance.AppendLine("QRecive");

                                    response = _CustomDbQueueService.Receive(CustomsWorkerRole.Utils.GenUtil.GetQueueTimeOutInMin() * 60);
                                    LogMessagingUtilWR.Instance.AppendLine("QRecive:after");

                                    scopeRecive.Complete();




                                    // receivedMessage = _QueueClient.Receive(TimeSpan.FromSeconds(5)); //islam
                                }
                                catch (Exception)
                                {

                                    throw;
                                }
                            }

                            if (response == null || (response != null && response.MessageId == null))
                            {
                                QueueThreadStateService.Upsert(QueueThreadStateService.GetWRKey(this.GetType().Name), "No Work");
                                Thread.Sleep(TimeSpan.FromSeconds(CustomsWorkerRole.Utils.GenUtil.IfNoQueue_ServerWaitTimeInSec()));

                                break;
                            }
                            PerformanceM.EnqueueLastInstance();
                            PerformanceM.LastInstance.QueueStartDate = QueueStartDate;
                            PerformanceM.LastInstance.QueueReceiveDate = DateTime.Now;

                            LastActivity = DateTime.UtcNow;
                            proccesDone = true;
                            LogMessagingUtilWR.Instance.AppendLine("ProcessMessage_Db");
                            bool successProcessMessage = ProcessMessage_Db(response);
                            LogMessagingUtilWR.Instance.AppendLine("successProcessMessage");
                            if (successProcessMessage)
                            {
                                _CustomDbQueueService.SafeComplete();
                                Queue_scope.Complete();
                                PerformanceM.LastInstance.QueueSuccessComplete = true;
                            }
                            else if (!successProcessMessage)/// IF FAILED USE NEW TRANS !!!!
                            {

                                try
                                {

                                    // if inner scope dispose without Complete // this can crush 

                                    // but there is case that there is acrush withou transaction
                                    // like while dca check status = so we want that the try of the step will increase in 1 - we must try commit it !!
                                    Queue_scope.Complete();
                                }
                                catch (Exception)
                                {
                                    //throw;
                                }
                                try
                                {
                                    Queue_scope.Dispose();//remove lock !!
                                }
                                catch (Exception)
                                {


                                }

                                using (var Abandon_Queue_scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                                {

                                    _CustomDbQueueService.SafeAbandon();//if (CurrentCustomQueueResponse.Retries > 10)
                                    Abandon_Queue_scope.Complete();
                                }

                            }
                            LogMessagingUtilWR.Instance.AppendLine("LogDoneItemInMemory();");
                            LogDoneItemInMemory();
                            LogMessagingUtilWR.Instance.AppendLine("LogDoneItemInMemory();AFTER");
                            PerformanceM.LastInstance.QueueEndDate = DateTime.Now;
                            PerformanceM.EnqueueLastInstance();

                            string logItMessagingUtilWR = ConfigurationManager.AppSettings.Get("LogMessagingUtilWR");

                            if (!string.IsNullOrWhiteSpace(logItMessagingUtilWR))
                            {
                                string morethan = "";
                                string str = LogMessagingUtilWR.Instance.GetString(out morethan);
                                NetCommonHelper.Logger.DevLog.Instance.WriteDebug(str + "_" + morethan);
                            }
                        }

                    }
                    finally
                    {
                        PerformanceM.SleepMSAfterEachQueuePeek();
                    }
                }
            }


        }





        // islam db queue service
        void WorkUntilQEmpty_Db_new()
        {
            //LogTime(className + " start all");
            List<CustomDBQueueMessage> responseList=null;
            List<long> deferredSequenceNumbers = new List<long>();
            bool proccesDone = false;
            for (int filtterPriority = 2; filtterPriority < 3; filtterPriority++)
            {
                while (!WorkerRoleServiceLocator.PleaseShutDown)
                {
                    LogMessagingUtilWR.Instance.Clear();
                    LogMessagingUtil.Instance.Clear();

                    DateTime QueueStartDate = DateTime.Now;
                    //throw new Exception("BrokeredMessage receivedMessage = _QueueClient.Receive(TimeSpan.FromSeconds(5));");
                    try
                    {
                        LogMessagingUtilWR.Instance.AppendLine("TransactionFactory.GetTransaction");
                        //int transactionTimeOutInMin = Math.Max(10, CustomsWorkerRole.Utils.GenUtil.GetQueueTimeOutInMin());
                        using (TransactionScope Queue_scope = TransactionFactory.GetTransaction())
                        {
                            using (TransactionScope scopeRecive = TransactionFactory.GetNewReadCommittedTransaction())
                            {


                                try
                                {

                                    LogMessagingUtilWR.Instance.AppendLine("QRecive");
                                    LogTime(className + " start get data from DB");
                                    responseList = _CustomDbQueueService.Receive_new(CustomsWorkerRole.Utils.GenUtil.GetQueueTimeOutInMin() * 60);
                                    //LogTime(className + " end get data from DB");
                                    LogMessagingUtilWR.Instance.AppendLine("QRecive:after");

                                    scopeRecive.Complete();

                                    // receivedMessage = _QueueClient.Receive(TimeSpan.FromSeconds(5)); //islam
                                }
                                catch (Exception ex)
                                {
                                    LogTime(className + " exception: " + ex.Message);
                                    scopeRecive.Dispose();
                                    throw;
                                }
                            }

                            if (responseList == null || (responseList != null && responseList.Count == 0))
                            {
                                //LogTime(className + " queue is empty");
                                QueueThreadStateService.Upsert(QueueThreadStateService.GetWRKey(this.GetType().Name), "No Work");
                                Thread.Sleep(TimeSpan.FromSeconds(CustomsWorkerRole.Utils.GenUtil.IfNoQueue_ServerWaitTimeInSec()));

                                break;
                            }

                            PerformanceM.EnqueueLastInstance();
                            PerformanceM.LastInstance.QueueStartDate = QueueStartDate;
                            PerformanceM.LastInstance.QueueReceiveDate = DateTime.Now;

                            LastActivity = DateTime.UtcNow;
                            proccesDone = true;
                            var taskLIst = new List<Task>();

                            var currentThreads = Process.GetCurrentProcess().Threads;
                            var currentThreadsCast = currentThreads.Cast<ProcessThread>();
                            var runningThreads = currentThreadsCast.Where(thread => thread.ThreadState.ToString() == "Running").Count();
                            var waitThreads = currentThreadsCast.Where(thread => thread.ThreadState.ToString().StartsWith("Wait")).Count();
                            var stopThreads = currentThreadsCast.Where(thread => thread.ThreadState.ToString() == "Unstarted").Count();

                            LogTime($"{className} start open tasks for {responseList.Count} returned rows from Db (threads count: {currentThreads.Count}, {runningThreads}, {waitThreads}, {stopThreads})");
                            var totalStopwatch = Stopwatch.StartNew();

                            foreach (var item in responseList)
                            {
                                var stopwatch = Stopwatch.StartNew();
                                //LogTime(className + " create new task for msg id: " + item.MessageId);
                                var t =
                                Task.Factory.StartNew(() =>
                                {
                                    var createdElapsed = stopwatch.Elapsed.TotalSeconds;
                                    // LogTime(className + " start task (created " + stopwatch.Elapsed.TotalSeconds + " seconds ago) for row MessageId: " + item.MessageId);
                                    var taskstopwatch = Stopwatch.StartNew();

                                    CustomsCommandBaseHelper helper = new CustomsCommandBaseHelper();
                                    helper.RunTask(item, className);
                                    LogMessagingUtilWR.Instance.AppendLine("LogDoneItemInMemory();");
                                    LogDoneItemInMemory();
                                    LogMessagingUtilWR.Instance.AppendLine("LogDoneItemInMemory();AFTER");
                                    LogTime(className + " end task (elapsed: " + (int)taskstopwatch.Elapsed.TotalSeconds + " seconds. started after: " + (int)createdElapsed + " seconds) for row MessageId: " + item.MessageId);
                                });
                                taskLIst.Add(t);
                            }
                            Task.WaitAll(taskLIst.ToArray());
                            LogTime(className + " end waiting for all of them (total elapsed: " + (int)totalStopwatch.Elapsed.TotalSeconds + " seconds)");
                            Queue_scope.Complete();
                        }
                    }
                    finally
                    {
                        PerformanceM.SleepMSAfterEachQueuePeek();
                    }
                }
            }
            //LogTime(className + " end all");
        }
        private void LogTime(string msg)
        {
            DateTime stopLogAt = new DateTime(2023, 06, 01);
            string UntilDateyyyyMMdd = ConfigurationManager.AppSettings["20230601T000000.LogUntilDateyyyyMMdd"];
            if (!string.IsNullOrWhiteSpace(UntilDateyyyyMMdd))
                stopLogAt = DateTime.ParseExact(UntilDateyyyyMMdd, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
            // msg += DateTime.Now.ToString();
            
            LogitudeSettings.HandleLogMe(msg, false, "WorkUntilQEmpty_Db_new", stopLogAt);
        }


        protected virtual bool ProcessMessage_Db(CustomDBQueueMessage msgResponse)
        {
            LogMessagingUtilWR.Instance.AppendLine("ProcessMessage_Db");
            try
            {
                int tenant = -1;
                string analyzeClass = msgResponse.Properties["InterfaceTypeCode"].ToString();




                if (String.IsNullOrWhiteSpace(analyzeClass))
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteError("analyzeClass is null");
                    //_CustomDbQueueService.SafeAbandon();
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method :analyzeClass ==null", null);
                    //message.DeadLetter();
                    return false;//
                }

                //if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(analyzeClass))
                //{
                //    //_CustomDbQueueService.SafeAbandon();
                //    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage():!ContainerAccessor.Container.IsRegistered :analyzeClass=" + analyzeClass, null);
                //    //message.DeadLetter();
                //    return false;
                //}
                //var tenant = message.GetProperty<int>(QueueExt.QueuePropertyNames.Tenant, -1);
                int.TryParse(msgResponse.Properties["Tenant"].ToString(), out tenant);
                if (tenant == -1)
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteError("Tenant is null");
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method :tenant==-1", null);
                    return false;
                }

                string correlationId = msgResponse.Properties["CorrelationId"].ToString();
                LogMessagingUtilWR.Instance.AppendLine($"correlationId = {correlationId};analyzeClass={analyzeClass}");
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
                PerformanceM.LastInstance.InterfaceTypeCode = analyzeClass;
                PerformanceM.LastInstance.RequestSheetID = correlationId;
                PerformanceM.LastInstance.QueueDefinitionCode = myCustomsCommandEnum.ToString();
                LogMessagingUtilWR.Instance.AppendLine("ResolveAndExecute");
                try
                {
                    QueueThreadStateService.Upsert(
        QueueThreadStateService.GetWRKey(this.GetType().Name),
        $"Interface:{analyzeClass},RequestSheetID:{correlationId},QId:{msgResponse?.MessageId},QDefinition:{PerformanceM.LastInstance?.QueueDefinitionCode}"
        );

                }
                catch //(Exception)
                {

                    
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
