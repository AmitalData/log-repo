using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Domain.Interfaces;

namespace AmitalCloud.Infrastructure.Data.Queries
{
    public class ObjectTableRuleQuery
    {
        private readonly Repository<ObjectTableRule> repository;
        private readonly IAmitalCloudContext context;

        public ObjectTableRuleQuery(int tenant)
        {
            context = AmitalCloudContext.GetContext(tenant);
            repository = new Repository<ObjectTableRule>(context);
        }

        public List<ObjectTableRulePM> GetObjectTableRulePMsByTenant(int tenant)
        {
            string pmslistName = "objecttablerulepmstenant" + tenant;
            List<ObjectTableRulePM> allRules = new List<ObjectTableRulePM>();

            if (CacheManager.CacheWrapper.Get(pmslistName) == null)
            {
                List<ObjectTableRulePM> loggedTenantobjectTableRulePMs = new List<ObjectTableRulePM>();
                List<ObjectTableRulePM> zeroTenantobjectTableRulePMs = new List<ObjectTableRulePM>();
                Repository<RuleConditionField> ruleConditionFieldRepository = new Repository<RuleConditionField>(context);

                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    loggedTenantobjectTableRulePMs = repository.GetMulti(a => a.Tenant == tenant, a => new ObjectTableRulePM(a)
                    {
                        RuleTypeName = a.RuleType.Name,
                    }, "RuleType");

                    foreach (ObjectTableRulePM rule in loggedTenantobjectTableRulePMs)
                    {
                        rule.RuleConditionFields = ruleConditionFieldRepository.GetMulti(a => (a.Tenant == rule.Tenant) && a.ObjectTableRuleId == rule.Id, a => new RuleConditionFieldPM(a) {
                            ObjectFieldName = a.ObjectField.FieldName,
                        }, "ObjectField");
                    }

                    scope.Complete();
                }

                if (tenant != 0)
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        zeroTenantobjectTableRulePMs = repository.GetMulti(a => a.Tenant == 0, a => new ObjectTableRulePM(a)
                        {
                            RuleTypeName = a.RuleType.Name,
                        }, "RuleType");

                        foreach (ObjectTableRulePM rule in zeroTenantobjectTableRulePMs)
                        {
                            rule.RuleConditionFields = ruleConditionFieldRepository.GetMulti(a => (a.Tenant == rule.Tenant) && a.ObjectTableRuleId == rule.Id, a => new RuleConditionFieldPM(a)
                            {
                                ObjectFieldName = a.ObjectField.FieldName,
                            }, "ObjectField");
                        }
                        scope.Complete();
                    }

                    foreach (ObjectTableRulePM rule in loggedTenantobjectTableRulePMs)
                    {
                        rule.IsCreatedFromSystemRule = zeroTenantobjectTableRulePMs.Any(r => r.RuleCode == rule.RuleCode && r.Tenant == 0);
                        allRules.Add(rule);
                    }

                    foreach (ObjectTableRulePM rule in zeroTenantobjectTableRulePMs)
                    {
                        if (!allRules.Any(r => r.RuleCode == rule.RuleCode))
                        {
                            allRules.Add(rule);
                        }
                    }
                }
                else
                {
                    allRules = loggedTenantobjectTableRulePMs;
                }
               
                CacheManager.CacheWrapper.Insert(pmslistName, allRules, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
            }
            else
            {
                allRules = (List<ObjectTableRulePM>)CacheManager.CacheWrapper.Get(pmslistName);
            }

            return allRules;
        }
    }
}