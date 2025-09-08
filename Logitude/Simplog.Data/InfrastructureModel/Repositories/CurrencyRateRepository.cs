using System;
using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
   public class CurrencyRateRepository:IRepository<CurrencyRate>
   {
        IWebFreightContext webFreightContext;

        public CurrencyRateRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public CurrencyRateRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public CurrencyRateRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public  CurrencyRate GetSingle(string id, int tenant)
        {
            return (from a in context.CurrencyRates
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public List<CurrencyRate> GetSingleByExchangeRateId(string exchangeRateId)
        {
            return (from a in context.CurrencyRates
                    where a.ExchangeRateId == exchangeRateId
                    select a).ToList();
        }
        public List<CurrencyRate> GetSingleByExchangeRateIdAndAdditionalCurrencyRateId(string exchangeRateId,string additionalCurrencyRateId,int tenant)
        {
            return (from a in context.CurrencyRates
                    where a.ExchangeRateId == exchangeRateId && a.AdditionalCurrencyRateId  == additionalCurrencyRateId && a.Tenant == tenant
                    select a).ToList();
        }

        public List<CurrencyRate> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CurrencyRate GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<CurrencyRate> GetAll(int tenant)
        {
            return from a in context.CurrencyRates  
                   where a.Tenant == tenant
                   select a;
        }
		 		                 
        public void Add(CurrencyRate entity)
        {
            context.CurrencyRates.Add(entity);
        }

        public void Remove(CurrencyRate entity)
        {
            context.CurrencyRates.Attach(entity);
            context.CurrencyRates.Remove(entity);
        }

        public void Update(CurrencyRate entity)
        {
            context.CurrencyRates.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CurrencyRate> All()
        {
            return context.CurrencyRates.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 