using Logitude.CustomsMessaging.RabbitMQ;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.Utils;
using Logitude.SystemLogs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace CustomsWorkerRole
{
    public class RabbitMQConsumerService
    {
        private string _myClass;
        private CustomDbQueueService _CustomDbQueueService;
        private string _RabbitQueueCode;
        private DateTime _LastReprtAt= DateTime.MinValue;
        public RabbitMQConsumerService(string myClass, string RabbitQueueCode)
        {
            _myClass = myClass;
            _CustomDbQueueService = new CustomDbQueueService(myClass, 0);
            this._RabbitQueueCode = RabbitQueueCode;
        }

        public void WorkUntilPrcossesStop_RabbitMQ(Func<CustomDBQueueMessage,bool> ProcessMessage, Action LogDoneItemInMemory)
        {
            bool isConnectionShutdown = false;
            EventHandler<BasicDeliverEventArgs> consumerEventArgs = null;
            try
            {
                DateTime lastworkAt = DateTime.Now;
                string RabbitMQLogFILE = "RabbitMQLog"+ _myClass;
                if (ProcessMessage==null)
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteError("ProcessMessage_Db ==null"+ ":rabbitmq");
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", $"{_myClass}:WorkerRoleRabbitMQ", $"ProcessMessage_Db == null", null);
                    Thread.Sleep(5000);
                    return;
                }
                string myRabbitQueueCode = RabbitQueueCodeService.GetRabbitQueueCode(_RabbitQueueCode, "" /*base.QueueGroupCodeRabbit*/);
                var factory = RabbitmqHelper.GetConnectionFactory(tryFromAppSettings: false);
                //var factory = new ConnectionFactory() { HostName = "unimq", UserName = "v5101", Password = "Aa123"  };
                factory.RequestedHeartbeat = TimeSpan.FromMinutes(10);
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    try
                    {

                        connection.ConnectionShutdown += Connection_ConnectionShutdown;
                        NetCommonHelper.Logger.DevLog.Instance.WriteInfo($"CONNECTION:{myRabbitQueueCode}"+":"+ RabbitMQLogFILE);
                        channel.BasicQos(0, 5, true);
                        RabbitmqHelper.DeclareQueue(channel, myRabbitQueueCode, true);



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
                                lastworkAt = DateTime.Now;
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
                                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", $"{_myClass}:RabbitMQ", $"BasicProperties.Headers[que_id] is null  ", null);
                                    NetCommonHelper.Logger.DevLog.Instance.WriteError("No interfaceTypeCode in header " + messageId+":" +RabbitMQLogFILE);
                                    Thread.Sleep(1000);

                                    return;
                                }

                                LogMessagingUtil.Instance.AppendLine($"GetRabbitMQPseudoByMessageId({longQId})");
                                customDBQueueMessage = _CustomDbQueueService.GetRabbitMQPseudoByMessageId(longQId);
                                if (customDBQueueMessage == null)
                                {
                                    channel.BasicAck(ea.DeliveryTag, false);
                                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", $"{_myClass}:WorkerRoleRabbitMQ", $"GetRabbitMQPseudoByMessageId not found({longQId})", null);
                                    NetCommonHelper.Logger.DevLog.Instance.WriteError($"GetRabbitMQPseudoByMessageId not found({longQId})" + ":" + RabbitMQLogFILE);
                                    Thread.Sleep(100);
                                    return;

                                }

                                
                                bool successProcessMessage = false;
                                using (TransactionScope queue_TransactionScope = TransactionFactory.GetTransaction())
                                {
                                    try
                                    {
                                        successProcessMessage = ProcessMessage(customDBQueueMessage);
                                        
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
                                        //Logger.LogMe(e1.ToString(), true, "rabbitmq");
                                        ExceptionHandler.HandleException(e1, DateTime.Now, 0, "", $"{_myClass}:WorkerRoleRabbitMQ", $"WorkUntilPrcossesStop_RabbitMQ{messageId}", null);
                                        NetCommonHelper.Logger.DevLog.Instance.WriteError($"WorkUntilPrcossesStop_RabbitMQ{messageId}" + ":" + RabbitMQLogFILE);
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
                                                    Debug.WriteLineIf(isTimeToEndDueMaxTries, "isTimeToEndDueMaxTries==true");
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

                                            NetCommonHelper.Logger.DevLog.Instance.WriteFatal(eee, "rabbitmq");
                                            ExceptionHandler.HandleException(eee, DateTime.Now, 0, "", $"{_myClass}:WorkerRoleRabbitMQ", $"WorkUntilPrcossesStop_RabbitMQ{messageId}", null);
                                            Thread.Sleep(1000);
                                        }



                                        
                                    }
                                }





                                LogDoneItemInMemory?.Invoke();


                            }

                            catch (Exception ex)
                            {

                                //Logger.LogMe(ex.Message, false, RabbitMQLogFILE);
                                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRoleRabbitMQ", $"WorkUntilPrcossesStop_RabbitMQ{messageId}", null);
                                Thread.Sleep(1000);


                            }

                        };

                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += consumerEventArgs;
                        consumer.Shutdown += (sender, e) => {
                            isConnectionShutdown = true;
                            NetCommonHelper.Logger.DevLog.Instance.WriteInfo("Connection broke!" + ":" + RabbitMQLogFILE);
                        };

                        channel.BasicConsume(queue:myRabbitQueueCode /*this._RabbitQueueCode*/,
                                            autoAck: false,
                                            consumer: consumer);
                        Debug.WriteLine("Start BasicConsume " + myRabbitQueueCode);
                        while (!WorkerRoleServiceLocator.PleaseShutDown)
                        {
                            if (
                        isConnectionShutdown ||
                        connection?.IsOpen == false ||
                        channel?.IsOpen == false

                        )
                            {

                                NetCommonHelper.Logger.DevLog.Instance.WriteWarning("Connection broke!"+ RabbitMQLogFILE);
                                break;
                            }

                            if (DateTime.Now.Subtract(lastworkAt) > TimeSpan.FromMinutes(10))
                            {
                                Thread.Sleep(1000);
                                NetCommonHelper.Logger.DevLog.Instance.WriteWarning("No work (FromMinutes(10)) or connection fail ??! - dispose old create new one"+ RabbitMQLogFILE);
                                break;

                            }
                            Thread.Sleep(200);
                            bool getOut = false;
                            if (getOut)
                            {
                                break;
                            }
                            if (DateTime.Now.Subtract(_LastReprtAt) > TimeSpan.FromHours(1))
                            {
                                _LastReprtAt = DateTime.Now;
                                NetCommonHelper.Logger.DevLog.Instance.WriteInfo(this.GetType().FullName + ":Still Alive");
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

        private void Connection_ConnectionShutdown(object sender, RabbitMQ.Client.ShutdownEventArgs e)
        {
            ///throw new NotImplementedException();
        }
    }
}
