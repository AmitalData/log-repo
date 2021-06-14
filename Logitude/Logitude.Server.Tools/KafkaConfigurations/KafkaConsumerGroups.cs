namespace Logitude.Server.Tools.KafkaConfigurations
{ 
    public class KafkaConsumerGroups
    {
        public static string Default = "$Default";
        public static string TaskDone = "taskdone";
        public static string WorkflowStarter = "workflowstarter";
        public static string CreateTask = "createconsumer";
        public static string UpdateTask = "updateconsumer";
        public static string LogitudePorts = "ports";
        public static string LogitudeCountries = "countries";
        public static string LogitudeContacts = "contacts";
        public static string LogitudeCards = "cards";
    }
}
