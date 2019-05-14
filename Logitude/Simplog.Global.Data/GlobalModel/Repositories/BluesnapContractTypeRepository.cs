using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.Repositories
{


    public class BluesnapContractTypeRepository : IRepository<BluesnapContractType>
    {
        IGlobalContext globalContext;

        public BluesnapContractTypeRepository()
        {
            globalContext = GlobalContext.GetContext();
        }
        public BluesnapContractTypeRepository(IGlobalContext context)
        {
            globalContext = context;
        }
        public BluesnapContractTypeRepository(int tenant)
        {
            globalContext = GlobalContext.GetContext(tenant);
        }
        public BluesnapContractType GetSingleBluesnapContractType(string code)
        {
            return (from a in context.BluesnapContractTypes
                    where a.Code == code
                    select a).FirstOrDefault();
        }
        public IQueryable<BluesnapContractType> GetAll()
        {
            return context.BluesnapContractTypes;
        }
        public IQueryable<BluesnapContractType> GetBluesnapContractTypes()
        {
            return context.BluesnapContractTypes;
        }

        public void Add(BluesnapContractType entity)
        {
            context.BluesnapContractTypes.Add(entity);
        }

        public void Remove(BluesnapContractType entity)
        {
            context.BluesnapContractTypes.Attach(entity);
            context.BluesnapContractTypes.Remove(entity);
        }

        public void Update(BluesnapContractType entity)
        {
            context.BluesnapContractTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BluesnapContractType> All()
        {
            return context.BluesnapContractTypes.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<BluesnapContractType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public BluesnapContractType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }

}
