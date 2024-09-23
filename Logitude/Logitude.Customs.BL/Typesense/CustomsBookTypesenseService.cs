using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.Typesence.Models;
using Logitude.Customs.Data.Typesence.Services;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CustomsItem = Logitude.Customs.Data.Typesence.Models.CustomsItem;

namespace Logitude.Customs.BL.Typesense
{
    public class CustomsBookTypesenseService
    {
        private readonly string apiKey = "xyz";
        private readonly string baseUrl = "http://tomer-11pc:8108/";

        public async Task<RemarkAndCustomsBook> SearchInItmesAndRemark(string searchValue, string customsBookType, int tenant)
        {
            CustomsBookServices customsBookEngineSearch = new CustomsBookServices(apiKey, baseUrl);
            RemarksCustomsBookServices remarksCustomsBookEngineSearch = new RemarksCustomsBookServices(apiKey, baseUrl);
            Task<List<CustomsItem>> customsBookListTask = customsBookEngineSearch.SearchCustomsAsync(searchValue, customsBookType);
            Task<List<RemarkWithCustomsBook>> remarkCustomsBookListTask = remarksCustomsBookEngineSearch.GetRemarksAndcustomsItems(searchValue, customsBookType);
            await Task.WhenAll(customsBookListTask, remarkCustomsBookListTask);
            RemarkAndCustomsBook res = new RemarkAndCustomsBook { CustomsItems = await customsBookListTask, Remarks = await remarkCustomsBookListTask };

            return res;
        }

        public async Task RecreateCustomsBook()
        {
            CustomsBookServices customsBookEngineSearch = new CustomsBookServices(apiKey, baseUrl);
            List<CB_CustomsItemComputedData> allData = new CB_CustomsItemComputedDataRepository(0).GetAll().ToList();
            DataTable customsBookTable = ConvertQueryableToDataTable(allData);
            await customsBookEngineSearch.ReCreateTableAsync(customsBookTable);
        }

        public async Task RecreateRemarks()
        {
            RemarksCustomsBookServices remarksCustomsBookEngineSearch = new RemarksCustomsBookServices(apiKey, baseUrl);
            List<RemarksClassification> allData = new RemarksClassificationRepository(0).GetAll(0).ToList();
            DataTable remarksOnCustomsBookTable = ConvertQueryableToDataTable(allData);
            await remarksCustomsBookEngineSearch.ReCreateTableAsync(remarksOnCustomsBookTable);
        }

        public DataTable ConvertQueryableToDataTable<T>(List<T> results)
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
        public List<RemarkWithCustomsBook> Remarks { get; set; }
        public List<CustomsItem> CustomsItems { get; set; }
    }
}
