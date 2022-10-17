using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.TreeFilterQuery;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                seriesMeasure.Name = GetSeriesName(measure) ;
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
            if (measureField != null)
                return $"{measureCodeName} of {measureField.DisplayName}";


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
            var results = resultQueryables.ToList();
            if (groupBy.DataTypeCode == "Date" || groupBy.DataTypeCode == "DateTime")
            {
                UpdateDateString(results);
            }
            return results;
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

    }
}
