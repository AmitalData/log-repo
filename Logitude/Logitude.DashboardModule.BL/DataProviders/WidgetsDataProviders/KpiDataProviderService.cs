using Logitude.DashboardModule.BL.DataProviders.Models;
using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.Server.Tools.TreeFilterQuery;
using Simplog.Server.Infrastructure.DataContracts;
using System.Linq;
using Logitude.Server.Tools;
using System.Collections.Generic;
using System;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.DashboardModule.BL.DataProviders.WidgetsDataProviders
{
    public class KpiDataProviderService : BaseDataProviderService
    {
        private AnalyticsFactsFieldsMetaData measureField;
        private TenantRepository tenantRepository;

        public KpiDataProviderService(WidgetPM widget, AnalyticsFactsMetaData entity) : base(widget, entity)
        {
            tenantRepository = new TenantRepository(_Widget.Tenant);
        }

        internal KpiChart GetData<T>(IQueryable<T> query)
        {
            var kpiChart = new KpiChart();

            var widgetMeasureField = _Widget?.WidgetMeasures.FirstOrDefault();
            if (widgetMeasureField == null) return kpiChart;

            this.measureField = widgetMeasureField.MeasureFieldId == null ? null : _EntityFields[widgetMeasureField.MeasureFieldId];
            kpiChart.MeasureLabel = measureField?.DisplayName ?? widgetMeasureField.MeasureCode;
            kpiChart.Unit = GetUnit();
            kpiChart.Value = BuildKpiChartValue<T>(widgetMeasureField, query);
            
            if (_Widget.TimeOverTime && (measureField?.DataTypeCode == null || measureField?.DataTypeCode == "Integer" || measureField?.DataTypeCode == "Decimal"))
            {
                kpiChart.Ratio = GetRatio(kpiChart.Value, widgetMeasureField, query);
                kpiChart.ComparisonValue = BuildKpiChartComparsionValue<T>(widgetMeasureField, query)?.ToString() + " " + kpiChart.Unit;
            }
            kpiChart.Value = FormatKpiValue(kpiChart.Value, measureField?.DataTypeCode)?.ToString() + " " + kpiChart.Unit;            
            return kpiChart;
        }

        private int GetRatio<T>(object value, WidgetMeasurePM widgetMeasureField, IQueryable<T> query)
        {
            var comparsionValue = 0;
            object comparsionObject = BuildKpiChartComparsionValue<T>(widgetMeasureField, query);

            if (comparsionObject != null && comparsionObject != DBNull.Value)
            {
                comparsionValue = Convert.ToInt32(comparsionObject);
            }
            if (comparsionValue == 0) return 1;
            int diffValue = (Convert.ToInt32(value ?? 0) - comparsionValue);
            int ratio =  Convert.ToInt32(((double)diffValue / Math.Abs(comparsionValue)) * 100);
            return ratio;
        }

        private string GetUnit()
        {
            if (!measureField.HasUnit || string.IsNullOrEmpty(measureField.Unit)) return null;
            string genericUnitCode = GetGenericUnitCode();
            if (string.IsNullOrEmpty(genericUnitCode)) return measureField.Unit;
            return GetUnitByCode(genericUnitCode);
        }

        private string GetUnitByCode(string genericUnitCode)
        {
            if (genericUnitCode == "LocalCurrency") return tenantRepository.GetSingleTenant(_Widget.Tenant)?.Currency?.Code;
            if (genericUnitCode == "ProfitCurrency") return tenantRepository.GetSingleTenant(_Widget.Tenant)?.ProfitCurrency?.Code;
            throw new Exception("Please Define The Unit");
        }

        private string GetGenericUnitCode()
        {
            int pFrom = measureField.Unit.IndexOf("{(") + "{(".Length;
            int pTo = measureField.Unit.LastIndexOf(")}");
            if (pFrom == -1 || pTo == -1) return null;
            return measureField.Unit.Substring(pFrom, pTo - pFrom);
        }

        private object FormatKpiValue(object value, string dataTypeCode)
        {
            if (value == null || value == System.DBNull.Value) return null;
            if (dataTypeCode == "Date" || dataTypeCode == "DateTime") return ((DateTime)value).ToString("yyyy-MM-dd");
            if (dataTypeCode == null || dataTypeCode == "Integer") return String.Format("{0:n0}", value);
            return String.Format("{0:n}", value);
        }

        private object BuildKpiChartValue<T>(WidgetMeasurePM widgetMeasureField, IQueryable<T> query)
        {
            query = new TreeFilterQueryService().Apply(query, new TreeFilterQueryArgs() { AdditionalTreeFilter = _Widget.Filters, ObjectTableName = "", Tenant = 0 });
            string queryString = CreateQuery(widgetMeasureField, query);
            var resultQueryables = DynamicListFromSql(queryString).FirstOrDefault();

            return ((IDictionary<string, object>)resultQueryables)["result"];
        }

        private object BuildKpiChartComparsionValue<T>(WidgetMeasurePM widgetMeasureField, IQueryable<T> query)
        {
            query = new TreeFilterQueryService().Apply(query, new TreeFilterQueryArgs() { AdditionalTreeFilter = _Widget.Filters, ObjectTableName = "", Tenant = 0 });
            string queryString = CreateComparsionQuery(widgetMeasureField, query);
            var resultQueryables = DynamicListFromSql(queryString).FirstOrDefault();
            return ((IDictionary<string, object>)resultQueryables)["result"];
        }

        private string CreateComparsionQuery<T>(WidgetMeasurePM widgetMeasureField, IQueryable<T> resultQueryable)
        {
            var query = BuildSelectQuery(widgetMeasureField);
            var queryString = $@"select {query} as result
                             From ({resultQueryable.ToQueryStringWithParameter()}) as data";

            if (_Widget.ComparisonOperator == "Between")
            {
                var diffTowDates = (_Widget.ToDate).Value.Subtract(_Widget.FromDate.Value).TotalDays;
                var diffTowDatesInt = Convert.ToInt32(diffTowDates);
         
                return queryString + $@" where data.{GeteComparsionDate()} Between dateadd(Day, {-diffTowDatesInt}, cast(getDate() as DateTime)) AND '{_Widget.FromDate}'";
            }
            return queryString + $@" where data.{GeteComparsionDate()} Between dateadd ({_Widget.ComparisonDateGroup}, {-2 * _Widget.ComparisonPeriod}, cast(getDate() as DateTime))
                             AND dateadd ({_Widget.ComparisonDateGroup}, {-_Widget.ComparisonPeriod}, cast(getDate() as DateTime))";
        }

        private string CreateQuery<T>(WidgetMeasurePM widgetMeasureField, IQueryable<T> resultQueryable)
        {
            var query = BuildSelectQuery(widgetMeasureField);
            var queryString = $@"select {query} as result From ({resultQueryable.ToQueryStringWithParameter()}) as data";
            return !_Widget.TimeOverTime ? queryString : queryString + CreateBetweenQuery();
        }

        private string CreateBetweenQuery()
        {
            if (_Widget.ComparisonOperator != "Between")
            {
                return $@" where data.{GeteComparsionDate()} Between dateadd ({_Widget.ComparisonDateGroup}, {-_Widget.ComparisonPeriod}, cast(getDate() as DateTime)) AND cast(getDate() as DateTime)";
            }
            return $@" where data.{GeteComparsionDate()} Between '{_Widget.FromDate}' AND '{_Widget.ToDate}'";
        }

        private string GeteComparsionDate()
        {
            if (_Entity.TableName == "ShipmentAnalytics") return "CreateDateTime";
            if (_Entity.TableName == "QuoteAnalytics") return "OpenDate";
            if (_Entity.TableName == "APInvoiceAnalytics") return "CreateDate";
            if (_Entity.TableName == "ARInvoiceAnalytics") return "CreateDate";
            if (_Entity.TableName == "OpportunityAnalytics") return "CreateDate";
            return "";
        }

        private string BuildSelectQuery(WidgetMeasurePM widgetMeasureField)
        {
            if (widgetMeasureField.MeasureCode == "Count") return $"{widgetMeasureField.MeasureCode}(data.Id)";
            return $"{widgetMeasureField.MeasureCode}(data.{measureField.FieldCode})";
        }
    }
}
