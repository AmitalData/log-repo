 
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
   public partial class AutomaticReconcileMethodRepository:IRepository<AutomaticReconcileMethod>
   {
   
        private IAccountingContext currentContext;
        public AutomaticReconcileMethodRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public AutomaticReconcileMethodRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  AutomaticReconcileMethod GetSingle(string id, int tenant)
        {
            return (from a in context.AutomaticReconcileMethods
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<AutomaticReconcileMethod> GetAll(int tenant)
        {
            return from a in context.AutomaticReconcileMethods  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public AutomaticReconcileMethod GetSingle(EntityKeyFields entityKeys)
        {
            AutomaticReconcileMethodKeys keys = entityKeys as AutomaticReconcileMethodKeys;
            return (from a in context.AutomaticReconcileMethods
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AutomaticReconcileMethod entity)
        {
            onAdd();
            context.AutomaticReconcileMethods.Add(entity);
        }

        public void Remove(AutomaticReconcileMethod entity)
        {
            context.AutomaticReconcileMethods.Attach(entity);
            context.AutomaticReconcileMethods.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AutomaticReconcileMethod entity)
        {
            onUpdate();
            context.AutomaticReconcileMethods.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AutomaticReconcileMethod> All()
        {
            return context.AutomaticReconcileMethods.ToList();
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
	 