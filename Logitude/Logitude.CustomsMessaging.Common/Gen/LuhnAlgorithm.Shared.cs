using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows;
using System.Text.RegularExpressions;

namespace Logitude.CustomsMessaging.Common.Gen
{
    public static class LuhnAlgorithm
    {
        public static string ConvertDeclartionToReshimon(string declarationNumber)
        {
            //check Declaration validity
            //14 digits
            if (declarationNumber.Length != 14)
            {
                return null;
            }
            //check digits 3-4 is 98 or 99 according to DclarationType
            if (declarationNumber.Substring(2, 2) != "98" && declarationNumber.Substring(2, 2) != "99")
            {
                return null;
            }

            int checkDigit = 0;
            string tmp = declarationNumber.Substring(1, 1) + declarationNumber.Substring(6, 7);
            checkDigit = CalculateLuhnAlgorithm(tmp);
            tmp = tmp + checkDigit;

            return tmp;
        }

        public static string ConvertReshimonToDeclartion(string reshimonNumber, string declarationConvertionDigits)
        {
            int checkDigit = 0;

            //check reshimon validity
            //9 digits
            if (reshimonNumber.Length != 9)
            {
                return null;
            }
            string tmp = DateTime.Now.ToString("yy").Substring(0, 1) + reshimonNumber.Substring(0, 1) + declarationConvertionDigits + "00" + reshimonNumber.Substring(1, 7);
            checkDigit = CalculateLuhnAlgorithm(tmp.Substring(5, 8));
            tmp = tmp + checkDigit;
            return tmp;
        }

        public static int CalculateLuhnAlgorithm(string number)
        {
            int sum = 0, d;
            for (int i = 0; i < number.Length; i++)
            {
                d = Convert.ToInt32(number.Substring(i, 1));
                if (i % 2 != 0)
                    d = d * 2;
                if (d > 9)
                    d -= 9;
                sum += d;
            }

            if (sum % 10 == 0)
            {
                return 0;
            }
            else
            {
                return 10 - (sum % 10);
            }
        }

        public static bool IsVatNumberValid(string number)
        {
            var hasCharacters = Regex.IsMatch(number, @"[^\d{9}$]");
            if (hasCharacters) 
                return false;

            var checksumDigit = CalculateLuhnAlgorithm(number);
            return checksumDigit == 0;
        }
    }
}
