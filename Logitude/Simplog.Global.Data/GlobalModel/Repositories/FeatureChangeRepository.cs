using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class FeatureChangeRepository : IRepository<FeatureChange>
    {

		IGlobalContext globalContext;

		public IGlobalContext context
		{
			get { return globalContext; }
		}
		public FeatureChangeRepository(IGlobalContext context)
		{
			globalContext = context;
		}

		public FeatureChangeRepository()
		{
			globalContext = GlobalContext.GetContext();
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
