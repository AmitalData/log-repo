using Logitude.Customs.Data.AzureSearch.Repo;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Logitude.Customs.Data.AzureSearch.Entities;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.BL.Helpers;

namespace Logitude.Customs.BL.AzureSearch
{
    public static class CustomsBookAzureSearchService
    {
        //static string serviceName => DefaultService.Instance.Get(0, "AzureSearchAI", "connection").Value1;
        //static string apiKey => DefaultService.Instance.Get(0, "AzureSearchAI", "connection").Value2;
        static string serviceName => System.Configuration.ConfigurationManager.AppSettings["AzureSearchAIUrl"];
        static string apiKey => System.Configuration.ConfigurationManager.AppSettings["AzureSearchAIKey"];

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

        public static async Task ReCreateRemarkTable()
        {
            var remarkSerivce = new RemarksCustomsBookAzureSearchRepo(serviceName, apiKey);
            List<RemarksClassification> allData = new RemarksClassificationRepository(0).GetAll(0).ToList();
            DataTable remarksOnCustomsBookTable = ConvertQueryableToDataTable(allData);
            await remarkSerivce.ReCreateAsync(remarksOnCustomsBookTable);
        }

        public static async Task ReCreateCustomsBookTable()
        {
            CustomsBookAzureSearchRepo customsBookAzureSearchService = new CustomsBookAzureSearchRepo(serviceName, apiKey);
            List<CB_CustomsItemComputedData> allData = new CB_CustomsItemComputedDataRepository(0).GetAll().ToList();
            DataTable customsBookTable = ConvertQueryableToDataTable(allData);
            await customsBookAzureSearchService.ReCreateAsync(customsBookTable);
        }

        public static DataTable ConvertQueryableToDataTable<T>(List<T> results)
        {
            DataTable dataTable = new DataTable();

            foreach (var property in typeof(T).GetProperties())
                dataTable.Columns.Add(property.Name, property.PropertyType);

            foreach (var result in results)
            {
                DataRow row = dataTable.NewRow();

                foreach (var property in typeof(T).GetProperties())
                    row[property.Name] = property.GetValue(result);

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
    }

    public class RemarkAndCustomsBook
    {
        public List<RemarkWithCustomsBookASEntity> Remarks { get; set; }
        public List<CustomsItemASEntity> CustomsItems { get; set; }
    }
}