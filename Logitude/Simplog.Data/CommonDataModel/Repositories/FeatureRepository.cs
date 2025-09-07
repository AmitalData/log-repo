using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class FeatureRepository:IRepository<Feature>
    {
        ICommonDataContext commonDataContext;



        public FeatureRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public FeatureRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }


        public Feature GetSingleFeatureByCode(string objectTableId, string code, int tenant)
        {
            return (from a in context.Features.Include("NameTextCode")
                    where a.ObjectTableId == objectTableId 
                    && a.Code == code
                    && (a.Tenant == tenant || a.Tenant == 0)
                    select a).FirstOrDefault();
        }

        public Feature GetSingleFeature(string id)
        {
            return (from a in context.Features
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public Feature GetSingleFeatureByUniqeCode(string featureUniqeCode)
        {
            return (from a in context.Features
                    where a.FeatureUniqeCode == featureUniqeCode
                    select a).FirstOrDefault();
        }

        public IQueryable<Feature> GetFeaturesByTenant(int tenant)
        {
            return (from a in context.Features.Include("NameTextCode")
                    where a.Tenant == tenant
                    select a);
        }

        public void Add(Feature entity)
        {
            context.Features.Add(entity);
        }

        public void Remove(Feature entity)
        {
            context.Features.Attach(entity);
            context.Features.Remove(entity);
        }

        public void Update(Feature entity)
        {
            context.Features.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Feature> All()
        {
            return context.Features.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<Feature> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Feature GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
