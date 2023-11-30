using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class WorkDaysPerProjectDataProvider
    {
        public string EmployeeUserId { get; set; }
        public string BudgetId { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CustomerId { get; set; }
        public string EmployeeName { get; set; }
        public string CustomerName { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string ProjectName { get; set; }
        public string ProjectId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? Today_DateTime { get; set; }
        public string Total_TotalWIWorkedHours { get; set; }
        public string Total_TotalWIWorkedHours_Employee { get; set; }
        public TimeSpan? Total_TotalWIWorkedHours_Employee_Time { get; set; }
        public double? Total_TotalWIWorkedHours_Employee_double { get; set; }
        public List<WorkDaysPerProjectData> DetailedWorkHoursPerProjectList { get; set; }
        public List<WorkDaysPerProjectData> SummarizedWorkHoursPerProjectList { get; set; }
        public List<ProjectsByCategoryGroup> ProjectsByCategoryGroupList { get; set; }
    }

    public class ProjectsByCategoryGroup
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Total { get; set; }
        public List<WorkDaysPerProjectData> ProjectsRecordList { get; set; }
    }

    public class WorkDaysPerProjectData
    {
        public string ProjectName { get; set; }
        public string ProjectNumber { get; set; }
        public string ProjectDescription { get; set; }
        public string CustomerName { get; set; }
        public DateTime? DateOfWork { get; set; }
        public string EmployeeName { get; set; }
        public string OwnerName { get; set; }
        public string TotalWIWorkedDays_Employee { get; set; }
        public string TotalWIWorkedDays { get; set; }
        public double TotalWIWorkedDays_number { get; set; }
        public string WINumber { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ExternalProjectNumber { get; set; }
        public double TotalMinutes { get; set; }
        public TimeSpan? TotalWIWorkedDays_Employee_Time { get; set; }
        public double? TotalWIWorkedDays_Employee_double { get; set; }
    }
}