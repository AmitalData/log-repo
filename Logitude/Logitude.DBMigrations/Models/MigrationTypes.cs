using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DBMigrations.Models
{
    public static class MigrationTypes
    {
        public const int ADDCOLUMN = 0;
        public const int ALTERCOLUMN = 1;
        public const int DROPCOLUMN = 2;
    }
}
