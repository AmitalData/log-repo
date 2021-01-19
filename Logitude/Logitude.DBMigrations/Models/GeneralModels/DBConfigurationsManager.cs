using System.Collections.Generic;
using System.Linq;

namespace Logitude.DBMigrations.Models
{
    public static class DBConfigurationsManager
    {
        private static List<DBConfiguration> DBConfigurations;

        public static string GetDBConfigurationValue(string dbConfigurationType)
        {
            return DBConfigurations?.Where(c => c.Type?.ToLower() == dbConfigurationType?.ToLower()).FirstOrDefault()?.Value;
        }

        public static void AddDBConfiguration(string dbConfigurationType, string dbConfigurationValue)
        {
            if (DBConfigurations == null)
            {
                DBConfigurations = new List<DBConfiguration>();
            }

            if (!IsDBConfigurationExists(dbConfigurationType))
            {
                DBConfigurations.Add(new DBConfiguration
                {
                    Type = dbConfigurationType,
                    Value = dbConfigurationValue
                });
            }
        }

        public static bool IsDBConfigurationExists(string dbConfigurationType)
        {
            if (DBConfigurations == null)
            {
                return false;
            }
            return DBConfigurations.Where(c => c.Type?.ToLower() == dbConfigurationType?.ToLower()).Any();
        }
    }
}