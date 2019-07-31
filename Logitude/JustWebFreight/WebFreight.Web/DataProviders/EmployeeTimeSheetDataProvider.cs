using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class EmployeeTimeSheetDataProvider
    {
        public string EmployeeUserId { get; set; }
        public string EmployeeUserName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime? Today_DateTime { get; set; }
        public List<EmployeeTimeSheetData> EmployeeTimeSheetList { get; set; }
        public List<EmployeeTimeDayOff> EmployeeTimeDaysOff { get; set; }

        public string Total_RequiredWorkHours { get; set; }
        public string Total_TimeFromClock { get; set; }
        public string Total_TimeFromOffice { get; set; }
        public string Total_DifferenceTime { get; set; }
        public string Total_TimeFromHome { get; set; }
        public string Total_TimeFromClient { get; set; }
        public string Total_TimeFromDayOff { get; set; }
        public string Total_TotalWorkHrs { get; set; }
        public string Total_OverTime { get; set; }        
    }

    public class EmployeeTimeSheetData
    {
        public string EmployeeName { get; set; }
        public string DayOfWork { get; set; }
        public DateTime? DateOfWork { get; set; }
        public int RequiredWorkMins { get; set; }
        public string RequiredWorkHours { get; set; }
        public double MinutesFromClock { get; set; }
        public double MinutesFromOffice { get; set; }
        public double MinutesFromHome { get; set; }
        public double MinutesFromClient { get; set; }
        public double MinutesFromDayOff { get; set; }
        public double MinutesDifference { get; set; }
        public double MinutesTotalWork { get; set; }
        public double MinutesOverTime { get; set; }

        public string TimeFromClock { get; set; }
        public string TimeFromOffice { get; set; }
        public string DifferenceTime { get; set; }
        public string TimeFromHome { get; set; }
        public string TimeFromClient { get; set; }
        public string TimeFromDayOff { get; set; }
        public string TotalWorkHrs { get; set; }
        public string OverTime { get; set; }
    }
    public class EmployeeTimeDayOff
    {
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string TimeInHours { get; set; }
        public double TimeInMinutes { get; set; }
    }
}