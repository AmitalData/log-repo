using Logitude.TimeManagement.Data;
using Logitude.TimeManagement.Data.EntityPOCOs;
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
        private IQueryable<TMEmployeeTime> iQueryable_DataTime = null;
        private IQueryable<TMProjectCategory> iQueryable_Categories = null;

        private WorkDaysPerProjectDataProvider myDataProvider;
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
            WorkDaysPerProjectDataProvider myDataProvider = this.LoadDataProvider();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(WorkDaysPerProjectDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, myDataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);

            StreamReader streamReader = new StreamReader(memoryStream);
            string content = streamReader.ReadToEnd();
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }

        private WorkDaysPerProjectDataProvider LoadDataProvider()
        {
            this.myDataProvider = new WorkDaysPerProjectDataProvider()
            {
                SummarizedWorkHoursPerProjectList = new List<WorkDaysPerProjectData>(),
                DetailedWorkHoursPerProjectList = new List<WorkDaysPerProjectData>(),
                ProjectsByCategoryGroupList = new List<ProjectsByCategoryGroup>(),
            };

            this.BuildReportHeader();

            if (this.fromDate != null && this.toDate != null)
            {
                this.iContext = TimeManagementContext.GetContext(tenant);

                this.BuildSourceData();
                this.BuildReportData();
            }

            return myDataProvider;
        }


        private void BuildReportHeader()
        {
            myDataProvider.FromDate = this.fromDate;
            myDataProvider.ToDate = this.toDate;
            myDataProvider.BudgetId = this.budgetId;
            myDataProvider.EmployeeUserId = this.employeeUserId;
            myDataProvider.CategoryId = this.categoryId;
            myDataProvider.CustomerId = this.customerId;
            myDataProvider.OwnerId = this.ownerId;
            myDataProvider.ProjectId = this.projectId;

            if (!string.IsNullOrEmpty(this.customerId))
            {
                Card iCard = CardRepository.GetSingleCard(this.customerId, this.tenant, true);
                if (iCard != null)
                {
                    myDataProvider.CustomerName = iCard.EnglishName;
                }
            }

            if (!string.IsNullOrEmpty(this.employeeUserId))
            {
                Contact contact = ContactRepository.GetSingleContact(employeeUserId, tenant, true);
                if (contact != null)
                {
                    myDataProvider.EmployeeName = contact.EnglishName;
                }
            }
        }
        private void BuildSourceData()
        {
            this.iQueryable_Projects = (from d in iContext.TMProjects where d.Tenant == tenant && d.IsProrated == false select d);
            this.iQueryable_DataTime = (from d in iContext.TMEmployeeTimes where d.Tenant == tenant && d.ProjectId != null select d);
            this.iQueryable_Categories = (from d in iContext.TMProjectCategories where d.Tenant == tenant select d);

            if (!string.IsNullOrEmpty(this.projectId))
            {
                iQueryable_Projects = iQueryable_Projects.Where(d => d.Id == this.projectId);
                iQueryable_DataTime = iQueryable_DataTime.Where(d => d.ProjectId == this.projectId);
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
            List<TMProject> AllProjects = iQueryable_Projects.ToList();
            List<TMProjectCategory> AllCategories = new List<TMProjectCategory>();

            List<string> AllProjectsIds = AllProjects.Select(s => s.Id).ToList();
            List<TMEmployeeTime> AllDataList = (from d in iQueryable_DataTime where AllProjectsIds.Contains(d.ProjectId) select d).ToList();

            var DataGroup = (from d in AllDataList
                             group d by new { d.DateOfWork, d.EmployeeUserId, d.ProjectId, d.WINumber, d.Description } into g
                             select new
                             {
                                 DateOfWork = g.Key.DateOfWork,
                                 EmployeeUserId = g.Key.EmployeeUserId,
                                 ProjectId = g.Key.ProjectId,
                                 WINumber = g.Key.WINumber,
                                 Description = g.Key.Description,
                             });

            if (DataGroup.Count() > 0)
            {
                foreach (var item in DataGroup)
                {
                    WorkDaysPerProjectData SummarizedItem = new WorkDaysPerProjectData();

                    List<TMEmployeeTime> itemGrouplist = AllDataList.Where(d => d.ProjectId == item.ProjectId).ToList();

                    TMProject iProject = AllProjects.Where(d => d.Id == item.ProjectId).FirstOrDefault();
                    if (iProject != null)
                    {
                        SummarizedItem.ProjectName = iProject.Name;
                        SummarizedItem.ProjectNumber = iProject.ProjectNumber;
                        SummarizedItem.Description = iProject.Description;

                        if (!string.IsNullOrEmpty(iProject.CustomerId))
                        {
                            Card iCard = CardRepository.GetSingleCard(iProject.CustomerId, this.tenant, true);
                            if (iCard != null)
                            {
                                SummarizedItem.CustomerName = iCard.EnglishName;
                            }
                        }

                        if (!string.IsNullOrEmpty(iProject.CategoryId))
                        {
                            TMProjectCategory iCategory = AllCategories.Where(d => d.Id == iProject.CategoryId).FirstOrDefault();
                            if (iCategory == null)
                            {
                                iCategory = (from d in this.iQueryable_Categories where d.Id == iProject.CategoryId select d).FirstOrDefault();

                                if (iCategory != null)
                                {
                                    AllCategories.Add(iCategory);
                                }
                            }

                            if (iCategory != null)
                            {
                                SummarizedItem.CategoryId = iCategory.Id;
                                SummarizedItem.CategoryName = iCategory.Name;
                            }
                        }

                        //var wIWorkedDays = Math.Round((itemGrouplist.Sum(a => a.TimeInMinutes)) / 60.0, 2) / 8;
                        //totalWIWorkedDays += wIWorkedDays;
                        //timSheetItem.TotalWIWorkedDays = DateFormat(wIWorkedDays);
                        //timSheetItem.TotalWIWorkedDays_number = wIWorkedDays;

                        this.myDataProvider.SummarizedWorkHoursPerProjectList.Add(SummarizedItem);
                    }
                }

                if (this.myDataProvider.SummarizedWorkHoursPerProjectList.Count > 0)
                {
                    List<ProjectsByCategoryGroup> finalResults = (from p in myDataProvider.SummarizedWorkHoursPerProjectList
                                                                  group p by new { p.CategoryId, p.CategoryName } into g
                                                                  select new ProjectsByCategoryGroup()
                                                                  {
                                                                      CategoryId = g.Key.CategoryId,
                                                                      CategoryName = g.Key.CategoryName,
                                                                      ProjectsRecordList = g.ToList(),
                                                                      //Total = DateFormat((Math.Round(g.Sum(s => s.TotalWIWorkedDays_number), 2))),
                                                                  }).ToList();

                    myDataProvider.ProjectsByCategoryGroupList = finalResults.OrderBy(d => d.CategoryName).ToList();
                }
            }


        }
    }
}