using System.Configuration;

namespace Logitude.Server.Tools.Messages
{
    public static class KafkaCredentials
    {
        public static string BrokerList { get; set; }
        public static string ConnectionString { get; set; }
        public static string Topic { get; set; }
        public static string ConsumerGroup { get; set; }

        public static void SetEventHubConfigurations()
        {
            BrokerList = ConfigurationManager.AppSettings["EH_FQDN"];
            ConnectionString = ConfigurationManager.AppSettings["EH_CONNECTION_STRING"];
            Topic = ConfigurationManager.AppSettings["EH_Topic"];
            ConsumerGroup = ConfigurationManager.AppSettings["CONSUMER_GROUP"];
        }
    }
}
