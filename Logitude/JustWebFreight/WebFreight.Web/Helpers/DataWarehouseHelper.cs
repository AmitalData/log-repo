using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class DataWarehouseHelper
    {
        public string ResolveWarehoueDateField(string fieldName, string fieldValue, int tenant)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(fieldValue))
            {
                var valuesArray = fieldValue.Split('^');
                if (valuesArray.Length > 0)
                {
                    if (valuesArray[0] == "Prev" && valuesArray.Length == 3) result = ResovePreviousDateValue(fieldName, valuesArray[1], valuesArray[2], tenant);
                    else if (valuesArray[0] == "Next" && valuesArray.Length == 3) result = ResoveNextDateValue(fieldName, valuesArray[1], valuesArray[2], tenant);
                    else if (valuesArray[0] == "Current" && valuesArray.Length == 2) result = ResoveCurrentDateValue(fieldName, valuesArray[1], tenant);
                }
            }
            return result;

        }

        private string ResoveCurrentDateValue(string fieldName, string range, int tenant)
        {
            string result = string.Empty;
            DateTime currentDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            DateTime? fromDate = null;
            DateTime? toDate = null;
            switch (range)
            {
                case "Day":
                    fromDate = currentDate;
                    toDate = currentDate.AddDays(1);

                    break;

                case "Week":
                    fromDate = currentDate.AddDays(7);
                    toDate = fromDate.Value.AddDays(-1);
                    break;

                case "Month":

                    fromDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                    toDate = fromDate.Value.AddMonths(1).AddDays(-1);
                    break;

                case "Quarter":
                    fromDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                    toDate = fromDate.Value.AddMonths(3).AddDays(-1);
                    break;

                case "Year":

                    fromDate = new DateTime(currentDate.Year, 1, 1);
                    toDate = fromDate.Value.AddYears(1).AddDays(-1);
                    break;



                default:
                    fromDate = currentDate;
                    toDate = currentDate;
                    break;
            }


            result = fieldName + " >= '" + fromDate + "' and " + fieldName + " < '" + toDate + "'";
            return result;
        }

        private string ResoveNextDateValue(string fieldName, string interval, string range, int tenant)
        {
            string result = string.Empty;
            DateTime currentDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            DateTime? fromDate = null;
            DateTime? toDate = null;

            int intervalNumber = !string.IsNullOrEmpty(interval) ? Int32.Parse(interval) : 0;

            switch (range)
            {
                case "Day":
                    fromDate = currentDate.AddDays(-1);
                    toDate = currentDate.AddDays(intervalNumber);
                    break;

                case "Week":

                    toDate = currentDate.AddDays(7 * (intervalNumber + 1));
                    fromDate = DateTime.Parse(toDate.ToString()).AddDays(7 * (intervalNumber > 0 ? (intervalNumber * -1) : Math.Abs(intervalNumber)));
                    toDate = toDate.Value.AddDays(-1);
                    break;

                case "Month":

                    toDate = currentDate.AddMonths((intervalNumber + 1));
                    toDate = new DateTime(toDate.Value.Year, toDate.Value.Month, 1);
                    fromDate = DateTime.Parse(toDate.ToString()).AddMonths(intervalNumber > 0 ? (intervalNumber * -1) : Math.Abs(intervalNumber));
                    toDate = toDate.Value.AddDays(-1);

                    break;

                case "Quarter":

                    toDate = AddQuarters(currentDate, (intervalNumber + 1));
                    toDate = new DateTime(toDate.Value.Year, toDate.Value.Month, 1);
                    fromDate = AddQuarters((DateTime)toDate, intervalNumber > 0 ? (intervalNumber * -1) : Math.Abs(intervalNumber));
                    toDate = toDate.Value.AddDays(-1);

                    break;


                case "Year":

                    toDate = new DateTime(currentDate.AddYears(intervalNumber + 1).Year, 1, 1);
                    fromDate = DateTime.Parse(toDate.ToString()).AddYears(intervalNumber > 0 ? (intervalNumber * -1) : Math.Abs(intervalNumber));
                    toDate = toDate.Value.AddDays(-1);

                    break;
                default:
                    toDate = currentDate;
                    fromDate = currentDate;
                    break;
            }

            result = fieldName + " > '" + fromDate + "' and " + fieldName + " <= '" + toDate + "'";

            return result != null ? result.ToString() : "";
        }

        private string ResovePreviousDateValue(string fieldName, string interval, string range, int tenant)
        {
            string result = string.Empty;
            DateTime currentDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            DateTime? fromDate = null;
            DateTime? toDate = null;

            int intervalNumber = !string.IsNullOrEmpty(interval) ? Int32.Parse(interval) : 0;
            intervalNumber = intervalNumber * -1;

            switch (range)
            {
                case "Day":
                    fromDate = currentDate.AddDays(intervalNumber);
                    toDate = currentDate.AddDays(-1);

                    break;

                case "Week":
                    fromDate = currentDate.AddDays(7 * intervalNumber);
                    toDate = currentDate.AddDays(-1);
                    fromDate = fromDate.Value.AddDays(-1);
                    break;

                case "Month":
                    fromDate = currentDate.AddMonths(intervalNumber);
                    fromDate = new DateTime(fromDate.Value.Year, fromDate.Value.Month, 1);
                    toDate = DateTime.Parse(fromDate.ToString()).AddMonths(intervalNumber > 0 ? (intervalNumber * -1) : Math.Abs(intervalNumber));
                    toDate = toDate.Value.AddDays(-1);

                    break;

                case "Quarter":

                    fromDate = AddQuarters(currentDate, intervalNumber);
                    fromDate = new DateTime(fromDate.Value.Year, fromDate.Value.Month, 1);
                    toDate = AddQuarters((DateTime)fromDate, intervalNumber > 0 ? (intervalNumber * -1) : Math.Abs(intervalNumber)).AddDays(-1);

                    break;

                case "Year":

                    fromDate = new DateTime(currentDate.AddYears(intervalNumber).Year, 1, 1);
                    toDate = DateTime.Parse(fromDate.ToString()).AddYears(intervalNumber > 0 ? (intervalNumber * -1) : Math.Abs(intervalNumber)).AddDays(-1);

                    break;
                default:
                    fromDate = currentDate;
                    toDate = currentDate;
                    break;
            }

            result = fieldName + " > '" + fromDate + "' and " + fieldName + " <= '" + toDate + "'";

            return result != null ? result.ToString() : "";
        }

        private DateTime AddQuarters(DateTime originalDate, int quarters)
        {
            return originalDate.AddMonths(quarters * 3);
        }
    }
}