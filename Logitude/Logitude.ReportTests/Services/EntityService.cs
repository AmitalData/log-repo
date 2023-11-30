using Logitude.Base.Models.Api;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.ReportTests.Services
{
    public class EntityService
    {
        public string GetIdentity(string fieldValue, string fieldName, string entityName)
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFiltersBuilder().WithDefualtValues()
                .Filter1Name(fieldName ?? "SearchFields")
                .Filter1Operator("Equal")
                .Filter1Value(fieldValue).Build();

            return CallGetByFilters(entityName, apiQueryFilters, fieldName);
        }

        private string CallGetByFilters(string entityName, ApiQueryFilters apiQueryFilters, string fieldName)
        {
            ApiResponse<IEnumerable<dynamic>> response = APICaller.CallGetByFilters<IEnumerable<dynamic>>(GetURL(entityName), UserTenant.Token, apiQueryFilters);

            var entity = response.Data?.FirstOrDefault();
            if (entity == null) throw new InvalidOperationException("Invalid " + entityName + " " + fieldName);
            if (entity["Id"] != null) return entity["Id"];
            if (entity["Code"] != null) return entity["Code"];
            throw new InvalidOperationException("Invalid " + entityName + " " + fieldName);
        }


        private string GetURL(string tableName)
        {
            return tableName + "Views/GetByFilters";
        }
    }
}
