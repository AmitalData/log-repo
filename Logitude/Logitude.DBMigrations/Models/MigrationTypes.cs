using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DBMigrations.Models
{
    public static class MigrationTypes
    {
        public const int ADD = 0;
        public const int RENAME = 1;
        public const int DROP = 2;
        public const int ALTERTYPE = 3;
        public const int ALTERSIZE = 4;
        public const int ADDPRIMARYKEY = 5;
        public const int DROPPRIMARYKEY = 6;
        public const int SETNULLABLE = 7;
        public const int UNSETNULLABLE = 8;
        public const int ALTERPRECISIONANDSCALE = 9;
        public const int ADDFOREIGNKEY = 10;
        public const int DROPFOREIGNKEY = 11;
    }
}
