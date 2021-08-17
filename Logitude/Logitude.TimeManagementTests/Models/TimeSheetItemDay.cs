using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TimeManagementTests.Models
{
    public class TimeSheetItemDay
    {
        public int Index { get; set; }
        public DateTime Date { get; set; }
        public int? Minuts { get; set; }
        public int? Minuts_db { get; set; }
        public double? TotalFromClock { get; set; }
        public string TotalFromClockString { get; set; }
        public double MinutesFromClock { get; set; }
    }
}
