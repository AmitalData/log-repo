using Logitude.BL.Helpers;
using Logitude.Customs.BL.AzureSearch.Objects;
using NetCommonHelper.Logger;
using Newtonsoft.Json;
using Simplog.Data.AzureSearch.Repo;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.AzureSearch
{
    public class FastSearchService
    {
        private static DefaultAndConfiguration_Ext ConnectionDetails => DefaultService.Instance.Get(0, "AzureSearchAI", "Customs");
        private static string serviceName => ConnectionDetails.Value1;
        private static string apiKey => ConnectionDetails.Value2;
        private static readonly DevLog logger = DevLog.Instance;

        private async Task<List<dynamic>> Search(ApiQueryFilters apiQueryFilters, int tenant, string searchText, string tableName)
        {
            logger.WriteDebug($"Search {tableName} ASAI: {apiQueryFilters?.AdditionalFilters}, tenant: {tenant}, searchText: {searchText}");

            string filters = "";
            List<QueryFilterItem> additionalFilters = null;

            if (tenant == null)
                throw new ArgumentNullException(nameof(tenant), "tenant is null");

            if (apiQueryFilters?.AdditionalFilters != null)
            {
                additionalFilters = JsonConvert.DeserializeObject<List<QueryFilterItem>>(apiQueryFilters.AdditionalFilters);

                MenipulateAdditionalFilters(additionalFilters, tenant);

                List<QueryFilterItem> simpleFilters = additionalFilters.Where(x => x.IsCustom == false).ToList();
                filters = FilterHelper.ConvertQueryFilter(simpleFilters);

                QueryOperations queryOperations = new QueryOperations() { QueryFilterItems = additionalFilters };
                IQueryable customFilterQueryable = GetCustomFilterQueryable(queryOperations, tenant);
                if (customFilterQueryable != null)
                {
                    string customFilters = FilterHelper.ConvertQueryable(customFilterQueryable);
                    if (!string.IsNullOrEmpty(customFilters) && !string.IsNullOrEmpty(filters))
                        filters += " and ";
                    filters += customFilters;
                }
            }

            filters = MenipulateFilters(additionalFilters, filters, tenant);

            FastSearchAzureSearchRepo fastSearchAzureSearchRepo = new FastSearchAzureSearchRepo(serviceName, apiKey, tableName);
            List<string> fieldsNotExistsInIndex = await FieldsNotExistsInIndex(filters, fastSearchAzureSearchRepo);
            if (fieldsNotExistsInIndex.Count > 0)
                throw new FieldsNotExistsInIndexException(fieldsNotExistsInIndex);

            string indexSettingsName = GetSettingsName(additionalFilters, filters, tenant) ?? tableName;
            FastSearchSettings settings = await GetIndexSettingsAsync(tenant, indexSettingsName);
            List<string> selectedFields = GetSelectedFields(settings);

            return await fastSearchAzureSearchRepo.SearchAsync(filters, searchText, settings.maxResults, selectedFields);
        }

        protected virtual void MenipulateAdditionalFilters(List<QueryFilterItem> additionalFilters, int tenant) { }

        protected virtual IQueryable GetCustomFilterQueryable(QueryOperations queryOperations, int tenant) => null;

        protected virtual string MenipulateFilters(List<QueryFilterItem> additionalFilters, string filters, int tenant) => filters;

        protected virtual string GetSettingsName(List<QueryFilterItem> additionalFilters, string filters, int tenant) => null;

        public static Task<FastSearchSettings> GetIndexSettingsAsync(int tenant, string index) => Task.Run(() => GetIndexSettings(tenant, index));

        private static FastSearchSettings GetIndexSettings(int tenant, string index)
        {
            string settings = DefaultService.Instance.Get(tenant, "AzureSearchAI", index)?.Value1;
            if (string.IsNullOrEmpty(settings))
                throw new ArgumentNullException(nameof(settings), "AzureSearch settings default not found");

            return JsonConvert.DeserializeObject<FastSearchSettings>(settings);
        }

        private static List<string> GetSelectedFields(FastSearchSettings settings) =>
            Regex.Matches(settings.ddlHtmlLine, @"\{([^:{}\s]+)(?::[^{}]*)?\}")
                               .Cast<Match>()
                               .Select(m => m.Groups[1].Value)
                               .ToList();

        private static async Task<List<string>> FieldsNotExistsInIndex(string filter, FastSearchAzureSearchRepo fastSearchAzureSearchRepo)
        {
            List<string> filtersFields = ExtractFieldsNameFromFilter(filter);
            List<string> indexFields = await fastSearchAzureSearchRepo.GetFieldsNameAsync();
            return filtersFields.Where(x => !indexFields.Contains(x, StringComparer.OrdinalIgnoreCase)).ToList();
        }

        private static List<string> ExtractFieldsNameFromFilter(string filter)
        {
            Regex fieldPattern = new Regex(@"\(?\s*(\w+)\s+(eq|ne|gt|lt|ge|le)\s+[^()]+\)?", RegexOptions.IgnoreCase);
            MatchCollection matches = fieldPattern.Matches(filter);

            HashSet<string> fieldList = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (Match match in matches)
                if (match.Groups.Count > 1)
                    fieldList.Add(match.Groups[1].Value);

            string[] fields = new string[fieldList.Count];
            fieldList.CopyTo(fields);
            return fields.ToList();
        }

        public static async Task<List<dynamic>> Search(ApiQueryFilters filters, string searchText, string index, int tenant)
        {
            FastSearchService fastSearchService = new FastSearchService();

            switch (index)
            {
                case "declarations":
                    fastSearchService = new DeclarationAzureSearchService();
                    break;

                default:
                    throw new Exception(message: $"Index {index} not found");
            }

            List<dynamic> result = await fastSearchService.Search(filters, tenant, searchText, index);

            return result;
        }
    }
}
