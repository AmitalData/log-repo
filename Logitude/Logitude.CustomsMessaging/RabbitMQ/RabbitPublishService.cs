using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.RabbitMQ
{
    public class RabbitPublishService
    {

        public void Publish(byte[] message, string communicationLogId, 
            string InterfaceTypeCode, 
            string rabbitMQCode, int messagePriority)
        {

            if (!string.IsNullOrWhiteSpace(InterfaceTypeCode))
            {
                throw new ArgumentException("InterfaceTypeCode");
            }
            if (!string.IsNullOrWhiteSpace(rabbitMQCode))
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

            
            var factory = RabbitmqHelper.GetConnectionFactory();

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

            }
        }


        
    }
}
