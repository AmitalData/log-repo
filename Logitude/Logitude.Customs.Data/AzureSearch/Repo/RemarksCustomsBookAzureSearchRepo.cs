using Azure.Search.Documents;
using Logitude.Customs.Data.AzureSearch.Entities;
using Simplog.Data.AzureSearch.Repo;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logitude.Customs.Data.AzureSearch.Repo
{
    public class RemarksCustomsBookAzureSearchRepo : AzureSearchRepoBase<RemarksCustomsBookASEntity>
    {
        public RemarksCustomsBookAzureSearchRepo(string serviceName, string apiKey) 
            : base(
                  serviceName, 
                  apiKey,
                  "remarks-customs-book",
                  new string[] { "RemarkDescription" }
            )
        { }

        public async Task<List<RemarkWithCustomsBookASEntity>> GetRemarksAndcustomsItemsAsync(string searchValue, string customsBookType, int tenant)
        {
            SearchOptions searchOptions = new SearchOptions { Filter = $"Tenant eq {tenant}" };
            List<RemarksCustomsBookASEntity> remarks = await SearchAsync(searchValue);

            int[] customsItemsIds = remarks.Select(x => x.CustomsItemsID).ToArray();
            if (customsItemsIds.Length == 0)
                return new List<RemarkWithCustomsBookASEntity>();

            List<CustomsItemASEntity> customsItems = await new CustomsBookAzureSearchRepo(serviceName, credential.Key).SearchByCustomsItemsAsync(customsBookType, customsItemsIds);
            List<RemarkWithCustomsBookASEntity> remarksAndCustomsBook = remarks.Join(
                customsItems,
                remark => remark.CustomsItemsID,
                customsItem => customsItem.CustomsItemID,
                (remark, customsItem) => new RemarkWithCustomsBookASEntity { Remark = remark, CustomsItem = customsItem }
            ).ToList();

            return remarksAndCustomsBook;
        }
    }
}
