using Logitude.DashboardModule.Data;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.Data.Repositories;
using Logitude.Server.Tools.Counters;
using Newtonsoft.Json;
using Simplog.Data.InfrastructureModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebFreight.Web.MetaDataUpdate
{
    public class DashboardAnalyticTablesUpdateClass
    {
        private readonly IWebFreightContext WebFreightContext;
        private readonly IDashboardContext DashboardContext;
        private readonly AnalyticsFactsFieldsMetaDataRepository AnalyticsFactsFieldsMetaDataRepository;
        private readonly AnalyticsFactsMetaDataRepository AnalyticsFactsMetaDataRepository;

        public DashboardAnalyticTablesUpdateClass(IWebFreightContext context)
        {
            WebFreightContext = context;
            DashboardContext = Logitude.DashboardModule.Data.DashboardContext.GetContext(0);
            AnalyticsFactsFieldsMetaDataRepository = new AnalyticsFactsFieldsMetaDataRepository(DashboardContext);
            AnalyticsFactsMetaDataRepository = new AnalyticsFactsMetaDataRepository(DashboardContext);
        }

        internal void Update()
        {
            return;
            var analyticTables = new List<Logitude.DashboardModule.MetaDataTool.Models.AnalyticsFactsMetaData>();
            foreach (string fileName in Directory.GetFiles(GetProjectPath(), "*.ljson"))
            {
                analyticTables.Add(BuildFileJson(fileName));
            }
            if (!analyticTables.Any()) return;
            UpdateAnalyticsFactsMetaDatas(analyticTables);
            AnalyticsFactsMetaDataRepository.SubmitChanges();
            AnalyticsFactsFieldsMetaDataRepository.SubmitChanges();
        }

        private void UpdateAnalyticsFactsMetaDatas(List<Logitude.DashboardModule.MetaDataTool.Models.AnalyticsFactsMetaData> jsonFilesTables)
        {
            var analyticsFactsMetaDatas = GetAnalyticsFactsMetaDatasFromDataBase();
            foreach (var jsonTable in jsonFilesTables)
            {
                UpdateAnalyticsFactsMetaData(jsonTable, analyticsFactsMetaDatas.ContainsKey(jsonTable.TableName) ? analyticsFactsMetaDatas[jsonTable.TableName] : null);
            }
        }

        private Dictionary<string, AnalyticsFactsMetaData> GetAnalyticsFactsMetaDatasFromDataBase()
        {
            return DashboardContext.AnalyticsFactsMetaDatas.ToDictionary(d => d.TableName, a => a);
        }

        private void UpdateAnalyticsFactsMetaData(Logitude.DashboardModule.MetaDataTool.Models.AnalyticsFactsMetaData jsonTable, AnalyticsFactsMetaData sqlTable)
        {
            if (sqlTable == null) AddNewTableToDB(jsonTable);
            else if (sqlTable.HashString != sqlTable.HashString) UpdateTable(jsonTable, sqlTable);
        }

        private void UpdateTable(Logitude.DashboardModule.MetaDataTool.Models.AnalyticsFactsMetaData jsonTable, AnalyticsFactsMetaData sqlTable)
        {
            var table = JsonConvert.DeserializeObject<AnalyticsFactsMetaData>(JsonConvert.SerializeObject(jsonTable));
            table.Id = sqlTable.Id;
            AnalyticsFactsMetaDataRepository.Update(table);

            var sqlFields = DashboardContext.AnalyticsFactsFieldsMetaDatas.ToDictionary(d => d.FieldCode, a => a);
            foreach (var jsonField in jsonTable.AnalyticsFactsFieldsMetaDatas)
            {
                UpdateField(jsonField, sqlFields.ContainsKey(jsonField.FieldCode) ? sqlFields[jsonField.FieldCode] : null, table.Id);
            }
        }

        private void UpdateField(Logitude.DashboardModule.MetaDataTool.Models.FieldModels.AnalyticsFactsFieldsMetaData jsonField, AnalyticsFactsFieldsMetaData analyticsFactsFieldsMetaData, string tableId)
        {
            if (analyticsFactsFieldsMetaData == null)
            {
                AddField(jsonField, tableId);
                return;
            }
            var field = JsonConvert.DeserializeObject<AnalyticsFactsFieldsMetaData>(JsonConvert.SerializeObject(jsonField));
            field.Id = analyticsFactsFieldsMetaData.Id;
            AnalyticsFactsFieldsMetaDataRepository.Update(field);
        }

        private void AddField(Logitude.DashboardModule.MetaDataTool.Models.FieldModels.AnalyticsFactsFieldsMetaData jsonField, string tableId)
        {
            var field = JsonConvert.DeserializeObject<AnalyticsFactsFieldsMetaData>(JsonConvert.SerializeObject(jsonField));
            field.Id = IdCounter.GetNumber("AnalyticsFactsFieldsMetaData", 0);
            field.AnalyticsFactsMetaDataId = tableId;
            AnalyticsFactsFieldsMetaDataRepository.Add(field);
        }

        private void AddNewTableToDB(Logitude.DashboardModule.MetaDataTool.Models.AnalyticsFactsMetaData jsonTable)
        {
            var table = JsonConvert.DeserializeObject<AnalyticsFactsMetaData>(JsonConvert.SerializeObject(jsonTable));
            table.Id = IdCounter.GetNumber("AnalyticsFactsMetaData", 0);
            AnalyticsFactsMetaDataRepository.Add(table);

            foreach (var item in jsonTable.AnalyticsFactsFieldsMetaDatas)
            {
                AddField(item, table.Id);
            }
        }

        private static string GetProjectPath()
        {
            return "AnalyticsEntityFiles";
        }

        private Logitude.DashboardModule.MetaDataTool.Models.AnalyticsFactsMetaData BuildFileJson(string fileName)
        {
            StreamReader r = new StreamReader(fileName);
            var analyticsFactsMetaData = JsonConvert.DeserializeObject<Logitude.DashboardModule.MetaDataTool.Models.AnalyticsFactsMetaData>(r.ReadToEnd());
            return analyticsFactsMetaData;
        }

    }
}