using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class FeatureAccessLevelRepository : IRepository<FeatureAccessLevel>
    {

		IGlobalContext globalContext;

		public IGlobalContext context
		{
			get { return globalContext; }
		}
		public FeatureAccessLevelRepository(IGlobalContext context)
		{
			globalContext = context;
		}

		public FeatureAccessLevelRepository()
		{
			globalContext = GlobalContext.GetContext();
		}


		public FeatureAccessLevel GetSingleFeatureAccessLevel(string code)
        {
            return (from a in this.context.FeatureAccessLevels where a.Code == code select a).FirstOrDefault();
        }

        public IQueryable<FeatureAccessLevel> GetFeatureAccessLevels()
        {
            return context.FeatureAccessLevels;
        }
        public IQueryable<FeatureAccessLevel> GetAll()
        {
            return context.FeatureAccessLevels;
        }

        public void Add(FeatureAccessLevel entity)
        {
            context.FeatureAccessLevels.Add(entity);
        }

        public void Remove(FeatureAccessLevel entity)
        {
            context.FeatureAccessLevels.Remove(entity);
        }

        public void Update(FeatureAccessLevel entity)
        {
            context.FeatureAccessLevels.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<FeatureAccessLevel> All()
        {
            return context.FeatureAccessLevels.ToList();
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<FeatureAccessLevel> GetMulti(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public FeatureAccessLevel GetSingle(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
