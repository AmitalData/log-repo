using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class AWBMessagesCCSTypeRepository : IRepository<AWBMessagesCCSType>
    {
        IGlobalContext globalContext;

        public AWBMessagesCCSTypeRepository()
        {
            globalContext = GlobalContext.GetContext();
        }

        public AWBMessagesCCSTypeRepository(int tenant)
        {
            globalContext = GlobalContext.GetContext();
        }

        public AWBMessagesCCSTypeRepository(IGlobalContext context)
        {
            globalContext = context;
        }

        public AWBMessagesCCSType GetSingleAWBMessagesCCSType(string code)
        {
            return (from a in context.AWBMessagesCCSTypes where a.Code == code select a).FirstOrDefault();
        }

        public IQueryable<AWBMessagesCCSType> GetAWBMessagesCCSTypes()
        {
            return from a in context.AWBMessagesCCSTypes select a;
        }
        public IQueryable<AWBMessagesCCSType> GetAll()
        {
            return from a in context.AWBMessagesCCSTypes select a;
        }

        public void Add(AWBMessagesCCSType entity)
        {
            context.AWBMessagesCCSTypes.Add(entity);
        }

        public void Remove(AWBMessagesCCSType entity)
        {
            context.AWBMessagesCCSTypes.Attach(entity);
            context.AWBMessagesCCSTypes.Remove(entity);
        }

        public void Update(AWBMessagesCCSType entity)
        {
            context.AWBMessagesCCSTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AWBMessagesCCSType> All()
        {
            return context.AWBMessagesCCSTypes.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<AWBMessagesCCSType> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public AWBMessagesCCSType GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
