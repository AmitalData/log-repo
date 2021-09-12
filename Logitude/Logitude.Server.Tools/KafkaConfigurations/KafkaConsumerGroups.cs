namespace Logitude.Server.Tools.KafkaConfigurations
{ 
    public class KafkaConsumerGroups
    {
        public static string Default = "$Default";
        public static string TaskDone = "taskdone";
        public static string WorkflowStarter = "workflowstarter";
        public static string CreateTask = "createconsumer";
        public static string UpdateShipment = "updateconsumer";
        public static string LogitudePorts = "ports";
        public static string LogitudeCountries = "countries";
        public static string LogitudeVessels = "vessels";
        public static string LogitudeCurrencies = "currency";
        public static string LogitudeEntityStatus = "entitystatus";
        public static string LogitudeSpecialServicesTypes = "specialservicestype";
        public static string LogitudeContacts = "contacts";
        public static string LogitudeCards = "cards";
        public static string LogitudeDocumentTypes = "documenttypes";
        public static string LogitudePackageTypes = "packagetypes";
        public static string UpdateWorkflowsVariables = "updateworkflowsvariables";
        public static string ObjectFieldTrackChanges = "objectfieldtrackchanges";
        public static string LogitudeCustomFields = "customfields";
    }
}
