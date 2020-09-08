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
        private QuoteDashboardArguments args;
        public DashboardQuery(int tenant)
        {
            this.tenant = tenant;
            context = QuotesContext.GetContext(tenant);
        }
        public IQueryable<Quote> FilterBasicValues(QuoteDashboardArguments args)
        {
            this.args = args;

            this.FixFilters();

            dataSourceQuery =
                (from d in context.Quotes
                 where d.Tenant == tenant
                 //&& !d.IsClosed
                 && !d.IsCancelled
                 select d);

            switch (args.ChartCode)
            {
                case "TFS":
                    {
                        dataSourceQuery = (from d in dataSourceQuery
                                           join db_Stages in context.QuoteStages 
                                           on d.StageId equals db_Stages.Id into QuoteStages
                                           from s in QuoteStages.DefaultIfEmpty()
                                           where s.Tenant == tenant
                                           && s.Code == "QTAC"
                                           select d);                            
                        break;
                    }
                case "QCV":
                    {
                        dataSourceQuery = (from d in dataSourceQuery
                                           join db_Stages in context.QuoteStages
                                           on d.StageId equals db_Stages.Id into QuoteStages
                                           from s in QuoteStages.DefaultIfEmpty()
                                           where s.Tenant == tenant
                                           && s.Code != "QTCR" && s.Code != "QTDR"
                                           select d);
                        break;
                    }
            }

            if(args.ChartCode != "QCV" && args.ChartCode != "TFS" && args.ChartCode != "KPI")
            {
                dataSourceQuery = dataSourceQuery.Where(d => !d.IsClosed);
            }

            if (args.ChartCode == "KPI")
            {
                dataSourceQuery = dataSourceQuery.Where(a => a.SentDate != null && a.RequestDate != null);
            }

            QuoteBusinessUnitFilter filter = new QuoteBusinessUnitFilter(tenant);
            dataSourceQuery = filter.RunFilter(dataSourceQuery);
            FilterOwner(args.OwnerId);
            FilterBusinessUnit(args.BusinessUnitId);
            FilterCreateDate();

            if (args.ChartCode == "QOC")
            {
                FilterDirectionAndTransportMode(args.DirectionId, args.TransportModeId);
            }

            return dataSourceQuery;
        }

        private void FixFilters()
        {
            if(args.FromDate != null)
            {
                args.FromDate = args.FromDate.Value.Date;
            }

            if (args.ToDate != null)
            {
                args.ToDate = args.ToDate.Value.Date;
            }
        }

        private void FilterCreateDate()
        {
            if (args.FromDate != null && args.ToDate != null)
            {
                dataSourceQuery = dataSourceQuery.Where(d => DbFunctions.TruncateTime(d.OpenDate) >= args.FromDate && DbFunctions.TruncateTime(d.OpenDate) <= args.ToDate);
            }
        }        

        private void FilterOwner(string ownerId)
        {
            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.SalesmanUserId == ownerId);
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
                result = myQuery.FilterQuotesByProductType(dataSourceQuery);
            }

            else if (chartCode == "TFS")
            {
                TopFiveSalesmanQuery topFiveSalesmanQuery = new TopFiveSalesmanQuery(dataSourceQuery, args, tenant);
                result = topFiveSalesmanQuery.GetChartData();
            }

            else if (chartCode == "KPI")
            {
                SentQuotesKPIQuery sentQuotesKPIQuery = new SentQuotesKPIQuery();
                result = sentQuotesKPIQuery.FilterQuotesByKPI(dataSourceQuery);
            }

            return result;
        }
    }
}
