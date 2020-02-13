using Logitude.BL.DataContracts;
using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.QuoteModel.EntityQueries.Charts
{
    public class SentQuotesKPIQuery
    {
        public List<ChartingDataClass> FilterQuotesByKPI(IQueryable<Quote> dataSourceQuery)
        {
            List<ChartingDataClass> quoteChartListResult = new List<ChartingDataClass>();
            List<ChartingDataClass> quoteChartist_Labels = new List<ChartingDataClass>();

            dataSourceQuery  = dataSourceQuery.Where(a => a.SentDate != null && a.RequestDate != null);
            var quoteList = (from a in dataSourceQuery
                             select new
                             {
                                 Period = (System.Data.Entity.DbFunctions.DiffDays(a.RequestDate, a.SentDate)).Value,
                             }).ToList();


            quoteChartist_Labels.Add(new ChartingDataClass()
            {
                StringProperty = "< 1d",
                IntegerProperty = quoteList.Where(a=>a.Period < 1).Count() * 100 / quoteList.Count(),
            });

            quoteChartist_Labels.Add(new ChartingDataClass()
            {
                StringProperty = "1-2 d",
                IntegerProperty = quoteList.Where(a => a.Period >= 1 && a.Period <= 2).Count() * 100 / quoteList.Count(),
            });

            quoteChartist_Labels.Add(new ChartingDataClass()
            {
                StringProperty = "3-4 d",
                IntegerProperty = quoteList.Where(a => a.Period >= 3 && a.Period <= 4).Count() * 100 / quoteList.Count(),
            });

            quoteChartist_Labels.Add(new ChartingDataClass()
            {
                StringProperty = "5-6 d",
                IntegerProperty = quoteList.Where(a => a.Period >= 5 && a.Period <= 6).Count() * 100 / quoteList.Count(),
            });

            quoteChartist_Labels.Add(new ChartingDataClass()
            {
                StringProperty = "7-8 d",
                IntegerProperty = quoteList.Where(a => a.Period >= 7 && a.Period <= 8).Count() * 100 / quoteList.Count(),
            });

            quoteChartist_Labels.Add(new ChartingDataClass()
            {
                StringProperty = "9+ d",
                IntegerProperty = quoteList.Where(a => a.Period >= 9).Count() * 100 / quoteList.Count(),
            });

            quoteChartListResult = (from a in quoteChartist_Labels
                                    group a by new { a.StringProperty,a.IntegerProperty } into g
                                    select new ChartingDataClass()
                                    {
                                        StringProperty = g.Key.StringProperty,
                                        IntegerProperty =  g.Key.IntegerProperty, 
                                    }).ToList();

            return quoteChartListResult;
        }
    }
}
