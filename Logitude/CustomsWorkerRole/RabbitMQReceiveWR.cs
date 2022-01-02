
using CustomsWorkerRole.L2U;

using Logitude.Server.Tools.Models;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
//using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Blob;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnifreightIIG.UServer;
using CustomsWorkerRole.Queue;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Server.Infrastructure;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.Helpers;
using Logitude.Server.Tools;
using Newtonsoft.Json;
using System.Net.Http;
using Logitude.Customs.BL.Messaging.Maman;
using Microsoft.Practices.Unity;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.FTP;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel.Repositories;
using Unifreight.Data.AmitalModel;
using Logitude.Server.Tools.Utils;
using Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Logitude.CustomsMessaging.ResponseServices;
using System.Xml.Serialization;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.AmitalMessaging.Utils;
using System.Xml;

using Logitude.CustomsMessaging.RabbitMQ;

namespace CustomsWorkerRole
{

    public class RabbitMQReceiveWR
        : CustomsWorkerEntryPoint
    {
        QueueDescription _QueueDescription;
        QueueClient _QueueClient;

        private AnalyzeResultModel _AnalyzeResultModel;

        public override void Run()
        {

            while (true)
            {

                if (!General.IsUpdating())
                {
                    try
                    {

                        WorkOnce();
                        Thread.Sleep(TimeSpan.FromSeconds(_SeedTenant));
                    }
                    catch (Exception e)
                    {
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "RabbitMQReceiveWR : Run() Method", null);
                        Thread.Sleep(10000);
                    }
                }
                else
                {
                    Thread.Sleep(60000);
                }


            }

        }
        bool _OnStartDone = false;
        private ICommonDataContext _ICommonDataContext;

        private CommunicationLogRepository _CommunicationLogRep;
        private int _Tenant;

        private CommunicationLog _WaitingCommLog;

        private DateTime _LastCreateFtpDefinition;
        private List<CustomsPartnerFtpPM> _FtpDefinitions;
        private int _SeedTenant = 1;

        public override bool OnStart()
        {
            //WorkUntil_AnalyzeQueue_Empty_Db_NOTINUSE();

            List<int> listTenant = GetCourierTenant();
            if (listTenant.Count==0)
            {
                Thread.Sleep(5000);
                return true;
            }
            var workers = new List<Task>();
            foreach (var currTenant in listTenant)
            {
                var task = new Task(async (myState) =>
                {
                    int tenant = (int)myState;
                    while (!WorkerRoleServiceLocator.PleaseShutDown)
                    {
                        
                        ConsumePerTenant(tenant);
                    }

                }, state: currTenant);
                task.Start();
                workers.Add(task);
            }


            Task.WhenAll(workers.ToArray());
            return true;

        }

        private List<int> GetCourierTenant()
        {
            var _SeedDefaultTenant = 1; 
            var customsSettingQueryService = new CustomsSettingQueryService(_SeedDefaultTenant);
            var res = customsSettingQueryService.GetAll().Where(r => r.CompanyType == "B").Select(r=>r.Tenant).ToList();//Courier
            return res;
        }

