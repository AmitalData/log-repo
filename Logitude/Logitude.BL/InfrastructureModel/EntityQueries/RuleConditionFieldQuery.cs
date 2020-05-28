using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class RuleConditionFieldQuery
    {
           RuleConditionFieldRepository repository;
        public RuleConditionFieldQuery()
        {
            repository = new RuleConditionFieldRepository(); 
        }

        public RuleConditionFieldQuery(int tenant)
        {
            repository = new RuleConditionFieldRepository(tenant);
        }

        public RuleConditionFieldQuery(RuleConditionFieldRepository ruleConditionFieldRepository)
        {
            repository = ruleConditionFieldRepository;
        }


        public IQueryable<RuleConditionFieldPM> GetRuleConditionFieldPMsByTenant(int tenant)
        {
            IQueryable<RuleConditionFieldPM> ruleConditionFields = from a in repository.context.RuleConditionFields.Include("ObjectField")
                                                                   where a.Tenant == tenant
                                                                   select new RuleConditionFieldPM()
                                                                   {

                                                                       Id = a.Id,
                                                                       ObjectFieldId = a.ObjectFieldId,
                                                                       ObjectFieldCode = a.ObjectFieldCode,
                                                                       ObjectFieldName = a.ObjectField.FieldName,
                                                                       Operator = a.Operator,
                                                                       Tenant = a.Tenant,
                                                                       ObjectTableRuleId = a.ObjectTableRuleId,
                                                                       Value = a.Value,

                                                                   };
            return ruleConditionFields;

        }



        public IQueryable<RuleConditionFieldPM> GetRuleConditionFieldsByRuleId(int tenant, string ruleId)
        {
            IQueryable<RuleConditionFieldPM> ruleConditionFields = from a in repository.context.RuleConditionFields.Include("ObjectField")
                                                                   where (a.Tenant == tenant) && a.ObjectTableRuleId == ruleId
                                                                   select new RuleConditionFieldPM()
                                                                   {

                                                                       Id = a.Id,
                                                                       ObjectFieldId = a.ObjectFieldId,
                                                                       ObjectFieldCode = a.ObjectFieldCode,
                                                                       ObjectFieldName = a.ObjectField.FieldName,
                                                                       Operator = a.Operator,
                                                                       Tenant = a.Tenant,
                                                                       ObjectTableRuleId = a.ObjectTableRuleId,
                                                                       Value = a.Value,

                                                                   };
            return ruleConditionFields;

        }


    }
}