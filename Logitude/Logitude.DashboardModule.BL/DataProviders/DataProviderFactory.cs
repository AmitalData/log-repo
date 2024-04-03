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
        const string QuoteAnalyticMetaData = "QuoteAnalytics";
        const string OpportunityAnalyticMetaData = "OpportunityAnalytics";
        const string ContainerAnalyticMetaData = "ContainerAnalytics";
        public BaseTablesDataProvider GetDataProviderService(WidgetPM widget, int tenant)
        {
            var entity = GetEntity(widget.EntityId);
            switch (entity.TableName)
            {
                case ShipmentAnalyticsMetaData: return new ShipmentDataProviderService(widget, entity, tenant);
                case APInvoiceAnalyticsMetaData: return new APInvoiceDataProviderService(widget, entity, tenant);
                case ARInvoiceAnalyticMetaData: return new ARInvoiceDataProviderService(widget, entity, tenant);
                case QuoteAnalyticMetaData: return new QuoteDataProviderService(widget, entity, tenant);
                case OpportunityAnalyticMetaData: return new OpportunityDataProviderService(widget, entity, tenant);
                case ContainerAnalyticMetaData: return new ContainerDataProviderService(widget, entity, tenant);
                default: throw new Exception($"Meta Data Name {entity.TableName} not Provided in Data Provider Factory");
            }

        }
        private AnalyticsFactsMetaData GetEntity(string entityId)
        {
            var analyticsFactsMetaDataRepository = new AnalyticsFactsMetaDataRepository(0);
            var entity = analyticsFactsMetaDataRepository.GetSingle(entityId, 0);
            if (entity == null)  throw new Exception($"Meta Data entity '{entityId}' not found");
            return entity;
        }
    }
}
