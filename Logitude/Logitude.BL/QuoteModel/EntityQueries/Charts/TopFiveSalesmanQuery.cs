using Logitude.BL.DataContracts;
using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.QuoteModel.EntityQueries.Charts
{
    class TopFiveSalesmanQuery
    {
        List<ChartingDataClass> result;
        public List<ChartingDataClass> FilterToFiveSalesmanByProfit(IQueryable<Quote> dataSourceQuery)
        {

            result = (from q in dataSourceQuery.Include("SalesmanUser").Include("Contact")
                      where q.EstimatedProfitInLocal != null
                      group q by new { q.SalesmanUserId, q.SalesmanUser.Contact.EnglishName, q.EstimatedProfitInLocal } into g
                      orderby g.Sum(q => q.EstimatedProfitInLocal)
                      select new ChartingDataClass()
                      {
                          Id = g.Key.SalesmanUserId,
                          //DecimalProperty = Convert.ToDecimal(g.Key.EstimatedProfitInLocal),
                      }).Take(5).ToList();
            
            return result;
        }
    }
}

