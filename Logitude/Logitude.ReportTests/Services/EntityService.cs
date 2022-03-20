using Logitude.ReportTests.Models;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.ReportTests.Services
{
    public class EntityService
    {
        public string GetIdentity(string fieldValue , string fieldName , string entityName)
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFiltersBuilder().WithDefualtValues()
                .Filter1Name(fieldName ?? "SearchFields")
                .Filter1Operator("Equal")
                .Filter1Value(fieldValue).Build();

            return CallGetByFilters(entityName, apiQueryFilters);
        }

        private string CallGetByFilters(string entityName, ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<dynamic>> response = APICaller.CallGetByFilters<IEnumerable<dynamic>>(GetURL(entityName), UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?["Id"];
        }


        private string GetURL(string tableName)
        {
            return tableName + "Views/GetByFilters";
        }
    }
}
