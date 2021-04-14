using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class NumbersConverterToWords
    {
        public NumbersConverterToWords()
        {

        }

        public string NumbersToSpanish(double number)
        {
            string s = number.ToString();
            string a, c, b, result = "";
            int j, orlen;
            if (s == "0") { return ("cero"); }
            orlen = s.Length;
            if ((s.Length % 3) > 0)
                s = " " + s;
            if ((s.Length % 3) > 0)
                s = " " + s;
            for (var i = 0; i < s.Length; i = i + 3)
            {
                j = s.Length - i - 1;
                a = s.Substring(j, 1);
                b = s.Substring(j - 1, 1);
                c = s.Substring(j - 2, 1);
                if (a != " ")
                {
                    if ((i == 3) & (c + b + a != "000")) { result = "mil " + result; }
                    else if (((i == 6) & (c + b + a != "000")) & (orlen == 7) & (a == "1")) { result = "millón " + result; }
                    else if ((i == 6) & (c + b + a != "000")) { result = "millones " + result; }
                    else if ((i == 9) & (c + b + a != "000")) { result = "mil millones" + result; }
                    else if (((i == 12) & (c + b + a != "000")) & (orlen == 13) & (a == "1")) { result = "billón " + result; }
                    else if ((i == 12) & (c + b + a != "000")) { result = "billones " + result; }
                    else if ((i == 15) & (c + b + a != "000")) { result = "mil billones" + result; }
                    else if (((i == 18) & (c + b + a != "000")) & (orlen == 19) & (a == "1")) { result = "trillón " + result; }
                    else if ((i == 18) & (c + b + a != "000")) { result = "trillones " + result; }
                    else if ((i == 21) & (c + b + a != "000")) { result = "mil trillones " + result; }
                    else if (((i == 24) & (c + b + a != "000")) & (orlen == 25) & (a == "1")) { result = "quadrillón " + result; }
                    else if ((i == 24) & (c + b + a != "000")) { result = "quadrillones " + result; }
                    else if ((i == 27) & (c + b + a != "000")) { result = "mil quadrillones " + result; }
                    else if (((i == 30) & (c + b + a != "000")) & (orlen == 31) & (a == "1")) { result = "quintillón " + result; }
                    else if ((i == 30) & (c + b + a != "000")) { result = "quintillones " + result; }
                    else if ((i == 33) & (c + b + a != "000")) { result = "mil quintillones " + result; }
                    else if (((i == 36) & (c + b + a != "000")) & (orlen == 37) & (a == "1")) { result = "sextillón " + result; }
                    else if ((i == 36) & (c + b + a != "000")) { result = "sextillones " + result; }
                    else if ((i == 39) & (c + b + a != "000")) { result = "mil sextillones " + result; }
                    else if (((i == 42) & (c + b + a != "000")) & (orlen == 43) & (a == "1")) { result = "septillón " + result; }
                    else if ((i == 42) & (c + b + a != "000")) { result = "septillones " + result; }
                    else if ((i == 45) & (c + b + a != "000")) { result = "milseptillones " + result; }
                    else if (((i == 48) & (c + b + a != "000")) & (orlen == 49) & (a == "1")) { result = "octillón " + result; }
                    else if ((i == 48) & (c + b + a != "000")) { result = "octillones " + result; }
                    else if ((i == 51) & (c + b + a != "000")) { result = "mil octillones " + result; }
                    else if (((i == 54) & (c + b + a != "000")) & (orlen == 55) & (a == "1")) { result = "nonillón " + result; }
                    else if ((i == 57) & (c + b + a != "000")) { result = "nonillones " + result; }
                    else if (i == 60) { result = "mil nonillones " + result; }
                }
                if ((b != 1 + "") || (b == " "))
                {
                    if (a == 1 + "" && b != 2 + "") { result = "un " + result; }
                    else if (a == 2 + "" && b != 2 + "") { result = "dos " + result; }
                    else if (a == 3 + "" && b != 2 + "") { result = "tres " + result; }
                    else if (a == 4 + "" && b != 2 + "") { result = "cuatro " + result; }
                    else if (a == 5 + "" && b != 2 + "") { result = "cinco " + result; }
                    else if (a == 6 + "" && b != 2 + "") { result = "seis " + result; }
                    else if (a == 7 + "" && b != 2 + "") { result = "siete " + result; }
                    else if (a == 8 + "" && b != 2 + "") { result = "ocho " + result; }
                    else if (a == 9 + "" && b != 2 + "") { result = "nueve " + result; }
                }
                if ((b != " ") & (b != "0"))
                {
                    if ((b == 1 + "") | (b == 2 + ""))
                    {
                        if (b + a == 10 + "") { result = "diez " + result; }
                        else if (b + a == 11 + "") { result = "once " + result; }
                        else if (b + a == 12 + "") { result = "doce " + result; }
                        else if (b + a == 13 + "") { result = "trece " + result; }
                        else if (b + a == 14 + "") { result = "catorce " + result; }
                        else if (b + a == 15 + "") { result = "quince " + result; }
                        else if (b + a == 16 + "") { result = "dieciséis " + result; }
                        else if (b + a == 17 + "") { result = "diecisiete " + result; }
                        else if (b + a == 18 + "") { result = "dieciocho " + result; }
                        else if (b + a == 19 + "") { result = "diecinueve " + result; }
                        else if (b + a == 20 + "") { result = "veinte " + result; }
                        else if (b + a == 21 + "") { result = "veintiuno " + result; }
                        else if (b + a == 22 + "") { result = "veintidós " + result; }
                        else if (b + a == 23 + "") { result = "veintitrés " + result; }
                        else if (b + a == 24 + "") { result = "veinticuatro " + result; }
                        else if (b + a == 25 + "") { result = "veinticinco " + result; }
                        else if (b + a == 26 + "") { result = "veintiséis " + result; }
                        else if (b + a == 27 + "") { result = "veintisiete " + result; }
                        else if (b + a == 28 + "") { result = "veintiocho " + result; }
                        else if (b + a == 29 + "") { result = "veintinueve " + result; }
                    }
                    else
                    {
                        var temp = "";

                        if (a != 0 + "") { temp = "y "; }
                        if (b == 3 + "") { result = "treinta " + temp + result; }
                        else if (b == 4 + "") { result = "cuarenta " + temp + result; }
                        else if (b == 5 + "") { result = "cincuenta " + temp + result; }
                        else if (b == 6 + "") { result = "sesenta " + temp + result; }
                        else if (b == 7 + "") { result = "setenta " + temp + result; }
                        else if (b == 8 + "") { result = "ochenta " + temp + result; }
                        else if (b == 9 + "") { result = "noventa " + temp + result; }

                    }
                }
                if ((c != " ") & (c != "0"))
                {
                    if ((a == "0") & (b == "0"))
                    {
                        if (c == 1 + "") { result = "cien " + result; }
                        else if (c == 2 + "") { result = "doscientos " + result; }
                        else if (c == 3 + "") { result = "trescientos " + result; }
                        else if (c == 4 + "") { result = "cuatrocientos " + result; }
                        else if (c == 5 + "") { result = "quinientos " + result; }
                        else if (c == 6 + "") { result = "seiscientos " + result; }
                        else if (c == 7 + "") { result = "setecientos " + result; }
                        else if (c == 8 + "") { result = "ochocientos " + result; }
                        else if (c == 9 + "") { result = "novecientos " + result; }
                    }
                    else
                    {
                        if (c == 1 + "") { result = "ciento " + result; }
                        else if (c == 2 + "") { result = "doscientos " + result; }
                        else if (c == 3 + "") { result = "trescientos " + result; }
                        else if (c == 4 + "") { result = "cuatrocientos " + result; }
                        else if (c == 5 + "") { result = "quinientos " + result; }
                        else if (c == 6 + "") { result = "seiscientos " + result; }
                        else if (c == 7 + "") { result = "setecientos " + result; }
                        else if (c == 8 + "") { result = "ochocientos " + result; }
                        else if (c == 9 + "") { result = "novecientos " + result; }
                    }
                }
            }
            result = FixSpaces(result);
            result = Trim(result);

            if (result.Length >= 7)
            {
                if (result.Substring(0, 7) == "un mil ")
                {
                    result = result.Substring(3, result.Length - 3);
                }
            }

            if (result.Length >= 3)
            {
                if (result.Substring(result.Length - 3, 3) == " un")
                {
                    result = result + "o";
                }
            }

            if (result == "un ")
            {
                result = "uno";
            }

            if (result.Length >= 2)
            {
                if (result.Substring(result.Length - 2, 2) == "y ")
                {
                    result = result.Substring(0, result.Length - 2);
                }
            }

            if (InStr(result, "millones") != RInStr(result, "millones"))
            {
                var z = InStr(result, "millones");

                result = result.Substring(0, z - 1) + result.Substring(z + 7, result.Length - (z + 7));

            }
            if (InStr(result, "billones") != RInStr(result, "billones"))
            {
                var z = InStr(result, "billones");

                result = result.Substring(0, z - 1) + result.Substring(z + 7, result.Length - (z + 7));
            }
            if (InStr(result, "trillones") != RInStr(result, "trillones"))
            {
                var z = InStr(result, "trillones");

                result = result.Substring(0, z - 1) + result.Substring(z + 8, result.Length - (z + 8));
            }
            if (InStr(result, "quadrillones") != RInStr(result, "quadrillones"))
            {
                var z = InStr(result, "quadrillones");

                result = result.Substring(0, z - 1) + result.Substring(z + 11, result.Length - (z + 11));
            }
            if (InStr(result, "quintillones") != RInStr(result, "quintillones"))
            {
                var z = InStr(result, "quintillones");

                result = result.Substring(0, z - 1) + result.Substring(z + 11, result.Length - (z + 11));
            }
            if (InStr(result, "sextillones") != RInStr(result, "sextillones"))
            {
                var z = InStr(result, "sextillones");

                result = result.Substring(0, z - 1) + result.Substring(z + 10, result.Length - (z + 10));
            }
            if (InStr(result, "septillones") != RInStr(result, "septillones"))
            {
                var z = InStr(result, "septillones");

                result = result.Substring(0, z - 1) + result.Substring(z + 10, result.Length - (z + 10));
            }
            if (InStr(result, "octillones") != RInStr(result, "octillones"))
            {
                var z = InStr(result, "octillones");

                result = result.Substring(0, z - 1) + result.Substring(z + 9, result.Length - (z + 9));
            }
            if (InStr(result, "nonillones") != RInStr(result, "nonillones"))
            {
                var z = InStr(result, "nonillones");

                result = result.Substring(0, z - 1) + result.Substring(z + 9, result.Length - (z + 9));
            }
            result = FixSpaces(result);
            return (result);

        }
        public string NumbersToFrench(double number)
        {
            string s = number.ToString();
            string a, c, b, result = "";
            int j, orlen;
            if (s == "0") { return ("z�ro"); }
            orlen = s.Length;
            if ((s.Length % 3) > 0)
                s = " " + s;
            if ((s.Length % 3) > 0)
                s = " " + s;
            for (var i = 0; i < s.Length; i = i + 3)
            {
                j = s.Length - i - 1;
                a = s.Substring(j, 1);
                b = s.Substring(j - 1, 1);
                c = s.Substring(j - 2, 1);
                if (a != " ")
                {
                    if ((i == 3) & (c + b + a != "000")) { result = "mille " + result; }
                    else if (((i == 6) & (c + b + a != "000")) & (orlen == 7) & (a == "1")) { result = "millón " + result; }
                    else if ((i == 6) & (c + b + a != "000")) { result = "millions " + result; }
                    else if ((i == 9) & (c + b + a != "000")) { result = "mille millions" + result; }
                    else if (((i == 12) & (c + b + a != "000")) & (orlen == 13) & (a == "1")) { result = "billion " + result; }
                    else if ((i == 12) & (c + b + a != "000")) { result = "billions " + result; }
                    else if ((i == 15) & (c + b + a != "000")) { result = "mille billions" + result; }
                    else if (((i == 18) & (c + b + a != "000")) & (orlen == 19) & (a == "1")) { result = "trillion " + result; }
                    else if ((i == 18) & (c + b + a != "000")) { result = "trillions " + result; }
                    else if ((i == 21) & (c + b + a != "000")) { result = "mille trillions " + result; }
                    else if (((i == 24) & (c + b + a != "000")) & (orlen == 25) & (a == "1")) { result = "quadrillion " + result; }
                    else if ((i == 24) & (c + b + a != "000")) { result = "quadrillions " + result; }
                    else if ((i == 27) & (c + b + a != "000")) { result = "mille quadrillions " + result; }
                    else if (((i == 30) & (c + b + a != "000")) & (orlen == 31) & (a == "1")) { result = "quintillion " + result; }
                    else if ((i == 30) & (c + b + a != "000")) { result = "quintillions " + result; }
                    else if ((i == 33) & (c + b + a != "000")) { result = "mille quintillions " + result; }
                    else if (((i == 36) & (c + b + a != "000")) & (orlen == 37) & (a == "1")) { result = "sextillion " + result; }
                    else if ((i == 36) & (c + b + a != "000")) { result = "sextillions " + result; }
                    else if ((i == 39) & (c + b + a != "000")) { result = "mille sextillions " + result; }
                    else if (((i == 42) & (c + b + a != "000")) & (orlen == 43) & (a == "1")) { result = "septillion " + result; }
                    else if ((i == 42) & (c + b + a != "000")) { result = "septillions " + result; }
                    else if ((i == 45) & (c + b + a != "000")) { result = "milseptillions " + result; }
                    else if (((i == 48) & (c + b + a != "000")) & (orlen == 49) & (a == "1")) { result = "octillion " + result; }
                    else if ((i == 48) & (c + b + a != "000")) { result = "octillions " + result; }
                    else if ((i == 51) & (c + b + a != "000")) { result = "mille octillions " + result; }
                    else if (((i == 54) & (c + b + a != "000")) & (orlen == 55) & (a == "1")) { result = "nonillion " + result; }
                    else if ((i == 57) & (c + b + a != "000")) { result = "nonillions " + result; }
                    else if (i == 60) { result = "mille nonillions " + result; }
                }
                if (((b != 1 + "") & (b != 7 + "")) | (b == " "))
                {
                    if (a == 1 + "") { result = "un " + result; }
                    else if (a == 2 + "") { result = "deux " + result; }
                    else if (a == 3 + "") { result = "trois " + result; }
                    else if (a == 4 + "") { result = "quatre " + result; }
                    else if (a == 5 + "") { result = "cinq " + result; }
                    else if (a == 6 + "") { result = "six " + result; }
                    else if (a == 7 + "") { result = "sept " + result; }
                    else if (a == 8 + "") { result = "huit " + result; }
                    else if (a == 9 + "") { result = "neuf " + result; }
                }
                if ((b != " ") & (b != "0"))
                {
                    if ((b == 1 + "") | (b == 7 + ""))
                    {
                        if (b + a == 10 + "") { result = "dix " + result; }
                        else if (b + a == 11 + "") { result = "onze " + result; }
                        else if (b + a == 12 + "") { result = "douze " + result; }
                        else if (b + a == 13 + "") { result = "treize " + result; }
                        else if (b + a == 14 + "") { result = "quatorze " + result; }
                        else if (b + a == 15 + "") { result = "quinze " + result; }
                        else if (b + a == 16 + "") { result = "seize " + result; }
                        else if (b + a == 17 + "") { result = "dix-sept " + result; }
                        else if (b + a == 18 + "") { result = "dix-huit " + result; }
                        else if (b + a == 19 + "") { result = "dix-neuf " + result; }
                        else if (b + a == 70 + "") { result = "soixante-dix " + result; }
                        else if (b + a == 71 + "") { result = "soixante-onze " + result; }
                        else if (b + a == 72 + "") { result = "soixante-douze " + result; }
                        else if (b + a == 73 + "") { result = "soixante-treize " + result; }
                        else if (b + a == 74 + "") { result = "soixante-quatorze " + result; }
                        else if (b + a == 75 + "") { result = "soixante-quinze " + result; }
                        else if (b + a == 76 + "") { result = "soixante-seize " + result; }
                        else if (b + a == 77 + "") { result = "soixante-dix-sept " + result; }
                        else if (b + a == 78 + "") { result = "soixante-dix-huit " + result; }
                        else if (b + a == 79 + "") { result = "soixante-dix-neuf " + result; }
                    }
                    else
                    {
                        var temp = "";

                        if (a == 1 + "") { temp = "et "; }
                        if (int.Parse(a) > 1) { temp = "-"; }
                        if (b == 2 + "") { result = "vingt " + temp + result; }
                        else if (b == 3 + "") { result = "trente " + temp + result; }
                        else if (b == 4 + "") { result = "quarante " + temp + result; }
                        else if (b == 5 + "") { result = "cinquante " + temp + result; }
                        else if (b == 6 + "") { result = "soixante " + temp + result; }
                        else if (b == 7 + "") { result = "soixante-dix " + temp + result; }
                        else if (b == 8 + "") { result = "quatre-vingts " + temp + result; }
                        else if (b == 9 + "") { result = "quatre-vingt-dix " + temp + result; }
                    }
                }
                if ((c != " ") & (c != "0"))
                {
                    if (c == 1 + "") { result = "cent " + result; }
                    else if (c == 2 + "") { result = "deux cents " + result; }
                    else if (c == 3 + "") { result = "trois cents " + result; }
                    else if (c == 4 + "") { result = "quatre cents " + result; }
                    else if (c == 5 + "") { result = "cinq cents " + result; }
                    else if (c == 6 + "") { result = "six cents " + result; }
                    else if (c == 7 + "") { result = "sept cents " + result; }
                    else if (c == 8 + "") { result = "huit cents " + result; }
                    else if (c == 9 + "") { result = "neuf cents " + result; }
                }
            }
            result = FixSpaces(result);
            result = Trim(result);

            if (result.Length >= 9)
            {
                if (result.Substring(0, 9) == "un mille ")
                {
                    result = result.Substring(3, result.Length - 3);
                }
            }

            if (result.Length >= 3)
            {
                if (result.Substring(result.Length - 3, 3) == "et ")
                {
                    result = result.Substring(0, result.Length - 3);
                }
            }

            if (result.Length >= 2)
            {
                if (result.Substring(result.Length - 2, 2) == " -")
                {
                    result = result.Substring(0, result.Length - 2);
                }
            }

            if (InStr(result, "millions") != RInStr(result, "millions"))
            {
                var z = InStr(result, "millions");

                result = result.Substring(0, z - 1) + result.Substring(z + 7, result.Length - (z + 7));
            }
            if (InStr(result, "billions") != RInStr(result, "billions"))
            {
                var z = InStr(result, "billions");

                result = result.Substring(0, z - 1) + result.Substring(z + 7, result.Length - (z + 7));
            }
            if (InStr(result, "trillions") != RInStr(result, "trillions"))
            {
                var z = InStr(result, "trillions");

                result = result.Substring(0, z - 1) + result.Substring(z + 8, result.Length - (z + 8));
            }
            if (InStr(result, "quadrillions") != RInStr(result, "quadrillions"))
            {
                var z = InStr(result, "quadrillions");

                result = result.Substring(0, z - 1) + result.Substring(z + 11, result.Length - (z + 11));
            }
            if (InStr(result, "quintillions") != RInStr(result, "quintillions"))
            {
                var z = InStr(result, "quintillions");

                result = result.Substring(0, z - 1) + result.Substring(z + 11, result.Length - (z + 11));
            }
            if (InStr(result, "sextillions") != RInStr(result, "sextillions"))
            {
                var z = InStr(result, "sextillions");

                result = result.Substring(0, z - 1) + result.Substring(z + 10, result.Length - (z + 10));
            }
            if (InStr(result, "septillions") != RInStr(result, "septillions"))
            {
                var z = InStr(result, "septillions");

                result = result.Substring(0, z - 1) + result.Substring(z + 10, result.Length - (z + 10));
            }
            if (InStr(result, "octillions") != RInStr(result, "octillions"))
            {
                var z = InStr(result, "octillions");

                result = result.Substring(0, z - 1) + result.Substring(z + 9, result.Length - (z + 9));
            }
            if (InStr(result, "nonillions") != RInStr(result, "nonillions"))
            {
                var z = InStr(result, "nonillions");

                result = result.Substring(0, z - 1) + result.Substring(z + 9, result.Length - (z + 9));
            }
            result = FixSpaces(result);
            while (InStr(result, " -") > 0)
            {
                var z = InStr(result, " -");
                result = result.Substring(0, z - 1) + result.Substring(z, result.Length - z);
            }
            return (result);

        }
        public string NumbersToRussian(double numberNumeric)
        {
            string ZERO_NAME = "нуль";
            string ONE_THOUSANT_NAME = "одна";
            string[] HUNDRED_NAMES = { "", "сто", "двести", "триста", "четыреста", "пятьсот", "шестьсот", "семьсот", "восемьсот", "девятьсот" };
            string[] TEN_NAMES = { "", "", "двадцать", "тридцать", "сорок", "пятьдесят", "шестьдесят", "семьдесят", "восемьдесят", "девяносто" };
            string[] UNIT_NAMES = { ZERO_NAME, "один", "два", "три", "четыре", "пять", "шесть", "семь", "восемь", "девять" };
            List<string[]> TRIPLET_NAMES = new List<string[]>();
            string[] Undifined = { null };
            string[] Triplet1 = { "тысяча", "тысячи", "тысяч" };
            string[] Triplet2 = { "миллион", "миллиона", "миллионов" };
            string[] Triplet3 = { "миллиард", "миллиарда", "миллиардов" };
            string[] Triplet4 = { "триллион", "триллиона", "триллионов" };
            string[] Triplet5 = { "квадрилион", "квадрилиона", "квадрилионов" };
            TRIPLET_NAMES.Add(Undifined);
            TRIPLET_NAMES.Add(Triplet1);
            TRIPLET_NAMES.Add(Triplet1);
            TRIPLET_NAMES.Add(Triplet2);
            TRIPLET_NAMES.Add(Triplet3);
            TRIPLET_NAMES.Add(Triplet4);
            TRIPLET_NAMES.Add(Triplet5);

            string[] TEN_UNIT_NAMES = { "десять", "одиннадцать", "двенадцать", "тринадцать", "четырнадцать", "пятнадцать", "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать" };


            List<string> numberInWords = new List<string>();
            if (numberNumeric <= -1)
            {
                numberNumeric *= -1;
                numberInWords.Add("минус");
            }

            string number = numberNumeric + "";


            int length = number.Length;

            for (int i = 0; i < length; i += 1)
            {
                int pos = length - 1 - i;
                decimal d = pos / 3;
                decimal tripletIndex = Math.Floor(d);
                int digitPosition = pos % 3;
                int digitValue = int.Parse(number[i] + "");

                if (digitPosition == 2)
                {
                    numberInWords.Add(HUNDRED_NAMES[digitValue]);
                    continue;
                }
                if (digitPosition == 1)
                {
                    if (digitValue == 1)
                    {
                        numberInWords.Add(TEN_UNIT_NAMES[int.Parse(number[i + 1] + "")]);
                    }
                    else
                    {
                        numberInWords.Add(TEN_NAMES[digitValue]);
                    }
                    continue;
                }
                if (digitPosition == 0)
                {
                    int prevDigitValue = -99999999;
                    if (i - 1 >= 0)
                    {
                        prevDigitValue = int.Parse(number[i - 1] + "");

                    }

                    if (digitValue == 0)
                    {
                        if (length == 1)
                        {
                            numberInWords.Add(ZERO_NAME);
                        }
                    }
                    else if (prevDigitValue != 1 || prevDigitValue == -99999999)
                    {
                        numberInWords.Add(tripletIndex == 1 && digitValue == 1 ? ONE_THOUSANT_NAME : UNIT_NAMES[digitValue]);
                    }

                    string[] tripletNames = TRIPLET_NAMES[(int)tripletIndex];
                    if (tripletNames.Length > 1)
                    {

                        if (prevDigitValue == 1)
                        {
                            numberInWords.Add(pluralEnding(10 + digitValue, tripletNames));
                        }
                        else
                        {
                            numberInWords.Add(pluralEnding(digitValue, tripletNames));
                        }
                    }
                    continue;
                }
            }

            return String.Join(" ", numberInWords);




        }
        public string pluralEnding(int number, string[] variants)
        {
            var one = variants[0];
            string two = variants[1];
            string five = variants[2];
            number = Math.Abs(number);
            number %= 100;
            if (number >= 5 && number <= 20)
            {
                return five;
            }
            number %= 10;
            if (number == 1)
            {
                return one;
            }
            if (number >= 2 && number <= 4)
            {
                return two;
            }
            return five;

        }
        public string NumbersToEnglish(double number)
        {
            string s = number.ToString();
            string a, c, b, result = "";
            int j, orlen;
            if (s == "0") { return ("zero"); }
            orlen = s.Length;
            if ((s.Length % 3) > 0)
                s = ' ' + s;
            if ((s.Length % 3) > 0)
                s = ' ' + s;
            for (var i = 0; i < s.Length; i = i + 3)
            {
                j = s.Length - i - 1;
                a = s.Substring(j, 1);
                b = s.Substring(j - 1, 1);
                c = s.Substring(j - 2, 1);
                if (a != " ")
                {
                    if ((i == 3) & (c + b + a != "000")) { result = "thousand " + result; }
                    else if ((i == 6) & (c + b + a != "000")) { result = "million " + result; }
                    else if ((i == 9) & (c + b + a != "000")) { result = "billion " + result; }
                    else if ((i == 12) & (c + b + a != "000")) { result = "trillion " + result; }
                    else if ((i == 15) & (c + b + a != "000")) { result = "quadrillion " + result; }
                    else if ((i == 18) & (c + b + a != "000")) { result = "quintillion " + result; }
                    else if ((i == 21) & (c + b + a != "000")) { result = "sextillion " + result; }
                    else if ((i == 24) & (c + b + a != "000")) { result = "septillion " + result; }
                    else if ((i == 27) & (c + b + a != "000")) { result = "octillion " + result; }
                    else if ((i == 30) & (c + b + a != "000")) { result = "nonillion " + result; }
                    else if ((i == 33) & (c + b + a != "000")) { result = "decillion " + result; }
                    else if ((i == 36) & (c + b + a != "000")) { result = "undecillion " + result; }
                    else if ((i == 39) & (c + b + a != "000")) { result = "duodecillion " + result; }
                    else if ((i == 42) & (c + b + a != "000")) { result = "tredecillion " + result; }
                    else if ((i == 45) & (c + b + a != "000")) { result = "quattuordecillion " + result; }
                    else if ((i == 48) & (c + b + a != "000")) { result = "quindecillion " + result; }
                    else if ((i == 51) & (c + b + a != "000")) { result = "sexdecillion " + result; }
                    else if ((i == 54) & (c + b + a != "000")) { result = "septendecillion " + result; }
                    else if ((i == 57) & (c + b + a != "000")) { result = "octodecillion " + result; }
                    else if (i == 60) { result = "novemdecillion " + result; }
                }
                if ((b != "1") | (b == " "))
                {
                    if (a == 1 + "") { result = "one " + result; }
                    else if (a == 2 + "") { result = "two " + result; }
                    else if (a == 3 + "") { result = "three " + result; }
                    else if (a == 4 + "") { result = "four " + result; }
                    else if (a == 5 + "") { result = "five " + result; }
                    else if (a == 6 + "") { result = "six " + result; }
                    else if (a == 7 + "") { result = "seven " + result; }
                    else if (a == 8 + "") { result = "eight " + result; }
                    else if (a == 9 + "") { result = "nine " + result; }
                }
                if ((b != " ") & (b != "0"))
                {
                    if (b == "1")
                    {
                        if (a == 0 + "") { result = "ten " + result; }
                        else if (a == 1 + "") { result = "eleven " + result; }
                        else if (a == 2 + "") { result = "twelve " + result; }
                        else if (a == 3 + "") { result = "thirteen " + result; }
                        else if (a == 4 + "") { result = "fourteen " + result; }
                        else if (a == 5 + "") { result = "fifteen " + result; }
                        else if (a == 6 + "") { result = "sixteen " + result; }
                        else if (a == 7 + "") { result = "seventeen " + result; }
                        else if (a == 8 + "") { result = "eighteen " + result; }
                        else if (a == 9 + "") { result = "nineteen " + result; }
                    }
                    else
                    {
                        if (b == 2 + "") { result = "twenty " + result; }
                        else if (b == 3 + "") { result = "thirty " + result; }
                        else if (b == 4 + "") { result = "fourty " + result; }
                        else if (b == 5 + "") { result = "fifty " + result; }
                        else if (b == 6 + "") { result = "sixty " + result; }
                        else if (b == 7 + "") { result = "seventy " + result; }
                        else if (b == 8 + "") { result = "eighty " + result; }
                        else if (b == 9 + "") { result = "ninety " + result; }
                    }
                }
                if ((c != " ") & (c != "0"))
                {
                    if (c == 1 + "") { result = "one hundred " + result; }
                    else if (c == 2 + "") { result = "two hundred " + result; }
                    else if (c == 3 + "") { result = "three hundred " + result; }
                    else if (c == 4 + "") { result = "four hundred " + result; }
                    else if (c == 5 + "") { result = "five hundred " + result; }
                    else if (c == 6 + "") { result = "six hundred " + result; }
                    else if (c == 7 + "") { result = "seven hundred " + result; }
                    else if (c == 8 + "") { result = "eight hundred " + result; }
                    else if (c == 9 + "") { result = "nine hundred " + result; }
                }
            }
            result = Trim(result);
            result = FixSpaces(result);
            return (result);


        }
        public string NumbersToHebrew(double number)
        {
            string rv = NumbersToHebrew(number, "", "", false);
            return rv;
        }
        public static IEnumerable<String> SliceRow<String>(String[,] array, int row)
        {
            for (var i = 0; i < array.GetLength(1); i++)
            {
                yield return array[row, i];
            }
        }
        public string NumbersToHebrew(double amount, string curr_name, String subunit_name, Boolean cents_in_words)
        {
            String shkalim = "";
            String agorot = "";
            String shah = "";
            String pattern = "";
            String left = "";
            String right = "";
            String ag = "";

            int pos = 0;
            int digit = 0;
            int len = 0;
            int n = 0;
            int prec = 0;


            String add = "";

            String[,] tr;
            String number = "";
            String text = "";
            String ve = "";
            String asar = "";
            String meot = "";
            String elef = "";
            String mili = "";
            String cents = "";


            Boolean eng = false;
            String str1;


            prec = 2;// sample: 2.1 => 2.10


            shah = curr_name;

            //  eng = !((shah[0] >= 'א' && shah[0] <= 'ת') | shah[0] == '₪');

            ag = subunit_name;
            number = amount.ToString();

            if (!number.ToLower().Contains('.'))
            {
                shkalim = number;
                agorot = "";
            }
            else
            {
                string[] words = number.Split('.');
                shkalim = words[0];
                agorot = words[1];
                if (agorot.Length == 1) // e.g. 60 --> 6
                {
                    agorot = agorot + "0";
                }
                agorot = agorot.Substring(0, prec);
                agorot = agorot.PadRight(prec, '0');
            }

            tr = new String[,]
            {
                {"אחד", "אלף", "מאה", "עשר", "עשרת אלפים"},//1
                {"שנים", "אלפיים", "מאתיים", "עשרים", "שני"},
                {"שלושה", "שלושת אלפים", "שלוש", "שלושים", ""},//3
                {"ארבעה", "ארבעת אלפים", "ארבע", "ארבעים", ""},
                {"חמישה", "חמשת אלפים", "חמש", "חמישים", ""},//5
                {"שישה", "ששת אלפים", "שש", "שישים", ""},
                {"שבעה", "שבעת אלפים", "שבע", "שבעים", ""},//7
                {"שמונה", "שמונת אלפים", "שמונה", "שמונים", ""},
                {"תשעה", "תשעת אלפים", "תשע", "תשעים", ""},//9
            };

            ve = " ו";
            asar = " עשר";
            meot = " מאות";
            elef = " אלף";
            mili = " מיליון";

            len = shkalim.Length;
            pos = len;

            while (pos > 0)
            {

                n = len - pos + 1;//counting from leftmost digit to the right; starting with 1 

                digit = shkalim[n - 1] - '0';//This works because each character is internally represented by a number.
                String[] str;
                if (digit > 0)
                {
                    str = SliceRow(tr, digit - 1).ToArray();//one_dimensional slice of the corresponding digit 
                }
                else
                {
                    str = new String[] { "", "", "", "", "" };
                }
                //     num = n - 2;

                //     if (num < 1) { num = 1; }

                //     num = $Number(shkalim.Substring(num - 1, n);

                add = "";

                switch (pos)
                {
                    case 1:
                        if (n > 1 && shkalim[n - 2] == '1')// *10 - *19
                        {
                            if (digit == 0) // *10
                            {
                                str1 = tr[0, 3]; // eser
                                if (text != "" && !String.IsNullOrEmpty(str1))
                                {
                                    text = text + ve;
                                }
                                text = text + str1;
                            }
                            else
                            { // *11 - *19
                                str1 = str[0]; // ahad, shneim, slosha, ...
                                if (text != "")
                                {
                                    //text = text + " ";
                                    if (len > 1 && digit > 0)
                                    {
                                        str1 = ve + str1;
                                    }
                                    else
                                    {
                                        text = text + " ";
                                    }

                                }

                                text = text + str1 + asar;
                            }
                        }
                        else // *##
                        {
                            if (digit == 2 && len == 1)
                            {
                                str1 = str[4]; // shnei 
                            }
                            else
                            {
                                str1 = str[0]; // ehad, shnaiim, slosha, ... 
                            }
                            if (len > 1 && digit > 0)
                            {
                                str1 = ve + str1;
                            }
                            text = text + str1;
                        }
                        break;

                    case 2:
                        str1 = str[3]; // eser, esrim, shloshim, ...
                        if (digit > 1)
                        {
                            if (text != "" && !String.IsNullOrEmpty(str1))
                            {
                                text = text + " ";
                            }
                            text = text + str1;
                        }
                        break;

                    case 3:
                        str1 = str[2]; // mea, mataiim, shlosh, arba, ...
                        if (digit > 0)
                        {
                            if (text != "" && !String.IsNullOrEmpty(str1))
                            {
                                str1 = " " + str1;
                            }
                            if (digit > 2) { add = meot; }
                        }
                        text = text + str1 + add;
                        break;

                    case 4:
                        if (n > 1 && shkalim[n - 2] == '1')// *10 - *19
                        {
                            if (digit == 0) // *10
                            {
                                if (len == 5)
                                {
                                    text = tr[0, 4];
                                } // aseret alafim 
                                else
                                {
                                    str1 = tr[0, 3]; //eser
                                    if (text != "" && !String.IsNullOrEmpty(str1))
                                    {
                                        text = text + ve;
                                    }
                                    text = text + str1 + elef;
                                }
                            }
                            else // *11 - *19
                            {
                                str1 = str[0]; // ahad, shneim, slosha, ...  
                                if (text != "")
                                {
                                    //text = text + " ";
                                    if (len > 1 && digit > 0)
                                    {
                                        str1 = ve + str1;
                                    }
                                    else
                                    {
                                        text = text + " ";
                                    }

                                }

                                text = text + str1 + asar + elef;

                            }
                        }
                        else // *## 
                        {
                            if (pos == 4 && len == 4)
                            {
                                text = str[1]; // elef, alpaiim, shloshet alafim, ...
                            }
                            else if (n > 0 && shkalim[n - 1] - '0' > 0 || n > 1 && shkalim[n - 2] - '0' > 0 || n > 2 && shkalim[n - 3] - '0' > 0)
                            {

                                str1 = str[0]; // ehad, shnaiim, slosha, ...
                                if (text != "" && !String.IsNullOrEmpty(str1))
                                {
                                    text = text + ve;
                                }
                                text = text + str1 + elef;
                            }
                        }
                        break;

                    case 5:
                        str1 = str[3]; // eser, esrim, shloshim, ...
                        if (digit > 1)
                        {
                            if (text != "") { text = text + " "; }
                            text = text + str1;
                        }
                        break;

                    case 6:
                        str1 = str[2]; // mea, mataiim, shlosh, arba, ...
                        if (digit > 0)
                        {
                            if (text != "")
                            {
                                str1 = " " + str1;
                            }
                            if (digit > 2) { add = meot; }
                        }
                        text = text + str1 + add;
                        break;

                    case 7:
                        if (n > 1 && shkalim[n - 2] == '1')// *10 - *19
                        {
                            if (digit == 0) // *10
                            {
                                str1 = tr[0, 3]; //eser
                                if (str1 == "עשר") { str1 = "עשרה"; }
                                if (text != "" && !String.IsNullOrEmpty(str1))
                                {
                                    text = text + ve;
                                }
                                text = text + str1;
                            }
                            else // *11 - *19
                            {
                                str1 = str[0]; // ahad, shneim, slosha, ...  
                                if (text != "")
                                {
                                    //text = text + " ";
                                    if (len > 1 && digit > 0)
                                    {
                                        str1 = ve + str1;
                                    }
                                    else
                                    {
                                        text = text + " ";
                                    }

                                }

                                text = text + str1 + asar;
                            }
                        }
                        else // *## 
                        {
                            if (n > 0 && shkalim[n - 1] - '0' > 0 || n > 1 && shkalim[n - 2] - '0' > 0 || n > 2 && shkalim[n - 3] - '0' > 0)
                            {

                                if (digit == 2 && len == 7)
                                {
                                    str1 = str[4]; // shnei 
                                }
                                else
                                {
                                    str1 = str[0]; // ehad, shnaiim, slosha, ... 
                                }
                                if (text != "" && !String.IsNullOrEmpty(str1))
                                {
                                    text = text + ve;
                                }
                                text = text + str1;
                            }
                        }

                        text = text + mili;
                        break;

                    case 8:
                        str1 = str[3]; // eser, esrim, shloshim, ...
                        if (digit > 1)
                        {
                            if (text != "") { text = text + " "; }
                            text = text + str1;
                        }
                        break;

                    case 9:
                        str1 = str[2]; // mea, mataiim, shlosh, arba, ...
                        if (digit > 0)
                        {
                            if (text != "")
                            {
                                str1 = " " + str1;
                            }
                            if (digit > 2) { add = meot; }
                        }
                        text = text + str1 + add;
                        break;

                    default:
                        break;
                } // switch 

                pos -= 1;
            } // while 

            text = text + " " + shah;

            decimal dcents = 0M;
            if (!Decimal.TryParse(agorot, out dcents))
            {
                dcents = 0M;
            }
            dcents = decimal.Round(dcents, 2);
            cents = dcents.ToString();

            if (String.IsNullOrWhiteSpace(subunit_name))
            {
                if (dcents == 0)
                {
                   // text = text + " בלבד";
                    text = Trim(text);
                    text = FixSpaces(text);
                    return (text);
                }
                else if (cents.Length == 1)
                {
                    cents = "0" + cents;
                }
                text = text + " + " + cents + "/100";
            }
            else if (!cents_in_words)
            {
                if (dcents == 0)
                {
                  //  text = text + " בלבד";
                    text = Trim(text);
                    text = FixSpaces(text);
                    return (text);
                }
                else if (cents.Length == 1)
                {
                    cents = "0" + cents;
                }
                text = text + " + " + cents + " " + subunit_name;
            }

            else if (cents_in_words)
            {
                if (dcents == 0)
                {
                  //  text = text + " בלבד";
                }
                else
                {
                    text = text + " +";
                    len = cents.Length;
                    pos = len;
                    while (pos > 0)
                    {
                        n = len - pos + 1;
                        digit = cents[n - 1] - '0';//This works because each character is internally represented by a number.
                        String[] str;
                        if (digit > 0)
                        {
                            str = SliceRow(tr, digit - 1).ToArray();//one_dimensional slice of the corresponding digit 
                        }
                        else
                        {
                            str = new String[] { "", "", "", "", "" };
                        }

                        //                      v_num = v_n - 2
                        //                      If(v_num < 1) v_num = 1
                        //                      v_num = $Number(v_cents[v_num, v_n])
                        add = "";
                        switch (pos)
                        {
                            case 1:
                                if (n > 1 && cents[n - 2] == '1')// *10 - *19
                                {
                                    if (digit == 0) // *10
                                    {
                                        str1 = tr[0, 3]; // eser
                                        if (text != "") { text = text + ve; }
                                        text = text + str1;
                                    }
                                    else
                                    { // *11 - *19
                                        str1 = str[0]; // ahad, shneim, slosha, ...
                                        if (subunit_name == "אגורות" && str1 == "שנים")
                                        {
                                            str1 = "שתים";
                                        }
                                        if (text != "")
                                        {
                                            //text = text + " ";
                                            if (len > 1 && digit > 0)
                                            {
                                                str1 = ve + str1;
                                            }
                                            else
                                            {
                                                text = text + " ";
                                            }

                                        }

                                        text = text + str1 + asar + elef;
                                    }
                                }
                                else // *##
                                {
                                    if (digit == 2 && len == 1)
                                    {
                                        str1 = str[4]; // shnei 
                                        if (subunit_name == "אגורות")
                                        {
                                            str1 = "שתי";
                                        }
                                    }
                                    else
                                    {
                                        str1 = str[0]; // ehad, shnaiim, slosha, ... 
                                        if (subunit_name == "אגורות" && str1 == "שנים")
                                        {
                                            str1 = "שתים";
                                        }
                                    }
                                    if (len > 1 && digit > 0) { str1 = ve + str1; }
                                    text = text + str1;
                                }

                                break;

                            case 2:
                                str1 = str[3]; // eser, esrim, shloshim, ...
                                if (digit > 1)
                                {
                                    if (text != "") { text = text + " "; }
                                    text = text + str1;
                                }
                                break;
                            default:
                                break;
                        } // switch

                        pos -= 1;

                    } // while
                    text = text + " " + subunit_name;

                }
            }
            else
            {
                if (dcents == 0)
                {
                   // text = text + " בלבד";
                }
                else if (dcents == 1)
                {
                    text = text + " + " + cents + " " + subunit_name;
                }
                else
                {
                    text = text + " + " + cents + " " + subunit_name;
                }
            }

            if (eng)
            {
                pattern = " " + shah;
                if (text.Contains(pattern))
                {
                    string[] words = text.Split(pattern.ToArray<Char>());
                    left = words[0];
                    right = words[1];
                    text = left + right + pattern;
                }
            }

            text = Trim(text);
            text = FixSpaces(text);
            return (text+ " בלבד");
        }
        public string ConvertNumbersToFrenchNewVersion(double number,string localCurrencyName)
        {
            string currencyNameOfDecimalPart = "centimes";
            return HandleUnsignedNumberInFrenchWords(number,localCurrencyName, currencyNameOfDecimalPart);
            
        }

        private string FixSpaces(string s)
        {
            var t = "";
            for (var i = 0; i < s.Length; i++)
            {
                if (i > 0)
                {
                    if (!((s.Substring(i - 1, 1) == " ") & (s.Substring(i, 1) == " ")))
                        t = t + s.Substring(i, 1);
                }
                else
                    t = t + s.Substring(i, 1);
            }
            return t;


        }
        private string Trim(string s)
        {
            return LTrim(RTrim(s));
        }
        private string LTrim(string s)
        {
            s = s.Trim();
            var i = 0;
            var j = 0;
            for (i = 0; i <= s.Length - 1; i++)
                if (s.Substring(i, 1) != " ")
                {
                    j = i;
                   
                   break;
                }
            return  s.Substring(j, s.Length);
        }
        private string RTrim(string s)
        {
            var j = 0;
            for (var i = s.Length - 1; i > -1; i--)
                if (s.Substring(i, 1) != " ")
                {
                    j = i;
                    break;
                }
            return s.Substring(0, j + 1);
        }
        private int InStr(string n, string s1, string s2 = null)
        {
            if (s2 == null)
                return (n.IndexOf(s1) + 1);
            else
                return (s1.IndexOf(s2, int.Parse(n)) + 1);
        }
        private int RInStr(string n, string s1, string s2 = null)
        {
            if (s2 == null)
                return (n.LastIndexOf(s1) + 1);
            else
                return (s1.LastIndexOf(s2, int.Parse(n)) + 1);
        }

        private  string[] unitsMapInFranch = new[] { "zéro", "un", "deux", "trois", "quatre", "cinq", "six", "sept", "huit", "neuf", "dix", "onze", "douze", "treize", "quatorze", "quinze", "seize", "dix-sept", "dix-huit", "dix-neuf" };
        private  string[] tensMapInFranch = new[] { "zéro", "dix", "vingt", "trente", "quarante", "cinquante", "soixante", "soixante", "quatre-vingt", "quatre-vingt" };
        private  string HandleUnsignedNumberInFrenchWords(double number, string currencyName, string currencyNameOfDecimalPart)
        {
            if (number == 0)
                return "zéro";
            if (number == 1)
                return "un";
            if (number < 0)
                return "moins " + HandleSignedFullNumberInFrenchWords(Math.Abs(number), currencyName, currencyNameOfDecimalPart);

            return HandleSignedFullNumberInFrenchWords(number, currencyName, currencyNameOfDecimalPart);
        }
        private  string HandleSignedFullNumberInFrenchWords(double number, string currencyName, string currencyNameOfDecimalPart)
        {
            string amountInWords = "";
            string numberAsString = number.ToString();
            double valueOfDecimalPartOfTheNumber = 0, intgerNumberWithoutDecimals = 0;
            if (numberAsString.Contains('.'))
            {
                string decimalPartOfTheNumber = numberAsString.Split('.')[1];
                valueOfDecimalPartOfTheNumber = Convert.ToDouble(decimalPartOfTheNumber);
                intgerNumberWithoutDecimals = Convert.ToDouble(numberAsString.Split('.')[0]);
                bool checkIfDecimalDigitIsNotAnyTensValue = valueOfDecimalPartOfTheNumber >= 1 && valueOfDecimalPartOfTheNumber <= 9 && !decimalPartOfTheNumber.StartsWith("0");
                valueOfDecimalPartOfTheNumber = checkIfDecimalDigitIsNotAnyTensValue ? valueOfDecimalPartOfTheNumber * 10 : valueOfDecimalPartOfTheNumber;
                amountInWords += HandleIntegerNumberPartInFrenchWords(intgerNumberWithoutDecimals);
                if (!String.IsNullOrEmpty(currencyName))
                {
                    amountInWords += (" " + currencyName + HandleDecimalNumberPartInFrenchWords(valueOfDecimalPartOfTheNumber, true) + " " + currencyNameOfDecimalPart);
                }
                else
                {
                    amountInWords += (" virgule" + HandleDecimalNumberPartInFrenchWords(valueOfDecimalPartOfTheNumber, false));
                }
            }
            else
            {
                amountInWords += HandleIntegerNumberPartInFrenchWords(number) + " " + currencyName;
            }
            return amountInWords;
        }
        private  string HandleIntegerNumberPartInFrenchWords(double number)
        {
            string amountInWords = "";
            if ((number / 100000000000) > 0 && number >= 100000000000)
            {
                amountInWords += HandleIntegerNumberPartInFrenchWords(number / 1000000000000) + " billions ";
                number %= 100000000000;
            }
            if ((number / 1000000000) > 0 && number >= 1000000000)
            {
                amountInWords += HandleIntegerNumberPartInFrenchWords(number / 1000000000) + " milliard ";
                number %= 1000000000;
            }
            if ((number / 1000000) > 0 && number >= 1000000)
            {
                amountInWords += HandleIntegerNumberPartInFrenchWords(number / 1000000) + " million ";
                number %= 1000000;
            }
            if ((number / 1000) > 0 && number >= 1000)
            {
                amountInWords += HandleIntegerNumberPartInFrenchWords(number / 1000) + " mille ";
                number %= 1000;
            }
            if ((number / 100) > 0 && number >= 100)
            {
                amountInWords += HandleIntegerNumberPartInFrenchWords(number / 100) + " cent ";
                number %= 100;
            }
            if (number > 0)
            {
                amountInWords += HandleIntegerNumberFromOneToHundredInFrenchWords(number);
            }
            return amountInWords;
        }
        private  string HandleDecimalNumberPartInFrenchWords(double number, bool hasDecimalCurreny)
        {
            string amountInWords = "";
            List<int> splitedNumbers = new List<int>();
            int integerValueOfNumber = (int)number;
            if (hasDecimalCurreny)
            {  
              amountInWords += " " + HandleIntegerNumberPartInFrenchWords(number);
            }
            else
            {
                while (integerValueOfNumber > 0)
                {
                    int mod = integerValueOfNumber % 10;
                    splitedNumbers.Add(mod);
                    integerValueOfNumber = integerValueOfNumber / 10;
                }
                splitedNumbers.Reverse();
                foreach (int i in splitedNumbers)
                {
                    amountInWords += " " + unitsMapInFranch[i];
                }
            }
            return amountInWords;
        }
        private  string HandleIntegerNumberFromOneToHundredInFrenchWords(double number)
        {
            string amountInWords = "";
            if (number < 20)
            {
                amountInWords = ((int)number != 1) ? unitsMapInFranch[(int)number] : "";
            }
            else if (number < 70)
            {
                amountInWords += tensMapInFranch[(int)number / 10];
                if (number % 10 != 0 && number % 10 >= 1)
                    amountInWords += (number % 10) > 0 && (number % 10) != 1 ? "-" + unitsMapInFranch[(int)number % 10] : "-et-" + unitsMapInFranch[(int)number % 10];
            }
            else if (number < 80)
            {
                amountInWords += tensMapInFranch[(int)number / 10];
                if (number % 10 != 0 && number % 10 >= 1)
                {
                    number = number % 10 + 10;
                    if (number == 11)
                    {
                        amountInWords += "-et";
                    }
                    amountInWords += "-" + unitsMapInFranch[(int)number];
                }
            }
            else
            {
                amountInWords += HandleIntegerNumberFromEightyToHundredInFrenchWords(ref number);
            }
            return amountInWords;
        }
        private  string HandleIntegerNumberFromEightyToHundredInFrenchWords(ref double number)
        {
            string amountInWords = "";
            if (number < 90)
            {
                if (number == 80)
                    amountInWords += "quatre-vingts";
                else
                {
                    amountInWords += tensMapInFranch[(int)number / 10];
                    if (number == 0)
                    {
                        amountInWords += "s";
                    }
                    else
                    {
                        number = number % 10;
                        amountInWords += "-" + unitsMapInFranch[(int)number];
                    }
                }
            }
            else
            {
                amountInWords += tensMapInFranch[(int)number / 10];
                number = number % 10 + 10;
                amountInWords += "-" + unitsMapInFranch[(int)number];
            }
            return amountInWords;
        }

    }
}