using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.CloseTables.Codes
{
    public static class DeclarationDirection
    {
        public const string Export = "E";
        public const string Import = "I";

        // Convenience helpers
        public static bool IsExport(this string val) => val == Export;
        public static bool IsImport(this string val) => val == Import;
    }
}
