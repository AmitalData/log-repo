using Confluent.Kafka;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Messages
{
    public class Producer
    {
        public IProducer<long, string> ProducerBuilder { get; set; }

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

        public Task<DeliveryResult<long, string>> Produce(string topic, long key, string logitudeUpdateMessage)
        {
            return ProducerBuilder.ProduceAsync(topic, new Message<long, string> { Key = key, Value = logitudeUpdateMessage });
        }

        public Task<DeliveryResult<long, string>> Produce(TopicPartition topicPartition, long key, string logitudeUpdateMessage)
        {
            return ProducerBuilder.ProduceAsync(topicPartition, new Message<long, string> { Key = key, Value = logitudeUpdateMessage });
        }
    }
}
