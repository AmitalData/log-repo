using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simplog.Data.AzureSearch.Repo
{
    public class FastSearchAzureSearchRepo : AzureSearchRepoBase<dynamic>
    {
        public FastSearchAzureSearchRepo(string serviceName, string apiKey, string tableName)
            : base(
                  serviceName,
                  apiKey,
                  tableName,
                  new string[] { }
            )
        { }

        public async Task<List<dynamic>> SearchAsync(string filter, string searchText, int maxResult, List<string> selectedFields)
        {
            searchText += "*";

            SearchOptions searchOptions = new SearchOptions
            {
                Filter = filter,
                Size = maxResult,
            };

            string keyFieldName = (await GetKeyFieldAsync()).Name;
            selectedFields.Add(keyFieldName);
            searchOptions.Select.Clear();
            selectedFields.ForEach(searchOptions.Select.Add);

            Response<SearchResults<SearchDocument>> searchResponse = await GetSearchClient().SearchAsync<SearchDocument>(searchText, searchOptions);
            Pageable<SearchResult<SearchDocument>> searchResults = searchResponse.Value.GetResults();
            List<dynamic> res = searchResults.Select(x => x.Document).ToList<dynamic>();

            return res;
        }

        public async Task<SearchField> GetKeyFieldAsync() => (await GetFieldsAsync()).FirstOrDefault(f => f.IsKey == true);

        public async Task<string> GetKeyFieldNameAsync() => (await GetKeyFieldAsync()).Name;

        public async Task<List<string>> GetFieldsNameAsync() => (await GetFieldsAsync()).Select(x => x.Name).ToList();

        public async Task<AzureSerchResponse> RunIndexerAsync(string indexerName)
        {
            SearchIndexerClient searchIndexerClient = new SearchIndexerClient(serviceEndpoint, credential);
            Response response = await searchIndexerClient.RunIndexerAsync(indexerName);
            return new AzureSerchResponse(response);
        }

        public async Task<AzureSerchResponse> DeleteAsync(string filter, int size)
        {
            string keyFieldName = await GetKeyFieldNameAsync();

            SearchOptions searchOptions = new SearchOptions
            {
                Size = size,
                Filter = filter,
                Select = { keyFieldName },
            };

            Response<SearchResults<dynamic>> searchResponse = await GetSearchClient().SearchAsync<dynamic>("*", searchOptions);
            Pageable<SearchResult<dynamic>> searchResults = searchResponse.Value.GetResults();
            List<dynamic> keysToDelete = searchResults.Select(res => res.Document).ToList();

            if (keysToDelete.Count == 0)
                return null;

            Response response = (await DeleteAsync(keysToDelete)).GetRawResponse();
            return new AzureSerchResponse(response);
        }
    }
}
