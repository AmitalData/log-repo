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
    public class BluesnapTransactionRepository : IRepository<BluesnapTransaction>
    {
        IGlobalContext globalContext;
        public BluesnapTransactionRepository()
        {
            globalContext = GlobalContext.GetContext();
        }

        public BluesnapTransactionRepository(IGlobalContext context)
        {
            globalContext = context;
        }

        public BluesnapTransaction GetSingleBluesnapTransaction(string Id, int tenant)
        {
            return (from a in context.BluesnapTransactions
                    where a.Id == Id
                    select a).FirstOrDefault();
        }

        public IQueryable<BluesnapTransaction> GetBluesnapTransactions(int tenant)
        {
            return from a in context.BluesnapTransactions
                   select a;
        }

        public void Add(BluesnapTransaction entity)
        {
            context.BluesnapTransactions.Add(entity);
        }

        public void Remove(BluesnapTransaction entity)
        {
            context.BluesnapTransactions.Attach(entity);
            context.BluesnapTransactions.Remove(entity);
        }

        public void Update(BluesnapTransaction entity)
        {
            context.BluesnapTransactions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BluesnapTransaction> All()
        {
            return context.BluesnapTransactions.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<BluesnapTransaction> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public BluesnapTransaction GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
