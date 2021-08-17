using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.PartnersPreparation;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using Logitude.TimeManagementTests.Models;
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
            string projectId = GetProjectIdFromUserTenant(apiQueryFilters);
            if (string.IsNullOrEmpty(projectId))
            {
                projectId = GetCreatedProjectFromTenantZero(name, ownerId);
            }
            return projectId;
        }


        private string GetProjectIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<TMProjectPM>> response = APICaller.CallGetByFilters<IEnumerable<TMProjectPM>>(Urls.TMProjectViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private string GetCreatedProjectFromTenantZero(string name, string ownerId)
        {
            TMProjectPM tMProject = CreateProject(name, ownerId);
            ApiResponse<TMProjectPM> response = APICaller.CallPost<TMProjectPM>(tMProject, Urls.TmprojectsController, UserTenant.Token);
            return response.Data?.Id;
        }

        private TMProjectPM CreateProject(string name, string ownerId)
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
            string projectId = GetBudgetIdFromUserTenant(apiQueryFilters);
            if (string.IsNullOrEmpty(projectId))
            {
                projectId = GetCreatedBudgetFromTenantZero(name);
            }
            return projectId;
        }

    
        private string GetBudgetIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<BudgetPM>> response = APICaller.CallGetByFilters<IEnumerable<BudgetPM>>(Urls.TMBudgetViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private string GetCreatedBudgetFromTenantZero(string name)
        {
            BudgetPM budget = CreateBudget(name);
            ApiResponse<BudgetPM> response = APICaller.CallPost<BudgetPM>(budget, Urls.TMBudgetsController, UserTenant.Token);
            return response.Data?.Id;
        }

        private BudgetPM CreateBudget(string name)
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
            string categoryId = GetCategoryIdFromUserTenant(apiQueryFilters);
            if (string.IsNullOrEmpty(categoryId))
            {
                categoryId = GetCreatedCategoryFromTenantZero(name);
            }
            return categoryId;
        }


        private string GetCategoryIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<TMProjectCategoryPM>> response = APICaller.CallGetByFilters<IEnumerable<TMProjectCategoryPM>>(Urls.TmprojectcategoryViewsByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private string GetCreatedCategoryFromTenantZero(string name)
        {
            TMProjectCategoryPM category = CreateCategory(name);
            ApiResponse<TMProjectCategoryPM> response = APICaller.CallPost<TMProjectCategoryPM>(category, Urls.TmprojectcategoriesController, UserTenant.Token);
            return response.Data?.Id;
        }

        private TMProjectCategoryPM CreateCategory(string name)
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
            string sprintId = GetSprintIdFromUserTenant(apiQueryFilters);
            if (string.IsNullOrEmpty(sprintId))
            {
                sprintId = GetCreatedSprintFromTenantZero(name);
            }
            return sprintId;
        }


        private string GetSprintIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<SprintPM>> response = APICaller.CallGetByFilters<IEnumerable<SprintPM>>(Urls.SprintViewsByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private string GetCreatedSprintFromTenantZero(string name)
        {
            SprintPM sprint = CreateSprint(name);
            ApiResponse<SprintPM> response = APICaller.CallPost<SprintPM>(sprint, Urls.SprintsController, UserTenant.Token);
            return response.Data?.Id;
        }

        private SprintPM CreateSprint(string name)
        {
            return new SprintPM
            {
                Name = name,
                Tenant = UserTenant.Tenant,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now.AddYears(1),
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

    }
}
