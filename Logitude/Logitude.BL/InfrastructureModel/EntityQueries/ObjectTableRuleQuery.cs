using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;

using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class ObjectTableRuleQuery
    {
        ObjectTableRuleRepository repository;
        public ObjectTableRuleQuery()
        {
            repository = new ObjectTableRuleRepository(); 
        }

        public ObjectTableRuleQuery(int tenant)
        {
            repository = new ObjectTableRuleRepository(tenant);
        }

        public ObjectTableRuleQuery(ObjectTableRuleRepository objectTableRuleRepository)
        {
            repository = objectTableRuleRepository;
        }


        public ObjectTableRulePM GetSinglePM(string id, int tenant)
        {
            ObjectTableRulePM objectTableRulePm = (from a in repository.context.ObjectTableRules.Include("RuleType")
                                                   where a.Tenant == tenant && a.Id == id
                                                   select new ObjectTableRulePM()
                                                   {
                                                       Id = a.Id,
                                                       Tenant = a.Tenant,
                                                       Condition = a.Condition,
                                                       InActive = a.InActive,
                                                       Name = a.Name,
                                                       ObjectTableId = a.ObjectTableId,
                                                       OutputMessage = a.OutputMessage,
                                                       RuleCode = a.RuleCode,
                                                       RuleTypeCode = a.RuleTypeCode,
                                                       SystemLevel = a.SystemLevel,
                                                       RuleTypeName = a.RuleType.Name,
                                                       ActiveForNew = a.ActiveForNew,
                                                       ActiveForUpdate = a.ActiveForUpdate,
                                                       TriggerFieldId = a.TriggerFieldId,
                                                       TriggerFieldCode = a.TriggerFieldCode,
                                                       TriggerTypeCode = a.TriggerTypeCode,
                                                       RuleNotificationTypeCode = a.RuleNotificationTypeCode,
                                                       Internal = a.Internal,
                                                       AdvancedCondition = a.AdvancedCondition,
                                                   }).FirstOrDefault();

            RuleConditionFieldRepository ruleConditionFieldRepository = new RuleConditionFieldRepository(tenant);
            RuleConditionFieldQuery ruleConditionFieldQuery = new RuleConditionFieldQuery(ruleConditionFieldRepository);
            objectTableRulePm.RuleConditionFields = ruleConditionFieldQuery.GetRuleConditionFieldsByRuleId(tenant, objectTableRulePm.Id).ToList();

            return objectTableRulePm;
        }

        public ObjectTableRulePM GetSinglePMByCode(string code, int tenant)
        {
            ObjectTableRulePM objectTableRulePm = (from a in repository.context.ObjectTableRules.Include("RuleType")
                                                   where a.Tenant == tenant && a.RuleCode == code
                                                   select new ObjectTableRulePM()
                                                   {
                                                       Id = a.Id,
                                                       Tenant = a.Tenant,
                                                       Condition = a.Condition,
                                                       InActive = a.InActive,
                                                       Name = a.Name,
                                                       ObjectTableId = a.ObjectTableId,
                                                       OutputMessage = a.OutputMessage,
                                                       RuleCode = a.RuleCode,
                                                       RuleTypeCode = a.RuleTypeCode,
                                                       SystemLevel = a.SystemLevel,
                                                       RuleTypeName = a.RuleType.Name,
                                                       ActiveForNew = a.ActiveForNew,
                                                       ActiveForUpdate = a.ActiveForUpdate,
                                                       TriggerFieldId = a.TriggerFieldId,
                                                       TriggerFieldCode = a.TriggerFieldCode,
                                                       TriggerTypeCode = a.TriggerTypeCode,
                                                       RuleNotificationTypeCode = a.RuleNotificationTypeCode,
                                                       Internal = a.Internal,
                                                       AdvancedCondition = a.AdvancedCondition,
                                                   }).FirstOrDefault();
            if (objectTableRulePm != null)
            {
                RuleConditionFieldRepository ruleConditionFieldRepository = new RuleConditionFieldRepository(tenant);
                RuleConditionFieldQuery ruleConditionFieldQuery = new RuleConditionFieldQuery(ruleConditionFieldRepository);
                objectTableRulePm.RuleConditionFields = ruleConditionFieldQuery.GetRuleConditionFieldsByRuleId(tenant, objectTableRulePm.Id).ToList();
            }

            return objectTableRulePm;
        }

        public List<ObjectTableRulePM> GetObjectTableRulePMsByTenant(int tenant)
        {
			string pmslistName = "objecttablerulepmstenant" + tenant;
			List<ObjectTableRulePM> allRules = new List<ObjectTableRulePM>();

			if (CacheManager.CacheWrapper.Get(pmslistName) == null)
			{
				List<ObjectTableRulePM> loggedTenantobjectTableRulePMs = new List<ObjectTableRulePM>();
				List<ObjectTableRulePM> zeroTenantobjectTableRulePMs = new List<ObjectTableRulePM>();
				using (TransactionScope scope = TransactionFactory.GetTransaction())
				{
					//List<ObjectTableRule> allRules = ObjectTableRuleRepository.GetObjectTableRulesByTenant(tenant);
					WebFreightContext webFreightContext = (WebFreightContext)WebFreightContext.GetContext(tenant);
                    loggedTenantobjectTableRulePMs = (from a in repository.context.ObjectTableRules.Include("RuleType")
										   where a.Tenant == tenant
										   select new ObjectTableRulePM()
										   {
											   Id = a.Id,
											   Tenant = a.Tenant,
											   Condition = a.Condition,
											   InActive = a.InActive,
											   Name = a.Name,
											   ObjectTableId = a.ObjectTableId,
											   OutputMessage = a.OutputMessage,
											   RuleCode = a.RuleCode,
											   RuleTypeCode = a.RuleTypeCode,
											   SystemLevel = a.SystemLevel,
											   RuleTypeName = a.RuleType.Name,
											   ActiveForNew = a.ActiveForNew,
											   ActiveForUpdate = a.ActiveForUpdate,
											   TriggerFieldId = a.TriggerFieldId,
                                               TriggerFieldCode = a.TriggerFieldCode,
                                               TriggerTypeCode = a.TriggerTypeCode,
											   RuleNotificationTypeCode = a.RuleNotificationTypeCode,
											   Internal = a.Internal,
											   AdvancedCondition = a.AdvancedCondition,

										   }).ToList();

					RuleConditionFieldRepository ruleConditionFieldRepository = new RuleConditionFieldRepository(webFreightContext);
					RuleConditionFieldQuery ruleConditionFieldQuery = new RuleConditionFieldQuery(ruleConditionFieldRepository);

					foreach (ObjectTableRulePM rule in loggedTenantobjectTableRulePMs)
					{
						rule.RuleConditionFields = ruleConditionFieldQuery.GetRuleConditionFieldsByRuleId(rule.Tenant, rule.Id).ToList();
					}

					scope.Complete();
				}

                if(tenant != 0)
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        WebFreightContext webFreightContext = (WebFreightContext)WebFreightContext.GetContext(tenant);
                        zeroTenantobjectTableRulePMs = (from a in repository.context.ObjectTableRules.Include("RuleType")
                                                        where a.Tenant == 0
                                                        select new ObjectTableRulePM()
                                                        {
                                                            Id = a.Id,
                                                            Tenant = a.Tenant,
                                                            Condition = a.Condition,
                                                            InActive = a.InActive,
                                                            Name = a.Name,
                                                            ObjectTableId = a.ObjectTableId,
                                                            OutputMessage = a.OutputMessage,
                                                            RuleCode = a.RuleCode,
                                                            RuleTypeCode = a.RuleTypeCode,
                                                            SystemLevel = a.SystemLevel,
                                                            RuleTypeName = a.RuleType.Name,
                                                            ActiveForNew = a.ActiveForNew,
                                                            ActiveForUpdate = a.ActiveForUpdate,
                                                            TriggerFieldId = a.TriggerFieldId,
                                                            TriggerFieldCode = a.TriggerFieldCode,
                                                            TriggerTypeCode = a.TriggerTypeCode,
                                                            RuleNotificationTypeCode = a.RuleNotificationTypeCode,
                                                            Internal = a.Internal,
                                                            AdvancedCondition = a.AdvancedCondition,
                                                        }).ToList();

                        RuleConditionFieldRepository ruleConditionFieldRepository = new RuleConditionFieldRepository(webFreightContext);
                        RuleConditionFieldQuery ruleConditionFieldQuery = new RuleConditionFieldQuery(ruleConditionFieldRepository);

                        foreach (ObjectTableRulePM rule in zeroTenantobjectTableRulePMs)
                        {
                            rule.RuleConditionFields = ruleConditionFieldQuery.GetRuleConditionFieldsByRuleId(rule.Tenant, rule.Id).ToList();
                        }
                        scope.Complete();
                    }
                }
                if (tenant != 0)
                {
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
                //List<ObjectTableRulePM> objectTableRulePMs = loggedTenantobjectTableRulePMs.Concat(zeroTenantobjectTableRulePMs).ToList();
                //foreach (ObjectTableRulePM rule in objectTableRulePMs)
                //{
                //	ObjectTableRulePM existedRule = (from a in allRules
                //                                                 where a.RuleCode == rule.RuleCode
                //									 select a).FirstOrDefault();

                //	if (existedRule != null)
                //	{
                //		if (existedRule.Tenant == 0 && rule.Tenant == tenant)
                //		{
                //                        allRules.Remove(existedRule);
                //                        allRules.Add(rule);
                //		}
                //	}
                //	else
                //	{

                //                    allRules.Add(rule);
                //	}
                //}

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