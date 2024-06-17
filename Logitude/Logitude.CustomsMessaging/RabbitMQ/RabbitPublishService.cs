using Logitude.Server.Tools.Utils;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.RabbitMQ
{
    public class RabbitPublishService
    {

        static Semaphore _SemaphoreObject = new Semaphore(initialCount: 10, maximumCount: 10, name: "RabbitPublishService");
        public  bool Publish(byte[] message, string communicationLogId,
    string InterfaceTypeCode,
    string rabbitMQCode, int messagePriority)
        {

            if (string.IsNullOrWhiteSpace(InterfaceTypeCode))
            {
                throw new ArgumentException("InterfaceTypeCode");
            }
            if (string.IsNullOrWhiteSpace(rabbitMQCode))
            {
                throw new ArgumentException("rabbitMQCode");
            }

            if (messagePriority < 1)
            {
                messagePriority = 6;
            }
            if (messagePriority > 10)
            {
                messagePriority = 9;
            }



            //var args = new Dictionary<string, object>();
            bool usePooledRabbitMQPublisher = !String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["RabbitMQPublisherUsePooled"]);
            if (usePooledRabbitMQPublisher)
            {
                if (true)
                {
                    //SingletonRabbitMQPublisher.Instance
                    //    .PoolPublish(message, communicationLogId, InterfaceTypeCode, rabbitMQCode, messagePriority);
                    RabbitPublishWorker.DoOne(new RabbitQueue()
                    {
                        message = message,
                        communicationLogId = communicationLogId,
                        InterfaceTypeCode = InterfaceTypeCode,
                        rabbitMQCode = rabbitMQCode,
                        messagePriority = messagePriority
                    });
                    Thread.Sleep(50);

                }
                else
                {
                    PoolPublish(message, communicationLogId, InterfaceTypeCode, rabbitMQCode, messagePriority);
                }


            }
            else
            {

                //bool isSignalled = _SemaphoreObject.WaitOne(TimeSpan.FromSeconds(30));
                //if (!isSignalled)
                //{
                //    throw new Exception("RabbitPublishService.semaphoreObject.WaitOne 10 sec");
                //}
                try
                {
                    var factory = RabbitmqHelper.GetConnectionFactory(true);

                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        BasicPublish(message, communicationLogId, InterfaceTypeCode, rabbitMQCode, messagePriority, channel);
                        NetCommonHelper.Logger.DevLog.Instance.WriteInfo($"regelarRabbitMQPublisher" + ":" + rabbitMQCode);
                    }
                    Thread.Sleep(100);//better slowly dispose connection than crash!!
                }
                finally
                {
                    //_SemaphoreObject.Release();
                }
            }
            return true;

        }

        private static void PoolPublish(byte[] message, string communicationLogId, string InterfaceTypeCode, string rabbitMQCode, int messagePriority)
        {
            bool pool = true;
            IModel channel = null;
            channel = PooledRabbitMQPublisher.Instance.Get();

            //var 
            bool haveExc = false;
            try
            {
                BasicPublish(message, communicationLogId, InterfaceTypeCode, rabbitMQCode, messagePriority, channel);

                NetCommonHelper.Logger.DevLog.Instance.WriteInfo($"PooledRabbitMQPublisher:{PooledRabbitMQPolicy.GetCounter()}" + ":" + rabbitMQCode);
            }
            catch (Exception ex)
            {
                haveExc = true;
                try
                {

                    channel?.Dispose();

                }
                catch (Exception)
                {

                    ///throw;
                }
                throw ex;
            }
            finally
            {
                //if (!haveExc)
                {
                    PooledRabbitMQPublisher.Instance.Return(channel);

                }

            }
        }

        public static void BasicPublish(byte[] message, string communicationLogId, string InterfaceTypeCode, string rabbitMQCode, int messagePriority, IModel channel)
        {
            //channel.BasicQos(0, 5, true);


            RabbitmqHelper.DeclareQueue(channel, rabbitMQCode, true);


            var header = new Dictionary<string, object>();
            header.Add("InterfaceTypeCode", InterfaceTypeCode);
            var prop = channel.CreateBasicProperties();
            prop.Persistent = true;
            prop.MessageId = communicationLogId;
            prop.DeliveryMode = 2; //persistent
            prop.Headers = header;
            prop.Priority = (byte)messagePriority;


            channel.BasicPublish(exchange: "",
                mandatory: false,
                                         routingKey: rabbitMQCode,
                                         basicProperties: prop,
                                         body: message);


            Debug.WriteLine($"RABBITMQ.BasicPublish {rabbitMQCode}");
        }

        private void PublishOld(byte[] message, string communicationLogId,
            string InterfaceTypeCode,
            string rabbitMQCode, int messagePriority)
        {

            if (string.IsNullOrWhiteSpace(InterfaceTypeCode))
            {
                throw new ArgumentException("InterfaceTypeCode");
            }
            if (string.IsNullOrWhiteSpace(rabbitMQCode))
            {
                throw new ArgumentException("rabbitMQCode");
            }

            if (messagePriority < 1)
            {
                messagePriority = 6;
            }
            if (messagePriority > 10)
            {
                messagePriority = 9;
            }
            var args = new Dictionary<string, object>();


            var factory = RabbitmqHelper.GetConnectionFactory(tryFromAppSettings: true);

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.BasicQos(0, 5, true);


                RabbitmqHelper.DeclareQueue(channel, rabbitMQCode, true);


                var header = new Dictionary<string, object>();
                header.Add("InterfaceTypeCode", InterfaceTypeCode);
                var prop = channel.CreateBasicProperties();
                prop.Persistent = true;
                prop.MessageId = communicationLogId;
                prop.DeliveryMode = 2; //persistent
                prop.Headers = header;
                prop.Priority = (byte)messagePriority;


                channel.BasicPublish(exchange: "",
                                             routingKey: rabbitMQCode,
                                             basicProperties: prop,
                                             body: message);
                Debug.WriteLine($"RABBITMQ.BasicPublish {rabbitMQCode}");

            }
        }



    }
}
