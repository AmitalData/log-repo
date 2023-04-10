using Logitude.DashboardModule.BL.APIDataContract;
using Logitude.DashboardModule.BL.DataProviders.Models;
using Logitude.DashboardModule.BL.DataProviders.WidgetsDataProviders;
using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.DashboardModule.BL.DataProviders.TablesDataProviders
{
    public class ContainerDataProviderService : BaseTablesDataProvider
    {
        private readonly int tenant;
        public ContainerDataProviderService(WidgetPM widget, AnalyticsFactsMetaData entity, int tenant) : base(widget, entity, tenant)
        {
            this.tenant = tenant;
        }

        public override List<SeriesMeasure> GetChartData()
        {
            IQueryable<ContainerAnalytic> query = GetDefaultQuery();
            var result = new ChartDataProviderService(_Widget, _Entity).GetData(query);
            return result;
        }

        public override AnalyticData GeChartDataPart(WidgetArguments widgetPartArguments)
        {
            IQueryable<ContainerAnalytic> query = GetDefaultQuery();
            List<string> analyticTableFields = GetSelectFields();
            var result = new ChartDataProviderService(_Widget, _Entity).GetDataPart<ContainerAnalytic>(query, analyticTableFields, widgetPartArguments);
            return result;
        }

        private List<string> GetSelectFields()
        {
            return new List<string>
            {
                "ContainerNumber",
                "MainCarriageCarrierId"
            };
        }

        public override KpiChart GetKpiData()
        {
            IQueryable<ContainerAnalytic> query = GetDefaultQuery();
            return new KpiDataProviderService(_Widget, _Entity, tenant).GetData(query);
        }

        private IQueryable<ContainerAnalytic> GetDefaultQuery()
        {
            IShipmentsContext myContext = ShipmentsContext.GetContext(tenant);
            var query = myContext.ContainerAnalytics.AsQueryable();
            query = query.Where(e => e.Tenant == tenant);
            return query;
        }
    }
}
