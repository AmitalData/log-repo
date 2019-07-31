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

        public ObjectTableRulePM GetSingleObjectTableRulePMById(string id, int tenant)
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



        public List<ObjectTableRulePM> GetObjectTableRulePMsByTenant(int tenant)
        {
			string pmslistName = "objecttablerulepmstenant" + tenant;
			List<ObjectTableRulePM> selectedRules = new List<ObjectTableRulePM>();

			if (CacheManager.CacheWrapper.Get(pmslistName) == null)
			{
				List<ObjectTableRulePM> objectTableRulePMs1 = new List<ObjectTableRulePM>();
				List<ObjectTableRulePM> objectTableRulePMs2 = new List<ObjectTableRulePM>();
				using (TransactionScope scope = TransactionFactory.GetTransaction())
				{
					//List<ObjectTableRule> allRules = ObjectTableRuleRepository.GetObjectTableRulesByTenant(tenant);
					WebFreightContext webFreightContext = (WebFreightContext)WebFreightContext.GetContext(tenant);
					objectTableRulePMs1 = (from a in repository.context.ObjectTableRules.Include("RuleType")
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
											   TriggerTypeCode = a.TriggerTypeCode,
											   RuleNotificationTypeCode = a.RuleNotificationTypeCode,
											   Internal = a.Internal,
											   AdvancedCondition = a.AdvancedCondition,

										   }).ToList();

					RuleConditionFieldRepository ruleConditionFieldRepository = new RuleConditionFieldRepository(webFreightContext);
					RuleConditionFieldQuery ruleConditionFieldQuery = new RuleConditionFieldQuery(ruleConditionFieldRepository);

					foreach (ObjectTableRulePM rule in objectTableRulePMs1)
					{
						rule.RuleConditionFields = ruleConditionFieldQuery.GetRuleConditionFieldsByRuleId(rule.Tenant, rule.Id).ToList();
					}

					scope.Complete();
				}

				using (TransactionScope scope = TransactionFactory.GetTransaction())
				{
					WebFreightContext webFreightContext = (WebFreightContext)WebFreightContext.GetContext(0);
					objectTableRulePMs2 = (from a in repository.context.ObjectTableRules.Include("RuleType")
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
											   TriggerTypeCode = a.TriggerTypeCode,
											   RuleNotificationTypeCode = a.RuleNotificationTypeCode,
											   Internal = a.Internal,
											   AdvancedCondition = a.AdvancedCondition,
										   }).ToList();

					RuleConditionFieldRepository ruleConditionFieldRepository = new RuleConditionFieldRepository(webFreightContext);
					RuleConditionFieldQuery ruleConditionFieldQuery = new RuleConditionFieldQuery(ruleConditionFieldRepository);

					foreach (ObjectTableRulePM rule in objectTableRulePMs2)
					{
						rule.RuleConditionFields = ruleConditionFieldQuery.GetRuleConditionFieldsByRuleId(rule.Tenant, rule.Id).ToList();
					}

					scope.Complete();

				}

				List<ObjectTableRulePM> objectTableRulePMs = objectTableRulePMs1.Concat(objectTableRulePMs2).ToList();
				foreach (ObjectTableRulePM rule in objectTableRulePMs)
				{
					ObjectTableRulePM existedRule = (from a in selectedRules
													 where a.RuleCode == rule.RuleCode
													 select a).FirstOrDefault();

					if (existedRule != null)
					{
						if (existedRule.Tenant == 0 && rule.Tenant == tenant)
						{
							selectedRules.Remove(existedRule);
							selectedRules.Add(rule);
						}
					}
					else
					{

						selectedRules.Add(rule);
					}
				}

				CacheManager.CacheWrapper.Insert(pmslistName, selectedRules, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);

			}
			else
			{
				selectedRules = (List<ObjectTableRulePM>)CacheManager.CacheWrapper.Get(pmslistName);
			}

            return selectedRules;
        }

        public List<ObjectTableRulePM> GetObjectTableRulePMsByObjectTableId(string objectTableId, int tenant)
        {


            List<ObjectTableRulePM> query = (from a in repository.context.ObjectTableRules.Include("RuleType")
                                             where a.Tenant == tenant && a.ObjectTableId == objectTableId
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
                                                 TriggerTypeCode = a.TriggerTypeCode,
                                                 RuleNotificationTypeCode = a.RuleNotificationTypeCode,
                                                 Internal = a.Internal,
                                                 AdvancedCondition = a.AdvancedCondition,
                                             }).ToList();


            foreach (ObjectTableRulePM rule in query)
            {

                RuleConditionFieldRepository ruleConditionFieldRepository = new RuleConditionFieldRepository();
                RuleConditionFieldQuery ruleConditionFieldQuery = new RuleConditionFieldQuery(ruleConditionFieldRepository);
                rule.RuleConditionFields = ruleConditionFieldQuery.GetRuleConditionFieldsByRuleId(tenant, rule.Id).ToList();
            }

            return query;
        }

        public List<ObjectTableRulePM> GetObjectTableRulePMsByRyleTypeCode(string objectTableId, int tenant, string ruleTypeCode)
        {


            List<ObjectTableRulePM> query = (from a in repository.context.ObjectTableRules.Include("RuleType")
                                             where (a.Tenant == tenant || a.Tenant == 0) && a.ObjectTableId == objectTableId && a.RuleTypeCode == ruleTypeCode && a.InActive == false
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
                                                 TriggerTypeCode = a.TriggerTypeCode,
                                                 RuleNotificationTypeCode = a.RuleNotificationTypeCode,
                                                 Internal = a.Internal,
                                                 AdvancedCondition = a.AdvancedCondition,
                                             }).ToList();


            foreach (ObjectTableRulePM rule in query)
            {

                RuleConditionFieldRepository ruleConditionFieldRepository = new RuleConditionFieldRepository();
                RuleConditionFieldQuery ruleConditionFieldQuery = new RuleConditionFieldQuery(ruleConditionFieldRepository);
                rule.RuleConditionFields = ruleConditionFieldQuery.GetRuleConditionFieldsByRuleId(rule.Tenant, rule.Id).ToList();
            }




            return query;
        }



    }
}