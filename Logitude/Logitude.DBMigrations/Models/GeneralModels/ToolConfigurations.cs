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
        public static string DatabaseType = ConfigurationManager.AppSettings["DatabaseType"];
        public static string GlobalConnectionString = ConfigurationManager.AppSettings["GlobalConnectionString"];
        public static string MainConnectionString = ConfigurationManager.AppSettings["MainConnectionString"];
        public static string SystemLogsConnectionString = ConfigurationManager.AppSettings["SystemLogsConnectionString"];
        public static string CargoTrackingConnectionString = ConfigurationManager.AppSettings["CargoTrackingConnectionString"];

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