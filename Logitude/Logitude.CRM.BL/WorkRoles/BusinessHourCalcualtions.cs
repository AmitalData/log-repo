using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.WorkRoles
{
    public class BusinessHourCalcualtions
    {
        public List<BusinessHoursHoliday> holidays { get; set; }
        public BusinessHour businessHour { get; set; }

        public BusinessHourCalcualtions(BusinessHour businessHour)
        {
            holidays = new List<BusinessHoursHoliday>();
            this.businessHour = businessHour;
            BusinessHoursHolidayRepository holidayRep = new BusinessHoursHolidayRepository(businessHour.Tenant);
            holidays = holidayRep.GetHolidaysByBusinessHour(businessHour.Id, businessHour.Tenant);
            fillHolidays(); // Fill Holidays
        }

        #region Business Hour Calcualtions

        private List<string> _holidays { get; set; }
        private const string DateFormatWithYear = "yyyy-MM-dd";
        private const string DateFormatWithoutYear = "MM-dd";

        public int CalculateBusinessHours(DateTime? startDate, DateTime? endDate, bool is724)
        {
            if (is724)
            {
                int totalUsedMinutes = 0;
                if (startDate!= null && (startDate.Value.ToString(DateFormatWithYear).Equals(endDate.Value.ToString(DateFormatWithYear))))
                {
                    if (isHolidayDay(startDate.Value))
                        return 0;

                    TimeSpan span = endDate.Value.Subtract(startDate.Value);
                    totalUsedMinutes = span.Minutes;
                }
                else
                {
                    var endOfDay = AbsoluteEnd(startDate.Value);
                    var startOfDay = AbsoluteStart(endDate.Value);

                    var usedMinutesinEndDate = endDate.Value.Subtract(startOfDay).TotalMinutes;
                    var usedMinutesinStartDate = endOfDay.Subtract(startDate.Value).TotalMinutes;
                    
                    var tempStartDate = startDate.Value.AddDays(1);

                    totalUsedMinutes = Convert.ToInt32(usedMinutesinEndDate + usedMinutesinStartDate);

                    for (DateTime day = tempStartDate.Date; day < endDate.Value.Date; day = day.AddDays(1.0))
                    {
                        bool isHoliday = isHolidayDay(day);
                        if (isHoliday)
                        {
                            day = NextDayAfterHoliday24Hour(day);
                        }

                        var workingHoursInMinutes = 24 * 60;
                        totalUsedMinutes += workingHoursInMinutes;
                    }
                }

                return totalUsedMinutes;
            }

            else
            {
                if (startDate!= null && endDate  != null && (startDate.Value.ToString(DateFormatWithYear).Equals(endDate.Value.ToString(DateFormatWithYear))))
                {
                    if (!isWorkingDay(startDate.Value))
                        return 0;

                    //if (isDateBeforeOpenHours(startDate.Value))
                    //{
                    //    startDate = getStartOfDay(startDate.Value);
                    //}

                    //if (isDateAfterOpenHours(endDate.Value))
                    //{
                    //    endDate = getEndOfDay(endDate.Value);
                    //}

                    var endminutes = (endDate.Value.Hour * 60) + endDate.Value.Minute;
                    var startminutes = (startDate.Value.Hour * 60) + startDate.Value.Minute;

                    return endminutes - startminutes;
                }

                var endOfDay = getEndOfDay(startDate.Value);
                var startOfDay = getStartOfDay(endDate.Value);
                var usedMinutesinEndDate = endDate.Value.Subtract(startOfDay).TotalMinutes;
                var usedMinutesinStartDate = endOfDay.Subtract(startDate.Value).TotalMinutes;
                var tempStartDate = startDate.Value.AddDays(1);

                var totalUsedMinutes = usedMinutesinEndDate + usedMinutesinStartDate;

                for (DateTime day = tempStartDate.Date; day < endDate.Value.Date; day = day.AddDays(1.0))
                {
                    if (isWorkingDay(day))
                    {
                        var workingHoursInMinutes = getTotalMinutes(day) * 60;
                        totalUsedMinutes += workingHoursInMinutes;
                    }
                }

                return Convert.ToInt32(totalUsedMinutes);
            }
        }

        public  DateTime AbsoluteStart(DateTime dateTime)
        {
            return dateTime.Date;
        }

        /// <summary>
        /// Gets the 11:59:59 instance of a DateTime
        /// </summary>
        public  DateTime AbsoluteEnd(DateTime dateTime)
        {
            return AbsoluteStart(dateTime).AddDays(1).AddTicks(-1);
        }

        public double getElapsedMinutes(DateTime? date1, DateTime? date2, bool is247)
        {
            DateTime startDate = date1.Value;
            DateTime endDate = date2.Value;

            int hour = startDate.Hour;
            int minute = startDate.Minute;

            if (hour == 0 && minute == 0)
            {
                startDate = getStartOfDay(startDate);
            }

            hour = endDate.Hour;
            minute = endDate.Minute;
            if (hour == 0 && minute == 0)
            {
                endDate = getEndOfDay(endDate);
            }

            startDate = nextOpenDay(startDate);
            endDate = prevOpenDay(endDate);

            if (startDate > endDate)
                return 0;

            if (startDate.ToString(DateFormatWithYear).Equals(endDate.ToString(DateFormatWithYear)))
            {
                if (!isWorkingDay(startDate) || (is247 && !isHolidayDay(startDate)))
                    return 0;

                if (isDateBeforeOpenHours(startDate))
                {
                    startDate = getStartOfDay(startDate);
                }

                if (isDateAfterOpenHours(endDate))
                {
                    endDate = getEndOfDay(endDate);
                }

                var endminutes = (endDate.Hour * 60) + endDate.Minute;
                var startminutes = (startDate.Hour * 60) + startDate.Minute;

                return endminutes - startminutes;
            }

            var endOfDay = getEndOfDay(startDate);
            var startOfDay = getStartOfDay(endDate);
            var usedMinutesinEndDate = endDate.Subtract(startOfDay).TotalMinutes;
            var usedMinutesinStartDate = endOfDay.Subtract(startDate).TotalMinutes;
            var tempStartDate = startDate.AddDays(1);

            var totalUsedMinutes = usedMinutesinEndDate + usedMinutesinStartDate;

            for (DateTime day = tempStartDate.Date; day < endDate.Date; day = day.AddDays(1.0))
            {
                if (isWorkingDay(day) || (is247 && !isHolidayDay(startDate)))
                {
                    var workingHoursInMinutes = getTotalMinutes(day) * 60;
                    totalUsedMinutes += workingHoursInMinutes;
                }
            }

            return totalUsedMinutes;
        }

      public DateTime prevOpenDay(DateTime endDate)
        {
            if (_holidays.Contains(endDate.ToString(DateFormatWithYear)) || _holidays.Contains(endDate.ToString(DateFormatWithoutYear)))
            {
                return prevOpenDayAfterHoliday(endDate);
            }

            else if (endDate.DayOfWeek == DayOfWeek.Saturday && !businessHour.IsSaturdayEnabeled)
            {
                return prevOpenDayAfterHoliday(endDate);
            }

            else if (endDate.DayOfWeek == DayOfWeek.Sunday && !businessHour.IsSundayEnabeled)
            {
                return prevOpenDayAfterHoliday(endDate);
            }

            else if (endDate.DayOfWeek == DayOfWeek.Monday && !businessHour.IsMondayEnabeled)
            {
                return prevOpenDayAfterHoliday(endDate);
            }

            else if (endDate.DayOfWeek == DayOfWeek.Tuesday && !businessHour.IsTuesdayEnabeled)
            {
                return prevOpenDayAfterHoliday(endDate);
            }

            else if (endDate.DayOfWeek == DayOfWeek.Wednesday && !businessHour.IsWednesdayEnabeled)
            {
                return prevOpenDayAfterHoliday(endDate);
            }

            else if (endDate.DayOfWeek == DayOfWeek.Thursday && !businessHour.IsThursdayEnabeled)
            {
                return prevOpenDayAfterHoliday(endDate);
            }

            else if (endDate.DayOfWeek == DayOfWeek.Friday && !businessHour.IsFridayEnabeled)
            {
                return prevOpenDayAfterHoliday(endDate);
            }

            else if (isDateBeforeOpenHours(endDate))
            {
                return getStartOfDay(endDate);
            }

            else if (isDateAfterOpenHours(endDate))
            {   
                return getEndOfDay(endDate);
            }

            return endDate;
        }

     
        //public double CalculateBusinessHours(DateTime dtStart, DateTime dtEnd)
        //{
        //    int StartingHour = 9;
        //    int EndingHour = 18;

        //    // initialze our return value
        //    double OverAllMinutes = 0.0;

        //    // start time must be less than end time
        //    if (dtStart > dtEnd)
        //    {
        //        return OverAllMinutes;
        //    }

        //    DateTime ctTempEnd = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, 0, 0, 0);
        //    DateTime ctTempStart = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, 0, 0, 0);

        //    // check if startdate and enddate are the same day
        //    bool bSameDay = (ctTempStart == ctTempEnd);

        //    // calculate the business days between the dates
        //    int iBusinessDays = 0;
        //    for (DateTime day = ctTempStart.Date; day < ctTempEnd.Date; day = day.AddDays(1.0))
        //    {
        //        if (isWorkingDay(day))
        //        {
        //            iBusinessDays++;
        //        }
        //    }

        //    // now add the time values to our temp times
        //    TimeSpan CTimeSpan = new TimeSpan(0, dtStart.Hour, dtStart.Minute, 0);
        //    ctTempStart += CTimeSpan;
        //    CTimeSpan = new TimeSpan(0, dtEnd.Hour, dtEnd.Minute, 0);
        //    ctTempEnd += CTimeSpan;


        //    Int32 OverAllSec = 0;

        //    // now sum-up all values
        //    if (bSameDay)
        //    {
        //        if (iBusinessDays != 0)
        //        {
        //            TimeSpan cts = ctMaxTime - ctMinTime;
        //            Int32 dwBusinessDaySeconds = (cts.Days * 24 * 60 * 60) + (cts.Hours * 60 * 60) + (cts.Minutes * 60) + cts.Seconds;
        //            OverAllSec = FirstDaySec + LastDaySec - dwBusinessDaySeconds;
        //        }
        //    }

        //    else
        //    {
        //        if (iBusinessDays > 1)
        //            OverAllSec =
        //            ((iBusinessDays - 2) * 9 * 60 * 60) + FirstDaySec + LastDaySec;
        //    }

        //    OverAllMinutes = OverAllSec / 60;

        //    return OverAllMinutes / 60;
        //}

        public DateTime nextOpenDay(DateTime startDate)
        {
            // If holiday date 
            if (_holidays.Contains(startDate.ToString(DateFormatWithYear)) || _holidays.Contains(startDate.ToString(DateFormatWithoutYear)))
            {
                var day = holidays.Where(a => (a.IsRecurring == false && a.Day == startDate.Day && a.Month == startDate.Month && a.Year == startDate.Year) || (a.Day == startDate.Day && a.Month == startDate.Month && a.IsRecurring == true)).FirstOrDefault();

                if (!day.Inactive)
                {
                    return nextOpenDayAfterHoliday(startDate);
                }

            }
            // else 
            if (startDate.DayOfWeek == DayOfWeek.Saturday && !businessHour.IsSaturdayEnabeled)
            {
                return nextOpenDayAfterHoliday(startDate);
            }

            if (startDate.DayOfWeek == DayOfWeek.Sunday && !businessHour.IsSundayEnabeled)
            {
                return nextOpenDayAfterHoliday(startDate);
            }

            if (startDate.DayOfWeek == DayOfWeek.Monday && !businessHour.IsMondayEnabeled)
            {
                return nextOpenDayAfterHoliday(startDate);
            }

            if (startDate.DayOfWeek == DayOfWeek.Tuesday && !businessHour.IsTuesdayEnabeled)
            {
                return nextOpenDayAfterHoliday(startDate);
            }

            if (startDate.DayOfWeek == DayOfWeek.Wednesday && !businessHour.IsWednesdayEnabeled)
            {
                return nextOpenDayAfterHoliday(startDate);
            }

            if (startDate.DayOfWeek == DayOfWeek.Thursday && !businessHour.IsThursdayEnabeled)
            {
                return nextOpenDayAfterHoliday(startDate);
            }

            if (startDate.DayOfWeek == DayOfWeek.Friday && !businessHour.IsFridayEnabeled)
            {
                return nextOpenDayAfterHoliday(startDate);
            }

            if (isDateBeforeOpenHours(startDate))
            {
                return getStartOfDay(startDate);
            }

            if (isDateAfterOpenHours(startDate))
            {
                var nextDate = startDate.AddDays(1);

                if (_holidays.Contains(startDate.ToString(DateFormatWithYear)) || _holidays.Contains(startDate.ToString(DateFormatWithoutYear)))
                {
                    //var day = holidays.Where(a => a.HolidayName == startDate.ToString(DateFormatWithYear) || a.HolidayName == startDate.ToString(DateFormatWithoutYear)).FirstOrDefault();
                    var day = holidays.Where(a => (a.IsRecurring == false && a.Day == startDate.Day && a.Month == startDate.Month && a.Year == startDate.Year) || (a.Day == startDate.Day && a.Month == startDate.Month && a.IsRecurring == true)).FirstOrDefault();

                    if (!day.Inactive)
                    {
                        return nextOpenDayAfterHoliday(nextDate);
                    }

                }

                return getStartOfDay(nextDate);
            }

            return startDate;
        }

        private bool isWorkingDay(DateTime date)
        {
            bool workingDay = false;

            if (_holidays.Contains(date.ToString(DateFormatWithYear)) || _holidays.Contains(date.ToString(DateFormatWithoutYear)))
            {
                //var day = holidays.Where(a => a.HolidayName == date.ToString(DateFormatWithYear) || a.HolidayName == date.ToString(DateFormatWithoutYear)).FirstOrDefault();
                var day = holidays.Where(a => (a.IsRecurring == false && a.Day == date.Day && a.Month == date.Month && a.Year == date.Year) || (a.Day == date.Day && a.Month == date.Month && a.IsRecurring == true)).FirstOrDefault();

                if (!day.Inactive)
                {
                    workingDay = false;
                }

            }

            if (date.DayOfWeek == DayOfWeek.Saturday && businessHour.IsSaturdayEnabeled)
            {
                workingDay = true;
            }

            if (date.DayOfWeek == DayOfWeek.Sunday && businessHour.IsSundayEnabeled)
            {
                workingDay = true;
            }

            if (date.DayOfWeek == DayOfWeek.Monday && businessHour.IsMondayEnabeled)
            {
                workingDay = true;
            }

            if (date.DayOfWeek == DayOfWeek.Tuesday && businessHour.IsTuesdayEnabeled)
            {
                workingDay = true;
            }

            if (date.DayOfWeek == DayOfWeek.Wednesday && businessHour.IsWednesdayEnabeled)
            {
                workingDay = true;
            }

            if (date.DayOfWeek == DayOfWeek.Thursday && businessHour.IsThursdayEnabeled)
            {
                workingDay = true;
            }

            if (date.DayOfWeek == DayOfWeek.Friday && businessHour.IsFridayEnabeled)
            {
                workingDay = true;
            }

            return workingDay;
        }

        public bool isDateBeforeOpenHours(DateTime startDate)
        {
            bool isStartDate = false;

            if (startDate.DayOfWeek == DayOfWeek.Saturday && businessHour.IsSaturdayEnabeled)
            {
                isStartDate = startDate.Hour < businessHour.SaturdayFromHour.Hours || (startDate.Hour == businessHour.SaturdayFromHour.Hours && startDate.Minute < businessHour.SaturdayFromHour.Minutes);
            }

            else if (startDate.DayOfWeek == DayOfWeek.Sunday && businessHour.IsSundayEnabeled)
            {
                isStartDate = startDate.Hour < businessHour.SundayFromHour.Hours || (startDate.Hour == businessHour.SundayFromHour.Hours && startDate.Minute < businessHour.SundayFromHour.Minutes);
            }

            else if (startDate.DayOfWeek == DayOfWeek.Monday && businessHour.IsMondayEnabeled)
            {
                isStartDate = startDate.Hour < businessHour.MondayFromHour.Hours || (startDate.Hour == businessHour.MondayFromHour.Hours && startDate.Minute < businessHour.MondayFromHour.Minutes);
            }

            else if (startDate.DayOfWeek == DayOfWeek.Tuesday && businessHour.IsTuesdayEnabeled)
            {
                isStartDate = startDate.Hour < businessHour.TuesdayFromHour.Hours || (startDate.Hour == businessHour.TuesdayFromHour.Hours && startDate.Minute < businessHour.TuesdayFromHour.Minutes);
            }

            else if (startDate.DayOfWeek == DayOfWeek.Wednesday && businessHour.IsWednesdayEnabeled)
            {
                isStartDate = startDate.Hour < businessHour.WednesdayFromHour.Hours || (startDate.Hour == businessHour.WednesdayFromHour.Hours && startDate.Minute < businessHour.WednesdayFromHour.Minutes);
            }

            else if (startDate.DayOfWeek == DayOfWeek.Thursday && businessHour.IsThursdayEnabeled)
            {
                isStartDate = startDate.Hour < businessHour.ThursdayFromHour.Hours || (startDate.Hour == businessHour.ThursdayFromHour.Hours && startDate.Minute < businessHour.ThursdayFromHour.Minutes);
            }

            else if (startDate.DayOfWeek == DayOfWeek.Friday && businessHour.IsFridayEnabeled)
            {
                isStartDate = startDate.Hour < businessHour.FridayFromHour.Hours || (startDate.Hour == businessHour.FridayFromHour.Hours && startDate.Minute < businessHour.FridayFromHour.Minutes);
            }

            return isStartDate;
        }

        public bool isDateAfterOpenHours(DateTime startDate)
        {
            bool isStartDate = false;

            if (startDate.DayOfWeek == DayOfWeek.Saturday && businessHour.IsSaturdayEnabeled)
            {
                isStartDate = startDate.Hour > businessHour.SaturdayToHour.Hours || (startDate.Hour == businessHour.SaturdayToHour.Hours && startDate.Minute > businessHour.SaturdayToHour.Minutes);
            }

            else if (startDate.DayOfWeek == DayOfWeek.Sunday && businessHour.IsSundayEnabeled)
            {
                isStartDate = startDate.Hour > businessHour.SundayToHour.Hours || (startDate.Hour == businessHour.SundayToHour.Hours && startDate.Minute > businessHour.SundayToHour.Minutes);
            }

            else if (startDate.DayOfWeek == DayOfWeek.Monday && businessHour.IsMondayEnabeled)
            {
                isStartDate = startDate.Hour > businessHour.MondayToHour.Hours || (startDate.Hour == businessHour.MondayToHour.Hours && startDate.Minute > businessHour.MondayToHour.Minutes);
            }

            else if (startDate.DayOfWeek == DayOfWeek.Tuesday && businessHour.IsTuesdayEnabeled)
            {
                isStartDate = startDate.Hour > businessHour.TuesdayToHour.Hours || (startDate.Hour == businessHour.TuesdayToHour.Hours && startDate.Minute > businessHour.TuesdayToHour.Minutes);
            }

            else if (startDate.DayOfWeek == DayOfWeek.Wednesday && businessHour.IsWednesdayEnabeled)
            {
                isStartDate = startDate.Hour > businessHour.WednesdayToHour.Hours || (startDate.Hour == businessHour.WednesdayToHour.Hours && startDate.Minute > businessHour.WednesdayToHour.Minutes);
            }

            else if (startDate.DayOfWeek == DayOfWeek.Thursday && businessHour.IsThursdayEnabeled)
            {
                isStartDate = startDate.Hour > businessHour.ThursdayToHour.Hours || (startDate.Hour == businessHour.ThursdayToHour.Hours && startDate.Minute > businessHour.ThursdayToHour.Minutes);
            }

            else if (startDate.DayOfWeek == DayOfWeek.Friday && businessHour.IsFridayEnabeled)
            {
                isStartDate = startDate.Hour > businessHour.FridayToHour.Hours || (startDate.Hour == businessHour.FridayToHour.Hours && startDate.Minute > businessHour.FridayToHour.Minutes);
            }

            return isStartDate;
        }

        public DateTime nextOpenDayAfterHoliday(DateTime holiday)
        {
            var nextDay = holiday.AddDays(1);

            if (nextDay.DayOfWeek == DayOfWeek.Saturday && !businessHour.IsSaturdayEnabeled)
            {
                nextDay = nextDay.AddDays(1);
            }

            else if (nextDay.DayOfWeek == DayOfWeek.Sunday && !businessHour.IsSundayEnabeled)
            {
                nextDay = nextDay.AddDays(1);
            }

            else if (nextDay.DayOfWeek == DayOfWeek.Monday && !businessHour.IsMondayEnabeled)
            {
                nextDay = nextDay.AddDays(1);
            }

            else if (nextDay.DayOfWeek == DayOfWeek.Tuesday && !businessHour.IsTuesdayEnabeled)
            {
                nextDay = nextDay.AddDays(1);
            }

            else if (nextDay.DayOfWeek == DayOfWeek.Wednesday && !businessHour.IsWednesdayEnabeled)
            {
                nextDay = nextDay.AddDays(1);
            }

            else if (nextDay.DayOfWeek == DayOfWeek.Thursday && !businessHour.IsThursdayEnabeled)
            {
                nextDay = nextDay.AddDays(1);
            }

            else if (nextDay.DayOfWeek == DayOfWeek.Friday && !businessHour.IsFridayEnabeled)
            {
                nextDay = nextDay.AddDays(1);
            }

            while (_holidays.Contains(nextDay.ToString(DateFormatWithYear)) || _holidays.Contains(nextDay.ToString(DateFormatWithoutYear)))
            {
                //var day = holidays.Where(a => a.HolidayName == nextDay.ToString(DateFormatWithYear) || a.HolidayName == nextDay.ToString(DateFormatWithoutYear)).FirstOrDefault();
                var day = holidays.Where(a => (a.IsRecurring == false && a.Day == nextDay.Day && a.Month == nextDay.Month && a.Year == nextDay.Year) || (a.Day == nextDay.Day && a.Month == nextDay.Month && a.IsRecurring == true)).FirstOrDefault();

                if (!day.Inactive)
                {
                    nextDay = nextDay.AddDays(1);
                }
            }

            return getStartOfDay(nextDay);
        }

        public DateTime prevOpenDayAfterHoliday(DateTime holiday)
        {
            var prevDay = holiday.AddDays(-1);

            if (prevDay.DayOfWeek == DayOfWeek.Saturday && businessHour.IsSaturdayEnabeled)
            {
                prevDay = prevDay.AddDays(-1);
            }

            else if (prevDay.DayOfWeek == DayOfWeek.Sunday && businessHour.IsSundayEnabeled)
            {
                prevDay = prevDay.AddDays(-1);
            }

            else if (prevDay.DayOfWeek == DayOfWeek.Monday && businessHour.IsMondayEnabeled)
            {
                prevDay = prevDay.AddDays(-1);
            }

            else if (prevDay.DayOfWeek == DayOfWeek.Tuesday && businessHour.IsTuesdayEnabeled)
            {
                prevDay = prevDay.AddDays(-1);
            }

            else if (prevDay.DayOfWeek == DayOfWeek.Wednesday && businessHour.IsWednesdayEnabeled)
            {
                prevDay = prevDay.AddDays(-1);
            }

            else if (prevDay.DayOfWeek == DayOfWeek.Thursday && businessHour.IsThursdayEnabeled)
            {
                prevDay = prevDay.AddDays(-1);
            }

            else if (prevDay.DayOfWeek == DayOfWeek.Friday && businessHour.IsFridayEnabeled)
            {
                prevDay = prevDay.AddDays(-1);
            }

            while (_holidays.Contains(prevDay.ToString(DateFormatWithYear)) || _holidays.Contains(prevDay.ToString(DateFormatWithoutYear)))
            {
                //var day = holidays.Where(a => a.HolidayName == prevDay.ToString(DateFormatWithYear) || a.HolidayName == prevDay.ToString(DateFormatWithoutYear)).FirstOrDefault();
                var day = holidays.Where(a => (a.IsRecurring == false && a.Day == prevDay.Day && a.Month == prevDay.Month && a.Year == prevDay.Year) || (a.Day == prevDay.Day && a.Month == prevDay.Month && a.IsRecurring == true)).FirstOrDefault();

                if (!day.Inactive)
                {
                    prevDay = prevDay.AddDays(-1);
                }
            }

            return getEndOfDay(prevDay);
        }

        public DateTime getStartOfDay(DateTime nextDay)
        {
            DateTime startDay;

            if (nextDay.DayOfWeek == DayOfWeek.Saturday)
            {
                startDay = DateTime.Parse(string.Format("{0} {1}:{2}", nextDay.ToString(DateFormatWithYear), businessHour.SaturdayFromHour.Hours, businessHour.SaturdayFromHour.Minutes));
            }

            else if (nextDay.DayOfWeek == DayOfWeek.Sunday)
            {

                startDay = DateTime.Parse(string.Format("{0} {1}:{2}", nextDay.ToString(DateFormatWithYear), businessHour.SundayFromHour.Hours, businessHour.SundayFromHour.Minutes));
            }

            else if (nextDay.DayOfWeek == DayOfWeek.Monday)
            {
                startDay = DateTime.Parse(string.Format("{0} {1}:{2}", nextDay.ToString(DateFormatWithYear), businessHour.MondayFromHour.Hours, businessHour.MondayFromHour.Minutes));
            }

            else if (nextDay.DayOfWeek == DayOfWeek.Tuesday)
            {
                startDay = DateTime.Parse(string.Format("{0} {1}:{2}", nextDay.ToString(DateFormatWithYear), businessHour.TuesdayFromHour.Hours, businessHour.TuesdayFromHour.Minutes));
            }

            else if (nextDay.DayOfWeek == DayOfWeek.Wednesday)
            {
                startDay = DateTime.Parse(string.Format("{0} {1}:{2}", nextDay.ToString(DateFormatWithYear), businessHour.WednesdayFromHour.Hours, businessHour.WednesdayFromHour.Minutes));
            }

            else if (nextDay.DayOfWeek == DayOfWeek.Thursday)
            {
                startDay = DateTime.Parse(string.Format("{0} {1}:{2}", nextDay.ToString(DateFormatWithYear), businessHour.ThursdayFromHour.Hours, businessHour.ThursdayFromHour.Minutes));
            }

            else if (nextDay.DayOfWeek == DayOfWeek.Friday)
            {
                startDay = DateTime.Parse(string.Format("{0} {1}:{2}", nextDay.ToString(DateFormatWithYear), businessHour.FridayFromHour.Hours, businessHour.FridayFromHour.Minutes));
            }

            else
            {
                startDay = DateTime.Today;// not exactly 
            }

            return startDay;
        }

        public DateTime getEndOfDay(DateTime startDate)
        {
            DateTime? endDay = startDate;

            if (startDate.DayOfWeek == DayOfWeek.Saturday)
            {
                endDay = DateTime.Parse(string.Format("{0} {1}:{2}", startDate.ToString(DateFormatWithYear), businessHour.SaturdayToHour.Hours, businessHour.SaturdayToHour.Minutes));
            }

            else if (startDate.DayOfWeek == DayOfWeek.Sunday)
            {
                endDay = DateTime.Parse(string.Format("{0} {1}:{2}", startDate.ToString(DateFormatWithYear), businessHour.SundayToHour.Hours, businessHour.SundayToHour.Minutes));
            }

            else if (startDate.DayOfWeek == DayOfWeek.Monday)
            {
                endDay = DateTime.Parse(string.Format("{0} {1}:{2}", startDate.ToString(DateFormatWithYear), businessHour.MondayToHour.Hours, businessHour.MondayToHour.Minutes));
            }

            else if (startDate.DayOfWeek == DayOfWeek.Tuesday)
            {
                endDay = DateTime.Parse(string.Format("{0} {1}:{2}", startDate.ToString(DateFormatWithYear), businessHour.TuesdayToHour.Hours, businessHour.TuesdayToHour.Minutes));
            }

            else if (startDate.DayOfWeek == DayOfWeek.Wednesday)
            {
                endDay = DateTime.Parse(string.Format("{0} {1}:{2}", startDate.ToString(DateFormatWithYear), businessHour.WednesdayToHour.Hours, businessHour.WednesdayToHour.Minutes));
            }

            else if (startDate.DayOfWeek == DayOfWeek.Thursday)
            {
                endDay = DateTime.Parse(string.Format("{0} {1}:{2}", startDate.ToString(DateFormatWithYear), businessHour.ThursdayToHour.Hours, businessHour.ThursdayToHour.Minutes));
            }

            else if (startDate.DayOfWeek == DayOfWeek.Friday)
            {
                endDay = DateTime.Parse(string.Format("{0} {1}:{2}", startDate.ToString(DateFormatWithYear), businessHour.FridayToHour.Hours, businessHour.FridayToHour.Minutes));
            }

            return endDay.Value;
        }

        public int getTotalMinutes(DateTime date)
        {
            int total = 0;
            if (_holidays.Contains(date.ToString(DateFormatWithYear)) || _holidays.Contains(date.ToString(DateFormatWithoutYear)))
            {
                //var day = holidays.Where(a => a.HolidayName == date.ToString(DateFormatWithYear) || a.HolidayName == date.ToString(DateFormatWithoutYear)).FirstOrDefault();
                var day = holidays.Where(a => (a.IsRecurring == false && a.Day == date.Day && a.Month == date.Month && a.Year == date.Year) || (a.Day == date.Day && a.Month == date.Month && a.IsRecurring == true)).FirstOrDefault();

                if (!day.Inactive)
                {
                    total = 0;
                }
            }

            if (date.DayOfWeek == DayOfWeek.Saturday && businessHour.IsSaturdayEnabeled)
            {
                total = businessHour.SaturdayToHour.Hours - businessHour.SaturdayFromHour.Hours;
            }

            if (date.DayOfWeek == DayOfWeek.Sunday && businessHour.IsSundayEnabeled)
            {
                total = businessHour.SundayToHour.Hours - businessHour.SundayFromHour.Hours;
            }

            if (date.DayOfWeek == DayOfWeek.Monday && businessHour.IsMondayEnabeled)
            {
                total = businessHour.MondayToHour.Hours - businessHour.MondayFromHour.Hours;
            }

            if (date.DayOfWeek == DayOfWeek.Tuesday && businessHour.IsTuesdayEnabeled)
            {
                total = businessHour.TuesdayToHour.Hours - businessHour.TuesdayFromHour.Hours;
            }

            if (date.DayOfWeek == DayOfWeek.Wednesday && businessHour.IsWednesdayEnabeled)
            {
                total = businessHour.WednesdayToHour.Hours - businessHour.WednesdayFromHour.Hours;
            }

            if (date.DayOfWeek == DayOfWeek.Thursday && businessHour.IsThursdayEnabeled)
            {
                total = businessHour.ThursdayToHour.Hours - businessHour.ThursdayFromHour.Hours;
            }

            if (date.DayOfWeek == DayOfWeek.Friday && businessHour.IsFridayEnabeled)
            {
                total = businessHour.FridayToHour.Hours - businessHour.FridayFromHour.Hours;
            }

            return total;
        }

        public void fillHolidays()
        {
            _holidays = new List<string>();
            foreach (BusinessHoursHoliday holiday in holidays)
            {
                if (holiday.Year == null)
                {
                    string date = holiday.Month.ToString("D2") + "-" + holiday.Day.ToString("D2");
                    _holidays.Add(date);
                }

                else
                {
                    string date = holiday.Year.ToString("D2") + "-" + holiday.Month.ToString("D2") + "-" + holiday.Day.ToString("D2");
                    //DateTime dt = Convert.ToDateTime(date); 
                    _holidays.Add(date);
                }
            }
        }

        public bool isWorkingHours(DateTime date)
        {
            bool workingHour = false;

            if (_holidays.Contains(date.ToString(DateFormatWithYear)) || _holidays.Contains(date.ToString(DateFormatWithoutYear)))
            {
                //var day = holidays.Where(a => a.HolidayName == date.ToString(DateFormatWithYear) || a.HolidayName == date.ToString(DateFormatWithoutYear)).FirstOrDefault();
                var day = holidays.Where(a => (a.IsRecurring == false && a.Day == date.Day && a.Month == date.Month && a.Year == date.Year) || (a.Day == date.Day && a.Month == date.Month && a.IsRecurring == true)).FirstOrDefault();
                if (!day.Inactive)
                {
                    workingHour = false;
                }
            }

            if (date.DayOfWeek == DayOfWeek.Saturday && businessHour.IsSaturdayEnabeled)
            {
                if (date.TimeOfDay <= businessHour.SaturdayToHour && date.TimeOfDay >= businessHour.SaturdayFromHour)
                {
                    workingHour = true;
                }
            }

            if (date.DayOfWeek == DayOfWeek.Sunday && businessHour.IsSundayEnabeled)
            {
                if (date.TimeOfDay <= businessHour.SundayToHour && date.TimeOfDay >= businessHour.SundayFromHour)
                {
                    workingHour = true;
                }
            }

            if (date.DayOfWeek == DayOfWeek.Monday && businessHour.IsMondayEnabeled)
            {
                if (date.TimeOfDay <= businessHour.MondayToHour && date.TimeOfDay >= businessHour.MondayFromHour)
                {
                    workingHour = true;
                }
            }

            if (date.DayOfWeek == DayOfWeek.Tuesday && businessHour.IsTuesdayEnabeled)
            {
                if (date.TimeOfDay <= businessHour.TuesdayToHour && date.TimeOfDay >= businessHour.TuesdayFromHour)
                {
                    workingHour = true;
                }
            }

            if (date.DayOfWeek == DayOfWeek.Wednesday && businessHour.IsWednesdayEnabeled)
            {
                if (date.TimeOfDay <= businessHour.WednesdayToHour && date.TimeOfDay >= businessHour.WednesdayFromHour)
                {
                    workingHour = true;
                }
            }

            if (date.DayOfWeek == DayOfWeek.Thursday && businessHour.IsThursdayEnabeled)
            {
                if (date.TimeOfDay <= businessHour.ThursdayToHour && date.TimeOfDay >= businessHour.ThursdayFromHour)
                {
                    workingHour = true;
                }
            }

            if (date.DayOfWeek == DayOfWeek.Friday && businessHour.IsFridayEnabeled)
            {
                if (date.TimeOfDay <= businessHour.FridayToHour && date.TimeOfDay >= businessHour.FridayFromHour)
                {
                    workingHour = true;
                }
            }

            return workingHour;
        }

        public bool isHolidayDay(DateTime date)
        {
            bool isWorkingDay = false;

            if (_holidays.Contains(date.ToString(DateFormatWithYear)) || _holidays.Contains(date.ToString(DateFormatWithoutYear)))
            {
                var day = holidays.Where(a => (a.IsRecurring == false && a.Day == date.Day && a.Month == date.Month && a.Year == date.Year) || (a.Day == date.Day && a.Month == date.Month && a.IsRecurring == true)).FirstOrDefault();

                if (!day.Inactive)
                {
                    isWorkingDay = true;
                }
            }

            return isWorkingDay;
        }

        public DateTime NextDayAfterHoliday24Hour(DateTime holiday)
        {
            DateTime nextDay =holiday.AddDays(1);
            while (_holidays.Contains(nextDay.ToString(DateFormatWithYear)) || _holidays.Contains(nextDay.ToString(DateFormatWithoutYear)))
            {
                var day = holidays.Where(a => (a.IsRecurring == false && a.Day == nextDay.Day && a.Month == nextDay.Month && a.Year == nextDay.Year) || (a.Day == nextDay.Day && a.Month == nextDay.Month && a.IsRecurring == true)).FirstOrDefault();
                if (!day.Inactive)
                {
                    nextDay = nextDay.AddDays(1);
                }
            }

            return nextDay;
        }

        public DateTime addResolveMinutes(DateTime date, int minutes)
        {
            date = nextOpenDay(date);
            var endOfDay = getEndOfDay(date);
            var minutesLeft = (int)endOfDay.Subtract(date).TotalMinutes;

            if (minutesLeft < minutes)
            {
                //DateTime tt = endOfDay.AddMinutes(1);
                date = nextOpenDay(endOfDay.AddMinutes(1));
                date = nextOpenDay(date);
                minutes -= minutesLeft;
            }

            var workingHoursInMinutes = (getTotalMinutes(date)) * 60;

            while (minutes > workingHoursInMinutes)
            {
                date = getStartOfDay(date.AddDays(1));
                date = nextOpenDay(date);
                minutes -= workingHoursInMinutes;
            }

            return date.AddMinutes(minutes);
        }

        #endregion
    }
}
