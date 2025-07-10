using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CustomerTeamRepository:IRepository<CustomerTeam>
    {
        ICommonDataContext commonDataContext;

        public CustomerTeamRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CustomerTeamRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CustomerTeamRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public int GetCustomerTeamsCount(int tenant)
        {
            return (from record in context.CustomerTeams where record.Tenant == tenant select record).Count();
        }

        public IQueryable<CustomerTeam> GetCustomerTeams(int tenant)
        {
            return (from record in context.CustomerTeams where record.Tenant == tenant select record);
        }

        public IQueryable<CustomerTeam> GetCustomerTeams()
        {
            return context.CustomerTeams;
        }

        public CustomerTeam GetSingleCustomerTeam(string id, int tenant)
        {
            return (from record in context.CustomerTeams where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public void Add(CustomerTeam entity)
        {
            context.CustomerTeams.Add(entity);
        }

        public void Remove(CustomerTeam entity)
        {
            try
            {
                context.CustomerTeams.Attach(entity);
            }
            catch { }
            context.CustomerTeams.Remove(entity);
        }

        public void Update(CustomerTeam entity)
        {
            try
            {
                context.CustomerTeams.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<CustomerTeam> All()
        {
            return context.CustomerTeams.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CustomerTeam> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CustomerTeam GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
