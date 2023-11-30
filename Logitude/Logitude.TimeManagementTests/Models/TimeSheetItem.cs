using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TimeManagementTests.Models
{
    public class TimeSheetItem
    {
        public TimeSheetItem()
        {
            this.Days = new List<TimeSheetItemDay>();
        }

        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string WINumber { get; set; }
        public string LocationCode { get; set; }
        public string EmployeeUserId { get; set; }
        public int? TotalMinutes { get; set; }
        public bool IsHeaderUpdated { get; set; }
        public List<TimeSheetItemDay> Days { get; set; }
        public string ProjectId_db { get; set; }
        public string Description_db { get; set; }
        public string WINumber_db { get; set; }
    }
}
