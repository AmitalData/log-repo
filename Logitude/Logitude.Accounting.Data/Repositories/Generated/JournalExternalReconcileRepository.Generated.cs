 
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
   public partial class JournalExternalReconcileRepository:IRepository<JournalExternalReconcile>
   {
   
        private IAccountingContext currentContext;
        public JournalExternalReconcileRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public JournalExternalReconcileRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  JournalExternalReconcile GetSingle(string journalid, int line, int tenant)
        {
            return (from a in context.JournalExternalReconciles
                    where a.JournalId == journalid && a.Line == line && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<JournalExternalReconcile> GetAll(int tenant)
        {
            return from a in context.JournalExternalReconciles  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public JournalExternalReconcile GetSingle(EntityKeyFields entityKeys)
        {
            JournalExternalReconcileKeys keys = entityKeys as JournalExternalReconcileKeys;
            return (from a in context.JournalExternalReconciles
                    where a.JournalId == keys.JournalId && a.Line == keys.Line
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(JournalExternalReconcile entity)
        {
            onAdd();
            context.JournalExternalReconciles.Add(entity);
        }

        public void Remove(JournalExternalReconcile entity)
        {
            context.JournalExternalReconciles.Attach(entity);
            context.JournalExternalReconciles.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(JournalExternalReconcile entity)
        {
            onUpdate();
            context.JournalExternalReconciles.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<JournalExternalReconcile> All()
        {
            return context.JournalExternalReconciles.ToList();
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
	 