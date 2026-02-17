using System;

using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace WebFreight.Web.Helpers
{
    public class FieldValueResolver
    {

        #region GetFieldDataValue


        public static object GetFieldDataValue(ObjectField field, string value)
        {
            if (field != null && value != null)
            {
                switch (field.DataTypeCode.Trim())
                {
                    case "Text":
                        {
                            //this._Text = customField.ToString();
                            return value.ToString();

                        }
                    case "nText":
                        {
                            //this._Text = customField.ToString();
                            return value.ToString();

                        }
                    case "DateTime":
                        {
                            //DateTime result;
                            //bool success = DateTime.TryParse(value, out result);
                            //DateTime? date = null;
                            //if (success)
                            //{
                            //    date = result;
                            //}
                            DateTime? date = ConvertToDate(value);
                            //this._DateTime = date;
                            return date;
                        }

                    case "Date":
                        {
                            //DateTime result;
                            //bool success = DateTime.TryParse(value, out result);
                            //DateTime? date = null;
                            //if (success)
                            //{
                            //    date = result.Date;
                            // }

                            DateTime? date = ConvertToDate(value);
                            //this._Date = date;
                            return date;
                        }
                    case "Decimal":
                        {
                            decimal d = Convert.ToDecimal(value);
                            //this._Decimal = d;
                            return d;
                        }
                    case "Integer":
                        {
                            int i = Convert.ToInt32(value);
                            //this._Integer = i;
                            return i;
                        }
                    case "Double":
                        {
                            double d = Convert.ToDouble(value);
                            //this._Double = d;
                            return d;
                        }
                    case "Boolean":
                        {
                            bool b = Convert.ToBoolean(value);
                            //this._Boolean = b;
                            return b;
                        }
                    case "LookUp":
                        {
                            //this._Text = customField.ToString();
                            return value.ToString();

                        }
                    case "PickList":
                        {
                            //this._Text = customField.ToString();
                            return value.ToString();

                        }
                    default:
                        {
                            return value != null ? value.ToString() : null;
                        }
                }



            }
            return null;



        }

        #endregion


        #region  #endregion


        public static string GetFieldStringValue(ObjectField field, object value)
        {
            if (field != null && value != null)
            {
                if (String.IsNullOrEmpty(value.ToString()))
                {
                    return null;
                }
                switch (field.DataTypeCode.Trim())
                {
                    case "Text":
                        {
                            return value.ToString();

                        }

                    case "nText":
                        {
                            return value.ToString();

                        }
                    case "LookUp":
                        {
                            return value.ToString();

                        }
                    case "PickList":
                        {
                            return value.ToString();

                        }
                    case "DateTime":
                        {

                            DateTime date = Convert.ToDateTime(value);
                            string dateToStore = ConvertToString(date);
                            return dateToStore;

                        }

                    case "Date":
                        {

                            DateTime date = Convert.ToDateTime(value);
                            string dateToStore = ConvertToString(date);
                            return dateToStore;

                        }
                    case "Decimal":
                        {
                            if (String.IsNullOrEmpty(value.ToString()))
                            {
                                return null;
                            }
                            decimal d = Convert.ToDecimal(value);
                            string decimalTostore = ConvertToString(d);
                            return decimalTostore;
                        }
                    case "Integer":
                        {
                            int d = Convert.ToInt32(value);
                            string intTostore = ConvertToString(d);
                            return intTostore;
                        }
                    case "Double":
                        {
                            double d = Convert.ToDouble(value);
                            string doubleTostore = ConvertToString(d);
                            return doubleTostore;
                        }
                    case "Boolean":
                        {
                            return value.ToString();
                        }
                    default:
                        {
                            return value != null ? value.ToString() : null;
                        }
                }



            }
            return null;




        }

        #endregion


        #region ConvertToString Date



        public static string ConvertToString(DateTime date)
        {


            string time = "";
            string month, day, minuit, second, hour;


            if (date.Month < 10)
            {
                month = "0" + date.Month.ToString();

            }
            else { month = date.Month.ToString(); }

            if (date.Day < 10)
            {
                day = "0" + date.Day.ToString();

            }
            else { day = date.Day.ToString(); }

            if (date.Hour < 10)
            {
                hour = "0" + date.Hour.ToString();

            }
            else { hour = date.Hour.ToString(); }

            if (date.Minute < 10)
            {
                minuit = "0" + date.Minute.ToString();

            }
            else { minuit = date.Minute.ToString(); }

            if (date.Second < 10)
            {
                second = "0" + date.Second.ToString();

            }
            else { second = date.Second.ToString(); }


            time = date.Year.ToString() + month + day + hour + minuit + second;


            return time;


        }

        #endregion

        #region ConvertToString Integer


        public static string ConvertToString(int d)
        {
            string dstring = d.ToString();
            string s = "";
            if (dstring.Length == 1)
            {
                s = "0000000" + dstring;
            }

            if (dstring.Length == 2)
            {
                s = "000000" + dstring;
            }
            if (dstring.Length == 3)
            {
                s = "00000" + dstring;
            }
            if (dstring.Length == 4)
            {
                s = "0000" + dstring;
            }
            if (dstring.Length == 5)
            {
                s = "000" + dstring;
            }
            if (dstring.Length == 6)
            {
                s = "00" + dstring;
            }

            if (dstring.Length == 7)
            {
                s = "0" + dstring;
            }
            if (dstring.Length == 8)
            {
                s = dstring;
            }
            return s;
        }

        #endregion

        #region ConvertToString Double


        public static string ConvertToString(double d)
        {
            string dstring = d.ToString();
            string s = "";
            if (dstring.Length == 1)
            {
                s = "0000000" + dstring;
            }

            if (dstring.Length == 2)
            {
                s = "000000" + dstring;
            }
            if (dstring.Length == 3)
            {
                s = "00000" + dstring;
            }
            if (dstring.Length == 4)
            {
                s = "0000" + dstring;
            }
            if (dstring.Length == 5)
            {
                s = "000" + dstring;
            }
            if (dstring.Length == 6)
            {
                s = "00" + dstring;
            }

            if (dstring.Length == 7)
            {
                s = "0" + dstring;
            }
            if (dstring.Length == 8)
            {
                s = dstring;
            }
            return s;
        }

        #endregion


        #region ConvertToString Decimal



        public static string ConvertToString(decimal d)
        {
            string dstring = d.ToString();
            string s = "";
            if (dstring.Length == 1)
            {
                s = "0000000" + dstring;
            }

            if (dstring.Length == 2)
            {
                s = "000000" + dstring;
            }
            if (dstring.Length == 3)
            {
                s = "00000" + dstring;
            }
            if (dstring.Length == 4)
            {
                s = "0000" + dstring;
            }
            if (dstring.Length == 5)
            {
                s = "000" + dstring;
            }
            if (dstring.Length == 6)
            {
                s = "00" + dstring;
            }

            if (dstring.Length == 7)
            {
                s = "0" + dstring;
            }
            if (dstring.Length == 8)
            {
                s = dstring;
            }
            return s;
        }

        #endregion


        #region ConvertToDate


        private static DateTime? ConvertToDate(string s)
        {
            DateTime? date = null;
            if (!string.IsNullOrEmpty(s))
            {

                date = new DateTime(System.Convert.ToInt32(s.Substring(0, 4)), System.Convert.ToInt32(s.Substring(4, 2)), System.Convert.ToInt32(s.Substring(6, 2)), System.Convert.ToInt32(s.Substring(8, 2)), System.Convert.ToInt32(s.Substring(10, 2)), System.Convert.ToInt32(s.Substring(12, 2)));


            }

            return date;

        }
        #endregion




        public string GetFieldDataString(ObjectField field, object value)
        {
            if (field != null && value != null)
            {
                if (String.IsNullOrEmpty(value.ToString()))
                {
                    return null;
                }
                switch (field.DataTypeCode.Trim())
                {
                    case "Text":
                    case "nText":
                    case "LookUp":
                    case "PickList":
                        {
                            return value.ToString();

                        }

                    case "DateTime":
                    case "Date":
                        {

                            DateTime date = Convert.ToDateTime(value);
                            string dateToStore = ConvertToString(date);
                            return dateToStore;

                        }
                    case "Decimal":
                        {
                            decimal d = 0;
                            decimal.TryParse(value.ToString(), out d);
                            string decimalTostore = ConvertToString(d, true);
                            return decimalTostore;
                        }
                    case "UnsDecimal":
                        {

                            decimal d = 0;
                            decimal.TryParse(value.ToString(), out d);
                            string decimalTostore = ConvertToString(d, false);
                            return decimalTostore;
                        }
                    case "Integer":
                        {
                            int d = 0;
                            int.TryParse(value.ToString(), out d);
                            string intTostore = ConvertToString(d, true);
                            return intTostore;
                        }
                    case "UnsInteger":
                        {
                            int d = 0;
                            int.TryParse(value.ToString(), out d);
                            string intTostore = ConvertToString(d, false);
                            return intTostore;
                        }
                    case "Double":
                        {
                            double d = 0;
                            double.TryParse(value.ToString(), out d);
                            string doubleTostore = ConvertToString(d, false);
                            return doubleTostore;
                        }

                    case "SigDouble":
                        {
                            double d = 0;
                            double.TryParse(value.ToString(), out d);
                            string doubleTostore = ConvertToString(d, true);
                            return doubleTostore;
                        }
                    case "Boolean":
                        {
                            return value.ToString();
                        }
                    default:
                        {
                            return (value != null ? value.ToString() : null);
                        }
                }

            }

            return null;
        }

      
        
        public string ConvertToString(int d, bool signed)
        {
            string fmt = "000000000000";
            string dString = d.ToString(fmt);

            if (signed)
            {
                dString = (d.ToString().Contains("-") ? "-" + d.ToString("D12") : "+" + d.ToString("D12"));
            }


            return dString;
        }
        public string ConvertToString(double d, bool signed)
        {
            string fmt = "000000000000.000";

            string dString = d.ToString(fmt);
            dString = dString.Replace("+", "").Replace("-", "").Replace(".", "");

            if (signed)
            {
                dString = (d.ToString().Contains("-") ? "-" + dString : "+" + dString);
            }

            return dString;
        }
        public string ConvertToString(decimal d, bool signed)
        {
            string fmt = "000000000000.000";

            string dString = d.ToString(fmt);
            dString = dString.Replace("+", "").Replace("-", "").Replace(".", "");

            if (signed)
            {
                dString = (d.ToString().Contains("-") ? "-" + dString : "+" + dString);
            }

            return dString;
        }

    }
}