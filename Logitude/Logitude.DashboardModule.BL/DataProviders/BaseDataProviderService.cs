using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.Data.Repositories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.TreeFilterQuery;
using Logitude.Server.Tools.TreeFilterQuery.Interpreter;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DashboardModule.BL.DataProviders
{
    public abstract class BaseDataProviderService
    {
        internal WidgetPM _Widget;
        internal AnalyticsFactsMetaData _Entity;
        internal Dictionary<string,AnalyticsFactsFieldsMetaData> _EntityFields;

        protected BaseDataProviderService(WidgetPM widget, AnalyticsFactsMetaData entity)
        {
            this._Widget = widget;
            this._Entity = entity;
            FillEntityFields();
        }

        public abstract List<SeriesMeasure> GetWidgetData();

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
            var measureField = _EntityFields.ContainsKey(measure.MeasureFieldId) ? _EntityFields[measure.MeasureFieldId] : throw new Exception($"Meta Data Field '{measure.MeasureFieldId}' not found");

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
                groupByField = ConverDateByDateGroupCode( groupBy);
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
            return $@"select  {top}
                            {Label} as Label,
                            {groupByField} as GroupById,
                            CAST({measure.MeasureCode}(IIF(data.{measureField.FieldCode} is null , '0' , data.{measureField.FieldCode})) AS DECIMAL(32,2) ) as Value From 
                            ({resultQueryable.ToQueryStringWithParameter()}) as data
                            {join}
                            group by {Label},{groupByField} {sortBy}";
        }

        private object CreateSortBy()
        {
            if(_Widget.SortBy == null)
            {
                return $" order by Label {_Widget.SortDirection}";
            }
            return $" order by Value {_Widget.SortDirection}";
        }

        private string ConverDateByDateGroupCode( AnalyticsFactsFieldsMetaData groupBy)
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
            var analyticsFactsFieldsMetaDataRepository = new AnalyticsFactsFieldsMetaDataRepository(0);
            var entityFields = analyticsFactsFieldsMetaDataRepository.GetAll(0).Where(e=>e.AnalyticsFactsMetaDataId == _Entity.Id).ToList();
            _EntityFields = entityFields.ToDictionary(e => e.Id, e => e);
        }

        
    }


}
