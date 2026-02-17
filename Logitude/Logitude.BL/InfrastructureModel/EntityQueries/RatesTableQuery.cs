using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class RatesTableQuery
    {
        RatesTableRepository repository;
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

        public RatesTablePM GetSinglePM(string id, int tenent)
        {
            RatesTablePM instance = (from a in repository.context.RatesTable.Include("ForeignCurrency")
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
            IQueryable<RatesTableList> result = from entity in iQueryable.Include("ForeignCurrency")
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
                                                };
            return result;
        }


        /* Last Rates */
        public LastRate GetLastRecord(int tenant, string foreignCurrencyId, string baseCurrencyId)
        {
            List<RatesTable> myList = (from r in repository.context.RatesTable.Include("ForeignCurrency")
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
        public LastRate GetLastRecordByValueDate(int tenant, string foreignCurrencyId, string baseCurrencyId, DateTime? date)
        {
            LastRate myResult = null;
            
            if (date != null)
            {
                DateTime? iDateValue = date.Value.Date;

                IQueryable<RatesTable> iQuery = from r in repository.context.RatesTable.Include("ForeignCurrency")
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
                        myResult = new LastRate()
                        {
                            Id = myRecord.Id,
                            Tenant = myRecord.Tenant,
                            ValueDate = myRecord.ValueDate,
                            Rate = myRecord.Rate,
                            ForeignCurrencyId = myRecord.ForeignCurrency.Id,
                            ForeignCurrencyCode = myRecord.ForeignCurrency.Code,
                            ForeignCurrencyName = myRecord.ForeignCurrency.EnglishName,
                            LogDateTime = myRecord.LogDateTime,
                            HistoryCount = iQuery.Count(),
                            BaseCurrencyId = baseCurrencyId,
                        };
                    }
                }
            }

            return myResult;
        }

        public RatesTablePM GetLastRateByValueDate(int tenant, string foreignCurrencyId, string baseCurrencyId, DateTime? date)
        {
            RatesTablePM myResult = null;

            RatesTable lastRecord =
                (from r in repository.context.RatesTable.Include("ForeignCurrency")
                 where r.Tenant == tenant
                 && r.ForeignCurrencyId == foreignCurrencyId
                 && r.BaseCurrencyId == baseCurrencyId
                 && r.ValueDate <= date
                 select r).OrderByDescending(r => r.ValueDate).FirstOrDefault();


            if (lastRecord == null)
            {
                lastRecord =
                    (from r in repository.context.RatesTable.Include("ForeignCurrency")
                     where r.Tenant == tenant
                     && r.ForeignCurrencyId == foreignCurrencyId
                     && r.BaseCurrencyId == baseCurrencyId
                     select r).OrderByDescending(r => r.ValueDate).ThenByDescending(o => o.LogDateTime).FirstOrDefault();
            }


            if (lastRecord != null)
            {
                DateTime? lastExistingDateTime = lastRecord.ValueDate;
                RatesTable resultRecord =
                    (from r in repository.context.RatesTable.Include("ForeignCurrency")
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
    }
}
