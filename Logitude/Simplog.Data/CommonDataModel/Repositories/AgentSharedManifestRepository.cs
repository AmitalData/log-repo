using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class AgentSharedManifestRepository : IRepository<AgentSharedManifest>
    {
        ICommonDataContext commonDataContext;

        public AgentSharedManifestRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public AgentSharedManifestRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public AgentSharedManifestRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<AgentSharedManifest> GetAgentSharedManifests(int tenant)
        {
            return (from record in context.AgentSharedManifests.Include("FromPort").Include("ToPort").Include("SharedManifestsStatus").Include("ShipmentLevel").Include("Agent.Card").Include("ShipmentType") where record.Tenant == tenant select record);
        }

        public AgentSharedManifest GetSingleAgentSharedManifest(string id, int tenant)
        {
            return (from record in context.AgentSharedManifests.Include("FromPort").Include("ToPort").Include("SharedManifestsStatus").Include("ShipmentLevel").Include("Agent.Card").Include("ShipmentType") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }


        public IQueryable<AgentSharedManifest> GetAgentSharedManifestsForDashBoard(int tenant)
        {
            return (from record in context.AgentSharedManifests where record.Tenant == tenant && record.StatusCode =="WAIT" select record);
        }



        public IQueryable<AgentSharedManifest> GetUsersWorkspaceLastLogins(int tenant)
        {
            return (from d in context.AgentSharedManifests
                    where d.Tenant == tenant
                    select d);
        }



        public AgentSharedManifest GetSingleAgentSharedManifestByAgentRef(string agentRef, int tenant)
        {
            return (from record in context.AgentSharedManifests where record.AgentReference == agentRef && record.Tenant == tenant select record).FirstOrDefault();
        }






        public void Add(AgentSharedManifest entity)
        {
            context.AgentSharedManifests.Add(entity);
        }

        public void Remove(AgentSharedManifest entity)
        {
            try
            {
                context.AgentSharedManifests.Attach(entity);
            }
            catch { };
            context.AgentSharedManifests.Remove(entity);
        }

        public void Update(AgentSharedManifest entity)
        {
            try
            {
                context.AgentSharedManifests.Attach(entity);
            }
            catch { };
            context.SetAsModified(entity);
        }

        public List<AgentSharedManifest> All()
        {
            return context.AgentSharedManifests.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<AgentSharedManifest> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public AgentSharedManifest GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

    }
}