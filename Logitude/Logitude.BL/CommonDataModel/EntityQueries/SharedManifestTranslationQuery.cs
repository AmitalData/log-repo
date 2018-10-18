using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class SharedManifestTranslationQuery
    {
        SharedManifestTranslationRepository repository;
        public SharedManifestTranslationQuery()
        {
            repository = new SharedManifestTranslationRepository();
        }

        public SharedManifestTranslationQuery(int tenant)
        {
            repository = new SharedManifestTranslationRepository(tenant);
        }

        public SharedManifestTranslationQuery(SharedManifestTranslationRepository SharedManifestTranslationRepository)
        {
            repository = SharedManifestTranslationRepository;
        }


        public SharedManifestTranslationPM GetSinglePM(string id, int tenant)
        {

            SharedManifestTranslationPM entity = (from a in repository.context.SharedManifestTranslations
                                                  where a.Tenant == tenant && a.Id == id
                                                  select new SharedManifestTranslationPM()
                                                  {
                                                      Id = a.Id,
                                                      AgentId = a.AgentId,
                                                      AgentCode = a.AgentCode,
                                                      CreateDate = a.CreateDate,
                                                      CreatedByUserId = a.CreatedByUserId,
                                                      MyCode = a.MyCode,
                                                      ObjectTableName = a.ObjectTableName,
                                                      Tenant = a.Tenant,
                                                      UpdateDate = a.UpdateDate,
                                                      UpdatedByUserId = a.UpdatedByUserId,

                                                  }).FirstOrDefault();
            return entity;
        }

        public IQueryable<SharedManifestTranslationPM> GetSharedManifestTranslationPMsByAgentId(int tenant,string agentId)
        {
            IQueryable<SharedManifestTranslationPM> SharedManifestTranslations = from a in repository.context.SharedManifestTranslations
                                                   where a.Tenant == tenant && a.AgentId == agentId
                                                   select new SharedManifestTranslationPM()
                                                   {
                                                       Id = a.Id,
                                                       AgentId = a.AgentId,
                                                       AgentCode = a.AgentCode,
                                                       CreateDate = a.CreateDate,
                                                       CreatedByUserId = a.CreatedByUserId,
                                                       MyCode = a.MyCode,
                                                       ObjectTableName = a.ObjectTableName,
                                                       Tenant = a.Tenant,
                                                       UpdateDate = a.UpdateDate,
                                                       UpdatedByUserId = a.UpdatedByUserId,
                                                   };
            return SharedManifestTranslations;
        }
 
    }
}
