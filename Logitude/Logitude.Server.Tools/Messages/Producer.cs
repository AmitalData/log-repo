using Confluent.Kafka;
using Simplog.Server.Infrastructure;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Messages
{
    public class Producer
    {
        public IProducer<long, string> ProducerBuilder { get; set; }
        public IProducer<long, string> TestingProducer { get; set; }


        public Producer()
        {
            KafkaCredentials.SetEventHubConfigurations();
            ProducerBuilder = BuildProducer();
            if (IsProductionEnvironment())
            {
                KafkaCredentials.SetTestingEventHubConfigurations();
                TestingProducer = BuildTestingProducer();
            }
        }

        private bool IsProductionEnvironment()
        {
            return LogitudeSettings.DeploymentStage == "Simplog";
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

        public DeliveryResult<long, string> Produce(string topic, long key, string logitudeUpdateMessage)
        {
            if (IsProductionEnvironment())
            {
                TestingProducer.ProduceAsync(topic, new Message<long, string> { Key = key, Value = logitudeUpdateMessage }).GetAwaiter().GetResult();
            }
            return ProducerBuilder.ProduceAsync(topic, new Message<long, string> { Key = key, Value = logitudeUpdateMessage }).GetAwaiter().GetResult();
        }

        public DeliveryResult<long, string> Produce(TopicPartition topicPartition, long key, string logitudeUpdateMessage)
        {
            if (IsProductionEnvironment())
            {
                TestingProducer.ProduceAsync(topicPartition, new Message<long, string> { Key = key, Value = logitudeUpdateMessage }).GetAwaiter().GetResult();
            }
            return ProducerBuilder.ProduceAsync(topicPartition, new Message<long, string> { Key = key, Value = logitudeUpdateMessage }).GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            ProducerBuilder.Flush();
            ProducerBuilder.Dispose();
            if (IsProductionEnvironment())
            {
                TestingProducer.Flush();
                TestingProducer.Dispose();
            }

        }
    }
}
