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
        private int tenant;
        public ARInvoiceDataProviderService(WidgetPM widget, AnalyticsFactsMetaData entity, int tenant) : base(widget, entity, tenant)
        {
            this.tenant = tenant;
        }

        public override List<SeriesMeasure> GetChartData()
        {
            IInvoiceContext myContext = InvoiceContext.GetContext(tenant);
            var arInvoiceAnalyticIQueryable = myContext.ARInvoiceAnalytics.AsQueryable();
            arInvoiceAnalyticIQueryable = arInvoiceAnalyticIQueryable.Where(e => e.Tenant == tenant);
            var result = new ChartDataProviderService(_Widget, _Entity).GetData(arInvoiceAnalyticIQueryable);
            return result;
        }

        public override AnalyticData GeChartDataPart(WidgetArguments widgetPartArguments)
        {
            IInvoiceContext myContext = InvoiceContext.GetContext(tenant);
            var arInvoiceAnalyticIQueryable = myContext.ARInvoiceAnalytics.AsQueryable();

            arInvoiceAnalyticIQueryable = arInvoiceAnalyticIQueryable.Where(e => e.Tenant == tenant);
            List<string> analyticTableFields = GetSelectFields();
            var result = new ChartDataProviderService(_Widget, _Entity).GetDataPart<ARInvoiceAnalytic>(arInvoiceAnalyticIQueryable, analyticTableFields, widgetPartArguments);
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
            IInvoiceContext myContext = InvoiceContext.GetContext(tenant);
            var arInvoiceAnalyticIQueryable = myContext.ARInvoiceAnalytics.AsQueryable();

            arInvoiceAnalyticIQueryable = arInvoiceAnalyticIQueryable.Where(e => e.Tenant == tenant);
            return new KpiDataProviderService(_Widget, _Entity, tenant).GetData(arInvoiceAnalyticIQueryable);
        }
    }
}
