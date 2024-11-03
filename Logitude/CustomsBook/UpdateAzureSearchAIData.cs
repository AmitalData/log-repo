using Logitude.Customs.Data.AzureSearch.Repo;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using NLog;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CustomsBook
{
    internal class UpdateAzureSearchAIData
    {
        static readonly Logger logger = Program.logger;

        public static async Task Update()
        {
            try
            {
                logger.Info("Update Azure Search AI data started");

                await ReCreateRemarkTable();
                await ReCreateCustomsBookTable();

                logger.Info("Update Azure Search AI data finished");
            }
            catch (Exception e)
            {
                logger.Fatal(e, "Update Azure Search AI data failed");
            }
        }

        private static async Task ReCreateRemarkTable()
        {
            List<RemarksClassification> allData = new RemarksClassificationRepository(0).GetAll(0).ToList();
            DataTable remarksOnCustomsBookTable = ConvertQueryableToDataTable(allData);
            await new RemarksCustomsBookAzureSearchRepo(AzureSearchData.Instance.ServiceName, AzureSearchData.Instance.ApiKey).ReCreateAsync(remarksOnCustomsBookTable);
        }

        public static async Task ReCreateCustomsBookTable()
        {
            List<CB_CustomsItemComputedData> allData = new CB_CustomsItemComputedDataRepository(0).GetAll().ToList();
            DataTable customsBookTable = ConvertQueryableToDataTable(allData);
            await new CustomsBookAzureSearchRepo(AzureSearchData.Instance.ServiceName, AzureSearchData.Instance.ApiKey).ReCreateAsync(customsBookTable);
        }

        private static DataTable ConvertQueryableToDataTable<T>(List<T> results)
        {
            DataTable dataTable = new DataTable();

            foreach (var property in typeof(T).GetProperties())
            {
                Type propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                dataTable.Columns.Add(property.Name, propertyType);
            }

            foreach (var result in results)
            {
                DataRow row = dataTable.NewRow();

                foreach (var property in typeof(T).GetProperties())
                    row[property.Name] = property.GetValue(result) ?? DBNull.Value;

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        class AzureSearchData
        {
            public string ServiceName { get; set; }
            public string ApiKey { get; set; }
            private static AzureSearchData instance;
            public static AzureSearchData Instance
            {
                get { return instance ?? (instance = Get()); }
            }

            public static AzureSearchData Get()
            {
                string ServiceName = System.Configuration.ConfigurationManager.AppSettings["AzureSearchAIUrl"];
                string ApiKey = System.Configuration.ConfigurationManager.AppSettings["AzureSearchAIKey"];

                if (string.IsNullOrEmpty(ServiceName) || string.IsNullOrEmpty(ApiKey))
                    throw new Exception("Please provide the Azure Search API server name and Key in AppSettings 'AzureSearchAIUrl' and 'AzureSearchAIKey'");

                AzureSearchData data = new AzureSearchData() { ServiceName = ServiceName, ApiKey = ApiKey };

                //DefaultAndConfiguration azureSearchData = new DefaultAndConfigurationRepository(0)
                //.GetDefaultAndConfigurations(0)
                //.Where(x => x.SetKey == "AzureSearchAI" && x.AdditionalKey == "connection")
                //.FirstOrDefault();

                //if (azureSearchData == null)
                //    throw new Exception("Please provide the Azure Search API server name and Key in tenant 0 setKey 'AzureSearchAI' AdditionalKey 'connection'");

                //AzureSearchData data = new AzureSearchData() { ServiceName = azureSearchData.Value1, ApiKey = azureSearchData.Value2 };

                return data;
            }
        }
    }
}
