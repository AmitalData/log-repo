using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue;
using Logitude.CustomsMessaging.RabbitMQ;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Utils;
using Logitude.SystemLogs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
//using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace RabbitMQSRV
{

    public class RabbitMQReceiveWR

    {
        

        private AnalyzeResultModel _AnalyzeResultModel;

        public  void Run()
        {

            while (!WorkerRoleServiceLocator.PleaseShutDown)
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

        }
        bool _OnStartDone = false;
        private ICommonDataContext _ICommonDataContext;

        private CommunicationLogRepository _CommunicationLogRep;
        private int _Tenant;

        

        private DateTime _LastCreateFtpDefinition;
        
        private int _SeedTenant = 1;

        public  bool OnStart()
        {
            //WorkUntil_AnalyzeQueue_Empty_Db_NOTINUSE();
            if (WorkerRoleServiceLocator.PleaseShutDown)
            {
                return true;
            }

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


            var t=Task.WhenAll(workers.ToArray());
            t.Wait();
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
                //GWSFLOGITUDE > GGGFRABBITMQ
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
                                    Exec(customRabbitMQQueue, myQueueDetails, analyzeQueueRepository, messageId, currTenant, message, out log, out success);
                                }
                                catch (Exception)
                                {
                                    success = false;
                                    // throw;
                                }
                                Logger.LogMe("END  Exec : " + messageId, false, RabbitMQLogFILE);
                                Logger.LogMe("END  Exec : " + messageId + " , Log:" + log, false, RabbitMQLogFILE);

                                //LogDoneItemInMemory();



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
#if false
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


#endif
        public void Exec(CustomRabbitMQQueue customRabbitMQQueue, QueueDetails queue, AnalyzeQueueRepository analyzeQueueRepository, string communicationLogId, int tenant , string message, out string log, out bool success)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                var serviceAnalyzer = customRabbitMQQueue.GetCustomAnalyzerQueueService(queue);
                //ArtemusAnalyzer analyzer = new Artemus(analyzeQueue, analyzeQueueRepository);
                serviceAnalyzer.Run( analyzeQueueRepository, tenant , communicationLogId , message , queue, out  log, out success);
                scope.Complete();
            }
        }



 
        public void WorkOnce()
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
