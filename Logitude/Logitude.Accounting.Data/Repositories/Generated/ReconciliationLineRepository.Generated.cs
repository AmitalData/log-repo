 
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
   public partial class ReconciliationLineRepository:IRepository<ReconciliationLine>
   {
   
        private IAccountingContext currentContext;
        public ReconciliationLineRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public ReconciliationLineRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  ReconciliationLine GetSingle(string reconciliationid, int line, int tenant)
        {
            return (from a in context.ReconciliationLines
                    where a.ReconciliationId == reconciliationid && a.Line == line && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ReconciliationLine> GetAll(int tenant)
        {
            return from a in context.ReconciliationLines  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ReconciliationLine GetSingle(EntityKeyFields entityKeys)
        {
            ReconciliationLineKeys keys = entityKeys as ReconciliationLineKeys;
            return (from a in context.ReconciliationLines
                    where a.ReconciliationId == keys.ReconciliationId && a.Line == keys.Line
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ReconciliationLine entity)
        {
            onAdd();
            context.ReconciliationLines.Add(entity);
        }

        public void Remove(ReconciliationLine entity)
        {
            context.ReconciliationLines.Attach(entity);
            context.ReconciliationLines.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ReconciliationLine entity)
        {
            onUpdate();
            context.ReconciliationLines.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ReconciliationLine> All()
        {
            return context.ReconciliationLines.ToList();
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
	 