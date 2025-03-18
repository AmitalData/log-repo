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
   public partial class AdditionalCurrencyRateQuery
    {
        AdditionalCurrencyRateRepository repository;

        public AdditionalCurrencyRateQuery(int tenant = 0)
        {
            repository = tenant == 0
                ? new AdditionalCurrencyRateRepository()
                : new AdditionalCurrencyRateRepository(tenant);
        }

        public AdditionalCurrencyRateQuery(AdditionalCurrencyRateRepository additionalCurrencyRateRepository)
        {
            repository = additionalCurrencyRateRepository ?? throw new ArgumentNullException(nameof(additionalCurrencyRateRepository));
        }

        public AdditionalCurrencyRatePM GetSinglePM(string id, int tenant)
        {
            AdditionalCurrencyRatePM instance = (from a in repository.context.AdditionalCurrencyRates
                                     where a.Tenant == tenant && a.Id == id
                                     select new AdditionalCurrencyRatePM()
                                     {
                                         Id = a.Id,
                                         Tenant = a.Tenant,
                                         CreateDate = a.CreateDate,
                                         CreatedByUserId = a.CreatedByUserId,
                                         UpdateDate = a.UpdateDate,
                                         UpdatedByUserId = a.UpdatedByUserId,
                                         SearchFields = a.SearchFields,
                                         Name = a.Name,
                                         RateCoefficient = a.RateCoefficient,
                                     }).FirstOrDefault();

            return instance;
        }

        public IQueryable<AdditionalCurrencyRateList> GetIQueryableEntityList(IQueryable<AdditionalCurrencyRate> iQueryable)
        {
            IQueryable<AdditionalCurrencyRateList> result = from a in iQueryable
                                                 select new AdditionalCurrencyRateList()
                                                 {
                                                     Id = a.Id,
                                                     Tenant = a.Tenant,
                                                     CreateDate = a.CreateDate,
                                                     CreatedByUserId = a.CreatedByUserId,
                                                     UpdateDate = a.UpdateDate,
                                                     UpdatedByUserId = a.UpdatedByUserId,
                                                     SearchFields = a.SearchFields,
                                                     Name = a.Name,
                                                     RateCoefficient = a.RateCoefficient,
                                                 };
            return result;
        }
    }
   
}
	 