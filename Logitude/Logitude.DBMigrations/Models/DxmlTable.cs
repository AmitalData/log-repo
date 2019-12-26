using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DBMigrations.Models
{
    public class DxmlTable
    {
        public string DxmlFileName { get; set; }

        public TableDefinition TableDefinition { get; set; }
    }
}
