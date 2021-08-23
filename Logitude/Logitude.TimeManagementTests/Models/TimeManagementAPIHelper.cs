using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TimeManagementTests.Models
{
    public class TimeManagementAPIHelper
    {
        public int Id { get; set; }
        public string LocationCode { get; set; }
        public string EmployeeUserId { get; set; }
        public string TotalFromClock { get; set; }
        public double TotalMinutesFromClock { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<TimeSheetItem> Items { get; set; }
        public List<TMEmployeeTimePM> ItemsPM { get; set; }
        public List<TimeSheetItemDay> OfficeClockDays { get; set; }
    }
}
