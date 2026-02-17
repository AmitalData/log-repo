using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class FeaturePackageTypeRepository : IRepository<FeaturePackageType>
    {

		IGlobalContext globalContext;

		public IGlobalContext context
		{
			get { return globalContext; }
		}
		public FeaturePackageTypeRepository(IGlobalContext context)
		{
			globalContext = context;
		}

		public FeaturePackageTypeRepository()
		{
			globalContext = GlobalContext.GetContext();
		}



		public IQueryable<FeaturePackageType> GetFeaturePackageTypes()
        {
            return (from d in context.FeaturePackageTypes select d);
        }
        public IQueryable<FeaturePackageType> GetAll()
        {
            return (from d in context.FeaturePackageTypes select d);
        }

        public FeaturePackageType GetSingleFeaturePackageType(string code)
        {
            return (from record in context.FeaturePackageTypes where record.Code == code select record).FirstOrDefault();
        }
        
        public void Add(FeaturePackageType entity)
        {
            this.context.FeaturePackageTypes.Add(entity);
        }

        public void Remove(FeaturePackageType entity)
        {
            this.context.FeaturePackageTypes.Attach(entity);
            this.context.FeaturePackageTypes.Remove(entity);
        }

        public void Update(FeaturePackageType entity)
        {
            try
            {
                this.context.FeaturePackageTypes.Attach(entity);
            }
            catch { }
            this.context.SetAsModified(entity);
        }

        public List<FeaturePackageType> All()
        {
            return this.context.FeaturePackageTypes.ToList<FeaturePackageType>();
        }

 
        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }

        public List<FeaturePackageType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public FeaturePackageType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
