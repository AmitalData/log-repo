using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Server.Infrastructure.DataContracts
{
    public class CustomFieldClass
    {
        public CustomFieldClass()
        {

        }

        public string Value { get; set; }
        public string FieldName { get; set; }
        public string TableName { get; set; }
        public CustomFieldClass(string fieldName, string objectTableName, string value)
        {
            this.FieldName = fieldName;
            this.TableName = objectTableName;
            this.Value = value;
        }

        public string SetFieldDataType(string dataType, object value)
        {
            if (value != null)
            {
                if (String.IsNullOrEmpty(value.ToString()))
                {
                    return null;
                }
                switch (dataType)
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
                            //if (String.IsNullOrEmpty(value.ToString()))
                            //{
                            //    return null;
                            //}
                            decimal d = 0;
                            decimal.TryParse(value.ToString(), out d);
                            string decimalTostore = ConvertToString(d,true);
                            return decimalTostore;
                        }
                    case "UnsDecimal":
                        {
                            //if (String.IsNullOrEmpty(value.ToString()))
                            //{
                            //    return null;
                            //}
                            decimal d = 0;
                            decimal.TryParse(value.ToString(), out d);
                            string decimalTostore = ConvertToString(d,false);
                            return decimalTostore;
                        }
                    case "Integer":
                        {
                            int d = 0;//Convert.ToInt32(value);
                            int.TryParse(value.ToString(), out d);
                            string intTostore = ConvertToString(d,true);
                            return intTostore;
                        }
                    case "UnsInteger":
                        {
                            int d = 0;//Convert.ToInt32(value);
                            int.TryParse(value.ToString(), out d);
                            string intTostore = ConvertToString(d, false);
                            return intTostore;
                        }
                    case "Double":
                        {
                            double d = 0;//Convert.ToDouble(value);
                            double.TryParse(value.ToString(), out d);
                            string doubleTostore = ConvertToString(d,false);
                            return doubleTostore;
                        }
                    case "SigDouble":
                        {
                            double d = 0;//Convert.ToDouble(value);
                            double.TryParse(value.ToString(), out d);
                            string doubleTostore = ConvertToString(d, true);
                            return doubleTostore;
                        }
                    case "boolean":
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

        public string ConvertToString(DateTime date)
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

        public string ConvertToString(int d,bool signed)
        {
            string fmt = "000000000000";
            string dString = d.ToString(fmt);

            if (signed)
            {
                dString = (d.ToString().Contains("-") ? "-" + d.ToString("D12") : "+" + d.ToString("D12"));
            }   


            return dString;
        }
        public string ConvertToString(double d,bool signed)
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
        public string ConvertToString(decimal d,bool signed)
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
