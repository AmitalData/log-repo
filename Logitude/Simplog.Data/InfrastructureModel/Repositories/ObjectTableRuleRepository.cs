using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class ObjectTableRuleRepository:IRepository<ObjectTableRule>
    {

        IWebFreightContext webFreightContext;
        public ObjectTableRuleRepository()
        {
            //Context = new WebFreightContext();

        }
        public ObjectTableRuleRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }
        public ObjectTableRuleRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
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
                    IWebFreightContext context = WebFreightContext.GetContext(tenant);
                    zeroObjectTableRules = (from a in context.ObjectTableRules.Include("ObjectTable").Include("RuleType")
											where a.Tenant == 0
                                            select a).ToList();
                }
                if (tenant != 0)
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        IWebFreightContext context = WebFreightContext.GetContext(tenant);
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

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ObjectTableRule> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ObjectTableRule GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}