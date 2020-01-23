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
            dataSourceQuery = dataSourceQuery.Where(d => d.StageId != null);

            //List<ChartingDataClass> myResult =
            //  (from d in dataSourceQuery
            //   group d by new { d.StageId, d.Stage.Name, d.Stage.Probability } into g
            //   select new CRMChartingClass()
            //   {
            //       Id = g.Key.StageId,
            //       LabelProperty = g.Key.Name,
            //       DecimalProperty = filterCode == "CNT" ? g.Count() : g.Sum(s => s.NumberOfShipments).Value,
            //       IntegerProperty = g.Key.Probability == null ? 0 : g.Key.Probability.Value,
            //       GroupedId = g.Key.StageId,
            //   }).ToList();
            return result;
        }
    }
}
