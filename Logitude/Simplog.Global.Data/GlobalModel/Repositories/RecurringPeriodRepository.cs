using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class RecurringPeriodRepository : IRepository<RecurringPeriod>
    {
        IGlobalContext globalContext;

        public RecurringPeriodRepository(IGlobalContext context)
        {
            globalContext = context;
        }

        public RecurringPeriodRepository()
        {
            globalContext = new GlobalContext();
        }

        public RecurringPeriodRepository(int tenant)
        {
            globalContext = GlobalContext.GetContext();
        }

        public RecurringPeriod GetSingleRecurringPeriod(string code)
        {
            RecurringPeriod instance = (from i in context.RecurringPeriods
                                 where i.Code == code                                 
                                 select i).FirstOrDefault();
            string entityName = "RecurringPeriod" + code;
            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null && instance != null)
                {
                    CacheManager.CacheWrapper.Insert(entityName, instance, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    instance = (RecurringPeriod)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            return instance;
        }

        public IQueryable<RecurringPeriod> GetRecurringPeriods()
        {
            IQueryable<RecurringPeriod> items = context.RecurringPeriods;
            string entityName = "RecurringPeriods";
            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null && items != null)
                {
                    CacheManager.CacheWrapper.Insert(entityName, items, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    items = (IQueryable<RecurringPeriod>)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            return items;
        }

        public IQueryable<RecurringPeriod> GetAll()
        {
            return context.RecurringPeriods;
        }
        public void Add(RecurringPeriod entity)
        {
            context.RecurringPeriods.Add(entity);
        }

        public void Remove(RecurringPeriod entity)
        {
            context.RecurringPeriods.Attach(entity);
            context.RecurringPeriods.Remove(entity);
        }

        public void Update(RecurringPeriod entity)
        {
            context.RecurringPeriods.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<RecurringPeriod> All()
        {
            return context.RecurringPeriods.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<RecurringPeriod> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public RecurringPeriod GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}