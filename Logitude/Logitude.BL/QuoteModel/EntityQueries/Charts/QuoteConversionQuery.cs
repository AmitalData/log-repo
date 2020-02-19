using Logitude.BL.DataContracts;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.QuoteModel.EntityQueries.Charts
{
    public class QuoteConversionQuery
    {
        List<ChartingDataClass> myResult;
        public List<ChartingDataClass> FilterQuotesByProductType(IQueryable<Quote> dataSourceQuery)
        {
            myResult = new List<ChartingDataClass>();

            myResult = (from d in dataSourceQuery.Include("TransportMode").Include("Direction")
                        group d by new
                        {
                            d.TransportModeId,
                            d.DirectionId,
                            DirectionName = d.Direction.Name,
                            TransportModeName = d.TransportMode.Name,
                        } into gr
                        orderby gr.Key.TransportModeId
                        select new ChartingDataClass()
                        {
                            GroupedId = gr.Key.TransportModeId + gr.Key.DirectionId,
                            DataTypeCode = gr.Key.TransportModeId + "," + gr.Key.DirectionId,
                            StringProperty = gr.Key.TransportModeName + " " + gr.Key.DirectionName,
                            DirectionId = gr.Key.DirectionId,
                            TransportModeId = gr.Key.TransportModeId,
                            Count_All = gr.Count(),
                            Count_Converted = gr.Where(d => d.UsageCount > 0).Count(),
                        }).ToList();

            return myResult;
        }
    }
}
