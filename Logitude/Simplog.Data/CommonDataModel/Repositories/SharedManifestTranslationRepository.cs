using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
namespace Simplog.Data.CommonDataModel.Repositories
{
    public class SharedManifestTranslationRepository : IRepository<SharedManifestTranslation>
    {
        ICommonDataContext commonDataContext;

        public SharedManifestTranslationRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public SharedManifestTranslationRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public SharedManifestTranslationRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<SharedManifestTranslation> GetSharedManifestTranslations(int tenant)
        {
            return (from record in context.SharedManifestTranslations where record.Tenant == tenant || record.Tenant == 0 select record);
        }

        public SharedManifestTranslation GetSingleSharedManifestTranslation(string id, int tenant)
        {
            return (from record in context.SharedManifestTranslations where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public SharedManifestTranslation GetSingleSharedManifestTranslation(string id)
        {
            SharedManifestTranslation entity = (from record in context.SharedManifestTranslations.Include("UpdatedByUser").Include("CreatedByUser") where record.Id == id select record).FirstOrDefault();
            return entity;
        }

        public void Add(SharedManifestTranslation entity)
        {
            this.context.SharedManifestTranslations.Add(entity);
        }

        public void Remove(SharedManifestTranslation entity)
        {
            try
            {
                this.context.SharedManifestTranslations.Attach(entity);
            }
            catch { }
            this.context.SharedManifestTranslations.Remove(entity);
        }

        public void Update(SharedManifestTranslation entity)
        {
            try
            {
                this.context.SharedManifestTranslations.Attach(entity);
            }
            catch { }
            this.context.SetAsModified(entity);
        }

        public List<SharedManifestTranslation> All()
        {
            return this.context.SharedManifestTranslations.ToList<SharedManifestTranslation>();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }

        public IQueryable<SharedManifestTranslation> GetSharedManifestTranslationByAgentId(string agentId, int tenant)
        {

            var query = (from a in context.SharedManifestTranslations
                         where a.Tenant == tenant && a.AgentId == agentId
                         select a);

            return query;
        }

        public List<SharedManifestTranslation> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public SharedManifestTranslation GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

    }
}
