 
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
   public partial class AccountingIntegrityCheckRepository:IRepository<AccountingIntegrityCheck>
   {
   
        private IAccountingContext currentContext;
        public AccountingIntegrityCheckRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public AccountingIntegrityCheckRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  AccountingIntegrityCheck GetSingle(string id, int tenant)
        {
            return (from a in context.AccountingIntegrityChecks
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<AccountingIntegrityCheck> GetAll(int tenant)
        {
            return from a in context.AccountingIntegrityChecks  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public AccountingIntegrityCheck GetSingle(EntityKeyFields entityKeys)
        {
            AccountingIntegrityCheckKeys keys = entityKeys as AccountingIntegrityCheckKeys;
            return (from a in context.AccountingIntegrityChecks
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AccountingIntegrityCheck entity)
        {
            onAdd();
            context.AccountingIntegrityChecks.Add(entity);
        }

        public void Remove(AccountingIntegrityCheck entity)
        {
            context.AccountingIntegrityChecks.Attach(entity);
            context.AccountingIntegrityChecks.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AccountingIntegrityCheck entity)
        {
            onUpdate();
            context.AccountingIntegrityChecks.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AccountingIntegrityCheck> All()
        {
            return context.AccountingIntegrityChecks.ToList();
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
	 