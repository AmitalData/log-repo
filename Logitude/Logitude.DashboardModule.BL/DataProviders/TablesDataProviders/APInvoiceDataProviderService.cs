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
    public class APInvoiceDataProviderService : BaseTablesDataProvider
    {
        private readonly int tenant;
        public APInvoiceDataProviderService(WidgetPM widget, AnalyticsFactsMetaData entity, int tenant) : base(widget, entity, tenant)
        {
            this.tenant = tenant;
        }

        public override List<SeriesMeasure> GetChartData()
        {
            IQueryable<APInvoiceAnalytic> query = GetDefaultQuery();
            var result = new ChartDataProviderService(_Widget, _Entity).GetData(query);
            return result;
        }

        public override AnalyticData GeChartDataPart(WidgetArguments widgetPartArguments)
        {
            IQueryable<APInvoiceAnalytic> query = GetDefaultQuery();
            List<string> analyticTableFields = GetSelectFields();
            var result = new ChartDataProviderService(_Widget, _Entity).GetDataPart<APInvoiceAnalytic>(query, analyticTableFields, widgetPartArguments);
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
            IQueryable<APInvoiceAnalytic> query = GetDefaultQuery();
            return new KpiDataProviderService(_Widget, _Entity, tenant).GetData(query);
        }

        private IQueryable<APInvoiceAnalytic> GetDefaultQuery()
        {
            IInvoiceContext myContext = InvoiceContext.GetContext(tenant);
            var query = myContext.APInvoiceAnalytics.AsQueryable();
            query = query.Where(e => e.Tenant == tenant);
            query = AddUserBranchRestrictionFilters(query);
            return query;
        }
    }
}
