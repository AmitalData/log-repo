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
    public class ExpenseAllocationSettingQuery
    {
        ExpenseAllocationSettingRepository repository;
        public ExpenseAllocationSettingQuery()
        {
            repository = new ExpenseAllocationSettingRepository(); 
        }


        public ExpenseAllocationSettingQuery(ExpenseAllocationSettingRepository expenseAllocationSettingRepository)
        {
            repository = expenseAllocationSettingRepository;
        }

        public ExpenseAllocationSettingQuery(int tenant)
        {
            repository = new ExpenseAllocationSettingRepository(tenant);
        }

      
        public ExpenseAllocationSettingPM GetSingleByEntityIdAndObjectTable(string entityId,string objectTableId, int tenant)
        {
           var  expenseAllocationSetting=  repository.GetSingleByEntityIdAndObjectTable(entityId, objectTableId, tenant);
            if (expenseAllocationSetting == null)
                return null;    
            return new ExpenseAllocationSettingPM()
            {
                Id = expenseAllocationSetting.Id,
                Tenant = expenseAllocationSetting.Tenant,
                CreateDate = expenseAllocationSetting.CreateDate,
                UpdateDate = expenseAllocationSetting.UpdateDate,
                EntityId = expenseAllocationSetting.EntityId,
                ObjectTableId = expenseAllocationSetting.ObjectTableId,
                StartDateTime = expenseAllocationSetting.StartDateTime,
                EndDateTime = expenseAllocationSetting.EndDateTime,
                NumberOfPayments = expenseAllocationSetting.NumberOfPayments,
                MonthInterval = expenseAllocationSetting.MonthInterval,
                PaymentDateType = expenseAllocationSetting.PaymentDateType,
                CreatedByUserId = expenseAllocationSetting.CreatedByUserId,
            };
           
        }


        public ExpenseAllocationSettingPM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.context.ExpenseAllocationSettings
                    where a.Id == id && a.Tenant == tenant
                    select new ExpenseAllocationSettingPM()
                    {
                           CreateDate = a.CreateDate,
                            EndDateTime = a.EndDateTime,
                            EntityId = a.EntityId,
                            Id = a.Id,
                            MonthInterval = a.MonthInterval,
                            NumberOfPayments = a.NumberOfPayments,
                            ObjectTableId = a.ObjectTableId,
                            PaymentDateType = a.PaymentDateType,
                            StartDateTime = a.StartDateTime,
                            Tenant = a.Tenant,
                            UpdateDate = a.UpdateDate,
                            CreatedByUserId = a.CreatedByUserId,
                            UpdatedByUserId =a.UpdatedByUserId


                    }).FirstOrDefault();
        }


    }
}