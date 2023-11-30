 
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
   public partial class ReconcileExternalPageRepository:IRepository<ReconcileExternalPage>
   {
   
        private IAccountingContext currentContext;
        public ReconcileExternalPageRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public ReconcileExternalPageRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  ReconcileExternalPage GetSingle(string id, int tenant)
        {
            return (from a in context.ReconcileExternalPages
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ReconcileExternalPage> GetAll(int tenant)
        {
            return from a in context.ReconcileExternalPages  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ReconcileExternalPage GetSingle(EntityKeyFields entityKeys)
        {
            ReconcileExternalPageKeys keys = entityKeys as ReconcileExternalPageKeys;
            return (from a in context.ReconcileExternalPages
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ReconcileExternalPage entity)
        {
            onAdd();
            context.ReconcileExternalPages.Add(entity);
        }

        public void Remove(ReconcileExternalPage entity)
        {
            context.ReconcileExternalPages.Attach(entity);
            context.ReconcileExternalPages.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ReconcileExternalPage entity)
        {
            onUpdate();
            context.ReconcileExternalPages.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ReconcileExternalPage> All()
        {
            return context.ReconcileExternalPages.ToList();
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
	 