 
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
   public partial class ReconcileExternalPageLineRepository:IRepository<ReconcileExternalPageLine>
   {
   
        private IAccountingContext currentContext;
        public ReconcileExternalPageLineRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public ReconcileExternalPageLineRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  ReconcileExternalPageLine GetSingle(string id, int tenant)
        {
            return (from a in context.ReconcileExternalPageLines
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ReconcileExternalPageLine> GetAll(int tenant)
        {
            return from a in context.ReconcileExternalPageLines  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ReconcileExternalPageLine GetSingle(EntityKeyFields entityKeys)
        {
            ReconcileExternalPageLineKeys keys = entityKeys as ReconcileExternalPageLineKeys;
            return (from a in context.ReconcileExternalPageLines
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ReconcileExternalPageLine entity)
        {
            onAdd();
            context.ReconcileExternalPageLines.Add(entity);
        }

        public void Remove(ReconcileExternalPageLine entity)
        {
            context.ReconcileExternalPageLines.Attach(entity);
            context.ReconcileExternalPageLines.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ReconcileExternalPageLine entity)
        {
            onUpdate();
            context.ReconcileExternalPageLines.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ReconcileExternalPageLine> All()
        {
            return context.ReconcileExternalPageLines.ToList();
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
	 