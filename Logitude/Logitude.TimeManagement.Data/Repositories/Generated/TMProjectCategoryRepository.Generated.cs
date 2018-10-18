 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.TimeManagement.Data.Repositories
{
   public partial class TMProjectCategoryRepository:IRepository<TMProjectCategory>
   {
   
        private ITimeManagementContext currentContext;
        public TMProjectCategoryRepository(int tenant)
        {
            currentContext = TimeManagementContext.GetContext(tenant);
        }

        public TMProjectCategoryRepository(ITimeManagementContext context)
        {
            currentContext = context;
        }

		 
		
		public  TMProjectCategory GetSingle(string id, int tenant)
        {
            return (from a in context.TMProjectCategories
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TMProjectCategory> GetAll(int tenant)
        {
            return from a in context.TMProjectCategories  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TMProjectCategory GetSingle(EntityKeyFields entityKeys)
        {
            TMProjectCategoryKeys keys = entityKeys as TMProjectCategoryKeys;
            return (from a in context.TMProjectCategories
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TMProjectCategory entity)
        {
            onAdd();
            context.TMProjectCategories.Add(entity);
        }

        public void Remove(TMProjectCategory entity)
        {
            context.TMProjectCategories.Attach(entity);
            context.TMProjectCategories.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TMProjectCategory entity)
        {
            onUpdate();
            context.TMProjectCategories.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TMProjectCategory> All()
        {
            return context.TMProjectCategories.ToList();
        }

        private ITimeManagementContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 