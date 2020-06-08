using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DBMigrations.Models
{
    public class IncludedModules
    {
        public bool Include { get; set; }

        public List<string> Modules { get; set; }
    }
}