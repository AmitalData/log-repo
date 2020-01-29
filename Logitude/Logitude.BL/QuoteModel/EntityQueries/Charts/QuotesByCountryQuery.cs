using Logitude.BL.DataContracts;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
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
            List<ChartingDataClass> resultList = null;
            List<ChartingDataClass> othersResultList = null;

            if (top < 0)
            {
                top = 0;
            }

            resultList = (from s in dataSourceQuery.Include("CountryForStatistics")
                          group s by new
                          {
                              s.CountryForStatisticsId,
                              s.CountryForStatistics.EnglishName,
                          } into m
                          select new ChartingDataClass()
                          {
                              CountryId = m.Key.CountryForStatisticsId,
                              CountryName = m.Key.EnglishName,
                              Total = m.Count(),
                          }).OrderByDescending(d => d.Total).Take(top.Value).ToList();

            if (incluseOtherCoutnries)
            {
                List<ChartingDataClass> allCountriesResult = (from s in dataSourceQuery.Include("CountryForStatistics")
                                                              group s by new
                                                              {
                                                                  s.CountryForStatisticsId,
                                                                  s.CountryForStatistics.EnglishName,
                                                              } into m
                                                              select new ChartingDataClass()
                                                              {
                                                                  CountryId = m.Key.CountryForStatisticsId,
                                                                  CountryName = m.Key.EnglishName,
                                                                  Total = m.Count(),
                                                              }).ToList();

                othersResultList = (from a in allCountriesResult
                                    where !(from r in resultList where r.CountryId == a.CountryId select r).Any()
                                    select a).ToList();

                foreach (ChartingDataClass d in othersResultList)
                {
                    d.CountryName = "Others";
                }

                myResult = resultList.Union(othersResultList).OrderByDescending(d => d.Total).ToList();
            }

            else
            {
                myResult = resultList.ToList();
            }

            return myResult;
        }
    }
}
