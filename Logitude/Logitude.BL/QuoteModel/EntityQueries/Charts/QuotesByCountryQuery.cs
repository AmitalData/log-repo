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
        public List<ChartingDataClass> FilterQuotesByCountry(IQueryable<Quote> dataSourceQuery, bool includeOtherCoutnries, int? top)
        {
            List<ChartingDataClass> resultList = null;

            if (top < 0)
            {
                top = 0;
            }

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

            resultList = allCountriesResult.OrderByDescending(d => d.Total).Take(top.Value).ToList();

            if (includeOtherCoutnries)
            {
                List<ChartingDataClass> othersResultList = (from a in allCountriesResult
                                    where !(from r in resultList where r.CountryId == a.CountryId select r).Any()
                                    select a).ToList();
                
                if (othersResultList.Count > 0)
                {
                    double othersTotal = othersResultList.Sum(s => s.Total);
                    string otherCountriesIds = null;

                    foreach (ChartingDataClass otherCountry in othersResultList)
                    {
                        if (string.IsNullOrEmpty(otherCountriesIds))
                        {
                            otherCountriesIds = otherCountry.CountryId;
                        }

                        else
                        {
                            otherCountriesIds = otherCountriesIds + "," + otherCountry.CountryId;
                        }
                    }

                    resultList.Add(new ChartingDataClass()
                    {
                        CountryName = "Others",
                        CountryId = otherCountriesIds,
                        Total = othersTotal,
                    });
                }
            }

            myResult = resultList.ToList();
            return myResult;
        }
    }
}
