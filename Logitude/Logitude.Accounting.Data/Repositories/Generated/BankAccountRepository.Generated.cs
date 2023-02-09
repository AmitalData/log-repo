 
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
   public partial class BankAccountRepository:IRepository<BankAccount>
   {
   
        private IAccountingContext currentContext;
        public BankAccountRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public BankAccountRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  BankAccount GetSingle(string id, int tenant)
        {
            return (from a in context.BankAccounts
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<BankAccount> GetAll(int tenant)
        {
            return from a in context.BankAccounts  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public BankAccount GetSingle(EntityKeyFields entityKeys)
        {
            BankAccountKeys keys = entityKeys as BankAccountKeys;
            return (from a in context.BankAccounts
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(BankAccount entity)
        {
            onAdd();
            context.BankAccounts.Add(entity);
        }

        public void Remove(BankAccount entity)
        {
            context.BankAccounts.Attach(entity);
            context.BankAccounts.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(BankAccount entity)
        {
            onUpdate();
            context.BankAccounts.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BankAccount> All()
        {
            return context.BankAccounts.ToList();
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
	 