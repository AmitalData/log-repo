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

            if (apiQueryFilters != null)
            {
                List<QueryFilterItem> additionalFilters = JsonConvert.DeserializeObject<List<QueryFilterItem>>(apiQueryFilters.AdditionalFilters);

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

            List<DeclarationASEntity> declarationList = await new DeclarationAzureSearchRepo(FilterHelper.serviceName, FilterHelper.apiKey).SearchAsync(filters, searchText);
            //List<string> displayFields = new List<string>() { "CustomFileNo", "DeclarationNumber" }; TO DO: Implement display fields like "{תאריך פתיחה} {סוג משלוח ICON } {מספר תיק יצוא} / {מספר תיק מכס{ {לקוח} "
            //List<string> result = declarationList.Select( x => $"{x.CustomFileNo} {x.DeclarationNumber}").ToList();
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
