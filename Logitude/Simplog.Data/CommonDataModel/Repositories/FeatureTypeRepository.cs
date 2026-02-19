using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class FeatureTypeRepository:IRepository<FeatureType>
    {

        ICommonDataContext commonDataContext;
        public FeatureTypeRepository()
        {
            commonDataContext = new CommonDataContext();

        }
        public FeatureTypeRepository(ICommonDataContext context)
        {
            commonDataContext = context;

        }
        public FeatureTypeRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
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
            commonDataContext.FeatureTypes.Add(entity);
        }

        public void Remove(FeatureType entity)
        {
            commonDataContext.FeatureTypes.Remove(entity);
        }

        public void Update(FeatureType entity)
        {
            commonDataContext.FeatureTypes.Attach(entity);
            commonDataContext.SetAsModified(entity);
        }

        public List<FeatureType> All()
        {
            return context.FeatureTypes.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            commonDataContext.SaveChanges();
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