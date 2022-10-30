using Logitude.DashboardModule.BL.APIDataContract;
using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.Data.Repositories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.TreeFilterQuery;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Linq;

namespace Logitude.DashboardModule.BL.DataProviders
{
    public abstract class BaseDataProviderService
    {
        internal WidgetPM _Widget;
        internal AnalyticsFactsMetaData _Entity;
        internal Dictionary<string, AnalyticsFactsFieldsMetaData> _EntityFields;
        private AnalyticsFactsFieldsMetaDataRepository analyticsFactsFieldsMetaDataRepository;
        private string[] Months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        protected BaseDataProviderService(WidgetPM widget, AnalyticsFactsMetaData entity)
        {
            this._Widget = widget;
            this._Entity = entity;
            analyticsFactsFieldsMetaDataRepository = new AnalyticsFactsFieldsMetaDataRepository(0);
            FillEntityFields();
        }


        private void FillEntityFields()
        {
            var entityFields = analyticsFactsFieldsMetaDataRepository.GetAll(0).Where(e => e.AnalyticsFactsMetaDataId == _Entity.Id).ToList();
            _EntityFields = entityFields.ToDictionary(e => e.Id, e => e);
        }

        protected void UpdateDateString(List<SeriesMeasureVulue> results)
        {
            foreach (var item in results)
            {
                switch (_Widget.DateGroupCode)
                {
                    case "Day":
                        item.Label = GetDayFormat(item.Label);
                        break;
                    case "Month":
                        item.Label = GetMonthFormat(item.Label);
                        break;
                    case "Year":
                        item.Label = GetYearFormat(item.Label);
                        break;
                    default:
                        break;
                }

            }
        }

        private string GetDayFormat(string label)
        {
            var dateparts = label.Split('/');
            var month = GetMonthName(dateparts[1]);

            return $"{dateparts[0]}/{month}/{dateparts[2]}";
        }
        private string GetMonthFormat(string label)
        {
            var dateparts = label.Split('/');
            var month = GetMonthName(dateparts[1]);

            return $"{dateparts[0]}/{month}";
        }
        private string GetYearFormat(string label)
        {
            var dateparts = label.Split('/');
            return $"{dateparts[0]}";
        }

        private string GetMonthName(string v)
        {
            var month = int.Parse(v);
            return Months[month - 1];
        }

        protected string GetValueQuery(string measureCode, AnalyticsFactsFieldsMetaData measureField)
        {
            if (measureCode != "Count") return $"CAST({measureCode}(IIF(data.{measureField.FieldCode} is null , '0' , data.{measureField.FieldCode})) AS DECIMAL(32,2))";
            var key = "Id";
            return $"CAST({measureCode}(data.{key}) AS DECIMAL(32, 2))";
        }

        protected object CreateSortBy()
        {
            if (_Widget.SortBy == null)
            {
                return $" order by Label {_Widget.SortDirection}";
            }
            return $" order by Value {_Widget.SortDirection}";
        }

        protected string ConverDateByDateGroupCode(AnalyticsFactsFieldsMetaData groupBy)
        {
            switch (_Widget.DateGroupCode)
            {
                case "Day":
                    return $"convert(varchar, data.{groupBy.FieldCode}, 111)";
                case "Month":
                    return $"CONVERT(varchar,DateAdd(Month, DateDiff(Month, 0, data.{groupBy.FieldCode}), 0),111)";
                case "Year":
                    return $"CONVERT(varchar,DateAdd(yy, DateDiff(yy, 0, data.{groupBy.FieldCode}), 0),111)";
                case "Quarter":
                    return $"CONCAT(CAST(Year(data.{groupBy.FieldCode}) as varchar(5)) ,'/Q',MONTH(data.{groupBy.FieldCode})/4+1)";
                default:
                    return $"convert(varchar, data.{groupBy.FieldCode}, 111)";
            }
        }



        public AnalyticData GetDataPart<T>(IQueryable<T> query, List<string> analyticTableFields, WidgetArguments widgetPartArguments)
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

        protected static IEnumerable<dynamic> DynamicListFromSql(string Sql)
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

            if (!string.IsNullOrEmpty(_Widget.GroupById))
            {
                var groupBy = _EntityFields.ContainsKey(_Widget.GroupById) ? _EntityFields[_Widget.GroupById] : throw new Exception($"Meta Data Field '{_Widget.GroupById}' not found");
                columns.Add(groupBy);
            }

