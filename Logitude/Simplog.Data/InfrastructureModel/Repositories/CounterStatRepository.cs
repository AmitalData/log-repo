using System;
using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class CounterStatRepository: IRepository<CounterStat>
    {
        IWebFreightContext webFreightContext;
        public CounterStatRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }

        public CounterStatRepository()
        {
               webFreightContext=new WebFreightContext(); 
        }
        public CounterStatRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public IQueryable<CounterStat> GetCounterStats()
        {
            return context.CounterStats;
        }


        public CounterStat GetSingleCounterStat(string counterId, string prefix, int tenant)
        {
            if (!String.IsNullOrEmpty(prefix))
            {
                return context.CounterStats.Where(d => d.Tenant == tenant && d.CounterId == counterId && d.Prefix == prefix).FirstOrDefault();
            }
            else
            {
                return context.CounterStats.Where(d => d.Tenant == tenant && d.CounterId == counterId).FirstOrDefault();
            }
        }

        public List<CounterStat> GetCounterCounterStats(string counterId, int tenant)
        {
            return context.CounterStats.Where(d => d.Tenant == tenant && d.CounterId == counterId).ToList();
        }


        public void Add(CounterStat entity)
        {
            
            context.CounterStats.Add(entity);
        }

        public void Remove(CounterStat entity)
        {
            context.CounterStats.Attach(entity);
            context.CounterStats.Remove(entity);
        }

        public void Update(CounterStat entity)
        {
            context.CounterStats.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CounterStat> All()
        {
            return context.CounterStats.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<CounterStat> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CounterStat GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}