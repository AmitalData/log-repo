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
        public List<ChartingDataClass> FilterQuotesByCountry(IQueryable<Quote> dataSourceQuery, bool incluseOtherCoutnries, int? top)
        {
            //List<ChartingDataClass> resultList = null;
            //List<ChartingDataClass> othersResultList = null;

            if (top < 0)
            {
                top = 0;
            }

            //resultList = (from s in dataSourceQuery
            //              group s by new
            //              {
            //                  s.CountryForStatisticsCode,
            //                  s.CountryForStatisticsName

            //              } into m
            //              select new ChartingDataClass()
            //              {
            //                  CountryCode = m.Key.CountryForStatisticsCode,
            //                  CountryName = m.Key.CountryForStatisticsName,                  
            //                  Total = m.Count(),
            //              }).OrderByDescending(d => d.total).Take(top).ToList();

            //if (incluseOtherCoutnries)
            //{
            //    List<ChartingDataClass> allCountriesResult = (from s in dataSourceQuery
            //                                                  group s by new
            //                                               {
            //                                                   s.CountryForStatisticsCode,
            //                                                   s.CountryForStatisticsName

            //                                               } into m
            //                                               select new ChartingDataClass()
            //                                               {
            //                                                   CountryCode = m.Key.CountryForStatisticsCode,
            //                                                   CountryName = m.Key.CountryForStatisticsName,                                                  
            //                                                   Total = m.Count(),                                                
            //                                               }).ToList();
                                
            //    othersResultList = (from a in allCountriesResult
            //                        where !(from r in resultList where r.CountryCode == a.CountryCode select r).Any()
            //                        select a).ToList();

            //    foreach (ChartingDataClass d in othersResultList)
            //    {
            //        d.CountryCode = "Others";
            //        d.CountryName = "Others";
            //    }

            //    myResult = resultList.Union(othersResultList).OrderByDescending(d => d.Total).ToList();
            //}

            //else
            //{
            //    myResult = resultList.ToList();
            //}
            
            return myResult;
        }
    }
}

public class QuotesByCountry
{
    public string Id { get; set; }



}
