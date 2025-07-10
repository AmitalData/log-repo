using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.RabbitMQ
{
    public class SingletonRabbitMQPublisher
    {

        static SingletonRabbitMQPublisher _Instance = null;
        private static readonly object padlock = new object();
        

        public static SingletonRabbitMQPublisher Instance
        {
            ///get { return SingletonRabbitMQPublisher._Instance = SingletonRabbitMQPublisher._Instance ?? new SingletonRabbitMQPublisher(); }
            get
            {
                if (_Instance == null)
                {
                    lock (padlock)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new SingletonRabbitMQPublisher();
                        }
                    }
                }
                return _Instance;
            }
        }


        private readonly MyPoolRabbitMQ _objectPool;

        SingletonRabbitMQPublisher(/*IPooledObjectPolicy<IModel> objectPolicy*/)
        {
            _objectPool = new MyPoolRabbitMQ();
        }



        internal void PoolPublish(byte[] message, string communicationLogId, string interfaceTypeCode, string rabbitMQCode, int messagePriority)
        {
            _objectPool.BasicPublish(message, communicationLogId, interfaceTypeCode, rabbitMQCode, messagePriority);
        }





    }

    class MyPoolRabbitMQ //: IPooledObjectPolicy<IModel>
    {
        private static readonly object _objLock = new object();
        private static readonly object _objLockChannel = new object();
        //private readonly RabbitMQOptions _options;
        static int _Counter = 0;
        private static IConnection _connection;
        private static Dictionary<string, Queue<IModel>> _QueueChannel = new Dictionary<string, Queue<IModel>>();
        const string exchangeName = "direct_logs";

        public MyPoolRabbitMQ(/*IOptions<RabbitMQOptions>  options*/)
        {

            GetConnection();


        }
        public static int GetCounter()
        {
            return _Counter;
        }
        private IConnection GetConnection()
        {
            lock (_objLock)
            {
                if (_connection == null /*|| _connection?.IsOpen != true*/)
                {
                    //try
                    //{
                    //    _connection?.Dispose();
                    //}
                    //catch
                    //{


                    //}
                    try
                    {
                        var factory = RabbitmqHelper.GetConnectionFactory(true);
                        _connection = factory.CreateConnection();
                    }
                    catch (Exception)
                    {

                        _connection = null;
                        Thread.Sleep(1000);
                        throw;
                    }


                }
                return _connection;


            }
        }
        public void BasicPublish(byte[] message, string communicationLogId, string InterfaceTypeCode, string rabbitMQCode, int messagePriority)
        {
            var channel = this.Create(rabbitMQCode);
            try
            {


                var header = new Dictionary<string, object>();
                header.Add("InterfaceTypeCode", InterfaceTypeCode);
                var prop = channel.CreateBasicProperties();
                prop.Persistent = true;
                prop.MessageId = communicationLogId;
                prop.DeliveryMode = 2; //persistent
                prop.Headers = header;
                prop.Priority = (byte)messagePriority;


                channel.BasicPublish(exchange: exchangeName,
                    mandatory: false,
                                             routingKey: rabbitMQCode,
                                             basicProperties: prop,
                                             body: message);


               NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"RABBITMQ.BasicPublish {rabbitMQCode}");
            }
            finally
            {
                this.Return(rabbitMQCode, channel);
            }
        }
        public IModel Create(string rabbitMQCode)
        {

            IModel channel = null;
            lock (_objLockChannel)
            {
                if (!_QueueChannel.ContainsKey(rabbitMQCode))
                {
                    channel = CreateNew(rabbitMQCode);
                    var q = new Queue<IModel>();
                    
                    RabbitmqHelper.DeclareQueue(channel, rabbitMQCode, true);
                    q.Enqueue(channel);
                    _QueueChannel.Add(rabbitMQCode, q);
                }
                while (_QueueChannel[rabbitMQCode].Count > 0)
                {
                    channel = _QueueChannel[rabbitMQCode].Dequeue();
                    if (channel?.IsClosed==true)
                    {
                        try
                        {
                            channel?.Dispose();
                            Thread.Sleep(500);
                        }
                        catch {}



                    }
                    else
                    {
                        return channel;
                    }
                }
                return CreateNew(rabbitMQCode);

            }
        }
        private static IModel CreateNew(string rabbitMQCode)
        {
            var channel1 = _connection.CreateModel();
            channel1.QueueBind(queue: rabbitMQCode, exchange: exchangeName, routingKey: rabbitMQCode);
            return channel1;
        }

        public bool Return(string rabbitMQCode, IModel channel)
        {
            if (channel!=null)
            {
                lock (_objLockChannel)
                {
                    _QueueChannel[rabbitMQCode].Enqueue(channel);
                }

            }
            return true;
        }
    }
}
