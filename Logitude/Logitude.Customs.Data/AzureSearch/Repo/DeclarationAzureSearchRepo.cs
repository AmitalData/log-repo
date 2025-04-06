using Azure.Search.Documents;
using Logitude.Customs.Data.AzureSearch.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using Simplog.Data.AzureSearch.Repo;

namespace Logitude.Customs.Data.AzureSearch.Repo
{
    public class DeclarationAzureSearchRepo : AzureSearchRepoBase<DeclarationASEntity>
    {
        private static readonly string[] _indexFields = new string[] { "SearchFields", "CasualSupplierName", "CourierHAWB", "CargoDescription" };
        private static readonly string tableName = "declarations";

        public DeclarationAzureSearchRepo(string serviceName, string apiKey)
            : base(
                  serviceName,
                  apiKey,
                  tableName,
                  _indexFields
            )
        { }

        public async Task<List<DeclarationASEntity>> SearchAsync(string filter, string searchText)
        {            
            searchText += "*";

            SearchOptions options = new SearchOptions
            {
                Filter = filter,
                Size = 500,
            };

            List<DeclarationASEntity> res = await SearchAsync(searchText, options);

            return res;
        }        
    }
}
