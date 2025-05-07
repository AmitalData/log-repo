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
        protected override void MenipulateAdditionalFilters(List<QueryFilterItem> additionalFilters, int tenant)
        {
            QueryFilterItem transportModeFilter = additionalFilters.Find(x => x.FieldName == "TransportModeForExport");
            if (transportModeFilter != null)
                transportModeFilter.FieldName = "TransportModeId";
        }

        protected override IQueryable GetCustomFilterQueryable(QueryOperations queryOperations, int tenant)
        {
            ICustomContext context = CustomContext.GetContext(tenant);
            IQueryable<Declaration> iQueryable = context.Declarations.AsQueryable();
            iQueryable = new DeclarationListQueryService(context).ApplyCustomFilters(queryOperations, iQueryable, tenant);
            return iQueryable;
        }

        protected override string MenipulateFilters(List<QueryFilterItem> additionalFilters, string filters, int tenant)
        {
            if (additionalFilters == null)
                filters = "tenant eq " + tenant;
            else if (additionalFilters.All(x => x.FieldName != "Tenant"))
            {
                if (filters != "")
                    filters += " and ";
                filters += "tenant eq " + tenant;
            }

            return filters;
        }

        protected override string GetSettingsName(List<QueryFilterItem> additionalFilters, string filters, int tenant) =>
            filters.Contains("(direction eq 'E')") ? "exportDeclarations" : "declarations";
    }
}