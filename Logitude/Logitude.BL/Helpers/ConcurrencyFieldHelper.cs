using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.Helpers
{
    public class ConcurrencyFieldHelper
    {
        public static string GetConcurrencyFieldValue_String(string oiginalValue, string newValue, string serverValue)
        {
            if (oiginalValue == null && newValue == null)
                return serverValue;

            else if (oiginalValue == null && newValue != null)
                return newValue;

            else if (oiginalValue != null && serverValue == null)
                return newValue;

            else if (oiginalValue != newValue)
                return newValue;

            else
                return serverValue;
        }
        public static bool GetConcurrencyFieldValue_Bool(bool oiginalValue, bool newValue, bool serverValue)
        {
            if (!oiginalValue && !newValue)
                return serverValue;

            else if (!oiginalValue && newValue)
                return newValue;

            else if (oiginalValue && !serverValue)
                return newValue;

            else if (oiginalValue != newValue)
                return newValue;

            else
                return serverValue;
        }
    }
}
