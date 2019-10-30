using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logitude.XSD.INTTRA
{
    public class INTTRAGeneralMethods
    {
        public long GetDateShortFormat(System.DateTime? date)
        {
            long myResult = 0;

            if (date != null)
            {
                string Year = date.Value.Year.ToString().Substring(2, 2);
                string Month = date.Value.Month.ToString();
                string Day = date.Value.Day.ToString();
                string Hour = date.Value.Hour.ToString();
                string Minute = date.Value.Minute.ToString();

                if (Month.Length == 1)
                {
                    Month = "0" + Month;
                }

                if (Day.Length == 1)
                {
                    Day = "0" + Day;
                }

                if (Hour.Length == 1)
                {
                    Hour = "0" + Hour;
                }

                if (Minute.Length == 1)
                {
                    Minute = "0" + Minute;
                }

                string myString = Year + Month + Day + Hour + Minute;

                myResult = (long)Convert.ToDouble(myString);
            }

            return myResult;
        }
        public long GetDateLongFormat(System.DateTime? date)
        {
            long myResult = 0;

            if (date != null)
            {
                string Year = date.Value.Year.ToString();
                string Month = date.Value.Month.ToString();
                string Day = date.Value.Day.ToString();
                string Hour = date.Value.Hour.ToString();
                string Minute = date.Value.Minute.ToString();

                if (Month.Length == 1)
                {
                    Month = "0" + Month;
                }

                if (Day.Length == 1)
                {
                    Day = "0" + Day;
                }

                if (Hour.Length == 1)
                {
                    Hour = "0" + Hour;
                }

                if (Minute.Length == 1)
                {
                    Minute = "0" + Minute;
                }

                string myString = Year + Month + Day + Hour + Minute;

                myResult = (long)Convert.ToDouble(myString);
            }

            return myResult;
        }

        public List<string> GetStringList(string inputString, int maxOccurs, int length)
        {
            List<string> myResult = new List<string>();

            if (!string.IsNullOrEmpty(inputString))
            {
                inputString = this.FixSpecialCharacters(inputString);

                if (inputString != null)
                {
                    List<string> myResult_PRE = new List<string>();

                    while (inputString.Length > length)
                    {
                        if (myResult_PRE.Count < maxOccurs)
                        {
                            myResult_PRE.Add(inputString.Substring(0, length));
                        }

                        inputString = inputString.Remove(0, length);
                    }

                    if (inputString.Length > 0)
                    {
                        if (myResult_PRE.Count < maxOccurs)
                        {
                            myResult_PRE.Add(inputString);
                        }
                    }

                    foreach (string item in myResult_PRE)
                    {
                        myResult.Add(item);
                    }
                }
            }

            return myResult;
        }
        public string FormatString(string input)
        {
            return this.FormatString(input, INTTRAPattern.Text, null);
        }
        public string FormatString(string input, int length)
        {
            return this.FormatString(input, INTTRAPattern.Text, length);
        }
        public string FormatString(string input, INTTRAPattern pattern)
        {
            return this.FormatString(input, pattern, null);
        }
        public string FormatString(string input, INTTRAPattern pattern, int? length = null)
        {
            string myResult = null;

            if (!string.IsNullOrEmpty(input))
            {
                string myFormat = null;

                switch (pattern)
                {
                    //case INTTRAPattern.Alpha:
                    //    {
                    //        myFormat = @"[^A-Z]*";
                    //        break;
                    //    }

                    //case INTTRAPattern.AlphaNumeric:
                    //    {
                    //        myFormat = @"[^A-Z0-9]*";
                    //        break;
                    //    }

                    case INTTRAPattern.Text:
                        {
                            myFormat = @"[^a-zA-Z0-9\-\,\. ]*";
                            break;
                        }

                    default:
                        {
                            myFormat = @"[^a-zA-Z0-9\-\,\. ]*";
                            break;
                        }
                }

                input = input.Trim().ToUpper();
                myResult = Regex.Replace(input, myFormat, string.Empty, RegexOptions.Compiled);

                if (length != null)
                {
                    myResult = (myResult.Length <= length) ? myResult : myResult.Substring(0, length.Value);
                }
            }

            return myResult;
        }
        public string FixSpecialCharacters(string input)
        {
            string myResult = null;

            if (!string.IsNullOrEmpty(input))
            {
                input = input.Replace("&", "&amp;");
                input = input.Replace("<", "&lt;");
                input = input.Replace(">", "&gt;");
                input = input.Replace("'", "&apos;");
                input = input.Replace("\"", "&quot;");
                myResult = input;
            }

            return myResult;
        }
        public string GetAddress_OneLine(Address address)
        {
            string resultAddress = "";

            if (address != null)
            {
                resultAddress = address.Address1 != null ? address.Address1 : "";

                if (!string.IsNullOrEmpty(address.Address2))
                {
                    resultAddress = resultAddress + ", " + address.Address2;
                }

                if (!string.IsNullOrEmpty(address.City))
                {
                    resultAddress = resultAddress + ", " + address.City;
                }

                if (address.State != null)
                {
                    resultAddress = resultAddress + ", " + (address.State.Code != null ? address.State.Code : "");
                }

                if (!string.IsNullOrEmpty(address.ZipCode))
                {
                    resultAddress = resultAddress + ", " + address.ZipCode;
                }

                if (address.Country != null)
                {
                    if (address.IsLocalLanguage)
                    {
                        resultAddress = resultAddress + ", " + address.Country.LocalName;
                    }

                    else
                    {
                        resultAddress = resultAddress + ", " + address.Country.EnglishName;
                    }
                }
            }

            return resultAddress;
        }
        public bool IsDecimalFormat(string field)
        {
            bool isDecimalFormat = true;

            if (!string.IsNullOrEmpty(field))
            {
                isDecimalFormat = false;

                Regex isMatched = new Regex(@"[^0-9\-\.]*");
                if (isMatched.IsMatch(field))
                {
                    string myStringfield = field;
                    field = field.Replace(".", "");
                    field = field.Replace("-", "");

                    if (field.Length == 3)
                    {
                        isDecimalFormat = true;
                    }
                }
            }

            return isDecimalFormat;
        }

        public enum INTTRAPattern
        {
            Text = 0,
        }
    }
}
