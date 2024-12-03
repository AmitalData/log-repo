using Azure.Search.Documents.Indexes;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using System.Threading.Tasks;
using System.Data;
using System.Net;

namespace Simplog.Data.AzureSearch.Repo
{
    public class AzureSearchRepoBase<T>
    {
        public readonly Uri serviceEndpoint;
        public readonly AzureKeyCredential credential;
        public readonly SearchIndexClient adminClient;        
        public readonly string serviceName;
        public readonly string indexName;
        public readonly string suggesterName;
        public readonly string[] indexFields;

        public AzureSearchRepoBase(string serviceName, string apiKey, string tableName, string[] indexFields)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            serviceEndpoint = new Uri($"https://{serviceName}.search.windows.net/");
            credential = new AzureKeyCredential(apiKey);
            adminClient = new SearchIndexClient(serviceEndpoint, credential);
            indexName = tableName + "-index";
            suggesterName = tableName + "-sg";
            this.indexFields = indexFields;
            this.serviceName = serviceName;
        }

        public async Task DeleteAsync(List<T> records)
        {
            await adminClient.GetSearchClient(indexName).DeleteDocumentsAsync(records);
        }

        public async Task DropAsync()
        {
            await adminClient.DeleteIndexAsync(indexName);
        }

        public async Task CreateAsync()
        {
            FieldBuilder fieldBuilder = new FieldBuilder();
            var searchFields = fieldBuilder.Build(typeof(T));
            var definition = new SearchIndex(indexName, searchFields);
            var suggester = new SearchSuggester(suggesterName, indexFields);
            definition.Suggesters.Add(suggester);

            await adminClient.CreateOrUpdateIndexAsync(definition);
        }

        //public async Task<AutocompleteResults> Search(string searchText = "*")
        //{
        //    var options = new AutocompleteOptions
        //    {              
        //        Size = 100,
        //        Filter = "CI_CustomsItemCategoryIDNum eq '1'"
        //    };
        //    var res = await new SearchClient(serviceEndpoint, indexName, credential).AutocompleteAsync(searchText, suggesterName, options);
        //    return res.Value;
        //}

        public async Task<List<T>> SearchAsync(string searchText = "*", SearchOptions options = null)
        {
            searchText = System.Text.RegularExpressions.Regex.Replace(searchText, @"\s+", " ").Trim();
            if (searchText.EndsWith(" *"))
                searchText = searchText.Substring(0, searchText.Length - 2) + "*";

            if (options == null)
                options = new SearchOptions();

            options.SearchMode = options.SearchMode ?? SearchMode.All;

            Response<SearchResults<T>> response = await new SearchClient(serviceEndpoint, indexName, credential)
                .SearchAsync<T>(searchText, options);

            return response.Value.GetResults().Select(x => x.Document).ToList();
        }

        public async Task ReCreateAsync(DataTable dt = null)
        {
            try
            {
                await DropAsync();
            }
            catch (Exception) { }

            await CreateAsync();
            if (dt != null)
                await AddOrMergeAsync(dt);
        }

        public async Task AddOrMergeAsync(DataTable dt)
            => await AddOrMergeAsync(DataTableToList(dt));

        public async Task AddOrMergeAsync(List<T> records)
        {
            int chunkSize = 32000;
            List<Task> list = new List<Task>();
            
            for (int i = 0; i < records.Count; i += chunkSize)
            {
                List<T> chunk = records.Skip(i).Take(chunkSize).ToList();
                list.Add(IndexDocumentsAsync(chunk));
            }

            await Task.WhenAll(list);
        }

        private async Task IndexDocumentsAsync(List<T> records)
        {
            IndexDocumentsBatch<T> batch = IndexDocumentsBatch.Create(
                records.Select(record => IndexDocumentsAction.MergeOrUpload(record)).ToArray()
            );
            IndexDocumentsResult result = await adminClient.GetSearchClient(indexName).IndexDocumentsAsync(batch);
        }

        private List<T> DataTableToList(DataTable dt)
        {
            List<T> objList = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                T obj = DataRowToObject(row);
                objList.Add(obj);
            }

            return objList;
        }

        private T DataRowToObject(DataRow row)
        {
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in row.Table.Columns)
            {
                var property = typeof(T).GetProperty(column.ColumnName);
                try
                {

                    if (property != null && row[column] != DBNull.Value)
                    {
                        property.SetValue(obj, row[column]);
                    }
                }
                catch (Exception e)
                {

                    throw;
                }
            }

            return obj;
        }
    }
}
