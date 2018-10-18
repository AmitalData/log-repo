 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class AccountingPeriodRepository:IRepository<AccountingPeriod>
   {
   
        private IAccountingContext currentContext;
        public AccountingPeriodRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public AccountingPeriodRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  AccountingPeriod GetSingle(string id, int tenant)
        {
            return (from a in context.AccountingPeriods
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<AccountingPeriod> GetAll(int tenant)
        {
            return from a in context.AccountingPeriods  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public AccountingPeriod GetSingle(EntityKeyFields entityKeys)
        {
            AccountingPeriodKeys keys = entityKeys as AccountingPeriodKeys;
            return (from a in context.AccountingPeriods
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AccountingPeriod entity)
        {
            onAdd();
            context.AccountingPeriods.Add(entity);
        }

        public void Remove(AccountingPeriod entity)
        {
            context.AccountingPeriods.Attach(entity);
            context.AccountingPeriods.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AccountingPeriod entity)
        {
            onUpdate();
            context.AccountingPeriods.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AccountingPeriod> All()
        {
            return context.AccountingPeriods.ToList();
        }

        private IAccountingContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 