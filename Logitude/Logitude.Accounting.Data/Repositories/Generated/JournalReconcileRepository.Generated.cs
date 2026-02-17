 
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
   public partial class JournalReconcileRepository:IRepository<JournalReconcile>
   {
   
        private IAccountingContext currentContext;
        public JournalReconcileRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public JournalReconcileRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  JournalReconcile GetSingle(string journalid, string ledgertransactionid, int tenant)
        {
            return (from a in context.JournalReconciles
                    where a.JournalId == journalid && a.LedgerTransactionId == ledgertransactionid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<JournalReconcile> GetAll(int tenant)
        {
            return from a in context.JournalReconciles  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public JournalReconcile GetSingle(EntityKeyFields entityKeys)
        {
            JournalReconcileKeys keys = entityKeys as JournalReconcileKeys;
            return (from a in context.JournalReconciles
                    where a.JournalId == keys.JournalId && a.LedgerTransactionId == keys.LedgerTransactionId
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(JournalReconcile entity)
        {
            onAdd();
            context.JournalReconciles.Add(entity);
        }

        public void Remove(JournalReconcile entity)
        {
            context.JournalReconciles.Attach(entity);
            context.JournalReconciles.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(JournalReconcile entity)
        {
            onUpdate();
            context.JournalReconciles.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<JournalReconcile> All()
        {
            return context.JournalReconciles.ToList();
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
	 