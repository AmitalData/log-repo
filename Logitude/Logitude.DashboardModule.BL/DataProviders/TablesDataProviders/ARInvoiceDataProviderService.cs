using Logitude.DashboardModule.BL.APIDataContract;
using Logitude.DashboardModule.BL.DataProviders.Models;
using Logitude.DashboardModule.BL.DataProviders.WidgetsDataProviders;
using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Simplog.Data.InvoiceModel;

using System.Collections.Generic;
using System.Linq;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Logitude.DashboardModule.BL.DataProviders.TablesDataProviders
{
    public class ARInvoiceDataProviderService : BaseTablesDataProvider
    {
        private readonly int tenant;
        public ARInvoiceDataProviderService(WidgetPM widget, AnalyticsFactsMetaData entity, int tenant) : base(widget, entity, tenant)
        {
            this.tenant = tenant;
        }

        public override List<SeriesMeasure> GetChartData()
        {
            IQueryable<ARInvoiceAnalytic> query = GetDefaultQuery();
            var result = new ChartDataProviderService(_Widget, _Entity).GetData(query);
            return result;
        }

        public override AnalyticData GeChartDataPart(WidgetArguments widgetPartArguments)
        {
            IQueryable<ARInvoiceAnalytic> query = GetDefaultQuery();
            List<string> analyticTableFields = GetSelectFields();
            var result = new ChartDataProviderService(_Widget, _Entity).GetDataPart<ARInvoiceAnalytic>(query, analyticTableFields, widgetPartArguments);
            return result;
        }

        private List<string> GetSelectFields()
        {
            return new List<string>
            {
                "InvoiceNumber"
            };
        }

        public override KpiChart GetKpiData()
        {
            IQueryable<ARInvoiceAnalytic> query = GetDefaultQuery();
            return new KpiDataProviderService(_Widget, _Entity, tenant).GetData(query);
        }

        private IQueryable<ARInvoiceAnalytic> GetDefaultQuery()
        {
            IInvoiceContext myContext = InvoiceContext.GetContext(tenant);
            var query = myContext.ARInvoiceAnalytics.AsQueryable();
            query = query.Where(e => e.Tenant == tenant);
            query = AddUserBranchRestrictionFilters(query);
            return query;
        }

    }
}
