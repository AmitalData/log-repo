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
                                                         Id = g.Key.SalesmanUserId,
                                                         SalesmanUserName = g.Key.EnglishName,
                                                         SalesmanUserId = g.Key.SalesmanUserId,
                                                     }).ToList();

            
            foreach (ChartingDataClass item_salesman in myData_Salesman)
            {
                //myResult.Add(new ChartingDataClass()
                //{
                //    Id = item_salesman.Id + ":AD",
                //    TransportModeDirection = "AD",
                //    TransportModeDirection_Display = "A,D",
                //    SalesmanUserName = item_salesman.SalesmanUserName,
                //    Count_All = dataSourceQuery.Where(d => d.SalesmanUserId == item_salesman.Id && d.TransportModeId + d.DirectionId == "AD").Count(),
                //    Count_Convert = dataSourceQuery.Where(d => d.SalesmanUserId == item_salesman.Id && d.TransportModeId + d.DirectionId == "AD" && d.UsageCount > 0).Count(),
                //    SalesmanUserId = item_salesman.Id,
                //});

                //myResult.Add(new ChartingDataClass()
                //{
                //    Id = item_salesman.Id + ":AE",
                //    TransportModeDirection = "AE",
                //    TransportModeDirection_Display = "A,E",
                //    SalesmanUserName = item_salesman.SalesmanUserName,
                //    Count_All = dataSourceQuery.Where(d => d.SalesmanUserId == item_salesman.Id && d.TransportModeId + d.DirectionId == "AE").Count(),
                //    Count_Convert = dataSourceQuery.Where(d => d.SalesmanUserId == item_salesman.Id && d.TransportModeId + d.DirectionId == "AE" && d.UsageCount > 0).Count(),
                //    SalesmanUserId = item_salesman.Id,
                //});

                //myResult.Add(new ChartingDataClass()
                //{
                //    Id = item_salesman.Id + ":AI",
                //    TransportModeDirection = "AI",
                //    TransportModeDirection_Display = "A,I",
                //    SalesmanUserName = item_salesman.SalesmanUserName,
                //    Count_All = dataSourceQuery.Where(d => d.SalesmanUserId == item_salesman.Id && d.TransportModeId + d.DirectionId == "AI").Count(),
                //    Count_Convert = dataSourceQuery.Where(d => d.SalesmanUserId == item_salesman.Id && d.TransportModeId + d.DirectionId == "AI" && d.UsageCount > 0).Count(),
                //    SalesmanUserId = item_salesman.Id,
                //});

                //myResult.Add(new ChartingDataClass()
                //{
                //    Id = item_salesman.Id + ":AR",
                //    TransportModeDirection = "AR",
                //    TransportModeDirection_Display = "A,R",
                //    SalesmanUserName = item_salesman.SalesmanUserName,
                //    Count_All = dataSourceQuery.Where(d => d.SalesmanUserId == item_salesman.Id && d.TransportModeId + d.DirectionId == "AR").Count(),
                //    Count_Convert = dataSourceQuery.Where(d => d.SalesmanUserId == item_salesman.Id && d.TransportModeId + d.DirectionId == "AR" && d.UsageCount > 0).Count(),
                //    SalesmanUserId = item_salesman.Id,
                //});

                //myResult.Add(new ChartingDataClass()
                //{
                //    Id = item_salesman.Id + ":ID",
                //    TransportModeDirection = "ID",
                //    TransportModeDirection_Display = "I,D",
                //    SalesmanUserName = item_salesman.SalesmanUserName,
                //    Count_All = dataSourceQuery.Where(d => d.SalesmanUserId == item_salesman.Id && d.TransportModeId + d.DirectionId == "ID").Count(),
                //    Count_Convert = dataSourceQuery.Where(d => d.SalesmanUserId == item_salesman.Id && d.TransportModeId + d.DirectionId == "ID" && d.UsageCount > 0).Count(),
                //    SalesmanUserId = item_salesman.Id,
                //});

                //myResult.Add(new ChartingDataClass()
                //{
                //    Id = item_salesman.Id + ":IE",
                //    TransportModeDirection = "IE",
                //    TransportModeDirection_Display = "I,E",
                //    SalesmanUserName = item_salesman.SalesmanUserName,
                //    Count_All = dataSourceQuery.Where(d => d.SalesmanUserId == item_salesman.Id && d.TransportModeId + d.DirectionId == "IE").Count(),
                //    Count_Convert = dataSourceQuery.Where(d => d.SalesmanUserId == item_salesman.Id && d.TransportModeId + d.DirectionId == "IE" && d.UsageCount > 0).Count(),
                //    SalesmanUserId = item_salesman.Id,
                //});

                //myResult.Add(new ChartingDataClass()
                //{
                //    Id = item_salesman.Id + ":II",
                //    TransportModeDirection = "II",
                //    TransportModeDirection_Display = "I,I",
                //    SalesmanUserName = item_salesman.SalesmanUserName,
                //    Count_All = dataSourceQuery.Where(d => d.SalesmanUserId == item_salesman.Id && d.TransportModeId + d.DirectionId == "II").Count(),
                //    Count_Convert = dataSourceQuery.Where(d => d.SalesmanUserId == item_salesman.Id && d.TransportModeId + d.DirectionId == "II" && d.UsageCount > 0).Count(),
                //    SalesmanUserId = item_salesman.Id,
                //});

                //myResult.Add(new ChartingDataClass()
                //{
                //    Id = item_salesman.Id + ":IR",
                //    TransportModeDirection = "IR",
                //    TransportModeDirection_Display = "I,R",
                //    SalesmanUserName = item_salesman.SalesmanUserName,
                //    Count_All = dataSourceQuery.Where(d => d.SalesmanUserId == item_salesman.Id && d.TransportModeId + d.DirectionId == "IR").Count(),
                //    Count_Convert = dataSourceQuery.Where(d => d.SalesmanUserId == item_salesman.Id && d.TransportModeId + d.DirectionId == "IR" && d.UsageCount > 0).Count(),
                //    SalesmanUserId = item_salesman.Id,
                //});

                //myResult.Add(new ChartingDataClass()
                //{
                //    Id = item_salesman.Id + ":OD",
                //    TransportModeDirection = "OD",
                //    TransportModeDirection_Display = "O,D",
                //    SalesmanUserName = item_salesman.SalesmanUserName,
                //    Count_All = dataSourceQuery.Where(d => d.SalesmanUserId == item_salesman.Id && d.TransportModeId + d.DirectionId == "OD").Count(),
                //    Count_Convert = dataSourceQuery.Where(d => d.SalesmanUserId == item_salesman.Id && d.TransportModeId + d.DirectionId == "OD" && d.UsageCount > 0).Count(),
                //    SalesmanUserId = item_salesman.Id,
                //});

                //myResult.Add(new ChartingDataClass()
                //{
                //    Id = item_salesman.Id + ":OE",
                //    TransportModeDirection = "OE",
                //    TransportModeDirection_Display = "O,E",
                //    SalesmanUserName = item_salesman.SalesmanUserName,
                //    Count_All = dataSourceQuery.Where(d => d.SalesmanUserId == item_salesman.Id && d.TransportModeId + d.DirectionId == "OE").Count(),
                //    Count_Convert = dataSourceQuery.Where(d => d.SalesmanUserId == item_salesman.Id && d.TransportModeId + d.DirectionId == "OE" && d.UsageCount > 0).Count(),
                //    SalesmanUserId = item_salesman.Id,
                //});

                //myResult.Add(new ChartingDataClass()
                //{
                //    Id = item_salesman.Id + ":OI",
                //    TransportModeDirection = "OI",
                //    TransportModeDirection_Display = "O,I",
                //    SalesmanUserName = item_salesman.SalesmanUserName,
                //    Count_All = dataSourceQuery.Where(d => d.SalesmanUserId == item_salesman.Id && d.TransportModeId + d.DirectionId == "OI").Count(),
                //    Count_Convert = dataSourceQuery.Where(d => d.SalesmanUserId == item_salesman.Id && d.TransportModeId + d.DirectionId == "OI" && d.UsageCount > 0).Count(),
                //    SalesmanUserId = item_salesman.Id,
                //});

                //myResult.Add(new ChartingDataClass()
                //{
                //    Id = item_salesman.Id + ":OR",
                //    TransportModeDirection = "OR",
                //    TransportModeDirection_Display = "O,R",
                //    SalesmanUserName = item_salesman.SalesmanUserName,
                //    Count_All = dataSourceQuery.Where(d => d.SalesmanUserId == item_salesman.Id && d.TransportModeId + d.DirectionId == "OR").Count(),
                //    Count_Convert = dataSourceQuery.Where(d => d.SalesmanUserId == item_salesman.Id && d.TransportModeId + d.DirectionId == "OR" && d.UsageCount > 0).Count(),
                //    SalesmanUserId = item_salesman.Id,
                //});

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
                    double percentage = 0;
                    if (item.Count_All > 0)
                    {
                        int convertedCount = dataSourceQuery.Where(d => d.SalesmanUserId == item_salesman.SalesmanUserId && d.DirectionId == item.DirectionId && d.TransportModeId == item.TransportModeId && d.UsageCount > 0).Count();
                        percentage = (convertedCount / item.Count_All) * 100;
                    }

                    myResult.Add(new ChartingDataClass()
                    {
                        TransportModeDirection = item.TransportModeDirection,
                        TransportModeDirection_Display = item.TransportModeDirection_Display,
                        SalesmanUserName = item.SalesmanUserName,
                        SalesmanUserId = item.SalesmanUserId,
                        Count_All = item.Count_All,
                        Total = percentage,
                    });
                }               
            }
            
            return myResult;
        }
    }
}
