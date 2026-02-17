using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class RestrictionRepository:IRepository<Restriction>
    {
        ICommonDataContext commonDataContext;

        public RestrictionRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public RestrictionRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public RestrictionRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public Restriction GetSingleRestriction(string id,int tenant = 0)
        {
            return (from a in context.Restrictions.Include("ObjectField")
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public void Add(Restriction entity)
        {
            context.Restrictions.Add(entity);
        }

        public void Remove(Restriction entity)
        {
            context.Restrictions.Attach(entity);
            context.Restrictions.Remove(entity);
        }

        public void Update(Restriction entity)
        {
            context.Restrictions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Restriction> All()
        {
            return context.Restrictions.ToList();
        }

        public ICommonDataContext context
        {
            get {return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<Restriction> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Restriction GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}