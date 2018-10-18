 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Customs.Data.Repsitories
{
    public partial class CustomsExchangeRateRepository : IRepository<CustomsExchangeRate>
    {

        public List<CustomsExchangeRate> GetMulti(EntityKeyFields entityKeys)
        {

            throw new NotImplementedException();
        }

        public List<CustomsExchangeRate> GetRatesByDate(DateTime rateDate, int Tenant)
        {
            DateTime fromdate = rateDate.Date;
            DateTime todate = rateDate.Date.AddHours(23).AddMinutes(59);
            return (from a in context.CustomsExchangeRates.Include("CurrencyType")
                    where a.RateDate >= fromdate && a.RateDate <= todate && a.Tenant == Tenant
                    select a).ToList();
        }


        public string GetIdByCurrencyAndDate(string currencyTypeID, DateTime rateDate, int tenant)
        {
            return
                  (from rec in context.CustomsExchangeRates
                   where (rec.CurrencyTypeCode == currencyTypeID && rec.RateDate == rateDate) && rec.Tenant == tenant
                   select rec.Id)
                  .FirstOrDefault();
        }

        public List<CustomsExchangeRate> GetCustomsExchangeRateByCurrencyAndDate(string currencyTypeCodes, DateTime? date, int tenant)
        {
            List<CustomsExchangeRate> rates = new List<CustomsExchangeRate>();
            if (currencyTypeCodes != null)
            {
                string[] codes = currencyTypeCodes.Split(',');
                foreach (string currency in codes)
                {
                    if (!string.IsNullOrEmpty(currency))
                    {
                        CustomsExchangeRate rate = (from a in context.CustomsExchangeRates
                                                    where a.CurrencyTypeCode == currency && a.RateDate <= date && a.Tenant == tenant
                                                    select a).OrderByDescending(d => d.RateDate).FirstOrDefault();
                        if (rate != null)
                        {
                            rates.Add(rate);
                        }
                    }

                }
            }

            return rates;
        }




        //<--- Yuval Chalup 14.12.2014 TASK-4238
        public List<CustomsExchangeRate> GetCustomsExchangeRateForDate(DateTime? date, int tenant)
        {



            if (date == null)
            {
                return null;
            }

            var key = "GetCustomsExchangeRateForDate," + date.Value.ToString() + "," + tenant.ToString();
            var myCustomsExchangeRate = CacheManager.GetOrInsertNewObject<List<CustomsExchangeRate>>(key,
                () =>
                {




                    var oracle12 = false;
                    var CustomsExchangeRate = new List<EntityPOCOs.CustomsExchangeRate>();
                    if (oracle12)
                    {
                        var tq = (from a in context.CustomsExchangeRates
                                  where a.RateDate <= date && a.Tenant == tenant
                                  group a by a.CurrencyTypeCode into gCurrency
                                  select gCurrency.OrderByDescending(t => t.RateDate).FirstOrDefault()
                        );
                        CustomsExchangeRate = tq.ToList();
                    }
                    else
                    {

                        var tk2 = true;

                        var q = (from a in context.CustomsExchangeRates
                                 where a.RateDate <= date && a.Tenant == tenant
                                 group a by a.CurrencyTypeCode into gCurrency
                                 select new { gCurrency.Key, MaxRateDate = gCurrency.Max(r => r.RateDate) }
                         );



                        var l = q.ToList();
                        if (tk2)
                        {
                            // call to oracle  2 time but can fetch 34* (listOfDate not more then 34)
                            var listOfDate = l.Select(r => r.MaxRateDate).Distinct();

                            CustomsExchangeRate = (from a in context.CustomsExchangeRates
                                                   where a.RateDate <= date && a.Tenant == tenant
                                                   where listOfDate.Contains(a.RateDate)
                                                   select a
                           ).ToList();

                            CustomsExchangeRate =
                            (from a in CustomsExchangeRate
                             where a.RateDate <= date && a.Tenant == tenant
                             group a by a.CurrencyTypeCode into gCurrency
                             select gCurrency.OrderByDescending(t => t.RateDate).FirstOrDefault()
                             ).ToList();
                        }
                        else
                        {
                            //call to oracle 1 + 34  = 35 

                            foreach (var item in l)
                            {
                                var poco = (from a in context.CustomsExchangeRates
                                            where a.RateDate <= date && a.Tenant == tenant
                                            where a.CurrencyTypeCode == item.Key && a.RateDate == item.MaxRateDate
                                            select a
                                 ).FirstOrDefault();
                                CustomsExchangeRate.Add(poco);
                            }
                        }
                        return CustomsExchangeRate;
                    }
                    //List<CustomsExchangeRate> 
                    //CustomsExchangeRate = (from a in context.CustomsExchangeRates
                    //                                                 where a.RateDate <= date && a.Tenant == tenant
                    //                                                 select a
                    //).ToList();
                    return CustomsExchangeRate;


                }
                );
            return myCustomsExchangeRate;
        }
        //Yuval Chalup 14.12.2014 TASK-4238 --->


        public CustomsExchangeRate GetCustomsExchangeRateForDateAndCurrencyTypeCode(string invoiceCurrencyTypeCode, DateTime? taxationDateTime, int tenant)
        {
            if (taxationDateTime == null || string.IsNullOrEmpty(invoiceCurrencyTypeCode))
            {
                return null;
            }

            var key = "GetCustomsExchangeRateForDate," + invoiceCurrencyTypeCode.ToString() + "," + taxationDateTime.Value.ToString() + "," + tenant.ToString();
            var myCustomsExchangeRate = CacheManager.GetOrInsertNewObject<CustomsExchangeRate>(key,
                () =>
                {
                    var customsExchangeRate = new EntityPOCOs.CustomsExchangeRate();
                    customsExchangeRate = (from a in context.CustomsExchangeRates
                                           where a.CurrencyTypeCode == invoiceCurrencyTypeCode && a.RateDate <= taxationDateTime && a.Tenant == tenant
                                           select a).OrderByDescending(t => t.RateDate).FirstOrDefault();
                    return customsExchangeRate;
                });
            return myCustomsExchangeRate;

        }

    }
}
   