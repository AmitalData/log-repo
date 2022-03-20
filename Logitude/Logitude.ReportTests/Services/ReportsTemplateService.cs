using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.ReportTests.Services
{
    public class ReportsTemplateService
    {
        private readonly ApiQueryFiltersBuilder apiQueryFiltersBuilder;

        public ReportsTemplateService()
        {
            this.apiQueryFiltersBuilder = new ApiQueryFiltersBuilder();
        }

        public string GetDefaultTemplateId(string templateName, string reportId, string defaultTemplateId)
        {
            if (string.IsNullOrEmpty(templateName)) return defaultTemplateId;

            ApiQueryFilters apiQueryFilters = BuildTemplateFilter(templateName, reportId);
            ApiResponse<IEnumerable<dynamic>> response = APICaller.CallGetByFilters<IEnumerable<dynamic>>(Urls.ReportsTemplateViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?["Id"];
        }

        private ApiQueryFilters BuildTemplateFilter(string templateName, string reportId)
        {
            ApiQueryFilters apiQueryFilters = apiQueryFiltersBuilder.WithDefualtValues()
              .Filter2Name("ReportId")
              .Filter2Operator("Equal")
              .Filter2Value(reportId)
              .Filter3Name("Description")
              .Filter3Operator("Equal")
               .Filter3Value(templateName)
              .Build();

            return apiQueryFilters;
        }
    }
}
