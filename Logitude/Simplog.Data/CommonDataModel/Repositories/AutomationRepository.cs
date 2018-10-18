using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System.Web;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;
using System;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class AutomationRepository : IRepository<Automation>
    {
        ICommonDataContext commonDataContext;

        public AutomationRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public AutomationRepository(ICommonDataContext context)
        {
            commonDataContext = context;

        }

        public AutomationRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public Automation GetSingleAutomation(string id, int tenant)
        {
            return (from a in this.context.Automations
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Automation> GetAutomations(int tenant)
        {
            return (from a in this.context.Automations
                    where a.Tenant == tenant
                    select a);
        }

        

        public void Add(Automation entity)
        {
            this.context.Automations.Add(entity);
        }

        public void Remove(Automation entity)
        {

            this.context.Automations.Attach(entity);

            this.context.Automations.Remove(entity);
        }

        public void Update(Automation entity)
        {
            this.context.Automations.Attach(entity);

            this.context.SetAsModified(entity);
        }

        public List<Automation> All()
        {
            return this.context.Automations.ToList();
        }

        public ICommonDataContext context
        {
            get { return this.commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }


        public List<Automation> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Automation GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public List<Automation> GetAutomationsByObjectTableId(string objectTableId, int tenant, string lastUpdateDate)
        {

            string automationListName = "automations" + objectTableId.ToLower() + tenant + lastUpdateDate;

            List<Automation> result = new List<Automation>();

            List<Automation> currentAutomations = new List<Automation>();
            if (tenant != 0)
            {
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(automationListName) == null)
                    {

                        using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                        {

                            currentAutomations = (from a in this.context.Automations
                                    where a.Tenant == tenant && a.ObjectTableId == objectTableId &&  !a.Inactive
                                     select a).ToList();

                            scope.Complete();
                        }



                        CacheManager.CacheWrapper.Insert(automationListName, currentAutomations, null, System.DateTime.UtcNow.AddHours(12), TimeSpan.Zero);
                    }
                    else
                    {
                        currentAutomations = (List<Automation>)CacheManager.CacheWrapper.Get(automationListName);
                    }
                }
                else
                {

                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {

                        currentAutomations  = (from a in this.context.Automations
                                             where a.Tenant == tenant && a.ObjectTableId == objectTableId && !a.Inactive
                                             select a).ToList();

                        scope.Complete();
                    }

                }
            }




            result = currentAutomations.ToList();



            return result;


        }


        public bool CheckIfEntityHaveAutomation(string objectTableId, string type,int tenant)
        {
            bool result = false;
            Automation automation=  (from a in this.context.Automations
                                    where a.ObjectTableId == objectTableId && a.Type == type && a.Tenant == tenant && !a.Inactive
                                    select a).FirstOrDefault();

            if (automation != null) result = true;
            return result;
        }





    public    Automation GetAutomationByIdFromCache(string objectTableId, string automationId, int tenant, string lastUpdateDate)
        {

            string automationListName = "automations" + objectTableId.ToLower() + tenant + lastUpdateDate;



            Automation currentAutomation = null;
            if (tenant != 0)
            {
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(automationListName) == null)
                    {

                        using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                        {

                            currentAutomation = (from a in this.context.Automations
                                                  where a.Tenant == tenant && a.ObjectTableId == objectTableId && !a.Inactive && a.Id == automationId
                                                  select a).FirstOrDefault();

                            scope.Complete();
                        }

                    }

                    else
                    {
                       List<Automation> result  = (List<Automation>)CacheManager.CacheWrapper.Get(automationListName);
                       if (result != null)
                       {
                           currentAutomation = result.Where(d => d.Id == automationId).FirstOrDefault();
                       }
                    }
                   
                }
                else
                {

                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {

                        currentAutomation = (from a in this.context.Automations
                                             where a.Tenant == tenant && a.ObjectTableId == objectTableId && !a.Inactive && a.Id == automationId
                                             select a).FirstOrDefault();

                        scope.Complete();
                    }

                }
            }








            return currentAutomation;


        }

      


    }
}