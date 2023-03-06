using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class DeploymentPackageExecutionLogRepository : IRepository<DeploymentPackageExecutionLog>
    {
        IWebFreightContext webFreightContext;
        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public DeploymentPackageExecutionLogRepository()
        {
            webFreightContext = new WebFreightContext();
        }

        public DeploymentPackageExecutionLogRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public DeploymentPackageExecutionLogRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public IQueryable<DeploymentPackageExecutionLog> GetDeploymentPackageExecutionLogs(int tenant)
        {
            return (from record in context.DeploymentPackageExecutionLogs where record.Tenant == tenant select record);
        }

        public DeploymentPackageExecutionLog GetSingleDeploymentPackageExecutionLog(string id, int tenant)
        {
            return (from record in context.DeploymentPackageExecutionLogs where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }



        public IQueryable<DeploymentPackageExecutionLog> GetAllDeploymentPackageExecutionLogs()
        {
            return (from record in context.DeploymentPackageExecutionLogs select record);
        }

        public void Add(DeploymentPackageExecutionLog entity)
        {
            context.DeploymentPackageExecutionLogs.Add(entity);
        }

        public void Remove(DeploymentPackageExecutionLog entity)
        {
            try
            {
                context.DeploymentPackageExecutionLogs.Attach(entity);
            }
            catch { };
            context.DeploymentPackageExecutionLogs.Remove(entity);
        }

        public void Update(DeploymentPackageExecutionLog entity)
        {
            try
            {
                context.DeploymentPackageExecutionLogs.Attach(entity);
            }
            catch { };
            context.SetAsModified(entity);
        }

        public List<DeploymentPackageExecutionLog> All()
        {
            return context.DeploymentPackageExecutionLogs.ToList();
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<DeploymentPackageExecutionLog> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public DeploymentPackageExecutionLog GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

    }
}