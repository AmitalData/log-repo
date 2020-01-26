using Logitude.BL.DataContracts;
using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.QuoteModel.EntityQueries.Charts
{
    public class QuotesByCountryQuery
    {
        List<ChartingDataClass> myResult;
        public List<ChartingDataClass> FilterQuotesByCountry(IQueryable<Quote> dataSourceQuery)
        {
            dataSourceQuery = dataSourceQuery.Where(d => d.StageId != null && d.Stage.Rank > 0);

            myResult = (from d in dataSourceQuery
                      group d by new { d.StageId, d.Stage.Name, d.Stage.Rank } into g
                      orderby g.Count() descending
                      select new ChartingDataClass()
                      {
                          Id = g.Key.StageId,
                          LabelProperty = g.Key.Name,
                          DecimalProperty = g.Count(),
                          IntegerProperty = g.Key.Rank,
                          GroupedId = g.Key.StageId,
                      }).ToList();

            return myResult;
        }
    }
}
