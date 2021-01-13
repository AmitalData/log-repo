using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DBMigrations.Models
{
    public static class DBConfigrationsList
    {
        public static List<DBMigrationConfigrations> Configs;

        public static string GetConfigrations (string configType)
        {
            return Configs?.Where(c => c.Type == configType).FirstOrDefault()?.Value;
        }
    }
}
