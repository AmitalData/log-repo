using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class BatchServicesDefinitionModsRepository : IRepository<BatchServicesDefinitionMods>
    {

         IGlobalContext globalContext;
        public BatchServicesDefinitionModsRepository()
        {
            globalContext = GlobalContext.GetContext();
        }

        public BatchServicesDefinitionModsRepository(IGlobalContext context)
        {
            globalContext = context;
        }



        public BatchServicesDefinitionMods GetSingleBatchServicesDefinitionMod(string Code)
        {
            BatchServicesDefinitionMods item = context.BatchServicesDefinitionMods.Where(d => d.Code == Code).FirstOrDefault();
            return item;
        }

        public IQueryable<BatchServicesDefinitionMods> GetAllBatchServicesDefinitionMods()
        {
            return from a in context.BatchServicesDefinitionMods
                   select a;
        }

        public IQueryable<BatchServicesDefinitionMods> GetAllActiveBatchServicesDefinitionMods()
        {
            return from a in context.BatchServicesDefinitionMods where a.InActive == false
                   select a;
        }


        public void Add(BatchServicesDefinitionMods entity)
        {
            context.BatchServicesDefinitionMods.Add(entity);
        }

        public void Remove(BatchServicesDefinitionMods entity)
        {
            context.BatchServicesDefinitionMods.Attach(entity);
            context.BatchServicesDefinitionMods.Remove(entity);
        }

        public void Update(BatchServicesDefinitionMods entity)
        {
            context.BatchServicesDefinitionMods.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BatchServicesDefinitionMods> All()
        {
            return context.BatchServicesDefinitionMods.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<BatchServicesDefinitionMods> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public BatchServicesDefinitionMods GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }



    }
}
