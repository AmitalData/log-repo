using Logitude.TimeManagement.Data;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.TimeManagement
{
    public class WorkPerDaysCategoryManager
    {
        private int tenant;
        private DateTime? fromDate = null;
        private DateTime? toDate = null;
        private string customerId = null;
        private string employeeUserId = null;
        private string budgetId = null;
        private string categoryId = null;
        private string projectId = null;
        private string ownerId = null;
        private string externalProjectNumber = null;
        private bool IncludeInnerProject = false;
        private ITimeManagementContext iContext;
        private IQueryable<TMProject> iQueryable_Projects = null;
        private IQueryable<TMProject> iQueryable_AllProjects = null;
        private IQueryable<TMEmployeeTime> iQueryable_EmployeeTimes = null;
        private IQueryable<TMEmployeeTime> iQueryable_AllEmployeeTimes = null;
        private List<WorkDaysPerGategoryData> iWorkDaysPerGategoryDataList = null; 

        private List<TMProjectCategory> AllCategories = null;
        private WorkDaysPerCategoryDataProvider iDataProvider;
        public WorkPerDaysCategoryManager(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;

            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations iQueryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);

            this.FilterByDates(iQueryOperations);
            this.FilterByCustomer(iQueryOperations);
            this.FilterByOwner(iQueryOperations);
            this.FilterByProject(iQueryOperations);
            this.FilterByEmployee(iQueryOperations);
            this.FilterByBudget(iQueryOperations);
            this.FilterByCategory(iQueryOperations);
        }

        private void FilterByCategory(QueryOperations iQueryOperations)
        {
            QueryFilterItem filterItem_CategoryId = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "CategoryId").FirstOrDefault();
            if (filterItem_CategoryId != null)
            {
                if (filterItem_CategoryId.FieldValue != null)
                {
                    categoryId = filterItem_CategoryId.FieldValue.ToString();
                }
            }
        }
        private void FilterByBudget(QueryOperations iQueryOperations)
        {
            QueryFilterItem filterItem_BudgetId = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "BudgetId").FirstOrDefault();
            if (filterItem_BudgetId != null)
            {
                if (filterItem_BudgetId.FieldValue != null)
                {
                    budgetId = filterItem_BudgetId.FieldValue.ToString();
                }
            }
        }
        private void FilterByEmployee(QueryOperations iQueryOperations)
        {
            QueryFilterItem filterItem_EmployeeUserId = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "EmployeeUserId").FirstOrDefault();
            if (filterItem_EmployeeUserId != null)
            {
                if (filterItem_EmployeeUserId.FieldValue != null)
                {
                    employeeUserId = filterItem_EmployeeUserId.FieldValue.ToString();
                }
            }
        }
        private void FilterByCustomer(QueryOperations iQueryOperations)
        {
            QueryFilterItem filterItem_CustomerId = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "CustomerId").FirstOrDefault();
            if (filterItem_CustomerId != null)
            {
                if (filterItem_CustomerId.FieldValue != null)
                {
                    customerId = filterItem_CustomerId.FieldValue.ToString();
                }
            }
        }
        private void FilterByProject(QueryOperations iQueryOperations)
        {
            QueryFilterItem filterItem_IncludeInnerProject = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "IncludeInnerProject").FirstOrDefault();
            QueryFilterItem filterItem_ExternalProjectNumber = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ExternalProjectNumber").FirstOrDefault();
            QueryFilterItem filterItem_ProjectId = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ProjectId").FirstOrDefault();
            if (filterItem_ProjectId != null)
            {
                if (filterItem_ProjectId.FieldValue != null)
                {
                    projectId = filterItem_ProjectId.FieldValue.ToString();
                }
            }
            if (filterItem_ExternalProjectNumber != null)
            {
                if (filterItem_ExternalProjectNumber.FieldValue != null)
                {
                    externalProjectNumber = filterItem_ExternalProjectNumber.FieldValue.ToString();
                }
            }
            if (filterItem_IncludeInnerProject != null)
            {
                if (filterItem_IncludeInnerProject.FieldValue != null)
                {
                    IncludeInnerProject = Convert.ToBoolean(filterItem_IncludeInnerProject.FieldValue);
                }
            }
        }
        private void FilterByOwner(QueryOperations iQueryOperations)
        {
            QueryFilterItem filterItem_OwnerId = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "OwnerId").FirstOrDefault();
            if (filterItem_OwnerId != null)
            {
                if (filterItem_OwnerId.FieldValue != null)
                {
                    ownerId = filterItem_OwnerId.FieldValue.ToString();
                }
            }
        }
        private void FilterByDates(QueryOperations iQueryOperations)
        {
            QueryFilterItem filterItem_FromDate = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            if (filterItem_FromDate != null)
            {
                if (filterItem_FromDate.FieldValue != null)
                {
                    fromDate = (DateTime)filterItem_FromDate.FieldValue;
                }
            }
            if (filterItem_ToDate != null)
            {
                if (filterItem_ToDate.FieldValue != null)
                {
                    toDate = (DateTime)filterItem_ToDate.FieldValue;
                }
            }
        }

        public byte[] GetData()
        {
            this.LoadDataProvider();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(WorkDaysPerCategoryDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, iDataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);

            StreamReader streamReader = new StreamReader(memoryStream);
            string content = streamReader.ReadToEnd();
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }

        private void LoadDataProvider()
        {
            this.iDataProvider = new WorkDaysPerCategoryDataProvider() { GategoryRecordList = new List<WorkDaysPerGategoryData>(), };
            this.iContext = TimeManagementContext.GetContext(tenant);
            this.AllCategories = (from d in iContext.TMProjectCategories where d.Tenant == tenant select d).ToList();
            this.BuildReportHeader();
            if (this.fromDate != null && this.toDate != null)
            {
                this.BuildSourceData();
                this.BuildReportData();
            }
        }

        private void BuildReportHeader()
        {
            iDataProvider.FromDate = this.fromDate;
            iDataProvider.ToDate = this.toDate;
            iDataProvider.BudgetId = this.budgetId;
            iDataProvider.EmployeeUserId = this.employeeUserId;
            iDataProvider.CategoryId = this.categoryId;
            iDataProvider.CustomerId = this.customerId;
            iDataProvider.OwnerId = this.ownerId;
            iDataProvider.ProjectId = this.projectId;
        }

        private void BuildSourceData()
        {
            this.iQueryable_Projects = (from d in iContext.TMProjects where d.Tenant == tenant && d.IsProrated == false select d);
            this.iQueryable_AllProjects = this.iQueryable_Projects;
            this.iQueryable_EmployeeTimes = (from d in iContext.TMEmployeeTimes where d.Tenant == tenant select d);
            this.iQueryable_EmployeeTimes = this.iQueryable_EmployeeTimes.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
            this.iQueryable_EmployeeTimes = this.iQueryable_EmployeeTimes.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
            this.iQueryable_AllEmployeeTimes = this.iQueryable_EmployeeTimes;
            if (!string.IsNullOrEmpty(this.projectId))
            {
                iQueryable_Projects = iQueryable_Projects.Where(d => d.Id == this.projectId);
                iQueryable_EmployeeTimes = iQueryable_EmployeeTimes.Where(d => d.ProjectId == this.projectId);
            }

            if (!string.IsNullOrEmpty(this.employeeUserId))
            {
                iQueryable_EmployeeTimes = iQueryable_EmployeeTimes.Where(d => d.EmployeeUserId == this.employeeUserId);
            }

            if (!string.IsNullOrEmpty(this.ownerId))
            {
                iQueryable_Projects = iQueryable_Projects.Where(d => d.OwnerId == this.ownerId);
            }

            if (!string.IsNullOrEmpty(this.customerId))
            {
                iQueryable_Projects = iQueryable_Projects.Where(d => d.CustomerId == this.customerId);
            }

            if (!string.IsNullOrEmpty(this.budgetId))
            {
                iQueryable_Projects = iQueryable_Projects.Where(d => d.BudgetId == this.budgetId);
            }

            if (!string.IsNullOrEmpty(this.categoryId))
            {
                iQueryable_Projects = iQueryable_Projects.Where(d => d.CategoryId == this.categoryId);
            }

            if (!string.IsNullOrEmpty(this.externalProjectNumber))
            {
                iQueryable_Projects = iQueryable_Projects.Where(d => d.ExternalProjectNumber == this.externalProjectNumber);
            }
        }

        private WorkDaysPerGategoryData itemRecord = null;
        private void BuildReportData()
        {

            var iQueryable_List = (
                                   (from EmployeeTimes in iQueryable_EmployeeTimes
                                    join Projects in iQueryable_Projects on EmployeeTimes.ProjectId equals Projects.Id
                                    where EmployeeTimes.ProjectId != null && EmployeeTimes.ProjectId != ""
                                    group EmployeeTimes by new
                                    {
                                        EmployeeTimes.DateOfWork.Year,
                                        EmployeeTimes.DateOfWork.Month,
                                        EmployeeTimes.DateOfWork.Day,
                                        EmployeeTimes.EmployeeUserId,
                                        EmployeeTimes.ProjectId,
                                        EmployeeTimes.WINumber,
                                        EmployeeTimes.Description,
                                        Projects.Name,
                                        Projects.ProjectNumber,
                                        Projects.ExternalProjectNumber,
                                        Projects.CustomerId,
                                        Projects.OwnerId,
                                        Projects.CategoryId,
                                        ProjectDescription = Projects.Description,
                                    } into g

                                    select new
                                    {
                                        Year = g.Key.Year,
                                        Month = g.Key.Month,
                                        Day = g.Key.Day,
                                        EmployeeUserId = g.Key.EmployeeUserId,
                                        WINumber = g.Key.WINumber,
                                        Description = g.Key.Description,
                                        ProjectId = g.Key.ProjectId,
                                        ProjectName = g.Key.Name,
                                        ProjectNumber = g.Key.ProjectNumber,
                                        ExternalProjectNumber = g.Key.ExternalProjectNumber,
                                        TimeInMinutes = g.Sum(s => s.TimeInMinutes),
                                        FullDuration = g.Sum(s => s.FullDuration),
                                        CustomerId = g.Key.CustomerId,
                                        OwnerId = g.Key.OwnerId,
                                        CategoryId = g.Key.CategoryId,
                                        ProjectDescription = g.Key.ProjectDescription,
                                    })

                                   .Union

                                   (from EmployeeTimes in iQueryable_EmployeeTimes
                                    where EmployeeTimes.ProjectId == null || EmployeeTimes.ProjectId == ""
                                    group EmployeeTimes by new
                                    {
                                        EmployeeTimes.DateOfWork.Year,
                                        EmployeeTimes.DateOfWork.Month,
                                        EmployeeTimes.DateOfWork.Day,
                                        EmployeeTimes.EmployeeUserId,
                                        EmployeeTimes.ProjectId,
                                        EmployeeTimes.WINumber,
                                        EmployeeTimes.Description,
                                    } into g
                                    select new
                                    {
                                        Year = g.Key.Year,
                                        Month = g.Key.Month,
                                        Day = g.Key.Day,
                                        EmployeeUserId = g.Key.EmployeeUserId,
                                        WINumber = g.Key.WINumber,
                                        Description = g.Key.Description,
                                        ProjectId = g.Key.ProjectId,
                                        ProjectName = "",
                                        ProjectNumber = "",
                                        ExternalProjectNumber = "",
                                        TimeInMinutes = g.Sum(s => s.TimeInMinutes),
                                        FullDuration = g.Sum(s => s.FullDuration),
                                        CustomerId = "",
                                        OwnerId = "",
                                        CategoryId = "",
                                        ProjectDescription = "",
                                    })
                                    ).ToList();

            iWorkDaysPerGategoryDataList = new List<WorkDaysPerGategoryData>();
            List<WorkDaysPerGategoryData> dataGroups = (from x in iQueryable_List
                                                        group x by new { x.CategoryId, x.ProjectId, x.OwnerId, x.ProjectNumber } into g
                                                        select new WorkDaysPerGategoryData()
                                                        {
                                                            CategoryId = g.Key.CategoryId,
                                                            ProjectId = g.Key.ProjectId,
                                                            ProjectNumber = g.Key.ProjectNumber,
                                                            OwnerId = g.Key.OwnerId,
                                                            TotalMinutes = g.Sum(a => a.FullDuration),
                                                        }).ToList();

            foreach (var item in dataGroups)
            {
                List<WorkDaysPerGategoryData> gategoryLines = dataGroups.Where(d => d.CategoryId == item.CategoryId).ToList();
                itemRecord = new WorkDaysPerGategoryData()
                {
                    CategoryId = item.CategoryId,
                    ProjectId = item.ProjectId,
                    OwnerId = item.OwnerId,
                    TotalDaysWithoutIncludingInnerDouble = item.TotalMinutes,
                    IsVisisble = true,
                };
                this.CalculateCategoryTotals(item, gategoryLines);
                this.FillProjectData(item);
                this.FillCategoryData(item);
                this.FillOwnerData(item);
                iWorkDaysPerGategoryDataList.Add(itemRecord);
            }
            this.iDataProvider.GategoryRecordList = iWorkDaysPerGategoryDataList.Where(a=>a.IsVisisble).OrderBy(a => a.CategoryName).ToList();
            if (this.iDataProvider.GategoryRecordList.Count > 0)
            {
                this.CalculateAllTotalsOfCategoryFields();

            }
        }

        private void CalculateAllTotalsOfCategoryFields()
        {
            double iTotalDaysIncludingInnerDouble = this.iDataProvider.GategoryRecordList.Sum(s => s.TotalDaysIncludingInnerDouble);
            double iTotalDaysWithoutIncludingInnerDouble = this.iDataProvider.GategoryRecordList.Sum(s => s.TotalDaysWithoutIncludingInnerDouble);
            double iTotalGategoryDaysDouble = this.iDataProvider.GategoryRecordList.Sum(s => s.TotalGategoryDaysDouble);
            this.iDataProvider.Total_TotalDaysIncludingInner = this.GetDaysFormatFromMinutes(iTotalDaysIncludingInnerDouble);
            this.iDataProvider.Total_TotalDaysWithoutIncludingInner = this.GetDaysFormatFromMinutes(iTotalDaysWithoutIncludingInnerDouble);
            this.iDataProvider.Total_TotalGategoryDays = this.GetDaysFormatFromMinutes(iTotalGategoryDaysDouble);
        }

        private void FillOwnerData(WorkDaysPerGategoryData item)
        {
            if (string.IsNullOrEmpty(itemRecord.OwnerName))
            {
                if (!string.IsNullOrEmpty(item.OwnerId))
                {
                    Contact iContact = ContactRepository.GetSingleContact(item.OwnerId, this.tenant, true);
                    if (iContact != null)
                    {
                        itemRecord.OwnerName = iContact.EnglishName;
                    }
                }
            }
        }

        private void FillCategoryData(WorkDaysPerGategoryData item)
        {
            if (string.IsNullOrEmpty(itemRecord.CategoryName))
            {
                if (!string.IsNullOrEmpty(item.CategoryId))
                {
                    TMProjectCategory iCategory = this.AllCategories.Where(d => d.Id == item.CategoryId).FirstOrDefault();
                    if (iCategory != null)
                    {
                        itemRecord.CategoryName = iCategory.Name;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(item.ProjectId))
                    {
                        itemRecord.CategoryName = "Not Connected to Projects";
                    }
                }
            }
        }

        private void FillProjectData(WorkDaysPerGategoryData item)
        {
            if (string.IsNullOrEmpty(itemRecord.ProjectName))
            {
                if (!string.IsNullOrEmpty(item.ProjectId))
                {
                    TMProject iProject = this.iQueryable_AllProjects.Where(a => a.Id == item.ProjectId).FirstOrDefault();
                    if (iProject != null)
                    {
                        itemRecord.ProjectName = iProject.Name;

                        if (!this.IncludeInnerProject)
                        {
                            itemRecord.IsVisisble = iProject.IsInnerProject ? false : true;
                        }
                    }
                }
            }
        }

        private void CalculateCategoryTotals(WorkDaysPerGategoryData item, List<WorkDaysPerGategoryData> gategoryLines)
        {
            var listOfCategoryInnerProjects = this.iQueryable_AllProjects.Where(d => d.ProjectNumber.StartsWith(item.ProjectNumber + "-") || d.ProjectNumber == item.ProjectNumber);
            var daysOfListCategoryInnerProjects = (from EmployeeTimes in iQueryable_AllEmployeeTimes
                                                  join Projects in listOfCategoryInnerProjects on EmployeeTimes.ProjectId equals Projects.Id
                                                  where EmployeeTimes.ProjectId != null && EmployeeTimes.ProjectId != ""
                                                  select new
                                                  {
                                                      TotalMinutes = EmployeeTimes.FullDuration,
                                                  }).ToList();

            itemRecord.TotalDaysWithoutIncludingInner = this.GetDaysFormatFromMinutes(item.TotalMinutes);

            if (daysOfListCategoryInnerProjects != null)
            {
                itemRecord.TotalDaysIncludingInnerDouble = daysOfListCategoryInnerProjects.Sum(s => s.TotalMinutes);
            }

            itemRecord.TotalDaysIncludingInner = this.GetDaysFormatFromMinutes(itemRecord.TotalDaysIncludingInnerDouble);

            var isCategoryFirstRow = iWorkDaysPerGategoryDataList.Where(a => a.CategoryId == item.CategoryId && a.TotalGategoryDays != null).FirstOrDefault();
            if (isCategoryFirstRow == null)
            {
                itemRecord.TotalGategoryDaysDouble = gategoryLines.Sum(s => s.TotalMinutes);
                itemRecord.TotalGategoryDays = this.GetDaysFormatFromMinutes(itemRecord.TotalGategoryDaysDouble);
            }
        }
        private string GetDaysFormatFromMinutes(double minutes)
        {
            string iResult = "";

            if (minutes != 0)
            {
                TimeSpan iTimeSpan = TimeSpan.FromMinutes(Math.Abs(minutes));


                double TotalHours = minutes / 60;

                int iDays = (int)(TotalHours / 9);
                double Hours = TotalHours % 9;
                double iHours = Math.Round(Hours / 9, 2);

                iResult = iDays + ":" + iHours.ToString().Replace("0.", "").PadRight(2, '0');

                if (minutes < 0)
                {
                    iResult = "- " + iResult;
                }
            }

            return iResult;
        }

    }
}