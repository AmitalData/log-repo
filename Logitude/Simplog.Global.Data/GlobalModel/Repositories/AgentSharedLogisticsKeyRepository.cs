using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class AgentSharedLogisticsKeyRepository : IRepository<AgentSharedLogisticsKey>
    {
        IGlobalContext globalContext;
        public AgentSharedLogisticsKeyRepository()
        {
            globalContext = GlobalContext.GetContext();
        }

        public AgentSharedLogisticsKeyRepository(IGlobalContext context)
        {
            globalContext = context;
        }



        public AgentSharedLogisticsKey GetSingleAgentSharedLogisticsKey(string key)
        {
            return (from a in context.AgentSharedLogisticsKeys
                    where a.SharedKey == key
                    select a).FirstOrDefault();
        }

        public IQueryable<AgentSharedLogisticsKey> GetAllAgentSharedLogisticsKeys()
        {
            return from a in context.AgentSharedLogisticsKeys
                   select a;
        }

        public void Add(AgentSharedLogisticsKey entity)
        {
            context.AgentSharedLogisticsKeys.Add(entity);
        }

        public void Remove(AgentSharedLogisticsKey entity)
        {
            context.AgentSharedLogisticsKeys.Attach(entity);
            context.AgentSharedLogisticsKeys.Remove(entity);
        }

        public void Update(AgentSharedLogisticsKey entity)
        {
            context.AgentSharedLogisticsKeys.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AgentSharedLogisticsKey> All()
        {
            return context.AgentSharedLogisticsKeys.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<AgentSharedLogisticsKey> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public AgentSharedLogisticsKey GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}