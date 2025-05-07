using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Simplog.Data.AzureSearch.Repo;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logitude.Customs.Data.AzureSearch.Repo
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

            Response<SearchResults<SearchDocument>> searchResponse = await GetSearchClient().SearchAsync<SearchDocument>("*", searchOptions);
            Pageable<SearchResult<SearchDocument>> searchResults = searchResponse.Value.GetResults();
            List<dynamic> res = searchResults.Select(x => x.Document).ToList<dynamic>();
            
            return res;
        }
    }
}
