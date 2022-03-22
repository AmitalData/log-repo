using Logitude.Base.Models.Api;
using Logitude.Base.Models.Partners;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using Logitude.TimeManagementTests.Models;
using Logitude.TimeManagementTests.Models.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TimeManagementTests.Services
{
    public class TimeManagementDataPreparation
    {
        public void Prepar()
        {
            TimeManagementData.ProjectId = GetProjectId("specflow project", UserTenant.UserId);
            TimeManagementData.SprintId = GetSprintId("specflow sprint");
        }

        #region project
        private string GetProjectId(string name, string ownerId)
        {
            ApiQueryFilters apiQueryFilters = BuildProjectApiQueryFilters(name, ownerId);
            string projectId = GetProjectId(apiQueryFilters);
            if (string.IsNullOrEmpty(projectId))
            {
                projectId = CreateProject(name, ownerId);
            }
            return projectId;
        }


        private string GetProjectId(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<TMProjectPM>> response = APICaller.CallGetByFilters<IEnumerable<TMProjectPM>>(Urls.TMProjectViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private string CreateProject(string name, string ownerId)
        {
            TMProjectPM tMProject = GetNewProjectInstance(name, ownerId);
            ApiResponse<TMProjectPM> response = APICaller.CallPost<TMProjectPM>(tMProject, Urls.TmprojectsController, UserTenant.Token);
            return response.Data?.Id;
        }

        private TMProjectPM GetNewProjectInstance(string name, string ownerId)
        {
            return new TMProjectPM
            {
                CreateDate = DateTime.Now,
                CreatedByUserId = UserTenant.UserId,
                CustomerId = PartnersData.CustomerId,
                Name = name,
                OwnerId = ownerId,
                Tenant = UserTenant.Tenant,
                UpdateDate = DateTime.Now,
                UpdatedByUserId = UserTenant.UserId,
                BudgetId = GetBudgetId("specflowtest"),
                CategoryId = GetCategoryId("specflowtest"),

            };

        }

        private ApiQueryFilters BuildProjectApiQueryFilters(string name, string ownerId)
        {
            return new ApiQueryFiltersBuilder().WithDefualtValues()
                .Filter1Name("Name")
                .Filter1Operator("equals")
                .Filter1Value(name)
                .Filter2Name("OwnerId")
                .Filter2Operator("equals")
                .Filter2Value(ownerId)
                .Build();
        }

        #endregion

        #region Budget
        private string GetBudgetId(string name)
        {
            ApiQueryFilters apiQueryFilters = BuildBudgetApiQueryFilters(name);
            string projectId = GetBudgetId(apiQueryFilters);
            if (string.IsNullOrEmpty(projectId))
            {
                projectId = CreateBudget(name);
            }
            return projectId;
        }

        private string GetBudgetId(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<BudgetPM>> response = APICaller.CallGetByFilters<IEnumerable<BudgetPM>>(Urls.TMBudgetViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private string CreateBudget(string name)
        {
            BudgetPM budget = GetNewBudgetInstance(name);
            ApiResponse<BudgetPM> response = APICaller.CallPost<BudgetPM>(budget, Urls.TMBudgetsController, UserTenant.Token);
            return response.Data?.Id;
        }

        private BudgetPM GetNewBudgetInstance(string name)
        {
            return new BudgetPM
            {
                Name = name,
                Tenant = UserTenant.Tenant
            };
        }

        private ApiQueryFilters BuildBudgetApiQueryFilters(string name)
        {
            return new ApiQueryFiltersBuilder().WithDefualtValues()
                   .Filter1Name("Name")
                   .Filter1Operator("equals")
                   .Filter1Value(name)
                   .Build();
        }

        #endregion

        #region Category
        private string GetCategoryId(string name)
        {
            ApiQueryFilters apiQueryFilters = BuildCategoryApiQueryFilters(name);
            string categoryId = GetCategoryId(apiQueryFilters);
            if (string.IsNullOrEmpty(categoryId))
            {
                categoryId = CreateCategory(name);
            }
            return categoryId;
        }

        private string GetCategoryId(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<TMProjectCategoryPM>> response = APICaller.CallGetByFilters<IEnumerable<TMProjectCategoryPM>>(Urls.TmprojectcategoryViewsByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private string CreateCategory(string name)
        {
            TMProjectCategoryPM category = GetNewCategoryInstance(name);
            ApiResponse<TMProjectCategoryPM> response = APICaller.CallPost<TMProjectCategoryPM>(category, Urls.TmprojectcategoriesController, UserTenant.Token);
            return response.Data?.Id;
        }

        private TMProjectCategoryPM GetNewCategoryInstance(string name)
        {
            return new TMProjectCategoryPM
            {
                Name = name,
                Tenant = UserTenant.Tenant
            };
        }

        private ApiQueryFilters BuildCategoryApiQueryFilters(string name)
        {
            return new ApiQueryFiltersBuilder().WithDefualtValues()
                   .Filter1Name("Name")
                   .Filter1Operator("equals")
                   .Filter1Value(name)
                   .Build();
        }

        #endregion

        #region Sprint
        private string GetSprintId(string name)
        {
            ApiQueryFilters apiQueryFilters = BuildSprintApiQueryFilters(name);
            string sprintId = GetSprintId(apiQueryFilters);
            if (string.IsNullOrEmpty(sprintId))
            {
                sprintId = CreateSprint(name);
            }
            return sprintId;
        }

        private string GetSprintId(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<SprintPM>> response = APICaller.CallGetByFilters<IEnumerable<SprintPM>>(Urls.SprintViewsByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private string CreateSprint(string name)
        {
            SprintPM sprint = GetNewSprintInstance(name);
            ApiResponse<SprintPM> response = APICaller.CallPost<SprintPM>(sprint, Urls.SprintsController, UserTenant.Token);
            return response.Data?.Id;
        }

        private SprintPM GetNewSprintInstance(string name)
        {
            return new SprintPM
            {
                Name = name,
                Tenant = UserTenant.Tenant,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now.AddDays(14),
                UpdatedByUserId = UserTenant.UserId,
                CreatedByUserId = UserTenant.UserId,
            };
        }

        private ApiQueryFilters BuildSprintApiQueryFilters(string name)
        {
            return new ApiQueryFiltersBuilder().WithDefualtValues()
                   .Filter1Name("Name")
                   .Filter1Operator("equals")
                   .Filter1Value(name)
                   .Build();
        }

        #endregion

        public void PreparForUpdate()
        {
            CreateDataEntry();
            TimeManagementData.UpdateProjectId = GetProjectId("specflow project update", UserTenant.UserId);
            TimeManagementData.UpdateSprintId = GetSprintId("specflow sprint update");
            TimeManagementData.UpdatedDateNumber = -1;
        }
        private void CreateDataEntry()
        {
            var dataEntry = CreateInstance();
            var wINumber = dataEntry.ItemsPM.First().WINumber;
            var updatedDataEntry = APICaller.CallPut<TimeManagementAPIHelper>(dataEntry, Urls.TimeManagementDomainController, UserTenant.Token)?.Data;
            var item = updatedDataEntry.ItemsPM.Where(e => e.WINumber == wINumber).First();
            TimeManagementData.DataEntryID = item.Id;
        }
        private TimeManagementAPIHelper CreateInstance()
        {
            return new TimeManagementAPIHelperBuilder()
                .WithDefualtValues()
                .LocationCode("Office")
                .ItemsPM(GetTMEmployeeTime())
                .Build();
        }
        private TMEmployeeTimePM GetTMEmployeeTime()
        {
            return new TMEmployeeTimePMBuilder().WithDefualtValues()
                .LocationCode("Office")
                .Description("Description")
                .TimeInMinutes(200)
                .DateOfWork(DateTime.Now)
                .WINumber(DateTime.Now.Ticks.ToString())
                .Build();
        }
    }
}
