using Logitude.DashboardModule.BL.DataProviders.Models;
using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.TreeFilterQuery;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Logitude.DashboardModule.BL.DataProviders.WidgetsDataProviders
{
    public class ChartDataProviderService : BaseDataProviderService
    {
        public ChartDataProviderService(WidgetPM widget, AnalyticsFactsMetaData entity) : base(widget, entity)
        {

        }

        public List<SeriesMeasure> GetData<T>(IQueryable<T> query)
        {

            var seriesMeasures = new List<SeriesMeasure>();
            foreach (var measure in _Widget.WidgetMeasures)
            {
                var seriesMeasure = new SeriesMeasure
                {
                    Name = GetSeriesName(measure),
                    RenderAs = measure.RenderAs,
                    MeasureFieldId = measure.MeasureFieldId,
                    Values = GetSeriesMeasureVulues(query, measure)
                };
                seriesMeasures.Add(seriesMeasure);
            }
            return seriesMeasures;
        }

        private string GetSeriesName(WidgetMeasurePM measure)
        {
            AnalyticsFactsFieldsMetaData measureField = null;

            if (measure.MeasureFieldId != null)
                measureField = _EntityFields.ContainsKey(measure.MeasureFieldId) ? _EntityFields[measure.MeasureFieldId] : throw new Exception($"Meta Data Field '{measure.MeasureFieldId}' not found");
            var measureCodeName = GetMeasureCodeName(measure.MeasureCode);
            if (measureField != null) return $"{measureCodeName} of {measureField.DisplayName}";
            return $"{measureCodeName} of {_Entity.Name}";
        }

        private string GetMeasureCodeName(string measureCode)
        {
            switch (measureCode)
            {
                case "Avg": return "Average";
                case "Count": return "Count";
                case "Max": return "Max";
                case "Min": return "Min";
                case "Sum": return "Sum";
            }
            return measureCode;
        }

        private List<SeriesMeasureValue> GetSeriesMeasureVulues<T>(IQueryable<T> query, WidgetMeasurePM measure)
        {

            AnalyticsFactsFieldsMetaData measureField = null;
            if (measure.MeasureFieldId != null)
                measureField = _EntityFields.ContainsKey(measure.MeasureFieldId) ? _EntityFields[measure.MeasureFieldId] : throw new Exception($"Meta Data Field '{measure.MeasureFieldId}' not found");

            TreeFilterQueryService treeFilterQueryService = new TreeFilterQueryService();
            var resultQueryable = treeFilterQueryService.Apply(query, new TreeFilterQueryArgs() { AdditionalTreeFilter = _Widget.Filters, ObjectTableName = "", Tenant = 0 });
            string queryString = BuildQuery(measure, measureField, resultQueryable);
            var conterxt = DashboardContext.GetContext(0);
            var resultQueryables = conterxt.GetActiveDbContext().Database.SqlQuery<SeriesMeasureValue>(queryString, new object[0]).AsQueryable();
            List<SeriesMeasureValue> seriesMeasureVulues = resultQueryables.ToList();
            seriesMeasureVulues = FillGroupDateGaps(seriesMeasureVulues);
            return seriesMeasureVulues;
        }

        private string BuildQuery<T>(WidgetMeasurePM measure, AnalyticsFactsFieldsMetaData measureField, IQueryable<T> resultQueryable)
        {
            if (IsMultiGroup()) return CreateMultiGroupQuery<T>(measure, measureField, resultQueryable);
            return CreateQuery<T>(measure, measureField, resultQueryable);
        }

        private bool IsMultiGroup()
        {
            return !string.IsNullOrEmpty(_Widget.SecondaryGroupById);
        }


        private string CreateMultiGroupQuery<T>(WidgetMeasurePM measure, AnalyticsFactsFieldsMetaData measureField, IQueryable<T> resultQueryable)
        {
            var groupByParts = BuildGroupsParts();
            var value = GetValueQuery(measure.MeasureCode, measureField);
            var searchQuery = GetSearchQuery<T>(resultQueryable);
            return $@"select  {BuildGroupsSelectQuery(groupByParts)}
                              {value} as Value 
                              From {_Entity.TableName} as data
                              INNER JOIN (
                                   {CreateFirstGroupSortedQuery<T>(measure, measureField, resultQueryable, groupByParts[0])}
                              ) sortedData On {groupByParts[0].GroupById} = sortedData.Id
                              {BuildGroupsJoinQuery(groupByParts)}
                              WHERE {searchQuery}
                              group by {BuildGroupsGroupQuery(groupByParts)},sortedData.indx order by sortedData.indx";
        }

        private string GetSearchQuery<T>(IQueryable<T> resultQueryable)
        {
            var s = resultQueryable.ToQueryStringWithParameter();
            var after = "WHERE ";
            int ix = s.IndexOf(after);
            s = s.Substring(ix + after.Length);
            s = " " + s;

            int pFrom = s.IndexOf("[") + "[".Length;
            int pTo = s.IndexOf("].");
            String replacedString = s.Substring(pFrom, pTo - pFrom);

            string replace = "data";
            return Regex.Replace(s, replacedString, replace);
        }

        private string CreateFirstGroupSortedQuery<T>(WidgetMeasurePM measure, AnalyticsFactsFieldsMetaData measureField, IQueryable<T> resultQueryable, GroupByFieldQueryParts part)
        {
            var top = _Widget.MaximumGrouping.HasValue ? $"top({ _Widget.MaximumGrouping})" : "";
            var sortBy = $"order by g1.{GetSortByField()} {_Widget.SortDirection}";
            var value = GetValueQuery(measure.MeasureCode, measureField);

            return $@"select  {top}
                              g1.GroupById as Id,
                              row_number() over ({sortBy}) as indx
                              From (
                                     select 
                                     {BuildSingleGroupSelectQuery(part, 1)}
                                     {value} as Value From
                                     ({resultQueryable.ToQueryStringWithParameter()}) as data
                                     {part.Join}
                                      group by {part.Label},{part.GroupById}
                              ) as g1
                              {sortBy}";
        }

        private string CreateQuery<T>(WidgetMeasurePM measure, AnalyticsFactsFieldsMetaData measureField, IQueryable<T> resultQueryable)
        {
            var groupByParts = BuildGroupsParts();
            var top = _Widget.MaximumGrouping.HasValue ? $"top({ _Widget.MaximumGrouping})" : "";

            var value = GetValueQuery(measure.MeasureCode, measureField);
            return $@"select  {top}
                              {BuildGroupsSelectQuery(groupByParts)}
                              {value} as Value From 
                              ({resultQueryable.ToQueryStringWithParameter()}) as data
                              {BuildGroupsJoinQuery(groupByParts)}
                              group by {BuildGroupsGroupQuery(groupByParts)} {CreateSortBy()}";
        }

        private string BuildGroupsGroupQuery(List<GroupByFieldQueryParts> groupByParts)
        {
            var query = "";
            foreach (var part in groupByParts)
            {
                query = $@"{query} {part.Label},{part.GroupById},";
            }
            return query.Remove(query.Length - 1);
        }

        private string BuildGroupsJoinQuery(List<GroupByFieldQueryParts> groupByParts)
        {
            var query = "";
            foreach (var part in groupByParts)
            {
                query = $@"{query}
                           {part.Join}";
            }
            return query;
        }

        private string BuildGroupsSelectQuery(List<GroupByFieldQueryParts> groupByParts)
        {
            var query = "";
            int pos = 1;
            foreach (var part in groupByParts)
            {
                query = $@"{query}
                           {BuildSingleGroupSelectQuery(part, pos)}";
                pos++;
            }
            return query;
        }

        private string BuildSingleGroupSelectQuery(GroupByFieldQueryParts part, int pos)
        {
            if (pos == 1)
            {
                return $@"{part.Label} as Label,
                          {part.GroupById} as GroupById,";
            }
            return $@"{part.Label} as LabelSec,
                          {part.GroupById} as GroupByIdSec,";
        }

        private List<GroupByFieldQueryParts> BuildGroupsParts()
        {
            List<GroupByFieldQueryParts> parts = new List<GroupByFieldQueryParts>();

            parts.Add(BuildGroupByFieldQueryParts(_Widget.GroupById, "", _Widget.DateGroupCode));
            if (!string.IsNullOrEmpty(_Widget.SecondaryGroupById))
            {
                parts.Add(BuildGroupByFieldQueryParts(_Widget.SecondaryGroupById, "1", _Widget.SecondaryDateGroupCode));
            }
            return parts;
        }

        private GroupByFieldQueryParts BuildGroupByFieldQueryParts(string groupById, string joinedlabel, string dateGroupCode)
        {
            var field = _EntityFields[groupById];
            var groupByField = $"data.{field.FieldCode}";
            string label = "";
            string join = "";
            if (field.DataTypeCode == "Date" || field.DataTypeCode == "DateTime")
            {
                groupByField = ConverDateByDateGroupCode(field, dateGroupCode);
                label = groupByField;
            }
            else if (field.DataTypeCode == "LookUp")
            {
                join = $" left join {field.JoinedTableDBName} as JoinedTable{joinedlabel} on JoinedTable{joinedlabel}.{field.JoinedTableKey} = {groupByField} ";
                label = $"JoinedTable{joinedlabel}.{field.JoinedTableDisplayField}";
            }
            return new GroupByFieldQueryParts
            {
                Label = label,
                Join = join,
                GroupById = groupByField
            };
        }

        private List<SeriesMeasureValue> FillGroupDateGaps(List<SeriesMeasureValue> seriesMeasureVulues)
        {
            var groupBy = _EntityFields[_Widget.GroupById];
            if ((groupBy.DataTypeCode == "Date" || groupBy.DataTypeCode == "DateTime"))/* && _Widget.SortBy == null*/
            {
                seriesMeasureVulues = FillDateGaps(seriesMeasureVulues);
                UpdateDateString(seriesMeasureVulues);
            }
            return seriesMeasureVulues;
        }

        private List<SeriesMeasureValue> FillDateGaps(List<SeriesMeasureValue> seriesMeasureVulues)
        {
            if (_Widget.TypeCode == "pie" || _Widget.TypeCode == "donut") return seriesMeasureVulues;
            if (seriesMeasureVulues == null || seriesMeasureVulues.Count == 0 || seriesMeasureVulues.Count == 1) return seriesMeasureVulues;

            var result = FillAllDateGaps(seriesMeasureVulues);

            if (_Widget.SortBy != null) return result;
            return _Widget.SortDirection == "asc" ? result.OrderBy(x => x.Label).ToList() : result.OrderByDescending(x => x.Label).ToList();
        }

        private List<SeriesMeasureValue> FillAllDateGaps(List<SeriesMeasureValue> seriesMeasureVulues)
        {
            var dates = new List<string>();
            if (_Widget.DateGroupCode == "Quarter") dates = BuildQuarterDates(seriesMeasureVulues);
            else dates = BuildDateList(seriesMeasureVulues);

            var result = new List<SeriesMeasureValue>();
            foreach (var item in dates)
            {
                AddDateGapsValue(seriesMeasureVulues, result, item);

            }
            return result;
        }

        private static void AddDateGapsValue(List<SeriesMeasureValue> seriesMeasureVulues, List<SeriesMeasureValue> result, string label)
        {
            var values = seriesMeasureVulues.Where(x => x.Label == label).ToList();
            if (values.Any())
            {
                result.AddRange(values);
                return;
            }

            var seriesMeasureVulue = new SeriesMeasureValue
            {
                Label = label,
                GroupById = label
            };
            result.Add(seriesMeasureVulue);
        }

        private List<string> BuildQuarterDates(List<SeriesMeasureValue> seriesMeasureVulues)
        {
            var listDates = seriesMeasureVulues.Select(x => x.Label).OrderBy(x => x).ToList();
            var minDate = listDates.FirstOrDefault();
            var maxDate = listDates.LastOrDefault();

            int currentYear = int.Parse(minDate.Split('/')[0]);
            int currentQuarter = int.Parse(minDate.Split('/')[1].Remove(0, 1));

            int endYear = int.Parse(maxDate.Split('/')[0]);
            int endQuarter = int.Parse(maxDate.Split('/')[1].Remove(0, 1));

            var allDates = new List<string>();
            for (; currentYear <= endYear; currentYear++)
            {
                if (allDates.Any()) currentQuarter = 1;
                for (; currentQuarter <= 4; currentQuarter++)
                {
                    if (currentYear == endYear && currentQuarter == endQuarter) break;
                    allDates.Add(currentYear + "/Q" + currentQuarter);
                }
            }
            return allDates;
        }

        private List<string> BuildDateList(List<SeriesMeasureValue> seriesMeasureVulues)
        {
            List<DateTime> listDates = seriesMeasureVulues.Where(x => x.GroupById != null).Select(x => DateTime.ParseExact(x.GroupById, "yyyy/MM/dd", null)).ToList();
            DateTime minDate = listDates.Min();
            DateTime maxDate = listDates.Max();

            var allDates = new List<DateTime>();
            for (; maxDate.CompareTo(minDate) >= 0; minDate = ApplyDateAddition(minDate))
            {
                allDates.Add(minDate);
            }
            if (_Widget.MaximumGrouping == null) return allDates.Select(x => x.ToString("yyyy/MM/dd")).ToList();

            allDates = _Widget.SortDirection == "asc" ? allDates.OrderBy(x => x).ToList() : allDates.OrderByDescending(x => x).ToList();
            return allDates.Take(_Widget.MaximumGrouping.Value).Select(x => x.ToString("yyyy/MM/dd")).ToList();
        }

        private DateTime ApplyDateAddition(DateTime minDate)
        {
            switch (_Widget.DateGroupCode)
            {
                case "Day": return minDate.AddDays(1);
                case "Month": return minDate.AddMonths(1);
                case "Year": return minDate.AddYears(1);
                default: return minDate.AddDays(1);
            }
        }
    }
}
