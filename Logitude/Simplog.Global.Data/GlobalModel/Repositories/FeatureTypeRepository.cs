using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class FeatureTypeRepository:IRepository<FeatureType>
    {

		IGlobalContext globalContext;

		public IGlobalContext context
		{
			get { return globalContext; }
		}
		public FeatureTypeRepository(IGlobalContext context)
		{
			globalContext = context;
		}

		public FeatureTypeRepository()
		{
			globalContext = GlobalContext.GetContext();
		}

		public IQueryable<FeatureType> GetFeatureTypes()
        {
            return context.FeatureTypes;
        }

        public FeatureType GetSingleFeatureType(string code)
        {
            return (from a in context.FeatureTypes
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public void Add(FeatureType entity)
        {
			context.FeatureTypes.Add(entity);
        }

        public void Remove(FeatureType entity)
        {
			context.FeatureTypes.Remove(entity);
        }

        public void Update(FeatureType entity)
        {
			context.FeatureTypes.Attach(entity);
			context.SetAsModified(entity);
        }

        public List<FeatureType> All()
        {
            return context.FeatureTypes.ToList();
        }

        public void SubmitChanges()
        {
			context.SaveChanges();
        }


        public List<FeatureType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public FeatureType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}