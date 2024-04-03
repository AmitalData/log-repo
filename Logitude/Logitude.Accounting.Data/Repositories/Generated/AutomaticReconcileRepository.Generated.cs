 
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
   public partial class AutomaticReconcileRepository:IRepository<AutomaticReconcile>
   {
   
        private IAccountingContext currentContext;
        public AutomaticReconcileRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public AutomaticReconcileRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  AutomaticReconcile GetSingle(string code)
        {
            return (from a in context.AutomaticReconciles
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<AutomaticReconcile> GetAll()
        {
            return from a in context.AutomaticReconciles  
                   select a;
        }
				 
        public AutomaticReconcile GetSingle(EntityKeyFields entityKeys)
        {
            AutomaticReconcileKeys keys = entityKeys as AutomaticReconcileKeys;
            return (from a in context.AutomaticReconciles
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AutomaticReconcile entity)
        {
            onAdd();
            context.AutomaticReconciles.Add(entity);
        }

        public void Remove(AutomaticReconcile entity)
        {
            context.AutomaticReconciles.Attach(entity);
            context.AutomaticReconciles.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AutomaticReconcile entity)
        {
            onUpdate();
            context.AutomaticReconciles.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AutomaticReconcile> All()
        {
            return context.AutomaticReconciles.ToList();
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
	 