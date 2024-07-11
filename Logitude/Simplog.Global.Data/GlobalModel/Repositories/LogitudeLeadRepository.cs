using System.Collections.Generic;
using System.Linq;

using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class LogitudeLeadRepository:IRepository<LogitudeLead>
    {
        IGlobalContext globalContext;
        public LogitudeLeadRepository()
        {
            globalContext = GlobalContext.GetContext();
        }

        public LogitudeLeadRepository(IGlobalContext context)
        {
            globalContext = context;
        }


        public LogitudeLeadRepository(int tenant)
        {
            globalContext = GlobalContext.GetContext();
        }



        public IQueryable<LogitudeLead> GetOpenLeads()
        {

            return (from a in context.LogitudeLeads
                    where a.StatusCode == "InProgress"
                    select a);
        }

        public LogitudeLead GetSingleLogitudeLead(string id)
        {
            LogitudeLead item = (from a in context.LogitudeLeads
                    where a.Id == id
                    select a).FirstOrDefault();

            string entityName = "LogitudeLead" + id;
            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null && item != null)
                {
                    CacheManager.CacheWrapper.Insert(entityName, item, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    item = (LogitudeLead)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            return item;
        }

        public LogitudeLead GetFirstNotCompletedLogitudeLead()
        {
            return (from a in context.LogitudeLeads
                    where a.StatusCode == "InProgress" && (a.IsEmailVerified || (a.IsEmailVerified == false && a.IsSentToCustomer == false)) //&& (a.IsEmailVerified == true || a.IsSentToCustomer == false)
                    select a).FirstOrDefault();
        }


        //public LogitudeLead GetFirstSingleLogitudeLeadSent()
        //{
        //    return (from a in context.LogitudeLeads
        //            where a.IsSentToCustomer == true && a.IsEmailVerified == true && a.StatusCode == "InProgress"
        //            select a).FirstOrDefault();
        //}

        public IQueryable<LogitudeLead> GetAllLogitudeLeads()
        {
            IQueryable<LogitudeLead> items = from a in context.LogitudeLeads
                                            select a;
            string entityName = "AllLogitudeLeads";
            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null && items != null)
                {
                    CacheManager.CacheWrapper.Insert(entityName, items, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    items = (IQueryable<LogitudeLead>)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            return items;
        }

        public void Add(LogitudeLead entity)
        {
            context.LogitudeLeads.Add(entity);
        }

        public void Remove(LogitudeLead entity)
        {
            context.LogitudeLeads.Attach(entity);
            context.LogitudeLeads.Remove(entity);
        }

        public void Update(LogitudeLead entity)
        {
            context.LogitudeLeads.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<LogitudeLead> All()
        {
            return context.LogitudeLeads.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<LogitudeLead> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public LogitudeLead GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<LogitudeLead> GetLogitudeLeadsByTenant(int tenant)
        {
            IQueryable<LogitudeLead> logitudelead = from a in context.LogitudeLeads  select a;
            return logitudelead;
        }



        public LogitudeLead GetSingleLogitudeLeadByCustomerId(string customerid)
        {
            return (from a in context.LogitudeLeads
                    where a.CustomerId == customerid 
                    select a).FirstOrDefault();
        }
    }
}