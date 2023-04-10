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
    public class ShipmentDataProviderService : BaseTablesDataProvider
    {
        private readonly int tenant;
        public ShipmentDataProviderService(WidgetPM widget, AnalyticsFactsMetaData entity, int tenant) : base(widget, entity, tenant)
        {
            this.tenant = tenant;
        }

        public override List<SeriesMeasure> GetChartData()
        {
            IQueryable<ShipmentAnalytic> data = GetDefaultQuery();
            var result = new ChartDataProviderService(_Widget, _Entity).GetData(data);
            return result;
        }

        public override AnalyticData GeChartDataPart(WidgetArguments widgetPartArguments)
        {
            IQueryable<ShipmentAnalytic> data = GetDefaultQuery();
            List<string> analyticTableFields = GetSelectFields();
            var result = new ChartDataProviderService(_Widget, _Entity).GetDataPart<ShipmentAnalytic>(data, analyticTableFields, widgetPartArguments);
            return result;
        }

        private List<string> GetSelectFields()
        {
            return new List<string>
            {
                "ShipmentNumber"
            };
        }

        public override KpiChart GetKpiData()
        {
            IQueryable<ShipmentAnalytic> data = GetDefaultQuery();
            return new KpiDataProviderService(_Widget, _Entity, tenant).GetData(data);
        }

        private IQueryable<ShipmentAnalytic> GetDefaultQuery()
        {
            IShipmentsContext myContext = ShipmentsContext.GetContext(tenant);
            var query = myContext.ShipmentAnalytics.AsQueryable();
            query = query.Where(e => e.Tenant == tenant);
            query = AddUserBranchRestrictionFilters(query);
            return query;
        }
    }
}
