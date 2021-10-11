using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomsWorkerRole.RabbitMQ
{
  public  class RabbitmqConnection
    {

       public IModel Channel = null;
       public IConnection Connection = null;
       public EventingBasicConsumer Consumer = null;
       public DateTime ChannelCreateDate;
       public DateTime LastConnectionCheck;
      // public ThreadParam ThreadParam;
       public bool SuccessfullChannelQueueDefinition = false;
       public int Counter = 0;
       public object Obj = new object();
    }
}
