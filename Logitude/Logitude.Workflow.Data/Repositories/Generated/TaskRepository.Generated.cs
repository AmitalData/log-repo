 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Workflow.Data.Repositories
{
   public partial class TaskRepository:IRepository<Task>
   {
   
        private IWorkflowContext currentContext;
        public TaskRepository(int tenant)
        {
            currentContext = WorkflowContext.GetContext(tenant);
        }

        public TaskRepository(IWorkflowContext context)
        {
            currentContext = context;
        }

		 
		
		public  Task GetSingle(string id, int tenant)
        {
            return (from a in context.Tasks
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Task> GetAll(int tenant)
        {
            return from a in context.Tasks  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Task GetSingle(EntityKeyFields entityKeys)
        {
            TaskKeys keys = entityKeys as TaskKeys;
            return (from a in context.Tasks
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Task entity)
        {
            onAdd();
            context.Tasks.Add(entity);
        }

        public void Remove(Task entity)
        {
            context.Tasks.Attach(entity);
            context.Tasks.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Task entity)
        {
            onUpdate();
            context.Tasks.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Task> All()
        {
            return context.Tasks.ToList();
        }

        private IWorkflowContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 