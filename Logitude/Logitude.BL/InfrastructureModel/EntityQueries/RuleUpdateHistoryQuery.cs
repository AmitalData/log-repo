using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class RuleUpdateHistoryQuery
    {
        RuleUpdateHistoryRepository repository;

        public RuleUpdateHistoryQuery()
        {
            repository = new RuleUpdateHistoryRepository();
        }

        public RuleUpdateHistoryQuery(int tenant)
        {
            repository = new RuleUpdateHistoryRepository(tenant);
        }

        public RuleUpdateHistoryQuery(RuleUpdateHistoryRepository RuleUpdateHistoryRepository)
        {
            repository = RuleUpdateHistoryRepository;
        }

        public RuleUpdateHistoryPM GetSinglePM(string id, int tenant)
        {
            RuleUpdateHistoryPM entity;
            entity = (from a in repository.context.RuleUpdateHistories
                      where a.Tenant == tenant && a.Id == id
                      select new RuleUpdateHistoryPM()
                      {
                          Id = a.Id,
                          Tenant = a.Tenant,
                          CreateDate = a.CreateDate,
                          CreatedByUserId = a.CreatedByUserId,
                          EventName = a.EventName,
                          RuleCode = a.RuleCode,
                          UpdateDate = a.UpdateDate,
                          UpdatedByUserId = a.UpdatedByUserId,
                      }).FirstOrDefault();

            return entity;
        }

        public IQueryable<RuleUpdateHistoryList> GetIQueryableEntityList(IQueryable<RuleUpdateHistory> iQueryable)
        {
            IQueryable<RuleUpdateHistoryList> result = from a in iQueryable
                                                       select new RuleUpdateHistoryList()
                                                       {
                                                           Id = a.Id,
                                                           Tenant = a.Tenant,
                                                           CreateDate = a.CreateDate,
                                                           CreatedBy = a.CreatedByUser.Contact.EnglishName,
                                                           EventName = a.EventName,
                                                           RuleCode = a.RuleCode,
                                                           UpdateDate = a.UpdateDate,
                                                           UpdatedBy = a.UpdatedByUser.Contact.EnglishName,
                                                       };
            return result;
        }


        public IQueryable<RuleUpdateHistoryPM> GetRuleUpdateHistoryPMsByTenant(int tenant)
        {
            IQueryable<RuleUpdateHistoryPM> Temp = from a in repository.context.RuleUpdateHistories
                                                   where a.Tenant == tenant
                                                   select new RuleUpdateHistoryPM()
                                                   {
                                                       Id = a.Id,
                                                       Tenant = a.Tenant,
                                                       CreateDate = a.CreateDate,
                                                       CreatedByUserId = a.CreatedByUserId,
                                                       EventName = a.EventName,
                                                       RuleCode = a.RuleCode,
                                                       UpdateDate = a.UpdateDate,
                                                       UpdatedByUserId = a.UpdatedByUserId,
                                                   };
            return Temp;
        }

        public IQueryable<RuleUpdateHistoryPM> GetRuleUpdateHistoryPMsByRuleCodeTenant(string ruleCode, int tenant)
        {
            IQueryable<RuleUpdateHistoryPM> Temp = from a in repository.context.RuleUpdateHistories
                                                   where a.Tenant == tenant && a.RuleCode == ruleCode
                                                   select new RuleUpdateHistoryPM()
                                                   {
                                                       Id = a.Id,
                                                       Tenant = a.Tenant,
                                                       CreateDate = a.CreateDate,
                                                       CreatedByUserId = a.CreatedByUserId,
                                                       EventName = a.EventName,
                                                       RuleCode = a.RuleCode,
                                                       UpdateDate = a.UpdateDate,
                                                       UpdatedByUserId = a.UpdatedByUserId,
                                                   };
            return Temp;
        }


    }
}
