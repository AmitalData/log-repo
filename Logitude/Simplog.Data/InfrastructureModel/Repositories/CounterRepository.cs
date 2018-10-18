using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class CounterRepository : IRepository<Counter>
    {
        IWebFreightContext webFreightContext;
        public CounterRepository()
        {
             webFreightContext = new WebFreightContext();
        }
        public CounterRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }
        public CounterRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }


       
        public IQueryable<Counter> GetCounters(int tenant)
        {
            IQueryable<Counter> result = (from a in context.Counters
                                            where a.Tenant == tenant
                                            select a);


            return result;
        }


      

        public Counter GetSingleCounter(string id, int tenant)
        {
            return context.Counters.Where(d => d.Tenant == tenant && d.Id == id).FirstOrDefault();
        }

        public Counter GetCounterByCode(string code, int tenant)
        {
            return context.Counters.Where(d => d.Tenant == tenant && d.Code == code).FirstOrDefault();
        }

        public List<Counter> GetCountersByCode(string code)
        {
            return context.Counters.Where(d => d.Code == code).ToList();
        }

        public bool IsCounterExist(string code, int tenant)
        {
            return context.Counters.Where(d => d.Tenant == tenant && d.Code == code).Any();
        }

        public void Add(Counter entity)
        {
            this.context.Counters.Add(entity);
        }

        public void Remove(Counter entity)
        {
            this.context.Counters.Remove(entity);
        }

        public void Update(Counter entity)
        {
            this.context.Counters.Attach(entity);
            this.context.SetAsModified(entity);
        }

        public List<Counter> All()
        {
            return this.context.Counters.ToList();
        }

        public IWebFreightContext context
        {
            get { return this.webFreightContext; }
        }

        public void SubmitChanges()
        {
            this.webFreightContext.SaveChanges();
        }


        public List<Counter> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Counter GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}