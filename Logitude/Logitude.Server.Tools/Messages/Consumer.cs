using Confluent.Kafka;
using System.Collections.Generic;

namespace Logitude.Server.Tools.Messages
{
    public class Consumer
    {
        public IConsumer<long, string> ConsumerSubscription { get; set; }
        private int Timeout = 30;

        public Consumer(string consumerGroup, List<string> topics, TopicPartition partition)
        {
            KafkaCredentials.SetEventHubConfigurations();
            ConsumerSubscription = SubscribeToTopic(consumerGroup, topics, partition);
        }

        public void SetTimeOut(int timeout)
        {
            Timeout = timeout;
        }

        private IConsumer<long, string> SubscribeToTopic(string consumerGroup, List<string> topics, TopicPartition partition)
        {
            ConsumerConfig config = GetConsumerConfigurations(consumerGroup);
            IConsumer<long, string> consumer = new ConsumerBuilder<long, string>(config)
                .SetKeyDeserializer(Deserializers.Int64)
                .SetValueDeserializer(Deserializers.Utf8)
                .Build();

            if (topics.Count > 0)
                consumer.Subscribe(topics);

            if (partition != null)
                consumer.Assign(partition);

            return consumer;
        }

        private ConsumerConfig GetConsumerConfigurations(string consumerGroup)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = KafkaCredentials.BrokerList,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SocketTimeoutMs = 60000,
                SessionTimeoutMs = 30000,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = "$ConnectionString",
                SaslPassword = KafkaCredentials.ConnectionString,
                GroupId = consumerGroup,
                AutoOffsetReset = AutoOffsetReset.Latest,
            };
            return config;
        }

        public ConsumeResult<long, string> Consume()
        {
            return ConsumerSubscription.Consume(Timeout);
        }
    }
}