using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.Validating;
using Logitude.BL.InfrastructureModel.Tools.TraceEvents;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class ObjectTableRuleService
    {

        bool isNewEntity;
        private int tenant;
        public ObjectTableRule Poco { get; set; }


        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ObjectTableRulePM entityPM;
        private IWebFreightContext objectContext;
        private ObjectTableRuleRepository entityRepository;
        private RuleTypeRepository ruleTypeRepository;
        private RuleConditionFieldRepository  ruleConditionFieldRepository;
        private RuleConditionFieldService service;
        public ObjectTableRuleService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ObjectTableRuleRepository(objectContext);
        }

        public void Create(ObjectTableRulePM theEntityPm)
        {

			string ruleslistName = "objecttablerulestenant" + tenant;
			if (CacheManager.CacheWrapper.Get(ruleslistName) != null)
			{
				CacheManager.CacheWrapper.Invalidate(ruleslistName);
			}

			string pmslistName = "objecttablerulepmstenant" + tenant;
			if (CacheManager.CacheWrapper.Get(pmslistName) != null)
			{
				CacheManager.CacheWrapper.Invalidate(pmslistName);
			}

			this.isNewEntity = true;
            this.entityPM = theEntityPm;
         //   this.entityPM.Id = IdCounter.GetNumber("ObjectTableRule", tenant).ToString();
            this.Poco = new ObjectTableRule();
         
            ruleTypeRepository = new RuleTypeRepository(ObjectContext);
            ruleConditionFieldRepository = new RuleConditionFieldRepository(ObjectContext);

            this.Poco.Id = IdCounter.GetNumber("ObjectTableRule", theEntityPm.Tenant).ToString();
            theEntityPm.Id = this.Poco.Id;
            theEntityPm.RuleTypeName = ruleTypeRepository.GetRuleTypeByCode(theEntityPm.RuleTypeCode).Name;
            ObjectTableRuleMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            if (this.Poco.RuleCode == null)
            {
                this.Poco.RuleCode = this.Poco.Id;
            }

			

			ObjectTableRuleQuery objectTableRuleQuery = new ObjectTableRuleQuery(entityRepository);

            if (objectTableRuleQuery.GetObjectTableRulePMsByTenant(theEntityPm.Tenant).Where(r => r.RuleCode == this.Poco.RuleCode && r.Tenant == theEntityPm.Tenant).FirstOrDefault() == null)
            {
                entityRepository.Add(Poco);
                entityRepository.SubmitChanges();

                if (theEntityPm.RuleConditionFields != null)
                {
                    foreach (RuleConditionFieldPM condFieldPM in theEntityPm.RuleConditionFields)
                    {
                        RuleConditionField condField = new RuleConditionField();
                        condField.Id = IdCounter.GetNumber("RuleConditionField", theEntityPm.Tenant).ToString();
                        condFieldPM.Id = condField.Id;
                        condFieldPM.ObjectTableRuleId = this.Poco.Id;
                        RuleConditionFieldMapping.MapEntity(condFieldPM, condField, isNewEntity);
                        ruleConditionFieldRepository.Add(condField);
                    }
                }
            }
            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", theEntityPm.Tenant);
                msg = msg.Replace("%Entity", "Rule");
                throw new Exception(msg);
            }

            CreateRuleUpdateHistory(theEntityPm);
            ObjectContext.SaveChanges();

			
		}

        public void Update(ObjectTableRulePM theEntityPm, List<RuleConditionFieldPM> ruleCondetionFiledList)
        {

            string ruleslistName = "objecttablerulestenant" + tenant;
            if (CacheManager.CacheWrapper.Get(ruleslistName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(ruleslistName);
            }
            string pmslistName = "objecttablerulepmstenant" + tenant;
            if (CacheManager.CacheWrapper.Get(pmslistName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(pmslistName);
            }

            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleObjectTableRule(theEntityPm.Id, theEntityPm.Tenant);

            ruleTypeRepository = new RuleTypeRepository(ObjectContext);
            ruleConditionFieldRepository = new RuleConditionFieldRepository(ObjectContext);
            service = new RuleConditionFieldService(objectContext, theEntityPm.Tenant);

            string objectRulesListName = this.Poco.ObjectTable.Name + "DuplicationRules" + theEntityPm.Tenant;
            if (CacheManager.CacheWrapper.Get(objectRulesListName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(objectRulesListName);
            }
            ObjectTableRuleValidating.Validate(theEntityPm);
            ObjectTableRuleTracing.Trace(theEntityPm, Poco, isNewEntity);

            CreateRuleUpdateHistory(theEntityPm);

            ObjectTableRuleMapping.MapEntity(theEntityPm, Poco, isNewEntity);

            bool replicated = entityRepository.GetObjectTableRules(theEntityPm.Tenant).Where(r => r.RuleCode == theEntityPm.RuleCode && r.Id != theEntityPm.Id).Any();
            if (!replicated)
            {
                entityRepository.Update(Poco);
                entityRepository.SubmitChanges();
            }
            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", theEntityPm.Tenant);
                msg = msg.Replace("%Entity", "Rule");
                throw new Exception(msg);
            }

            foreach (RuleConditionFieldPM condFieldPM in ruleCondetionFiledList)
            {

                string listName = "ruleconditionfieldstenant" + condFieldPM.Tenant;

                if (CacheManager.CacheWrapper.Get(listName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(listName);
                }


                switch (condFieldPM.ChangeSetOp)
                {
                    case ChangeSetOperation.Insert:
                        {
                            service.Create(condFieldPM);

                            //RuleConditionField condField = new RuleConditionField();
                            //condField.Id = IdCounter.GetNumber("RuleConditionField", condFieldPM.Tenant).ToString();
                            //condFieldPM.Id = condField.Id;
                            //RuleConditionFieldMapping.MapEntity(condFieldPM, condField, isNewEntity);
                            //ruleConditionFieldRepository.Add(condField);
                            //ruleConditionFieldRepository.SubmitChanges();
                            break;
                        }

                    case ChangeSetOperation.Delete:
                        {

                            RuleConditionField condField = ruleConditionFieldRepository.GetSingleRuleConditionField(condFieldPM.Id, condFieldPM.Tenant);
                            ruleConditionFieldRepository.Remove(condField);
                            ruleConditionFieldRepository.SubmitChanges();

                            break;
                        }
                    case ChangeSetOperation.Update:
                        {

                            service.Update(condFieldPM);
                            //RuleConditionField condField = ruleConditionFieldRepository.GetSingleRuleConditionField(condFieldPM.Id, condFieldPM.Tenant);
                            //RuleConditionFieldMapping.MapEntity(condFieldPM, condField, isNewEntity);
                            //ruleConditionFieldRepository.Update(condField);
                            //ruleConditionFieldRepository.SubmitChanges();

                            break;
                        }
                    case ChangeSetOperation.None:
                        {
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }

            

            ObjectContext.SaveChanges();


        }

        private void CreateRuleUpdateHistory(ObjectTableRulePM theEntityPm)
        {
            RuleUpdateHistoryService ruleUpdateHistoryService = new RuleUpdateHistoryService(ObjectContext, tenant);
            ContactPM loggedContact = LoggedContactResolver.GetLoggedContact(tenant);
            RuleUpdateHistoryPM ruleUpdateHistory = new RuleUpdateHistoryPM();
            ruleUpdateHistory.Id = IdCounter.GetNumber("RuleUpdateHistory", theEntityPm.Tenant);
            ruleUpdateHistory.Tenant = theEntityPm.Tenant;
            ruleUpdateHistory.CreateDate = TenantServerConfigration.GetCurrentDateTime(theEntityPm.Tenant);
            ruleUpdateHistory.CreatedByUserId = loggedContact.Id;
            ruleUpdateHistory.UpdateDate = TenantServerConfigration.GetCurrentDateTime(theEntityPm.Tenant);
            ruleUpdateHistory.UpdatedByUserId = loggedContact.Id;
            ruleUpdateHistory.RuleCode = theEntityPm.RuleCode;
            if (isNewEntity)
            {
                if (theEntityPm.IsCreatedFromSystemRule)
                {
                   ObjectTableRule systemRule = entityRepository.GetSingleObjectTableRuleByCode(theEntityPm.RuleCode, 0);
                    if(systemRule != null)
                    {
                        if (systemRule.InActive != this.entityPM.InActive)
                        {
                            ruleUpdateHistory.EventName = (this.entityPM.InActive == true ? "Rule set as Inactive" : "Rule set as Active");
                        }
                        else
                            ruleUpdateHistory.EventName = "Rule Updated";
                    }
                    else
                        ruleUpdateHistory.EventName = "Rule Added";
                }
                else

                    ruleUpdateHistory.EventName = "Rule Added";
            }
            else
            {
                if (!IsDeletedEntity)
                {
                    if (this.Poco.InActive != this.entityPM.InActive)
                    {
                        ruleUpdateHistory.EventName = (this.entityPM.InActive == true ? "Rule set as Inactive" : "Rule set as Active");
                    }
                    else
                        ruleUpdateHistory.EventName = "Rule Updated";
                }
                else
                    ruleUpdateHistory.EventName = "Rule Restored";
            }
            ruleUpdateHistoryService.Create(ruleUpdateHistory);
        }

        bool IsDeletedEntity = false;
        public void Delete(string ruleId)
        {
            IsDeletedEntity = true;
            string ruleslistName = "objecttablerulestenant" + tenant;
            if (CacheManager.CacheWrapper.Get(ruleslistName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(ruleslistName);
            }
            string pmslistName = "objecttablerulepmstenant" + tenant;
            if (CacheManager.CacheWrapper.Get(pmslistName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(pmslistName);
            }

            string objectRulesListName = ruleId + "DuplicationRules" + tenant;
            if (CacheManager.CacheWrapper.Get(objectRulesListName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(objectRulesListName);
            }

          
            this.isNewEntity = false;
               
            this.Poco = entityRepository.GetSingleObjectTableRule(ruleId, tenant);
            if(this.Poco.Tenant == 0)
            {
                throw new Exception("You can not delete a system rule!");
            }

            ObjectTableRuleQuery rulesQuery = new ObjectTableRuleQuery(entityRepository);
            this.entityPM = rulesQuery.GetSinglePM(ruleId, tenant);
            this.CreateRuleUpdateHistory(this.entityPM);

            ruleTypeRepository = new RuleTypeRepository(ObjectContext);
            ruleConditionFieldRepository = new RuleConditionFieldRepository(ObjectContext);

            ObjectTableRuleFieldRepository objectTableRuleFieldRepository = new ObjectTableRuleFieldRepository(ObjectContext);
            List<ObjectTableRuleField> objectTableRuleFieldList = objectTableRuleFieldRepository.GetRuleFieldsByRuleId(ruleId, tenant).ToList();
            foreach (var field in objectTableRuleFieldList)
            {
                objectTableRuleFieldRepository.Remove(field);
            }

             List<RuleConditionField> ruleConditionFieldsList = ruleConditionFieldRepository.GetRuleConditionFieldsByRuleId(ruleId, tenant).ToList();
            foreach (var field in ruleConditionFieldsList)
            {
                ruleConditionFieldRepository.Remove(field);
            }

            entityRepository.Remove(this.Poco);

         
            

            ObjectContext.SaveChanges();


        }
    }
}