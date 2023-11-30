 
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
   public partial class AutomaticExternalRconcilMthodRepository:IRepository<AutomaticExternalRconcilMthod>
   {
   
        private IAccountingContext currentContext;
        public AutomaticExternalRconcilMthodRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public AutomaticExternalRconcilMthodRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  AutomaticExternalRconcilMthod GetSingle(string code)
        {
            return (from a in context.AutomaticExternalRconcilMthods
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<AutomaticExternalRconcilMthod> GetAll()
        {
            return from a in context.AutomaticExternalRconcilMthods  
                   select a;
        }
				 
        public AutomaticExternalRconcilMthod GetSingle(EntityKeyFields entityKeys)
        {
            AutomaticExternalRconcilMthodKeys keys = entityKeys as AutomaticExternalRconcilMthodKeys;
            return (from a in context.AutomaticExternalRconcilMthods
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AutomaticExternalRconcilMthod entity)
        {
            onAdd();
            context.AutomaticExternalRconcilMthods.Add(entity);
        }

        public void Remove(AutomaticExternalRconcilMthod entity)
        {
            context.AutomaticExternalRconcilMthods.Attach(entity);
            context.AutomaticExternalRconcilMthods.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AutomaticExternalRconcilMthod entity)
        {
            onUpdate();
            context.AutomaticExternalRconcilMthods.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AutomaticExternalRconcilMthod> All()
        {
            return context.AutomaticExternalRconcilMthods.ToList();
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
	 