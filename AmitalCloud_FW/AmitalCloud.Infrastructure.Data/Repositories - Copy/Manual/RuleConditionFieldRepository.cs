using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class RuleConditionFieldRepository:IRepository<RuleConditionField, string>
    {
        IAmitalCloudContext amitalCloudContext;
        public RuleConditionFieldRepository(IAmitalCloudContext context)
        {
            amitalCloudContext = context;
        }

        public RuleConditionFieldRepository()
        {
            amitalCloudContext = new AmitalCloudContext();
        }
        public RuleConditionFieldRepository(int tenant)
        {
            amitalCloudContext = AmitalCloudContext.GetContext(tenant);
        }
        public RuleConditionField GetSingleRuleConditionField(string id,int tenant)
        {
            return (from a in context.RuleConditionFields.Include("ObjectField")
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<RuleConditionField> GetRuleConditionFieldsByTenant(int tenant)
        {
            IQueryable<RuleConditionField> ruleConditionFields = from a in context.RuleConditionFields.Include("ObjectField")
                                                              where a.Tenant == tenant
                                                              select a;
            return ruleConditionFields;

        }

        public IQueryable<RuleConditionField> GetRuleConditionFieldsByRuleId(string ruleId,int tenant)
        {
            IQueryable<RuleConditionField> ruleConditionFields = from a in context.RuleConditionFields.Include("ObjectField")
                                                                 where a.Tenant == tenant && a.ObjectTableRuleId == ruleId
                                                                 select a;
            return ruleConditionFields;

        }


        public static List<RuleConditionField> GetObjectRuleConditionFieldsByTenant(int tenant)
        {

            string listName = "ruleconditionfieldstenant" + tenant;

            //IQueryable<RuleConditionField> ruleConditionFields = from a in context.RuleConditionFields
            //                                                       where (a.Tenant == tenant || a.Tenant == 0) && a.ObjectTableRuleId == ruleId
            //                                                       select a;

            List<RuleConditionField> zeroRuleConditionField;
            List<RuleConditionField> currentRuleConditionField = new List<RuleConditionField>();
            List<RuleConditionField> ruleConditionField;
            List<RuleConditionField> selectedFields = new List<RuleConditionField>();
            if (CacheManager.CacheWrapper.Get(listName) == null)
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    IAmitalCloudContext context = AmitalCloudContext.GetContext(0);
                    zeroRuleConditionField = (from a in context.RuleConditionFields.Include("ObjectField") 
                                            where a.Tenant == 0
                                            select a).ToList();
                }
                if (tenant != 0)
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        IAmitalCloudContext context = AmitalCloudContext.GetContext(tenant);
                        currentRuleConditionField = (from a in context.RuleConditionFields.Include("ObjectField")
                                                     where a.Tenant == tenant
                                                     select a).ToList();
                    }


                }

                ruleConditionField = zeroRuleConditionField.Concat(currentRuleConditionField).ToList();


                foreach (RuleConditionField field in ruleConditionField)
                {
                    RuleConditionField existedRuleField = (from a in selectedFields
                                                   where a.ObjectFieldCode == field.ObjectFieldCode && a.ObjectTableRuleId == field.ObjectTableRuleId
                                                   select a).FirstOrDefault();
                    if (existedRuleField != null)
                    {
                        if (existedRuleField.Tenant == 0 && existedRuleField.Tenant == tenant)
                        {
                            selectedFields.Remove(existedRuleField);
                            selectedFields.Add(field);
                        }
                    }
                    else
                    {

                        selectedFields.Add(field);
                    }



                   

                }


                CacheManager.CacheWrapper.Insert(listName, selectedFields, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
            }
            else
            {
                selectedFields = (List<RuleConditionField>)CacheManager.CacheWrapper.Get(listName);
            }

            return selectedFields;

        }




        public void Add(RuleConditionField entity)
        {
            context.RuleConditionFields.Add(entity);
        }

        public void Remove(RuleConditionField entity)
        {
            try
            {
                context.RuleConditionFields.Attach(entity);
            }
            catch { }
            context.RuleConditionFields.Remove(entity);
        }

        public void Update(RuleConditionField entity)
        {
            try
            {
                context.RuleConditionFields.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<RuleConditionField> All()
        {
            return context.RuleConditionFields.ToList();
        }

        public IAmitalCloudContext context
        {
            get { return amitalCloudContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<RuleConditionField> GetMulti(IEntityKeyFields<RuleConditionField,string> entityKeys)
        {
            throw new NotImplementedException();
        }

        public RuleConditionField GetSingle(IEntityKeyFields<RuleConditionField,string> entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
