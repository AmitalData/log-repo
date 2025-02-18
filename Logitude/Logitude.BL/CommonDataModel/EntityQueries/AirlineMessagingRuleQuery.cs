using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class AirlineMessagingRuleQuery
    {
        AirlineMessagingRuleRepository repository;

        public AirlineMessagingRuleQuery()
        {
            repository = new AirlineMessagingRuleRepository(); 
        }

        public AirlineMessagingRuleQuery(int tenant)
        {
            repository = new AirlineMessagingRuleRepository(tenant);
        }

        public AirlineMessagingRuleQuery(AirlineMessagingRuleRepository repository)
        {
            this.repository = repository;
        }

        public AirlineMessagingRulePM GetSinglePM(string id, int tenant)
        {
            AirlineMessagingRulePM entity = (from a in repository.context.AirlineMessagingRules
                                             where a.Tenant == tenant && a.Id == id
                                             select new AirlineMessagingRulePM()
                                             {
                                                 Id = a.Id,
                                                 Tenant = a.Tenant,
                                                 AirlineId = a.AirlineId,
                                                 MessageTypeCode = a.MessageTypeCode,
                                                 RuleFieldId = a.RuleFieldId,
                                                 RuleFieldCode = a.RuleFieldCode,
                                                 IsMandatoryForSending = a.IsMandatoryForSending,
                                                 MaxSize = a.MaxSize,
                                                 InActive = a.InActive,
                                                 CreatedByUserId = a.CreatedByUserId,
                                                 UpdatedByUserId = a.UpdatedByUserId,
                                                 CreateDate = a.CreateDate,
                                                 UpdateDate = a.UpdateDate,
                                             }).FirstOrDefault();

            return entity;
        }

        public IQueryable<AirlineMessagingRulePM> GetAdditionalServicePMsByTenant(int tenant)
        {
            IQueryable<AirlineMessagingRulePM> AirlineMessagingRules = from a in repository.context.AirlineMessagingRules
                                            where a.Tenant == tenant
                                            select new AirlineMessagingRulePM()
                                            {
                                                Id = a.Id,
                                                Tenant = a.Tenant,
                                                AirlineId = a.AirlineId,
                                                MessageTypeCode = a.MessageTypeCode,
                                                RuleFieldId = a.RuleFieldId,
                                                RuleFieldCode = a.RuleFieldCode,
                                                IsMandatoryForSending = a.IsMandatoryForSending,
                                                MaxSize = a.MaxSize,
                                                InActive = a.InActive,
                                                CreatedByUserId = a.CreatedByUserId,
                                                UpdatedByUserId = a.UpdatedByUserId,
                                                CreateDate = a.CreateDate,
                                                UpdateDate = a.UpdateDate,
                                            };
            return AirlineMessagingRules;
        }

        public IQueryable<AirlineMessagingRuleList> GetIQueryableEntityList(IQueryable<AirlineMessagingRule> iQueryable)
        {
            IQueryable<AirlineMessagingRuleList> result = from a in iQueryable.Include("RuleField").Include("Airline")
                                                          select new AirlineMessagingRuleList()
                                            {
                                                Id = a.Id,
                                                Tenant = a.Tenant,
                                                AirlineId = a.AirlineId,
                                                MessageTypeCode = a.MessageTypeCode,
                                                RuleFieldId = a.RuleFieldId,
                                                RuleFieldCode = a.RuleFieldCode,
                                                IsMandatoryForSending = a.IsMandatoryForSending,
                                                MaxSize = a.MaxSize,
                                                RuleFieldName = a.RuleField == null ? null : a.RuleField.FieldName,
                                                AirlineCode = a.Airline == null ? null : a.Airline.Code,
                                                InActive = a.InActive,
                                            };
            return result;
        }
    }
}
