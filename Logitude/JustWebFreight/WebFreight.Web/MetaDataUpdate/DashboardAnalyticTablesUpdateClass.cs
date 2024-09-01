using Logitude.DashboardModule.Data;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.Data.Repositories;
using Logitude.DashboardModule.MetaData.AnalyticsEntityFiles;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Newtonsoft.Json;
using Simplog.Data.InfrastructureModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            var analyticTables = new AnalyticsMetadatas<Logitude.DashboardModule.MetaDataTool.Models.AnalyticsFactsMetaData>().GetAllTables();
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
                UpdateAnalyticsFactsMetaData(jsonTable, analyticsFactsMetaDatas.ContainsKey(jsonTable.Name) ? analyticsFactsMetaDatas[jsonTable.Name] : null);
            }
        }

        private Dictionary<string, AnalyticsFactsMetaData> GetAnalyticsFactsMetaDatasFromDataBase()
        {
            return DashboardContext.AnalyticsFactsMetaDatas.AsNoTracking().GroupBy(d => d.Name).ToDictionary(g => g.Key, a => a.FirstOrDefault());
        }

        private void UpdateAnalyticsFactsMetaData(Logitude.DashboardModule.MetaDataTool.Models.AnalyticsFactsMetaData jsonTable, AnalyticsFactsMetaData sqlTable)
        {
            if (sqlTable == null) AddNewTableToDB(jsonTable);
            else if (jsonTable.HashString != sqlTable.HashString) UpdateTable(jsonTable, sqlTable);
        }

        private void UpdateTable(Logitude.DashboardModule.MetaDataTool.Models.AnalyticsFactsMetaData jsonTable, AnalyticsFactsMetaData sqlTable)
        {
            var table = JsonConvert.DeserializeObject<AnalyticsFactsMetaData>(JsonConvert.SerializeObject(jsonTable));
            table.Id = sqlTable.Id;
            table.SearchFields = BuildTableFieldSearchField(table);
            AnalyticsFactsMetaDataRepository.Update(table);

            var sqlFields = DashboardContext.AnalyticsFactsFieldsMetaDatas.AsNoTracking().Where(e=>e.AnalyticsFactsMetaDataId == table.Id).GroupBy(d => d.FieldCode).ToDictionary(g => g.Key, a => a.FirstOrDefault());
            foreach (var jsonField in jsonTable.AnalyticsFactsFieldsMetaDatas)
            {
                UpdateField(jsonField, sqlFields.ContainsKey(jsonField.FieldCode) ? sqlFields[jsonField.FieldCode] : null, table.Id);
            }
           // RemoveDeletedFields(jsonTable.AnalyticsFactsFieldsMetaDatas, sqlFields);
        }

        private void RemoveDeletedFields(List<Logitude.DashboardModule.MetaDataTool.Models.FieldModels.AnalyticsFactsFieldsMetaData> analyticsFactsFieldsMetaDatas, Dictionary<string, AnalyticsFactsFieldsMetaData> sqlFields)
        {

            var fieldDictionary = analyticsFactsFieldsMetaDatas.GroupBy(d => d.FieldCode).ToDictionary(g => g.Key, a => a.FirstOrDefault());
            foreach (var item in sqlFields)
            {
                if (!fieldDictionary.ContainsKey(item.Key)) AnalyticsFactsFieldsMetaDataRepository.Remove(item.Value);
            }
        }

        private void UpdateField(Logitude.DashboardModule.MetaDataTool.Models.FieldModels.AnalyticsFactsFieldsMetaData jsonField, AnalyticsFactsFieldsMetaData analyticsFactsFieldsMetaData, string tableId)
        {
            if (analyticsFactsFieldsMetaData == null)
            {
                AddField(jsonField, tableId);
                return;
            }
            AnalyticsFactsFieldsMetaData field = ConvertJsonFieldToSqlField(jsonField, analyticsFactsFieldsMetaData.Id, tableId);
            AnalyticsFactsFieldsMetaDataRepository.Update(field);
        }

        private void AddField(Logitude.DashboardModule.MetaDataTool.Models.FieldModels.AnalyticsFactsFieldsMetaData jsonField, string tableId)
        {

            AnalyticsFactsFieldsMetaData field = ConvertJsonFieldToSqlField(jsonField, IdCounter.GetNumber("AnalyticsFactsFieldsMetaData", 0), tableId);
            AnalyticsFactsFieldsMetaDataRepository.Add(field);
        }

        private AnalyticsFactsFieldsMetaData ConvertJsonFieldToSqlField(Logitude.DashboardModule.MetaDataTool.Models.FieldModels.AnalyticsFactsFieldsMetaData jsonField, string fieldId, string tableId)
        {
            var field = JsonConvert.DeserializeObject<AnalyticsFactsFieldsMetaData>(JsonConvert.SerializeObject(jsonField));
            field.Id = fieldId;
            field.AnalyticsFactsMetaDataId = tableId;
            field.SearchFields = BuildFieldSearchField(field);
            return field;
        }

        private void AddNewTableToDB(Logitude.DashboardModule.MetaDataTool.Models.AnalyticsFactsMetaData jsonTable)
        {
            var table = JsonConvert.DeserializeObject<AnalyticsFactsMetaData>(JsonConvert.SerializeObject(jsonTable));
            table.Id = IdCounter.GetNumber("AnalyticsFactsMetaData", 0);
            table.SearchFields = BuildTableFieldSearchField(table);
            AnalyticsFactsMetaDataRepository.Add(table);

            foreach (var item in jsonTable.AnalyticsFactsFieldsMetaDatas)
            {
                AddField(item, table.Id);
            }
        }

        private string BuildTableFieldSearchField(AnalyticsFactsMetaData table)
        {
            string mySearchFields = "";
            MethodHelper.AddToSearchFields(ref mySearchFields, table.Name);
            return mySearchFields;
        }

        private string BuildFieldSearchField(AnalyticsFactsFieldsMetaData field)
        {
            string mySearchFields = "";
            MethodHelper.AddToSearchFields(ref mySearchFields, field.FieldCode);
            MethodHelper.AddToSearchFields(ref mySearchFields, field.DisplayName);
            MethodHelper.AddToSearchFields(ref mySearchFields, field.DisplayNamePlural);
            return mySearchFields;
        }

    }
}