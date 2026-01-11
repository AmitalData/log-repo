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
        private const string AzureSearchAISetKey = "AzureSearchAI";
        private const string AzureSearchAISettingsSetKey = "AzureSearchAISettings";
        private static readonly DevLog logger = DevLog.Instance;

        private static readonly Dictionary<string, Func<FastSearchService>> _indexRegistry = new Dictionary<string, Func<FastSearchService>>()
        {
            { "declarations", () => new DeclarationAzureSearchService() },
             {"journallines" ,()=> new JournalAzureSearchService()}


        };

        public static DefaultAndConfiguration_Ext GetAzureSearchAISettings(int tenant) => DefaultService.Instance.Get(tenant, AzureSearchAISettingsSetKey, "Customs");


        public static DefaultAndConfiguration_Ext GetConnectionDetails(int tenant)
        {
            DefaultAndConfiguration_Ext ConnectionDetails = DefaultService.Instance.Get(tenant, AzureSearchAISetKey, "Customs");
            if (ConnectionDetails == null)
                ConnectionDetails = DefaultService.Instance.Get(0, AzureSearchAISetKey, "Customs");
            if (ConnectionDetails == null)
                throw new Exception("connection detailes for search AI not found");

            return ConnectionDetails;
        }

        public static async Task<List<dynamic>> Search(ApiQueryFilters filters, string searchText, string index, int tenant)
        {
            if (!_indexRegistry.TryGetValue(index, out Func<FastSearchService> serviceFactory))
                throw new Exception($"Index {index} not found");

            FastSearchService fastSearchService = serviceFactory();
            return await fastSearchService.Search(filters, tenant, searchText, index);
        }

        private async Task<List<dynamic>> Search(ApiQueryFilters apiQueryFilters, int tenant, string searchText, string tableName)
        {
            logger.WriteDebug($"[FastSearch] Starting search, Index: {tableName}, tenant: {tenant}, searchText: {searchText} filters: {apiQueryFilters?.AdditionalFilters}");

            string filters = "";
            List<QueryFilterItem> additionalFilters = null;

            if (apiQueryFilters?.AdditionalFilters != null)
            {
                additionalFilters = JsonConvert.DeserializeObject<List<QueryFilterItem>>(apiQueryFilters.AdditionalFilters);

                ManipulateAdditionalFilters(additionalFilters, tenant);

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

            filters = ManipulateFilters(additionalFilters, filters, tenant);

            DefaultAndConfiguration_Ext connectionDetails = GetConnectionDetails(tenant);
            string indexBaseName = GetIndexBaseName(additionalFilters, filters, tenant, tableName) ?? tableName;

            FastSearchAzureSearchRepo fastSearchAzureSearchRepo = new FastSearchAzureSearchRepo(connectionDetails.Value1, connectionDetails.Value2, indexBaseName);

            List<string> fieldsNotExistsInIndex = await FieldsNotExistsInIndex(filters, fastSearchAzureSearchRepo);
            if (fieldsNotExistsInIndex.Count > 0)
                throw new FieldsNotExistsInIndexException(fieldsNotExistsInIndex);

            string indexSettingsName = GetSettingsName(additionalFilters, filters, tenant) ?? tableName;
            FastSearchSettings settings = await GetIndexSettingsAsync(tenant, indexSettingsName);
            List<string> selectedFields = GetSelectedFields(settings);

            DefaultAndConfiguration_Ext azureSearchAISettings = GetAzureSearchAISettings(tenant);
            bool prefixSearch = azureSearchAISettings == null || (bool)azureSearchAISettings.ObjVal1;

            return await fastSearchAzureSearchRepo.SearchAsync(filters, searchText, settings.maxResults, selectedFields, prefixSearch, settings.orderByField, settings.descending);
        }

        protected virtual void ManipulateAdditionalFilters(List<QueryFilterItem> additionalFilters, int tenant) { }

        protected virtual IQueryable GetCustomFilterQueryable(QueryOperations queryOperations, int tenant) => null;

        protected virtual string ManipulateFilters(List<QueryFilterItem> additionalFilters, string filters, int tenant) => filters;

        protected virtual string GetSettingsName(List<QueryFilterItem> additionalFilters, string filters, int tenant) => null;

        public static Task<FastSearchSettings> GetIndexSettingsAsync(int tenant, string index) => Task.FromResult(GetIndexSettings(tenant, index));

        protected virtual string GetIndexBaseName(List<QueryFilterItem> additionalFilters, string finalFilters, int tenant, string requestedIndex) => requestedIndex;


        private static FastSearchSettings GetIndexSettings(int tenant, string index)
        {
            string settings = DefaultService.Instance.Get(tenant, AzureSearchAISetKey, index)?.Value1;
            if (string.IsNullOrEmpty(settings))
                throw new ArgumentNullException(nameof(settings), "AzureSearch settings default not found");

            FastSearchSettings fastSearchSettings = JsonConvert.DeserializeObject<FastSearchSettings>(settings);
            fastSearchSettings.minimumSearchQueryLength = fastSearchSettings.minimumSearchQueryLength ?? 3;

            return fastSearchSettings;
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
    }
}
