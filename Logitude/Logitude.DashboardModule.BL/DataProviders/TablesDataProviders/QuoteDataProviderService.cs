using Logitude.DashboardModule.BL.APIDataContract;
using Logitude.DashboardModule.BL.DataProviders.Models;
using Logitude.DashboardModule.BL.DataProviders.WidgetsDataProviders;
using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.DashboardModule.BL.DataProviders.TablesDataProviders
{
    public class QuoteDataProviderService : BaseTablesDataProvider
    {
        private readonly int tenant;
        public QuoteDataProviderService(WidgetPM widget, AnalyticsFactsMetaData entity, int tenant) : base(widget, entity, tenant)
        {
            this.tenant = tenant;
        }

        public override List<SeriesMeasure> GetChartData()
        {
            IQueryable<QuoteAnalytic> query = GetDefaultQuery();
            var result = new ChartDataProviderService(_Widget, _Entity).GetData(query);
            return result;
        }

        public override AnalyticData GeChartDataPart(WidgetArguments widgetPartArguments)
        {
            IQueryable<QuoteAnalytic> query = GetDefaultQuery();
            List<string> analyticTableFields = GetSelectFields();
            var result = new ChartDataProviderService(_Widget, _Entity).GetDataPart<QuoteAnalytic>(query, analyticTableFields, widgetPartArguments);
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
            IQueryable<QuoteAnalytic> query = GetDefaultQuery();
            return new KpiDataProviderService(_Widget, _Entity, tenant).GetData(query);
        }

        private IQueryable<QuoteAnalytic> GetDefaultQuery()
        {
            IQuotesContext myContext = QuotesContext.GetContext(tenant);
            var query = myContext.QuoteAnalytics.AsQueryable();
            query = query.Where(e => e.Tenant == tenant);
            query = AddUserBranchRestrictionFilters(query);
            return query;
        }
    }
}
