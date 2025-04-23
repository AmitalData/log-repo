using Logitude.Customs.Data;
using Logitude.Customs.Data.AzureSearch.Entities;
using Logitude.Customs.Data.AzureSearch.Repo;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityPOCOs;
using NetCommonHelper.Logger;
using Newtonsoft.Json;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.AzureSearch
{
    public static class DeclarationAzureSearchService
    {
        private static readonly DevLog logger = DevLog.Instance;

        public static async Task<List<DeclarationASEntity>> Search(ApiQueryFilters apiQueryFilters, int tenant, string searchText)
        {
            logger.WriteTrace($"Search declaration ASAI: {apiQueryFilters?.AdditionalFilters}, tenant: {tenant}, searchText: {searchText}");

            string filters = "";

            if (tenant == null)
                throw new ArgumentNullException(nameof(tenant), "tenant is null");

            if (apiQueryFilters?.AdditionalFilters != null)
            {
                List<QueryFilterItem> additionalFilters = JsonConvert.DeserializeObject<List<QueryFilterItem>>(apiQueryFilters.AdditionalFilters);

                QueryFilterItem transportModeFilter = additionalFilters.Find(x => x.FieldName == "TransportModeForExport");
                if (transportModeFilter != null)
                    transportModeFilter.FieldName = "TransportModeId";

                List<QueryFilterItem> simpleFilters = additionalFilters.Where(x => x.IsCustom == false).ToList();
                filters = FilterHelper.ConvertQueryFilter(simpleFilters);

                string customFilters = ConvertCustomFilter(additionalFilters, tenant);
                if (!string.IsNullOrEmpty(customFilters) && !string.IsNullOrEmpty(filters))
                    filters += " and ";
                filters += customFilters;

                if (additionalFilters.All(x => x.FieldName != "Tenant"))
                {
                    if (filters != "")
                        filters += " and ";
                    filters += "tenant eq " + tenant;
                }
            }
            else
                filters = "tenant eq " + tenant;

            string indexSettingsName = filters.Contains("(direction eq 'E')") ?  "exportDeclarations" : "declarations";
            dynamic settings = ASHelper.GetIndexSettings(tenant, indexSettingsName);
            
            List<DeclarationASEntity> declarationList = await new DeclarationAzureSearchRepo(FilterHelper.serviceName, FilterHelper.apiKey).SearchAsync(filters, searchText, (int)settings.maxResults);
            return declarationList;
        }

        private static string ConvertCustomFilter(List<QueryFilterItem> additionalFilters, int tenant)
        {
            QueryOperations queryOperations = new QueryOperations() { QueryFilterItems = additionalFilters };
            ICustomContext context = CustomContext.GetContext(tenant);
            IQueryable<Declaration> iQueryable = (from a in context.Declarations select a);
            iQueryable = new DeclarationListQueryService(context).ApplyCustomFilters(queryOperations, iQueryable, tenant);

            string filters = FilterHelper.ConvertQueryable(iQueryable);

            return filters;
        }
    }
}
