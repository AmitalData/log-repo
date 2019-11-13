using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DBMigrations.Models
{
    public class ColumnMigrations
    {
        public int MigrationType { get; set; }
        public string ColumnName { get; set; }
        public string NewColumnName { get; set; }
        public string NewColumnType { get; set; }
        public int NewColumnSize { get; set; }
        public ConstraintsDefinition NewConstraints { get; set; }
    }
}
