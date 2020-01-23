using Logitude.BL.DataContracts;
using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.QuoteModel.EntityQueries.Charts
{
    public class StageFunnelQuery
    {
        List<ChartingDataClass> result;
        public List<ChartingDataClass> FilterStageFunnelValues(IQueryable<Quote> dataSourceQuery)
        {
            dataSourceQuery = dataSourceQuery.Where(d => d.StageId != null && d.Stage.Rank > 0);

            result = (from d in dataSourceQuery
                      group d by new { d.StageId, d.Stage.Name, d.Stage.Rank } into g
                      select new ChartingDataClass()
                      {
                          Id = g.Key.StageId,
                          LabelProperty = g.Key.Name,
                          DecimalProperty = g.Count(),
                          IntegerProperty = g.Key.Rank,
                          GroupedId = g.Key.StageId,
                      }).ToList();

            return result;
        }
    }
}
