using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DBMigrations.Models
{
    public class DBMigrationsSetDefaultValue
    {
        public string Id { get; set; }
        public string DatabaseType { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string DefaultValue { get; set; }
        public int UpdateNumber { get; set; }
        public int DoneRecordsCount { get; set; }
    }
}