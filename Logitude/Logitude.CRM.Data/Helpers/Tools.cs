using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.CRM.Data.Helpers
{
    public class Tools
    {
        [ThreadStatic] public static string AuthenticatedUserEmail = string.Empty;
        public static string GetAuthenticatedUser()
        {
            if(HttpContext.Current == null && !string.IsNullOrEmpty(AuthenticatedUserEmail))
            {
                return AuthenticatedUserEmail;
            }
            if (HttpContext.Current != null)
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    return HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    if (!string.IsNullOrEmpty(token))
                    {
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        if (authToken != null)
                            return authToken.Email;
                    }

                    throw new Exception("Sorry! this user is not authorized!");
                }
            }
            throw new Exception("Sorry! this user is not authorized!");
        }

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

        public static DatesHelper GetDates(string code, int tenant)
        {
            DatesHelper helper = new DatesHelper();

            DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime? date1 = null;
            DateTime? date2 = null;
            int days = 0;

            if (code.Contains("_1"))
            {
                string str = code.Replace("_1", "");

                days = Convert.ToInt32(str);
            }
            else
            {
                days = Convert.ToInt32(code);
            }

            if (code == "-365_1")
            {
                date1 = new DateTime(todayDate.Value.Year - 1, 1, 1);
                date2 = new DateTime(todayDate.Value.Year - 1, 12, 31);
            }

            else if (code == "-30_1")
            {
                if (todayDate.Value.Month == 1)
                {
                    date1 = new DateTime(todayDate.Value.Year - 1, 12, 1);
                }
                else
                {
                    date1 = new DateTime(todayDate.Value.Year, todayDate.Value.Month - 1, 1);
                }

                date2 = new DateTime(date1.Value.Year, date1.Value.Month, DateTime.DaysInMonth(date1.Value.Year, date1.Value.Month));
            }

            else if (code == "-90_1")
            {
                switch (todayDate.Value.Month)
                {
                    case 1:
                    case 2:
                    case 3:
                        {
                            date1 = new DateTime(todayDate.Value.Year - 1, 10, 1);
                            date2 = new DateTime(todayDate.Value.Year - 1, 12, 31);
                            break;
                        }

                    case 4:
                    case 5:
                    case 6:
                        {
                            date1 = new DateTime(todayDate.Value.Year, 1, 1);
                            date2 = new DateTime(todayDate.Value.Year, 3, 31);
                            break;
                        }

                    case 7:
                    case 8:
                    case 9:
                        {
                            date1 = new DateTime(todayDate.Value.Year, 4, 1);
                            date2 = new DateTime(todayDate.Value.Year, 6, 30);
                            break;
                        }

                    case 10:
                    case 11:
                    case 12:
                        {
                            date1 = new DateTime(todayDate.Value.Year, 7, 1);
                            date2 = new DateTime(todayDate.Value.Year, 9, 30);
                            break;
                        }
                }
            }

            else if (code == "0_1")
            {
                date1 = new DateTime(todayDate.Value.Year, todayDate.Value.Month, 1);
                date2 = new DateTime(todayDate.Value.Year, todayDate.Value.Month, DateTime.DaysInMonth(todayDate.Value.Year, todayDate.Value.Month));
                days = Convert.ToInt32("-30");
            }

            else
            {
                date1 = todayDate.Value.AddDays(days).Date;
                date2 = todayDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
            }

            helper.Date1 = date1;
            helper.Date2 = date2;
            helper.Days = days;

            return helper;
        }
    }

    public class DatesHelper
    {
        public DateTime? Date1 { get; set; }
        public DateTime? Date2 { get; set; }
        public int Days { get; set; }

        public DatesHelper()
        {

        }
    }
}
