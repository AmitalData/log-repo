using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class BatchServicesDefinitionRepository : IRepository<BatchServicesDefinition>
    {

         IGlobalContext globalContext;
        public BatchServicesDefinitionRepository()
        {
            globalContext = GlobalContext.GetContext();
        }

        public BatchServicesDefinitionRepository(IGlobalContext context)
        {
            globalContext = context;
        }



        public BatchServicesDefinition GetSingleBatchServicesDefinition(string Code)
        {
            BatchServicesDefinition item = context.BatchServicesDefinitions.Where(d => d.Code == Code).FirstOrDefault();
            return item;
        }
        public string GetSingleBatchServicesDefinitionByQueueBase(string QueueBase, string queueDefinitionGroup)
        {
            string item = context.BatchServicesDefinitions.FirstOrDefault(d => d.QueueBase != null && d.QueueBase == QueueBase && d.Code.EndsWith(queueDefinitionGroup))?.Code;
            return item;
        }

        public IQueryable<BatchServicesDefinition> GetAllBatchServicesDefinitions()
        {
            return from a in context.BatchServicesDefinitions
                   select a;
        }
        public List<string> GetAllBatchServicesDefinitionsByQueueBase(string code, bool fromCache = true)
        {
            string entityKeyString = "GetAllBatchServicesDefinitionsByQueueBase_" + code;

            var items = CacheManager.GetOrInsertNewObject<List<string>>(
                entityKeyString, () =>
                {
                    return context.BatchServicesDefinitions
                        .Where(d => d.Code == code || d.QueueBase == code)
                        .Select(d => d.Code)
                        .ToList();
                },
                fromCache

            );

            return items;
        }

        public IQueryable<BatchServicesDefinition> GetBatchServicesDefinitions()
        {
            return from a in context.BatchServicesDefinitions
                   select a;
        }

        public IQueryable<BatchServicesDefinition> GetAllActiveBatchServicesDefinitions()
        {
            //return from a in context.BatchServicesDefinitions where a.InActive == false
            //       select a;
            return null;
        }


        public void Add(BatchServicesDefinition entity)
        {
            context.BatchServicesDefinitions.Add(entity);
        }

        public void Remove(BatchServicesDefinition entity)
        {
            context.BatchServicesDefinitions.Attach(entity);
            context.BatchServicesDefinitions.Remove(entity);
        }

        public void Update(BatchServicesDefinition entity)
        {
            context.BatchServicesDefinitions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BatchServicesDefinition> All()
        {
            return context.BatchServicesDefinitions.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<BatchServicesDefinition> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public BatchServicesDefinition GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }



    }
}
