using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DBMigrations.Models
{
    public class DatabaseMigrationsResult
    {
        public string MigrationsScript { get; set; }
        public string RelationsScript { get; set; }
        public string IndexesScript { get; set; }
        public string UniqueConstraintsScript { get; set; }
        public string MissingIndexesWarnings { get; set; }
        public string MissingUniqueConstraintsWarnings { get; set; }
    }
}