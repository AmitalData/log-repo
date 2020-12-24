using CHAMP17;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class AdvancedDateResolver
    {
        public DateTime GetDateValueByOptionCode(string optionCode)
        {
            //Date date = new Date();
            DateTime dateValue = new DateTime();
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
                    new DateTime(DateTime.Today.Year, DateTime.Today.Month - 1, 1);
                    break;
                case "BTQ":
                    //dateValue = dateOption.Name;
                    break;
                case "BLQ":
                    //dateValue = dateOption.Name;
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
                    dateValue = dateValue.AddMonths(-1).AddDays(-1);
                    break;
                case "ETQ":
                    //dateValue = dateOption.Name;
                    break;
                case "ELQ":
                    //dateValue = dateOption.Name;
                    break;
                case "ETY":
                    dateValue = new DateTime(DateTime.Now.Year, 12, 31);
                    break;
                case "ELY":
                    dateValue = new DateTime(DateTime.Now.Year-1, 12, 31);
                    break;
                default:
                    dateValue = Convert.ToDateTime(optionCode);
                    break;
            }
            return dateValue;
        }
    }
}