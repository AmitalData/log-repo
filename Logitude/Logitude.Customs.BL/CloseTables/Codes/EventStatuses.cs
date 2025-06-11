using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.CloseTables.Codes
{
    public static class EventStatuses
    {
        public const string HTC = "HTC";   // Hatara-cancelled status

        public static bool IsHTC(this string value) =>
            string.Equals(value, HTC, StringComparison.Ordinal);
    }
}
