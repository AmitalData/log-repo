
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
using CustomsWorkerRole.RabbitMQ;

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
            WorkUntil_AnalyzeQueue_Empty_Db_NOTINUSE();
                return true;

        }

 

        private void WorkUntil_AnalyzeQueue_Empty_Db_NOTINUSE()
        {
            Logger.LogMe("START", false, "TESTELISH");
            var customRabbitMQQueue = new CustomRabbitMQQueue();
            var _CustomsAnalyzeQueueServices = customRabbitMQQueue.GetAllQueueDetails()
             .Where(r => r.AnalyzeQueueService != AnalyzeMQQueueServiceEnum.none)
            .ToList();

            while (true)
            {

                EventHandler<BasicDeliverEventArgs> consumerEventArgs = null;
                EventingBasicConsumer consumer = null;

              //  var factory = RabbitmqHelper.GetConnectionFactory();
                //factory.RequestedHeartbeat = TimeSpan.FromSeconds(600);
         
           foreach (QueueDetails queue in _CustomsAnalyzeQueueServices)
                {

                    {


                        try
                        {
                            var factory = new ConnectionFactory() { HostName = "unimq", UserName = "v5101", Password = "Aa123" ,   RequestedConnectionTimeout=new TimeSpan(600000000) };
                            using (var connection = factory.CreateConnection())
                            using (var channel = connection.CreateModel())
                            {
                                try
                                {
                                    Logger.LogMe("CONNECTION", false, "TESTELISH");
                                   
                                     channel.BasicQos(0, 5, true);
                                    //Create queue if not exists
                                     RabbitmqHelper.DeclareQueue(channel, queue.Code);
                                     AnalyzeQueueRepository analyzeQueueRepository = new AnalyzeQueueRepository();

                                
                                     consumerEventArgs = (model, ea) =>
                                    {
                                        //if (channel == null)
                                        //    return;
                                        //if (!channel.IsOpen)
                                        //    return;

                                        string messageId = "";
                                        try
                                        {
                                            var body = ea.Body.ToArray();
                                            string remark;
                                            var message = Encoding.UTF8.GetString(body);
                                            messageId = ea.BasicProperties.MessageId;
                                            Logger.LogMe("RUN", false, "TESTELISH");

                                            Exec(customRabbitMQQueue, queue, analyzeQueueRepository, messageId, 1 , message);
                                            LogDoneItemInMemory();



                                            if (true)
                                            {
                                                Logger.LogMe("BasicAck", false, "TESTELISH");

                                                channel.BasicAck(ea.DeliveryTag, false);
                                            }

                                        }

                                        catch (Exception ex)
                                        {

                                            Logger.LogMe(ex.Message, false, "TESTELISH");

                                        }

                                    };

                                    consumer = new EventingBasicConsumer(channel);
                                    consumer.Received += consumerEventArgs;

                                    channel.BasicConsume(queue: queue.Code  ,
                                                        autoAck: false,
                                                        consumer: consumer);
 

                                    Thread.Sleep(500);

                                }

                                catch (Exception e)
                                {

                                }

                                finally
                                {
                                   //channel.Close();
                                   // connection.Close();
                                }
                            
                            }
                        }

  
                        catch (Exception e)
                        {
                            Logger.LogMe(e.Message, false, "TESTELISH");

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

        private void Exec(CustomRabbitMQQueue customRabbitMQQueue, QueueDetails queue, AnalyzeQueueRepository analyzeQueueRepository, string communicationLogId, int tenant , string message)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                var serviceAnalyzer = customRabbitMQQueue.GetCustomAnalyzerQueueService(queue);
                //ArtemusAnalyzer analyzer = new Artemus(analyzeQueue, analyzeQueueRepository);
                serviceAnalyzer.Run( analyzeQueueRepository, tenant , communicationLogId , message);
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
