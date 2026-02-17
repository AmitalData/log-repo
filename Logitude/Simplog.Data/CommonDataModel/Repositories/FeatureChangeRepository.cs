using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class FeatureChangeRepository : IRepository<FeatureChange>
    {
        ICommonDataContext commonDataContext;
        public FeatureChangeRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }
        public FeatureChangeRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public FeatureChange GetSingleFeatureChanges(string id)
        {
            return (from a in context.FeatureChanges
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public IQueryable<FeatureChange> GetFeatureChangesByTenant(int tenant)
        {
            return (from a in context.FeatureChanges
                    where a.Tenant == tenant
                    select a);
        }

        public void Add(FeatureChange entity)
        {
            context.FeatureChanges.Add(entity);
        }

        public void Remove(FeatureChange entity)
        {
            context.FeatureChanges.Attach(entity);
            context.FeatureChanges.Remove(entity);
        }

        public void Update(FeatureChange entity)
        {
            context.FeatureChanges.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<FeatureChange> All()
        {
            return context.FeatureChanges.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<FeatureChange> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public FeatureChange GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
