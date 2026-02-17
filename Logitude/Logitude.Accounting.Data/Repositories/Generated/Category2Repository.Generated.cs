 
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
   public partial class Category2Repository:IRepository<Category2>
   {
   
        private IAccountingContext currentContext;
        public Category2Repository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public Category2Repository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  Category2 GetSingle(string id, int tenant)
        {
            return (from a in context.Category2
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Category2> GetAll(int tenant)
        {
            return from a in context.Category2  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Category2 GetSingle(EntityKeyFields entityKeys)
        {
            Category2Keys keys = entityKeys as Category2Keys;
            return (from a in context.Category2
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Category2 entity)
        {
            onAdd();
            context.Category2.Add(entity);
        }

        public void Remove(Category2 entity)
        {
            context.Category2.Attach(entity);
            context.Category2.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Category2 entity)
        {
            onUpdate();
            context.Category2.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Category2> All()
        {
            return context.Category2.ToList();
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
	 