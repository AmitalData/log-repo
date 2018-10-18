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
    public class ObjectTableRuleFieldRepository : IRepository<ObjectTableRuleField>
    {

        IWebFreightContext webFreightContext;
        public ObjectTableRuleFieldRepository()
        {
            //Context = new WebFreightContext();

        }
        public ObjectTableRuleFieldRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }
        public ObjectTableRuleFieldRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public IQueryable<ObjectTableRuleField> GetObjectTableRuleFields(int tenant)
        {
            return (from record in context.ObjectTableRuleFields where record.Tenant == tenant select record);
        }

        public ObjectTableRuleField GetSingleObjectTableRuleField(string id, int tenant)
        {
            return (from record in context.ObjectTableRuleFields where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

      
        //public static List<ObjectTableRuleField> GetObjectTableRuleFieldByObjectTableRuleId(string objectTableRuleId, int tenant)
        //{
        //    List<ObjectTableRuleField> zeroquery;
        //    List<ObjectTableRuleField> currentquery;
        //    List<ObjectTableRuleField> query = new List<ObjectTableRuleField>();
        //    //List<ObjectTableRuleField> RuleFields;

        //    string RulesFieldsListName = "RuleFields" + objectTableRuleId + tenant;
        //    if (HttpContext.Current != null)
        //    {
        //        if (HttpContext.Current.Cache.Get(RulesFieldsListName) == null)
        //        {
        //            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
        //            {
        //                WebFreightContext context = WebFreightContext.GetContext(tenant);
        //                currentquery = (from a in context.ObjectTableRuleFields
        //                                where (a.Tenant == tenant) && a.ObjectTableRuleId == objectTableRuleId
        //                                select a).ToList();
        //            }

        //            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
        //            {
        //                WebFreightContext context = WebFreightContext.GetContext(0);
        //                zeroquery = (from a in context.ObjectTableRuleFields
        //                             where (a.Tenant == 0) && a.ObjectTableRuleId == objectTableRuleId
        //                             select a).ToList();
        //            }

        //            query = zeroquery.Concat(currentquery).ToList();

        //            HttpContext.Current.Cache.Insert(RulesFieldsListName, query, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
        //        }
        //        else
        //        {
        //            query = (List<ObjectTableRuleField>)HttpContext.Current.Cache.Get(RulesFieldsListName);
        //        }
        //    }
        //    else
        //    {

        //        using (TransactionScope scope = TransactionFactory.GetNewTransaction())
        //        {
        //            WebFreightContext context = WebFreightContext.GetContext(tenant);
        //            currentquery = (from a in context.ObjectTableRuleFields
        //                            where (a.Tenant == tenant) && a.ObjectTableRuleId == objectTableRuleId
        //                            select a).ToList();
        //        }

        //        using (TransactionScope scope = TransactionFactory.GetNewTransaction())
        //        {
        //            WebFreightContext context = WebFreightContext.GetContext(0);
        //            zeroquery = (from a in context.ObjectTableRuleFields
        //                         where (a.Tenant == 0) && a.ObjectTableRuleId == objectTableRuleId
        //                         select a).ToList();
        //        }

        //        query = zeroquery.Concat(currentquery).ToList();
        //    }

        //    return query;
        //}


        public static List<ObjectTableRuleField> GetTenantRuleFields(int tenant)
        {
            List<ObjectTableRuleField> zeroquery;
            List<ObjectTableRuleField> currentquery;
            List<ObjectTableRuleField> query = new List<ObjectTableRuleField>();
            //List<ObjectTableRuleField> RuleFields;

            string rulesFieldsListName = "RuleFields" + tenant;
            if (HttpContext.Current != null)
            {
                if (CacheManager.CacheWrapper.Get(rulesFieldsListName) == null)
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        IWebFreightContext context = WebFreightContext.GetContext(tenant);
                        currentquery = (from a in context.ObjectTableRuleFields.Include("ObjectField").Include("ObjectTableRule")
                                        where (a.Tenant == tenant) 
                                        select a).ToList();
                    }

                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        IWebFreightContext context = WebFreightContext.GetContext(0);
                        zeroquery = (from a in context.ObjectTableRuleFields.Include("ObjectField").Include("ObjectTableRule")
                                     where (a.Tenant == 0)
                                     select a).ToList();
                    }

                    query = zeroquery.Concat(currentquery).ToList();

                    CacheManager.CacheWrapper.Insert(rulesFieldsListName, query, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    query = (List<ObjectTableRuleField>)CacheManager.CacheWrapper.Get(rulesFieldsListName);
                }
            }
            else
            {

                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    IWebFreightContext context = WebFreightContext.GetContext(tenant);
                    currentquery = (from a in context.ObjectTableRuleFields.Include("ObjectField").Include("ObjectTableRule")
                                    where (a.Tenant == tenant)
                                    select a).ToList();
                }

                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                   IWebFreightContext context = WebFreightContext.GetContext(0);
                   zeroquery = (from a in context.ObjectTableRuleFields.Include("ObjectField").Include("ObjectTableRule")
                                 where (a.Tenant == 0) 
                                 select a).ToList();
                }

                query = zeroquery.Concat(currentquery).ToList();
            }

            return query;
        }


        public void Add(ObjectTableRuleField entity)
        {
            context.ObjectTableRuleFields.Add(entity);
        }

        public void Remove(ObjectTableRuleField entity)
        {
            try
            {
                context.ObjectTableRuleFields.Attach(entity);
            }
            catch { }
            context.ObjectTableRuleFields.Remove(entity);



        }

        public void Update(ObjectTableRuleField entity)
        {
            try
            {
                context.ObjectTableRuleFields.Attach(entity);
            }
            catch { }
            this.context.SetAsModified(entity);



        }

        public List<ObjectTableRuleField> All()
        {
            return context.ObjectTableRuleFields.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ObjectTableRuleField> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ObjectTableRuleField GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
