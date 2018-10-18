using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class ObjectTableRuleFieldQuery
    {
        ObjectTableRuleFieldRepository repository;
        public ObjectTableRuleFieldQuery()
        {
            repository = new ObjectTableRuleFieldRepository(); 
        }

        public ObjectTableRuleFieldQuery(int tenant)
        {
            repository = new ObjectTableRuleFieldRepository(tenant);
        }

        public ObjectTableRuleFieldQuery(ObjectTableRuleFieldRepository objectTableRuleFieldRepository )
        {
            repository = objectTableRuleFieldRepository;
        }

        public ObjectTableRuleFieldPM GetSinglePM(string id, int tenant)
        {
            ObjectTableRuleFieldPM objectTableRuleFieldPm = (from a in repository.context.ObjectTableRuleFields.Include("ObjectField").Include("ObjectTableRule")
                                                             where a.Tenant == tenant && a.Id == id
                                                             select new ObjectTableRuleFieldPM()
                                                             {
                                                                 Id = a.Id,
                                                                 Tenant = a.Tenant,
                                                                 ObjectFieldId = a.ObjectFieldId,
                                                                 ObjectTableRuleId = a.ObjectTableRuleId,
                                                                 SystemLevel = a.SystemLevel,
                                                                 ObjectFieldName = a.ObjectField.FieldName,
                                                                 Expression = a.Expression,
                                                                 ObjectTableRuleCode = a.ObjectTableRule.RuleCode,
                                                                 ObjectTableRuleTypeCode = a.ObjectTableRule.RuleTypeCode,
                                                                 RuleNotificationTypeCode = a.RuleNotificationTypeCode,
                                                             }).FirstOrDefault();




            return objectTableRuleFieldPm;
        }

        public ObjectTableRuleFieldPM GetSingleObjectTableRuleFieldPMById(string id, int tenant)
        {
            ObjectTableRuleFieldPM objectTableRuleFieldPm = (from a in repository.context.ObjectTableRuleFields.Include("ObjectField").Include("ObjectTableRule")
                                                             where (a.Tenant == tenant || a.Tenant == 0) && a.Id == id
                                                             select new ObjectTableRuleFieldPM()
                                                             {
                                                                 Id = a.Id,
                                                                 Tenant = a.Tenant,
                                                                 ObjectFieldId = a.ObjectFieldId,
                                                                 ObjectTableRuleId = a.ObjectTableRuleId,
                                                                 SystemLevel = a.SystemLevel,
                                                                 ObjectFieldName = a.ObjectField.FieldName,
                                                                 Expression = a.Expression,
                                                                 ObjectTableRuleCode = a.ObjectTableRule.RuleCode,
                                                                 ObjectTableRuleTypeCode = a.ObjectTableRule.RuleTypeCode,
                                                                 RuleNotificationTypeCode = a.RuleNotificationTypeCode,
                                                             }).FirstOrDefault();
            return objectTableRuleFieldPm;
        }



        public IQueryable<ObjectTableRuleFieldPM> GetObjectTableRuleFieldPMsByTenant(int tenant)
        {
            IQueryable<ObjectTableRuleFieldPM> objectTableRuleFieldPMs = (from a in repository.context.ObjectTableRuleFields.Include("ObjectField").Include("ObjectTableRule")
                                                                          where a.Tenant == tenant || a.Tenant == 0
                                                                          select new ObjectTableRuleFieldPM()
                                                                          {
                                                                              Id = a.Id,
                                                                              Tenant = a.Tenant,
                                                                              ObjectFieldId = a.ObjectFieldId,
                                                                              ObjectTableRuleId = a.ObjectTableRuleId,
                                                                              SystemLevel = a.SystemLevel,
                                                                              ObjectFieldName = a.ObjectField.FieldName,
                                                                              Expression = a.Expression,
                                                                              ObjectTableRuleCode = a.ObjectTableRule.RuleCode,
                                                                              ObjectTableRuleTypeCode = a.ObjectTableRule.RuleTypeCode,
                                                                              RuleNotificationTypeCode = a.RuleNotificationTypeCode,
                                                                          }).AsQueryable();
            return objectTableRuleFieldPMs;
        }

        public IQueryable<ObjectTableRuleFieldPM> GetObjectTableRuleFieldPMsByObjectTableRuleId(string objectTableRuleId, int tenant)
        {


            IQueryable<ObjectTableRuleFieldPM> query = (from a in repository.context.ObjectTableRuleFields.Include("ObjectField").Include("ObjectTableRule")
                                                        where (a.Tenant == tenant || a.Tenant == 0) && a.ObjectTableRuleId == objectTableRuleId
                                                        select new ObjectTableRuleFieldPM()
                                                        {
                                                            Id = a.Id,
                                                            Tenant = a.Tenant,
                                                            ObjectFieldId = a.ObjectFieldId,
                                                            ObjectTableRuleId = a.ObjectTableRuleId,
                                                            SystemLevel = a.SystemLevel,
                                                            ObjectFieldName = a.ObjectField.FieldName,
                                                            Expression = a.Expression,
                                                            ObjectTableRuleCode = a.ObjectTableRule.RuleCode,
                                                            ObjectTableRuleTypeCode = a.ObjectTableRule.RuleTypeCode,
                                                            RuleNotificationTypeCode = a.RuleNotificationTypeCode,
                                                        }).AsQueryable();




            return query;
        }


    }
}