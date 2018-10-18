using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logitude.WarehouseLib.BL.Helpers
{
    public class ContainerNumberWarehouseValidator
    {
        public static string Validate(string containerNumber)
        {
            string warnmsg = String.Empty;
            if (!string.IsNullOrEmpty(containerNumber))
            {
                Regex isContainerNumberMatches = new Regex("^([a-zA-Z]{4})([0-9]{7})$");
                string str = containerNumber.ToUpper();
                if (isContainerNumberMatches.IsMatch(str))
                {
                    double sum = 0;

                    for (int i = 0; i < str.Length - 1; i++)
                    {
                        if (Char.IsLetter(str[i]))
                        {
                            sum = sum + (GetCharCode(str[i]) * Math.Pow(2, i));
                        }

                        else
                        {
                            sum = sum + (GetIntegerDigit(str[i]) * Math.Pow(2, i));
                        }
                    }

                    int integrSum = Convert.ToInt32(sum);
                    int checkDigit = GetIntegerDigit(str[str.Length - 1]);

                    double divisionby11 = integrSum / 11;
                    int erasedecimaldigits = Convert.ToInt32(divisionby11);
                    int multiplyby11 = erasedecimaldigits * 11;
                    int validCheckDigit = (integrSum - multiplyby11);

                    if (validCheckDigit == 10)
                    {
                        validCheckDigit = 0;
                    }

                    if (validCheckDigit != checkDigit)
                    {
                        string wStr = "General.M.ContainerNumberCheckDigitiswrong";
                        wStr = wStr.Replace("%CheckDigit", validCheckDigit.ToString());
                        warnmsg = wStr;
                    }
                }

                else
                {
                    string wStr = "General.M.ContainerNumberFormatisInvalid";
                    warnmsg = wStr;
                }

            }

            return warnmsg;
        }


        private static int GetCharCode(Char c)
        {
            switch (c)
            {
                case 'A': return 10;
                case 'B': return 12;
                case 'C': return 13;
                case 'D': return 14;
                case 'E': return 15;
                case 'F': return 16;
                case 'G': return 17;
                case 'H': return 18;
                case 'I': return 19;
                case 'J': return 20;
                case 'K': return 21;
                case 'L': return 23;
                case 'M': return 24;
                case 'N': return 25;
                case 'O': return 26;
                case 'P': return 27;
                case 'Q': return 28;
                case 'R': return 29;
                case 'S': return 30;
                case 'T': return 31;
                case 'U': return 32;
                case 'V': return 34;
                case 'W': return 35;
                case 'X': return 36;
                case 'Y': return 37;
                case 'Z': return 38;
                default: return 0;
            }
        }

        private static int GetIntegerDigit(Char c)
        {
            switch (c)
            {
                case '1': return 1;
                case '2': return 2;
                case '3': return 3;
                case '4': return 4;
                case '5': return 5;
                case '6': return 6;
                case '7': return 7;
                case '8': return 8;
                case '9': return 9;
                default: return 0;
            }
        }


    }

}
