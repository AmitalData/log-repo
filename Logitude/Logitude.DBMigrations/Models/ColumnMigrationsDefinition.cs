using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DBMigrations.Models
{
    public class ColumnMigrationsDefinition
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Size { get; set; }
        public ConstraintsDefinition Constraints { get; set; }
    }
}
