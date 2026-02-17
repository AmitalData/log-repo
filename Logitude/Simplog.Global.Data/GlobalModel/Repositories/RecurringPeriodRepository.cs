using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure;
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
            return instance;
        }

        public IQueryable<RecurringPeriod> GetRecurringPeriods()
        {
            return context.RecurringPeriods;
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