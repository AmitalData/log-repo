using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DBMigrations.Models
{
    public static class Arguments
    {
        public const string ROOT = "-root";
        public const string EXE = "-exe";
        public const string DEPLOYMENT = "-deployment";
        public const string BASIC = "-basic";
        public const string IGNOREHASH = "-ignorehash";
        public const string IGNORESETTINGSCHECK = "-ignoresettingscheck";
        public const string ZERODOWNTIME = "-zerodowntime";
        public const string SERVICE = "-service";
        public const string DATATYPECHANGES = "-datatypechanges";
        public const string INCLUDEMODULES = "-includemodules";
        public const string EXCLUDEMODULES = "-excludemodules";
        public const string ALLOWDROP = "-allowdrop";
        public const string DEV = "-dev";
        public const string SCRIPTS = "-scripts";
    }
}