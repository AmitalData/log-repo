using Logitude.DashboardModule.BL.APIDataContract;
using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.Data.Repositories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.TreeFilterQuery;
using Logitude.Server.Tools.TreeFilterQuery.Interpreter;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DashboardModule.BL.DataProviders
{
    public abstract class BaseDataProviderService
    {
        internal WidgetPM _Widget;
        internal AnalyticsFactsMetaData _Entity;
        internal Dictionary<string, AnalyticsFactsFieldsMetaData> _EntityFields;
        private AnalyticsFactsFieldsMetaDataRepository analyticsFactsFieldsMetaDataRepository;

        protected BaseDataProviderService(WidgetPM widget, AnalyticsFactsMetaData entity)
        {
            this._Widget = widget;
            this._Entity = entity;
            analyticsFactsFieldsMetaDataRepository = new AnalyticsFactsFieldsMetaDataRepository(0);
            FillEntityFields();
        }

        public abstract List<SeriesMeasure> GetWidgetData();
        public abstract AnalyticData GetWidgetDataPart(WidgetArguments widgetPartArguments);

        public List<SeriesMeasure> GetData<T>(IQueryable<T> query)
        {

            var seriesMeasures = new List<SeriesMeasure>();
            foreach (var measure in _Widget.WidgetMeasures)
            {
                var seriesMeasure = new SeriesMeasure();
                seriesMeasure.MeasureFieldId = measure.MeasureFieldId;
                seriesMeasure.SeriesMeasureVulues = GetSeriesMeasureVulues(query, measure);
                seriesMeasures.Add(seriesMeasure);
            }


            return seriesMeasures;
        }

        private List<SeriesMeasureVulue> GetSeriesMeasureVulues<T>(IQueryable<T> query, WidgetMeasurePM measure)
        {

            var groupBy = _EntityFields.ContainsKey(_Widget.GroupById) ? _EntityFields[_Widget.GroupById] : throw new Exception($"Meta Data Field '{_Widget.GroupById}' not found");
            AnalyticsFactsFieldsMetaData measureField = null;
            if (measure.MeasureFieldId != null)
            measureField = _EntityFields.ContainsKey(measure.MeasureFieldId) ? _EntityFields[measure.MeasureFieldId] : throw new Exception($"Meta Data Field '{measure.MeasureFieldId}' not found");

            TreeFilterQueryService treeFilterQueryService = new TreeFilterQueryService();
            var resultQueryable = treeFilterQueryService.Apply(query, new TreeFilterQueryArgs() { AdditionalTreeFilter = _Widget.Filters, ObjectTableName = "", Tenant = 0 });
            string querys = CreateQuery(measure, groupBy, measureField, resultQueryable);
            var conterxt = DashboardContext.GetContext(0);
            var resultQueryables = conterxt.GetActiveDbContext().Database.SqlQuery<SeriesMeasureVulue>(querys, new object[0]).AsQueryable();

            return resultQueryables.ToList();
        }

        private string CreateQuery<T>(WidgetMeasurePM measure, AnalyticsFactsFieldsMetaData groupBy, AnalyticsFactsFieldsMetaData measureField, IQueryable<T> resultQueryable)
        {
            var groupByField = $"{groupBy.FieldCode}";
            if (groupBy.DataTypeCode == "Date" || groupBy.DataTypeCode == "DateTime")
            {
                groupByField = ConverDateByDateGroupCode(groupBy);
            }
            var Label = groupByField;
            var join = "";
            if (groupBy.DataTypeCode == "LookUp")
            {
                join = $" left join {groupBy.JoinedTableDBName} as JoinedTable on JoinedTable.{groupBy.JoinedTableKey} = {groupByField} ";
                Label = $"JoinedTable.{groupBy.JoinedTableDisplayField}";
            }
            var sortBy = CreateSortBy();
            var top = "";
            if (_Widget.MaximumGrouping.HasValue)
                top = $"top({ _Widget.MaximumGrouping})";

            var value = GetValueQuery(measure.MeasureCode,measureField);
            return $@"select  {top}
                            {Label} as Label,
                            {groupByField} as GroupById,
                            {value} as Value From 
                            ({resultQueryable.ToQueryStringWithParameter()}) as data
                            {join}
                            group by {Label},{groupByField} {sortBy}";
        }

        private string GetValueQuery(string measureCode, AnalyticsFactsFieldsMetaData measureField)
        {
            
            if (measureCode != "Count")
                return $"CAST({measureCode}(IIF(data.{measureField.FieldCode} is null , '0' , data.{measureField.FieldCode})) AS DECIMAL(32,2))";
            var key = _EntityFields.First().Value.FieldCode;
            return $"CAST({measureCode}(data.{key}) AS DECIMAL(32, 2))";
            



        }

        private object CreateSortBy()
        {
            if (_Widget.SortBy == null)
            {
                return $" order by Label {_Widget.SortDirection}";
            }
            return $" order by Value {_Widget.SortDirection}";
        }

        private string ConverDateByDateGroupCode(AnalyticsFactsFieldsMetaData groupBy)
        {
            switch (_Widget.DateGroupCode)
            {
                case "Day":
                    return $"CONCAT(CAST(Year(Data.{groupBy.FieldCode}) as varchar(5)) ,'/',DATENAME(MONTH,Data.{groupBy.FieldCode} ),'/' ,CAST(DAY(Data.{groupBy.FieldCode}) as varchar(3) ))";
                case "Month":
                    return $"CONCAT(CAST(Year(Data.{groupBy.FieldCode}) as varchar(5)) ,'/',DATENAME(MONTH,Data.{groupBy.FieldCode} ))";
                case "Year":
                    return $"CAST(YEAR(Data.{groupBy.FieldCode}) as varchar(20))";
                case "Quarter":
                    return $"CONCAT(CAST(Year(Data.{groupBy.FieldCode}) as varchar(5)) ,'/Q',MONTH(data.{groupBy.FieldCode})/4+1)";
                default:
                    return $"convert(varchar, Data.{groupBy.FieldCode}, 105)";
            }
        }

        private void FillEntityFields()
        {
            var entityFields = analyticsFactsFieldsMetaDataRepository.GetAll(0).Where(e => e.AnalyticsFactsMetaDataId == _Entity.Id).ToList();
            _EntityFields = entityFields.ToDictionary(e => e.Id, e => e);
        }


        internal AnalyticData GetDataPart<T>(IQueryable<T> query, List<string> analyticTableFields, WidgetArguments widgetPartArguments)
        {
            TreeFilterQueryService treeFilterQueryService = new TreeFilterQueryService();
            query = treeFilterQueryService.Apply(query, new TreeFilterQueryArgs() { AdditionalTreeFilter = _Widget.Filters, ObjectTableName = "", Tenant = 0 });

            var columns = BuildColumns(analyticTableFields, widgetPartArguments);
            var queryString = CreateQuery(query, columns, widgetPartArguments);


            AnalyticData analyticData = new AnalyticData();
            analyticData.DataResult = DynamicListFromSql(queryString).ToList();
            analyticData.Fields = columns;
            return analyticData;
        }

        private static IEnumerable<dynamic> DynamicListFromSql(string Sql)
        {
            var context = DashboardContext.GetContext(0);
            var db = context.GetActiveDbContext().Database;
            using (var cmd = db.Connection.CreateCommand())
            {
                cmd.CommandText = Sql;
                if (cmd.Connection.State != ConnectionState.Open) { cmd.Connection.Open(); }

                using (var dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var row = new ExpandoObject() as IDictionary<string, object>;
                        for (var fieldCount = 0; fieldCount < dataReader.FieldCount; fieldCount++)
                        {
                            row.Add(dataReader.GetName(fieldCount), dataReader[fieldCount]);
                        }
                        yield return row;
                    }
                }
            }
        }

        private List<AnalyticsFactsFieldsMetaData> BuildColumns(List<string> analyticTableFields, WidgetArguments widgetPartArguments)
        {
            var columns = new List<AnalyticsFactsFieldsMetaData>();

            var groupBy = _EntityFields.ContainsKey(_Widget.GroupById) ? _EntityFields[_Widget.GroupById] : throw new Exception($"Meta Data Field '{_Widget.GroupById}' not found");
            columns.Add(groupBy);

            var measure = _EntityFields.ContainsKey(widgetPartArguments.MeasureFieldId) ? _EntityFields[widgetPartArguments.MeasureFieldId] : throw new Exception($"Meta Data Field '{widgetPartArguments.MeasureFieldId}' not found");
            columns.Add(measure);

            if (analyticTableFields != null && analyticTableFields.Count != 0)
            {
                columns.AddRange(analyticsFactsFieldsMetaDataRepository.GetAll(0).Where(e => e.AnalyticsFactsMetaDataId == _Entity.Id && analyticTableFields.Contains(e.FieldCode)).ToList());

            }
            return columns.GroupBy(x => x.FieldCode).Select(x => x.FirstOrDefault()).ToList();
        }

        private string CreateQuery<T>(IQueryable<T> resultQueryable, List<AnalyticsFactsFieldsMetaData> columns, WidgetArguments widgetPartArguments)
        {
            var groupByField = _EntityFields[_Widget.GroupById];
            return $@"select {BuildAnalyticTableFieldsSelectQuery(columns, groupByField)}
                     From ({resultQueryable.ToQueryStringWithParameter()}) as data 
                     {BuildQueryJoins(groupByField)}
                     Where {BuildQueryStatment(groupByField, widgetPartArguments.GroupByValue)} ";
        }

        private string BuildQueryStatment(AnalyticsFactsFieldsMetaData groupBy, string groupByValue)
        {

            var fieldCode = _EntityFields[_Widget.GroupById].FieldCode;
            if (groupByValue == null) return $@"{fieldCode} IS NULL";
            if (groupBy.DataTypeCode == "LookUp") return $@"{fieldCode} = '{groupByValue}'";

            var dateParts = groupByValue.Split('/');
            string date = "";
            switch (_Widget.DateGroupCode)
            {
                case "Day":
                    date = $@"{dateParts[0]}-{DateTime.ParseExact(dateParts[1], "MMMM", CultureInfo.CurrentCulture).Month}-{ dateParts[2]}";
                    return $"{fieldCode} >= '{date}' and {fieldCode} <='{date} 23:59:59.999'";

                case "Month":
                    int daysInMonth = DateTime.DaysInMonth(int.Parse(dateParts[0]), DateTime.ParseExact(dateParts[1], "MMMM", CultureInfo.CurrentCulture).Month);
                    date = $@"{dateParts[0]}-{DateTime.ParseExact(dateParts[1], "MMMM", CultureInfo.CurrentCulture).Month}-";
                    return $"{fieldCode} >= '{date}01' and {fieldCode} <='{date}{daysInMonth} 23:59:59.999'";

                case "Year":
                    date = $@"{dateParts[0]}-";
                    return $"{fieldCode} >= '{date}01-31' and {fieldCode} <='{date}12-31 23:59:59.999'";

                case "Quarter":
                    return GetQuarterGroupStatment(fieldCode, dateParts);

                default:
                    date = $@"{dateParts[0]}-{DateTime.ParseExact(dateParts[1], "MMMM", CultureInfo.CurrentCulture).Month}-{ dateParts[2]}";
                    return $"{fieldCode} >= '{date}' and {fieldCode} <='{date} 23:59:59.999'";
            }

        }

        private string GetQuarterGroupStatment(string fieldCode, string[] dateParts)
        {
            var quarterStartMonth = (int.Parse(dateParts[1][1].ToString()) * 3) - 2;
            var quarterEndMonth = (int.Parse(dateParts[1][1].ToString()) * 3);

            var startDateDaysInMonth = DateTime.DaysInMonth(int.Parse(dateParts[0]), quarterStartMonth);
            var endDateDaysInMonth = DateTime.DaysInMonth(int.Parse(dateParts[0]), quarterEndMonth);

            var startDate = $@"{dateParts[0]}-{quarterStartMonth.ToString().PadLeft(2, '0')}-{startDateDaysInMonth}";
            var endDate = $@"{dateParts[0]}-{quarterEndMonth.ToString().PadLeft(2, '0')}-{endDateDaysInMonth}";

            return $"{fieldCode} >= '{startDate}' and {fieldCode} <='{endDate} 23:59:59.999'";
        }

        private string BuildQueryJoins(AnalyticsFactsFieldsMetaData groupByField)
        {
            if (groupByField.DataTypeCode != "LookUp") return "";
            return $@"left join {groupByField.JoinedTableDBName} as jt on jt.{groupByField.JoinedTableKey} =  {groupByField.FieldCode}";
        }

        private string BuildAnalyticTableFieldsSelectQuery(List<AnalyticsFactsFieldsMetaData> analyticTableFields, AnalyticsFactsFieldsMetaData groupByField)
        {
            var query = string.Join(",", analyticTableFields.Select(x => x.FieldCode).Where(x => x != groupByField.FieldCode).ToList());
            if (groupByField.DataTypeCode == "LookUp") query = $@"{query}, jt.{groupByField.JoinedTableDisplayField} as {groupByField.FieldCode} ";
            else query = $@"{query},{groupByField.FieldCode} ";
            return query;
        }
    }

}
