using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.CommonIIGInterface;

namespace Logitude.CustomsMessaging.Helpers
{
    public static class ForbiddenSignsUtil
    {
        private static char _char = ' ';
        public static string ReplaceForbiddenSigns(string original, string forbiddenChars)
        {
            if (string.IsNullOrEmpty(original) || string.IsNullOrEmpty(forbiddenChars))
                return original;

            return original.Any(forbiddenChars.Contains)
                ? new string(original.Select(c => forbiddenChars.Contains(c) ? _char : c).ToArray())
                : original;

        }

    }
}
