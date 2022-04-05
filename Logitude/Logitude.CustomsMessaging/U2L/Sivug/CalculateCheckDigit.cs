using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.U2L.Sivug
{
    class CalculateCheckDigit
    {
        // this code is copy from frontend EditSupplierInvoiceItem.ts file, in orginal code is validate and update, return update code only
        public static string Calc(string code)
        {
            bool valid = true;
            string CustomsItemTextValue = code,
                CustomsItem = code,
                value = code,
                digit = "",
                checkDigit = "";


            bool hasDash = value.Contains("-");
            value = value.Replace("-", "");            

            if (value.Length > 12)
            {
                valid = false;
            }
            else if (value.Length < 8)
            {
                valid = false;
            }
            else if (value.Length == 8)
            {
                value = value + "00";

                checkDigit = CalculateLuhnAlgorithm(value);
                if (!string.IsNullOrEmpty(checkDigit))
                    value = value + checkDigit;
            }
            else if (value.Length == 9)
            {
                digit = value.Substring(8);

                value = value.Substring(0, 8) + "00" + value.Substring(8);
                checkDigit = CalculateLuhnAlgorithm(value.Substring(0, 10));

                if (digit != checkDigit)
                {
                    valid = false;
                }
                else
                {
                }

            }
            else if (value.Length == 10)
            {
                checkDigit = CalculateLuhnAlgorithm(value);
                if (!string.IsNullOrEmpty(checkDigit))
                    value = value + "" + checkDigit;

            }
            else if (value.Length == 11)
            {
                digit = value.Substring(10);
                checkDigit = CalculateLuhnAlgorithm(value.Substring(0, 10));
                if (digit != checkDigit)
                {
                    valid = false;
                }
                else
                {

                }

            }
            else
            {
            }

            CustomsItemTextValue = value;

            if (hasDash)
                CustomsItemTextValue = "-" + CustomsItemTextValue;

            return CustomsItemTextValue;
        }

        private static string  CalculateLuhnAlgorithm(string value)
        {
            int sum = 0;
            int d;
            for (var i = 0; i < value.Length; i++)
            {
                d = 0;
                d = int.Parse(value.Substring(i, i + 1));
                if (i % 2 != 0)
                    d = d * 2;
                if (d > 9)
                    d -= 9;
                sum += d;
            }

            if (sum % 10 == 0)
            {
                return "0";
            }
            else
            {
                var x = sum % 10;
                return (10 - x).ToString();
            }
        }
    }
}
