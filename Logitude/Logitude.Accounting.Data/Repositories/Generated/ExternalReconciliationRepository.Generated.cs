 
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
   public partial class ExternalReconciliationRepository:IRepository<ExternalReconciliation>
   {
   
        private IAccountingContext currentContext;
        public ExternalReconciliationRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public ExternalReconciliationRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  ExternalReconciliation GetSingle(string id, int tenant)
        {
            return (from a in context.ExternalReconciliations
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ExternalReconciliation> GetAll(int tenant)
        {
            return from a in context.ExternalReconciliations  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ExternalReconciliation GetSingle(EntityKeyFields entityKeys)
        {
            ExternalReconciliationKeys keys = entityKeys as ExternalReconciliationKeys;
            return (from a in context.ExternalReconciliations
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ExternalReconciliation entity)
        {
            onAdd();
            context.ExternalReconciliations.Add(entity);
        }

        public void Remove(ExternalReconciliation entity)
        {
            context.ExternalReconciliations.Attach(entity);
            context.ExternalReconciliations.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ExternalReconciliation entity)
        {
            onUpdate();
            context.ExternalReconciliations.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ExternalReconciliation> All()
        {
            return context.ExternalReconciliations.ToList();
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
	 