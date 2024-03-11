 
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
   public partial class ConfirmationNumberDefaultRepository:IRepository<ConfirmationNumberDefault>
   {
   
        private IAccountingContext currentContext;
        public ConfirmationNumberDefaultRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public ConfirmationNumberDefaultRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  ConfirmationNumberDefault GetSingle(string id, int tenant)
        {
            return (from a in context.ConfirmationNumberDefaults
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ConfirmationNumberDefault> GetAll(int tenant)
        {
            return from a in context.ConfirmationNumberDefaults  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ConfirmationNumberDefault GetSingle(EntityKeyFields entityKeys)
        {
            ConfirmationNumberDefaultKeys keys = entityKeys as ConfirmationNumberDefaultKeys;
            return (from a in context.ConfirmationNumberDefaults
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ConfirmationNumberDefault entity)
        {
            onAdd();
            context.ConfirmationNumberDefaults.Add(entity);
        }

        public void Remove(ConfirmationNumberDefault entity)
        {
            context.ConfirmationNumberDefaults.Attach(entity);
            context.ConfirmationNumberDefaults.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ConfirmationNumberDefault entity)
        {
            onUpdate();
            context.ConfirmationNumberDefaults.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ConfirmationNumberDefault> All()
        {
            return context.ConfirmationNumberDefaults.ToList();
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
	 