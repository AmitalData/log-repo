using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public static string GetConnectionString(string dbType)
        {
            if (dbType == "Global")
            {
                return GlobalConnectionString;
            }
            else if (dbType == "Main")
            {
                return MainConnectionString;
            }
            else if (dbType == "SystemLogs")
            {
                return SystemLogsConnectionString;
            }
            else if (dbType == "CargoTracking")
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