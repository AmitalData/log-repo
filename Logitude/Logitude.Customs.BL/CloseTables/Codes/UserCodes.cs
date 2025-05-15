using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.CloseTables.Codes
{
    public static class UserCodes
    {
        public const string Mehes = "MEHES";

        public static bool IsMehes(this string value) =>
            string.Equals(value, Mehes, StringComparison.Ordinal);
    }
}
