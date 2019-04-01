using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class DataWarehouseHelper
    {



        public string ResolveWarehoueDateField(string fieldName, string operationCode, string fieldValue, int tenant,bool isSample = false)
        {
            string result = string.Empty;


            if ((operationCode == "Before" || operationCode == "After")) result = ResolveBeforeAfterDateValue(fieldName, operationCode, fieldValue, tenant, isSample);
            else
            {
                if (ValidateFieldValue(operationCode, fieldValue))
                {
                    DateTime currentDate =  TenantServerConfigration.GetCurrentDateTime(tenant);
                    var valuesArray = fieldValue.Split('^');
                    if (operationCode == "Previous" && valuesArray.Length == 3) result = ResolvePreviousDateValue(fieldName, valuesArray[1], valuesArray[2], currentDate, isSample);
                    else if (operationCode == "Next" && valuesArray.Length == 3) result = ResolveNextDateValue(fieldName, valuesArray[1], valuesArray[2], currentDate, isSample);
                    else if (operationCode == "Current" && valuesArray.Length == 2) result = ResolveCurrentDateValue(fieldName, valuesArray[1], currentDate, isSample);

                }


            }



            return result;

        }

        public bool ValidateFieldValue(string operationCode, string fieldValue)
        {
            bool isValid = true;
            string range = string.Empty;
            int interval = 0;
            if (!string.IsNullOrEmpty(operationCode) && !string.IsNullOrEmpty(fieldValue) && fieldValue.Contains('^'))
            {
                var valuesArray = fieldValue.Split('^');
                if (operationCode == "Previous" || operationCode == "Next")
                {
                    if (valuesArray.Length == 3)
                    {
                        interval = !string.IsNullOrEmpty(valuesArray[1]) ? Int32.Parse(valuesArray[1]) : 0;
                        range = valuesArray[2];
                    }

                    //if (interval <= 0) isValid = false;

                }
                else
                {
                    if (valuesArray.Length == 2) range = valuesArray[1];
                }

                if (range != "Day" && range != "Week" && range != "Month" && range != "Quarter" && range != "Year") isValid = false;
            }
            else isValid = false;

            return isValid;

        }



        private string ResolveBeforeAfterDateValue(string fieldName, string operationCode, string fieldValue, int tenant,bool isSample = false)
        {
            DateTime date = DateTime.Parse(fieldValue);

            if (operationCode == "After" && !string.IsNullOrEmpty(fieldValue)) fieldValue = string.Format("{0:yyyy-MM-dd}", DateTime.Parse(fieldValue).AddDays(1));
            string operationSimpol = operationCode == "After" ? " >'" : "<'";
            string result = fieldName + operationSimpol + fieldValue + "'";
            if (isSample)
            {
                result = "( " + operationSimpol.Replace("'","") + " " + fieldValue.Replace("-", "/") + " )";
            }

            return result;
        }

        private string ResolveCurrentDateValue(string fieldName, string range, DateTime currentDate, bool isSample = false)
        {
            DateTime? fromDate = null;
            DateTime? toDate = null;

            switch (range)
            {
                case "Day":
                    fromDate = currentDate;
                    break;

                case "Week":

                    fromDate = StartOfWeek(currentDate, DayOfWeek.Monday);
                    toDate = fromDate.Value.AddDays(7);
                    break;

                case "Month":

                    fromDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                    toDate = fromDate.Value.AddMonths(1);
                    break;

                case "Quarter":
                    fromDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                    toDate = fromDate.Value.AddMonths(3);
                    break;

                case "Year":

                    fromDate = new DateTime(currentDate.Year, 1, 1);
                    toDate = fromDate.Value.AddYears(1);
                    break;



                default:
                    fromDate = currentDate;
                    toDate = currentDate;
                    break;
            }

            string result = BuildDateSql(fromDate, toDate, fieldName, "Current", range, isSample);

            return result;

        }

        private string ResolvePreviousDateValue(string fieldName, string interval, string range, DateTime currentDate, bool isSample = false)
        {

            DateTime? fromDate = null;
            DateTime? toDate = null;

            int intervalNumber = !string.IsNullOrEmpty(interval) ? Int32.Parse(interval) : 0;
            intervalNumber = intervalNumber * -1;

            if (range == "Quarter")
            {
                intervalNumber = intervalNumber * 3;
                range = "Month";
            }

            switch (range)
            {
                case "Day":
                    fromDate = currentDate.AddDays(intervalNumber);
                    toDate = currentDate;
                    break;

                case "Week":

                    DateTime startOfWeekDate = StartOfWeek(currentDate, DayOfWeek.Monday);
                    fromDate = startOfWeekDate.AddDays(7 * intervalNumber).AddDays(-1);
                    toDate = startOfWeekDate;

                    break;

                case "Month":
                    fromDate = currentDate.AddMonths(intervalNumber);
                    fromDate = new DateTime(fromDate.Value.Year, fromDate.Value.Month, 1);
                    toDate = DateTime.Parse(fromDate.ToString()).AddMonths(intervalNumber > 0 ? (intervalNumber * -1) : Math.Abs(intervalNumber));


                    break;

                case "Quarter":
                    intervalNumber = intervalNumber * 3;
                    fromDate = currentDate.AddMonths(intervalNumber);
                    fromDate = new DateTime(fromDate.Value.Year, fromDate.Value.Month, 1);
                    toDate = DateTime.Parse(fromDate.ToString()).AddMonths(intervalNumber > 0 ? (intervalNumber * -1) : Math.Abs(intervalNumber));

                    break;

                case "Year":

                    fromDate = new DateTime(currentDate.AddYears(intervalNumber).Year, 1, 1);
                    toDate = DateTime.Parse(fromDate.ToString()).AddYears(intervalNumber > 0 ? (intervalNumber * -1) : Math.Abs(intervalNumber));


                    break;
                default:
                    fromDate = currentDate;
                    toDate = currentDate;
                    break;
            }

            string result = BuildDateSql(fromDate, toDate, fieldName,null,null,isSample);

            return result;

        }

        private string ResolveNextDateValue(string fieldName, string interval, string range, DateTime currentDate, bool isSample = false)
        {

            DateTime? fromDate = null;
            DateTime? toDate = null;

            int intervalNumber = !string.IsNullOrEmpty(interval) ? Int32.Parse(interval) : 0;

            switch (range)
            {
                case "Day":
                    fromDate = currentDate.AddDays(1);
                    toDate = currentDate.AddDays((intervalNumber + 1));

                    break;

                case "Week":


                    DateTime startOfWeekDate = StartOfWeek(currentDate, DayOfWeek.Monday);
                    toDate = startOfWeekDate.AddDays(7 * (intervalNumber + 1));
                    fromDate = DateTime.Parse(toDate.ToString()).AddDays(7 * (intervalNumber > 0 ? (intervalNumber * -1) : Math.Abs(intervalNumber)));

                    break;

                case "Month":

                    toDate = currentDate.AddMonths((intervalNumber + 1));
                    toDate = new DateTime(toDate.Value.Year, toDate.Value.Month, 1);
                    fromDate = DateTime.Parse(toDate.ToString()).AddMonths(intervalNumber > 0 ? (intervalNumber * -1) : Math.Abs(intervalNumber));



                    break;

                case "Quarter":

                    toDate = AddQuarters(currentDate, (intervalNumber + 1));
                    toDate = new DateTime(toDate.Value.Year, toDate.Value.Month, 1);
                    fromDate = AddQuarters((DateTime)toDate, intervalNumber > 0 ? (intervalNumber * -1) : Math.Abs(intervalNumber));


                    break;

                case "Year":

                    toDate = new DateTime(currentDate.AddYears(intervalNumber + 1).Year, 1, 1);
                    fromDate = DateTime.Parse(toDate.ToString()).AddYears(intervalNumber > 0 ? (intervalNumber * -1) : Math.Abs(intervalNumber));


                    break;
                default:
                    toDate = currentDate;
                    fromDate = currentDate;
                    break;
            }

            string result = BuildDateSql(fromDate, toDate, fieldName,null,null,isSample);

            return result;

        }




        private DateTime AddQuarters(DateTime originalDate, int quarters)
        {
            return originalDate.AddMonths(quarters * 3);
        }

        private DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        private string BuildDateSql(DateTime? fromDate, DateTime? toDate, string fieldName, string operatorCode = null, string range = null, bool isSample = false)
        {
            string result = string.Empty;
            string fromDateString = fromDate != null ? string.Format("{0:yyyy-MM-dd}", fromDate) : "";
            string toDateString = toDate != null ? (string.Format("{0:yyyy-MM-dd}", toDate)) : "";


            if (range == "Day" && operatorCode == "Current")
            {
                result = (fieldName + "= '" + fromDateString + "'");
                if (isSample)
                {
                    result = "( = " + fromDateString.Replace("-", "/") + " )";
                }
            }
            else
            {
                result = (fieldName + " >= '" + fromDateString + "' and " + fieldName + " < '" + toDateString + "'");
                if (isSample)
                {
                    result = "( " + fromDateString.Replace("-","/") + " - " + toDateString.Replace("-", "/") + " )";
                }
            }



            return result;


        }

    }
}
