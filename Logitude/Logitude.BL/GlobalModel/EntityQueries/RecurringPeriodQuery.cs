using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityLists;

namespace Logitude.BL.GlobalModel.EntityQueries
{
    public class RecurringPeriodQuery
    {
        RecurringPeriodRepository repository;

        public RecurringPeriodQuery()
        {
            repository = new RecurringPeriodRepository(); 
        }

        public RecurringPeriodQuery(int tenant)
        {
            repository = new RecurringPeriodRepository(tenant);
        }

        public RecurringPeriodQuery(RecurringPeriodRepository recurringPeriodRepository)
        {
            repository = recurringPeriodRepository;
        }

        public RecurringPeriodPM GetSingleRecurringPeriodPM(string code)
        {
            return (from a in repository.context.RecurringPeriods
                    where a.Code == code
                    select new RecurringPeriodPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public IQueryable<RecurringPeriodList> GetIQueryableEntityList(IQueryable<RecurringPeriod> iQueryable)
        {
            IQueryable<RecurringPeriodList> result = from entity in iQueryable
                                                     select new RecurringPeriodList()
                                                     {
                                                         Name = entity.Name,
                                                         Code = entity.Code,
                                                         SearchFields = entity.SearchFields,
                                                     };
            return result;
        }
    }
}