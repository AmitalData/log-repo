using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DBMigrations.Models
{
    public class DBMigrationsDataScript
    {
        public string Id { get; set; }
        public string DatabaseType { get; set; }
        public string SxmlFileName { get; set; }
        public string SxmlScript { get; set; }
        public int ScriptExecutionNumber { get; set; }
        public int ScriptVersion { get; set; }
        public string ScriptHashValue { get; set; }
        public string ScriptHistoryAction { get; set; }
        public string TargetTableName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}