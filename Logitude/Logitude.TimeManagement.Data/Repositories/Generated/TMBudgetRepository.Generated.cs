 
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
   public partial class TMBudgetRepository:IRepository<TMBudget>
   {
   
        private ITimeManagementContext currentContext;
        public TMBudgetRepository(int tenant)
        {
            currentContext = TimeManagementContext.GetContext(tenant);
        }

        public TMBudgetRepository(ITimeManagementContext context)
        {
            currentContext = context;
        }

		 
		
		public  TMBudget GetSingle(string id, int tenant)
        {
            return (from a in context.TMBudgets
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TMBudget> GetAll(int tenant)
        {
            return from a in context.TMBudgets  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TMBudget GetSingle(EntityKeyFields entityKeys)
        {
            TMBudgetKeys keys = entityKeys as TMBudgetKeys;
            return (from a in context.TMBudgets
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TMBudget entity)
        {
            onAdd();
            context.TMBudgets.Add(entity);
        }

        public void Remove(TMBudget entity)
        {
            context.TMBudgets.Attach(entity);
            context.TMBudgets.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TMBudget entity)
        {
            onUpdate();
            context.TMBudgets.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TMBudget> All()
        {
            return context.TMBudgets.ToList();
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
	 