        private void ConsumePerTenant(int currTenant)
        {
            string RabbitMQLogFILE = "RabbitMQLog" + currTenant.ToString();

            Logger.LogMe("START", false, RabbitMQLogFILE);
            var customRabbitMQQueue = new CustomRabbitMQQueue();
            var allQueueDetails = customRabbitMQQueue.GetAllQueueDetails()
             .Where(r => r.AnalyzeQueueService != AnalyzeMQQueueServiceEnum.none)
            .ToList();


            EventHandler<BasicDeliverEventArgs> consumerEventArgs = null;
            EventingBasicConsumer consumer = null;

            var factory = RabbitmqHelper.GetConnectionFactory();

            
            try
            {
                string rabbitMQCode = RabbitmqHelper.GetRabbitMQCode(currTenant);
                //var factory = new ConnectionFactory() { HostName = "unimq", UserName = "v5101", Password = "Aa123"  };
                factory.RequestedHeartbeat = TimeSpan.FromSeconds(600);
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    try
                    {

                        connection.ConnectionShutdown += Connection_ConnectionShutdown;

                        Logger.LogMe("CONNECTION", false, RabbitMQLogFILE);

                        channel.BasicQos(0, 5, true);

                        RabbitmqHelper.DeclareQueue(channel, rabbitMQCode,true);
                        AnalyzeQueueRepository analyzeQueueRepository = new AnalyzeQueueRepository();


                        consumerEventArgs = (model, ea) =>
                        {
                            if (channel == null)
                                return;
                            if (!channel.IsOpen)
                                return;

                            string messageId = "";
                            try
                            {
                                var body = ea.Body.ToArray();
                                var message = Encoding.UTF8.GetString(body);
                                messageId = ea.BasicProperties.MessageId;
                                Logger.LogMe("RUN", false, RabbitMQLogFILE);
                                Logger.LogMe("START  Exec : " + messageId, false, RabbitMQLogFILE);
                                string log = "";
                                bool success = false;
                                object oInterfaceTypeCode = "";
                                // ea.BasicProperties.Headers.TryGetValue("InterfaceTypeCode", out oInterfaceTypeCode);
                                
                                string interfaceTypeCode = Encoding.UTF8.GetString(ea.BasicProperties.Headers["InterfaceTypeCode"] as byte[]);
                                if (string.IsNullOrWhiteSpace(interfaceTypeCode))
                                {
                                    Logger.LogMe("No interfaceTypeCode in header " + messageId, true, RabbitMQLogFILE);
                                    channel.BasicAck(ea.DeliveryTag, false);
                                    return;
                                }
                                var myQueueDetails = allQueueDetails.FirstOrDefault(r => r.Code == interfaceTypeCode);
                                if (myQueueDetails==null)
                                {
                                    Logger.LogMe($"messageId={messageId} header InterfaceTypeCode ={interfaceTypeCode}  but not exist in customRabbitMQQueue.GetAllQueueDetails " , true, RabbitMQLogFILE);
                                    channel.BasicAck(ea.DeliveryTag, false);
                                    return;
                                }


                                try
                                {
                                    Exec(customRabbitMQQueue, myQueueDetails, analyzeQueueRepository, messageId, 1, message, out log, out success);
                                }
                                catch (Exception)
                                {
                                    success = false;
                                    // throw;
                                }
                                Logger.LogMe("END  Exec : " + messageId, false, RabbitMQLogFILE);
                                Logger.LogMe("END  Exec : " + messageId + " , Log:" + log, false, RabbitMQLogFILE);

                                LogDoneItemInMemory();



                                if (success)
                                {
                                    Logger.LogMe("BasicAck : " + messageId, false, RabbitMQLogFILE);

                                    channel.BasicAck(ea.DeliveryTag, false);
                                }
                                else
                                {
                                    channel.BasicNack(ea.DeliveryTag, false, true);

                                }

                            }

                            catch (Exception ex)
                            {

                                Logger.LogMe(ex.Message, false, RabbitMQLogFILE);


                            }

                        };

                        consumer = new EventingBasicConsumer(channel);
                        consumer.Received += consumerEventArgs;

                        channel.BasicConsume(queue: rabbitMQCode,
                                            autoAck: false,
                                            consumer: consumer);

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
                Logger.LogMe(e.Message, false, RabbitMQLogFILE);

                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "CustomsAnalyzeQueueWR : Run() Method", null);
                Thread.Sleep(5000);
            }


        }

        

        private static void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("Connection broke!");

            //Cleanup();

            //while (true)
            //{
            //    try
            //    {
            //        Connect();

