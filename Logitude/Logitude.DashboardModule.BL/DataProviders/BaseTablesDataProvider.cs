using Logitude.DashboardModule.BL.APIDataContract;
using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data.EntityPOCOs;
using System.Collections.Generic;

namespace Logitude.DashboardModule.BL.DataProviders
{
    public abstract class BaseTablesDataProvider
    {
        protected WidgetPM _Widget;
        protected AnalyticsFactsMetaData _Entity;

        protected BaseTablesDataProvider(WidgetPM widget, AnalyticsFactsMetaData entity)
        {
            this._Widget = widget;
            this._Entity = entity;
        }

        public abstract List<SeriesMeasure> GetChartData();
        public abstract AnalyticData GeChartDataPart(WidgetArguments widgetPartArguments);
        //public abstract AnalyticData GetKpiData(WidgetArguments widgetPartArguments);
    }
}
