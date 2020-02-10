//using CHAMP17;
using Logitude.BL.DataContracts;
using Logitude.BL.QuoteModel.BusinessUnitFilters;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.QuoteModel.EntityQueries.Charts
{
    public class DashboardQuery
    {
        
        int tenant;
        IQueryable<Quote> dataSourceQuery;
        IQuotesContext context;
        public DashboardQuery(int tenant)
        {
            this.tenant = tenant;
            context = QuotesContext.GetContext(tenant);
        }
        public IQueryable<Quote> FilterBasicValues(QuoteDashboardArguments quoteDashboardArgs)
        {
            dataSourceQuery =
                (from d in context.Quotes
                 where d.Tenant == tenant
                 && !d.IsClosed
                 && !d.IsCancelled
                 select d);

            QuoteBusinessUnitFilter filter = new QuoteBusinessUnitFilter(tenant);
            dataSourceQuery = filter.RunFilter(dataSourceQuery);
            FilterOwner(quoteDashboardArgs.OwnerId);
            FilterBusinessUnit(quoteDashboardArgs.BusinessUnitId);
            FilterCreateDate(quoteDashboardArgs.FromDate, quoteDashboardArgs.ToDate);

            if (quoteDashboardArgs.ChartCode == "QOC")
            {
                FilterDirectionAndTransportMode(quoteDashboardArgs.DirectionId, quoteDashboardArgs.TransportModeId);
            }

            return dataSourceQuery;
        }
        
        private void FilterCreateDate(DateTime? fromDate, DateTime? toDate)
        {
            if (fromDate != null && toDate != null)
            {
                dataSourceQuery = dataSourceQuery.Where(d => DbFunctions.TruncateTime(d.OpenDate) >= fromDate && 
                                                             DbFunctions.TruncateTime(d.OpenDate) <= toDate);
            }
        }        

        private void FilterOwner(string ownerId)
        {
            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.CreatedByUserId == ownerId);
            }
        }

        private void FilterBusinessUnit(string businessUnitId)
        {
            if (!string.IsNullOrEmpty(businessUnitId))
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.BusinessUnitId == businessUnitId);
            }
        }

        private void FilterDirectionAndTransportMode(string directionId, string transportModeId)
        {
            if (!string.IsNullOrEmpty(directionId) && directionId != "All")
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.DirectionId == directionId);
            }

            if (!string.IsNullOrEmpty(transportModeId) && transportModeId != "All")
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.TransportModeId == transportModeId);
            }
        }

        public List<ChartingDataClass> GetChartValues(QuoteDashboardArguments quoteDashboardArgs)
        {            
            string chartCode = quoteDashboardArgs.ChartCode;
            IQueryable<Quote> iQueryable = FilterBasicValues(quoteDashboardArgs);

            List<ChartingDataClass> result = new List<ChartingDataClass>();

            if (chartCode == "OQS")
            {
                StageFunnelQuery stageFunnelQuery = new StageFunnelQuery();
                result = stageFunnelQuery.FilterStageFunnelValues(dataSourceQuery);
            }

            else if (chartCode == "QOC")
            {
                QuotesByCountryQuery myQuery = new QuotesByCountryQuery();
                result = myQuery.FilterQuotesByCountry(dataSourceQuery, quoteDashboardArgs.IncludeOthersCountries, quoteDashboardArgs.TopCountries);
            }

            else if (chartCode == "QCV")
            {
                QuoteConversionQuery myQuery = new QuoteConversionQuery();
                result = myQuery.FilterQuotesBySalesman(dataSourceQuery);
            }

            else if (chartCode == "TFS")
            {
                TopFiveSalesmanQuery topFiveSalesmanQuery = new TopFiveSalesmanQuery();
                result = topFiveSalesmanQuery.FilterToFiveSalesmanByProfit(dataSourceQuery);
            }

            else if (chartCode == "KPI")
            {

            }

            return result;
        }
    }
}
