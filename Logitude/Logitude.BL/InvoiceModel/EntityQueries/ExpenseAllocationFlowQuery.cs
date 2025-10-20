using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class ExpenseAllocationFlowQuery
    {
        ExpenseAllocationFlowRepository repository;
        public ExpenseAllocationFlowQuery()
        {
            repository = new ExpenseAllocationFlowRepository(); 
        }


        public ExpenseAllocationFlowQuery(ExpenseAllocationFlowRepository expenseAllocationSettingRepository)
        {
            repository = expenseAllocationSettingRepository;
        }

        public ExpenseAllocationFlowQuery(int tenant)
        {
            repository = new ExpenseAllocationFlowRepository(tenant);
        }

        public ExpenseAllocationFlowPM GetSinglePM(string id , int tenant)
        {
            var entity = repository.GetSingleById(id,tenant);
            if (entity == null)
                return null;

            return new ExpenseAllocationFlowPM
            {
                Id = entity.Id,
                Tenant = entity.Tenant,
                RunDate = entity.RunDate,
                Status = entity.Status,
                JournalId = entity.JournalId,
                SettingId = entity.SettingId,
                JournalNumber = null 
            };
        }
        public IQueryable<ExpenseAllocationFlowList> GetIQueryableEntityList(IQueryable<ExpenseAllocationFlow> iQueryable)
        {
            IQueryable<ExpenseAllocationFlowList> result = from ExpenseAllocationFlow in iQueryable
                                                               select new ExpenseAllocationFlowList()
                                                               {
                                                                   Id = ExpenseAllocationFlow.Id,
                                                                   Tenant = ExpenseAllocationFlow.Tenant,
                                                                   RunDate = ExpenseAllocationFlow.RunDate,
                                                                   Status = ExpenseAllocationFlow.Status,
                                                                   JournalId = ExpenseAllocationFlow.JournalId,
                                                                   SettingId = ExpenseAllocationFlow.SettingId,
                                                               };
            return result;
        }


    }
}