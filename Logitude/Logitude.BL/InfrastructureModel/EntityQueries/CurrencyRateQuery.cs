using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{ 
   public partial class CurrencyRateQuery
   {
        CurrencyRateRepository repository;
        public CurrencyRateQuery()
        {
            repository = new CurrencyRateRepository();
        }
        public CurrencyRateQuery(int tenant)
        {
            repository = new CurrencyRateRepository(tenant);

        }
        public CurrencyRateQuery(CurrencyRateRepository currencyRateRepository)
        {
            repository = currencyRateRepository;
        }

        public CurrencyRatePM GetSinglePM(string id, int tenant)
        {
            CurrencyRatePM instance = (from a in repository.context.CurrencyRates
                                       where a.Tenant == tenant && a.Id == id
                                     select new CurrencyRatePM()
                                     {
                                         Id = a.Id,
                                         ExchangeRateId = a.ExchangeRateId,
                                         Tenant = a.Tenant,
                                         AdditionalCurrencyRateId = a.AdditionalCurrencyRateId,
                                         Rate = a.Rate,
                                     }).FirstOrDefault();

            return instance;
        }
    }
   
}
	 