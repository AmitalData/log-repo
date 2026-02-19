using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using POCO = AmitalCloud.Infrastructure.Data.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class CounterRepository : IRepository<POCO.Counter,string>
    {
        IAmitalCloudContext currentContext;
        public CounterRepository()
        {
             currentContext = new AmitalCloudContext();
        }
        public CounterRepository(IAmitalCloudContext context)
        {
            currentContext = context;
        }
        public CounterRepository(int tenant)
        {
            currentContext = AmitalCloudContext.GetContext(tenant);
        }


       
        public IQueryable<POCO.Counter> GetCounters(int tenant)
        {
            IQueryable<POCO.Counter> result = (from a in context.Counters
                                            where a.Tenant == tenant
                                            select a);


            return result;
        }


      

        public POCO.Counter GetSingleCounter(string id, int tenant)
        {
            return context.Counters.Where(d => d.Tenant == tenant && d.Id == id).FirstOrDefault();
        }

        public POCO.Counter GetCounterByCode(string code, int tenant)
        {
            return context.Counters.Where(d => d.Tenant == tenant && d.Code == code).FirstOrDefault();
        }

        public List<POCO.Counter> GetCountersByCode(string code)
        {
            return context.Counters.Where(d => d.Code == code).ToList();
        }

        public bool IsCounterExist(string code, int tenant)
        {
            return context.Counters.Where(d => d.Tenant == tenant && d.Code == code).Any();
        }

        public void Add(POCO.Counter entity)
        {
            this.context.Counters.Add(entity);
        }

        public void Remove(POCO.Counter entity)
        {
            this.context.Counters.Remove(entity);
        }

        public void Update(POCO.Counter entity)
        {
            this.context.Counters.Attach(entity);
            this.context.SetAsModified(entity);
        }

        public List<POCO.Counter> All()
        {
            return this.context.Counters.ToList();
        }

        public IAmitalCloudContext context
        {
            get { return this.currentContext; }
        }

        public void SubmitChanges()
        {
            this.currentContext.SaveChanges();
        }


        public List<POCO.Counter> GetMulti(IEntityKeyFields<POCO.Counter,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public POCO.Counter GetSingle(IEntityKeyFields<POCO.Counter,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}