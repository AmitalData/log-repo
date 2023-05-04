 
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
   public partial class ReconcileMethodRepository:IRepository<ReconcileMethod>
   {
   
        private IAccountingContext currentContext;
        public ReconcileMethodRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public ReconcileMethodRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  ReconcileMethod GetSingle(string code)
        {
            return (from a in context.ReconcileMethods
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ReconcileMethod> GetAll()
        {
            return from a in context.ReconcileMethods  
                   select a;
        }
				 
        public ReconcileMethod GetSingle(EntityKeyFields entityKeys)
        {
            ReconcileMethodKeys keys = entityKeys as ReconcileMethodKeys;
            return (from a in context.ReconcileMethods
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ReconcileMethod entity)
        {
            onAdd();
            context.ReconcileMethods.Add(entity);
        }

        public void Remove(ReconcileMethod entity)
        {
            context.ReconcileMethods.Attach(entity);
            context.ReconcileMethods.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ReconcileMethod entity)
        {
            onUpdate();
            context.ReconcileMethods.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ReconcileMethod> All()
        {
            return context.ReconcileMethods.ToList();
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
	 