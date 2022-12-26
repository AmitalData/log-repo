using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class DeploymentPackageRepository : IRepository<DeploymentPackage>
    {
        IWebFreightContext webFreightContext;

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public DeploymentPackageRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public DeploymentPackageRepository()
        { }

        public DeploymentPackage GetSingleDeploymentPackage(string id, int tenant)
        {
            return (from a in context.DeploymentPackages
                    where a.Tenant == tenant && a.Id == id
                    select a).FirstOrDefault();
        }
        public DeploymentPackageRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public void Add(DeploymentPackage entity)
        {
            context.DeploymentPackages.Add(entity);
        }

        public void Remove(DeploymentPackage entity)
        {
            context.DeploymentPackages.Attach(entity);
            context.DeploymentPackages.Remove(entity);
        }

        public void Update(DeploymentPackage entity)
        {
            context.DeploymentPackages.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DeploymentPackage> All()
        {
            return context.DeploymentPackages.ToList();
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<DeploymentPackage> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public DeploymentPackage GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<DeploymentPackage> GetDeploymentPackages(int tenant)
        {
            return context.DeploymentPackages.Where(d => d.Tenant == tenant);
        }
    }
}
