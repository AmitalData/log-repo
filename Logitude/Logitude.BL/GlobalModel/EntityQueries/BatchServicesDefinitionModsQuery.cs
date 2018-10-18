using Logitude.BL.GlobalModel.EntityLists;
using Logitude.BL.GlobalModel.EntityPMs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.GlobalModel.EntityQueries
{
   public class BatchServicesDefinitionModsQuery
    {

        BatchServicesDefinitionModsRepository repository;

        public BatchServicesDefinitionModsQuery()
        {
            repository = new BatchServicesDefinitionModsRepository(); 
        }

        public BatchServicesDefinitionModsQuery(int tenant)
        {
            repository = new BatchServicesDefinitionModsRepository();
        }

        public BatchServicesDefinitionModsQuery(BatchServicesDefinitionModsRepository BatchServicesDefinitionModsRepository)
        {
            repository = BatchServicesDefinitionModsRepository;
        }

        public BatchServicesDefinitionModsPM GetSingleBatchServicesDefinitionModsPM(string code)
        {
            return (from a in repository.context.BatchServicesDefinitionMods
                    where a.Code == code
                    select new BatchServicesDefinitionModsPM() { Code = a.Code}).FirstOrDefault();
        }

        public IQueryable<BatchServicesDefinitionModsList> GetIQueryableEntityList(IQueryable<BatchServicesDefinitionMods> iQueryable)
        {
            IQueryable<BatchServicesDefinitionModsList> result = from entity in iQueryable
                                                    select new BatchServicesDefinitionModsList()
                                                     {
                                                       
                                                         Code = entity.Code,
                                                         InActive = entity.InActive,
                                                         NumberOfThreads = entity.NumberOfThreads, 
                                                         //Parameter1 = entity.Parameter1,
                                                         //Parameter2 = entity.Parameter2
                                                     };
            return result;
        }
    }
}
