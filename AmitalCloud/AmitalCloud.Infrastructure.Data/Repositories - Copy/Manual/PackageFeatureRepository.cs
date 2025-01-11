using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class PackageFeatureRepository : IRepository<PackageFeature,string>
    {
        IAmitalCloudContext currentContext;

        public PackageFeatureRepository()
        {
            currentContext = new AmitalCloudContext();
        }

        public PackageFeatureRepository(IAmitalCloudContext context)
        {
            currentContext = context;
        }

        public PackageFeatureRepository(int tenant)
        {
            currentContext = AmitalCloudContext.GetContext(tenant);
        }

        public PackageFeature GetSinglePackageFeature(string id)
        {
            return (from a in context.PackageFeatures
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public PackageFeature GetSinglePackageFeatureByPackageAndFeature(string packageCode, string featureId, int tenant)
        {
            return (from a in context.PackageFeatures
                    where a.PackageCode == packageCode && a.FeatureId == featureId && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public PackageFeature GetSinglePackageFeatureByPackageAndFeatureUCode(string packageCode, string featureUniqeCode, int tenant)
        {
            return (from a in context.PackageFeatures
                    where a.PackageCode == packageCode && a.FeatureUniqeCode == featureUniqeCode && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<PackageFeature> GetPackageFeaturesByTenant(int tenant)
        {
            return (from a in context.PackageFeatures
                    where a.Tenant == tenant
                    select a);
        }

        public void DeleteAllPackageFeaturesByPackage(string packageCode)
        {
            List<PackageFeature> packageFeatures = (from a in context.PackageFeatures
                                                    where a.PackageCode == packageCode
                                                    select a).ToList();
            foreach (PackageFeature packageFeature in packageFeatures)
            {
                this.Remove(packageFeature);
            }
        }

        public void Add(PackageFeature entity)
        {
            context.PackageFeatures.Add(entity);
        }

        public void Remove(PackageFeature entity)
        {
            context.PackageFeatures.Attach(entity);
            context.PackageFeatures.Remove(entity);
        }

        public void Update(PackageFeature entity)
        {
            context.PackageFeatures.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PackageFeature> All()
        {
            return context.PackageFeatures.ToList();
        }

        public IAmitalCloudContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<PackageFeature> GetMulti(IEntityKeyFields<PackageFeature,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public PackageFeature GetSingle(IEntityKeyFields<PackageFeature,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