            //        Console.WriteLine("Reconnected!");
            //        break;
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine("Reconnect failed!");
            //        Thread.Sleep(3000);
            //    }
            //}
        }
        private void WorkUntil_AnalyzeQueue_Empty_Db_NOTINUSE()
        {
            Logger.LogMe("START", false, "RabbitMQLog");
            var customRabbitMQQueue = new CustomRabbitMQQueue();
            var _CustomsAnalyzeQueueServices = customRabbitMQQueue.GetAllQueueDetails()
             .Where(r => r.AnalyzeQueueService != AnalyzeMQQueueServiceEnum.none)
            .ToList();

            while (true)
            {

                EventHandler<BasicDeliverEventArgs> consumerEventArgs = null;
                EventingBasicConsumer consumer = null;

               var factory = RabbitmqHelper.GetConnectionFactory();
          
           foreach (QueueDetails queue in _CustomsAnalyzeQueueServices)
                {

                    {


                        try
                        {
                            //var factory = new ConnectionFactory() { HostName = "unimq", UserName = "v5101", Password = "Aa123"  };
                            factory.RequestedHeartbeat = TimeSpan.FromSeconds(600);
                            using (var connection = factory.CreateConnection())
                            using (var channel = connection.CreateModel())
                            {
                                try
                                {

                                    connection.ConnectionShutdown += Connection_ConnectionShutdown;

                                    Logger.LogMe("CONNECTION", false, "RabbitMQLog");
                                   
                                     channel.BasicQos(0, 5, true);

                                      RabbitmqHelper.DeclareQueue(channel, queue.Code,true);
                                     AnalyzeQueueRepository analyzeQueueRepository = new AnalyzeQueueRepository();

                                
                                     consumerEventArgs = (model, ea) =>
                                    {
                                        if (channel == null)
                                            return;
                                        if (!channel.IsOpen)
                                            return;

                                        string messageId = "";
                                        try
                                        {
                                            var body = ea.Body.ToArray();
                                             var message = Encoding.UTF8.GetString(body);
                                            messageId = ea.BasicProperties.MessageId;
                                            Logger.LogMe("RUN", false, "RabbitMQLog");
                                            Logger.LogMe("START  Exec : " + messageId, false, "RabbitMQLog");
                                            string log = "";
                                            bool success = false;
                                            try
                                            {
                                                Exec(customRabbitMQQueue, queue, analyzeQueueRepository, messageId, 1, message, out log , out success);
                                             }
                                            catch (Exception)
                                            {
                                                success = false;
                                               // throw;
                                            }
                                            Logger.LogMe("END  Exec : " + messageId, false, "RabbitMQLog");
                                            Logger.LogMe("END  Exec : " + messageId + " , Log:" + log, false, "RabbitMQLog");

                                            LogDoneItemInMemory();



                                            if (success)
                                            {
                                                Logger.LogMe("BasicAck : " + messageId, false, "RabbitMQLog");

                                                channel.BasicAck(ea.DeliveryTag, false);
                                            }
                                            else
                                            {
                                                channel.BasicNack(ea.DeliveryTag, false, true);

                                            }

                                        }

                                        catch (Exception ex)
                                        {

                                            Logger.LogMe(ex.Message, false, "RabbitMQLog");

                                            
                                        }

                                    };

                                    consumer = new EventingBasicConsumer(channel);
                                    consumer.Received += consumerEventArgs;

                                    channel.BasicConsume(queue: queue.Code  ,
                                                        autoAck: false,
                                                        consumer: consumer);

                                    while (true)
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
                            Logger.LogMe(e.Message, false, "RabbitMQLog");

                            ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "CustomsAnalyzeQueueWR : Run() Method", null);
                            Thread.Sleep(5000);
                        }

                     
                    }
                }


                //Thread.Sleep(TimeSpan.FromSeconds(5));

                Thread.Sleep(TimeSpan.FromSeconds(1));//not using soo mach 
                break;


            }
        }

        private void Exec(CustomRabbitMQQueue customRabbitMQQueue, QueueDetails queue, AnalyzeQueueRepository analyzeQueueRepository, string communicationLogId, int tenant , string message, out string log, out bool success)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                var serviceAnalyzer = customRabbitMQQueue.GetCustomAnalyzerQueueService(queue);
                //ArtemusAnalyzer analyzer = new Artemus(analyzeQueue, analyzeQueueRepository);
                serviceAnalyzer.Run( analyzeQueueRepository, tenant , communicationLogId , message , queue, out  log, out success);
                scope.Complete();
            }
        }



 
        public override void WorkOnce()
        {

            try
            {
                OnStart();

              //  WorkUntil_AnalyzeQueue_Empty_Db_NOTINUSE();


            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole" + this.GetType().Name, " : Run() Method", null);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                _OnStartDone = false;
            }


        }

   
    }



}
