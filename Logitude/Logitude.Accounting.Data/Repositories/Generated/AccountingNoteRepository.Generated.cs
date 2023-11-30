 
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
   public partial class AccountingNoteRepository:IRepository<AccountingNote>
   {
   
        private IAccountingContext currentContext;
        public AccountingNoteRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public AccountingNoteRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  AccountingNote GetSingle(string id, int tenant)
        {
            return (from a in context.AccountingNotes
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<AccountingNote> GetAll(int tenant)
        {
            return from a in context.AccountingNotes  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public AccountingNote GetSingle(EntityKeyFields entityKeys)
        {
            AccountingNoteKeys keys = entityKeys as AccountingNoteKeys;
            return (from a in context.AccountingNotes
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AccountingNote entity)
        {
            onAdd();
            context.AccountingNotes.Add(entity);
        }

        public void Remove(AccountingNote entity)
        {
            context.AccountingNotes.Attach(entity);
            context.AccountingNotes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AccountingNote entity)
        {
            onUpdate();
            context.AccountingNotes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AccountingNote> All()
        {
            return context.AccountingNotes.ToList();
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
	 