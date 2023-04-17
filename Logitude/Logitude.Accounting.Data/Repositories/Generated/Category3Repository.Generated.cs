 
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
   public partial class Category3Repository:IRepository<Category3>
   {
   
        private IAccountingContext currentContext;
        public Category3Repository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public Category3Repository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  Category3 GetSingle(string id, int tenant)
        {
            return (from a in context.Category3
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Category3> GetAll(int tenant)
        {
            return from a in context.Category3  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Category3 GetSingle(EntityKeyFields entityKeys)
        {
            Category3Keys keys = entityKeys as Category3Keys;
            return (from a in context.Category3
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Category3 entity)
        {
            onAdd();
            context.Category3.Add(entity);
        }

        public void Remove(Category3 entity)
        {
            context.Category3.Attach(entity);
            context.Category3.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Category3 entity)
        {
            onUpdate();
            context.Category3.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Category3> All()
        {
            return context.Category3.ToList();
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
	 