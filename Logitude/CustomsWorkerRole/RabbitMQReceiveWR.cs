
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


        //public override bool OnStart()
        //{
        //    try
        //    {

        //        Logger.LogMe("SSSSS", false, "TESTELISH");

        //        //if (_OnStartDone) return true;
        //        //_OnStartDone = true;
        //        //DoneItemsInRange = new Dictionary<DateTime, int>();

        //        //if (WorkerRoleServiceLocator.PleaseShutDown) return true;



        //        var factory = new ConnectionFactory() { HostName = "unimq", UserName = "v5101", Password = "Aa123" };
        //        using (var connection = factory.CreateConnection())
        //        using (var channel = connection.CreateModel())
        //        {
        //            Logger.LogMe("create---", false, "TESTELISH");

        //            channel.QueueDeclare(queue: "connectToTicket",
        //                                 durable: false,
        //                                 exclusive: false,
        //                                 autoDelete: false,
        //                                 arguments: null);

        //            var consumer = new EventingBasicConsumer(channel);
        //            consumer.Received += (model, ea) =>
        //            {
        //                Logger.LogMe("recievd", false, "TESTELISH");
        //                 var body = ea.Body.ToArray();
        //                var message = Encoding.UTF8.GetString(body);

        //                UniCourierBatchSendUCBUD2LT_MsgResponseService uniCourierBatchSendUCBUD2LT_MsgResponseService = new UniCourierBatchSendUCBUD2LT_MsgResponseService();

        //                XmlSerializer serializer = new XmlSerializer(typeof(DCAInUCBUD2LTWithResponseContentHeader));
        //                DCAInUCBUD2LTWithResponseContentHeader result = new DCAInUCBUD2LTWithResponseContentHeader();
        //                using (TextReader reader = new StringReader(message))
        //                {
        //                    Logger.LogMe("read xml", false, "TESTELISH");

        //                    XmlDocument doc = new XmlDocument();
        //                doc.Load(reader);

        //                //Display all the book titles.
        //                XmlNodeList elemList = doc.GetElementsByTagName("Body");


        //                    result = (DCAInUCBUD2LTWithResponseContentHeader)serializer.Deserialize(new StringReader(elemList[0].InnerXml));

        //                //    dynamic test = XmlGenericUtil<dynamic>.DeSerializeObject(elemList[0].InnerXml);//serializer.Deserialize(reader);
        //                //    result = (DCAInUCBUD2LTWithResponseContentHeader)test.body.DCAInUCBUD2LTWithResponseContentHeader;
        //                 }

        //                Logger.LogMe("update", false, "TESTELISH");

        //                uniCourierBatchSendUCBUD2LT_MsgResponseService.RealUpdate2(result);

        //                Console.WriteLine(" [x] Received {0}", message);
        //            };

        //            Logger.LogMe("BasicConsume", false, "TESTELISH");

        //            channel.BasicConsume(queue: "connectToTicket",
        //                                 autoAck: true,
        //                                 consumer: consumer);

        //            Logger.LogMe("end BasicConsume", false, "TESTELISH");

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "amital send data worker role start", null, null);
        //    }

        //    // Set the maximum number of concurrent connections 
        //    ServicePointManager.DefaultConnectionLimit = 12;

        //    //DiagnosticMonitor.Start("DiagnosticsConnectionString");

        //    // For information on handling configuration changes
        //    // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.


        //    return base.OnStart();
        //}


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
                            var factory = new ConnectionFactory() { HostName = "unimq", UserName = "v5101", Password = "Aa123" };
                            using (var connection = factory.CreateConnection())
                            using (var channel = connection.CreateModel())
                            {


                            //    using (var connection = factory.CreateConnection())
                            //using (var channel = connection.CreateModel())
                            //{
                                Logger.LogMe("CONNECTION", false, "TESTELISH");

                                channel.BasicQos(0, 5, true);
                                //Create queue if not exists
                                RabbitmqHelper.DeclareQueue(channel, queue.Code);

                                AnalyzeQueueRepository analyzeQueueRepository = new AnalyzeQueueRepository();

                                //while (true)
                                //{
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
                                        string remark;
                                        var message =  Encoding.UTF8.GetString(body);
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

                                    catch(Exception ex)
                                    {

                                        Logger.LogMe(ex.Message, false, "TESTELISH");

                                    }

                                };
                                // }


                                Thread.Sleep(500);
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

                WorkUntil_AnalyzeQueue_Empty_Db_NOTINUSE();


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
