using Confluent.Kafka;
using Dropbox.Api.Sharing;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Microsoft.AspNet.SignalR.Messaging;
using Simplog.Server.Infrastructure;
using System;
using System.Threading.Tasks;
using static Confluent.Kafka.ConfigPropertyNames;

namespace Logitude.Server.Tools.Messages
{
    public class Producer : IDisposable
    {
        public IProducer<long, string> ProducerBuilder { get; set; }
        public IProducer<long, string> TestingProducer { get; set; }
       
        public Producer()
        {
            KafkaCredentials.SetEventHubConfigurations();
            ProducerBuilder = BuildProducer();

        }

        
        private IProducer<long, string> BuildProducer()
        {
            ProducerConfig config = GetProducerConfigurations();
            return new ProducerBuilder<long, string>(config)
                .SetKeySerializer(Serializers.Int64)
                .SetValueSerializer(Serializers.Utf8)
                .Build();
        }

        private IProducer<long, string> BuildTestingProducer()
        {
            ProducerConfig config = GetTestingProducerConfigurations();
            return new ProducerBuilder<long, string>(config)
                .SetKeySerializer(Serializers.Int64)
                .SetValueSerializer(Serializers.Utf8)
                .Build();
        }
        private ProducerConfig GetProducerConfigurations()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = KafkaCredentials.BrokerList,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = "$ConnectionString",
                SaslPassword = KafkaCredentials.ConnectionString,
            };
            return config;
        }
        private ProducerConfig GetTestingProducerConfigurations()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = KafkaCredentials.TestingBrokerList,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = "$ConnectionString",
                SaslPassword = KafkaCredentials.TestingConnectionString,
            };
            return config;
        }

        public DeliveryResult<long, string> Produce(string topic, long key, string logitudeUpdateMessage,bool useSync = false)
        {
            if (useSync)
            {
               
                ProducerBuilder.Produce(topic, new Message<long, string> { Key = key, Value = logitudeUpdateMessage });
                return new DeliveryResult<long, string>();
            }
           
            return ProducerBuilder.ProduceAsync(topic, new Message<long, string> { Key = key, Value = logitudeUpdateMessage }).GetAwaiter().GetResult();
        }

        public DeliveryResult<long, string> Produce(TopicPartition topicPartition, long key, string logitudeUpdateMessage, bool useSync = false)
        {
            if (useSync)
            {
                
                ProducerBuilder.Produce(topicPartition, new Message<long, string> { Key = key, Value = logitudeUpdateMessage });
                return new DeliveryResult<long, string>();
            }
           
            return ProducerBuilder.ProduceAsync(topicPartition, new Message<long, string> { Key = key, Value = logitudeUpdateMessage }).GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            ProducerBuilder.Flush();
            ProducerBuilder.Dispose();
           

        }
    }
}
