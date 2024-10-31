using Azure.Search.Documents;
using Logitude.Customs.Data.AzureSearch.Entities;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.AzureSearch.Repo;

namespace Logitude.Customs.Data.AzureSearch.Repo
{
    public class CustomsBookAzureSearchRepo : AzureSearchRepoBase<CustomsItemASEntity>
    {
        public CustomsBookAzureSearchRepo(string serviceName, string apiKey)
            : base(
                  serviceName,
                  apiKey,
                  "customs-book",
                  new string[] { "FullClassification", "CIH_GoodsDescription" }
            )
        { }

        public async Task<List<CustomsItemASEntity>> SearchByCustomsItemsAsync(string customsBookType, int[] customsItemIds)
        {
            SearchOptions options = new SearchOptions();
            options.Filter = "(" + string.Join(" or ", customsItemIds.Select(customsItemId => $"CustomsItemID eq {customsItemId}")) + ")";
            return await SearchAsync(customsBookType, "*", options);
        }

        public async Task<List<CustomsItemASEntity>> SearchCustomsItemAsync(string customsBookType, string searchText = "*") => await SearchAsync(customsBookType, searchText, new SearchOptions());

        public async Task<List<CustomsItemASEntity>> SearchAsync(string customsBookType, string searchText = "*", SearchOptions options = null)
        {
            if (options == null)
                options = new SearchOptions();

            if (searchText != "*")
                searchText += "*";

            if (options.Filter != null)
                options.Filter += " and ";

            options.Filter += $"CI_CustomsItemCategoryIDNum eq '1' and CustomsItemEntityStatusIDNum eq 2 and CI_CustomsBookTypeIDNum eq '{customsBookType}' and EndDate ge {DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ")} and StartDate le {DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ")}"; ;
            options.Size = 1000;
            options.Select.Clear();
            options.Select.Add("CustomsItemID");
            options.Select.Add("FullClassification");
            options.Select.Add("CIH_GoodsDescription");

            return await SearchAsync(searchText, options);
        }
    }
}