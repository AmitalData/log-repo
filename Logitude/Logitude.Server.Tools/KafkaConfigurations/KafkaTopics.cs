namespace Logitude.Server.Tools.KafkaConfigurations
{
    public class KafkaTopics
    {
        public static string Default = "CTool";
        public static string ShipmentsUpdateTopic = "shipmentsupdate_topic";
        public static string ShipmentsCreateTopic = "shipmentscreate_topic";
        public static string TasksUpdateTopic = "tasksupdate_topic";
        public static string TasksCreateTopic = "taskscreate_topic";
        public static string LookupsTopic = "lookups_topic";
    }
}
