using System.Text.RegularExpressions;

namespace Logitude.Test.Base.Constants
{
    public static class RegularExpressions
    {
        public static Regex ForeignEntityRegex = new Regex(@"^Get \{[^\{\}]*\} From \{[^\{\}]*\} \{[^\{\}]*\} Using \{[^\{\}]*\}$", RegexOptions.IgnoreCase);

        public static Regex CurlyBracketsRegex = new Regex(@"\{[^\{\}]*\}", RegexOptions.IgnoreCase);

        public static Regex RandomStringRegex = new Regex(@"^{RandomString\(\d+\)}$", RegexOptions.IgnoreCase);

        public static Regex RandomNumberRegex = new Regex(@"^{RandomNumber\(\d+,\d+\)}$", RegexOptions.IgnoreCase);
    }
}