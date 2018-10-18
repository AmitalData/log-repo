


using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class SessionPolicyRepository : IRepository<SessionPolicy>
    {
        IGlobalContext globalContext;
        public SessionPolicyRepository()
        {
            globalContext = GlobalContext.GetContext();
        }

        public SessionPolicyRepository(IGlobalContext context)
        {
            globalContext = context;
        }


        public SessionPolicy GetSingleSessionPolicy()
        {
            return (from a in context.SessionPolicies select a).FirstOrDefault();
        }

        public IQueryable<SessionPolicy> GetAllSessionPolicies()
        {
            return from a in context.SessionPolicies
                   select a;
        }

        public void Add(SessionPolicy entity)
        {
            context.SessionPolicies.Add(entity);
        }

        public void Remove(SessionPolicy entity)
        {
            context.SessionPolicies.Attach(entity);
            context.SessionPolicies.Remove(entity);
        }

        public void Update(SessionPolicy entity)
        {
            context.SessionPolicies.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SessionPolicy> All()
        {
            return context.SessionPolicies.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<SessionPolicy> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public SessionPolicy GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}