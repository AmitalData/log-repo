using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class WorkDaysPerCategoryDataProvider
    {
        public string EmployeeUserId { get; set; }
        public string BudgetId { get; set; }
        public string CategoryId { get; set; }
        public string CustomerId { get; set; }
        public string OwnerId { get; set; }
        public string ProjectId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? Today_DateTime { get; set; }
        public string Total_TotalDaysIncludingInner{ get; set; }
        public string Total_TotalDaysWithoutIncludingInner { get; set; }
        public string Total_TotalGategoryDays { get; set; }
        public List<WorkDaysPerGategoryData> GategoryRecordList { get; set; }
    }

    public class WorkDaysPerGategoryData
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string ProjectName { get; set; }
        public string ProjectId { get; set; }
        public string ProjectNumber { get; set; }
        public string TotalDaysIncludingInner { get; set; }
        public string TotalDaysWithoutIncludingInner { get; set; }
        public string TotalGategoryDays { get; set; }
        public double TotalMinutes { get; set; }
        public double TotalDaysIncludingInnerDouble { get; set; }
        public double TotalDaysWithoutIncludingInnerDouble { get; set; }
        public double TotalGategoryDaysDouble { get; set; }
        public bool IsVisisble { get; set; }
    }
}