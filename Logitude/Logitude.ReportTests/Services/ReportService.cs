using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.ReportTests.Services
{
    public class ReportService
    {

        public dynamic GetByCode(string code)
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFiltersBuilder().WithDefualtValues()
            .Filter1Name("Tenant")
            .Filter1Operator("Equal")
            .Filter1Value(UserTenant.Tenant.ToString())
            .Filter2Name("Code")
            .Filter2Operator("Equal")
            .Filter2Value(code)
            .Build();
            ApiResponse<IEnumerable<dynamic>> response = APICaller.CallGetByFilters<IEnumerable<dynamic>>(Urls.ReportViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault();
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
            ApiQueryFilters apiQueryFilters = new ApiQueryFiltersBuilder().WithDefualtValues()
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
