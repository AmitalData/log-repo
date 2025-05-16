using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

            string keyFieldName = (await GetKeyFieldAsync().ConfigureAwait(false)).Name;
            selectedFields.Add(keyFieldName);
            searchOptions.Select.Clear();
            selectedFields?.ForEach(searchOptions.Select.Add);

            Response<SearchResults<SearchDocument>> searchResponse = await GetSearchClient().SearchAsync<SearchDocument>(searchText, searchOptions).ConfigureAwait(false);
            Pageable<SearchResult<SearchDocument>> searchResults = searchResponse.Value.GetResults();
            List<dynamic> res = searchResults.Select(x => x.Document).ToList<dynamic>();

            return res;
        }

        public async Task<SearchField> GetKeyFieldAsync() => (await GetFieldsAsync().ConfigureAwait(false)).FirstOrDefault(f => f.IsKey == true);

        public async Task<string> GetKeyFieldNameAsync() => (await GetKeyFieldAsync().ConfigureAwait(false)).Name;

        public async Task<List<string>> GetFieldsNameAsync() => (await GetFieldsAsync().ConfigureAwait(false)).Select(x => x.Name).ToList();

        public async Task<AzureSerchResponse> RunIndexerAsync(string indexerName, CancellationToken cancellationToken = default)
        {
            SearchIndexerClient searchIndexerClient = new SearchIndexerClient(serviceEndpoint, credential);
            Response response = await searchIndexerClient.RunIndexerAsync(indexerName, cancellationToken).ConfigureAwait(false);
            return new AzureSerchResponse(response);
        }

        public async Task<AzureSerchResponse> DeleteAsync(string filter, int size, CancellationToken cancellationToken = default)
        {
            string keyFieldName = await GetKeyFieldNameAsync().ConfigureAwait(false);
            if(string.IsNullOrEmpty(keyFieldName))
                throw new System.Exception($"Could not determine key field for index '{indexName}'.");
                
            SearchOptions searchOptions = new SearchOptions
            {
                Size = size,
                Filter = filter,
                Select = { keyFieldName },
            };

            Response<SearchResults<dynamic>> searchResponse = await GetSearchClient().SearchAsync<dynamic>("*", searchOptions, cancellationToken).ConfigureAwait(false);
            Pageable<SearchResult<dynamic>> searchResults = searchResponse.Value.GetResults();
            List<dynamic> keysToDelete = searchResults.Select(res => res.Document).ToList();

            if (keysToDelete.Count == 0)
                return null;

            Response response = (await DeleteAsync(keysToDelete, cancellationToken).ConfigureAwait(false)).GetRawResponse();
            return new AzureSerchResponse(response, keysToDelete.Count);
        }
    }
}
