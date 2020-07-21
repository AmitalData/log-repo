using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DBMigrations.Models
{
    public class DatabaseMigrationSettings
    {
        public TableDefinition DxmlTableDefinition { get; set; }
        public string DxmlTableConnectionString { get; set; }
        public string DxmlFileName { get; set; }
        public List<TableDefinition> DxmlTablesDefinitions { get; set; }
        public bool IsBasicArgumentProvided { get; set; }
        public bool IsZeroDownTimeArgumentProvided { get; set; }
    }
}