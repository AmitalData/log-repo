using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class DateHelper
    {
        public static DateTime? GetDate(string dateString)
        {
            DateTime? myResult = null;

            if (!string.IsNullOrEmpty(dateString))
            {
                string[] dateParts = dateString.Split(':');

                int Year = 0;
                if (!string.IsNullOrEmpty(dateParts[0]))
                {
                    Int32.TryParse(dateParts[0], out Year);
                }

                int Month = 0;
                if (!string.IsNullOrEmpty(dateParts[1]))
                {
                    Int32.TryParse(dateParts[1], out Month);
                }

                int Day = 0;
                if (!string.IsNullOrEmpty(dateParts[2]))
                {
                    Int32.TryParse(dateParts[2], out Day);
                }

                int Hour = 0;
                if (!string.IsNullOrEmpty(dateParts[3]))
                {
                    Int32.TryParse(dateParts[3], out Hour);
                }

                int Minut = 0;
                if (!string.IsNullOrEmpty(dateParts[4]))
                {
                    Int32.TryParse(dateParts[4], out Minut);
                }

                int Second = 0;
                if (!string.IsNullOrEmpty(dateParts[5]))
                {
                    Int32.TryParse(dateParts[5], out Second);
                }

                myResult = new DateTime(Year, Month, Day, Hour, Minut, Second);
            }

            return myResult;
        }
    }
}