using Logitude.TimeManagement.Data;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.TimeManagement
{
    public class WorkPerDaysProjectManager
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
        private IQueryable<TMEmployeeTime> iQueryable_EmployeeTimes = null;
        private List<TMProjectCategory> AllCategories = null;
        private WorkDaysPerProjectDataProvider iDataProvider;
        public WorkPerDaysProjectManager(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;

            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations iQueryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);

            QueryFilterItem filterItem_ProjectId = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ProjectId").FirstOrDefault();
            QueryFilterItem filterItem_CustomerId = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "CustomerId").FirstOrDefault();
            QueryFilterItem filterItem_OwnerId = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "OwnerId").FirstOrDefault();
            QueryFilterItem filterItem_IncludeInnerProject = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "IncludeInnerProject").FirstOrDefault();
            QueryFilterItem filterItem_EmployeeUserId = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "EmployeeUserId").FirstOrDefault();
            QueryFilterItem filterItem_FromDate = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_BudgetId = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "BudgetId").FirstOrDefault();
            QueryFilterItem filterItem_CategoryId = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "CategoryId").FirstOrDefault();
            QueryFilterItem filterItem_ExternalProjectNumber = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ExternalProjectNumber").FirstOrDefault();

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
            if (filterItem_CustomerId != null)
            {
                if (filterItem_CustomerId.FieldValue != null)
                {
                    customerId = filterItem_CustomerId.FieldValue.ToString();
                }
            }
            if (filterItem_EmployeeUserId != null)
            {
                if (filterItem_EmployeeUserId.FieldValue != null)
                {
                    employeeUserId = filterItem_EmployeeUserId.FieldValue.ToString();
                }
            }
            if (filterItem_BudgetId != null)
            {
                if (filterItem_BudgetId.FieldValue != null)
                {
                    budgetId = filterItem_BudgetId.FieldValue.ToString();
                }
            }
            if (filterItem_CategoryId != null)
            {
                if (filterItem_CategoryId.FieldValue != null)
                {
                    categoryId = filterItem_CategoryId.FieldValue.ToString();
                }
            }
            if (filterItem_ProjectId != null)
            {
                if (filterItem_ProjectId.FieldValue != null)
                {
                    projectId = filterItem_ProjectId.FieldValue.ToString();
                }
            }
            if (filterItem_OwnerId != null)
            {
                if (filterItem_OwnerId.FieldValue != null)
                {
                    ownerId = filterItem_OwnerId.FieldValue.ToString();
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

        public byte[] GetData()
        {
            this.LoadDataProvider();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(WorkDaysPerProjectDataProvider));
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
            this.iDataProvider = new WorkDaysPerProjectDataProvider()
            {
                SummarizedWorkHoursPerProjectList = new List<WorkDaysPerProjectData>(),
                DetailedWorkHoursPerProjectList = new List<WorkDaysPerProjectData>(),
                ProjectsByCategoryGroupList = new List<ProjectsByCategoryGroup>(),
            };

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

            if (!string.IsNullOrEmpty(this.ownerId))
            {
                Contact iContact = ContactRepository.GetSingleContact(this.ownerId, tenant, true);
                if (iContact != null)
                {
                    iDataProvider.OwnerName = iContact.EnglishName;
                }
            }

            if (!string.IsNullOrEmpty(this.employeeUserId))
            {
                Contact iContact = ContactRepository.GetSingleContact(this.employeeUserId, tenant, true);
                if (iContact != null)
                {
                    iDataProvider.EmployeeName = iContact.EnglishName;
                }
            }

            if (!string.IsNullOrEmpty(this.customerId))
            {
                Card iCard = CardRepository.GetSingleCard(this.customerId, this.tenant, true);
                if (iCard != null)
                {
                    iDataProvider.CustomerName = iCard.EnglishName;
                }
            }

            if (!string.IsNullOrEmpty(this.projectId))
            {
                TMProject iProject = (from d in this.iContext.TMProjects where d.Id == this.projectId select d).FirstOrDefault();
                if (iProject != null)
                {
                    iDataProvider.ProjectName = iProject.Name;
                }
            }

            if (!string.IsNullOrEmpty(this.categoryId))
            {
                TMProjectCategory iCategory = this.AllCategories.Where(d => d.Id == this.categoryId).FirstOrDefault();
                if (iCategory != null)
                {
                    iDataProvider.CategoryName = iCategory.Name;
                }
            }
        }
        private void BuildSourceData()
        {
            this.iQueryable_Projects = (from d in iContext.TMProjects where d.Tenant == tenant && d.IsProrated == false select d);
            this.iQueryable_EmployeeTimes = (from d in iContext.TMEmployeeTimes where d.Tenant == tenant select d);
            this.iQueryable_EmployeeTimes = this.iQueryable_EmployeeTimes.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
            this.iQueryable_EmployeeTimes = this.iQueryable_EmployeeTimes.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));

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

            if (!this.IncludeInnerProject)
            {
                iQueryable_Projects = iQueryable_Projects.Where(d => d.IsInnerProject == this.IncludeInnerProject);
            }
        }
        private void BuildReportData()
        {
            this.BuildDetailedWorkHours();
            this.BuildProjectsByCategory();
        }

        private void BuildDetailedWorkHours()
        {
            // https://stackoverflow.com/questions/530925/linq-using-inner-join-group-and-sum           

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


            List<WorkDaysPerProjectData> iList = new List<WorkDaysPerProjectData>();

            foreach (var item in iQueryable_List)
            {
                WorkDaysPerProjectData itemRecord = new WorkDaysPerProjectData()
                {
                    CategoryId = item.CategoryId,
                    ProjectName = item.ProjectName,
                    ProjectNumber = item.ProjectNumber,
                    ProjectDescription = item.ProjectDescription,
                    DateOfWork = new DateTime(item.Year, item.Month, item.Day),
                    WINumber = item.WINumber,
                    Description = item.Description,
                    ExternalProjectNumber = item.ExternalProjectNumber,
                    TotalMinutes = item.FullDuration,
                    TotalWIWorkedDays_Employee = this.GetTimeFormatFromMinutes(item.FullDuration),
                    OwnerName = this.iDataProvider.OwnerName,
                    EmployeeName = this.iDataProvider.EmployeeName,
                    CustomerName = this.iDataProvider.CustomerName,
                    CategoryName = this.iDataProvider.CategoryName,                     
                };

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
                if (string.IsNullOrEmpty(itemRecord.EmployeeName))
                {
                    if (!string.IsNullOrEmpty(item.EmployeeUserId))
                    {
                        Contact iContact = ContactRepository.GetSingleContact(item.EmployeeUserId, this.tenant, true);
                        if (iContact != null)
                        {
                            itemRecord.EmployeeName = iContact.EnglishName;
                        }
                    }
                }
                if (string.IsNullOrEmpty(itemRecord.CustomerName))
                {
                    if (!string.IsNullOrEmpty(item.CustomerId))
                    {
                        Card iCard = CardRepository.GetSingleCard(item.CustomerId, this.tenant, true);
                        if (iCard != null)
                        {
                            itemRecord.CustomerName = iCard.EnglishName;
                        }
                    }
                }
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
                }

                iList.Add(itemRecord);
            }

            this.iDataProvider.DetailedWorkHoursPerProjectList = iList.OrderBy(o => o.DateOfWork).ToList();

            if (this.iDataProvider.DetailedWorkHoursPerProjectList.Count > 0)
            {
                double iTotalMinutes = this.iDataProvider.DetailedWorkHoursPerProjectList.Sum(s => s.TotalMinutes);

                this.iDataProvider.Total_TotalWIWorkedHours_Employee = this.GetTimeFormatFromMinutes(iTotalMinutes);
            }
        }
        private void BuildProjectsByCategory()
        {
            if (this.iDataProvider.DetailedWorkHoursPerProjectList.Count > 0)
            {
                List<ProjectsByCategoryGroup> DataGroups =
                    (from x in this.iDataProvider.DetailedWorkHoursPerProjectList
                     group x by new { x.CategoryId, x.CategoryName } into g
                     select new ProjectsByCategoryGroup()
                     {
                         CategoryId = g.Key.CategoryId,
                         CategoryName = g.Key.CategoryName,
                     }).ToList();

                foreach(ProjectsByCategoryGroup item in DataGroups)
                {
                    List<WorkDaysPerProjectData> lines = this.iDataProvider.DetailedWorkHoursPerProjectList.Where(d => d.CategoryId == item.CategoryId).ToList();

                    item.Total = this.GetDaysFormatFromMinutes(lines.Sum(s => s.TotalMinutes));

                    item.ProjectsRecordList = (from d in lines
                                               group d by new { d.ProjectName, d.ProjectNumber, d.ProjectDescription } into g
                                               select new WorkDaysPerProjectData()
                                               {
                                                   ProjectName = g.Key.ProjectName,
                                                   ProjectNumber = g.Key.ProjectNumber,
                                                   Description = g.Key.ProjectDescription,
                                                   TotalMinutes = g.Sum(s => s.TotalMinutes),
                                                   TotalWIWorkedDays_number = g.Sum(s => s.TotalMinutes),
                                                   TotalWIWorkedDays = this.GetDaysFormatFromMinutes(g.Sum(s => s.TotalMinutes)),
                                               }).ToList();
                }

                this.iDataProvider.ProjectsByCategoryGroupList = DataGroups.OrderBy(d => d.CategoryName).ToList();
            }
        }

        private string GetTimeFormatFromMinutes(double minutes)
        {
            string iResult = "";

            if (minutes != 0)
            {
                TimeSpan iTimeSpan = TimeSpan.FromMinutes(Math.Abs(minutes));

                iResult = (int)iTimeSpan.TotalHours + ":" + iTimeSpan.Minutes.ToString("00");

                if (minutes < 0)
                {
                    iResult = "- " + iResult;
                }
            }

            return iResult;
        }
        private string GetDaysFormatFromMinutes(double minutes)
        {
            string iResult = "";

            if (minutes != 0)
            {
                TimeSpan iTimeSpan = TimeSpan.FromMinutes(Math.Abs(minutes));


                double TotalHours = minutes / 60;

                int iDays = (int)(TotalHours / 8);
                double Hours = TotalHours % 8;
                double iHours = Math.Round(Hours / 8, 2);

                iResult = iDays + ":" + iHours.ToString().Replace("0.", "").PadRight(2, '0');

                if (minutes < 0)
                {
                    iResult = "- " + iResult;
                }
            }

            return iResult;
        }

        private void temp()
        {
            //var iQueryable_List1 = (from EmployeeTimes in iQueryable_EmployeeTimes
            //                        join Projects in iQueryable_Projects on EmployeeTimes.ProjectId equals Projects.Id
            //                        where EmployeeTimes.ProjectId != null && EmployeeTimes.ProjectId != ""
            //                        group EmployeeTimes by new
            //                        {
            //                            EmployeeTimes.DateOfWork.Year,
            //                            EmployeeTimes.DateOfWork.Month,
            //                            EmployeeTimes.DateOfWork.Day,
            //                            EmployeeTimes.EmployeeUserId,
            //                            EmployeeTimes.ProjectId,
            //                            EmployeeTimes.WINumber,
            //                            EmployeeTimes.Description,
            //                            Projects.Name,
            //                            Projects.ProjectNumber,
            //                            Projects.ExternalProjectNumber,
            //                            Projects.CustomerId,
            //                            Projects.OwnerId,
            //                            Projects.CategoryId,
            //                            ProjectDescription = Projects.Description,
            //                        }

            //           into g

            //                        select new
            //                        {
            //                            Year = g.Key.Year,
            //                            Month = g.Key.Month,
            //                            Day = g.Key.Day,
            //                            EmployeeUserId = g.Key.EmployeeUserId,
            //                            WINumber = g.Key.WINumber,
            //                            Description = g.Key.Description,
            //                            ProjectId = g.Key.ProjectId,
            //                            ProjectName = g.Key.Name,
            //                            ProjectNumber = g.Key.ProjectNumber,
            //                            ExternalProjectNumber = g.Key.ExternalProjectNumber,
            //                            TimeInMinutes = g.Sum(s => s.TimeInMinutes),
            //                            FullDuration = g.Sum(s => s.FullDuration),
            //                            CustomerId = g.Key.CustomerId,
            //                            OwnerId = g.Key.OwnerId,
            //                            CategoryId = g.Key.CategoryId,
            //                            ProjectDescription = g.Key.ProjectDescription,
            //                        }).ToList();

            //var iQueryable_List2 = (from EmployeeTimes in iQueryable_EmployeeTimes
            //                        where EmployeeTimes.ProjectId == null || EmployeeTimes.ProjectId == ""
            //                        group EmployeeTimes by new
            //                        {
            //                            EmployeeTimes.DateOfWork.Year,
            //                            EmployeeTimes.DateOfWork.Month,
            //                            EmployeeTimes.DateOfWork.Day,
            //                            EmployeeTimes.EmployeeUserId,
            //                            EmployeeTimes.ProjectId,
            //                            EmployeeTimes.WINumber,
            //                            EmployeeTimes.Description,
            //                        }

            //           into g

            //                        select new
            //                        {
            //                            Year = g.Key.Year,
            //                            Month = g.Key.Month,
            //                            Day = g.Key.Day,
            //                            EmployeeUserId = g.Key.EmployeeUserId,
            //                            WINumber = g.Key.WINumber,
            //                            Description = g.Key.Description,
            //                            ProjectId = g.Key.ProjectId,
            //                            ProjectName = "",
            //                            ProjectNumber = "",
            //                            ExternalProjectNumber = "",
            //                            TimeInMinutes = g.Sum(s => s.TimeInMinutes),
            //                            FullDuration = g.Sum(s => s.FullDuration),
            //                            CustomerId = "",
            //                            OwnerId = "",
            //                            CategoryId = "",
            //                            ProjectDescription = "",
            //                        }).ToList();
        }
    }
}