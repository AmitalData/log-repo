using Confluent.Kafka;

namespace Logitude.Server.Tools.Messages
{
    public class Consumer
    {
        public IConsumer<long, string> ConsumerSubscription { get; set; }
        public Consumer()
        {
            KafkaCredentials.SetEventHubConfigurations();
            ConsumerSubscription = SubscribeToTopic(KafkaCredentials.Topic);
        }

        private IConsumer<long, string> SubscribeToTopic(string topic)
        {
            ConsumerConfig config = GetConsumerConfigurations();
            IConsumer<long, string> consumer = new ConsumerBuilder<long, string>(config).SetKeyDeserializer(Deserializers.Int64).SetValueDeserializer(Deserializers.Utf8).Build();
            //CancellationTokenSource cts = new CancellationTokenSource();
            //Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); }; 
            consumer.Subscribe(topic);
            return consumer;
        }

        private ConsumerConfig GetConsumerConfigurations()
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
                //SslCaLocation = caCertLocation,
                GroupId = KafkaCredentials.ConsumerGroup,
                AutoOffsetReset = AutoOffsetReset.Latest,
                //BrokerVersionFallback = "1.0.0"
            };
            return config;
        }

        public ConsumeResult<long, string> Consume()
        {
            return ConsumerSubscription.Consume();
        }
    }
}