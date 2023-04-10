using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.Repositories;

using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class CounterDefinitionQuery
    {
        CounterDefinitionRepository repository;
        public CounterDefinitionQuery()
        {
            repository = new CounterDefinitionRepository(); 
        }

        public CounterDefinitionQuery(int tenant)
        {
            repository = new CounterDefinitionRepository(tenant);
        }

        public CounterDefinitionQuery(CounterDefinitionRepository counterDefinitionRepository)
        {
            repository = counterDefinitionRepository;
        }


        public IQueryable<CounterDefinitionPM> GetCounterDefinitionsByTenant(int tenant)
        {
            
            IQueryable<CounterDefinitionPM> result = (from a in repository.context.CounterDefinitions
                                                      where a.Tenant == tenant && !a.InActive
                                                      select new CounterDefinitionPM()
													  {
														  Id = a.Id,
														  CounterId = a.CounterId,
														  Parameter1 = a.Parameter1,
														  Parameter2 = a.Parameter2,
														  Prefix = a.Prefix,
														  Tenant = a.Tenant,
														  UniquePerPrefix = a.UniquePerPrefix,
														  StartNumber = a.StartNumber,
														  StartNumber_Old = a.StartNumber,
														  CounterSize = a.CounterSize,
														  Suffix = a.Suffix,
                                                          InActive = a.InActive,
                                                          UsePerBranch = a.UsePerBranch,
                                                          IsCustomized = a.IsCustomized,
                                                      }

       );
            //List<CounterDefinitionPM> defList = result.Where(
            //    c => c.Tenant == tenant &&
            //     c.CounterId == "1-6"
            //     ).ToList();

            return result;
        }

        public bool CheckIfUsed(string counterId, int tenant)
        {
            CounterLastNumberRepository counterLastNumberRep = new CounterLastNumberRepository(tenant);
            return counterLastNumberRep.CheckIfExists(counterId, tenant);
        }

        public IQueryable<CounterDefinitionPM> GetCounterDefinitionsByCounterId(string counterId, int tenant)
        {
            IQueryable<CounterDefinitionPM> result
                = (from a in repository.context.CounterDefinitions
                   where a.Tenant == tenant && a.CounterId == counterId && !a.InActive
                   select new CounterDefinitionPM()
                   {
                       Id = a.Id,
                       CounterId = a.CounterId,
                       Parameter1 = a.Parameter1,
                       Parameter2 = a.Parameter2,
                       Prefix = a.Prefix,
                       Tenant = a.Tenant,
                       UniquePerPrefix = a.UniquePerPrefix,
                       StartNumber = a.StartNumber,
                       StartNumber_Old = a.StartNumber,
					   CounterSize = a.CounterSize,
					   Suffix = a.Suffix,
                       InActive = a.InActive,
                       UsePerBranch = a.UsePerBranch,
                       IsCustomized = a.IsCustomized
                   });

            return result;
        }


        public IQueryable<CounterDefinitionPM> GetCustomizedCounterDefinitionsByCounterId(string counterId, int tenant)
        {
            IQueryable<CounterDefinitionPM> result
                = (from a in repository.context.CounterDefinitions
                   where a.Tenant == tenant && a.CounterId == counterId && a.IsCustomized && !a.InActive
                   select new CounterDefinitionPM()
                   {
                       Id = a.Id,
                       CounterId = a.CounterId,
                       Parameter1 = a.Parameter1,
                       Parameter2 = a.Parameter2,
                       Prefix = a.Prefix,
                       Tenant = a.Tenant,
                       UniquePerPrefix = a.UniquePerPrefix,
                       StartNumber = a.StartNumber,
                       StartNumber_Old = a.StartNumber,
                       CounterSize = a.CounterSize,
                       Suffix = a.Suffix,
                       InActive = a.InActive,
                       UsePerBranch = a.UsePerBranch,
                       IsCustomized = a.IsCustomized
                   });

            return result;
        }
    }
}