namespace Logitude.DBMigrations.Models
{
    public static class ToolConfigurations
    {
        public static string DatabaseType;
        public static string GlobalConnectionString;
        public static string MainConnectionString;
        public static string SystemLogsConnectionString;
        public static string CargoTrackingConnectionString;
        public static int AOTScriptsExecutionTimeOut;
        public static bool AOTCreateIndexWithOnline;

        public static string SmtpClientHost;
        public static int SmtpClientPort;
        public static string SmtpClientUsername;
        public static string SmtpClientPassword;
        public static string FromEmailAddress;
        public static string ToEmailAddresses;


        public static string GetConnectionString(string databaseType)
        {
            if (databaseType?.ToLower() == "Global".ToLower())
            {
                return GlobalConnectionString;
            }
            else if (databaseType?.ToLower() == "Main".ToLower())
            {
                return MainConnectionString;
            }
            else if (databaseType?.ToLower() == "SystemLogs".ToLower())
            {
                return SystemLogsConnectionString;
            }
            else if (databaseType?.ToLower() == "CargoTracking".ToLower())
            {
                return CargoTrackingConnectionString;
            }
            else
            {
                return null;
            }
        }
    }
}