            if (!string.IsNullOrEmpty(widgetPartArguments.MeasureFieldId))
            {
                var measure = _EntityFields.ContainsKey(widgetPartArguments.MeasureFieldId) ? _EntityFields[widgetPartArguments.MeasureFieldId] : throw new Exception($"Meta Data Field '{widgetPartArguments.MeasureFieldId}' not found");
                columns.Add(measure);
            }

            if (analyticTableFields != null && analyticTableFields.Count != 0)
            {
                columns.AddRange(analyticsFactsFieldsMetaDataRepository.GetAll(0).Where(e => e.AnalyticsFactsMetaDataId == _Entity.Id && analyticTableFields.Contains(e.FieldCode)).ToList());

            }
            return columns.GroupBy(x => x.FieldCode).Select(x => x.FirstOrDefault()).ToList();
        }

        private string CreateQuery<T>(IQueryable<T> resultQueryable, List<AnalyticsFactsFieldsMetaData> columns, WidgetArguments widgetPartArguments)
        {
            AnalyticsFactsFieldsMetaData groupByField = null;
            if (_Widget.GroupById != null)
            {
                groupByField = _EntityFields[_Widget.GroupById];
                if (!columns.Any(x => x.FieldCode == groupByField.FieldCode)) columns.Insert(0, groupByField);
            }

            return $@"select {BuildAnalyticTableFieldsSelectQuery(columns)}
                     From ({resultQueryable.ToQueryStringWithParameter()}) as data 
                     {BuildQueryJoins(columns)}
                     {BuildQueryStatment(groupByField, widgetPartArguments.GroupByValue)} ";
        }

        private string BuildQueryStatment(AnalyticsFactsFieldsMetaData groupBy, string groupByValue)
        {
            if (groupBy == null) return "";
            var query = "Where ";

            var fieldCode = "data." + groupBy.FieldCode;
            if (groupByValue == null) return $@"{query} {fieldCode} IS NULL";
            if (groupBy.DataTypeCode == "LookUp") return $@"{query} {fieldCode} = '{groupByValue}'";

            var dateParts = groupByValue.Split('/');
            string date;
            switch (_Widget.DateGroupCode)
            {
                case "Day":
                    date = $@"{dateParts[0]}-{dateParts[1]}-{dateParts[2]}";
                    return $"{query} {fieldCode} >= '{date}' and {fieldCode} <='{date} 23:59:59.999'";

                case "Month":
                    int daysInMonth = DateTime.DaysInMonth(int.Parse(dateParts[0]), int.Parse(dateParts[1]));
                    date = $@"{dateParts[0]}-{dateParts[1]}-";
                    return $"{query} {fieldCode} >= '{date}01' and {fieldCode} <='{date}{daysInMonth} 23:59:59.999'";

                case "Year":
                    date = $@"{dateParts[0]}-";
                    return $"{query} {fieldCode} >= '{date}01-31' and {fieldCode} <='{date}12-31 23:59:59.999'";

                case "Quarter":
                    return query + GetQuarterGroupStatment(fieldCode, dateParts);

                default:
                    date = $@"{dateParts[0]}-{dateParts[1]}";
                    return $"{query} {fieldCode} >= '{date}' and {fieldCode} <='{date} 23:59:59.999'";
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

        private string BuildQueryJoins(List<AnalyticsFactsFieldsMetaData> columns)
        {
            var joinTableFields = columns.Where(x => x.DataTypeCode == "LookUp");
            if (!joinTableFields.Any()) return "";
            var query = "";
            foreach (var field in joinTableFields)
            {
                query = $@"{query}
                           left join {field.JoinedTableDBName} as {BuildJoinTableName(field)} on {BuildJoinTableName(field)}.{field.JoinedTableKey} = {field.FieldCode}";
            }
            return query;
        }

        private string BuildAnalyticTableFieldsSelectQuery(List<AnalyticsFactsFieldsMetaData> analyticTableFields)
        {
            var query = "";
            foreach (var field in analyticTableFields)
            {
                if (field.DataTypeCode == "LookUp") query = $@"{query}, {BuildJoinTableName(field)}.{field.JoinedTableDisplayField} as {field.FieldCode} ";
                else query = $@"{query}, data.{field.FieldCode}";
            }
            return query.TrimStart(',');
        }

        private string BuildJoinTableName(AnalyticsFactsFieldsMetaData field)
        {
            return $@"{field.JoinedTableName}{field.FieldCode}";
        }
    }

}
