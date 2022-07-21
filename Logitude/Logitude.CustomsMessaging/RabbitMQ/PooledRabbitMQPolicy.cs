using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.RabbitMQ
{
    public class PooledRabbitMQPolicy : IPooledObjectPolicy<IModel>
    {
        private static readonly object _objLock = new object();
        //private readonly RabbitMQOptions _options;
        static int _Counter = 0;
        private IConnection _connection;
        //private readonly string _HostName;
        //private readonly string _UserName;
        //private readonly string _Password;
        //private IEnvironmentSettingService _environmentSettingService;

        public PooledRabbitMQPolicy(/*IOptions<RabbitMQOptions>  options*/)
        {
            //_environmentSettingService = serviceProvider.GetRequiredService<IEnvironmentSettingService>();
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
                if (_connection ==null || _connection?.IsOpen!=true)
                {
                    try
                    {
                        _connection?.Dispose();
                    }
                    catch 
                    {

                        
                    }
                    
                    var factory = RabbitmqHelper.GetConnectionFactory(true);
                    _connection = factory.CreateConnection();

                }
                return _connection;


            }
        }

        public IModel Create()
        {
            

            _Counter++;
            return GetConnection().CreateModel();
        }

        public bool Return(IModel channel)
        {
            if (channel !=null && channel.IsOpen)
            {
                return true;
            }
            else
            {
                channel?.Dispose();
                return false;
            }
        }
    }


    public class PooledRabbitMQPublisher //: IRabbitMQPublisher
    {
        ///https://csharpindepth.com/articles/singleton <summary>
        /// 
        
        static PooledRabbitMQPublisher _Instance=null;
        private static readonly object padlock = new object();
        private static readonly object getlock = new object();


        public static PooledRabbitMQPublisher Instance
        {
            ///get { return PooledRabbitMQPublisher._Instance = PooledRabbitMQPublisher._Instance ?? new PooledRabbitMQPublisher(); }
            get
            {
                if (_Instance == null)
                {
                    lock (padlock)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new PooledRabbitMQPublisher();
                        }
                    }
                }
                return _Instance;
            }
        }


        private readonly DefaultObjectPool<IModel> _objectPool;

        PooledRabbitMQPublisher(/*IPooledObjectPolicy<IModel> objectPolicy*/)
        {
            _objectPool = new DefaultObjectPool<IModel>(new PooledRabbitMQPolicy(),
                100//Environment.ProcessorCount * 2
                );
        }
         

        public IModel Get()
        {
            lock (getlock)
            {
                var channel = _objectPool.Get();
                return channel;

            }
        }

        public void Return(IModel channel)
        {
            _objectPool.Return(channel);
        }


        

    }
}
