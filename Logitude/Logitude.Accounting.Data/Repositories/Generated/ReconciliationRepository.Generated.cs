 
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
   public partial class ReconciliationRepository:IRepository<Reconciliation>
   {
   
        private IAccountingContext currentContext;
        public ReconciliationRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public ReconciliationRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  Reconciliation GetSingle(string id, int tenant)
        {
            return (from a in context.Reconciliations
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Reconciliation> GetAll(int tenant)
        {
            return from a in context.Reconciliations  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Reconciliation GetSingle(EntityKeyFields entityKeys)
        {
            ReconciliationKeys keys = entityKeys as ReconciliationKeys;
            return (from a in context.Reconciliations
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Reconciliation entity)
        {
            onAdd();
            context.Reconciliations.Add(entity);
        }

        public void Remove(Reconciliation entity)
        {
            context.Reconciliations.Attach(entity);
            context.Reconciliations.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Reconciliation entity)
        {
            onUpdate();
            context.Reconciliations.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Reconciliation> All()
        {
            return context.Reconciliations.ToList();
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
	 