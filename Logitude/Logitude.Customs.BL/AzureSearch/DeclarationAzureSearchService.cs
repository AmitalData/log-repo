using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Customs.BL.AzureSearch
{
    public class DeclarationAzureSearchService : FastSearchService
    {
        protected override void ManipulateAdditionalFilters(List<QueryFilterItem> additionalFilters, int tenant)
        {
            QueryFilterItem transportModeFilter = additionalFilters.Find(x => x.FieldName == "TransportModeForExport");
            if (transportModeFilter != null)
                transportModeFilter.FieldName = "TransportModeId";

            foreach (QueryFilterItem filter in additionalFilters)
                if (FieldNameMappings.TryGetValue(filter.FieldName, out string mappedName))
                    filter.FieldName = mappedName;
        }

        protected override IQueryable GetCustomFilterQueryable(QueryOperations queryOperations, int tenant)
        {
            ICustomContext context = CustomContext.GetContext(tenant);
            IQueryable<Declaration> iQueryable = context.Declarations.AsQueryable();
            iQueryable = new DeclarationListQueryService(context).ApplyCustomFilters(queryOperations, iQueryable, tenant);
            return iQueryable;
        }

        protected override string ManipulateFilters(List<QueryFilterItem> additionalFilters, string filters, int tenant)
        {
            if (string.IsNullOrEmpty(filters))
                filters = $"tenant eq {tenant}";
            else if (!filters.Contains("tenant eq"))
                filters += $" and tenant eq {tenant}";

            return filters;
        }

        protected override string GetSettingsName(List<QueryFilterItem> additionalFilters, string filters, int tenant) =>
            filters.Contains("(direction eq 'E')") ? "exportDeclarations" : "declarations";

        private static readonly Dictionary<string, string> FieldNameMappings = new Dictionary<string, string>() {
            { "TransportModeForExport", "TransportModeId" }
        };
    }
}