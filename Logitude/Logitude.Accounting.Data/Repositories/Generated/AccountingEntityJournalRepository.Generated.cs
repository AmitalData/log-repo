 
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
   public partial class AccountingEntityJournalRepository:IRepository<AccountingEntityJournal>
   {
   
        private IAccountingContext currentContext;
        public AccountingEntityJournalRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public AccountingEntityJournalRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  AccountingEntityJournal GetSingle(string id, int tenant)
        {
            return (from a in context.AccountingEntitiesJournals
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<AccountingEntityJournal> GetAll(int tenant)
        {
            return from a in context.AccountingEntitiesJournals  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public AccountingEntityJournal GetSingle(EntityKeyFields entityKeys)
        {
            AccountingEntityJournalKeys keys = entityKeys as AccountingEntityJournalKeys;
            return (from a in context.AccountingEntitiesJournals
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AccountingEntityJournal entity)
        {
            onAdd();
            context.AccountingEntitiesJournals.Add(entity);
        }

        public void Remove(AccountingEntityJournal entity)
        {
            context.AccountingEntitiesJournals.Attach(entity);
            context.AccountingEntitiesJournals.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AccountingEntityJournal entity)
        {
            onUpdate();
            context.AccountingEntitiesJournals.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AccountingEntityJournal> All()
        {
            return context.AccountingEntitiesJournals.ToList();
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
	 