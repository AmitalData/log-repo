using Logitude.DashboardModule.BL.DataProviders.TablesDataProviders;
using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DashboardModule.BL.DataProviders
{
    public class DataProviderFactory
    {
        const string ShipmentAnalyticsMetaData = "ShipmentAnalytics";
        const string APInvoiceAnalyticsMetaData = "APInvoiceAnalytics";
        const string ARInvoiceAnalyticMetaData = "ARInvoiceAnalytics";
        public BaseTablesDataProvider GetDataProviderService(WidgetPM widget)
        {
            var entity = GetEntity(widget.EntityId);
            switch (entity.TableName)
            {
                case ShipmentAnalyticsMetaData:
                    return new ShipmentDataProviderService(widget, entity);
                case APInvoiceAnalyticsMetaData:
                    return new APInvoiceDataProviderService(widget, entity);
                case ARInvoiceAnalyticMetaData:
                    return new ARInvoiceDataProviderService(widget, entity);
                default:
                    throw new Exception($"Meta Data Name {entity.TableName} not Provided in Data Provider Factory");
            }

        }
        private AnalyticsFactsMetaData GetEntity(string entityId)
        {
            var analyticsFactsMetaDataRepository = new AnalyticsFactsMetaDataRepository(0);
            var entity = analyticsFactsMetaDataRepository.GetSingle(entityId, 0);
            if (entity == null)
                throw new Exception($"Meta Data entity '{entityId}' not found");
            return entity;
        }
    }
}
