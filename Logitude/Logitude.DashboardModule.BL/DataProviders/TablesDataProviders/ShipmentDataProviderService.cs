using Logitude.DashboardModule.BL.APIDataContract;
using Logitude.DashboardModule.BL.DataProviders.Models;
using Logitude.DashboardModule.BL.DataProviders.WidgetsDataProviders;
using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.DashboardModule.BL.DataProviders.TablesDataProviders
{
    public class ShipmentDataProviderService : BaseTablesDataProvider
    {
        private int tenant;
        public ShipmentDataProviderService(WidgetPM widget, AnalyticsFactsMetaData entity, int tenant) : base(widget, entity, tenant)
        {
            this.tenant = tenant;
        }

        public override List<SeriesMeasure> GetChartData()
        {
            IShipmentsContext MyContext = ShipmentsContext.GetContext(tenant);
            ShipmentAnalyticRepository shipmentAnalyticRepository = new ShipmentAnalyticRepository(MyContext);
            var shipmentAnalyticIQueryable = shipmentAnalyticRepository.GetAll();
            shipmentAnalyticIQueryable = shipmentAnalyticIQueryable.Where(e => e.Tenant == tenant);
            var result = new ChartDataProviderService(_Widget, _Entity).GetData(shipmentAnalyticIQueryable);
            return result;
        }

        public override AnalyticData GeChartDataPart(WidgetArguments widgetPartArguments)
        {
            IShipmentsContext MyContext = ShipmentsContext.GetContext(tenant);
            ShipmentAnalyticRepository shipmentAnalyticRepository = new ShipmentAnalyticRepository(MyContext);
            var shipmentAnalyticIQueryable = shipmentAnalyticRepository.GetAll();
            shipmentAnalyticIQueryable = shipmentAnalyticIQueryable.Where(e => e.Tenant == tenant);
            List<string> analyticTableFields = GetSelectFields();
            var result = new ChartDataProviderService(_Widget, _Entity).GetDataPart<ShipmentAnalytic>(shipmentAnalyticIQueryable, analyticTableFields, widgetPartArguments);
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
            IShipmentsContext MyContext = ShipmentsContext.GetContext(tenant);
            ShipmentAnalyticRepository shipmentAnalyticRepository = new ShipmentAnalyticRepository(MyContext);
            var shipmentAnalyticIQueryable = shipmentAnalyticRepository.GetAll();
            shipmentAnalyticIQueryable = shipmentAnalyticIQueryable.Where(e => e.Tenant == tenant);
            return new KpiDataProviderService(_Widget, _Entity, tenant).GetData(shipmentAnalyticIQueryable);
        }
    }
}
