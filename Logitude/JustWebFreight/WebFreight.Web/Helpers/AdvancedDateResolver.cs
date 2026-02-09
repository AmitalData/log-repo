using CHAMP17;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class AdvancedDateResolver
    {
        public List<QueryFilterItem> ResolveDateValues(List<QueryFilterItem> reportFilterItemLists)
        {
            List<QueryFilterItem> reportFilterItems = reportFilterItemLists;
            reportFilterItems.ForEach(reportFilter =>
            {
                if (reportFilter.FieldDataType == "Date")
                {
                    reportFilter.FieldValue = GetDateValueByOptionCode(reportFilter.FieldValue?.ToString(), reportFilter.FieldName == "FromDate");
                }
            });
            return reportFilterItems;
        }
        public object GetDateValueByOptionCode(string optionCode, bool? fromDate = null)
        {
            DateTime dateValue = new DateTime();
            int quarterNumber = (DateTime.Now.Month - 1) / 3 + 1;
            switch (optionCode)
            {
                case "TOD":
                    dateValue = DateTime.Today;
                    break;
                case "YES":
                    dateValue = DateTime.Today.AddDays(-1);
                    break;
                case "BTM":
                    dateValue = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    break;
                case "BLM":
                    var temp = DateTime.Today.AddMonths(-1);
                    dateValue = new DateTime(temp.Year, temp.Month, 1);
                    break;
                case "BTQ":
                    dateValue = new DateTime(DateTime.Now.Year, (quarterNumber - 1) * 3 + 1, 1);
                    break;
                case "BLQ":
                    dateValue = GetPreviousQuarterFirstDate();
                    break;
                case "BTY":
                    dateValue = new DateTime(DateTime.Now.Year, 1, 1);
                    break;
                case "BLY":
                    dateValue = new DateTime(DateTime.Now.Year-1, 1, 1);
                    break;
                case "ETM":
                    dateValue = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    dateValue = dateValue.AddMonths(1).AddDays(-1);
                    break;
                case "ELM":
                    dateValue = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    dateValue = dateValue.AddDays(-1);
                    break;
                case "ETQ":
                    DateTime firstDayOfQuarter = new DateTime(DateTime.Now.Year, (quarterNumber - 1) * 3 + 1, 1);
                    dateValue = firstDayOfQuarter.AddMonths(3).AddDays(-1);
                    break;
                case "ELQ":
                    DateTime myFirstDayOfQuarter = GetPreviousQuarterFirstDate();
                    dateValue = myFirstDayOfQuarter.AddMonths(3).AddDays(-1);
                    break;
                case "ETY":
                    dateValue = new DateTime(DateTime.Now.Year, 12, 31);
                    break;
                case "ELY":
                    dateValue = new DateTime(DateTime.Now.Year-1, 12, 31);
                    break;
                default:
                    if (string.IsNullOrEmpty(optionCode))
                    {
                        return optionCode;
                    }
                    else if (optionCode.StartsWith("PER_"))
                    {
                        // example: PER_2_Weeks
                        if (fromDate == null)
                        {
                            throw new ArgumentException("fromDate parameter must be provided for period calculations (PER)");
                        }
                        var parts = optionCode.Split('_');
                        var value = int.Parse(parts[1]);
                        var unit = parts[2].ToLower();
                        var sign = fromDate == true ? -1 : 1;

                        dateValue = GetDateWithOffset(unit, sign * value, DateTime.Now);
                    }
                    else if (optionCode.Contains("_"))
                    {
                        // example: BTM_PLUS_2_Weeks
                        var parts = optionCode.Split('_');
                        if (parts?.Length != 4)
                        {
                            throw new ArgumentException("Invalid option code format: " + optionCode);
                        }

                        optionCode = parts[0];
                        var offset = (parts[1]?.ToLower() == "minus" ? -1 : 1) * int.Parse(parts[2]);
                        DateTime newDateValue = Convert.ToDateTime(GetDateValueByOptionCode(optionCode));
                        var unit = parts[3].ToLower();

                        dateValue = GetDateWithOffset(unit, offset, newDateValue);
                    }
                    else
                    {
                        dateValue = Convert.ToDateTime(optionCode);
                    }
                    break;
            }
            return dateValue;
        }

        private DateTime GetDateWithOffset(string unit, int offset, DateTime dateValue)
        {
            switch (unit)
            {
                case "days":
                    dateValue = dateValue.AddDays(offset);
                    break;

                case "weeks":
                    dateValue = dateValue.AddDays(offset * 7);
                    break;

                case "months":
                    dateValue = dateValue.AddMonths(offset);
                    break;

                case "years":
                    dateValue = dateValue.AddYears(offset);
                    break;

                default:
                    throw new ArgumentException("Unsupported period unit: " + unit);
            }
            return dateValue;
        }

        private DateTime GetPreviousQuarterFirstDate()
        {
            DateTime firstDayOfQuarter;
            int quarterNumber = (DateTime.Now.Month - 1) / 3 + 1;
            if (quarterNumber == 1)
            {
                quarterNumber = 4;
                firstDayOfQuarter = new DateTime(DateTime.Now.Year - 1, (quarterNumber - 1) * 3 + 1, 1);
            }
            else
            {
                quarterNumber--;
                firstDayOfQuarter = new DateTime(DateTime.Now.Year, (quarterNumber - 1) * 3 + 1, 1);
            }
            return firstDayOfQuarter;
        }

    }
}