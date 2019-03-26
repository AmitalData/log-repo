using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logitude.BL.Helpers
{
    public class FormatHelper
    {
        public static string FormatInteger(int numberOfDigits, object value)
        {            
            string myResult = null;

            if (value != null)
            {
                int data = 0;

                string dataString = value.ToString();

                if (!string.IsNullOrEmpty(dataString))
                {
                    if (dataString.Length > numberOfDigits)
                    {
                        dataString = dataString.Substring(0, numberOfDigits);
                    }

                    bool ok = Int32.TryParse(dataString, out data);
                }

                string format = "{0:d" + numberOfDigits + "}";

                myResult = string.Format(format, data);
            }

            return myResult;
        }
        public static string FormatString(string input, PatternType pattern)
        {
            string result = null;

            if (!string.IsNullOrEmpty(input))
            {
                int len = 0;                
                string textFormat = @"[^A-Z0-9\-\. ]*";
                string alphaNumericFormat = @"[^A-Z0-9]*";

                switch (pattern)
                {
                    case PatternType.Text:
                        {
                            input = input.Trim().ToUpper();
                            result = Regex.Replace(input, textFormat, string.Empty, RegexOptions.Compiled);
                            break;
                        }

                    case PatternType.Name:
                    case PatternType.Address:
                        {
                            len = 35;
                            input = input.Trim().ToUpper();
                            result = Regex.Replace(input, textFormat, string.Empty, RegexOptions.Compiled);
                            result = (result.Length <= len) ? result : result.Substring(0, len);
                            break;
                        }

                    case PatternType.State:
                    case PatternType.ZipCode:
                        {
                            len = 9;
                            input = input.Trim().ToUpper();
                            result = Regex.Replace(input, textFormat, string.Empty, RegexOptions.Compiled);
                            result = (result.Length <= len) ? result : result.Substring(0, len);
                            break;
                        }

                    case PatternType.City:
                    case PatternType.Place:
                        {
                            len = 17;
                            input = input.Trim().ToUpper();
                            result = Regex.Replace(input, textFormat, string.Empty, RegexOptions.Compiled);
                            result = (result.Length <= len) ? result : result.Substring(0, len);
                            break;
                        }

                    case PatternType.NatureAndQuantityOfGoods:
                    case PatternType.Signature:
                        {
                            len = 20;
                            input = input.Trim().ToUpper();
                            result = Regex.Replace(input, textFormat, string.Empty, RegexOptions.Compiled);
                            result = (result.Length <= len) ? result : result.Substring(0, len);
                            break;
                        }

                    case PatternType.ManifestDescriptionOfGoods:
                        {
                            len = 15;
                            input = input.Trim().ToUpper();
                            result = Regex.Replace(input, alphaNumericFormat, string.Empty, RegexOptions.Compiled);
                            result = (result.Length <= len) ? result : result.Substring(0, len);
                            break;
                        }

                    case PatternType.Phone:
                        {
                            len = 25;
                            input = input.Trim().ToUpper();
                            result = Regex.Replace(input, alphaNumericFormat, string.Empty, RegexOptions.Compiled);
                            result = (result.Length <= len) ? result : result.Substring(0, len);
                            break;
                        }

                    case PatternType.HWBSerialNumber:
                        {
                            len = 12;
                            input = input.Trim().ToUpper();
                            result = Regex.Replace(input, alphaNumericFormat, string.Empty, RegexOptions.Compiled);
                            result = (result.Length <= len) ? result : result.Substring(0, len);
                            break;
                        }

                    case PatternType.FlightNumber:
                        {
                            input = input.Trim().ToUpper();

                            Regex isMatch1 = new Regex("^[0-9]{3,4}$");
                            Regex isMatch2 = new Regex("^[0-9]{4}[A-Z]{1}$");
                            if (isMatch1.IsMatch(input) || isMatch2.IsMatch(input))
                            {
                                result = input;
                            }

                            break;
                        }

                    case PatternType.AllotmentIdentification:
                        {
                            len = 14;
                            input = input.Trim().ToUpper();
                            result = Regex.Replace(input, alphaNumericFormat, string.Empty, RegexOptions.Compiled);
                            result = (result.Length <= len) ? result : result.Substring(0, len);
                            break;
                        }

                    default: { break; }
                }
            }

            return result;
        }

        public static string FormatString(string input, PatternType pattern, int length)
        {
            string myResult = null;

            if (!string.IsNullOrEmpty(input))
            {
                string myFormat = null;

                switch (pattern)
                {
                    case PatternType.Alpha:
                        {
                            myFormat = @"[^A-Z]*";
                            break;
                        }

                    case PatternType.AlphaNumeric:
                        {
                            myFormat = @"[^A-Z0-9]*";
                            break;
                        }

                    case PatternType.Text:
                        {
                            myFormat = @"[^A-Z0-9\-\. ]*";
                            break;
                        }

                    default:
                        {
                            myFormat = @"[^A-Z0-9\-\. ]*";
                            break;
                        }
                }

                input = input.Trim().ToUpper();
                myResult = Regex.Replace(input, myFormat, string.Empty, RegexOptions.Compiled);
                myResult = (myResult.Length <= length) ? myResult : myResult.Substring(0, length);
            }

            return myResult;
        }

        public static decimal FormatDecimal(string input, int length)
        {
            decimal result = 0;

            if (!string.IsNullOrEmpty(input))
            {
                input = (input.Length <= length) ? input : input.Substring(0, length);
                Decimal.TryParse(input, out result);
            }

            return result;
        }

        public enum PatternType
        {
            Name = 0,
            Address = 1,
            ZipCode = 2,
            State = 3,
            City = 4,
            Place = 5,
            Signature = 6,
            Phone = 7,
            FlightNumber = 8,
            Text = 9,
            NatureAndQuantityOfGoods = 10,
            ManifestDescriptionOfGoods = 11,
            HWBSerialNumber = 12,
            Alpha = 13,
            AlphaNumeric = 14,
            AllotmentIdentification = 15,
        }

        public static string ConvertFromBase64(string myString)
        {
            string myResult;
            if (myString.Contains("Base64Encode"))
            {
                string value = myString.Substring(12);
                value = value.Trim();

                var base64EncodedBytes = System.Convert.FromBase64String(value);
                //myResult = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                Encoding hebrewEncoding = Encoding.GetEncoding("Windows-1255");
                myResult = hebrewEncoding.GetString(base64EncodedBytes);
            }

            else
            {
                myResult = myString;
            }

            return myResult;
        }
    }
}
