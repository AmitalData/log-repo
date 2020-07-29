using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DBMigrations.Models
{
    public class DatabaseMigrationSettings
    {
        public string DxmlFileName { get; set; }
        public TableDefinition DxmlTableDefinition { get; set; }
        public List<TableDefinition> DxmlTablesDefinitions { get; set; }
    }
}