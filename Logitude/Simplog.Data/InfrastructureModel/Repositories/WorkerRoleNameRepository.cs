

using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class WorkerRoleNameRepository : IRepository<WorkerRoleName>
    {
        public IWebFreightContext webFreightContext;

        public WorkerRoleNameRepository()
        {
            webFreightContext = new WebFreightContext();
        }

        public WorkerRoleNameRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public WorkerRoleNameRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }
        public void Add(WorkerRoleName entity)
        {
            context.WorkerRoleNames.Add(entity);
        }

        public void Remove(WorkerRoleName entity)
        {
            context.WorkerRoleNames.Attach(entity);
            context.WorkerRoleNames.Remove(entity);
        }

        public void Update(WorkerRoleName entity)
        {
            context.WorkerRoleNames.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<WorkerRoleName> All()
        {
            return context.WorkerRoleNames.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<WorkerRoleName> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public WorkerRoleName GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<WorkerRoleName> GetWorkerRoleNames()
        {
            IQueryable<WorkerRoleName> result = (from w in context.WorkerRoleNames select w);

            return result;
        }

        public WorkerRoleName GetSingleWorkerRoleName(string name)
        {
            return context.WorkerRoleNames.Where(w => w.Name == name).FirstOrDefault();
        }

        public WorkerRoleName GetLastAddedWorkerRoleName()
        {
            return context.WorkerRoleNames.OrderByDescending(d=>d.CreateDate).FirstOrDefault();
        }
    }
}
