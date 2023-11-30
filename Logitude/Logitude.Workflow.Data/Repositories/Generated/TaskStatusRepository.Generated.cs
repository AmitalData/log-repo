 
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
   public partial class TaskStatusRepository:IRepository<TaskStatus>
   {
   
        private IWorkflowContext currentContext;
        public TaskStatusRepository(int tenant)
        {
            currentContext = WorkflowContext.GetContext(tenant);
        }

        public TaskStatusRepository(IWorkflowContext context)
        {
            currentContext = context;
        }

		 
		
		public  TaskStatus GetSingle(string id, int tenant)
        {
            return (from a in context.TaskStatuses
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TaskStatus> GetAll(int tenant)
        {
            return from a in context.TaskStatuses  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TaskStatus GetSingle(EntityKeyFields entityKeys)
        {
            TaskStatusKeys keys = entityKeys as TaskStatusKeys;
            return (from a in context.TaskStatuses
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TaskStatus entity)
        {
            onAdd();
            context.TaskStatuses.Add(entity);
        }

        public void Remove(TaskStatus entity)
        {
            context.TaskStatuses.Attach(entity);
            context.TaskStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TaskStatus entity)
        {
            onUpdate();
            context.TaskStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TaskStatus> All()
        {
            return context.TaskStatuses.ToList();
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
	 