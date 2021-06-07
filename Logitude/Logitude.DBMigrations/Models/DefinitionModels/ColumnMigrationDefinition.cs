using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DBMigrations.Models
{
    public class ColumnMigrationDefinition
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Type { get; set; }
        public int? Size { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }
        public string DefaultValue { get; set; }
        public string InitialValueScript { get; set; }
        public ConstraintsDefinition Constraints { get; set; }
    }
}