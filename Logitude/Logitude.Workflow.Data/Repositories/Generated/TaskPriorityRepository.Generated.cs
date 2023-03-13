 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Workflow.Data.Repositories
{
   public partial class TaskPriorityRepository:IRepository<TaskPriority>
   {
   
        private IWorkflowContext currentContext;
        public TaskPriorityRepository(int tenant)
        {
            currentContext = WorkflowContext.GetContext(tenant);
        }

        public TaskPriorityRepository(IWorkflowContext context)
        {
            currentContext = context;
        }

		 
		
		public  TaskPriority GetSingle(string id, int tenant)
        {
            return (from a in context.TaskPriorities
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TaskPriority> GetAll(int tenant)
        {
            return from a in context.TaskPriorities  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TaskPriority GetSingle(EntityKeyFields entityKeys)
        {
            TaskPriorityKeys keys = entityKeys as TaskPriorityKeys;
            return (from a in context.TaskPriorities
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TaskPriority entity)
        {
            onAdd();
            context.TaskPriorities.Add(entity);
        }

        public void Remove(TaskPriority entity)
        {
            context.TaskPriorities.Attach(entity);
            context.TaskPriorities.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TaskPriority entity)
        {
            onUpdate();
            context.TaskPriorities.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TaskPriority> All()
        {
            return context.TaskPriorities.ToList();
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
	 