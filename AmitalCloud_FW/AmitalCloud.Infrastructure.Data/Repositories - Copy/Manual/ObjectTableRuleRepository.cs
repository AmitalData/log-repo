using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;

using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data;
using AmitalCloud.Infrastructure.Data.Interfaces;
using System.Data.Entity;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Context;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class ObjectTableRuleRepository:IRepository<ObjectTableRule, string>
    {

        IAmitalCloudContext amitalCloudContext;
        public ObjectTableRuleRepository()
        {
            //Context = new AmitalCloudContext();

        }
        public ObjectTableRuleRepository(IAmitalCloudContext context)
        {
            amitalCloudContext = context;

        }
        public ObjectTableRuleRepository(int tenant)
        {
            amitalCloudContext = AmitalCloudContext.GetContext(tenant);
        }
        public IQueryable<ObjectTableRule> GetObjectTableRules(int tenant)
        {
            return (from record in context.ObjectTableRules.Include("ObjectTable") where record.Tenant == tenant select record);
        }

        public ObjectTableRule GetSingleObjectTableRule(string id, int tenant)
        {
            ObjectTableRule rule = (from record in context.ObjectTableRules.Include("ObjectTable") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();

            //RuleConditionFieldRepository ruleConditionFieldRepository = new RuleConditionFieldRepository(tenant);
            //rule.RuleConditionFields = ruleConditionFieldRepository.GetRuleConditionFieldsByRuleIdTenant(rule.Tenant, rule.Id).ToList();


            return rule;

        }


        public ObjectTableRule GetSingleObjectTableRuleByCode(string code   , int tenant)
        {
            ObjectTableRule rule = (from record in context.ObjectTableRules.Include("ObjectTable") where record.RuleCode == code && record.Tenant == tenant select record).FirstOrDefault();

            //RuleConditionFieldRepository ruleConditionFieldRepository = new RuleConditionFieldRepository(tenant);
            //rule.RuleConditionFields = ruleConditionFieldRepository.GetRuleConditionFieldsByRuleIdTenant(rule.Tenant, rule.Id).ToList();


            return rule;

        }
        public static List<ObjectTableRule> GetObjectTableRulesByTenant(int tenant)
        {
            List<ObjectTableRule> zeroObjectTableRules;
            List<ObjectTableRule> currentObjectTableRules = new List<ObjectTableRule>();
            List<ObjectTableRule> objectTableRules;
            string listName = "objecttablerulestenant" + tenant;
            List<ObjectTableRule> selectedRules = new List<ObjectTableRule>();
            if (CacheManager.CacheWrapper.Get(listName) == null)
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    IAmitalCloudContext context = AmitalCloudContext.GetContext(0);
                    zeroObjectTableRules = (from a in context.ObjectTableRules.Include("ObjectTable").Include("RuleType")
											where a.Tenant == 0
                                            select a).ToList();
                }
                if (tenant != 0)
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        IAmitalCloudContext context = AmitalCloudContext.GetContext(tenant);
                        currentObjectTableRules = (from a in context.ObjectTableRules.Include("ObjectTable").Include("RuleType")
												   where a.Tenant == tenant
                                                   select a).ToList();
                    }
                }
                objectTableRules = zeroObjectTableRules.Concat(currentObjectTableRules).ToList();

                //RuleConditionFieldRepository ruleConditionFieldRepository = new RuleConditionFieldRepository(tenant);
                foreach (ObjectTableRule rule in objectTableRules)
                {
                    ObjectTableRule existedRule = (from a in selectedRules
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



                //   rule.RuleConditionFields = RuleConditionFieldRepository.GetObjectRuleConditionFieldsByTenant(tenant).ToList();

                }


                CacheManager.CacheWrapper.Insert(listName, selectedRules, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
            }
            else
            {
                selectedRules = (List<ObjectTableRule>)CacheManager.CacheWrapper.Get(listName);
            }

            return selectedRules;
        }

       
        public void Add(ObjectTableRule entity)
        {
            context.ObjectTableRules.Add(entity);
        }

        public void Remove(ObjectTableRule entity)
        {
            try
            {
                context.ObjectTableRules.Attach(entity);
            }
            catch { }
            context.ObjectTableRules.Remove(entity);



        }

        public void Update(ObjectTableRule entity)
        {
            try
            {
                context.ObjectTableRules.Attach(entity);
            }
            catch 
            { }

            this.context.SetAsModified(entity);



        }

        public List<ObjectTableRule> All()
        {
            return context.ObjectTableRules.ToList();
        }

        public IAmitalCloudContext context
        {
            get { return amitalCloudContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ObjectTableRule> GetMulti(IEntityKeyFields<ObjectTableRule,string> entityKeys)
        {
            throw new NotImplementedException();
        }

        public ObjectTableRule GetSingle(IEntityKeyFields<ObjectTableRule,string> entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}