using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class TasksWithoutProjectsDataProvider
    {
        public string EmployeeUserId { get; set; }
        public string EmployeeUserName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime? Today_DateTime { get; set; }
        public List<TasksWithoutProjectsData> TasksWithoutProjectsList { get; set; }
    }

    public class TasksWithoutProjectsData
    {
        public string EmployeeName { get; set; }
        public string DayOfWork { get; set; }
        public DateTime? DateOfWork { get; set; }
        public string WINumber { get; set; }
        public string Description { get; set; }
    }
}