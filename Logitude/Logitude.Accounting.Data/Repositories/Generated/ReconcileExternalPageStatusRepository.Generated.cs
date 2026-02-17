 
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
   public partial class ReconcileExternalPageStatusRepository:IRepository<ReconcileExternalPageStatus>
   {
   
        private IAccountingContext currentContext;
        public ReconcileExternalPageStatusRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public ReconcileExternalPageStatusRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  ReconcileExternalPageStatus GetSingle(string code)
        {
            return (from a in context.ReconcileExternalPageStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ReconcileExternalPageStatus> GetAll()
        {
            return from a in context.ReconcileExternalPageStatuses  
                   select a;
        }
				 
        public ReconcileExternalPageStatus GetSingle(EntityKeyFields entityKeys)
        {
            ReconcileExternalPageStatusKeys keys = entityKeys as ReconcileExternalPageStatusKeys;
            return (from a in context.ReconcileExternalPageStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ReconcileExternalPageStatus entity)
        {
            onAdd();
            context.ReconcileExternalPageStatuses.Add(entity);
        }

        public void Remove(ReconcileExternalPageStatus entity)
        {
            context.ReconcileExternalPageStatuses.Attach(entity);
            context.ReconcileExternalPageStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ReconcileExternalPageStatus entity)
        {
            onUpdate();
            context.ReconcileExternalPageStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ReconcileExternalPageStatus> All()
        {
            return context.ReconcileExternalPageStatuses.ToList();
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
	 