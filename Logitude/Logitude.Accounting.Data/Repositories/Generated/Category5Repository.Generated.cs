 
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
   public partial class Category5Repository:IRepository<Category5>
   {
   
        private IAccountingContext currentContext;
        public Category5Repository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public Category5Repository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  Category5 GetSingle(string id, int tenant)
        {
            return (from a in context.Category5
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Category5> GetAll(int tenant)
        {
            return from a in context.Category5  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Category5 GetSingle(EntityKeyFields entityKeys)
        {
            Category5Keys keys = entityKeys as Category5Keys;
            return (from a in context.Category5
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Category5 entity)
        {
            onAdd();
            context.Category5.Add(entity);
        }

        public void Remove(Category5 entity)
        {
            context.Category5.Attach(entity);
            context.Category5.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Category5 entity)
        {
            onUpdate();
            context.Category5.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Category5> All()
        {
            return context.Category5.ToList();
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
	 