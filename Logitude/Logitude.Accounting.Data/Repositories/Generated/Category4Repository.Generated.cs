 
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
   public partial class Category4Repository:IRepository<Category4>
   {
   
        private IAccountingContext currentContext;
        public Category4Repository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public Category4Repository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  Category4 GetSingle(string id, int tenant)
        {
            return (from a in context.Category4
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Category4> GetAll(int tenant)
        {
            return from a in context.Category4  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Category4 GetSingle(EntityKeyFields entityKeys)
        {
            Category4Keys keys = entityKeys as Category4Keys;
            return (from a in context.Category4
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Category4 entity)
        {
            onAdd();
            context.Category4.Add(entity);
        }

        public void Remove(Category4 entity)
        {
            context.Category4.Attach(entity);
            context.Category4.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Category4 entity)
        {
            onUpdate();
            context.Category4.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Category4> All()
        {
            return context.Category4.ToList();
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
	 