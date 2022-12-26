using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.DashboardModule.BL.APIDataContract;
using Logitude.DashboardModule.BL.DataProviders.Models;
using Logitude.DashboardModule.BL.DataProviders.WidgetsDataProviders;
using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data.EntityPOCOs;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.DashboardModule.BL.DataProviders.TablesDataProviders
{
    public class OpportunityDataProviderService : BaseTablesDataProvider
    {
        private int tenant;
        public OpportunityDataProviderService(WidgetPM widget, AnalyticsFactsMetaData entity, int tenant) : base(widget, entity, tenant)
        {
            this.tenant = tenant;
        }

        public override List<SeriesMeasure> GetChartData()
        {
            ICRMContext myContext = CRMContext.GetContext(tenant);
            var data = myContext.OpportunityAnalytics.AsQueryable();
            data = data.Where(e => e.Tenant == tenant);
            var result = new ChartDataProviderService(_Widget, _Entity).GetData(data);
            return result;
        }

        public override AnalyticData GeChartDataPart(WidgetArguments widgetPartArguments)
        {
            ICRMContext myContext = CRMContext.GetContext(tenant);
            var data = myContext.OpportunityAnalytics.AsQueryable();
            data = data.Where(e => e.Tenant == tenant);
            List<string> analyticTableFields = GetSelectFields();
            var result = new ChartDataProviderService(_Widget, _Entity).GetDataPart<OpportunityAnalytic>(data, analyticTableFields, widgetPartArguments);
            return result;
        }

        private List<string> GetSelectFields()
        {
            return new List<string>
            {
                "Subject",
                "StageId",
                "CustomerId",
                "OwnerId",
            };
        }

        public override KpiChart GetKpiData()
        {
            ICRMContext myContext = CRMContext.GetContext(tenant);
            var data = myContext.OpportunityAnalytics.AsQueryable();

            data = data.Where(e => e.Tenant == tenant);
            return new KpiDataProviderService(_Widget, _Entity, tenant).GetData(data);
        }
    }
}
