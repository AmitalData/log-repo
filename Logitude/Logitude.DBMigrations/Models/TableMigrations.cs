using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Logitude.DBMigrations.Models
{
    public class TableMigrations
    {
        public string DxmlTableName { get; set; }
        public string DxmlTableShortName { get; set; }
        public string CurrentTableName { get; set; }
        public List<ColumnMigration> ColumnsMigrations { get; set; }
    }
}
