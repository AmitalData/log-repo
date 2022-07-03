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
            _connection = GetConnection();


        }
        public static int GetCounter()
        {
            return _Counter;
        }
        private IConnection GetConnection()
        {
            
            var factory = RabbitmqHelper.GetConnectionFactory(true);
            return factory.CreateConnection();
        }

        public IModel Create()
        {
            _Counter++;
            return _connection.CreateModel();
        }

        public bool Return(IModel obj)
        {
            if (obj.IsOpen)
            {
                return true;
            }
            else
            {
                obj?.Dispose();
                return false;
            }
        }
    }


    public class PooledRabbitMQPublisher //: IRabbitMQPublisher
    {

        static PooledRabbitMQPublisher _Instance;

        public static PooledRabbitMQPublisher Instance
        {
            get { return PooledRabbitMQPublisher._Instance = PooledRabbitMQPublisher._Instance ?? new PooledRabbitMQPublisher(); }

        }


        private readonly DefaultObjectPool<IModel> _objectPool;

        PooledRabbitMQPublisher(/*IPooledObjectPolicy<IModel> objectPolicy*/)
        {
            _objectPool = new DefaultObjectPool<IModel>(new PooledRabbitMQPolicy(),
                Environment.ProcessorCount * 2
                );
        }
         

        public IModel Get()
        {
            var channel = _objectPool.Get();
            return channel;
        }

        public void Return(IModel channel)
        {
            _objectPool.Return(channel);
        }


        

    }
}
