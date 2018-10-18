using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class CounterDefinitionRepository : IRepository<CounterDefinition>
    {
           IWebFreightContext webFreightContext;
        public CounterDefinitionRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }

        public CounterDefinitionRepository()
        {
               webFreightContext=new WebFreightContext(); 
        }
        public CounterDefinitionRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public IQueryable<CounterDefinition> GetCounterDefinitions()
        {
            return context.CounterDefinitions;
        }

    

        public IQueryable<CounterDefinition> GetCounterDefinitions(int tenant)
        {
            IQueryable<CounterDefinition> result = (from a in context.CounterDefinitions
                                                      where a.Tenant == tenant
                                                     select a
       );

            return result;
        }


        public CounterDefinition GetSingleCounterDefinition(int tenant,string counterId,string param1,string param2)
        {
            CounterDefinition result = (from a in context.CounterDefinitions
                                                    where a.Tenant == tenant && a.CounterId == counterId && a.Parameter1 == param1 && a.Parameter2 == param2
                                                    select a).FirstOrDefault();
      

            return result;
        }
      

        public CounterDefinition GetSingleCounterDefinition(string id, int tenant)
        {
            return context.CounterDefinitions.Where(d => d.Tenant == tenant && d.Id == id).FirstOrDefault();
        }

        public IQueryable<CounterDefinition> GetCounterDefinitionsByCounterId(string counterId, int tenant)
        {
            IQueryable<CounterDefinition> result = (from a in context.CounterDefinitions
                                                    where a.Tenant == tenant && a.CounterId == counterId
                                                    select a);


            return result;
        }

        public void Add(CounterDefinition entity)
        {
            
            context.CounterDefinitions.Add(entity);
        }

        public void Remove(CounterDefinition entity)
        {
            context.CounterDefinitions.Attach(entity);
            context.CounterDefinitions.Remove(entity);
        }

        public void Update(CounterDefinition entity)
        {
            context.CounterDefinitions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CounterDefinition> All()
        {
            return context.CounterDefinitions.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<CounterDefinition> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CounterDefinition GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}