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

namespace Logitude.DashboardModule.BL.DataProviders
{
    public class ShipmentDataProviderService : BaseDataProviderService
    {

        public ShipmentDataProviderService(WidgetPM widget, AnalyticsFactsMetaData entity):base(widget, entity)
        {

        }

        public override List<SeriesMeasure> GetWidgetData()
        {
            IShipmentsContext MyContext = ShipmentsContext.GetContext(_Widget.Tenant);
            ShipmentAnalyticRepository shipmentAnalyticRepository = new ShipmentAnalyticRepository(MyContext);
            var shipmentAnalyticIQueryable = shipmentAnalyticRepository.GetAll();
            var result = GetData(shipmentAnalyticIQueryable);
            return result;
        }
    }
}
