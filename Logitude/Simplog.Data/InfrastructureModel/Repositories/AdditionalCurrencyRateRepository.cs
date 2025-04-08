using System;
using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public partial class AdditionalCurrencyRateRepository:IRepository<AdditionalCurrencyRate>
   {
        IWebFreightContext webFreightContext;

        public AdditionalCurrencyRateRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public AdditionalCurrencyRateRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public AdditionalCurrencyRateRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public AdditionalCurrencyRate GetSingle(string id, int tenant)
        {
            return webFreightContext.AdditionalCurrencyRates.Where(a => a.Id == id && a.Tenant == tenant).FirstOrDefault();
        }

        public IQueryable<AdditionalCurrencyRate> GetAdditionalCurrencyRate(int tenant)
        {
            return webFreightContext.AdditionalCurrencyRates.Where(a => a.Tenant == tenant);
        }

        public List<AdditionalCurrencyRate> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public AdditionalCurrencyRate GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public void Add(AdditionalCurrencyRate entity)
        {
            context.AdditionalCurrencyRates.Add(entity);
        }

        public void Remove(AdditionalCurrencyRate entity)
        {
            context.AdditionalCurrencyRates.Attach(entity);
            context.AdditionalCurrencyRates.Remove(entity);
        }

        public void Update(AdditionalCurrencyRate entity)
        {
            context.AdditionalCurrencyRates.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AdditionalCurrencyRate> All()
        {
            return context.AdditionalCurrencyRates.ToList();
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
	 