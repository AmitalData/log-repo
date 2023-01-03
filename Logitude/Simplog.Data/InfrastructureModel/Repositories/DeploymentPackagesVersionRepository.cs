using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class DeploymentPackagesVersionRepository : IRepository<DeploymentPackagesVersion>
    {
        IWebFreightContext webFreightContext;

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public DeploymentPackagesVersionRepository()
        { }

        public DeploymentPackagesVersionRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public DeploymentPackagesVersionRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public DeploymentPackagesVersion GetSingleDeploymentPackagesVersion(string id, int tenant)
        {
            return (from a in context.DeploymentPackagesVersions
                    where a.Tenant == tenant && a.Id == id
                    select a).FirstOrDefault();
        }

        public void Add(DeploymentPackagesVersion entity)
        {
            context.DeploymentPackagesVersions.Add(entity);
        }

        public void Remove(DeploymentPackagesVersion entity)
        {
            context.DeploymentPackagesVersions.Attach(entity);
            context.DeploymentPackagesVersions.Remove(entity);
        }

        public void Update(DeploymentPackagesVersion entity)
        {
            context.DeploymentPackagesVersions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DeploymentPackagesVersion> All()
        {
            return context.DeploymentPackagesVersions.ToList();
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<DeploymentPackagesVersion> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public DeploymentPackagesVersion GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<DeploymentPackagesVersion> GetDeploymentPackagesVersions(int tenant)
        {
            return context.DeploymentPackagesVersions.Where(d => d.Tenant == tenant);
        }

        public int GetLastDeploymentPackagesVersionByDeploymentPackageId(string deploymentPackageId, int tenant)
        {
            var deploymentPackageVersions = context.DeploymentPackagesVersions.Where(d => d.Tenant == tenant && d.DeploymentPackageID == deploymentPackageId).ToList();
            if (deploymentPackageVersions == null || deploymentPackageVersions.Count == 0) return 0;
            return deploymentPackageVersions.Max(d => d.VersionNumber);
        }
    }
}
