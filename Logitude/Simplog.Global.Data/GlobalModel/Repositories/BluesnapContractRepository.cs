using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class BluesnapContractRepository : IRepository<BluesnapContract>
    {
        IGlobalContext globalContext;
        public BluesnapContractRepository()
        {
            globalContext = GlobalContext.GetContext();
        }

        public BluesnapContractRepository(IGlobalContext context)
        {
            globalContext = context;
        }

        public BluesnapContract GetSingleBluesnapContract(string Id, int tenant)
        {
            return (from a in context.BluesnapContracts.Include("BluesnapContractType")
                    where a.Id == Id
                    select a).FirstOrDefault();
        }

        public IQueryable<BluesnapContract> GetBluesnapContracts(int tenant)
        {
            return from a in context.BluesnapContracts.Include("BluesnapContractType")
                   select a;
        }

        public void Add(BluesnapContract entity)
        {
            context.BluesnapContracts.Add(entity);
        }

        public void Remove(BluesnapContract entity)
        {
            context.BluesnapContracts.Attach(entity);
            context.BluesnapContracts.Remove(entity);
        }

        public void Update(BluesnapContract entity)
        {
            context.BluesnapContracts.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BluesnapContract> All()
        {
            return context.BluesnapContracts.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<BluesnapContract> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public BluesnapContract GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
