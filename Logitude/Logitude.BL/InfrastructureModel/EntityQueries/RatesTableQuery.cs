using Logitude.BL.CommonDataModel.APIDataContract;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs; 
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.Security;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using Logitude.BL.CommonDataModel.APIDataContract;
using Logitude.BL.Security;
using Logitude.Server.Tools;
using System.Linq;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class RatesTableQuery
    {
        RatesTableRepository repository;
        private bool isFullAccounting;
        public RatesTableQuery()
        {
            repository = new RatesTableRepository(); 
        }
        public RatesTableQuery(int tenant)
        {
            repository = new RatesTableRepository(tenant);
            
        }
        public RatesTableQuery(RatesTableRepository ratesTableRepository)
        {
            repository = ratesTableRepository;
        }

        public RatesTableQuery(RatesTableRepository ratesTableRepository,int tenant)
        {
            repository = ratesTableRepository;
            isFullAccounting = IsFullAccountingActivated(tenant);
        }

        public RatesTablePM GetSinglePM(string id, int tenent)
        {
            RatesTablePM instance = (from a in repository.context.RatesTable.Include(a => a.ForeignCurrency)
                                     where a.Tenant == tenent
                                     && a.Id == id
                                     select new RatesTablePM()
                                     {
                                         Id = a.Id,
                                         Tenant = a.Tenant,
                                         ForeignCurrencyId = a.ForeignCurrencyId,
                                         ForeignCurrencyCode = a.ForeignCurrency.Code,
                                         ForeignCurrencyName = a.ForeignCurrency.EnglishName,
                                         BaseCurrencyId = a.BaseCurrencyId,
                                         Rate = a.Rate,
                                         ValueDate = a.ValueDate,
                                         LogDateTime = a.LogDateTime,
                                     }).FirstOrDefault();

            return instance;
        }
        public IQueryable<RatesTableList> GetIQueryableEntityList(IQueryable<RatesTable> iQueryable)
        {
            IQueryable<RatesTableList> result = from entity in iQueryable.Include(a => a.ForeignCurrency).Include(a=>a.UpdatedByUser).Include(a=> a.UpdatedByUser.Contact)
                                                select new RatesTableList()
                                                {
                                                    Id = entity.Id,
                                                    Tenant = entity.Tenant,
                                                    Rate = entity.Rate,
                                                    ForeignCurrencyId = entity.ForeignCurrencyId,
                                                    ForeignCurrencyCode = entity.ForeignCurrency.Code,
                                                    ForeignCurrencyName = entity.ForeignCurrency.EnglishName,
                                                    BaseCurrencyId = entity.BaseCurrencyId,
                                                    ValueDate = entity.ValueDate,
                                                    LogDateTime = entity.LogDateTime,
                                                    UpdatedByUserId = entity.UpdatedByUserId,
                                                    UpdatedByUserName = entity.UpdatedByUser != null ? entity.UpdatedByUser.Contact.EnglishName : null,
                                                    UpdatedDate = entity.UpdatedDate,
                                                };
            return result;
        }


        /* Last Rates */
        public LastRate GetLastRecord(int tenant, string foreignCurrencyId, string baseCurrencyId)
        {
            List<RatesTable> myList = (from r in repository.context.RatesTable.Include(a => a.ForeignCurrency)
                                       where r.Tenant == tenant 
                                       && r.ForeignCurrencyId == foreignCurrencyId 
                                       && r.BaseCurrencyId == baseCurrencyId select r).OrderByDescending(r => r.ValueDate).ToList();

            RatesTable myRecord = (from r in myList select r).OrderByDescending(r => r.LogDateTime).FirstOrDefault();

            if (myRecord != null)
            {
                return new LastRate()
                {
                    Id = myRecord.Id,
                    Tenant = myRecord.Tenant,
                    ValueDate = myRecord.ValueDate,
                    Rate = myRecord.Rate,
                    ForeignCurrencyId = myRecord.ForeignCurrency.Id,
                    ForeignCurrencyCode = myRecord.ForeignCurrency.Code,
                    ForeignCurrencyName = myRecord.ForeignCurrency.EnglishName,
                    LogDateTime = myRecord.LogDateTime,
                    HistoryCount = myList.Count,
                    BaseCurrencyId = baseCurrencyId,
                };
            }

            else { return null; }
        }

        /* By Date Last Rates */
        public LastRate GetLastRecordByValueDate(int tenant, string foreignCurrencyId, string baseCurrencyId, DateTime? date,bool calculateRateAccordingNumberUnit = false)
        {
            LastRate myResult = null;
            
            if (date != null)
            {
                DateTime? iDateValue = date.Value.Date;

                IQueryable<RatesTable> iQuery = from r in repository.context.RatesTable.Include(a => a.ForeignCurrency)
                                                where r.Tenant == tenant
                                                && r.BaseCurrencyId == baseCurrencyId
                                                && r.ForeignCurrencyId == foreignCurrencyId
                                                && r.ValueDate != null
                                                && r.LogDateTime != null
                                                select r;

                if (iQuery != null)
                {
                    RatesTable myRecord = iQuery.Where(d => d.ValueDate == iDateValue).OrderByDescending(o => o.LogDateTime).FirstOrDefault();

                    if (myRecord == null)
                    {
                        myRecord = iQuery.Where(d => d.ValueDate < iDateValue).OrderByDescending(o => o.ValueDate).ThenByDescending(o => o.LogDateTime).FirstOrDefault();
                    }

                    if (myRecord == null)
                    {
                        myRecord = iQuery.OrderByDescending(o => o.ValueDate).ThenByDescending(o => o.LogDateTime).FirstOrDefault();
                    }

                    if (myRecord != null)
                    {
                        CurrencyRateRepository currencyRateRepository = new CurrencyRateRepository(tenant);
                         myResult = new LastRate()
                        {
                            Id = myRecord.Id,
                            Tenant = myRecord.Tenant,
                            ValueDate = myRecord.ValueDate,
                            Rate = CalculatesRateAccordingNumberUnit(myRecord,calculateRateAccordingNumberUnit),
                            Unit = myRecord.Unit,
                            ForeignCurrencyId = myRecord.ForeignCurrency.Id,
                            ForeignCurrencyCode = myRecord.ForeignCurrency.Code,
                            ForeignCurrencyName = myRecord.ForeignCurrency.EnglishName,
                            LogDateTime = myRecord.LogDateTime,
                            HistoryCount = iQuery.Count(),
                            BaseCurrencyId = baseCurrencyId,
                            CurrencyRates = currencyRateRepository.GetSingleByExchangeRateId(myRecord.Id)?.Select(r =>
                            {
                                r.Rate *= myRecord.Unit ?? 1;
                                return r;
                            }).ToList()
                         };
                    }
                }
            }

            return myResult;
        }


        public IQueryable<LastRate> GetCurrenciesExchangeRateByCurrencyId(int tenant, string foreignCurrencyId)
        {
            return repository.context.RatesTable
                   .Include(r => r.ForeignCurrency).Include(a => a.UpdatedByUser).Include(a => a.UpdatedByUser.Contact)
                 .Where(r => r.Tenant == tenant && r.ForeignCurrencyId == foreignCurrencyId && r.ValueDate != null)
                 .OrderByDescending(r => r.LogDateTime)
                 .Select(r => new LastRate
                 {
                     Id = r.Id,
                     Tenant = r.Tenant,
                     ValueDate = r.ValueDate,
                     Rate = r.Rate,
                     Unit = r.Unit,
                     ForeignCurrencyId = r.ForeignCurrency.Id,
                     ForeignCurrencyCode = r.ForeignCurrency.Code,
                     ForeignCurrencyName = r.ForeignCurrency.EnglishName,
                     LogDateTime = r.LogDateTime,
                     BaseCurrencyId = r.BaseCurrencyId,
                     UpdatedByUserName = r.UpdatedByUser != null ? r.UpdatedByUser.Contact.EnglishName : null,
                 });

        }


        public double getCurrencyRateAccordingUnit(RatesTable ratesTable)
        {
            if(ratesTable.Unit != null)
            {
                if(ratesTable.Unit > 0)
                {
                    return (double)(ratesTable.Rate * ratesTable.Unit);
                }
            }
            return (double)ratesTable.Rate;
        }

        private double CalculatesRateAccordingNumberUnit(RatesTable ratesTable,bool calculateRateAccordingNumberUnit)
        {
            if (isFullAccounting)
            {
                if (calculateRateAccordingNumberUnit)
                {
                    return getCurrencyRateAccordingUnit(ratesTable);
                } else
                {
                    return (double)ratesTable.Rate;
                }
            } else
            {
                return (double)ratesTable.Rate;
            }
            
        }
        private bool IsFullAccountingActivated(int tenant)
        {
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
            bool isFullAccountingActivated = tenantPOCO.AccountingActivated;
            return isFullAccountingActivated;
        }
        public RatesTablePM GetLastRateByValueDate(int tenant, string foreignCurrencyId, string baseCurrencyId, DateTime? date)
        {
            RatesTablePM myResult = null;

            RatesTable lastRecord =
                (from r in repository.context.RatesTable.Include(a=>a.ForeignCurrency)
                 where r.Tenant == tenant
                 && r.ForeignCurrencyId == foreignCurrencyId
                 && r.BaseCurrencyId == baseCurrencyId
                 && r.ValueDate <= date
                 select r).OrderByDescending(r => r.ValueDate).FirstOrDefault();


            if (lastRecord == null)
            {
                lastRecord =
                    (from r in repository.context.RatesTable.Include(a => a.ForeignCurrency)
                     where r.Tenant == tenant
                     && r.ForeignCurrencyId == foreignCurrencyId
                     && r.BaseCurrencyId == baseCurrencyId
                     select r).OrderByDescending(r => r.ValueDate).ThenByDescending(o => o.LogDateTime).FirstOrDefault();
            }


            if (lastRecord != null)
            {
                DateTime? lastExistingDateTime = lastRecord.ValueDate;
                RatesTable resultRecord =
                    (from r in repository.context.RatesTable.Include(a => a.ForeignCurrency)
                     where r.Tenant == tenant
                     && r.ForeignCurrencyId == foreignCurrencyId
                     && r.BaseCurrencyId == baseCurrencyId
                     && r.ValueDate == lastExistingDateTime
                     select r).OrderByDescending(r => r.LogDateTime).FirstOrDefault();


                if (resultRecord != null)
                {
                    myResult = new RatesTablePM()
                    {
                        Id = resultRecord.Id,
                        Tenant = resultRecord.Tenant,
                        ValueDate = resultRecord.ValueDate,
                        Rate = resultRecord.Rate,
                        ForeignCurrencyId = resultRecord.ForeignCurrency.Id,
                        ForeignCurrencyCode = resultRecord.ForeignCurrency.Code,
                        ForeignCurrencyName = resultRecord.ForeignCurrency.EnglishName,
                        LogDateTime = resultRecord.LogDateTime,
                        BaseCurrencyId = baseCurrencyId,
                    };
                }
            }

            return myResult;
        }

        public double? GetLastRecordByValueDateAndExchangeRateId(int tenant, string foreignCurrencyId, string baseCurrencyId, DateTime? date, string glaccountId)
        {

                var glAccountQueryService = new GLAccountQueryService(tenant);
            var exchangeRateId = glAccountQueryService.GetExchangeRateIdById(glaccountId, tenant);            

            if (exchangeRateId == null || !SecurityUtility.CheckFeature("AdditionalCurrencyRate", "AdditionalCurrencyRate.Features.Menu", tenant))
                return GetLastRateByValueDate(tenant, foreignCurrencyId, baseCurrencyId, date)?.Rate;

            var ratesQuery = repository.context.RatesTable
                .Include(r => r.ForeignCurrency)
                .Join(repository.context.CurrencyRates,
                      r => r.Id,
                      er => er.ExchangeRateId,
                      (r, er) => new { r, er })
                .Join(repository.context.AdditionalCurrencyRates,
                      re => re.er.AdditionalCurrencyRateId,
                      b => b.Id,
                      (re, b) => new { re.r, re.er, b })
                .Where(reb => reb.r.Tenant == tenant
                              && reb.r.ForeignCurrencyId == foreignCurrencyId
                              && reb.r.BaseCurrencyId == baseCurrencyId
                              && reb.er.AdditionalCurrencyRateId == exchangeRateId);

            var lastRecord = ratesQuery
                .Where(reb => reb.r.ValueDate <= date)
                .OrderByDescending(reb => reb.r.ValueDate)
                .Select(reb => new { RateTable = reb.r, RateValue = reb.er.Rate })
                .FirstOrDefault();

            if (lastRecord == null)
            {
                lastRecord = ratesQuery
                    .OrderByDescending(reb => reb.r.ValueDate)
                    .ThenByDescending(reb => reb.r.LogDateTime)
                    .Select(reb => new { RateTable = reb.r, RateValue = reb.er.Rate })
                    .FirstOrDefault();
            }

            if (lastRecord != null)
            {
                var lastExistingDateTime = lastRecord.RateTable.ValueDate;

                var resultRecord = ratesQuery
                    .Where(reb => reb.r.ValueDate == lastExistingDateTime)
                    .OrderByDescending(reb => reb.r.LogDateTime)
                    .Select(reb => new { RateTable = reb.r, RateValue = reb.er.Rate })
                    .FirstOrDefault();

                return resultRecord?.RateValue ?? resultRecord?.RateTable?.Rate;
            }

            return null;
        }


    }
}
