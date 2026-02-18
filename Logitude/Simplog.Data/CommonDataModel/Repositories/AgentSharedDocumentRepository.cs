using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class AgentSharedDocumentRepository : IRepository<AgentSharedDocument>
    {
        ICommonDataContext commonDataContext;

        public AgentSharedDocumentRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public AgentSharedDocumentRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public AgentSharedDocumentRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<AgentSharedDocument> GetAgentSharedDocuments(int tenant)
        {
            return (from record in context.AgentSharedDocuments.Include("SharedManifestsStatus").Include("ShipmentLevel").Include("Agent.Card") where record.Tenant == tenant select record);
        }

        public AgentSharedDocument GetSingleAgentSharedDocument(string id, int tenant)
        {
            return (from record in context.AgentSharedDocuments.Include("SharedManifestsStatus").Include("ShipmentLevel").Include("Agent.Card") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public AgentSharedDocument GetSingleAgentSharedDocumentWithOutInCluded(string id, int tenant)
        {
            return (from record in context.AgentSharedDocuments where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }


        public AgentSharedDocument GetAgentSharedDocumentByAgentRef(string agentReference, int tenant)
        {
            return (from record in context.AgentSharedDocuments where record.AgentReference == agentReference && record.Tenant == tenant select record).OrderByDescending(d=>d.CreateDate).FirstOrDefault();
        }

        public void Add(AgentSharedDocument entity)
        {
            context.AgentSharedDocuments.Add(entity);
        }

        public void Remove(AgentSharedDocument entity)
        {
            try
            {
                context.AgentSharedDocuments.Attach(entity);
            }
            catch { };
            context.AgentSharedDocuments.Remove(entity);
        }

        public void Update(AgentSharedDocument entity)
        {
            try
            {
                context.AgentSharedDocuments.Attach(entity);
            }
            catch { };
            context.SetAsModified(entity);
        }

        public List<AgentSharedDocument> All()
        {
            return context.AgentSharedDocuments.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<AgentSharedDocument> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public AgentSharedDocument GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

    }
}