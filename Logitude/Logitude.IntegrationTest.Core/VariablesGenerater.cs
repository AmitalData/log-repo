using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Core
{
    public class VariablesGenerater
    {
        public static string GetRandomString(int length)
        {
            return BaseRandomString("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789",length);
        }
        public static string GetNumbersRandomString(int length)
        {
            return BaseRandomString("0123456789", length);
        }
        public static string GetCapitalCharactersRandomString(int length)
        {
            return BaseRandomString("ABCDEFGHIJKLMNOPQRSTUVWXYZ", length);
        }
        public static string GetSmallCharactersRandomString(int length)
        {
            return BaseRandomString("abcdefghijklmnopqrstuvwxyz", length);
        }
        public static string GetCharactersRandomString(int length)
        {
            return BaseRandomString("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz", length);
        }
        public static string GetUniqueIdByDate()
        {
            long ticks = DateTime.Now.Ticks;
            byte[] bytes = BitConverter.GetBytes(ticks);
            string id = Convert.ToBase64String(bytes)
                                    .Replace('+', '_')
                                    .Replace('/', '-')
                                    .TrimEnd('=');
            return id;
        }

        private static string BaseRandomString(string Base, int length)
        {
            Random random = new Random();
            string chars = Base;
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
