using Logitude.DashboardModule.BL.APIDataContract;
using Logitude.DashboardModule.BL.DataProviders.Models;
using Logitude.DashboardModule.BL.DataProviders.WidgetsDataProviders;
using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DashboardModule.BL.DataProviders.TablesDataProviders
{
    public class QuoteDataProviderService : BaseTablesDataProvider
    {
        private int tenant;
        public QuoteDataProviderService(WidgetPM widget, AnalyticsFactsMetaData entity, int tenant) : base(widget, entity, tenant)
        {
            this.tenant = tenant;
        }

        public override List<SeriesMeasure> GetChartData()
        {
            IQuotesContext myContext = QuotesContext.GetContext(tenant);
            var QuoteAnalyticIQueryable = myContext.QuoteAnalytics.AsQueryable();
            QuoteAnalyticIQueryable = QuoteAnalyticIQueryable.Where(e => e.Tenant == tenant);
            var result = new ChartDataProviderService(_Widget, _Entity).GetData(QuoteAnalyticIQueryable);
            return result;
        }

        public override AnalyticData GeChartDataPart(WidgetArguments widgetPartArguments)
        {
            IQuotesContext myContext = QuotesContext.GetContext(tenant);
            var QuoteAnalyticIQueryable = myContext.QuoteAnalytics.AsQueryable();

            QuoteAnalyticIQueryable = QuoteAnalyticIQueryable.Where(e => e.Tenant == tenant);
            List<string> analyticTableFields = GetSelectFields();
            var result = new ChartDataProviderService(_Widget, _Entity).GetDataPart<QuoteAnalytic>(QuoteAnalyticIQueryable, analyticTableFields, widgetPartArguments);
            return result;
        }

        private List<string> GetSelectFields()
        {
            return new List<string>
            {
                "QuoteNumber"
            };
        }

        public override KpiChart GetKpiData()
        {
            IQuotesContext myContext = QuotesContext.GetContext(tenant);
            var quoteAnalyticIQueryable = myContext.QuoteAnalytics.AsQueryable();

            quoteAnalyticIQueryable = quoteAnalyticIQueryable.Where(e => e.Tenant == tenant);
            return new KpiDataProviderService(_Widget, _Entity, tenant).GetData(quoteAnalyticIQueryable);
        }
    }
}
