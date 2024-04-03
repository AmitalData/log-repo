 
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
   public partial class TaskExtendedRepository:IRepository<TaskExtended>
   {
   
        private IWorkflowContext currentContext;
        public TaskExtendedRepository(int tenant)
        {
            currentContext = WorkflowContext.GetContext(tenant);
        }

        public TaskExtendedRepository(IWorkflowContext context)
        {
            currentContext = context;
        }

		 
		
		public  TaskExtended GetSingle(string id, int tenant)
        {
            return (from a in context.TasksExtended
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TaskExtended> GetAll(int tenant)
        {
            return from a in context.TasksExtended  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TaskExtended GetSingle(EntityKeyFields entityKeys)
        {
            TaskExtendedKeys keys = entityKeys as TaskExtendedKeys;
            return (from a in context.TasksExtended
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TaskExtended entity)
        {
            onAdd();
            context.TasksExtended.Add(entity);
        }

        public void Remove(TaskExtended entity)
        {
            context.TasksExtended.Attach(entity);
            context.TasksExtended.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TaskExtended entity)
        {
            onUpdate();
            context.TasksExtended.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TaskExtended> All()
        {
            return context.TasksExtended.ToList();
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
	 