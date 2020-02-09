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
        public List<ChartingDataClass> FilterQuotesBySalesman(IQueryable<Quote> dataSourceQuery)
        {
            myResult = new List<ChartingDataClass>();

            dataSourceQuery = dataSourceQuery.Where(d => !string.IsNullOrEmpty(d.SalesmanUserId));

            List<ChartingDataClass> myData_Salesman = (from d in dataSourceQuery.Include("SalesmanUser").Include("SalesmanUser.Contact")                                                     
                                                     group d by new
                                                     {
                                                         d.SalesmanUserId,
                                                         d.SalesmanUser.Contact.EnglishName
                                                     } into g
                                                     select new ChartingDataClass()
                                                     {
                                                         SalesmanUserName = g.Key.EnglishName,
                                                         SalesmanUserId = g.Key.SalesmanUserId,
                                                     }).ToList();

            
            foreach (ChartingDataClass item_salesman in myData_Salesman)
            {
                List<ChartingDataClass> item_salesman_group = (from d in dataSourceQuery
                                                               where d.SalesmanUserId == item_salesman.SalesmanUserId
                                                               group d by new
                                                               {
                                                                   d.TransportModeId,
                                                                   d.DirectionId,
                                                               } into gr
                                                               orderby gr.Key.TransportModeId
                                                               select new ChartingDataClass()
                                                               {
                                                                   TransportModeDirection = gr.Key.TransportModeId + gr.Key.DirectionId,
                                                                   TransportModeDirection_Display = gr.Key.TransportModeId + "," + gr.Key.DirectionId,
                                                                   DirectionId = gr.Key.DirectionId,
                                                                   TransportModeId = gr.Key.TransportModeId,
                                                                   SalesmanUserId = item_salesman.SalesmanUserId,
                                                                   SalesmanUserName = item_salesman.SalesmanUserName,
                                                                   Count_All = gr.Count(),
                                                               }).ToList();
                
                foreach (ChartingDataClass item in item_salesman_group)
                {
                    int convertedCount = dataSourceQuery.Where(d => d.SalesmanUserId == item_salesman.SalesmanUserId && d.DirectionId == item.DirectionId && d.TransportModeId == item.TransportModeId && d.UsageCount > 0).Count();
                    myResult.Add(new ChartingDataClass()
                    {
                        TransportModeDirection = item.TransportModeDirection,
                        TransportModeDirection_Display = item.TransportModeDirection_Display,
                        SalesmanUserName = item.SalesmanUserName,
                        SalesmanUserId = item.SalesmanUserId,
                        Count_All = item.Count_All,
                        Count_Convert = convertedCount,
                    });                    
                }
            }
            
            return myResult;
        }
    }
}
