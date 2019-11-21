using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DBMigrations.Models
{
    public class ColumnMigration
    {
        public int MigrationType { get; set; }
        public ColumnMigrationDefinition CurrentColumn { get; set; }
        public ColumnMigrationDefinition NewColumn { get; set; }//from dxml
    }
}
