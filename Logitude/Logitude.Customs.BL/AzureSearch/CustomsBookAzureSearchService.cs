using Logitude.Customs.Data.AzureSearch.Repo;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logitude.Customs.Data.AzureSearch.Entities;
using Logitude.BL.Helpers;

namespace ConsoleDevFramwork.AzureSearch
{
    public static class CustomsBookAzureSearchService
    {
        static string serviceName => DefaultService.Instance.Get(0, "AzureSearchAI", "connection").Value1;
        static string apiKey => DefaultService.Instance.Get(0, "AzureSearchAI", "connection").Value2;

        public static async Task<RemarkAndCustomsBook> SearchItmesAndRemark(string searchValue, string customsBookType, int tenant)
        {
            if(string.IsNullOrEmpty(apiKey))
                throw new System.Exception("Please provide the Azure Search API Key in the apiKey variable in CustomsBookAzureSearchService.cs file");

            Task<List<CustomsItemASEntity>> customsBookListTask = new CustomsBookAzureSearchRepo(serviceName, apiKey).SearchCustomsItemAsync(customsBookType, searchValue);
            Task<List<RemarkWithCustomsBookASEntity>> remarkCustomsBookListTask = new RemarksCustomsBookAzureSearchRepo(serviceName, apiKey).GetRemarksAndcustomsItemsAsync(searchValue, customsBookType, tenant);
            await Task.WhenAll(customsBookListTask, remarkCustomsBookListTask);
            RemarkAndCustomsBook res = new RemarkAndCustomsBook { CustomsItems = await customsBookListTask, Remarks = await remarkCustomsBookListTask };

            return res;
        }
    }

    public class RemarkAndCustomsBook
    {
        public List<RemarkWithCustomsBookASEntity> Remarks { get; set; }
        public List<CustomsItemASEntity> CustomsItems { get; set; }
    }
}