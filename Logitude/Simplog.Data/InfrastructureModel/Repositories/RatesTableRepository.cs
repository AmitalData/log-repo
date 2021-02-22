using System;
using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class RatesTableRepository:IRepository<RatesTable>
    {
        IWebFreightContext webFreightContext;

        public RatesTableRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public RatesTableRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public RatesTableRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public RatesTable GetSingleRatesTable(string id, int tenant)
        {
            RatesTable instance = (from a in context.RatesTable
                                   where a.Tenant == tenant
                                   && a.Id == id
                                   select a).FirstOrDefault();
            return instance;
        }

        //public IQueryable<RatesTable> GetRatesTables()
        //{
        //    return context.RatesTable;
        //}

        public IQueryable<RatesTable> GetRatesTables(int tenant)
        {
            IQueryable<RatesTable> rates = from a in context.RatesTable
                                           where a.Tenant == tenant
                                           select a;
            return rates;
        }
      
        public IQueryable<RatesTable> GetRateFunction(string baseCurrenyId, string foreignCurrencyId, DateTime rateDate, int tenent)
        {
            return context.RatesTable.Where(
                r => r.BaseCurrencyId == baseCurrenyId
                    && r.ForeignCurrencyId == foreignCurrencyId
                    && r.ValueDate == rateDate
                    && r.Tenant == tenent);
        }

        public RatesTable GetClosestRate(string baseCurrenyId, string foreignCurrencyId, int tenent)
        {
            var query = (from a in webFreightContext.RatesTable
             where a.BaseCurrencyId == baseCurrenyId
             && a.ForeignCurrencyId == foreignCurrencyId
             && a.Tenant == tenent
             orderby a.ValueDate descending
             select a).FirstOrDefault();

            return query;
        }

        public RatesTable GetExchageRateByValueAndDate(string baseCurrenyId, string foreignCurrencyId, DateTime? date, int tenent)
        {
            var query = (from a in webFreightContext.RatesTable
                         where a.BaseCurrencyId == baseCurrenyId
                         && a.ForeignCurrencyId == foreignCurrencyId
                         && a.Tenant == tenent
                         && a.ValueDate <= date
                         select a).OrderByDescending(a => a.ValueDate).ThenByDescending(a => a.Id).FirstOrDefault();
         
            return query;
        }

        public void Add(RatesTable entity)
        {
            context.RatesTable.Add(entity);
        }

        public void Remove(RatesTable entity)
        {
            context.RatesTable.Attach(entity);
            context.RatesTable.Remove(entity);
        }

        public void Update(RatesTable entity)
        {
            context.RatesTable.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<RatesTable> All()
        {
            return context.RatesTable.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<RatesTable> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public RatesTable GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}