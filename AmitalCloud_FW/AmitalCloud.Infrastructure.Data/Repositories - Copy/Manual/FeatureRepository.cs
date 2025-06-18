using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class FeatureRepository:IRepository<Feature,string>
    {
        IAmitalCloudContext currentContext;
        public FeatureRepository()
        {
            currentContext = new AmitalCloudContext();
        }
        public FeatureRepository(IAmitalCloudContext context)
        {
            currentContext = context;
        }
        public FeatureRepository(int tenant)
        {
            currentContext = AmitalCloudContext.GetContext(tenant);
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
        public IAmitalCloudContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
        public List<Feature> GetMulti(IEntityKeyFields<Feature,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }
        public Feature GetSingle(IEntityKeyFields<Feature,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
