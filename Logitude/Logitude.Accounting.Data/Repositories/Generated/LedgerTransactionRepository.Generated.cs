 
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
   public partial class LedgerTransactionRepository:IRepository<LedgerTransaction>
   {
   
        private IAccountingContext currentContext;
        public LedgerTransactionRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public LedgerTransactionRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  LedgerTransaction GetSingle(string id, int tenant)
        {
            return (from a in context.LedgerTransactions
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<LedgerTransaction> GetAll(int tenant)
        {
            return from a in context.LedgerTransactions  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public LedgerTransaction GetSingle(EntityKeyFields entityKeys)
        {
            LedgerTransactionKeys keys = entityKeys as LedgerTransactionKeys;
            return (from a in context.LedgerTransactions
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(LedgerTransaction entity)
        {
            onAdd();
            context.LedgerTransactions.Add(entity);
        }

        public void Remove(LedgerTransaction entity)
        {
            context.LedgerTransactions.Attach(entity);
            context.LedgerTransactions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(LedgerTransaction entity)
        {
            onUpdate();
            context.LedgerTransactions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<LedgerTransaction> All()
        {
            return context.LedgerTransactions.ToList();
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
	 