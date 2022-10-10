using Logitude.DashboardModule.BL.APIDataContract;
using Logitude.DashboardModule.BL.DataProviders.WidgetsDataProviders;
using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DashboardModule.BL.DataProviders.TablesDataProviders
{
    public class ShipmentDataProviderService : BaseTablesDataProvider
    {

        public ShipmentDataProviderService(WidgetPM widget, AnalyticsFactsMetaData entity) : base(widget, entity)
        {

        }

        public override List<SeriesMeasure> GetChartData()
        {
            IShipmentsContext MyContext = ShipmentsContext.GetContext(_Widget.Tenant);
            ShipmentAnalyticRepository shipmentAnalyticRepository = new ShipmentAnalyticRepository(MyContext);
            var shipmentAnalyticIQueryable = shipmentAnalyticRepository.GetAll();
            shipmentAnalyticIQueryable = shipmentAnalyticIQueryable.Where(e => e.Tenant == _Widget.Tenant);
            var result = new ChartDataProviderService(_Widget, _Entity).GetData(shipmentAnalyticIQueryable);
            return result;
        }

        public override AnalyticData GeChartDataPart(WidgetArguments widgetPartArguments)
        {
            IShipmentsContext MyContext = ShipmentsContext.GetContext(_Widget.Tenant);
            ShipmentAnalyticRepository shipmentAnalyticRepository = new ShipmentAnalyticRepository(MyContext);
            var shipmentAnalyticIQueryable = shipmentAnalyticRepository.GetAll();
            shipmentAnalyticIQueryable = shipmentAnalyticIQueryable.Where(e => e.Tenant == _Widget.Tenant);
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
    }
}
