using System.Collections.Generic;
using System.Linq;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class PackageFeatureRepository : IRepository<PackageFeature>
	{
		IGlobalContext globalContext;

		public IGlobalContext context
		{
			get { return globalContext; }
		}
		public PackageFeatureRepository(IGlobalContext context)
		{
			globalContext = context;
		}

		public PackageFeatureRepository()
		{
			globalContext = GlobalContext.GetContext();
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

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<PackageFeature> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public PackageFeature GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
