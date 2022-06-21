using System.Configuration;

namespace Logitude.Server.Tools.Messages
{
    public static class KafkaCredentials
    {
        public static string BrokerList { get; set; }
        public static string ConnectionString { get; set; }

        public static string TestingBrokerList { get; set; }
        public static string TestingConnectionString { get; set; }

        public static void SetEventHubConfigurations()
        {
            BrokerList = ConfigurationManager.AppSettings["EH_FQDN"];
            ConnectionString = ConfigurationManager.AppSettings["EH_CONNECTION_STRING"];
        }

        public static void SetTestingEventHubConfigurations()
        {
            TestingBrokerList = ConfigurationManager.AppSettings["TESTING_EH_FQDN"];
            TestingConnectionString = ConfigurationManager.AppSettings["TESTING_EH_CONNECTION_STRING"];
        }
    }
}
