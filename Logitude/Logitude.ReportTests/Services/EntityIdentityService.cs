using Logitude.ReportTests.Models;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.ReportTests.Services
{
    public class EntityIdentityService
    {
        public string GetIdentity(EntityIdentifier entityIdentifier)
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFiltersBuilder().WithDefualtValues()
                .Filter1Name(entityIdentifier.From ?? "SearchFields")
                .Filter1Operator("Equal")
                .Filter1Value(entityIdentifier.Value).Build();

            ApiResponse<IEnumerable<dynamic>> response = APICaller.CallGetByFilters<IEnumerable<dynamic>>(GetURL(entityIdentifier.Table), UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?["Id"];
        }

        private string GetURL(string tableName)
        {
            return tableName + "Views/GetByFilters";
        }
    }
}
