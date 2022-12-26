using Logitude.DashboardModule.BL.APIDataContract;
using Logitude.DashboardModule.BL.DataProviders.Models;
using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data.EntityPOCOs;
using System.Collections.Generic;

namespace Logitude.DashboardModule.BL.DataProviders
{
    public abstract class BaseTablesDataProvider
    {
        protected WidgetPM _Widget;
        protected AnalyticsFactsMetaData _Entity;
        protected int _Tenant;

        protected BaseTablesDataProvider(WidgetPM widget, AnalyticsFactsMetaData entity, int tenant)
        {
            this._Widget = widget;
            this._Entity = entity;
            this._Tenant = tenant;
        }

        public abstract List<SeriesMeasure> GetChartData();
        public abstract AnalyticData GeChartDataPart(WidgetArguments widgetPartArguments);
        public abstract KpiChart GetKpiData();

    }
}
