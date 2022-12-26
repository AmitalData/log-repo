using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.TreeFilterQuery;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;

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
                var seriesMeasure = new SeriesMeasure();

                seriesMeasure.Name = GetSeriesName(measure);
                seriesMeasure.RenderAs = measure.RenderAs;
                seriesMeasure.MeasureFieldId = measure.MeasureFieldId;
                seriesMeasure.SeriesMeasureVulues = GetSeriesMeasureVulues(query, measure);
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
            List<SeriesMeasureVulue> seriesMeasureVulues = resultQueryables.ToList();
            if (groupBy.DataTypeCode == "Date" || groupBy.DataTypeCode == "DateTime")
            {
                seriesMeasureVulues = FillDateGaps(seriesMeasureVulues);
                UpdateDateString(seriesMeasureVulues);
            }
            return seriesMeasureVulues;
        }

        private string CreateQuery<T>(WidgetMeasurePM measure, AnalyticsFactsFieldsMetaData groupBy, AnalyticsFactsFieldsMetaData measureField, IQueryable<T> resultQueryable)
        {
            var groupByField = $"data.{groupBy.FieldCode}";
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

            var value = GetValueQuery(measure.MeasureCode, measureField);
            return $@"select  {top}
                            {Label} as Label,
                            {groupByField} as GroupById,
                            {value} as Value From 
                            ({resultQueryable.ToQueryStringWithParameter()}) as data
                            {join}
                            group by {Label},{groupByField} {sortBy}";
        }

        private List<SeriesMeasureVulue> FillDateGaps(List<SeriesMeasureVulue> seriesMeasureVulues)
        {
            if (_Widget.TypeCode == "pie" || _Widget.TypeCode == "donut") return seriesMeasureVulues;
            if (seriesMeasureVulues == null || seriesMeasureVulues.Count == 0 || seriesMeasureVulues.Count == 1) return seriesMeasureVulues;

            var result = FillAllDateGaps(seriesMeasureVulues);

            if (_Widget.SortBy != null) return result;
            return _Widget.SortDirection == "asc" ? result.OrderBy(x => x.Label).ToList() : result.OrderByDescending(x => x.Label).ToList();
        }

        private List<SeriesMeasureVulue> FillAllDateGaps(List<SeriesMeasureVulue> seriesMeasureVulues)
        {
            var dates = new List<string>();
            if (_Widget.DateGroupCode == "Quarter") dates = BuildQuarterDates(seriesMeasureVulues);
            else dates = BuildDateList(seriesMeasureVulues);

            var result = new List<SeriesMeasureVulue>();
            foreach (var item in dates)
            {
                var seriesMeasureVulue = seriesMeasureVulues.FirstOrDefault(x => x.Label == item);
                if (seriesMeasureVulue == null)
                {
                    seriesMeasureVulue = new SeriesMeasureVulue
                    {
                        Label = item,
                        GroupById = item
                    };
                }
                result.Add(seriesMeasureVulue);
            }
            return result;
        }

        private List<string> BuildQuarterDates(List<SeriesMeasureVulue> seriesMeasureVulues)
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

        private List<string> BuildDateList(List<SeriesMeasureVulue> seriesMeasureVulues)
        {
            List<DateTime> listDates = seriesMeasureVulues.Select(x => DateTime.ParseExact(x.GroupById, "yyyy/MM/dd", null)).ToList();
            DateTime minDate = listDates.Min();
            DateTime maxDate = listDates.Max();

            var allDates = new List<DateTime>();
            for (; maxDate.CompareTo(minDate) > 0; minDate = ApplyDateAddition(minDate))
            {
                allDates.Add(minDate);
            }
            if (_Widget.MaximumGrouping == null) allDates.Select(x => x.ToString("yyyy/MM/dd")).ToList();
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
