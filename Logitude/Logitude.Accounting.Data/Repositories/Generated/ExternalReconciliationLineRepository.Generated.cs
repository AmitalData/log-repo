 
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
   public partial class ExternalReconciliationLineRepository:IRepository<ExternalReconciliationLine>
   {
   
        private IAccountingContext currentContext;
        public ExternalReconciliationLineRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public ExternalReconciliationLineRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  ExternalReconciliationLine GetSingle(string reconciliationid, int line, int tenant)
        {
            return (from a in context.ExternalReconciliationLines
                    where a.ReconciliationId == reconciliationid && a.Line == line && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ExternalReconciliationLine> GetAll(int tenant)
        {
            return from a in context.ExternalReconciliationLines  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ExternalReconciliationLine GetSingle(EntityKeyFields entityKeys)
        {
            ExternalReconciliationLineKeys keys = entityKeys as ExternalReconciliationLineKeys;
            return (from a in context.ExternalReconciliationLines
                    where a.ReconciliationId == keys.ReconciliationId && a.Line == keys.Line
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ExternalReconciliationLine entity)
        {
            onAdd();
            context.ExternalReconciliationLines.Add(entity);
        }

        public void Remove(ExternalReconciliationLine entity)
        {
            context.ExternalReconciliationLines.Attach(entity);
            context.ExternalReconciliationLines.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ExternalReconciliationLine entity)
        {
            onUpdate();
            context.ExternalReconciliationLines.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ExternalReconciliationLine> All()
        {
            return context.ExternalReconciliationLines.ToList();
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
	 