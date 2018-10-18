using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
     public partial class CustomsExchangeRateQueryService: EntityQueryService<CustomsExchangeRate, CustomsExchangeRateKeys, CustomsExchangeRatePM, object, CustomsExchangeRateKeys>
    {

         public List<CustomsExchangeRateList> GetRatesByDate(DateTime rateDate, int tenant)
         {
             List<CustomsExchangeRate> rates = repository.GetRatesByDate(rateDate, tenant);
             List<CustomsExchangeRateList> rateLists = new List<CustomsExchangeRateList>();

             foreach (CustomsExchangeRate item in rates)
             {
                 CustomsExchangeRateList rateList = new CustomsExchangeRateList()
                 {
                     Id = item.Id,
                     Tenant = item.Tenant,
                     CurrencyTypeCode = item.CurrencyTypeCode,
                     ExchangeRate = item.ExchangeRate,
                     RateDate = item.RateDate,
                     CurrencyTypeName = item.CurrencyType != null ? item.CurrencyType.LocalName : null,
                     UpdateDateTime = item.UpdateDateTime,
                 };
                 rateLists.Add(rateList);

             }
             return rateLists;
         }

         public string GetIdByCurrencyAndDate(string currencyTypeID, DateTime rateDate, int tenant)
         {
             if (String.IsNullOrWhiteSpace(currencyTypeID)) return "";
             return repository.GetIdByCurrencyAndDate(currencyTypeID, rateDate, tenant);
         }

         public List<CustomsExchangeRatePM> GetExchangeRateByCurrencyAndDate(string currencyTypeCode, DateTime? date, int tenant)
         {
             List<CustomsExchangeRate> rates = repository.GetCustomsExchangeRateByCurrencyAndDate(currencyTypeCode, date, tenant);


             List<CustomsExchangeRatePM> ratepms = (from a in rates
                                                    select new CustomsExchangeRatePM()
                                                  {
                                                      CurrencyTypeCode = a.CurrencyTypeCode,
                                                      ExchangeRate = a.ExchangeRate,
                                                      Id = a.Id,
                                                      RateDate = a.RateDate,
                                                      Tenant = a.Tenant,
                                                      UpdateDateTime = a.UpdateDateTime,
                                                  }).ToList();

             return ratepms;
         }

        //<--- Yuval Chalup 14.12.2014 TASK-4238
         public List<CustomsExchangeRatePM> GetCustomsExchangeRateForDate(DateTime? date, int tenant)
         {
             List<CustomsExchangeRate> rates = repository.GetCustomsExchangeRateForDate(date, tenant);


             List<CustomsExchangeRatePM> ratepms = (from a in rates
                                                    select new CustomsExchangeRatePM()
                                                    {
                                                        CurrencyTypeCode = a.CurrencyTypeCode,
                                                        ExchangeRate = a.ExchangeRate,
                                                        Id = a.Id,
                                                        RateDate = a.RateDate,
                                                        Tenant = a.Tenant,
                                                        UpdateDateTime = a.UpdateDateTime,
                                                    }).ToList();

             return ratepms;
         }
        //Yuval Chalup 14.12.2014 TASK-4238 --->

        public CustomsExchangeRatePM GetCustomsExchangeRateForDateAndCurrencyTypeCode(string invoiceCurrencyTypeCode, DateTime? taxationDateTime, int tenant)
        {
            CustomsExchangeRate rate = repository.GetCustomsExchangeRateForDateAndCurrencyTypeCode(invoiceCurrencyTypeCode, taxationDateTime, tenant);
            if (rate != null)
            {
                CustomsExchangeRatePM ratepm = new CustomsExchangeRatePM()
                {
                    CurrencyTypeCode = rate.CurrencyTypeCode,
                    ExchangeRate = rate.ExchangeRate,
                    Id = rate.Id,
                    RateDate = rate.RateDate,
                    Tenant = rate.Tenant,
                    UpdateDateTime = rate.UpdateDateTime,
                };
                return ratepm;
            }
            return null;
        }

    }
}
